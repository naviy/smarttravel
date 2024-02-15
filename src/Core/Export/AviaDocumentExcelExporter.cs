using System;
using System.IO;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Export
{

	
	
	public class AviaDocumentExcelExporter : IAviaDocumentExporter
	{

		public string FullAccessPath { get; set; }

		public string ReadOnlyAccessPath { get; set; }

		public AviaDocumentExcelBuilder Builder { get; set; }



		public void Export(object obj)
		{

			var doc = obj as AviaDocument;

			if (!IsExportAvailable(doc))
				return;


			if (!Directory.Exists(FullAccessPath))
				Directory.CreateDirectory(FullAccessPath);

			if (!Directory.Exists(ReadOnlyAccessPath))
				Directory.CreateDirectory(ReadOnlyAccessPath);

			var bytes = Builder.Make(new Product[] { doc });

			var fileNameFullAccess = $"{FullAccessPath.TrimEnd('\\')}\\Export_{DateTime.Now:yyyyMMdd_HH-mm_ss}_{doc.Number}.xls".Replace(" ", "_");

			var fileNameReadOnly = $"{ReadOnlyAccessPath.TrimEnd('\\')}\\Export_{DateTime.Now:yyyyMMdd_HH-mm_ss}_{doc.Number}.xls".Replace(" ", "_");

			using (var fs = new FileStream(fileNameFullAccess, FileMode.Create, FileAccess.Write))
				fs.Write(bytes, 0, bytes.Length);

			using (var fs = new FileStream(fileNameReadOnly, FileMode.Create, FileAccess.Write))
				fs.Write(bytes, 0, bytes.Length);

			System.IO.File.SetAttributes(fileNameReadOnly, FileAttributes.ReadOnly);

		}


		private bool IsExportAvailable(AviaDocument doc)
		{
			return doc != null && !doc.IsVoid && FullAccessPath.Yes() && ReadOnlyAccessPath.Yes();
		}

	}



}