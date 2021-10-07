using System;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;


namespace Luxena.Travel.Web
{

	using Domain;


	partial class AppConfig
	{

		public static void RegisterOData(HttpConfiguration config)
		{

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			

			var edmModel = GetEdmModel(config);

			var route = config.MapODataServiceRoute("odata", "odata", edmModel);

			config.MapODataServiceRoute("odata.usecalculated", "odata.usecalculated", edmModel);

		}


		public static Microsoft.OData.Edm.IEdmModel GetEdmModel(HttpConfiguration config)
		{
			var mb = new ODataConventionModelBuilder();

			mb.ComplexType<Money>().Do(t =>
			{
				t.Property(a => a.Amount);
				t.Property(a => a.CurrencyId);
			});

			mb.EnumType<ProductType>();

			var m = new DomainEdmModel(mb);


			//mb.Function("Order_TotalByIssueDate")
			//	.Returns<string>();

			//mb.Function("Order_TotalByIssueDate")
			//	.ReturnsCollection<FloatByDate>()
			//	.Parameter<string>("CustomerId");

			var edmModel = mb.GetEdmModel();

			return edmModel;
		}


		static void AddEntityAction<TEntityType>(EntityTypeConfiguration<TEntityType> entityType, string actionName, Action<ActionConfiguration> init)
			where TEntityType : class
		{
			var action = entityType.Action(actionName);

			action.Parameter<string>("_delta").OptionalParameter = true;
			action.Parameter<bool>("_save").OptionalParameter = true;
			action.Parameter<bool>("_resync").OptionalParameter = true;

			init?.Invoke(action);
		}

		static void AddDefaultFunctions<TEntity>(EntityTypeConfiguration<TEntity> entityType) where TEntity : class
		{
			//entityType.Collection.Function("Defaults")
			//	.Returns<string>()
			//	.BindingParameter()
		}

	}


	//public class DomainFunctions1Controller : ODataController
	//{
	//	Domain db = new Domain();

	//	[HttpGet]
	//	[ODataRoute("Order_TotalByIssueDate(CustomerId={CustomerId})")]
	//	public IHttpActionResult Order_TotalByIssueDate([FromODataUri] string CustomerId)
	//	{
	//		var result = db.Order_TotalByIssueDate(CustomerId);
	//		return Ok(result);
	//	}
	//}

}
