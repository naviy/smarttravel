using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Common.Logging;

using DelegateDecompiler;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Base.Text;
using Luxena.Domain.Entities;


namespace Luxena.Travel.Domain
{

	public partial class Domain : Domain<Domain>, IDefaultLocalizationTypesSource
	{

		public Type[] DefaultLocalizationTypes => _defaultLocalizationTypes;

		static readonly Type[] _defaultLocalizationTypes =
		{
			typeof(RUAttribute), 
			typeof(UAAttribute), 
			typeof(ENAttribute)
		};



		public Domain(Func<Type, object> _resolveAction)
		{
			ResolveAction = _resolveAction;
			Init();
		}

		public Func<string> GetIdentityName;

		public Func<Type, object> ResolveAction;

		public override T Resolve<T>()
		{
			return (T)ResolveAction(typeof(T));
		}

		//protected override IEnumerable<T> GetAllInstances<T>()
		//{
		//	return _container.GetAllInstances<T>();
		//}

		public override object GetId(object entity)
		{
			if (entity == null) return null;

			var entityBase = entity as IEntity;
			if (entityBase == null)
				throw new NotImplementedException();

			return entityBase.Id;
		}


		public IErrorTranslator ErrorTranslator => Resolve(ref _errorTranslator);
		private IErrorTranslator _errorTranslator;

		public Exception Translate(Exception ex)
		{
			return ErrorTranslator.As(a => a.Translate(ex)) ?? ex;
		}


		#region Log

		[DebuggerStepThrough]
		public void Error(string message, Exception ex)
		{
			_log.Error(message, ex);
		}

		[DebuggerStepThrough]
		public override void Error(Exception ex)
		{
			_log.Error(ex);
		}

		[DebuggerStepThrough]
		public void Warn(Exception ex)
		{
			_log.Warn(ex.Message, ex);
			_log.Warn(ex.Source);
			_log.Warn(ex.StackTrace);
		}

		private static readonly ILog _log = LogManager.GetLogger(typeof(Domain));

		#endregion

	}


	public class DomainService : DomainService<Domain>
	{
	}


	public abstract class EntityService<TEntity> : EntityService<Domain, TEntity>
		where TEntity : class, IEntity
	{

		#region Permissions

		protected override OperationStatus CanDoOperation(Func<GenericPrivilegesAttribute, object[]> privileges)
		{
			var attribute = typeof(TEntity).GetAttribute<GenericPrivilegesAttribute>();

			if (attribute == null)
				return false;

			var privileges1 = privileges(attribute);
			return privileges1 == null || db.IsGranted(privileges1.Cast<UserRole>());
		}

		#endregion


		public string NewSequence()
		{
			return db.Sequence.Next<TEntity>();
		}

		[DebuggerStepThrough]
		public IList<TEntity> ListByIds(IEnumerable<IEntity> contracts)
		{
			return contracts == null ? EmptyList : ListByIds(contracts.Select(a => a.Id).ToArray());
		}

		public ItemListResponse GetItemListResponse(IList<TEntity> items, RangeRequest prms, Func<TEntity, object> convertToDto)
		{
			var list = items.Select(convertToDto).ToArray();

			var response = new ItemListResponse { Items = list };

			if (prms == null) return response;

			prms.PositionableObjectId = items[0].Id;

			response.RangeResponse = db.GetRange<TEntity>(prms);

			return response;
		}


		public RangeResponse Suggest3<TEntity3>(RangeRequest prms, IQueryable<TEntity3> query)
			where TEntity3 : class, IEntity3
		{
			var limit = prms.Limit;
			if (limit == 0) limit = 50;

			if (prms.Query == "*")
			{
				return new RangeResponse(query
					.OrderBy(a => a.Name)
					.Select(a => EntityReference.FromArray(a.Id, a.Name, a.GetType().Name))
					.Take(limit)
					.ToArray()
				);
			}

			var d1 = query
				.Where(a => a.Name.StartsWith(prms.Query))
				.OrderBy(a => a.Name)
				.Select(a => EntityReference.FromArray(a.Id, a.Name, a.GetType().Name))
				.Take(limit)
				.ToArray();

			if (d1.Length >= limit)
				return new RangeResponse(d1);

			var d2 = query
				.Where(a => !a.Name.StartsWith(prms.Query) && a.Name.Contains(" " + prms.Query))
				.OrderBy(a => a.Name)
				.Select(a => EntityReference.FromArray(a.Id, a.Name, a.GetType().Name))
				.Take(limit - d1.Length)
				.ToArray();

			if (d1.Length + d2.Length >= limit)
				return new RangeResponse(d1.Union(d2).ToArray());

			var d3 = query
				.Where(a => !a.Name.StartsWith(prms.Query) && a.Name.Contains(prms.Query) && !a.Name.Contains(" " + prms.Query))
				.OrderBy(a => a.Name)
				.Select(a => EntityReference.FromArray(a.Id, a.Name, a.GetType().Name))
				.Take(limit - d1.Length - d2.Length)
				.ToArray();

			return new RangeResponse(d1.Union(d2).Union(d3).ToArray());
		}

	}


	public abstract class Entity2Service<TEntity> : EntityService<TEntity>
		where TEntity : class, IEntity2
	{

		protected Entity2Service()
		{
			Inserting += r =>
			{
				r.CreatedOn = DateTime.Now;

				if (r.CreatedBy.No())
					r.CreatedBy = db.Security.UserName ?? "SYSTEM";
			};

			Modifing += r =>
			{
				if (db.UserModifingEntity == r)
				{
					r.ModifiedOn = DateTime.Now;
					r.ModifiedBy = db.Security.UserName ?? "SYSTEM";
				}
			};
		}
	}


	public abstract class Entity3Service<TEntity> : Entity2Service<TEntity>
		where TEntity : class, IEntity3
	{

		public TEntity ByName(string name, bool useLike = false)
		{
			return name.No()
				? null
				: useLike
					? By(a => a.Name.Contains(name))
					: By(a => a.Name == name);
		}

		public EntityReference[] ReferenceList()
		{
			var typeName = typeof(TEntity).Name;

			var referenceList = Query
				.OrderBy(a => a.Name)
				.Select(a => new EntityReference(typeName, a.Id, a.Name))
				.ToArray();

			return referenceList;
		}

		public override RangeResponse Suggest(RangeRequest prms)
		{
			return Suggest3(prms);
		}

		public RangeResponse Suggest3(RangeRequest prms, IQueryable<TEntity> query = null)
		{
			return base.Suggest3(prms, query ?? Query);
		}

	}

	public static partial class EntityExtentions
	{

		[Computed]
		public static EntityReference ToReference(this IEntity r)
		{
			return r == null ? null : new EntityReference(r);
		}

		public static EntityReference[] ToReferences<TEntity>(this IQueryable<TEntity> me)
			where TEntity : class, IEntity3
		{
			if (me == null) return new EntityReference[0];

			var cls = Class.Of<TEntity>();

			return me.Select(a => new EntityReference(cls.Id, a.Id, a.Name)).ToArray();
		}

		public static EntityReference[] ToReferences<TEntity>(this IEnumerable<TEntity> me)
			where TEntity : class, IEntity3
		{
			if (me == null) return new EntityReference[0];

			var cls = Class.Of<TEntity>();

			return me.Select(a => new EntityReference(cls.Id, a.Id, a.Name)).ToArray();
		}
	
	}


}