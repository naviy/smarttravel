//using System;
//using System.IO;
//using System.Text;

//using Common.Logging;

//using Luxena.Travel.Domain;
//using Luxena.Travel.Parsers;


//namespace Luxena.Travel.Services
//{
//	public class PaymentXmlImportTask : ITask
//	{
//		public Domain.Domain db { get; set; }

//		public string ImportPath { get; set; }

//		public string ArchivePath { get; set; }


//		public void Execute()
//		{
//			foreach (var fileName in Directory.EnumerateFiles(ImportPath))
//			{
//				try
//				{
//					_log.Info("Importing " + fileName);

//					Payment payment;

//					using (var reader = new StreamReader(fileName, Encoding.GetEncoding(1251)))
//						payment = PaymentXmlParser.ReadFrom(db, reader);

//					db.Commit(() => db.Payment.Import(payment));

//					_log.Info(string.Format("Payment {0} imported", payment.Number));
//				}
//				catch (Exception ex)
//				{
//					_log.Error("Import failed", ex);
//				}
//				finally
//				{
//					var moveTo = Path.Combine(ArchivePath, string.Format("{0:yyyy-MM-dd_HH-mm-ss}_{1}", DateTime.Now, Path.GetFileName(fileName)));

//					System.IO.File.Move(fileName, moveTo);
//				}
//			}
//		}

//		private static readonly ILog _log = LogManager.GetLogger(typeof (PaymentXmlImportTask));
//	}
//}