using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;


namespace Luxena.Domain
{

	partial class Domain<TDomain>
	{

		public abstract class EntityQuery<TEntity> : IQueryable<TEntity>
			where TEntity : Entity
		{

			public TDomain db;

			public IQueryable<TEntity> Query => _query ?? (_query = GetQuery());
			private IQueryable<TEntity> _query;

			protected abstract IQueryable<TEntity> GetQuery();

			public virtual void CalculateDefaults(TEntity r) { }


			#region IQueryable<TEntity>

			public IEnumerator<TEntity> GetEnumerator() => Query.GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

			public Expression Expression => Query.Expression;

			public Type ElementType => Query.ElementType;

			public IQueryProvider Provider => Query.Provider;

			#endregion


			[Key]
			public string Id { get; set; }

		}

	}

}
