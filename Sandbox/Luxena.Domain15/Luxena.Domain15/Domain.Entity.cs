using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Luxena.Domain
{


	partial class Domain<TDomain, TEntity, TKey>
	{

		public abstract class Entity
		{

			[DebuggerStepThrough]
			protected Entity()
			{
				Id = (TKey)NewId();
			}

			[DebuggerStepThrough]
			public static object NewId()
			{
				return ++lastId;
			}

			private static int lastId;

			protected internal TDomain db;
			protected internal TEntity _old, _new;
			protected internal TEntity _original;


			public abstract Type GetClass();

			public virtual TKey Id { get; set; }


			public abstract void Flush(bool isDeleted);


			public virtual TEntity Clone(TDomain domain)
			{
				var c = (TEntity)MemberwiseClone();

				c._original = (TEntity)this;

				c.db = db = domain;

				return c;
			}

			public virtual TEntity EmptyClone()
			{
				return (TEntity)Activator.CreateInstance(GetType());
			}

			public override bool Equals(object obj)
			{
				if (obj == null) return false;

				var entity2 = (TEntity)obj;

				return
					ReferenceEquals(this, entity2) ||
						ReferenceEquals(_original, entity2) ||
						ReferenceEquals(entity2._original, this) ||
						Equals(Id, entity2.Id);
			}

			public static bool operator ==(Entity a, Entity b)
			{
				return Equals(a, b);
			}

			public static bool operator !=(Entity a, Entity b)
			{
				return !Equals(a, b);
			}


			public virtual IList<TEntity> GetDependents()
			{
				return null;
			}

			public Action<TEntity> _OnPreCalculate;

			public virtual void Bind() { }

			public virtual void Calculate() { }

			public void ModifyMaster<TMasterEntity, TEntity2>(
				Func<TEntity2, TMasterEntity> masterGetter,
				Action<TEntity2, TMasterEntity> masterSetter,
				Func<TMasterEntity, IList<TEntity2>> detailsGetter,
				Action<TMasterEntity, IList<TEntity2>> detailsSetter
				)
				where TMasterEntity : TEntity
				where TEntity2 : TEntity
			{

				var entity = (TEntity2)this;
				var newEntity = (TEntity2)_new;
				var oldEntity = (TEntity2)_old;

				var newMaster = masterGetter(newEntity);
				var oldMaster = masterGetter(oldEntity);

				if (Equals(newMaster, oldMaster)) return;

				if (oldMaster != null)
				{
					var oldDetails = detailsGetter(oldMaster);

					if (oldDetails != null)
						oldDetails.Remove(entity);
				}

				if (newMaster != null)
				{
					var details = detailsGetter(newMaster);

					if (details == null)
						detailsSetter(newMaster, details = new List<TEntity2>());

					if (!details.Contains(entity))
						details.Add(entity);
				}
			}

		}
	}

}