using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{

	public enum TextAlign
	{
		Center,
		Left,
		Right
	}

	[Flags]
	public enum BorderStyle
	{
		None = 0,
		Top = 0x0001,
		Left = 0x0002,
		Right = 0x0004,
		Bottom = 0x0008,
		Box = Top | Left | Right | Bottom
	}

	public class RegistryReport
	{
		public DateTime? DateFrom { get; set; }

		public DateTime? DateTo { get; set; }

		public IList<AviaDocument> Products { get; set; }

		public Currency DefaultCurrency { get; set; }

		public bool UseDefaultCurrencyForInput { get; set; }

		public PaymentType? PaymentType { get; set; }

		public string AirlineCode { get; set; }

		public void Build(Stream stream, ReportType type)
		{
			if (Products == null)
				Products = EmptyDocumentList;

			AggregateData();

			if (type == ReportType.Excel)
				_reportWriter = new RegistryReportExcelWriter(stream);
			else
				_reportWriter = new RegistryReportPdfWriter(stream);

			GenerateHeader();

			GenerateContent();

			_reportWriter.Close();
		}

		private void AggregateData()
		{
			foreach (AviaDocument document in Products)
				AddProduct(document);
		}

		private void AddProduct(AviaDocument product)
		{
			var refund = product as AviaRefund;

			var sign = refund != null ? -1 : 1;

			AddSum(_fare, (product.EqualFare ?? product.Fare) * sign);
			if (refund != null)
			{
				AddSum(_cancelFee, refund.CancelFee);
			}
			AddSum(_feesTotal, product.FeesTotal * sign);
			AddSum(_total, product.Total * sign);
			AddSum(_commission, product.Commission * sign);
			AddSum(_extracharge, product.ExtraCharge * sign);
			AddSum(_profit, product.Profit * sign);
			AddSum(_grandTotal, product.GrandTotal * sign);

			if (product is AviaTicket)
				++_ticketCount;
			else if (product is AviaMco)
				++_mcoCount;
			else
				++_refundCount;
		}

		private void GenerateHeader()
		{
			_reportWriter.SetTextFont(20, false, false);
			_reportWriter.WriteReportTitle(ReportRes.RegistryReport_Title);

			SetFilterString();

			_reportWriter.SetTextFont(11, false, false);
			_reportWriter.WriteFilterString(_filterString.ToLowerFirstLetter());
		}

		private void SetFilterString()
		{
			string period;

			if (!DateFrom.HasValue && !DateTo.HasValue)
				period = ReportRes.Common_AllTime;
			else if (!DateFrom.HasValue)
				period = string.Format(ReportRes.Common_To, DateTo.Value.ToString("dd.MM.yyyy"));
			else if (!DateTo.HasValue)
				period = string.Format(ReportRes.Common_From, DateFrom.Value.ToString("dd.MM.yyyy"));
			else
				period = string.Format(ReportRes.Common_FromTo, DateFrom.Value.ToString("dd.MM.yyyy"),
					DateTo.Value.ToString("dd.MM.yyyy"));

			var filter = new StringBuilder(period);

			if (PaymentType.HasValue)
				filter
					.Append(Environment.NewLine)
					.AppendFormat("{0} = {1}", ReportRes.Common_PaymentType.ToLower(), PaymentType.Value.ToDisplayString());


			_filterString = filter.ToString();
		}

		private void GenerateContent()
		{
			GenerateDocumentTable();

			GenerateReportFooter();
		}

		private void GenerateDocumentTable()
		{
			_reportWriter.BeginDocumentTable();

			if (Products.Count == 0)
				return;

			_reportWriter.SetCellBorder(BorderStyle.Box);
			_reportWriter.SetTextAlign(TextAlign.Center);
			_reportWriter.SetTextFont(9, true, false);

			_reportWriter.BeginDocumentTableRow();

			_reportWriter.WriteDocumentTableCell(ReportRes.Common_Document);
			_reportWriter.WriteDocumentTableCell($"{ReportRes.Common_Date}{NewLine}{ReportRes.Common_Agent}");
			_reportWriter.WriteDocumentTableCell($"{ReportRes.Common_Passenger}{NewLine}{ReportRes.Common_Itinerary}");
			_reportWriter.WriteDocumentTableCell(DomainRes.Common_Fare);
			_reportWriter.WriteDocumentTableCell(DomainRes.Common_CancelFee);
			_reportWriter.WriteDocumentTableCell(DomainRes.Common_FeesTotal);
			_reportWriter.WriteDocumentTableCell(DomainRes.Product_Total);
			_reportWriter.WriteDocumentTableCell(DomainRes.Common_Commission);
			_reportWriter.WriteDocumentTableCell(DomainRes.AviaDocument_ExtraCharge);
			_reportWriter.WriteDocumentTableCell(DomainRes.AviaDocument_Profit);
			_reportWriter.WriteDocumentTableCell(DomainRes.Product_GrandTotal);
			_reportWriter.WriteDocumentTableCell(DomainRes.Invoice_Caption_List);

			_reportWriter.EndDocumentTableHeaders();

			_reportWriter.SetTextFont(7, false, false);

			foreach (var document in Products)
			{
				_reportWriter.BeginDocumentTableRow();

				_reportWriter.SetTextAlign(TextAlign.Left);

				var rowIndex = Products.IndexOf(document);

				_reportWriter.SetCellBorder(rowIndex == 0
					? BorderStyle.Right | BorderStyle.Left | BorderStyle.Top
					: BorderStyle.Right | BorderStyle.Left);

				if (rowIndex % 2 == 0)
					_reportWriter.SetCellColor(0xF5, 0xF5, 0xF5);
				else
					_reportWriter.SetCellColor(0xFF, 0xFF, 0xFF);

				var ticket = document as AviaTicket;
				var mco = document as AviaMco;
				var refund = document as AviaRefund;

				var text = document.Type.ToDisplayString();

				if (ticket?.ReissueFor != null)
					text = ReportRes.RegistryReport_Reissue;
				else if (mco?.ReissueFor != null)
					text = ReportRes.RegistryReport_Reissue;

				_reportWriter.WriteDocumentTableCell($"{text}{NewLine}{document.FullNumber}");

				_reportWriter.SetTextAlign(TextAlign.Center);

				_reportWriter.WriteDocumentTableCell(
					$"{document.IssueDate:dd.MM.yyyy}{NewLine}{document.TicketerCode}");

				_reportWriter.SetTextAlign(TextAlign.Left);
				text = document.PassengerName;
				text = $"{text}{NewLine}{document.Itinerary}";
				_reportWriter.WriteDocumentTableCell(text);

				_reportWriter.SetTextAlign(TextAlign.Right);
				bool isValid;
				_reportWriter.WriteDocumentTableCell(MoneyToString(document.EqualFare ?? document.Fare, refund != null, out isValid), isValid);
				_reportWriter.WriteDocumentTableCell(MoneyToString(refund?.CancelFee, refund == null, out isValid), isValid);
				_reportWriter.WriteDocumentTableCell(MoneyToString(document.FeesTotal, refund != null, out isValid), isValid);
				_reportWriter.WriteDocumentTableCell(MoneyToString(document.Total, refund != null, out isValid), isValid);
				_reportWriter.WriteDocumentTableCell(MoneyToString(document.Commission, refund != null, out isValid), isValid);
				_reportWriter.WriteDocumentTableCell(MoneyToString(document.ExtraCharge, refund != null, out isValid), isValid);
				_reportWriter.WriteDocumentTableCell(MoneyToString(document.Profit, refund != null, out isValid), isValid);
				_reportWriter.WriteDocumentTableCell(MoneyToString(document.GrandTotal, refund != null, out isValid), isValid);

				_reportWriter.SetTextAlign(TextAlign.Left);
				text = document.Order?.InvoiceNumbers;
				_reportWriter.WriteDocumentTableCell(text);
			}

			_reportWriter.EndDocumentTableContent();

			_reportWriter.SetCellColor(0xFF, 0xFF, 0xFF);

			foreach (var currencyCode in _currencies)
			{
				_reportWriter.BeginDocumentTableRow();

				_reportWriter.SetTextFont(9, true, false);
				_reportWriter.SetTextAlign(TextAlign.Left);

				BorderStyle border = BorderStyle.None;

				if (_currencies.IndexOf(currencyCode) == _currencies.Count - 1)
					border |= BorderStyle.Bottom;

				if (_currencies.IndexOf(currencyCode) == 0)
				{
					border |= BorderStyle.Top;

					_reportWriter.SetCellBorder(border | BorderStyle.Left);

					_reportWriter.WriteDocumentTableCell(ReportRes.Common_Total);
				}
				else
				{
					_reportWriter.SetCellBorder(border | BorderStyle.Left);

					_reportWriter.WriteDocumentTableCell(string.Empty);
				}

				_reportWriter.SetCellBorder(border);

				_reportWriter.WriteDocumentTableCell(string.Empty);

				_reportWriter.SetTextAlign(TextAlign.Right);

				_reportWriter.WriteDocumentTableCell(currencyCode);

				_reportWriter.SetCellBorder(border | BorderStyle.Left | BorderStyle.Right);

				_reportWriter.SetTextFont(9, true, true);

				_reportWriter.WriteDocumentTableCell(GetTotalString(_fare, currencyCode));
				_reportWriter.WriteDocumentTableCell(GetTotalString(_cancelFee, currencyCode));
				_reportWriter.WriteDocumentTableCell(GetTotalString(_feesTotal, currencyCode));
				_reportWriter.WriteDocumentTableCell(GetTotalString(_total, currencyCode));
				_reportWriter.WriteDocumentTableCell(GetTotalString(_commission, currencyCode));
				_reportWriter.WriteDocumentTableCell(GetTotalString(_extracharge, currencyCode));
				_reportWriter.WriteDocumentTableCell(GetTotalString(_profit, currencyCode));
				_reportWriter.WriteDocumentTableCell(GetTotalString(_grandTotal, currencyCode));
				_reportWriter.WriteDocumentTableCell("");
			}

			_reportWriter.EndDocumentTable();
		}

		private void GenerateReportFooter()
		{
			_reportWriter.BeginFooter();

			_reportWriter.SetTextFont(11, false, false);

			_reportWriter.WriteFooterText(_filterString);

			_reportWriter.WriteOperationsTable(GenerateOperationsTable());

			_reportWriter.EndFooter();
		}

		private string[,] GenerateOperationsTable()
		{
			var data = new string[4, 2];

			data[0, 0] = ReportRes.RegistryReport_Operations;
			data[0, 1] = Products.Count.ToString();

			data[1, 0] = ReportRes.RegistryReport_Tickets;
			data[1, 1] = _ticketCount.ToString();

			data[2, 0] = ReportRes.RegistryReport_Refunds;
			data[2, 1] = _refundCount.ToString();

			data[3, 0] = ReportRes.RegistryReport_Mco;
			data[3, 1] = _mcoCount.ToString();

			return data;
		}

		private void AddSum(Dictionary<string, decimal> dictionary, Money money)
		{
			if (money == null)
				return;

			var code = money.Currency.Code;

			if (!_currencies.Contains(code))
				_currencies.Add(code);

			if (!dictionary.ContainsKey(code))
				dictionary.Add(code, 0);

			dictionary[code] += money.Amount;
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
				text = money.ToString("F2");//money.Amount.ToString("N2", CultureInfo.InvariantCulture);

			return text;
		}

		private static string GetTotalString(Dictionary<string, decimal> total, string currencyCode)
		{
			if (total.ContainsKey(currencyCode))
				return total[currencyCode].ToMoneyString();

			return string.Empty;
		}

		private static readonly List<AviaDocument> EmptyDocumentList = new List<AviaDocument>();

		private readonly List<string> _currencies = new List<string>();
		private readonly Dictionary<string, decimal> _fare = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _cancelFee = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _feesTotal = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _total = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _commission = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _extracharge = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _profit = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _grandTotal = new Dictionary<string, decimal>();

		private int _ticketCount;
		private int _refundCount;
		private int _mcoCount;

		private IRegistryReportWriter _reportWriter;

		private string _filterString;

		private const string NewLine = "\n";
	}
}