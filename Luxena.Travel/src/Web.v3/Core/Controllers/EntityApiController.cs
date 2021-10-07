using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using DelegateDecompiler;

using Luxena.Base.Domain;
using Luxena.Domain.Contracts;
using Luxena.Domain.Entities;


namespace Luxena.Domain.Web
{

	public abstract class EntityApiController<TDomain, TEntity, TEntityService> : DomainApiController<TDomain>
		where TDomain : Domain<TDomain>
		where TEntity : class, IEntity
		where TEntityService : EntityService<TDomain, TEntity>, new()
	{
		public TEntityService Service { get { return Domain.Service(ref _entityService); } }
		private TEntityService _entityService;

		#region Result

		protected HttpResponseMessage Result<TEntityService2, T>(TEntityService2 service, Func<TEntityService2, T> query) where T : class
		{
			var data = Domain.Commit(() => query(service));
			return Result(data);
		}

		protected HttpResponseMessage Result<TEntityService2, T>(TEntityService2 service, Func<TEntityService2, IQueryable<T>> query) where T : class
		{
			var data = Domain.Commit(() => query(service).Decompile().ToList());
			return Result(data);
		}

		protected HttpResponseMessage Result<TEntityService2, T>(TEntityService2 service, Func<TEntityService2, IEnumerable<T>> query) where T : class
		{
			var data = Domain.Commit(() =>
			{
				var result = query(service);
				return result.ToList();
			});
			return Result(data);
		}

		protected HttpResponseMessage Result<TEntityService2, T>(TEntityService2 service, Func<TEntityService2, IList<T>> query) where T : class
		{
			var data = Domain.Commit(() => query(service));
			return Result(data);
		}

		protected HttpResponseMessage Result<T>(Func<TEntityService, T> query) where T : class
		{
			return Result(Service, query);
		}

		protected HttpResponseMessage Result<T>(Func<TEntityService, IQueryable<T>> query) where T : class
		{
			return Result(Service, query);
		}

		protected HttpResponseMessage Result<T>(Func<TEntityService, IEnumerable<T>> query) where T : class
		{
			return Result(Service, query);
		}

		protected HttpResponseMessage Result<T>(Func<TEntityService, IList<T>> query) where T : class
		{
			return Result(Service, query);
		}


		#endregion


		#region Range

		protected HttpResponseMessage Range<T>(Func<IQueryable<TEntity>, int> totalCount, Func<IQueryable<TEntity>, T> query)
		{
			var data = Domain.Commit(() => new
			{
				TotalCount = totalCount(Service),
				Data = query(Service)
			});

			return Result(data);
		}

		protected HttpResponseMessage Range<T>(Func<IQueryable<TEntity>, T> query)
		{
			return Range(q => q.Count(), query);
		}

		protected HttpResponseMessage Range<T>(RangeRequest request, Func<IQueryable<TEntity>, T> query)
		{
			var data = Domain.Commit(() => new
			{
				TotalCount = Service.Filter(request).Count(),
				Data = query(Service.Range(request))
			});

			return Result(data);
		}

		#endregion


		#region Contract

		[HttpPost]
		public virtual object List(RangeRequest request) { throw new NotImplementedException(); }

		protected virtual EntityContractService<,> EditContractService { get { return _editContractService ?? (_editContractService = NewEditContract()); } }
		private static EntityContractService<,> _editContractService;

		protected virtual EntityContractService<,> NewEditContract()
		{
			throw new NotImplementedException();
		}

		protected virtual EntityContractService<,> ViewContractService { get { return _viewContractService ?? (_viewContractService = NewViewContract()); } }
		private static EntityContractService<,> _viewContractService;

		protected virtual EntityContractService<,> NewViewContract()
		{
			throw new NotImplementedException();
		}

		protected static EntityContractService<,> NewContract<TContract>(
			Func<TDomain, TEntity, TContract> entityAsContract,
			bool useApplyDataToEntity = true,
			Action<TDomain, TContract, TEntity> contractToEntity = null,
			Func<TDomain, NameValueCollection, object> newContract = null
		)
			where TContract : class
		{
			return EntityContractService<,>.New(
				entityAsContract, useApplyDataToEntity, contractToEntity, newContract
			);
		}

		protected HttpResponseMessage Result(object id, EntityContractService<,> contractService, NameValueCollection prms = null)
		{
			var contract = Domain.Commit(() =>
			{
				var entity = Service.By(id);
				return entity != null 
					? entity.As(r => contractService.EntityAsContract(Domain, r))
					: contractService.GetNewContract(Domain, prms);

			});
			return Result(contract);
		}

		protected HttpResponseMessage Update(Dictionary<string, object> data, EntityContractService<,> contractService)
		{
			if (data == null) return null;

			var id = (string)data.By("Id");
			var isNew = id.No();

			var result = Domain.Commit(() =>
			{
				var entity = isNew ? Activator.CreateInstance<TEntity>() : Service.By(id);
				contractService.Apply(Domain, data, entity);
				Service.Save(entity);

				return contractService.EntityAsContract(Domain, entity);
			});

			return Result(result);
		}

		[HttpGet]
		public virtual object View(string id)
		{
			return Result(id, ViewContractService);
		}

		[HttpGet]
		public virtual object Edit(string id)
		{
			var prms = HttpUtility.ParseQueryString(Request.RequestUri.Query);
			return Result(id, EditContractService, prms);
		}

		[HttpPost, HttpPut]
		public virtual object Save(Dictionary<string, object> data)
		{
			return Update(data, EditContractService);
		}

		public class DeleteArgs
		{
			public object[] Ids { get; set; }
		}
		//Fake
		[HttpPost]
		public virtual object Delete(DeleteArgs e)
		{
			if (e.Ids.No())
				return Result(new string[] { });

			var result = Domain.Commit(() => new
			{
				Ids = Service.Delete(e.Ids),
				TotalCount = Service.Count(),
			});

			return result;
		}


		protected HttpResponseMessage Suggest<T>(Func<TEntityService, IQueryable<T>> query) where T : class
		{
			return Result(q => query(q).Take(15));
		}


		#endregion
	}

}