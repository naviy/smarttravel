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

			var bytes = Builder.Make(new[] { doc });

			var fileNameFullAccess = string.Format("{0}\\Export_{1}_{2}.xls", FullAccessPath.TrimEnd('\\'),
				DateTime.Now.ToString("yyyyMMdd_HH-mm_ss"),
				doc.Number).Replace(" ", "_");

			var fileNameReadOnly = string.Format("{0}\\Export_{1}_{2}.xls", ReadOnlyAccessPath.TrimEnd('\\'),
				DateTime.Now.ToString("yyyyMMdd_HH-mm_ss"),
				doc.Number).Replace(" ", "_");

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