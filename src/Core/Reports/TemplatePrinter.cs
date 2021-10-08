using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Web;

using Luxena.Travel.Domain;



namespace Luxena.Travel.Reports
{

	public class TemplatePrinter
	{
		public string TemplateFileName { get; set; }


		protected Stream Build<T>(T data) where T : class
		{
			return Build(TemplateFileName, data);
		}

		protected Stream Build<T>(string templateFileName, T data) where T : class
		{
			templateFileName = HttpContext.Current.Server.MapPath(templateFileName);

			var reportStream = new MemoryStream();

			using (var templateStream = System.IO.File.OpenRead(templateFileName))
			{
				templateStream.CopyTo(reportStream);
			}

			var extension = Path.GetExtension(templateFileName);
			using (var document = NGS.Templater.Configuration.Factory.Open(reportStream, extension))
			{
				document.Process(data);
			}

			if (extension == ".xlsx")
				RemoveExcelUnlicensedInfo(reportStream);
			else if (extension == ".docx")
				RemoveWordUnlicensedInfo(reportStream);

			return reportStream;
		}

		protected static void RemoveExcelUnlicensedInfo(Stream reportStream)
		{
			// Выгрызаем страницу с Unliсensed version
			using (var archive = new ZipArchive(reportStream, ZipArchiveMode.Update, true))
			{
				var workbook = archive.GetEntry(@"xl/workbook.xml");

				string text;
				using (var reader = new StreamReader(workbook.Open()))
				{
					text = reader.ReadToEnd();
				}

				text = reUnlicensedSheet.Replace(text, "");

				workbook.Delete();

				workbook = archive.CreateEntry(@"xl/workbook.xml");
				using (var writer = new StreamWriter(workbook.Open()))
				{
					writer.Write(text);
				}
			}
		}
		private static readonly Regex reUnlicensedSheet = new Regex(@"\<sheet name=""Unlicensed version"".*?/\>", RegexOptions.Compiled);


		protected static void RemoveWordUnlicensedInfo(Stream reportStream)
		{
			// Выгрызаем параграф с Unliсensed version
			using (var archive = new ZipArchive(reportStream, ZipArchiveMode.Update, true))
			{
				var document = archive.GetEntry(@"word/document.xml");

				string text;
				using (var reader = new StreamReader(document.Open()))
				{
					text = reader.ReadToEnd();
				}

				text = reUnlicensedParagraph.Replace(text, "");

				document.Delete();

				document = archive.CreateEntry(@"word/document.xml");
				using (var writer = new StreamWriter(document.Open()))
				{
					writer.Write(text);
				}
			}
		}
		private static readonly Regex reUnlicensedParagraph = new Regex(@"<w:p.*?Unlicensed version.*?</w:p>", RegexOptions.Compiled);



		public class TotalSum
		{
			public string Title { get; set; }
			public decimal Amount { get; set; }
		}


		public class TotalSums : List<TotalSum>
		{
			public TotalSums Add(decimal amount, string title, bool skipIfEmpty = false)
			{
				if (!skipIfEmpty || amount != 0)
					Add(new TotalSum { Title = title, Amount = amount });

				return this;
			}

			public TotalSums Add(Money money, string title, bool skipIfEmpty = false)
			{
				return Add(money.AsAmount(), title, skipIfEmpty);
			}
		}

	}

}
