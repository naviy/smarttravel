using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using Luxena.Base.Domain;
using Luxena.Travel.Domain;
using Luxena.Travel.Export;
using Luxena.Travel.Parsers;
using Luxena.Travel.Reports;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Reports
{
	[TestFixture]
	public class DocumentExportTests
	{
		private static IList<Product> GetDocuments()
		{
			var entities = new List<Entity2>();

			var defaultCurrency = new Currency("UAH");

			entities.AddRange(AirParser.Parse(Res.AirTicket1, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket2, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket3, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket4, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket5, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket6, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket7, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket8, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket9, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket10, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket11, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket12, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket13, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirTicket14, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirRefund1, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirRefund2, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirReservation1, defaultCurrency));
			entities.AddRange(AirParser.Parse(Res.AirMco1, defaultCurrency));

			return entities.OfType<Product>().ToList();
		}

		[Test]
		public void CreateDefaultExportConfig()
		{
			var structure = new ExportStructure(
				new[]
				{
					new ExportField
					{
						Caption = DomainRes.Common_IssueDate,
						PropertyName = "IssueDate",
						ExcelFormat = "DD.MM.YYYY",
						Width = 12
					},
					new ExportField
					{
						Caption = ReportRes.Common_Booker,
						PropertyName =
							"Booker",
						Width = 25
					},
					new ExportField
					{
						Caption = ReportRes.Common_Ticketer,
						PropertyName =
							"Ticketer",
						Width = 25
					},
					new ExportField
					{
						Caption = DomainRes.AviaDocument_TicketerCode,
						ChildFields = new[]
						{
							new ExportField
							{
								PropertyName =
									"TicketerOffice",
								Width = 12
							},
							new ExportField
							{
								PropertyName =
									"TicketerCode",
								Width = 10
							}
						}
					},
					new ExportField
					{
						Caption = DomainRes.Common_Type,
						PropertyName = "Type",
						Width = 8
					},
					new ExportField
					{
						Caption = DomainRes.AviaDocument_AirlinePrefixCode,
						PropertyName =
							"AirlinePrefixCodeString",
						ExcelFormat = "000",
						Width = 8
					},
					new ExportField
					{
						Caption = DomainRes.Common_Number,
						PropertyName =
							"Number",
						Width = 12
					},
					new ExportField
					{
						Caption = DomainRes.Common_PassengerName,
						PropertyName =
							"PassengerName",
						Width = 40
					},
					new ExportField
					{
						Caption = DomainRes.AviaDocument_Itinerary,
						PropertyName =
							"Itinerary",
						Width = 40
					},
					new ExportField
					{
						Caption = DomainRes.FlightSegment_DepartureTime,
						PropertyName =
							"Departure",
						Width = 16,
						ExcelFormat = "dd.MM.yyyy hh:mm"
					},
					new ExportField
					{
						Caption = DomainRes.AviaTicket_SegmentClasses,
						PropertyName =
							"SegmentClasses",
						Width = 15
					},
					new ExportField
					{
						Caption = DomainRes.AviaDocument_TourCode,
						PropertyName =
							"TourCode",
						Width = 15
					},
					new ExportField
					{
						Caption = DomainRes.Common_Fare,
						PropertyName = "Fare"
					},
					new ExportField
					{
						Caption = DomainRes.Common_EqualFare,
						PropertyName =
							"EqualFare"
					},
					new ExportField
					{
						Caption = DomainRes.Common_CancelFee,
						PropertyName =
							"CancelFee"
					},
					new ExportField
					{
						Caption = DomainRes.Common_FeesTotal,
						PropertyName =
							"FeesTotal"
					},
					new ExportField
					{
						Caption = DomainRes.Product_Total,
						PropertyName = "Total"
					},
					new ExportField
					{
						Caption = DomainRes.Common_Vat,
						PropertyName = "Vat"
					},
					new ExportField
					{
						Caption = DomainRes.Common_Commission,
						ChildFields = new[]
						{
							new ExportField
							{
								Caption = DomainRes.Common_Percent,
								PropertyName =
									"CommissionPercent",
								Width = 9,
								ExcelFormat = "#%"
							},
							new ExportField
							{
								Caption = DomainRes.Common_Amount,
								PropertyName = "Commission",
							}
						}
					},
					new ExportField
					{
						Caption = DomainRes.AviaRefund_CancelCommission,
						ChildFields = new[]
						{
							new ExportField
							{
								Caption = DomainRes.Common_Percent,
								PropertyName = "CancelCommissionPercent",
								Width = 9,
								ExcelFormat = "#%"
							},
							new ExportField
							{
								Caption = DomainRes.Common_Amount,
								PropertyName = "CancelCommission",
							}
						}
					},
					new ExportField
					{
						Caption = DomainRes.Product_TotalToBeTransfered,
						PropertyName = "TotalToTransfer"
					},
					new ExportField
					{
						Caption = DomainRes.Common_ServiceFee,
						PropertyName = "ServiceFee"
					},
					new ExportField
					{
						Caption = DomainRes.Common_Discount,
						PropertyName = "Discount"
					},
					new ExportField
					{
						Caption = DomainRes.Product_GrandTotal,
						PropertyName = "GrandTotal"
					},
					new ExportField
					{
						Caption = DomainRes.PaymentType,
						PropertyName = "PaymentType",
						Width = 9
					},
					new ExportField
					{
						Caption = DomainRes.Common_Customer,
						PropertyName = "Customer",
						Width = 40
					},
					new ExportField
					{
						Caption = DomainRes.Common_Intermediary,
						PropertyName = "Intermediary",
						Width = 40
					},
					new ExportField
					{
						Caption = ReportRes.AviaDocumentExporter_CustomerId,
						PropertyName = "Customer.Id",
						Width = 35
					},
					new ExportField
					{
						Caption = ReportRes.AviaDocumentExporter_IntermediaryId,
						PropertyName = "Intermediary.Id",
						Width = 35
					},
					new ExportField
					{
						Caption = DomainRes.Common_Note,
						PropertyName = "Note",
						Width = 40
					},
					new ExportField
					{
						Caption = DomainRes.Common_Seller,
						PropertyName = "Seller",
						Width = 40
					},
					new ExportField
					{
						Caption = DomainRes.Common_Owner,
						PropertyName = "Owner",
						Width = 40
					},
					new ExportField
					{
						Caption = DomainRes.Organization_AirlineIataCode,
						PropertyName = "Airline.AirlineIataCode",
						Width = 8
					},
					new ExportField
					{
						Caption = DomainRes.Order,
						PropertyName = "Invoice.Number",
						Width = 15
					},
					new ExportField
					{
						Caption = DomainRes.Order_IssueDate,
						PropertyName = "Invoice.IssueDate",
						ExcelFormat = "DD.MM.YYYY",
						Width = 12
					},
					/*new ExportField
					{
						Caption = DomainRes.Order_PaymentTimestamp,
						PropertyName = "Invoice.PaymentTimestamp",
						ExcelFormat = "DD.MM.YYYY",
						Width = 12
					},*/
					new ExportField
					{
						PropertyName = "Remarks",
						Width = 30
					},
					new ExportField
					{
						Caption = DomainRes.Common_Handling,
						PropertyName = "Handling"
					}
				})
			{
				DisplayCurrency = true
			};

			var xmlSerializer = new XmlSerializer(structure.GetType());


			using (var writer = XmlWriter.Create("~/export.xml".ResolvePath(), new XmlWriterSettings()))
			{
				xmlSerializer.Serialize(writer, structure);
			}

			using (var reader = XmlReader.Create("~/export.xml".ResolvePath(), new XmlReaderSettings()))
			{
				xmlSerializer.Deserialize(reader);
			}
		}

		[Test]
		public void CreateMersiExportConfig()
		{
			var structure = new ExportStructure(new[]
			{
				new ExportField
				{
					Caption = DomainRes.Common_IssueDate,
					PropertyName = "IssueDate",
					ExcelFormat = "DD.MM.YYYY",
					Width = 12
				},
				new ExportField
				{
					Caption = DomainRes.Common_Type,
					PropertyName = "Type",
					Width = 8
				},
				new ExportField
				{
					Caption = DomainRes.AviaDocument_AirlinePrefixCode,
					PropertyName =
						"AirlinePrefixCodeString",
					ExcelFormat = "000",
					Width = 8
				},
				new ExportField
				{
					Caption = DomainRes.Common_Number,
					PropertyName =
						"Number",
					Width = 12
				},
				new ExportField
				{
					Caption = DomainRes.Common_PassengerName,
					PropertyName =
						"PassengerName",
					Width = 40
				},
				new ExportField
				{
					Caption = DomainRes.AviaDocument_Itinerary,
					PropertyName =
						"Itinerary",
					Width = 40
				},
				new ExportField
				{
					Caption = DomainRes.FlightSegment_DepartureTime,
					PropertyName =
						"Departure",
					Width = 16,
					ExcelFormat = "dd.MM.yyyy hh:mm"
				},
				new ExportField
				{
					Caption = DomainRes.AviaTicket_SegmentClasses,
					PropertyName =
						"SegmentClasses",
					Width = 15
				},
				new ExportField
				{
					Caption = "Расчетная валюта",
					ValueConst = "Local",
					Width = 15
				},
				new ExportField
				{
					Caption = DomainRes.Common_EqualFare,
					PropertyName = "EqualFare"
				},
				new ExportField
				{
					Caption = DomainRes.Common_FeesTotal,
					PropertyName = "FeesTotal",
					Formula = "FeesTotal-Vat-CancelFee"
				},
				new ExportField
				{
					Caption = DomainRes.Common_Vat,
					PropertyName = "Vat"
				},
				new ExportField
				{
					Caption = DomainRes.Product_Total,
					PropertyName = "Total"
				},
				new ExportField
				{
					Caption = DomainRes.Common_ServiceFee,
					PropertyName = "ServiceFee"
				},
				new ExportField
				{
					Caption = DomainRes.AviaDocument_ExtraCharge,
					PropertyName = "ExtraCharge"
				},
				new ExportField
				{
					Caption = DomainRes.Product_GrandTotal,
					PropertyName = "GrandTotal"
				},
				new ExportField
				{
					Caption = DomainRes.PaymentType,
					PropertyName = "PaymentType",
					Width = 9
				},
				new ExportField
				{
					Caption = DomainRes.Common_Customer,
					PropertyName = "Customer",
					Width = 40
				},
				new ExportField
				{
					Caption = ReportRes.AviaDocumentExporter_CustomerId,
					PropertyName = "Customer.Id",
					Width = 35
				}
			})
			{
				DocumentTypeMapping = new SerializableDictionary<string, string>
				{
					{ "Ticket", "TKTB" }, { "Refund", "RFND" }, { "Mco", "MCO" }, { "Void", "VOID" }
				},
				DisplayCurrency = false,
				DefaultCurrency = new Currency("UAH")
			};

			var xmlSerializer = new XmlSerializer(structure.GetType());

			using (var writer = XmlWriter.Create("~/mersiexport.xml".ResolvePath(), new XmlWriterSettings()))
			{
				xmlSerializer.Serialize(writer, structure);
			}

			using (var reader = XmlReader.Create("~/mersiexport.xml".ResolvePath(), new XmlReaderSettings()))
			{
				xmlSerializer.Deserialize(reader);
			}
		}

		[Test]
		public void TestExport4()
		{
			var exporter = new AviaDocumentExcelBuilder("~/../../../../../conf/export.config");

			using (var stream = new FileStream("~/export4.xls".ResolvePath(), FileMode.Create))
			{
				var bytes = exporter.Make(GetDocuments());

				stream.Write(bytes, 0, bytes.Length);
			}
		}

		[Test]
		public void TestExport5()
		{
			var exporter = new AviaDocumentExcelBuilder("~/../../../../../profiles/merci/conf/export.config");

			using (var stream = new FileStream("~/export5_mersi.xls".ResolvePath(), FileMode.Create))
			{
				var bytes = exporter.Make(GetDocuments());

				stream.Write(bytes, 0, bytes.Length);
			}
		}
	}
}