using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{
	[WebService(Namespace = "http://travel.luxena.com/services/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	[ScriptService]
	public class BaseWebService<T>
	{
		public BaseWebService()
		{
			Service = ServiceResolver.Current.Resolve<T>();
			//ServiceResolver.Current.Resolve(this);
		}

		public T Service { get; set; }
	}

	[WebService(Namespace = "http://travel.luxena.com/services/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	[ScriptService]
	public class DomainWebService : WebService
	{
		public DomainWebService()
		{
			ServiceResolver.Current.Resolve(this);
			dc = new Contracts { Domain = db };
		}

		public Domain.Domain Domain { get { return db; } set { db = value; } }
		protected Domain.Domain db;

		public Contracts Contracts { get { return dc; } set { dc = value; } }
		protected Contracts dc;
	}

}