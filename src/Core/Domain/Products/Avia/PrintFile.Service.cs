using System.Collections.Generic;

using Luxena.Base;
using Luxena.Base.Domain;
using Luxena.Travel.Parsers;


namespace Luxena.Travel.Domain
{

	partial class PrintFile
	{

		public new class Service : Service<PrintFile>
		{

		}


		public override void Import(Domain db, out string userOutput)
		{

			var path = FilePath.Replace(".zip", null);

			var documents = new PrintParser { NeutralAirlineCode = db.Configuration.NeutralAirlineCode }
				.Parse(path);

			var list = new List<Entity2>();

			foreach (var document in documents)
			{
				document.OriginalDocument = this;

				document.Owner = db.Party.ByName(Office);

				var user = db.User.By(UserName);

				document.Ticketer = user != null ? user.Person : null;
				document.Booker = document.Ticketer;
				document.Seller = document.Ticketer;

				list.Add(document);
			}

			var importedDocuments = new Dictionary<Entity2, ImportStatus>();

			db.AviaDocument.Import(list, importedDocuments);

			ImportResult result;

			AppendOutput(db.GdsFile.GetImportOutput(list, importedDocuments, out result, out userOutput));
			ImportResult = result;
		}


		public virtual bool ExtractPrintZip()
		{
			if (!System.IO.File.Exists(FilePath))
			{
				AppendOutput(string.Format("File {0} doesn't exist", FilePath));
				ImportResult = ImportResult.Warn;

				return false;
			}

			var path = FilePath.Replace(".zip", null);

			new ZipExtractor(FilePath).ExtractTo(path);

			return true;
		}

	}

}