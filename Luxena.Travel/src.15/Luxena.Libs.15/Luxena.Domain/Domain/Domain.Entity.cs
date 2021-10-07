using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;


namespace Luxena.Domain
{

	public interface IIdContainer
	{
		object GetId();
	}

	public interface INameContainer
	{
		string GetName();
	}

	//public interface ICalculateContainer
	//{
	//	void CalculateOnLoad();
	//	void Calculate();
	//	bool IsSaving();
	//	void IsSaving(bool? value);
	//}


	partial class Domain<TDomain>
	{

		public IEnumerable<Entity> GetChangedEntities(EntityState state)
		{
			var objectContext = (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
			return objectContext.ObjectStateManager.GetObjectStateEntries(state).Select(a => a.Entity).OfType<Entity>();
		}

		public IEnumerable<Entity> AddedEntities => GetChangedEntities(EntityState.Added);
		public string AddedEntitiesString => AddedEntities.Join(", ");

		public IEnumerable<Entity> ModifiedEntities => GetChangedEntities(EntityState.Modified);
		public string ModifiedEntitiesString => ModifiedEntities.Join(", ");

		public IEnumerable<Entity> SavedEntities => GetChangedEntities(EntityState.Added | EntityState.Modified);
		public string SavedEntitiesString => SavedEntities.Join(", ");

		public IEnumerable<Entity> DeletedEntities => GetChangedEntities(EntityState.Deleted);
		public string DeletedEntitiesString => DeletedEntities.Join(", ");


		public abstract bool KeyIsEmpty(object key);

		public abstract class Entity //: ICalculateContainer
		{

			protected internal TDomain db;

			protected internal Entity _cloneSource;

			protected internal virtual Entity GetOld() { throw new NotImplementedException(); }
			protected internal virtual void SetOld(Entity value) { }

			protected internal virtual Entity GetNew() { throw new NotImplementedException(); }
			protected internal virtual void SetNew(Entity value) { }


			[DebuggerStepThrough]
			public abstract object GetId();

			[DebuggerStepThrough]
			public virtual Type GetClass()
			{
				throw new NotImplementedException();
			}

			[DebuggerStepThrough]
			public virtual DbSet GetDbSet(TDomain domain = null)
			{
				throw new NotImplementedException();
			}

			[DebuggerStepThrough]
			public virtual Entity GetFromDb()
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Вызывается при чтении сущности по Id (т.е. в формах просмотра и редактирования).
			/// </summary>
			public virtual void CalculateOnLoad() { }

			/// <summary>
			/// Вызывается при создании сущности в UI - заполняет свойства значениями по умолчанию.<br/>
			/// При этом уже переданы значения свойств из UI, заданные в качестве параметров (например, Master-свойство).
			/// </summary>
			public virtual void CalculateDefaults() { }

			/// <summary>
			/// Возвращает все сущности, зависящие от данной.<br/>
			/// Т.е. те, которые нужно пересчитать.
			/// </summary>
			protected internal virtual IList<Entity> GetDependents() { return null; }

			protected internal virtual void Bind() { }

			public virtual void Calculate() { }

			/// <summary>
			/// Имя последнего свойства, вызвавшего рекалькуляцию в интерфейсе.
			/// </summary>
			[NotDbMapped]
			public string LastChangedPropertyName { get; set; }


			[DebuggerStepThrough]
			public abstract bool IsNew();

			[DebuggerStepThrough]
			public abstract bool GetIsSaving();

			public bool IsSaving() => _isSaving ?? (_isSaving = GetIsSaving()) ?? false;
			public void IsSaving(bool? value) => _isSaving = value;
			internal bool? _isSaving; 


			protected internal virtual void Flush(bool isDeleted)
			{
				SetNew(null);
				SetOld(null);

				if (isDeleted && IsNew())
				{
					var entry = db.Entry(this);
					entry.State = EntityState.Detached;
				}
				else
				{
					var entry = db.Entry(this);

					if (entry.IsNew())
						GetDbSet().Add(this);
					else if (isDeleted)
						GetDbSet().Remove(this);
					else
						entry.State = EntityState.Modified;
				}
			}


			public static implicit operator string(Entity me) => me?.ToString();

			public static implicit operator bool(Entity me) => me != null;

			public virtual Entity Resolve() => this;

			[DebuggerStepThrough]
			public static Entity operator +(Entity r, TDomain domain)
			{
				if (r == null) return null;
				r.db = domain;
				return r.Resolve();
			}



			#region Clone

			//protected internal virtual Entity Clone()
			//{
			//	var c = (Entity)MemberwiseClone();

			//	c._cloneSource = this;

			//	return c;
			//}

			public virtual Entity EmptyClone()
			{
				return (Entity)Activator.CreateInstance(GetClass());
			}

			public Entity OriginalClone()
			{
				return IsNew() ? EmptyClone() : GetFromDb();
			}

			#endregion


			#region Binding Tools

			//public void ModifyMaster<TMasterEntity, Entity2>(
			//	Func<Entity2, TMasterEntity> masterGetter,
			//	Action<Entity2, TMasterEntity> masterSetter,
			//	Func<TMasterEntity, ICollection<Entity2>> detailsGetter,
			//	Action<TMasterEntity, ICollection<Entity2>> detailsSetter
			//)
			//	where TMasterEntity : Entity
			//	where Entity2 : Entity
			//{

			//	var entity = (Entity2)this;
			//	var newEntity = (Entity2)GetNew();
			//	var oldEntity = (Entity2)GetOld();

			//	var newMaster = masterGetter(newEntity);
			//	var oldMaster = masterGetter(oldEntity);

			//	if (Equals(newMaster, oldMaster)) return;

			//	if (oldMaster != null)
			//	{
			//		var oldDetails = detailsGetter(oldMaster);

			//		oldDetails?.Remove(entity);
			//	}

			//	if (newMaster != null)
			//	{
			//		var details = detailsGetter(newMaster);

			//		if (details == null)
			//			detailsSetter(newMaster, details = new List<Entity2>());

			//		if (!details.Contains(entity))
			//			details.Add(entity);
			//	}
			//}


			public void ModifyMaster<TMasterEntity, TDetailEntity>(
				Func<TDetailEntity, TMasterEntity> masterGetter,
				Expression<Func<TMasterEntity, ICollection<TDetailEntity>>> detailsExpr
			)
				where TMasterEntity : Entity
				where TDetailEntity : Entity
			{

				var entity = (TDetailEntity)this;
				var newEntity = (TDetailEntity)GetNew();
				var oldEntity = (TDetailEntity)GetOld();

				var detailsProp = detailsExpr.GetProperty();

				var newMaster = masterGetter(newEntity);
				var oldMaster = masterGetter(oldEntity);

				if (Equals(newMaster, oldMaster)) return;

				if (oldMaster != null)
				{
					var oldDetails = (ICollection<TDetailEntity>)detailsProp.GetValue(oldMaster);

					oldDetails?.Remove(entity);
				}

				if (newMaster != null)
				{
					var newDetails = (ICollection<TDetailEntity>)detailsProp.GetValue(newMaster);

					if (newDetails == null)
						detailsProp.SetValue(newMaster, newDetails = new List<TDetailEntity>());

					if (!newDetails.Contains(entity))
						newDetails.Add(entity);
				}
			}

			#endregion

		}


		public abstract class Entity<TKey> : Entity, IIdContainer
		{

			[Key]
			public TKey Id { get; set; }

			public override object GetId() => Id;

			public virtual TKey GetEmptyId() => default(TKey);

			object IIdContainer.GetId() => Id;

			//[DebuggerStepThrough]
			//protected abstract TKey NewId();

			[DebuggerStepThrough]
			protected abstract bool HasId();

			//public override void Calculate()
			//{
			//	base.Calculate();

			//	if (!HasId())
			//		Id = NewId();
			//}


			#region Equals

			public override bool Equals(object obj)
			{
				var entity = obj as Entity<TKey>;

				if (entity == null) return false;

				return
					ReferenceEquals(this, entity) ||
					ReferenceEquals(_cloneSource, entity) ||
					ReferenceEquals(entity._cloneSource, this) ||
					!Equals(Id, default(TKey)) && Equals(Id, entity.Id) && entity.GetClass() == GetClass();
			}

			public override int GetHashCode()
			{
				// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
				return Id != null ? Id.GetHashCode() : base.GetHashCode();
			}


			public static bool operator ==(Entity<TKey> a, Entity<TKey> b)
			{
				return Equals(a, b);
			}

			public static bool operator !=(Entity<TKey> a, Entity<TKey> b)
			{
				return !Equals(a, b);
			}

			#endregion

		}


	}


	public static class EntityExtensions
	{

		[DebuggerStepThrough]
		public static TEntity Domain<TDomain, TEntity>(this TEntity entity, TDomain db)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity
		{
			if (entity == null) return null;
			entity.db = db;
			return entity;
		}

		[DebuggerStepThrough]
		public static TEntity GetFromDb<TDomain, TEntity>(this TEntity entity, TDomain db)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity
		{
			if (entity == null) return null;
			entity.db = db;
			return (TEntity)entity.GetFromDb();
		}


		//public static TEntity Clone<TDomain, TEntity>(this TEntity entity, TDomain db)
		//	where TDomain : Domain<TDomain>, new()
		//	where TEntity : Domain<TDomain>.Entity
		//{
		//	if (entity == null) return null;

		//	entity.SetDomain(db);

		//	return (TEntity)entity.Clone();
		//}


		//[DebuggerStepThrough]
		//public static List<TEntity> Clone<TDomain, TEntity>(this ICollection<TEntity> entities, TDomain db)
		//	where TDomain : Domain<TDomain>, new()
		//	where TEntity : Domain<TDomain>.Entity
		//{
		//	return
		//		entities == null ? null :
		//		entities.Select(a => a.Clone(db)).ToList();
		//}


		[DebuggerStepThrough]
		public static void Delete<TDomain>(this Domain<TDomain>.Entity entity, TDomain db)
			where TDomain : Domain<TDomain>, new()
		{
			if (entity == null) return;

			db.CurrentTransaction.Save(entity, true);
		}

		//[DebuggerStepThrough]
		public static void Delete<TDomain>(this IEnumerable<Domain<TDomain>.Entity> entities, TDomain db)
			where TDomain : Domain<TDomain>, new()
		{
			if (entities == null) return;

			db.CurrentTransaction.Save(entities, true);
		}


		public static TEntity Resolve<TDomain, TEntity>(this TEntity entity, TDomain db)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity
		{
			if (entity == null) return null;

			entity.db = db;

			return (TEntity)entity.Resolve();
		}

		[DebuggerStepThrough]
		public static TEntity Save<TDomain, TEntity>(this TEntity entity, TDomain db)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity
		{
			if (entity == null) return null;

			db.CurrentTransaction.Save(entity);

			return entity;
		}

		[DebuggerStepThrough]
		public static void Save<TDomain, TEntity>(this IEnumerable<TEntity> entities, TDomain db)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity
		{
			if (entities == null) return;

			db.CurrentTransaction.Save(entities);
		}

		public static void Save<TDomain, TEntity>(this ICollection<TEntity> entities, TDomain db)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity
		{
			if (entities == null) return;

			db.CurrentTransaction.Save(entities);
		}


		[DebuggerStepThrough]
		public static TEntity Update<TDomain, TEntity>(this TEntity entity, TDomain db, Action<TEntity> update)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity
		{
			if (entity == null || update == null) return entity;
				
			update(entity);
			db.CurrentTransaction.Save(entity);

			return entity;
		}

		[DebuggerStepThrough]
		public static TEntity Update<TDomain, TEntity>(this TEntity entity, TDomain db, Action<TDomain, TEntity> update)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity
		{
			if (entity == null || update == null) return entity;

			update(db, entity);
			db.CurrentTransaction.Save(entity, false);

			return entity;
		}

		public static IQueryable<TEntity> WhereIdEquals<TDomain, TEntity, TKey>(this IQueryable<TEntity> entities, TKey key)
			where TDomain : Domain<TDomain>, new()
			where TEntity : Domain<TDomain>.Entity<TKey>
		{
			var parameter = Expression.Parameter(typeof(TEntity), "a");

			var predicate = Expression.Lambda<Func<TEntity, bool>>(
				Expression.Equal(
					Expression.Property(parameter, "Id"),
					Expression.Constant(key)
				),
				parameter
			);

			return entities.Where(predicate);
		}

	}

}