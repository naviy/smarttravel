using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

using Luxena.Base.Domain;


namespace Luxena.Base.Data.NHibernate.Mapping
{


	public class EntityMapping<TEntity> : ClassMapping<TEntity>, IEntityMapping<TEntity>
		where TEntity : class
	{

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			EntityMappingHelper.Bag(this, property,cascadeStyle, inverseProperty);
		}

		public void Bag<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			EntityMappingHelper.Bag(this, property, cascadeStyle, inverseProperty, orderProperty);
		}

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			EntityMappingHelper.Bag(this, property, cascadeStyle, inverseProperty, orderProperty);
		}


		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty);
		}

		public void Bag<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty, orderProperty);
		}

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty, orderProperty);
		}


		public void BagAggregate<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty);
		}

		public void BagAggregate<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty, orderProperty);
		}

		public void BagAggregate<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty, orderProperty);
		}


		public void BagPersist<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty);
		}

		public void BagPersist<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty, orderProperty);
		}

		public void BagPersist<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty, orderProperty);
		}

	}


	public class JoinedSubEntityMapping<TEntity> : JoinedSubclassMapping<TEntity>, IEntityMapping<TEntity>
		where TEntity : class
	{

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			EntityMappingHelper.Bag(this, property, cascadeStyle, inverseProperty);
		}

		public void Bag<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			EntityMappingHelper.Bag(this, property, cascadeStyle, inverseProperty, orderProperty);
		}

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			EntityMappingHelper.Bag(this, property, cascadeStyle, inverseProperty, orderProperty);
		}


		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty);
		}

		public void Bag<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty, orderProperty);
		}

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty, orderProperty);
		}


		public void BagAggregate<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty);
		}

		public void BagAggregate<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty, orderProperty);
		}

		public void BagAggregate<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty, orderProperty);
		}


		public void BagPersist<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty);
		}

		public void BagPersist<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty, orderProperty);
		}

		public void BagPersist<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty, orderProperty);
		}

	}


	public class SubEntityMapping<TEntity> : SubclassMapping<TEntity>, IEntityMapping<TEntity>
		where TEntity : class
	{

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			EntityMappingHelper.Bag(this, property, cascadeStyle, inverseProperty);
		}

		public void Bag<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			EntityMappingHelper.Bag(this, property, cascadeStyle, inverseProperty, orderProperty);
		}

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			EntityMappingHelper.Bag(this, property, cascadeStyle, inverseProperty, orderProperty);
		}


		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty);
		}

		public void Bag<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty, orderProperty);
		}

		public void Bag<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.None, inverseProperty, orderProperty);
		}


		public void BagAggregate<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty);
		}

		public void BagAggregate<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty, orderProperty);
		}

		public void BagAggregate<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.All | Cascade.DeleteOrphans, inverseProperty, orderProperty);
		}


		public void BagPersist<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty);
		}

		public void BagPersist<TElement, TOrderProperty>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty, orderProperty);
		}

		public void BagPersist<TElement>(
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
		{
			Bag(property, Cascade.Persist, inverseProperty, orderProperty);
		}

	}


	internal interface IEntityMapping<TEntity>
		where TEntity : class
	{
		void Bag<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Action<IBagPropertiesMapper<TEntity, TElement>> collectionMapping,
			Action<ICollectionElementRelation<TElement>> mapping);
	}

	internal static class EntityMappingHelper
	{

		public static void Bag<TEntity, TElement>(
			IEntityMapping<TEntity> mapping,
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
			where TEntity : class
		{
			mapping.Bag(
				property,
				m => BagMapping(m, property, cascadeStyle, inverseProperty),
				r => r.OneToMany()
			);
		}

		public static void Bag<TEntity, TElement, TOrderProperty>(
			IEntityMapping<TEntity> mapping,
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty,
			Expression<Func<TElement, TOrderProperty>> orderProperty
		)
			where TEntity : class
		{
			mapping.Bag(
				property,
				m =>
				{
					BagMapping(m, property, cascadeStyle, inverseProperty);
					m.OrderBy(orderProperty);
				},
				r => r.OneToMany()
			);
		}

		public static void Bag<TEntity, TElement>(
			IEntityMapping<TEntity> mapping,
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty,
			string orderProperty
		)
			where TEntity : class
		{
			mapping.Bag(
				property,
				m =>
				{
					BagMapping(m, property, cascadeStyle, inverseProperty);
					m.OrderBy(orderProperty);
				},
				r => r.OneToMany()
			);
		}

		
		private static void BagMapping<TEntity, TElement>(
			ICollectionPropertiesMapper<TEntity, TElement> m,
			Expression<Func<TEntity, IEnumerable<TElement>>> property,
			Cascade cascadeStyle,
			Expression<Func<TElement, TEntity>> inverseProperty
		)
			where TEntity : class
		{
			var member = global::NHibernate.Mapping.ByCode.TypeExtensions.DecodeMemberAccessExpression(property);
			var isAutoProperty = ((PropertyInfo)member).GetGetMethod().GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any();

			if (!isAutoProperty)
				m.Access(Accessor.Field);

			m.Cascade(cascadeStyle);

			m.Inverse(inverseProperty);
		}
	}


	public class Entity2Mapping<TEntity> : EntityMapping<TEntity>
		where TEntity : class, IEntity2
	{
		public Entity2Mapping()
		{
			Id(x => x.Id, Uuid.Mapping);

			Version(x => x.Version, m => { });

			Property(x => x.CreatedBy, m => { m.Length(32); m.NotNullable(true); });

			Property(x => x.CreatedOn, m => m.NotNullable(true));

			Property(x => x.ModifiedBy, m => m.Length(32));

			Property(x => x.ModifiedOn);
		}
	}

	public class Entity3Mapping<TEntity> : Entity2Mapping<TEntity>
		where TEntity : class, IEntity3
	{
		public Entity3Mapping()
		{
			Property(x => x.Name, m => { m.Length(200); m.NotNullable(true); });
		}
	}

	public class Entity3DMapping<TEntity> : Entity3Mapping<TEntity>
		where TEntity : class, IEntity3D
	{
		public Entity3DMapping()
		{
			Property(x => x.Description);
		}
	}


}