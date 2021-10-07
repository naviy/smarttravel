//using System.Web.Http;
//using System.Web.OData;


//namespace Luxena.Travel.Web
//{

//	partial class CashInOrderPaymentsController
//	{

//		[HttpPost]
//		public IHttpActionResult GetNote([FromODataUri] string key, ODataActionParameters parameters)
//		{
//			return ExecEntityAction(key, parameters, r => 
//				r.Note
//			);
//		}


//		[HttpPost, EnableQuery]
//		public IHttpActionResult Void([FromODataUri] string key, ODataActionParameters parameters)
//		{
//			return ExecEntityAction(key, parameters, r => 
//				r.Void((bool?)parameters.By("b1"))
//			);
//		}

//		[HttpPost, EnableQuery]
//		public IHttpActionResult Unvoid([FromODataUri] string key, ODataActionParameters parameters)
//		{
//			return ExecEntityAction(key, parameters, r =>
//				r.Unvoid()
//			);
//		}
//	}

//}