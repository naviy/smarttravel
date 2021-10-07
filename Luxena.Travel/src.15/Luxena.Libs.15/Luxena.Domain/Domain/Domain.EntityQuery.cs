using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

			public IQueryable<TEntity> Query { get { return _query ?? (_query = GetQuery()); } }
			private IQueryable<TEntity> _query;

			protected abstract IQueryable<TEntity> GetQuery();

			//public virtual void GetDefaultValues(ObjectValues<TEntity> v) { }

			public virtual void CalculateDefaults(TEntity r) { }


			#region IQueryable<TEntity>

			public IEnumerator<TEntity> GetEnumerator() { return Query.GetEnumerator(); }

			IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

			public Expression Expression { get { return Query.Expression; } }

			public Type ElementType { get { return Query.ElementType; } }

			public IQueryProvider Provider { get { return Query.Provider; } }

			#endregion


			[Key]
			public string Id { get; set; }

		}

	}

}
