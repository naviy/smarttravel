using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Managers;


namespace Luxena.Travel.Domain
{

	public interface IDomainContainer
	{
		Domain Domain { get; set; }
	}


	public class EntityManager<TEntity, TEntityService> : GenericManager
		where TEntity : class, IEntity
		where TEntityService : EntityService<TEntity>
	{

		public Domain db { get; set; }

		protected TEntityService Service { [DebuggerStepThrough] get { return _service ?? (_service = db.Service<TEntityService>()); } }
		private TEntityService _service;


		public EntityManager()
		{
			Class = Base.Metamodel.Class.Of(typeof(TEntity));
		}


		#region Permission

		//public override Permissions GetCustomPermissions()
		//{
		//	return Service.GetCustomPermissions();
		//}


		public override OperationStatus CanCopy()
		{
			return Service.CanCopy();
		}

		public override OperationStatus CanCreate()
		{
			return Service.CanCreate();
		}

		public override OperationStatus CanCreate(object obj)
		{
			return Service.CanCreate(obj as TEntity);
		}

		public override OperationStatus CanDelete()
		{
			return Service.CanDelete();
		}

		public override OperationStatus CanDelete(object obj)
		{
			return Service.CanDelete(obj as TEntity);
		}

		public override OperationStatus CanList()
		{
			return Service.CanList();
		}

		public override OperationStatus CanReplace(object obj)
		{
			return Service.CanReplace(obj as TEntity);
		}

		public override OperationStatus CanUpdate()
		{
			return Service.CanUpdate();
		}

		public override OperationStatus CanUpdate(object obj)
		{
			var service = Service;
			var entity = obj as TEntity; 
			if (entity != null)
				return service.CanUpdate(entity);

			var list = obj as IEnumerable<TEntity>;
			if (list != null)
				return db.Granted(list.All(a => service.CanUpdate(a)));

			throw new NotImplementedException();
		}

		public override OperationStatus CanView(object obj)
		{
			return Service.CanView((TEntity)obj);
		}

		#endregion


		#region Modify

		public override object Create()
		{
			var r = base.Create();
			r.As<IDomainContainer>().Do(a => a.Domain = db);
			return r;
		}

		public override void Update(object r, object newValue)
		{
			r.As<IDomainContainer>().Do(a => a.Domain = db);
			base.Update(r, newValue);
		}

		public override void Delete(object obj)
		{
			var r = (TEntity)obj;
			Service.Delete(r);
		}

		public override void Save(object obj)
		{
			Service.Save((TEntity)obj);
		}

		public override RangeResponse Suggest(RangeRequest request, RecordConfig config)
		{
			return Service.Suggest(request);
		}

		#endregion

	}

}
