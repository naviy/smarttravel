using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using Luxena.Base;
using Luxena.Base.Data;
using Luxena.Base.Domain;

using NHibernate;
using NHibernate.Linq;




namespace Luxena.Domain.Entities
{



	public interface IEntityCreator<out TEntity>
		where TEntity : class, IEntity
	{
		TEntity New();
	}




	public abstract class EntityService<TDomain, TEntity>
		: DomainService<TDomain>, IEntityPermissions, IEntityCreator<TEntity>//, IQueryable<TEntity>
		where TDomain : Domain<TDomain>
		where TEntity : class, IEntity
	{


		protected EntityService()
		{
			var entityType = GetDeclaringEntityType();
			if (entityType != null)
				FetchClass(entityType);
		}


		public override Type GetEntityType()
		{
			return typeof(TEntity);
		}


		#region Read

		[DebuggerStepThrough]
		public TEntity By(object id)
		{
			return id == null ? null : Session.Get<TEntity>(id);
		}

		public TEntity this[object id] => By(id);


		[DebuggerStepThrough]
		public TEntity ByVersion(object id, int version)
		{
			var obj = By(id);

			if (obj != null && !Equals(version, Session.SessionFactory.GetClassMetadata(typeof(TEntity)).GetVersion(obj, EntityMode.Poco)))
				throw new StaleStateException("Object has been modified by another user");

			return obj;
		}

		[DebuggerStepThrough]
		public TEntity By(EntityReference reference)
		{
			return reference == null ? null : By(reference.Id);
		}

		[DebuggerStepThrough]
		public TEntity By(Expression<Func<TEntity, bool>> predicate)
		{
			return Query.FirstOrDefault(predicate);
		}

		[DebuggerStepThrough]
		public TEntity By(Expression<Func<TEntity, bool>> predicate, bool cacheable)
		{
			if (predicate == null)
				return null;
			IQueryOver<TEntity> query = QueryOver.Where(predicate);
			if (cacheable)
				query = query.Cacheable();
			return query.SingleOrDefault();
		}

		[DebuggerStepThrough]
		public bool Exists(Expression<Func<TEntity, bool>> predicate)
		{
			return Query.Where(predicate).Select(a => a.Id).FirstOrDefault() != null;
		}


		[DebuggerStepThrough]
		public IList<TEntity> ListBy(Expression<Func<TEntity, bool>> predicate)
		{
			return predicate == null ? EmptyList : Query.Where(predicate).ToList();
		}

		[DebuggerStepThrough]
		public IList<TEntity> ListBy(Expression<Func<TEntity, bool>> predicate, bool cacheable)
		{

			if (predicate == null)
				return Array.Empty<TEntity>();

			
			IQueryOver<TEntity> query = QueryOver.Where(predicate);

			if (cacheable)
				query = query.Cacheable();

			return query.List();

		}


		[DebuggerStepThrough]
		public IList<TEntity> ListByIds(params object[] ids)
		{
			return Query.Where(a => ids.Contains(a.Id)).ToList();
		}

		[DebuggerStepThrough]
		public IList<TEntity> ListByIds(IEnumerable<object> ids)
		{
			return ListByIds(ids.ToArray());
		}

		[DebuggerStepThrough]
		public TEntity Read(object id)
		{
			var entity = Session.Get<TEntity>(id);
			if (entity == null)
				throw new Exception($"Object {typeof(TEntity)} {id} not found");
			return entity;
		}

		//[DebuggerStepThrough]
		//public TEntity Read(Reference info)
		//{
		//	return info == null ? null : Read(info.Id);
		//}

		[DebuggerStepThrough]
		public TEntity Load(object id)
		{
			return id == null ? null : Session.Load<TEntity>(id);
		}

		[DebuggerStepThrough]
		public TEntity Load(EntityReference reference)
		{
			return reference?.Id == null || reference.Id as string == "" ? null : Load(reference.Id);
		}


		//public List<TEntity> List()
		//{
		//	return db.Commit(() => Query.ToList());
		//}


		//public virtual RangeResponse GetRange(RangeRequest prms)
		//{
		//	return db.GetRange<TEntity>(prms);
		//}

		public virtual RangeResponse Suggest(RangeRequest request)
		{
			throw new NotImplementedException();
		}

		public IQueryOver<TEntity, TEntity> QueryOver => Session.QueryOver<TEntity>();

		public static IList<TEntity> EmptyList = Array.Empty<TEntity>();

		#endregion


		#region Permissions

		public void AssertCreate()
		{
			CanCreate().Assert(() => Resources.CreateOperationDenied_Msg);
		}

		public void AssertCreate(TEntity obj)
		{
			CanCreate(obj).Assert(() => Resources.CreateOperationDenied_Msg);
		}

		public void AssertCreate(IEnumerable<TEntity> list)
		{
			CanCreate(list).Assert(() => Resources.CreateOperationDenied_Msg);
		}

		public void AssertCreate(object[] ids)
		{
			CanCreate(ids).Assert(() => Resources.CreateOperationDenied_Msg);
		}


		public void AssertDelete()
		{
			CanDelete().Assert(() => Resources.DeleteOperationDenied_Msg);
		}

		public void AssertDelete(TEntity obj)
		{
			CanDelete(obj).Assert(() => Resources.DeleteOperationDenied_Msg);
		}

		public void AssertDelete(IEnumerable<TEntity> list)
		{
			CanDelete(list).Assert(() => Resources.DeleteOperationDenied_Msg);
		}

		public void AssertDelete(object[] ids)
		{
			CanDelete(ids).Assert(() => Resources.DeleteOperationDenied_Msg);
		}

		public void AssertModify(TEntity r)
		{
			(db.IsNew(r) ? CanCreate(r) : CanUpdate(r)).Assert(() => Resources.UpdateOperationDenied_Msg);
		}


		public void AssertUpdate()
		{
			CanUpdate().Assert(() => Resources.UpdateOperationDenied_Msg);
		}

		public void AssertUpdate(TEntity obj)
		{
			CanUpdate(obj).Assert(() => Resources.UpdateOperationDenied_Msg);
		}

		public void AssertUpdate(IEnumerable<TEntity> list)
		{
			CanUpdate(list).Assert(() => Resources.UpdateOperationDenied_Msg);
		}

		public void AssertUpdate(object[] ids)
		{
			CanUpdate(ids).Assert(() => Resources.UpdateOperationDenied_Msg);
		}


		public virtual OperationStatus CanCreate()
		{
			return CanDoOperation(attribute => attribute.Create);
		}

		public virtual OperationStatus CanCreate(TEntity obj)
		{
			return CanCreate();
		}

		public OperationStatus CanCreate(IEnumerable<TEntity> list)
		{
			return list.Select(CanCreate).By(a => !a) ?? OperationStatus.Enabled();
		}

		public virtual OperationStatus CanCreate(object[] ids)
		{
			return CanCreate(ListByIds(ids));
		}


		public virtual OperationStatus CanCopy()
		{
			return CanDoOperation(attribute => attribute.Copy);
		}

		public virtual OperationStatus CanCopy(TEntity obj)
		{
			return CanCopy();
		}

		public OperationStatus CanCopy(IEnumerable<TEntity> list)
		{
			return list.Select(CanCopy).By(a => !a) ?? OperationStatus.Enabled();
		}

		public virtual OperationStatus CanCopy(object[] ids)
		{
			return CanCopy(ListByIds(ids));
		}


		public virtual OperationStatus CanDelete()
		{
			return CanDoOperation(attribute => attribute.Delete);
		}

		public virtual OperationStatus CanDelete(TEntity obj)
		{
			return CanDelete();
		}

		public OperationStatus CanDelete(IEnumerable<TEntity> list)
		{
			return list.Select(CanDelete).By(a => !a) ?? OperationStatus.Enabled();
		}

		public virtual OperationStatus CanDelete(object[] ids)
		{
			return CanDelete(ListByIds(ids));
		}


		public virtual OperationStatus CanList()
		{
			return CanDoOperation(attribute => attribute.List);
		}

		public virtual OperationStatus CanList(TEntity obj)
		{
			return CanList();
		}

		public OperationStatus CanList(IEnumerable<TEntity> list)
		{
			return list.Select(CanList).By(a => !a) ?? OperationStatus.Enabled();
		}


		public virtual OperationStatus CanReplace()
		{
			return CanDoOperation(attribute => attribute.Replace);
		}

		public virtual OperationStatus CanReplace(TEntity obj)
		{
			return CanReplace();
		}

		public OperationStatus CanReplace(IEnumerable<TEntity> list)
		{
			return list.Select(CanReplace).By(a => !a) ?? OperationStatus.Enabled();
		}

		public virtual OperationStatus CanReplace(object[] ids)
		{
			return CanReplace(ListByIds(ids));
		}


		public virtual OperationStatus CanUpdate()
		{
			return CanDoOperation(attribute => attribute.Update);
		}

		public virtual OperationStatus CanUpdate(TEntity obj)
		{
			return CanUpdate();
		}

		public OperationStatus CanUpdate(IEnumerable<TEntity> list)
		{
			return list.All(a => CanUpdate(a));
		}

		public virtual OperationStatus CanUpdate(object[] ids)
		{
			return CanUpdate(ListByIds(ids));
		}


		public virtual OperationStatus CanView()
		{
			return CanDoOperation(attribute => attribute.View);
		}

		public virtual OperationStatus CanView(TEntity obj)
		{
			return CanView();
		}

		public OperationStatus CanView(IEnumerable<TEntity> list)
		{
			return list.Select(CanView).By(a => !a) ?? OperationStatus.Enabled();
		}

		public virtual OperationStatus CanView(object[] ids)
		{
			return CanView(ListByIds(ids));
		}


		protected abstract OperationStatus CanDoOperation(Func<GenericPrivilegesAttribute, object[]> privileges);

		#endregion


		#region State

		[DebuggerStepThrough]
		public TValue OldValue<TValue>(TEntity r, Expression<Func<TEntity, TValue>> getProperty)
		{
			return db.OldValue(r, getProperty);
		}

		//[DebuggerStepThrough]
		public bool IsDirty(TEntity r, Expression<Func<TEntity, object>> getProperty)
		{
			return db.IsDirty(r, getProperty);
		}

		[DebuggerStepThrough]
		public bool IsNew(TEntity r)
		{
			return db.IsNew(r);
		}

		#endregion


		#region Modify

		public delegate void EntityModifyEvent(TEntity r);


		#region Calculation

		public event EntityModifyEvent Calculating;

		public void Calculate(TEntity r)
		{
			Calculating?.Invoke(r);
		}

		public void Calculate(IEnumerable<TEntity> entities)
		{
			if (Calculating != null)
				entities.ForEach(r => Calculating(r));
		}

		public void Calculate(IList<TEntity> entities)
		{
			if (Calculating != null)
				entities.ForEach(r => Calculating(r));
		}

		#endregion


		#region Validation

		public event EntityModifyEvent Validating;

		public void Validate(TEntity r)
		{
			Validating?.Invoke(r);
		}

		public void Validate(IEnumerable<TEntity> entities)
		{
			if (Validating != null)
				entities.ForEach(r => Validating(r));
		}

		public void Validate(IList<TEntity> entities)
		{
			if (Validating != null)
				entities.ForEach(r => Validating(r));
		}

		#endregion


		#region Create/New

		public event EntityModifyEvent Created;

		public TEntity New()
		{
			var r = Activator.CreateInstance<TEntity>();
			Created?.Invoke(r);
			return r;
		}

		#endregion


		#region Save

		public event EntityModifyEvent Inserting;
		public event EntityModifyEvent Inserted;
		public event EntityModifyEvent Updating;
		public event EntityModifyEvent Updated;
		public event EntityModifyEvent Modifing;
		public event EntityModifyEvent Modified;

		private void InsertedKey(TEntity r) { }
		private void UpdatedKey(TEntity r) { }
		private void DeletedKey(TEntity r) { }


		protected virtual TEntity Save(TEntity r, Action<Action<TEntity>, Action<TEntity>> onCommit)
		{

			if (r == null) throw new ArgumentNullException(nameof(r));

			if (onCommit == null) throw new ArgumentNullException(nameof(onCommit));


			if (typeof(TEntity).IsAbstract)
				throw new InvalidOperationException(GetType().FullName + ": Can`t save abstract entity.");


			if (_savingEntities == null)
				_savingEntities = new HashSet<TEntity>();
			else if (_savingEntities.Contains(r))
				return r;


			_savingEntities.Add(r);


			try
			{
				db.LoadOldValues(r);

				var isNew = db.IsNew(r);

				Validate(r);

				if (isNew)
				{
					Inserting?.Invoke(r);
				}
				else
				{
					Updating?.Invoke(r);
				}

				Modifing?.Invoke(r);

				Calculate(r);


				Session.Save(r);


				if (isNew)
				{
					if (Inserted != null || Modified != null)
					{
						onCommit(InsertedKey, rr =>
						{
							Inserted?.Invoke(r);
							Modified?.Invoke(r);
						});
					}
				}
				else
				{
					if (Updated != null || Modified != null)
					{
						onCommit(UpdatedKey, rr =>
						{
							Updated?.Invoke(r);
							Modified?.Invoke(r);
						});
					}
				}
				return r;
			}
			finally { _savingEntities.Remove(r); }
		}

		[DebuggerStepThrough]
		public virtual TEntity Save(TEntity r)
		{
			return Save(r, (keyAction, action) => db.OnCommit(r, keyAction, action));
		}

		[DebuggerStepThrough]
		public TEntity Save<TMasterEntity>(TMasterEntity mr, TEntity r)
			where TMasterEntity : class, IEntity
		{
			return Save(r, (keyAction, action) => db.OnCommit(mr, r, keyAction, action));
		}

		[DebuggerStepThrough]
		public void Save(IEnumerable<TEntity> entities)
		{
			entities?.ForEach(a => Save(a));
		}

		private HashSet<TEntity> _savingEntities;

		#endregion


		#region Delete

		public event EntityModifyEvent Deleting;
		public event EntityModifyEvent Deleted;


		protected virtual bool Delete(TEntity r, Action<Action<TEntity>> onCommit)
		{
			if (r == null) return false;

			Deleting?.Invoke(r);

			Session.Delete(r);

			if (Deleted != null)
				db.OnCommit(r, DeletedKey, rr => Deleted(rr));

			return true;
		}


		[DebuggerStepThrough]
		public virtual bool Delete(TEntity r)
		{
			return Delete(r, action => db.OnCommit(r, action));
		}

		[DebuggerStepThrough]
		public void Delete(IEnumerable<TEntity> entities)
		{
			entities?.ForEach(a => Delete(a));
		}


		[DebuggerStepThrough]
		public bool Delete<TMasterEntity>(TMasterEntity mr, TEntity r)
			where TMasterEntity : class, IEntity
		{
			return Delete(r, action => db.OnCommit(mr, r, action));
		}


		[DebuggerStepThrough]
		public bool Delete(object id)
		{
			var r = Load(id);
			return Delete(r);
		}

		[DebuggerStepThrough]
		public bool Delete<TMasterEntity>(TMasterEntity mr, object id)
			where TMasterEntity : class, IEntity
		{
			var r = Load(id);
			return Delete(mr, r);
		}

		[DebuggerStepThrough]
		public List<object> Delete(object[] ids)
		{
			return (
				from id in ids
				let entity = Load(id)
				where Delete(entity)
				select id
				).ToList();
		}


		[DebuggerStepThrough]
		public List<object> Delete<TMasterEntity>(TMasterEntity mr, object[] ids)
			where TMasterEntity : class, IEntity
		{
			return (
				from id in ids
				let entity = Load(id)
				where Delete(mr, entity)
				select id
				).ToList();
		}

		#endregion

		#endregion


		#region Queryable

		protected virtual IQueryable<TEntity> NewQuery()
		{
			return Session.Query<TEntity>();
		}

		public IQueryable<TEntity> Query => NewQuery();

		//public Expression Expression
		//{
		//	get { return Query.Expression; }
		//}

		//public Type ElementType
		//{
		//	get { return Query.ElementType; }
		//}

		//public IQueryProvider Provider
		//{
		//	get { return Query.Provider; }
		//}


		//public IEnumerator<TEntity> GetEnumerator()
		//{
		//	return Query.GetEnumerator();
		//}

		//IEnumerator IEnumerable.GetEnumerator()
		//{
		//	return GetEnumerator();
		//}

		#endregion


//		#region Operators
//
//		[DebuggerStepThrough]
//		public static TEntity operator +(Reference reference, EntityService<TDomain, TEntity> service)
//		{
//			return service.Load(reference);
//		}
//
//		#endregion

	}



}