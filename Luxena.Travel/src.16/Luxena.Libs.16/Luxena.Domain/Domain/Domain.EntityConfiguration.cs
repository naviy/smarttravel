using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Luxena.Domain
{
	
	partial class Domain<TDomain>
	{

		public class EntityConfiguration<TEntity>
			where TEntity: Entity
		{

			// ReSharper disable once MemberHidesStaticFromOuterClass
			public readonly EntityInfo Entity;

			public EntityConfiguration(EntityInfo entity)
			{
				Entity = entity;
			}

			public EntityConfiguration<TEntity> Association<TTargetEntity>(
				Expression<Func<TEntity, TTargetEntity>> navigationPropertyExpression
			)
				where TTargetEntity : class
			{
				Entity.Associations[navigationPropertyExpression.GetProperties().One()] = null;

				return this;
			}

			public EntityConfiguration<TEntity> Association<TTargetEntity>(
				Expression<Func<TEntity, TTargetEntity>> navigationPropertyExpression,
				Expression<Func<TTargetEntity, ICollection<TEntity>>> collectionPropertyExpression
			)
				where TTargetEntity : class
			{
				Entity.Associations[navigationPropertyExpression.GetProperties().One()] =
					collectionPropertyExpression.GetProperties().One();

				return this;
			}

			public EntityConfiguration<TEntity> Association<TTargetEntity>(
				Expression<Func<TEntity, TTargetEntity>> navigationPropertyExpression,
				Expression<Func<TTargetEntity, TEntity>> collectionPropertyExpression
			)
				where TTargetEntity : class
			{
				Entity.Associations[navigationPropertyExpression.GetProperties().One()] =
					collectionPropertyExpression.GetProperties().One();

				return this;
			}

		}

	}

}
