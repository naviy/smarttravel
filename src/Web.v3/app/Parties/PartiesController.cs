using System.Linq;
using System.Web.Http;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Controllers
{


	public class PartiesController : EntityApiController<Party, Party.Service>
	{

		[HttpGet]
		public object Suggest(string name)
		{
			return Suggest(q => 
				from r in q
				where r.NameForDocuments.Contains(name)
				orderby r.NameForDocuments
				select r.ToReference()
			);
		}
	}

}