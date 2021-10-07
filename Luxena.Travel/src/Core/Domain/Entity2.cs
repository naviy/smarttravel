using System;

using Luxena.Base.Data;
using Luxena.Base.Metamodel;


namespace Luxena.Base.Domain
{

	public class Entity : IEntity
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

		public static implicit operator string(Entity me)
		{
			return me == null ? null : me.ToString();
		}


		public virtual object Clone()
		{
			var r = (Entity)MemberwiseClone();

			r.Id = null;
			r.Version = 0;

			return r;
		}

		public virtual T Clone<T>()
			where T: Entity
		{
			return (T)Clone();
		}

		public static implicit operator EntityReference(Entity entity)
		{
			return entity == null ? null : new EntityReference(entity);
		}

//		public virtual Entity Resolve(DomainBase db)
//		{
//			
//		}
//
//
//		public static operator +(Entity a, DomainBase db)
//		{
//			return a == null ? null : a.Resolve(db);
//		}

	}


	public class Entity2 : Entity, IEntity2
	{

		public virtual DateTime CreatedOn { get; set; }

		public virtual string CreatedBy { get; set; }

		public virtual DateTime? ModifiedOn { get; set; }

		public virtual string ModifiedBy { get; set; }


		public override object Clone()
		{
			var r = (Entity2) base.Clone();

			r.CreatedOn = DateTime.Now;
			r.CreatedBy = null;

			r.ModifiedOn = null;
			r.ModifiedBy = null;

			return r;
		}
	}

}