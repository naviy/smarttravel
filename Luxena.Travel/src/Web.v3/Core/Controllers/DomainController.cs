using System.Web.Mvc;

using Luxena.Domain.Entities;


namespace Luxena.Domain.Web
{

	public abstract class DomainController<TDomain> : Controller
		where TDomain : Domain<TDomain>
	{

		public virtual TDomain Domain { get; set; }

//		public ActionResult EmbeddedScript(string file)
//		{
//			var assembly = Assembly.GetExecutingAssembly();
//			var manifestResourceName = string.Format("{0}.Web.Scripts.{1}.js",
//				assembly.GetName().Name, file
//			);
//
//			var stream = assembly.GetManifestResourceStream(manifestResourceName);
//			if (stream == null)
//				throw new ArgumentException("Embedded Resource " + manifestResourceName + " not found.");
//
//			var reader = new StreamReader(stream);
//			var content = reader.ReadToEnd();
//
//			return JavaScript(content);
//		}

	}

}