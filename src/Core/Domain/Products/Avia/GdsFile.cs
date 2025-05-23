using System;
using System.Collections.Generic;
using System.IO;

using Luxena.Base.Metamodel;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{



	//===g






	[RU("Gds-файл", "Gds-файлы")]
	[AdminPrivileges]
	public abstract partial class GdsFile : Entity3
	{
		
		//---g



		[RU("Тип")]
		public virtual GdsFileType FileType { get; set; }

		[RU("Дата импорта"), DateTime2]
		public virtual DateTime TimeStamp { get; set; }

		[RU("Содержимое"), Hidden(true)]
		[Text(10)]
		public virtual string Content { get; set; }

		[RU("Результат импорта")]
		public virtual ImportResult ImportResult { get; set; }

		[RU("Журнал")]
		public virtual string ImportOutput { get; set; }



		//---g



		protected virtual IList<Entity2> ParseProducts(Domain db)
		{
			throw new NotImplementedException();
		}



		public virtual void Import(Domain db, out string userOutput)
		{

			var documents = ParseProducts(db);

			db.GdsFile.ImportDocuments(this, documents, out userOutput);

		}



		public virtual void AppendOutput(string output)
		{

			output = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {output}";

			if (ImportOutput.No())
				ImportOutput = output;
			else
				ImportOutput = output + "\n\n" + ImportOutput;

		}



		public virtual string SaveToInboxFolder(string path)
		{

			if (Content.No())
				return null;


			if (!path.EndsWith("\\"))
				path += "\\";

			if (FileType == GdsFileType.AirFile)
				path += "air\\";
			else if (FileType == GdsFileType.MirFile)
				path += "mir\\";
			else if (FileType == GdsFileType.SabreFilFile)
				path += "fil\\";
			else if (FileType == GdsFileType.CrazyllamaPnrFile)
				path += "pnr\\";
			else if (FileType == GdsFileType.LuxenaXmlFile)
				path += "luxena\\";
			else if (FileType == GdsFileType.DrctXmlFile)
				path += "drct\\";
			else if (FileType == GdsFileType.AdamAiJsonFile)
				path += "adamai\\";
			else
				return null;


			Directory.CreateDirectory(path);


			path += Name.Replace(':', '-');

			using (var w = System.IO.File.CreateText(path))
			{
				w.Write(Content);
			}

			System.IO.File.SetCreationTime(path, TimeStamp);


			return path;

		}



		//---g

	}






	//===g



}