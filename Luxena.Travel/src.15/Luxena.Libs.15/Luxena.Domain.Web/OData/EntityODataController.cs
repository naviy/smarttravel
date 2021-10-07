using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;

using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Core.UriParser.TreeNodeKinds;


namespace Luxena.Domain.Web
{

	public abstract class ReadOnlyEntityODataController<TDomain, TEntity, TKey> : DomainODataController<TDomain>
		where TDomain : Domain<TDomain>, new()
		where TEntity : Domain<TDomain>.Entity<TKey>
	{

		protected DbSet<TEntity> Set => _set ?? (_set = db.Set<TEntity>());
		private DbSet<TEntity> _set;

		protected IQueryable<TEntity> Query => _query ?? (_query = GetQuery());
		private IQueryable<TEntity> _query;

		protected virtual Domain<TDomain>.EntityQuery<TEntity> GetEntityQuery()
		{
			return null;
		}

		protected virtual IQueryable<TEntity> GetQuery()
		{
			return (IQueryable<TEntity>)GetEntityQuery() ?? Set;
		}

		protected IQueryable<TEntity> Select(TKey key)
		{
			return Query.WhereIdEquals<TDomain, TEntity, TKey>(key);
		}

		protected TEntity Find(TKey key)
		{
			if (key.AsString() == "single")
				return Query.SingleOrDefault();

			return Select(key).One();
		}



		[EnableQuery]
		public IHttpActionResult Get(ODataQueryOptions<TEntity> options)
		{
			db.InitSecurity();

			var query = Query;

			var id = GetIdFilterValue(options);

			if (id == "single")
				options.ClearFilter();

			query = query.Where(options);

			if (options.SelectExpand == null || Request.RequestUri.AbsoluteUri.As(a => a.Contains("usecalculated=true") || a.Contains(".usecalculated")))
			{
				var list = query.ToList();

				list.ForEach(a => a.Domain(db));

				if (id.Yes())
				{
					var entity = list.One();
					if (entity != null)
					{
						entity.IsSaving(false);
						entity.CalculateOnLoad();
						entity.IsSaving(null);
					}
				}

				query = list.AsQueryable();
			}

			if (options.SelectExpand != null)
				Request.ODataProperties().SelectExpandClause = options.SelectExpand.SelectExpandClause;

			return Ok(query);
		}

		//[EntityQuery]
		public SingleResult<TEntity> Get([FromODataUri] TKey key, ODataQueryOptions<TEntity> queryOptions)
		{
			db.InitSecurity();

			return SingleResult.Create(Set.WhereIdEquals<TDomain, TEntity, TKey>(key));
		}


		#region Utilites

		string GetIdFilterValue(ODataQueryOptions queryOptions)
		{
			var expr = queryOptions.Filter?.FilterClause.Expression as BinaryOperatorNode;
			if (expr != null && expr.OperatorKind == BinaryOperatorKind.Equal)
			{
				var propNode = expr.Left.Unconvert();
				var propName = ((SingleValuePropertyAccessNode)propNode).Property.Name;
				if (propName == "Id")
					return (expr.Right.Unconvert() as ConstantNode)?.Value.AsString();
			}

			return null;
		}


		protected IHttpActionResult ExecEntityAction<TResult>(TKey key, ODataActionParameters parameters, Func<TEntity, TResult> action)
		{
			var deltaJson = (string)parameters.By("_delta");

			TEntity r;

			if (deltaJson.Yes())
			{
				var delta = new Delta<TEntity>();
				var values = Deserialize(deltaJson);

				foreach (var pv in values)
				{
					delta.TrySetPropertyValue(pv.Key, pv.Value);
				}


				if (db.KeyIsEmpty(key))
					r = delta.GetEntity();
				else
				{
					r = Find(key);
					if (r == null) return NotFound();

					delta.Patch(r);
				}
			}
			else
			{
				r = Find(key);
				if (r == null) return NotFound();
			}


			var result = action(r);

			if ((bool?)parameters.By("_save") == true)
				db.Commit(() => r.Save(db));

			return result as IHttpActionResult ?? Ok(result);
		}


		protected IHttpActionResult ExecEntityAction(TKey key, ODataActionParameters parameters, Action<TEntity> action)
		{
			return ExecEntityAction<IHttpActionResult>(key, parameters, r =>
			{
				action(r);

				if ((bool?)parameters.By("_resync") == true)
				{
					if (r.IsNew())
						r.Id = r.GetEmptyId();
					return Ok(r);
				}

				return Ok();
			});
		}

		#endregion
	}



	public abstract class EntityODataController<TDomain, TEntity, TKey> : ReadOnlyEntityODataController<TDomain, TEntity, TKey>
	where TDomain : Domain<TDomain>, new()
	where TEntity : Domain<TDomain>.Entity<TKey>, new()
	{

		protected IHttpActionResult Save(TKey key, TEntity r, bool useFlush = true)
		{
			try
			{
				db.Commit(() => r.Save(db), useFlush: useFlush);

				return null;
			}

			catch (DbUpdateConcurrencyException)
			{
				if (!Select(key).Any())
					return NotFound();

				throw;
			}

			catch (DbEntityValidationException ex)
			{
				var msg = "Необходимо скорректировать введённые данные.\r\n" +
					ex.EntityValidationErrors.SelectMany(a => a.ValidationErrors.Select(b => b.ErrorMessage)).Join("\r\n");

				throw new Exception(msg, ex);
			}

			catch (Exception ex)
			{
				var ex2 = new Exception("SAVE ERROR", ex);
				db.Log(ex);
				throw ex2;
			}
		}



		//[HttpGet]
		//public IHttpActionResult Defaults(TEntity r)
		//{
		//	db.InitSecurity();

		//	r = r ?? new TEntity();

		//	r.SetDomain(db);

		//	r.CalculateDefaults();

		//	var entityQuery = GetEntityQuery();
		//	if (entityQuery != null)
		//		entityQuery.CalculateDefaults(r);

		//	var json = Serialize(r);

		//	return Ok(json);
		//}

		public IHttpActionResult Post(TEntity r)
		{
			db.InitSecurity();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			return Save(default(TKey), r) ?? Created(r);
		}


		/// <summary>
		/// Возвращает значения по умолчанию
		/// </summary>
		[EnableQuery]
		public IHttpActionResult Put([FromODataUri] TKey key, TEntity r)
		{
			db.InitSecurity();

			r = r ?? new TEntity();

			r.Id = r.GetEmptyId();

			r.Domain(db);

			r.CalculateDefaults();

			GetEntityQuery()?.CalculateDefaults(r);

			r.Calculate();

			return Ok(r);
		}


		[EnableQuery]
		public IHttpActionResult Patch([FromODataUri] TKey key, Delta<TEntity> delta)
		{
			if (delta == null)
				throw new Exception("PATCH: delta is null. Попытка передать несовместимые данные для Delta<" + typeof(TEntity).Name + ">");

			db.InitSecurity();

			var nr = delta.GetEntity();

			if (nr.IsSaving())
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var r = Find(key);
				if (r == null) return NotFound();

				try
				{
					delta.Patch(r);
				}
				catch (Exception ex)
				{
					var ex2 = new Exception("PATCH SAVE Error", ex);
					db.Log(ex);
					throw ex2;
				}

				return Save(key, r) ?? Updated(key); // ?? Updated(r);
			}
			else
			{
				var r = nr;
				if (!nr.IsNew())
				{
					r = Find(key);
					if (r == null) return NotFound();

					try
					{
						delta.Patch(r);
					}
					catch (Exception ex)
					{
						var ex2 = new Exception("PATCH CALCULATE Error", ex);
						db.Log(ex);
						throw ex2;
					}
				}

				return Save(key, r, useFlush: false) ?? Ok(r);
			}
		}


		public IHttpActionResult Delete([FromODataUri] TKey key)
		{
			db.InitSecurity();

			var r = Find(key);
			if (r == null) return NotFound();

			db.Commit(() => r.Delete(db));

			return StatusCode(HttpStatusCode.NoContent);
		}

	}

}