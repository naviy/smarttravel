using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Base.Serialization;
using Luxena.Domain.Entities;


namespace Luxena.Domain.Contracts
{

	public class EntityContractService<TDomain, TContracts>
		where TDomain : Domain<TDomain>
		where TContracts : Contracts<TDomain, TContracts>
	{

		public TDomain Domain
		{
			[DebuggerStepThrough]
			get { return db; }
			[DebuggerStepThrough]
			set { db = value; }
		}
		protected internal TDomain db;

		public TContracts Contracts
		{
			[DebuggerStepThrough]
			get { return dc; }
			[DebuggerStepThrough]
			set { dc = value; }
		}
		protected internal TContracts dc;


		protected ItemListResponse NewItemListResponse<TEntity>(IList<TEntity> entities, RangeRequest prms, Func<TEntity, object> @new)
			where TEntity : class, IEntity
		{
			var response = new ItemListResponse
			{
				Items = entities.Select(@new).ToArray()
			};

			if (prms != null)
			{
				prms.PositionableObjectId = entities.One(a => a.Id);

				response.RangeResponse = db.GetRange(prms);
			}

			return response;
		}
	}

	
	public abstract class EntityContractService<TDomain, TContracts, TEntity, TEntityService, TContract>
		: EntityContractService<TDomain, TContracts>
		where TDomain : Domain<TDomain>
		where TContracts : Contracts<TDomain, TContracts>
		where TEntity : class, IEntity
		where TEntityService : EntityService<TDomain, TEntity>
		where TContract : class, IEntity, new()
	{

		public TEntityService Service { [DebuggerStepThrough] get { return _service ?? (_service = db.Service<TEntityService>()); } }
		private TEntityService _service;

		protected EntityContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Id = r.Id;
				c.Version = r.Version;
			};

			EntityFromContract += (r, c) =>
			{
				r.Version = c.Version;
			};
		}

		public delegate void AssignAction(TEntity r, TContract c);

		protected event AssignAction ContractFromEntity;
		protected event AssignAction EntityFromContract;


		#region New

		public virtual TContract New(TEntity r)
		{
			if (r == null) return null;

			var c = new TContract();

			ContractFromEntity?.Invoke(r, c);

			return c;
		}

		public TContract[] New<TEntity2>(IList<TEntity2> entities)
			where TEntity2 : TEntity
		{
			return entities.No() ? new TContract[0] : entities.Select(a => New(a)).ToArray();
		}

		public TContract[] New<TEntity2>(IList<TEntity2> entities,
			Func<IEnumerable<TContract>, IEnumerable<TContract>> query)
			where TEntity2 : TEntity
		{
			if (entities.No())
				return new TContract[0];

			var list = entities.Select(a => New(a));

			if (query != null)
				list = query(list);

			return list.ToArray();
		}


		public TContract By(object id)
		{
			return id == null ? null : New(Service.By(id));
		}

		public TContract[] ListByIds(object[] ids)
		{
			return New(Service.ListByIds(ids));
		}

		#endregion


		#region Modify

		public TEntity Update(TEntity r, TContract c)
		{
			db.UserModifingEntity = r;

			EntityFromContract?.Invoke(r, c);

			Service.Save(r);

			return r;
		}


		protected virtual TEntity NewEntity(TContract c)
		{
			return Service.New();
		}

		protected virtual TEntity NewEntity(TContract c, string typeName)
		{
			var cls = Class.Of(typeName);

			var service = (IEntityCreator<TEntity>)db.ServiceByEntity(cls.Type);

			return service.New();
		}

		protected virtual object NewResponseContract(TEntity r, TContract c)
		{
			return New(r);
		}


		//[DebuggerStepThrough]
		public virtual ItemResponse ItemUpdate(
			TContract c,
			RangeRequest prms,
			Func<TEntity> newEntity,
			Func<TEntity, object> newResponseContract
		)
		{
			TEntity r;
			if (c.Id == null)
			{
				AssertCreate(c);
				r = newEntity();
			}
			else
			{
				r = Service.ByVersion(c.Id, c.Version);
				AssertUpdate(r, c);
			}

			db.UserModifingEntity = r;

			EntityFromContract?.Invoke(r, c);
			Service.Save(r);

			db.Committing();

			var response = new ItemResponse();

			if (prms != null)
			{
				prms.PositionableObjectId = r.Id;

				response.Item = new ObjectSerializer().Serialize(r);
				response.RangeResponse = db.GetRange(prms);
			}
			else
			{
				response.Item = newResponseContract(r);
			}

			return response;
		}

		protected virtual void AssertCreate(TContract c)
		{
			Service.AssertCreate();
		}

		protected virtual void AssertUpdate(TEntity r, TContract c)
		{
			Service.AssertUpdate(r);
		}

		public virtual ItemResponse Update(TContract c, RangeRequest prms)
		{
			return ItemUpdate(
				c, prms,
				() => NewEntity(c),
				r => NewResponseContract(r, c)
			);
		}

		public virtual ItemResponse Update(TContract c, string typeName, RangeRequest prms)
		{
			return ItemUpdate(
				c, prms,
				() => NewEntity(c, typeName),
				r => NewResponseContract(r, c)
			);
		}

		#endregion


		#region Modify for Details Properties

		public void UpdateKey<TMasterEntity>(TMasterEntity mr) { }

		public void Update<TMasterEntity>(
			TMasterEntity mr,
			IEnumerable<TEntity> entities,
			IEnumerable<TContract> contracts,
			Func<TContract, TEntity> newEntity,
			Action<TContract, TEntity> assignTo,
			Action<TDomain, TContract, TEntity> addEntity,
			Action<TDomain, TEntity> removeEntity
		)
			where TMasterEntity : class, IEntity
		{
			if (entities == null) throw new ArgumentNullException(nameof(entities));
			if (addEntity == null) throw new ArgumentNullException(nameof(addEntity));
			if (removeEntity == null) throw new ArgumentNullException(nameof(removeEntity));

			// ReSharper disable once LocalVariableHidesMember
			var db = this.db;

			db.OnCommit(mr, UpdateKey, mr0 =>
			{
				var oldEntities = entities.ToArray();

				if (contracts == null)
				{
					foreach (var oldEntity in oldEntities)
					{
						removeEntity(db, oldEntity);
					}

					db.Save(mr);
					return;
				}

				foreach (var entity in oldEntities.Where(e => contracts.All(c => !Equals(c.Id, e.Id))))
				{
					removeEntity(db, entity);
				}

				newEntity = newEntity ?? NewEntity;
				assignTo = assignTo ?? ((c0, r0) => EntityFromContract?.Invoke(r0, c0));

				foreach (var c in contracts)
				{
					if (c.Id == null)
					{
						var r = newEntity(c);
						assignTo(c, r);
						addEntity(db, c, r);
						Service.Save(mr, r);
					}
					else
					{
						var c1 = c;
						var r = oldEntities.By(a => Equals(a.Id, c1.Id));

						if (r == null)
						{
							r = Service.By(c.Id);
							assignTo(c, r);
							addEntity(db, c, r);
							Service.Save(mr, r);
						}
						else
						{
							assignTo(c, r);
							Service.Save(mr, r);
						}

					}
				}

				db.Save(mr);
			});

		}

		[DebuggerStepThrough]
		public void Update<TMasterEntity>(
			TMasterEntity mr,
			IList<TEntity> entities,
			IEnumerable<TContract> contracts,
			Func<TContract, TEntity> newEntity,
			Action<TContract, TEntity> assignTo,
			Action<TDomain, TEntity> addEntity,
			Action<TDomain, TEntity> removeEntity
		)
			where TMasterEntity : class, IEntity
		{
			Update(
				mr, entities, contracts,
				newEntity ?? NewEntity,
				assignTo,
				(db2, contract, entity) => addEntity(db2, entity),
				removeEntity
			);
		}

		[DebuggerStepThrough]
		public void Update<TMasterEntity>(
			TMasterEntity mr,
			IList<TEntity> entities,
			IEnumerable<TContract> contracts,
			Action<TDomain, TEntity> addEntity,
			Action<TDomain, TEntity> removeEntity
		)
			where TMasterEntity : class, IEntity
		{
			Update(
				mr, entities, contracts,
				null,
				null,
				(domain, contract, entity) => addEntity(domain, entity),
				removeEntity
			);
		}

		[DebuggerStepThrough]
		public void Update<TMasterEntity>(
			TMasterEntity mr,
			IList<TEntity> entities,
			IEnumerable<TContract> contracts,
			Func<TContract, TEntity> newEntity,
			Action<TContract, TEntity> assignTo,
			Action<TEntity> addEntity,
			Action<TEntity> removeEntity
		)
			where TMasterEntity : class, IEntity
		{
			Update(
				mr, entities, contracts,
				newEntity,
				assignTo,
				(domain, contract, entity) => addEntity(entity),
				(domain, entity) => removeEntity(entity)
			);
		}


		[DebuggerStepThrough]
		public void Update<TMasterEntity>(
			TMasterEntity mr,
			IList<TEntity> entities,
			IEnumerable<TContract> contracts,
			Action<TEntity> addEntity,
			Action<TEntity> removeEntity
		)
			where TMasterEntity : class, IEntity
		{
			Update(
				mr, entities, contracts,
				null,
				null,
				(domain, contract, entity) => addEntity(entity),
				(domain, entity) => removeEntity(entity)
			);
		}

		protected ItemListResponse NewItemListResponse<TEntity2>(IList<TEntity2> entities, RangeRequest prms)
			where TEntity2 : class, TEntity
		{
			var response = new ItemListResponse
			{
				Items = New(entities)
			};

			if (prms != null)
			{
				prms.PositionableObjectId = entities.One(a => a.Id);

				response.RangeResponse = db.GetRange(prms);
			}

			return response;
		}


		#endregion

	}

}