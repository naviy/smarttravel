using System;
using System.Collections.Generic;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Gds-файл", "Gds-файлы")]
	[AdminPrivileges]
	public abstract partial class GdsFile : Entity3
	{

		[RU("Тип")]
		public virtual GdsFileType FileType { get; set; }

		[RU("Дата импорта"), EntityDate, DateTime2]
		public DateTimeOffset TimeStamp { get; set; }

		[RU("Содержимое"), CodeText]
		public string Content { get; set; }

		[RU("Результат импорта")]
		public ImportResult ImportResult { get; set; }

		[RU("Журнал")]
		public string ImportOutput { get; set; }

		public virtual ICollection<Product> Products { get; set; }


		protected virtual IList<Entity2> ParseProducts()
		{
			throw new NotImplementedException();
		}

		//		public virtual void Import(Domain db, out string userOutput)
		//		{
		//			var documents = ParseProducts(db);
		//			db.GdsFile.ImportDocuments(this, documents, out userOutput);
		//		}

		public void AppendOutput(string output)
		{
			output = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {output}";

			if (ImportOutput.No())
				ImportOutput = output;
			else
				ImportOutput = output + "\r\n\r\n" + ImportOutput;
		}

	}


	partial class Domain
	{
		public DbSet<GdsFile> GdsFiles { get; set; }
	}


	public partial class AirFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.AirFile;

		//protected override IList<Entity2> ParseProducts()
		//{
		//	return AirParser.Parse(Content, db.Configuration.AmadeusRizUsingMode, db.Configuration.DefaultCurrency);
		//}

	}


	public partial class AmadeusXmlFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.AmadeusXmlFile;

		//protected override IList<Entity2> ParseProducts()
		//{
		//	var products =
		//		AmadeusXmlParser.Parse(Content, db.Configuration.DefaultCurrency, new string[0])
		//		.ToList();
		//	return products;
		//}

	}


	public partial class GalileoXmlFile : GdsFile
	{

		public override GdsFileType FileType => GdsFileType.GalileoXmlFile;

		//[NotMapped]
		//public XElement Xml { get; set; }


		//protected override IList<Entity2> ParseProducts()
		//{
		//	return GalileoXmlParser.Parse(
		//		Content,
		//		db.Configuration.DefaultCurrency,
		//		GalileoWebServiceTask.GlobalRobots.AsSplit(',').Clip()
		//	).ToArray();
		//}

	}


	public partial class MirFile : GdsFile
	{
		public override GdsFileType FileType => GdsFileType.MirFile;
	}

}