using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Luxena.Travel.Domain;
using Luxena.Travel.Export;

using File = System.IO.File;


namespace Luxena.Travel.Reports
{
	public class ExportFtp2011
	{
		public string FullAccessPath { get; set; }

		public string ReadOnlyAccessPath { get; set; }

		public string ExportConfigFileFtp { get; set; }


		public bool Export(AviaDocument aviaDocument)
		{
			if (!IsExportAvailable(aviaDocument))
				return false;

			if (!CreateExportConfig())
				return false;

			if (!Directory.Exists(FullAccessPath))
				Directory.CreateDirectory(FullAccessPath);

			if (!Directory.Exists(ReadOnlyAccessPath))
				Directory.CreateDirectory(ReadOnlyAccessPath);

			var exporter = new AviaDocumentExcelExporter2011(_exportConfig);

			var bytes = exporter.Export(new[] { aviaDocument });

			var fileNameFullAccess = string.Format("{0}\\Export_{1}_{2}.xls", FullAccessPath.TrimEnd('\\'),
				DateTime.Now.ToString("yyyyMMdd_HH-mm_ss"),
				aviaDocument.Number).Replace(" ", "_");

			var fileNameReadOnly = string.Format("{0}\\Export_{1}_{2}.xls", ReadOnlyAccessPath.TrimEnd('\\'),
				DateTime.Now.ToString("yyyyMMdd_HH-mm_ss"),
				aviaDocument.Number).Replace(" ", "_");

			using (var fs = new FileStream(fileNameFullAccess, FileMode.Create, FileAccess.Write))
				fs.Write(bytes, 0, bytes.Length);

			if (File.Exists(fileNameReadOnly))
				File.SetAttributes(fileNameReadOnly, FileAttributes.Normal);

			using (var fs = new FileStream(fileNameReadOnly, FileMode.Create, FileAccess.Write))
				fs.Write(bytes, 0, bytes.Length);

			File.SetAttributes(fileNameReadOnly, FileAttributes.ReadOnly);

			return true;
		}

		private bool IsExportAvailable(AviaDocument aviaDocument)
		{
			return (!string.IsNullOrEmpty(ExportConfigFileFtp) && aviaDocument != null && !aviaDocument.IsVoid &&
				!string.IsNullOrEmpty(FullAccessPath) && !string.IsNullOrEmpty(ReadOnlyAccessPath));
		}

		private bool CreateExportConfig()
		{
			var xmlSerializer = new XmlSerializer(typeof(ExportStructure));

			using (var reader = XmlReader.Create(ExportConfigFileFtp, new XmlReaderSettings()))
			{
				_exportConfig = (ExportStructure)xmlSerializer.Deserialize(reader);
			}

			return _exportConfig != null;
		}


		private ExportStructure _exportConfig;
	}

}