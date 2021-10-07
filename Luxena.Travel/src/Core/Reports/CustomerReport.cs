using System.Collections.Generic;
using System.IO;
using System.Linq;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public class CustomerReport
	{
		public CustomerReport(CustomerReportParams reportParams, IList<Product> products)
		{
			_params = reportParams;

			_products = products ?? new List<Product>();
		}

		public Currency DefaultCurrency { get; set; }

		public bool UseDefaultCurrencyForInput { get; set; }

		public void Build(Stream stream)
		{
			AggregateData();

			CreatePdfDocument(stream);

			_pdfDocument.Open();

			GenerateReportHeader();

			GenerateReportContent();

			_pdfDocument.Close();
		}

		private void CreatePdfDocument(Stream stream)
		{
			_pdfDocument = new Document(PageSize.A4, 20, 20, 20, 30);

			var writer = PdfWriter.GetInstance(_pdfDocument, stream);

			var font = new Font(_defaultFont)
			{
				Size = 7
			};

			writer.PageEvent = new PdfPageNumberHelper
			{
				DisplayTotalPageCount = true,
				Font = font,
				PageNumberTemplate = ReportRes.Common_PageNumberTemplate
			};
		}

		public class TypeProductData 
		{
			public ProductType Type { get; set; }
			public int Sign { get; set; }
			public List<Product> Products { get; set; }
			public decimal Total { get; set; }
			public decimal ExtraCharge { get; set; }
			public decimal GrandTotal { get; set; }
		}

		public class ClientTypeProductData 
		{
			public Party Client { get; set; }
			public List<TypeProductData> TypeProducts { get; set; }
			public int Count { get; set; }
			public decimal Total { get; set; }
			public decimal ExtraCharge { get; set; }
			public decimal GrandTotal { get; set; }
		}

		private void AggregateData()
		{
			_clienTypeProducts = (from product in _products
				let client = _params.Customer != null ? product.Customer : product.Order.As(a => a.BillTo)
				//where client != null
				group product by client into clienProducts
				let ctp = new
				{
					Client = clienProducts.Key,
					TypeProducts = (
						from product2 in clienProducts
						group product2 by product2.Type into clienTypeProducts
						let sign = Product.TypeIsRefund(clienTypeProducts.Key) ? -1 : 1
						select new TypeProductData
						{
							Type = clienTypeProducts.Key, 
							Sign = sign, 
							Products = clienTypeProducts.ToList(),
							Total = clienTypeProducts.Sum(a => GetDefaultAmount(a.Total) * sign),
							ExtraCharge = clienTypeProducts.Sum(a => GetDefaultAmount(a.ExtraCharge) * sign),
							GrandTotal = clienTypeProducts.Sum(a => GetDefaultAmount(a.GrandTotal) * sign)
						}
					).ToList()
				}
				select new ClientTypeProductData
				{
					Client = ctp.Client, 
					TypeProducts = ctp.TypeProducts, 
					Count = ctp.TypeProducts.Sum(a => a.Products.Count),
					Total = ctp.TypeProducts.Sum(a => a.Total), 
					ExtraCharge = ctp.TypeProducts.Sum(a => a.ExtraCharge), 
					GrandTotal = ctp.TypeProducts.Sum(a => a.GrandTotal),
				}
			).ToList();

			//_totalTotal = _clienTypeProducts.Sum(a => a.Total);
			//_extraChargeTotal = _clienTypeProducts.Sum(a => a.ExtraCharge);
			//_grandTotalTotal = _clienTypeProducts.Sum(a => a.GrandTotal);


			_departments = _clienTypeProducts.Select(a => a.Client).Where(a => !Equals(a.Id, _params.Client.Id)).ToList();

		}

		private void AddSum(ref decimal total, Money money)
		{
			if (money == null || !money.Currency.Equals(DefaultCurrency))
				return;

			total += money.Amount;
		}

		private decimal GetDefaultAmount(Money money)
		{
			return money != null && money.Currency.Equals(DefaultCurrency) ? money.Amount : 0;
		}


		private void GenerateReportHeader()
		{
			var font = new Font(_defaultFont) { Size = 20 };

			var paragraph = new Paragraph(
				string.Format(ReportRes.CustomerInternalReport_Title, _params.Client.NameForDocuments), 
				font
			) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 5 };

			_pdfDocument.Add(paragraph);

			if (_params.Passenger.Yes())
				AddFilterString(ReportRes.Common_Passenger.ToLower(), _params.Passenger);

			if (_params.Airline != null)
				AddFilterString(DomainRes.Airline.ToLower(), _params.Airline.ToString());

			if (_params.PaymentType.HasValue)
				AddFilterString(ReportRes.Common_PaymentType.ToLower(), _params.PaymentType.Value.ToDisplayString());

			string periodOfTimeString;

			if (!_params.DateFrom.HasValue && !_params.DateTo.HasValue)
				periodOfTimeString = ReportRes.Common_AllTime;
			else if (!_params.DateFrom.HasValue)
				periodOfTimeString = string.Format(ReportRes.Common_To, _params.DateTo.Value.ToString("dd.MM.yyyy"));
			else if (!_params.DateTo.HasValue)
				periodOfTimeString = string.Format(ReportRes.Common_From, _params.DateFrom.Value.ToString("dd.MM.yyyy"));
			else
				periodOfTimeString = string.Format(ReportRes.Common_FromTo, _params.DateFrom.Value.ToString("dd.MM.yyyy"),
					_params.DateTo.Value.ToString("dd.MM.yyyy"));

			font.Size = 11;

			paragraph = new Paragraph(15, periodOfTimeString.ToLowerFirstLetter(), font) { Alignment = Element.ALIGN_CENTER };

			_pdfDocument.Add(paragraph);
		}

		private void AddFilterString(string caption, string value)
		{
			var font = new Font(_defaultFont) { Size = 11 };

			var paragraph = new Paragraph(15,
					string.Format("{0} = {1}", caption, value), font) { Alignment = Element.ALIGN_CENTER };

			_pdfDocument.Add(paragraph);
		}

		private void GenerateReportContent()
		{
			if (_products.Count > 0)
			{
				GenerateCustomerReport(_params.Client);

				_pdfDocument.Add(new Paragraph(20));

				foreach (var department in _departments)
					GenerateCustomerReport(department);
			}

			GenerateTotal();
		}

		private void GenerateCustomerReport(Party client)
		{
			var clientTypeProduct = _clienTypeProducts.By(a => a.Client == client);

			if (clientTypeProduct != null)
			{
				foreach (var typeProduct in clientTypeProduct.TypeProducts)
				{
					GenerateDocumentTable(clientTypeProduct, typeProduct);
					_pdfDocument.Add(new Paragraph(10));
				}

				GenerateTotal(client, clientTypeProduct);
			}
			else
				GenerateTotal(client, null);
		}

		private void GenerateDocumentTable(ClientTypeProductData clientTypeProduct, TypeProductData typeProduct)
		{
			if (typeProduct.Products.Count == 0) return;

			var table = CreateDocumentTable();

			GenerateDocumentTableHeader(table, clientTypeProduct, typeProduct);
			GenerateDocumentTableList(typeProduct, table);
			GenerateDocumentTableFooter(typeProduct, table);

			_pdfDocument.Add(table);
		}

		private static Table CreateDocumentTable()
		{
			const int columns = 8;

			var table = new Table(columns)
			{
				Cellspacing = 0,
				Cellpadding = 2,
				Border = Rectangle.NO_BORDER,
				Widths = new[] { 11f, 9f, 8.5f, 20.5f, 21f, 10.5f, 9f, 10.5f },
				Width = 100,
				CellsFitPage = true,
				DefaultCellBorderColor = Color.LIGHT_GRAY,
				DefaultVerticalAlignment = Element.ALIGN_TOP,
				DefaultHorizontalAlignment = Element.ALIGN_LEFT
			};

			return table;
		}


		// ReSharper disable once RedundantExplicitArraySize
		static readonly string[] _productSectionNames = new string[(int)ProductTypes.MaxValue + 1]
		{
			ReportRes.CustomerReport_TicketSection,
			ReportRes.CustomerReport_RefundSection,
			ReportRes.CustomerReport_McoSection,
			ReportRes.CustomerReport_PasteboardSection,
			ReportRes.CustomerReport_SimCardSection,
			ReportRes.CustomerReport_IsicSection,
			ReportRes.CustomerReport_ExcursionSection,
			ReportRes.CustomerReport_TourSection,
			ReportRes.CustomerReport_AccommodationSection,
			ReportRes.CustomerReport_TransferSection,
			ReportRes.CustomerReport_InsuranceSection,
			ReportRes.CustomerReport_CarRentalSection,
			ReportRes.CustomerReport_GenericProductSection,
			ReportRes.CustomerReport_BusTicketSection,
			ReportRes.CustomerReport_PasteboardRefundSection,
			ReportRes.CustomerReport_InsuranceRefundSection,
			ReportRes.CustomerReport_BusTicketRefundSection,
		};

		private void GenerateDocumentTableHeader(Table table, ClientTypeProductData clientTypeProduct, TypeProductData typeProduct)
		{
			string customerText;
			var client = clientTypeProduct.Client;
			var type = typeProduct.Type;

			if (client == _params.Client)
				customerText = _departments.Count == 0 ? string.Format("\"{0}\"", _params.Client.NameForDocuments) : string.Format(ReportRes.CustomerReport_CustomerNameWithoutDepartment, _params.Client.NameForDocuments);
			else
				customerText = string.Format(ReportRes.CustomerReport_DepartmentName, client.NameForDocuments);

			var font = new Font(_defaultFontBoldItalic) { Size = 9 };

			var documentCount = string.Format(ReportRes.CustomerReport_DocumentCount, typeProduct.Products.Count);

			var cell = GetCell(string.Format("{0} - {1} ({2})", customerText, _productSectionNames[(int)type], documentCount), font);
			cell.Colspan = table.Columns;
			cell.Border = Rectangle.NO_BORDER;

			table.AddCell(cell);

			font = new Font(_defaultFontBold) { Size = 8 };

			AddCell(table, ReportRes.Common_Document, font, Element.ALIGN_CENTER);

			AddCell(table, ReportRes.CustomerReport_Order, font, Element.ALIGN_CENTER);

			AddCell(table, ReportRes.Common_Date, font, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.Common_Passenger, font, Element.ALIGN_CENTER);

			var description = type == ProductType.AviaMco ? DomainRes.Common_Description : ReportRes.Common_Itinerary;
			AddCell(table, description, font, Element.ALIGN_CENTER);

			var cost = type == ProductType.AviaMco ? ReportRes.CustomerReport_McoCost : ReportRes.CustomerReport_TicketCost;
			AddCell(table, cost, font, Element.ALIGN_CENTER);

			AddCell(table, ReportRes.CustomerReport_ServicreFee, font, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.CustomerReport_GrandTotal, font, Element.ALIGN_CENTER);

			table.EndHeaders();
		}

		private void GenerateDocumentTableList(TypeProductData typeProduct, Table table)
		{
			var products = typeProduct.Products;

			var font = new Font(_defaultFont) { Size = 8 };

			var invalidCellFont = new Font(font);
			invalidCellFont.SetColor(255, 0, 0);

			foreach (var product in products)
			{
				var pos = products.IndexOf(product);

				if (pos == 0)
					table.DefaultCellBorder = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
				else
					table.DefaultCellBorder = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;

				table.DefaultCell.BackgroundColor = pos % 2 == 0 ? new Color(0xF5, 0xF5, 0xF5) : Color.WHITE;

				AddCell(table, product.Name, font, Element.ALIGN_LEFT);

				AddCell(table, product.Order != null ? product.Order.Number : string.Empty, font, Element.ALIGN_LEFT);

				AddCell(table, product.IssueDate.ToString("dd.MM.yyyy"), font, Element.ALIGN_CENTER);
				AddCell(table, product.PassengerName, font, Element.ALIGN_LEFT);

				var text = product.As<AviaMco>().As(a => a.Description) ?? product.As<AviaDocument>().As(a => a.Itinerary);
				AddCell(table, text, font, Element.ALIGN_LEFT);

				var isRefund = product.IsRefund;
				bool isValid;

				AddCell(table, MoneyToString(product.Total, isRefund, out isValid), isValid ? font : invalidCellFont, Element.ALIGN_RIGHT);
				AddCell(table, MoneyToString(product.ExtraCharge, isRefund, out isValid), isValid ? font : invalidCellFont, Element.ALIGN_RIGHT);
				AddCell(table, MoneyToString(product.GrandTotal, isRefund, out isValid), isValid ? font : invalidCellFont, Element.ALIGN_RIGHT);
			}
		}

		private void GenerateDocumentTableFooter(TypeProductData typeProduct, Table table)
		{
			table.DefaultCellBackgroundColor = Color.WHITE;
			table.DefaultHorizontalAlignment = Element.ALIGN_RIGHT;
			table.DefaultCellBorder = Rectangle.BOX;


			var font = new Font(_defaultFontBoldItalic) { Size = 9 };

			var cell = GetCell(ReportRes.Common_Total, font);
			cell.Colspan = table.Columns - 3;
			table.AddCell(cell);

			AddCell(table, typeProduct.Total.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, typeProduct.ExtraCharge.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, typeProduct.GrandTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);
		}

		private void GenerateTotal(Party client, ClientTypeProductData clientTypeProduct)
		{
			if (_departments.Count == 0) return;

			var table = new Table(4)
			{
				Cellspacing = 0,
				Cellpadding = 2,
				Border = Rectangle.NO_BORDER,
				DefaultCellBorder = Rectangle.NO_BORDER,
				Widths = new[] { 70f, 10.5f, 9f, 10.5f },
				Width = 100,
				CellsFitPage = true,
				DefaultCellBorderColor = Color.LIGHT_GRAY,
				DefaultVerticalAlignment = Element.ALIGN_TOP,
				DefaultHorizontalAlignment = Element.ALIGN_RIGHT
			};

			_pdfDocument.Add(new Paragraph(5));

			var font = new Font(_defaultFontBold) { Size = 10 };

			var count = clientTypeProduct.As(a => a.Count);

			var text = client == _params.Client
				? string.Format(ReportRes.CustomerReport_CustomerTotalWithoutDepartment, client.NameForDocuments, count)
				: string.Format(ReportRes.CustomerReport_DepartmentTotal, client.NameForDocuments, count);

			var total = clientTypeProduct.As(a => a.Total);
			var extraCharge = clientTypeProduct.As(a => a.ExtraCharge);
			var grandTotal = clientTypeProduct.As(a => a.GrandTotal);

			AddCell(table, text, font, Element.ALIGN_RIGHT);
			AddCell(table, total.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, extraCharge.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, grandTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);

			_pdfDocument.Add(table);
		}

		private void GenerateTotal()
		{
			_pdfDocument.Add(new Paragraph(20));

			var table = new Table(4)
			{
				Cellspacing = 0,
				Cellpadding = 2,
				Border = Rectangle.TOP_BORDER,
				DefaultCellBorder = Rectangle.NO_BORDER,
				Widths = new[] { 70f, 10.5f, 9f, 10.5f },
				Width = 100,
				CellsFitPage = true,
				DefaultCellBorderColor = Color.LIGHT_GRAY,
				DefaultVerticalAlignment = Element.ALIGN_TOP,
				DefaultHorizontalAlignment = Element.ALIGN_LEFT
			};

			var font = new Font(_defaultFontBold) { Size = 10 };

			var caption = string.Format("{0} - {1}", string.Format(ReportRes.CustomerReport_Total, _params.Client.NameForDocuments),
				string.Format(ReportRes.CustomerReport_DocumentCount, _products.Count));

			AddCell(table, caption, font, Element.ALIGN_RIGHT);

			var totalTotal = _clienTypeProducts.Sum(a => a.Total);
			var extraChargeTotal = _clienTypeProducts.Sum(a => a.ExtraCharge);
			var grandTotalTotal = _clienTypeProducts.Sum(a => a.GrandTotal);

			AddCell(table, totalTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, extraChargeTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);
			AddCell(table, grandTotalTotal.ToMoneyString(), font, Element.ALIGN_RIGHT);

			_pdfDocument.Add(table);
		}

		private static void AddCell(Table table, string text, Font font, int horizontalAlignment)
		{
			var cell = GetCell(text ?? string.Empty, font);
			cell.HorizontalAlignment = horizontalAlignment;

			table.AddCell(cell);
		}

		private static Cell GetCell(string text, Font font)
		{
			return new Cell(new Chunk(text, font)) { Leading = font.Size };
		}

		private string MoneyToString(Money money, bool isNegative, out bool isValid)
		{
			isValid = true;

			if (money == null)
				return string.Empty;

			if (isNegative)
				money *= -1;

			string text;

			if (!money.Currency.Equals(DefaultCurrency))
			{
				if (UseDefaultCurrencyForInput)
					isValid = false;

				text = money.MoneyString;
			}
			else
				text = money.ToString();

			return text;
		}

		private Document _pdfDocument;

		private readonly IList<Product> _products;

		private IList<Party> _departments;
		//private readonly Dictionary<Party, List<Product>> _tickets = new Dictionary<Party, List<Product>>();
		//private readonly Dictionary<Party, List<Product>> _mco = new Dictionary<Party, List<Product>>();
		//private readonly Dictionary<Party, List<Product>> _refunds = new Dictionary<Party, List<Product>>();

		//private readonly Dictionary<Party, decimal> _total = new Dictionary<Party, decimal>();
		//private readonly Dictionary<Party, decimal> _extraCharge = new Dictionary<Party, decimal>();
		//private readonly Dictionary<Party, decimal> _grandTotal = new Dictionary<Party, decimal>();

		//private decimal _totalTotal;
		//private decimal _extraChargeTotal;
		//private decimal _grandTotalTotal;

		private readonly Font _defaultFont = PdfUtility.GetFont(PdfUtility.Times, 10, false, false);
		private readonly Font _defaultFontBoldItalic = PdfUtility.GetFont(PdfUtility.Times, 10, true, true);
		private readonly Font _defaultFontBold = PdfUtility.GetFont(PdfUtility.Times, 10, true, false);

		private readonly CustomerReportParams _params;
		private IList<ClientTypeProductData> _clienTypeProducts;
	}
}