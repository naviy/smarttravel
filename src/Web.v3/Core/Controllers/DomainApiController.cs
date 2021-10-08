using System.Net;
using System.Net.Http;
using System.Web.Http;

using Luxena.Domain.Entities;


namespace Luxena.Domain.Web
{

	public abstract class DomainApiController<TDomain> : ApiController
		where TDomain : Domain<TDomain>
	{
		public virtual TDomain Domain { get; set; }

		//protected TDomain db { get { return Domain; } } 


		protected HttpResponseMessage Result<T>(T data) where T: class
		{
			return data == null ? null : Request.CreateResponse(HttpStatusCode.OK, data, "application/json");
		}

	}

}