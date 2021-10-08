using System.Collections.Generic;

using Luxena.Base.Domain;
using Luxena.Travel.Parsers;


namespace Luxena.Travel.Domain
{

	partial class TktFile
	{
		public new class Service : Service<TktFile>
		{


		}

		public override void Import(Domain db, out string userOutput)
		{
			var document = TktParser.Parse(Content);

			var aviaDocument = document as AviaDocument;

			if (aviaDocument != null)
			{
				aviaDocument.OriginalDocument = this;
				aviaDocument.BookerOffice = OfficeCode;
				aviaDocument.TicketerOffice = OfficeCode;
				aviaDocument.TicketingIataOffice = OfficeIata;
			}
			else
			{
				var voiding = (AviaDocumentVoiding)document;

				voiding.AgentOffice = OfficeCode;
				voiding.IataOffice = OfficeIata;
			}

			document += db;
			db.Save(document);

			var importResult = new Dictionary<Entity2, ImportStatus>(1);
			importResult[document] = new ImportStatus(ImportResult.Success, "Ok");

			ImportResult result;

			AppendOutput(db.GdsFile.GetImportOutput(new[] { document }, importResult, out result, out userOutput));
			ImportResult = result;
		}

	}

}