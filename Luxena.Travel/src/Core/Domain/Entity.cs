using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public abstract class Entity : IEntity
	{

		public virtual object Id { get; set; }

		public virtual int Version { get; set; }

		public override bool Equals(object obj)
		{
			var entity = obj as Entity;

			if (entity == null)
				return false;

			if (ReferenceEquals(this, entity))
				return true;

			var id1 = Id as string;
			var id2 = entity.Id as string;

			if (id1 == null || id2 == null || id1 != id2)
				return false;

			var thisType = Class.TypeResolver.Resolve(this);
			var otherType = Class.TypeResolver.Resolve(entity);

			return thisType.Is(otherType) || otherType.Is(thisType);
		}

		public override int GetHashCode()
		{
			// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return Id != null ? HashCodeUtility.GetHashCode(Id) : base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Empty;
		}

		public static implicit operator string (Entity me)
		{
			return me?.ToString();
		}


		public virtual object Clone()
		{
			var r = (Entity)MemberwiseClone();

			r.Id = null;
			r.Version = 0;

			return r;
		}

		public virtual T Clone<T>()
			where T : Entity
		{
			return (T)Clone();
		}

		public static implicit operator EntityReference(Entity entity)
		{
			return entity == null ? null : new EntityReference(entity);
		}

		public virtual Entity Resolve(Domain db)
		{
			return this;
		}

		[DebuggerStepThrough]
		public static Entity operator +(Entity r, Domain db)
		{
			return r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public static implicit operator bool(Entity r)
		{
			return r != null;
		}

	}


	public abstract class Entity2 : Entity, IEntity2
	{

		[RU("Дата создания")]
		[DateTime2, ReadOnly, Required, Utility]
		public virtual DateTime CreatedOn { get; set; }

		[RU("Создано пользователем")]
		[ReadOnly, Required, Utility]
		public virtual string CreatedBy { get; set; }

		[RU("Дата изменения")]
		[DateTime2, ReadOnly, Utility]
		public virtual DateTime? ModifiedOn { get; set; }

		[RU("Изменено пользователем")]
		[ReadOnly, Utility]
		public virtual string ModifiedBy { get; set; }


		public override object Clone()
		{
			var r = (Entity2)base.Clone();

			r.CreatedOn = DateTime.Now;
			r.CreatedBy = null;

			r.ModifiedOn = null;
			r.ModifiedBy = null;

			return r;
		}

		[DebuggerStepThrough]
		public static Entity2 operator +(Entity2 r, Domain db)
		{
			return (Entity2)r?.Resolve(db);
		}

	}


	public abstract class Entity3 : Entity2, IEntity3
	{

		[Patterns.Name]
		[EntityName, Required]
		public virtual string Name { get; set; }


		public override string ToString()
		{
			return Name;
		}

		[DebuggerStepThrough]
		public static Entity2 operator +(Entity3 r, Domain db)
		{
			return (Entity3)r?.Resolve(db);
		}

	}

	public abstract class Entity3D : Entity3, IEntity3D
	{

		[Patterns.Description]
		public virtual string Description { get; set; }

	}
}