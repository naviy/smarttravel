using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

using Common.Logging;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Serialization;
using Luxena.Travel.Export;
using Luxena.Travel.Parsers;
using Luxena.Travel.Services;

using Exception = System.Exception;
using StringBuilder = System.Text.StringBuilder;


namespace Luxena.Travel.Domain
{

	partial class GdsFile
	{

		public abstract class Service<TGdsFile> : Entity2Service<TGdsFile>
			where TGdsFile : GdsFile
		{

		}


		public partial class Service : Service<GdsFile>
		{

			public OperationStatus CanReimport()
			{
				return db.Granted(UserRole.Administrator);
			}

			public void AddFile(GdsFile file = null)
			{
				string output;

				AddFile(file, out output);
			}

			public void AddFile(GdsFile file, out string userOutput)
			{
				db.Commit(() =>
				{
					try
					{
						file.Content = file.Content.As(a => a.Replace((char)0, ' '));
						Save(file);
					}
					catch (Exception ex)
					{
						throw new GdsFileSaveException($"Can't save Gds file {file.Name}", ex);
					}
				});

				string userOutput_;

				Import(file, out userOutput_);

				userOutput = userOutput_;
			}


			public object Reimport(object[] ids, RangeRequest prms)
			{
				if (!CanReimport())
					throw new SecurityException("Reimport operation is not permitted");

				var data = new ArrayList();
				object firstId = null;

				foreach (var id_ in ids)
				{
					var id = id_;
					var file = By(id);

					if (file.FileType == GdsFileType.PrintFile)
					{
						var printFile = (PrintFile)file;
						if (!printFile.ExtractPrintZip()) continue;

						Import(file);

						Directory.Delete(printFile.FilePath.Replace(".zip", null), true);
					}
					else
					{
						Import(file);
					}

					if (firstId == null)
						firstId = file.Id;

					data.Add(new ObjectSerializer().Serialize(file));
				}

				object result;

				if (prms != null)
				{
					prms.PositionableObjectId = firstId;

					result = new object[]
				{
					data,
					db.GetRange<GdsFile>(prms)
				};
				}
				else
				{
					result = data[0];
				}

				return result;
			}

			private void Import(GdsFile file)
			{
				string output;

				Import(file, out output);
			}


			private static readonly Regex _reImportDelete = new Regex(@"^DELETE (.+)$", RegexOptions.Compiled);



			private void Import(GdsFile file, out string userOutput_)
			{

				string userOutput = null;

				try
				{

					var deleteMath = _reImportDelete.Match(file.Content);

					if (deleteMath.Yes())
					{

						try
						{
							var number = deleteMath.Groups[1].Value;
							db.Commit(() => 
								db.Session.Delete($"from AviaTicket a where a.Number = '{number}'")
							);

							//var delDoc = db.AviaDocument.ByNumber(deleteMath.Groups[1].Value.As().Longn);
							//if (delDoc != null)
							//db.Delete(delDoc);
						}
						catch (Exception ex)
						{
							if (!ex.Message.Contains("orderitem_product_fkey"))
								throw;
						}

					}

					else
					{
						db.Commit(() => file.Import(db, out userOutput));
					}
					
				}
				catch (Exception ex)
				{

					_log.Warn(ex.Message, ex);
					_log.Warn(ex.Source);
					_log.Warn(ex.StackTrace);

					if (ex is DomainException || ex is GdsImportException)
					{
						file.ImportResult = ImportResult.Warn;
						file.AppendOutput(ex.Message);
						userOutput = ex.Message;
					}
					else
					{
						file.ImportResult = ImportResult.Error;
						file.AppendOutput(ex.ToString());

						userOutput = Exceptions.ImportGdsFile_UnexpectedError;
					}

					db.Try(() => db.Commit(() => Save(file)));

				}


				userOutput_ = userOutput;

			}



			public void ImportDocuments(GdsFile file, IList<Entity2> documents, out string userOutput)
			{

				foreach (var product in documents.OfType<Product>())
				{
					product.OriginalDocument = file;
				}

				var importedDocuments = new Dictionary<Entity2, ImportStatus>();

				db.AviaDocument.Import(documents, importedDocuments);

				ImportResult result;

				file.AppendOutput(GetImportOutput(documents, importedDocuments, out result, out userOutput));
				file.ImportResult = result;

				db.Resolve<GdsFileExporter>()?.Export(file, importedDocuments.Keys.ToList());

			}


			public string GetImportOutput(IEnumerable<Entity2> documents, Dictionary<Entity2, ImportStatus> importedDocuments, out ImportResult result, out string userOutput)
			{
				result = ImportResult.None;

				var importLog = new StringBuilder();
				var userLog = new StringBuilder();

				foreach (var document in documents)
				{
					var importStatus = importedDocuments[document];

					result = GetImportResult(result, importStatus.Result);

					var product = document as Product;
					var voiding = document as AviaDocumentVoiding;

					string type;

					if (product != null)
					{
						type = product.Type.ToDisplayString();

						if (!product.Name.Yes())
							type = DomainRes.Common_Reservation;
					}
					else
						type = "Void";

					switch (importStatus.Result)
					{
						case ImportResult.Success:

							var str = string.Format(CommonRes.ImportGdsFiles_DocumentImported, type, product ?? voiding.As(a => a.Document));

							userLog.AppendLine(str);

							importLog.AppendLine(str);

							break;

						case ImportResult.Warn:

							userLog.AppendLine(importStatus.ToString());

							importLog.AppendLine(importStatus.ToString());

							break;

						case ImportResult.Error:

							userLog.AppendLine(Exceptions.ImportGdsFile_UnexpectedError);

							importLog.AppendLine(importStatus.ToString());

							break;
					}
				}

				userOutput = userLog.ToString();

				return importLog.ToString();
			}

			private static ImportResult GetImportResult(ImportResult res1, ImportResult res2)
			{
				if (res1 == ImportResult.Error || res2 == ImportResult.Error)
					return ImportResult.Error;

				if (res1 == ImportResult.Warn || res2 == ImportResult.Warn)
					return ImportResult.Warn;

				if (res1 == ImportResult.Success || res2 == ImportResult.Success)
					return ImportResult.Success;

				return ImportResult.None;
			}


			private static readonly ILog _log = LogManager.GetLogger(typeof(Service));
		}

	}

}