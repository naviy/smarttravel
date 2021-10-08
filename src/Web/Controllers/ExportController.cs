using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Luxena.Base.Data;
using Luxena.Base.Services;


namespace Luxena.Travel.Web.Controllers
{

	public class ExportController : Controller
	{
		public GenericService GenericService { get; set; }


		[HttpPost]
		public ActionResult Export(string className, string exportParams, string fileName)
		{
			if (string.IsNullOrEmpty(exportParams))
				throw new ArgumentNullException(nameof(exportParams));

			try
			{
				var args = new JavaScriptSerializer().Deserialize<DocumentExportArgs>(exportParams);

				return File(GenericService.Export(className, args), MimeTypes.OctetStream, fileName);
			}
			catch (Exception ex)
			{

				throw;
			}
		}

	}

}
