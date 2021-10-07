using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;


namespace Luxena.Domain
{

	public static class EFExtensions
	{

		public static bool IsNew<TEntity>(this DbEntityEntry<TEntity> entry) where TEntity : class
		{
			return entry == null || entry.State == EntityState.Detached || entry.State == EntityState.Added;
		}

		public static TEntity ById<TEntity>(this DbSet<TEntity> set, object id) where TEntity : class
		{
			return set == null || id == null ? null : set.Find(id);
		}

		public static TEntity ById<TEntity>(this DbSet<TEntity> set, IIdContainer r) where TEntity : class
		{
			return set == null || r == null ? null : set.Find(r.GetId());
		}


		public static bool IsDbMapped(this MemberInfo member)
		{
			return !member.HasAttribute<NotDbMappedAttribute>() && !member.HasAttribute<NotMappedAttribute>();
		}

		public static bool IsUiMapped(this MemberInfo member)
		{
			return !member.HasAttribute<NotUiMappedAttribute>() && !member.HasAttribute<NotMappedAttribute>();
		}

		public static bool IsCollection(this PropertyInfo prop)
		{
			return
				prop.PropertyType.IsInterface &&
				prop.PropertyType.IsGenericType &&
				prop.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>);
		}

		public static bool IsEntity(this Type type)
		{
			return type.IsSubclassOfRawGeneric(typeof(Domain<>.Entity));
		}

		public static bool IsReference(this Type type)
		{
			return type.Name.EndsWith("Reference");// && type.DeclaringType.IsSubclassOfRawGeneric(typeof(Domain<>.Entity));
		}

		public static bool IsReference(this PropertyInfo prop)
		{
			return prop.PropertyType.IsReference();
		}

		public static bool IsReference(this FieldInfo prop)
		{
			return prop.FieldType.IsReference();
		}


		public static bool IsNavigation(this Type type)
		{
			return type.IsEntity() || type.IsReference();
		}

		public static bool IsNavigation(this PropertyInfo prop)
		{
			return prop.PropertyType.IsNavigation();
		}

		public static bool IsNavigation(this FieldInfo prop)
		{
			return prop.FieldType.IsNavigation();
		}


		public static CascadableNavigationPropertyConfiguration HasOptional<TEntityType, TTargetEntity>(
			this EntityTypeConfiguration<TEntityType> entityConfiguration,
			Expression<Func<TEntityType, TTargetEntity>> navigationPropertyExpression,
			Expression<Func<TTargetEntity, ICollection<TEntityType>>> collectionPropertyExpression
		)
			where TEntityType : class
			where TTargetEntity : class
		{

			return entityConfiguration
				.HasOptional(navigationPropertyExpression)
				.WithMany(collectionPropertyExpression);
		}

		public static CascadableNavigationPropertyConfiguration HasOptional<TEntity, TTargetEntity>(
			this EntityTypeConfiguration<TEntity> entityConfiguration,
			Expression<Func<TEntity, TTargetEntity>> navigationPropertyExpression,
			Expression<Func<TTargetEntity, TEntity>> dependentPropertyExpression
		)
			where TEntity : class
			where TTargetEntity : class
		{

			return entityConfiguration
				.HasOptional(navigationPropertyExpression)
				.WithOptionalDependent(dependentPropertyExpression)
			;
		}


		public static CascadableNavigationPropertyConfiguration HasRequired<TEntityType, TTargetEntity>(
			this EntityTypeConfiguration<TEntityType> entityConfiguration,
			Expression<Func<TEntityType, TTargetEntity>> navigationPropertyExpression,
			Expression<Func<TTargetEntity, ICollection<TEntityType>>> collectionPropertyExpression
		)
			where TEntityType : class
			where TTargetEntity : class
		{

			return entityConfiguration
				.HasRequired(navigationPropertyExpression)
				.WithMany(collectionPropertyExpression);
		}


		public static EntityClassMapper<TEntity> MapClass<TEntity>(this EntityTypeConfiguration<TEntity> cfg, string discriminator)
			where TEntity : class
		{
			return new EntityClassMapper<TEntity>(cfg, discriminator);
		}

		public class EntityClassMapper<TEntity>
			where TEntity : class
		{
			public EntityClassMapper(EntityTypeConfiguration<TEntity> cfg, string _discriminator)
			{
				_cfg = cfg;
				discriminator = _discriminator;
			}

			private readonly EntityTypeConfiguration<TEntity> _cfg;
			private readonly string discriminator;

			public EntityClassMapper<TEntity> Map<TDerived>(string discriminatorValue = null)
				where TDerived : class, TEntity
			{
				_cfg.Map<TDerived>(m => m.Requires(discriminator).HasValue(discriminatorValue ?? typeof(TDerived).Name));
				return this;
			}
		}

	}

}
