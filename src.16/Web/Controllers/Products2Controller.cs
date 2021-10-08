//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.Controllers;
//using System.Web.Http.Dispatcher;
//using System.Web.Http.Routing;
//using System.Web.OData;
//using System.Web.OData.Builder;
//using System.Web.OData.Query;
//
//using Luxena.Domain;
//using Luxena.Domain.Web;
//
//
//namespace Luxena.Travel.Web
//{
//
//	using Domain;
//
//
//	public interface IODataAnonymousService
//	{
//		string Name { get; }
//
//		Type GetControllerType();
//
//		ODataController NewController();
//	}
//
//
//	public abstract class ODataAnonymousService
//	{
//
//		public static readonly ConcurrentDictionary<string, HttpControllerDescriptor> ControllerDescriptors = new ConcurrentDictionary<string, HttpControllerDescriptor>();
//
//		public static ODataAnonymousService<TDomain, TEntity, TContract> Create<TDomain, TEntity, TContract>(
//			string name,
//			Expression<Func<TEntity, TContract>> selector
//		)
//			where TDomain : Domain<TDomain>, new()
//			where TEntity : Domain<TDomain>.Entity
//			where TContract : class
//		{
//			var service = new ODataAnonymousService<TDomain, TEntity, TContract>
//			{
//				Name = name,
//				Selector = selector
//			};
//
//			return service;
//		}
//
//
//		public class ODataControllerSelector : DefaultHttpControllerSelector
//		{
//			public ODataControllerSelector(HttpConfiguration configuration) : base(configuration) {}
//
//
//			public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
//			{
//				var routeData = request.GetRouteData();
//				if (routeData == null) return null;
//
//				object controllerName;
//				routeData.Values.TryGetValue("controller", out controllerName);
//				var controllerNameAsString = controllerName.AsString();
//				HttpControllerDescriptor descriptor;
//
//				return ControllerDescriptors.TryGetValue(controllerNameAsString, out descriptor) 
//					? descriptor 
//					: base.SelectController(request);
//			}
//
//			public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
//			{
//				return ControllerDescriptors.Concat(base.GetControllerMapping());
//			}
//
//		}
//
//		public class ODataControllerDescriptor : HttpControllerDescriptor
//		{
//
//			public readonly IODataAnonymousService Service;
//
//
//			public ODataControllerDescriptor(HttpConfiguration config, IODataAnonymousService service) :
//				base(config, service.Name, service.GetControllerType())
//			{
//				Service = service;
//			}
//
//
//			public override IHttpController CreateController(HttpRequestMessage request)
//			{
//				return Service.NewController();
//			}
//		}
//	}
//
//
//	public class ODataAnonymousService<TDomain, TEntity, TContract> : ODataAnonymousService, IODataAnonymousService
//		where TDomain : Domain<TDomain>, new()
//		where TEntity : Domain<TDomain>.Entity
//		where TContract : class
//	{
//
//		public string Name { get; set; }
//
//		public Expression<Func<TEntity, TContract>> Selector { get; set; }
//
//
//		public void Register<TKey>(
//			HttpConfiguration config,
//			ODataConventionModelBuilder mb,
//			Expression<Func<TContract, TKey>> keyExpr
//		)
//		{
//			var descriptor = new ODataControllerDescriptor(config, this);
//			ControllerDescriptors[Name] = descriptor;
//	
//			var odataSet = mb.EntitySet<TContract>(Name);
//			var odataType = odataSet.EntityType.HasKey(keyExpr);
//
//			var param = Expression.Parameter(typeof(TEntity), "a");
//
//			foreach (var prop in typeof(TContract).GetProperties())
//			{
//				var expr = prop.ToExpression();
//				odataSet.
//
//			}
//		}
//
//
//		public System.Web.OData.ODataController NewController()
//		{
//			return new ODataController { Selector = Selector };
//		}
//
//
//		public Type GetControllerType()
//		{
//			return typeof(ODataController);
//		}
//
//		public class ODataController : DomainODataController<TDomain>
//		{
//
//			public Expression<Func<TEntity, TContract>> Selector { get; set; }
//
//			[EnableQuery]
//			public IQueryable<TContract> Get(ODataQueryOptions<TEntity> options)
//			{
//				var set = db.Set<TEntity>();
//
//				return set.Select(Selector);
//			}
//
//			public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
//			{
//				var services = controllerContext.ControllerDescriptor.Configuration.Services;
//				try
//				{
//					// System.Web.Http.Controllers.ApiControllerActionSelector.SelectAction
//					var actionDescriptor = services.GetActionSelector().SelectAction(controllerContext);
//				}
//				catch (Exception ex)
//				{
//					
//				}
//
//				return base.ExecuteAsync(controllerContext, cancellationToken);
//			}
//		}
//
//	}
//
//
//	partial class AppConfig
//	{
//
//		public static void RegisterAnonymousEdmModel(HttpConfiguration config, ODataConventionModelBuilder mb, DomainEdmModel m)
//		{
//
//			config.Services.Replace(typeof(IHttpControllerSelector), 
//				new ODataAnonymousService.ODataControllerSelector(config));
//
//			// http://travel15/odata/Products2?$orderby=IssueDate%20desc,Name%20desc&$count=true
//			var products2 = NewODataService("Products2", (Product a) => new
//			{
//				a.Id,
//				a.IssueDate,
//				a.Type,
//				a.Name,
//				a.Total,
//				a.ServiceFee,
//				a.GrandTotal,
//			});
//
//			products2.Register(config, mb, a => a.Id);
//		}
//
//
//		public static ODataAnonymousService<Domain, TEntity, TContract> NewODataService<TEntity, TContract>(
//			string name,
//			Expression<Func<TEntity, TContract>> selector
//		)
//			where TEntity : Domain.Entity
//			where TContract : class
//		{
//			return ODataAnonymousService.Create<Domain, TEntity, TContract>(name, selector);
//		}
//
//	}
//
//}