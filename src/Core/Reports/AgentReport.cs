using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Base.Metamodel;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public class AgentReport
	{
		static AgentReport()
		{
			_cellFont = PdfUtility.GetFont(PdfUtility.Times, 8, false, false);
			_invalidCellFont = new Font(_cellFont);
			_invalidCellFont.SetColor(255, 0, 0);

			_cellFontBold = PdfUtility.GetFont(PdfUtility.Times, 8, true, false);

			_cellFontBoldItalic = PdfUtility.GetFont(PdfUtility.Times, 8, true, true);
			_cellFontBoldItalic.Size = 8;
		}

		public Currency DefaultCurrency { get; set; }

		public bool UseDefaultCurrencyForInput { get; set; }

		public bool ShowAviaHandling { get; set; }

		public string CompanyName { get; set; }

		public string AgentName { get; set; }

		public DateTime Date { get; set; }

		public IList<Product> Products { get; set; }

		public IList<Payment> Payments { get; set; }

		public void Build(Stream stream)
		{
			if (Products == null)
				Products = _emptyDocumentList;

			AggregateData();

			_document = new Document(PageSize.A4.Rotate(), 15, 15, 20, 20);

			PdfWriter.GetInstance(_document, stream);

			AddDocumentHeader();

			_document.Open();

			AddReportHeader();

			AddAviaDocumentReport();

			AddPaymentReport();

			_document.Close();
		}

		private void AggregateData()
		{
			foreach (var product in Products)
			{
				if (product.IsRefund)
				{
					++_refundCount;

					_refundDocuments.Add(product);

					if (!product.IsVoid)
					{
						var refund = product;

						AddSum(_refundCurrencies, _refundGrandTotal, -product.GrandTotal);
						AddSum(_refundCurrencies, _refundFare, -(product.EqualFare ?? product.Fare));
						AddSum(_refundCurrencies, _refundFeesTotal, -product.FeesTotal);
						AddSum(_refundCurrencies, _refundCancelFeeTotal, -refund.CancelFee);
						AddSum(_refundCurrencies, _refundTotal, -product.Total);
						AddSum(_refundCurrencies, _refundCommission, -product.Commission);
						AddSum(_refundCurrencies, _refundCommissionDiscount, -refund.CommissionDiscount);
						AddSum(_refundCurrencies, _refundServiceFee, -refund.ServiceTotal);
						AddSum(_refundCurrencies, _refundHandling, -refund.Handling);
						AddSum(_refundCurrencies, _refundDiscount, -product.Discount);
						AddSum(_refundCurrencies, _refundProfit, -product.Profit);

						AddSum(-product.GrandTotal, product.PaymentType, _expenseTotal);
						AddSum(-product.ExtraCharge, product.PaymentType, _extraChargeTotal);
					}
				}
				else
				{
					_ticketMcoDocuments.Add(product);

					if (product.IsAviaTicket)
					{
						if (product.ReissueFor != null)
							++_reissueCount;
						else
							++_ticketCount;
					}
					else
					{
						if ( product.ReissueFor != null)
							++_reissueCount;
						else
							++_mcoCount;
					}

					if (!product.IsVoid)
					{
						AddSum(_saleCurrencies, _saleGrandTotal, product.GrandTotal);
						AddSum(_saleCurrencies, _saleFare, product.EqualFare ?? product.Fare);
						AddSum(_saleCurrencies, _saleFeesTotal, product.FeesTotal);
						AddSum(_saleCurrencies, _saleTotal, product.Total);
						AddSum(_saleCurrencies, _saleCommission, product.Commission);
						AddSum(_saleCurrencies, _saleCommissionDiscount, product.CommissionDiscount);
						AddSum(_saleCurrencies, _saleServiceFee, product.ServiceFee);
						AddSum(_saleCurrencies, _saleHandling, product.Handling);
						AddSum(_saleCurrencies, _saleDiscount, product.Discount);
						AddSum(_saleCurrencies, _saleProfit, product.Profit);

						AddSum(product.GrandTotal, product.PaymentType, _incomeTotal);
						AddSum(product.ExtraCharge, product.PaymentType, _extraChargeTotal);
					}
				}
			}

			foreach (var code in _saleCurrencies)
			{
				if (!_saleGrandTotal.ContainsKey(code))
					_saleGrandTotal.Add(code, 0);
				if (!_saleFare.ContainsKey(code))
					_saleFare.Add(code, 0);
				if (!_saleFeesTotal.ContainsKey(code))
					_saleFeesTotal.Add(code, 0);
				if (!_saleTotal.ContainsKey(code))
					_saleTotal.Add(code, 0);
				if (!_saleCommission.ContainsKey(code))
					_saleCommission.Add(code, 0);
				if (!_saleCommissionDiscount.ContainsKey(code))
					_saleCommissionDiscount.Add(code, 0);
				if (!_saleServiceFee.ContainsKey(code))
					_saleServiceFee.Add(code, 0);
				if (!_saleHandling.ContainsKey(code))
					_saleHandling.Add(code, 0);
				if (!_saleDiscount.ContainsKey(code))
					_saleDiscount.Add(code, 0);
				if (!_saleProfit.ContainsKey(code))
					_saleProfit.Add(code, 0);

				if (!_currencies.Contains(code))
					_currencies.Add(code);
			}

			foreach (var code in _refundCurrencies)
			{
				if (!_refundGrandTotal.ContainsKey(code))
					_refundGrandTotal.Add(code, 0);
				if (!_refundFare.ContainsKey(code))
					_refundFare.Add(code, 0);
				if (!_refundFeesTotal.ContainsKey(code))
					_refundFeesTotal.Add(code, 0);
				if (!_refundCancelFeeTotal.ContainsKey(code))
					_refundCancelFeeTotal.Add(code, 0);
				if (!_refundTotal.ContainsKey(code))
					_refundTotal.Add(code, 0);
				if (!_refundCommission.ContainsKey(code))
					_refundCommission.Add(code, 0);
				if (!_refundCommissionDiscount.ContainsKey(code))
					_refundCommissionDiscount.Add(code, 0);
				if (!_refundServiceFee.ContainsKey(code))
					_refundServiceFee.Add(code, 0);
				if (!_refundHandling.ContainsKey(code))
					_refundHandling.Add(code, 0);
				if (!_refundDiscount.ContainsKey(code))
					_refundDiscount.Add(code, 0);
				if (!_refundProfit.ContainsKey(code))
					_refundProfit.Add(code, 0);

				if (!_currencies.Contains(code))
					_currencies.Add(code);
			}
		}

		private static void AddSum(Money money, PaymentType paymentType, Dictionary<string, Dictionary<PaymentType, decimal>> dictionary)
		{
			if (money == null)
				return;

			var code = money.Currency.Code;

			if (!dictionary.ContainsKey(code))
				dictionary.Add(code, new Dictionary<PaymentType, decimal>());

			if (!dictionary[code].ContainsKey(paymentType))
				dictionary[code].Add(paymentType, 0);

			dictionary[code][paymentType] += money.Amount;
		}

		private void AddDocumentHeader()
		{
			var font = PdfUtility.GetFont(PdfUtility.Times, 6, false, true);

			var captionFont = PdfUtility.GetFont(PdfUtility.Times, 11, true, false);

			var phrase = new Phrase
			{
				Leading = 6,
				Font = font
			};

			var table = new Table(2)
			{
				Border = Rectangle.NO_BORDER,
				Cellpadding = 0,
				Cellspacing = 0,
				DefaultCellBorder = Rectangle.NO_BORDER,
				Width = 99,
				Alignment = Element.ALIGN_LEFT,
				Offset = -6f
			};

			var cell = AddCell(table, CompanyName, captionFont);
			cell.HorizontalAlignment = Element.ALIGN_LEFT;
			cell.VerticalAlignment = Element.ALIGN_BOTTOM;

			cell = AddCell(table, ReportRes.AgentReport_Header, font);
			cell.HorizontalAlignment = Element.ALIGN_RIGHT;
			cell.VerticalAlignment = Element.ALIGN_BOTTOM;

			phrase.Add(table);

			var header = new HeaderFooter(phrase, true)
			{
				Alignment = Element.ALIGN_RIGHT,
				Border = Rectangle.BOTTOM_BORDER
			};

			_document.Header = header;
		}

		private void AddReportHeader()
		{
			var agentTbl = GetAgentInfoTable();
			var balanceTbl = GetBalanceByPaymentTypeTable();
			var docSummaryTbl = GetDocSummaryTable();

			var table = new PdfPTable(5)
			{
				WidthPercentage = 100
			};

			table.SetTotalWidth(new[]
			{
				47f, 2, 38f, 3f, 10f
			});
			table.DefaultCell.Padding = 0;
			table.DefaultCell.Border = Rectangle.NO_BORDER;

			table.AddCell(agentTbl);

			table.AddCell();

			if (balanceTbl != null)
				table.AddCell(balanceTbl);
			else
				table.AddCell();

			table.AddCell();

			var pdfPCell = new PdfPCell(table.DefaultCell);
			pdfPCell.AddElement(docSummaryTbl);
			table.AddCell(pdfPCell);

			_document.Add(table);

			var paragraph = new Paragraph(ReportRes.AgentReport_AgentSignature, _defaultFont)
			{
				SpacingBefore = 10,
				Leading = 0
			};
			paragraph.Add("                                       ");
			paragraph.Add(ReportRes.AgentReport_SupervisorSignature);

			_document.Add(paragraph);

			_document.Add(new Phrase(10));
		}

		private void AddAviaDocumentReport()
		{
			if (Products.Count == 0)
			{
				var paragraph = new Paragraph(ReportRes.AgentReport_NoData, _cellFont)
				{
					Alignment = Element.ALIGN_CENTER,
					Leading = 40
				};

				_document.Add(paragraph);
			}

			AddSales();

			AddRefunds();

			AddTotalBalance();
		}

		private void AddPaymentReport()
		{
			if (Payments == null || Payments.Count == 0)
				return;

			_document.NewPage();

			var caption = new Paragraph(ReportRes.AgentReport_Payments, _cellFontBold)
			{
				IndentationLeft = 2
			};

			_document.Add(caption);

			var table = new PdfPTable(10)
			{
				WidthPercentage = 100,
				SpacingBefore = 2,
			};

			table.SetTotalWidth(new[] { 10f, 12, 12, 12, 8, 10, 10, 12, 8, 8 });

			table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.DefaultCell.PaddingTop = 1;

			table.AddCell(_cellFontBold, DomainRes.Payment);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_Document);
			table.AddCell(_cellFontBold, DomainRes.Payment_Payer);
			table.AddCell(_cellFontBold, DomainRes.Payment_ReceivedFrom);
			table.AddCell(_cellFontBold, DomainRes.Payment_Amount);
			table.AddCell(_cellFontBold, DomainRes.Payment_Invoice);
			table.AddCell(_cellFontBold, DomainRes.Order);
			table.AddCell(_cellFontBold, DomainRes.Common_Customer);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_OrderTotal);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_OrderDue);

			table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

			var paymentGroup = Payments.GroupBy(p => p.Order);

			foreach (var payments in paymentGroup)
				AddPayments(table, payments.Key, payments.ToList());

			_document.Add(table);

			AddPaymentTotal();
		}

		private void AddPayments(PdfPTable table, Order order, IEnumerable<Payment> payments)
		{
			foreach (var payment in payments)
			{
				table.AddCell(_cellFont, payment.Number);

				var electronicPayment = payment as ElectronicPayment;

				if (electronicPayment == null || electronicPayment.PaymentSystem == null)
					table.AddCell(_cellFont, payment.GetClass().Caption + " " + payment.DocumentNumber);
				else
					table.AddCell(_cellFont, electronicPayment.PaymentSystem.Name + " " + payment.DocumentNumber);

				table.AddCell(_cellFont, payment.Payer != null ? payment.Payer.Name : null);
				table.AddCell(_cellFont, payment.ReceivedFrom);
				table.AddCell(_cellFont, _invalidCellFont, payment.Sign * payment.Amount, DefaultCurrency, UseDefaultCurrencyForInput);
				table.AddCell(_cellFont, payment.Invoice != null ? payment.Invoice.Number : null);

				if (order == null)
				{
					table.AddCell();
					table.AddCell();
					table.AddCell();
					table.AddCell();
				}
				else
				{
					table.AddCell(_cellFont, order.Number);
					table.AddCell(_cellFont, order.Customer.Name);
					table.AddCell(_cellFont, _invalidCellFont, order.Total, DefaultCurrency, UseDefaultCurrencyForInput);
					table.AddCell(_cellFont, _invalidCellFont, order.TotalDue, DefaultCurrency, UseDefaultCurrencyForInput);
				}
			}
		}

		private void AddPaymentTotal()
		{
			var total = new Dictionary<Currency, Dictionary<string, decimal>>();
			var grandTotal = new Dictionary<Currency, decimal>();

			foreach (var payment in Payments)
			{
				if (payment.Amount == null)
					continue;

				var currency = payment.Amount.Currency;

				var paymentType = payment.GetClass().Caption;

				if (!total.ContainsKey(currency))
					total.Add(currency, new Dictionary<string, decimal>());

				if (!total[currency].ContainsKey(paymentType))
					total[currency].Add(paymentType, 0);

				total[currency][paymentType] += payment.Sign * payment.Amount.Amount;

				if (!grandTotal.ContainsKey(currency))
					grandTotal.Add(currency, 0);

				grandTotal[currency] += payment.Sign * payment.Amount.Amount;
			}

			var font = new Font(_defaultFont)
			{
				Size = 9
			};

			var fontBold = new Font(_defaultFontBold)
			{
				Size = 9
			};


			var table = new PdfPTable(3)
			{
				WidthPercentage = 20,
				HorizontalAlignment = Element.ALIGN_LEFT,
				SpacingBefore = 5
			};

			table.SetTotalWidth(new[] { 5f, 15, 10 });

			table.DefaultCell.Border = Rectangle.NO_BORDER;

			foreach (var currency in total.Keys)
			{
				table.AddCell(fontBold, currency.Code);

				var isFirstRow = true;

				foreach (var paymentType in total[currency].Keys)
				{
					if (!isFirstRow)
						table.AddCell();

					table.AddCell(font, paymentType);
					table.AddDecimal(font, total[currency][paymentType], false);

					isFirstRow = false;

					table.DefaultCell.Border = Rectangle.NO_BORDER;
				}

				table.AddCell();
				table.AddCell(fontBold, ReportRes.Common_Total);
				table.AddDecimal(fontBold, grandTotal[currency], false);

				table.DefaultCell.Border = Rectangle.TOP_BORDER;
			}

			_document.Add(table);
		}

		private PdfPTable GetAgentInfoTable()
		{
			var table = new PdfPTable(2)
			{
				WidthPercentage = 100
			};
			table.SetTotalWidth(new float[]
			{
				17, 83
			});
			table.DefaultCell.Border = Rectangle.NO_BORDER;

			table.AddCell(_defaultFont, ReportRes.AgentReport_Report, Element.ALIGN_RIGHT);
			table.AddCell(_defaultFontBold, AgentName);

			var owners = GetDocumentOwners();

			if (owners.Count > 0)
			{
				table.AddCell(_defaultFont,
					owners.Count > 1 ? ReportRes.AgentReport_Offices : ReportRes.AgentReport_Office, Element.ALIGN_RIGHT);

				for (var index = 0; index < owners.Count; index++)
				{
					if (index > 0)
						table.AddCell();

					table.AddCell(_defaultFontBold, owners[index].Name);
				}
			}

			table.AddCell(_defaultFont, ReportRes.AgentReport_Date, Element.ALIGN_RIGHT);
			table.AddCell(_defaultFontBold, Date.ToString("dd.MM.yyyy"));

			return table;
		}

		private PdfPTable GetBalanceByPaymentTypeTable()
		{
			if (_incomeTotal.Count == 0 && _expenseTotal.Count == 0 && _extraChargeTotal.Count == 0)
				return null;

			var fontBold = new Font(_defaultFontBold)
			{
				Size = 9
			};

			var font = new Font(_defaultFont)
			{
				Size = 9
			};

			var table = new PdfPTable(6)
			{
				WidthPercentage = 100
			};
			table.SetTotalWidth(new[]
			{
				10f, 20f, 17, 17f, 18f, 18f
			});
			table.DefaultCell.Border = Rectangle.NO_BORDER;

			table.AddCell();
			table.AddCell();
			table.AddCell(fontBold, ReportRes.AgentReport_ExtraCharge, Element.ALIGN_CENTER);
			table.AddCell(fontBold, ReportRes.AgentReport_Income, Element.ALIGN_CENTER);
			table.AddCell(fontBold, ReportRes.AgentReport_Expense, Element.ALIGN_CENTER);
			table.AddCell(fontBold, ReportRes.AgentReport_Balance, Element.ALIGN_CENTER);

			foreach (var code in _currencies)
			{
				if (!_incomeTotal.ContainsKey(code) && !_expenseTotal.ContainsKey(code) && !_extraChargeTotal.ContainsKey(code))
					continue;

				table.AddCell(fontBold, code, Element.ALIGN_RIGHT, Rectangle.TOP_BORDER);

				var currencyRow = true;

				decimal extraChargeSum = 0;
				decimal incomeSum = 0;
				decimal expenseSum = 0;

				foreach (PaymentType type in Enum.GetValues(typeof (PaymentType)))
				{
					decimal extraCharge = 0;
					decimal income = 0;
					decimal expense = 0;

					if (_extraChargeTotal.ContainsKey(code) && _extraChargeTotal[code].ContainsKey(type))
						extraCharge = _extraChargeTotal[code][type];

					if (_incomeTotal.ContainsKey(code) && _incomeTotal[code].ContainsKey(type))
						income = _incomeTotal[code][type];

					if (_expenseTotal.ContainsKey(code) && _expenseTotal[code].ContainsKey(type))
						expense = _expenseTotal[code][type];

					if (income == 0 && expense == 0 && extraCharge == 0)
						continue;

					if (!currencyRow)
						table.AddCell();

					var border = currencyRow ? Rectangle.TOP_BORDER : Rectangle.NO_BORDER;

					table.AddCell(font, type.ToDisplayString(), Element.ALIGN_RIGHT, border);
					table.AddCell(font, extraCharge.ToMoneyString(), Element.ALIGN_RIGHT, border);
					table.AddCell(font, income.ToMoneyString(), Element.ALIGN_RIGHT, border);
					table.AddCell(font, expense.ToMoneyString(), Element.ALIGN_RIGHT, border);
					table.AddCell(font, (income + expense).ToMoneyString(), Element.ALIGN_RIGHT, border);

					extraChargeSum += extraCharge;
					incomeSum += income;
					expenseSum += expense;

					currencyRow = false;
				}

				table.AddCell();
				table.AddCell(fontBold, ReportRes.Common_Total, Element.ALIGN_RIGHT);
				table.AddCell(fontBold, extraChargeSum.ToMoneyString(), Element.ALIGN_RIGHT);
				table.AddCell(fontBold, incomeSum.ToMoneyString(), Element.ALIGN_RIGHT);
				table.AddCell(fontBold, expenseSum.ToMoneyString(), Element.ALIGN_RIGHT);
				table.AddCell(fontBold, (incomeSum + expenseSum).ToMoneyString(), Element.ALIGN_RIGHT);
			}
			return table;
		}

		private PdfPTable GetDocSummaryTable()
		{
			var table = new PdfPTable(2);
			table.SetTotalWidth(new float[]
			{
				70, 30
			});
			table.WidthPercentage = 100;
			table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
			table.DefaultCell.Padding = 4;

			var fontBold = new Font(_defaultFontBold)
			{
				Size = 9
			};

			var font = new Font(_defaultFont)
			{
				Size = 9
			};

			table.AddCell(fontBold, ReportRes.AgentReport_Documents,
				Element.ALIGN_RIGHT, Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER);

			table.AddCell(fontBold, Products.Count.ToString(),
				Element.ALIGN_RIGHT, Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);

			table.AddCell(font, DomainRes.AviaMco, Element.ALIGN_RIGHT, Rectangle.LEFT_BORDER);
			table.AddCell(font, _mcoCount.ToString(), Element.ALIGN_RIGHT, Rectangle.RIGHT_BORDER);

			table.AddCell(font, DomainRes.AviaTicket, Element.ALIGN_RIGHT, Rectangle.LEFT_BORDER);
			table.AddCell(font, _ticketCount.ToString(), Element.ALIGN_RIGHT, Rectangle.RIGHT_BORDER);

			table.AddCell(font, ReportRes.AgentReport_Reissue, Element.ALIGN_RIGHT, Rectangle.LEFT_BORDER);
			table.AddCell(font, _reissueCount.ToString(), Element.ALIGN_RIGHT, Rectangle.RIGHT_BORDER);

			table.AddCell(font, DomainRes.AviaRefund, Element.ALIGN_RIGHT, Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER);
			table.AddCell(font, _refundCount.ToString(), Element.ALIGN_RIGHT, Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER);

			return table;
		}

		private void AddSales()
		{
			if (_ticketMcoDocuments.Count == 0)
				return;

			var table = GetDocumentsTable(_ticketMcoDocuments, ReportRes.AgentReport_Sales);

			var caption = ReportRes.AgentReport_SalesTotal + "     ";

			table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
			table.DefaultCell.BackgroundColor = Color.WHITE;
			table.DefaultCell.Border = Rectangle.TOP_BORDER;

			foreach (var code in _saleCurrencies)
			{
				var text = caption + code;
				caption = string.Empty;

				table.AddCell(new PdfPCell(table.DefaultCell)
				{
					Phrase = new Phrase(text, _cellFontBold),
					Colspan = 3
				});

				table.AddDecimal(_cellFontBoldItalic, _saleFare[code], false);
				table.AddDecimal(_cellFontBoldItalic, _saleFeesTotal[code], false);
				table.AddCell();
				table.AddDecimal(_cellFontBoldItalic, _saleTotal[code], false);
				table.AddDecimal(_cellFontBoldItalic, _saleCommission[code], false);

				if (ShowAviaHandling)
					table.AddDecimal(_cellFontBoldItalic, _saleCommissionDiscount[code], false);

				table.AddDecimal(_cellFontBoldItalic, _saleServiceFee[code], false);

				if (ShowAviaHandling)
					table.AddDecimal(_cellFontBoldItalic, _saleHandling[code], false);

				table.AddDecimal(_cellFontBoldItalic, _saleDiscount[code], false);
				table.AddDecimal(_cellFontBoldItalic, _saleProfit[code], false);
				table.AddDecimal(_cellFontBoldItalic, _saleGrandTotal[code], false);

				table.AddCell();
				table.AddCell();

				table.DefaultCell.Border = Rectangle.NO_BORDER;
			}

			_document.Add(table);
		}

		private void AddRefunds()
		{
			if (_refundDocuments.Count == 0)
				return;

			var table = GetDocumentsTable(_refundDocuments, ReportRes.AgentReport_Refunds);

			var caption = ReportRes.AgentReport_RefundTotal + "     ";

			table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
			table.DefaultCell.BackgroundColor = Color.WHITE;
			table.DefaultCell.Border = Rectangle.TOP_BORDER;

			foreach (var code in _refundCurrencies)
			{
				var text = caption + code;
				caption = string.Empty;

				var cell = new PdfPCell(table.DefaultCell)
				{
					Phrase = new Phrase(text, _cellFontBold),
					Colspan = 3
				};

				table.AddCell(cell);

				table.AddDecimal(_cellFontBoldItalic, _refundFare[code], false);
				table.AddDecimal(_cellFontBoldItalic, _refundFeesTotal[code], false);
				table.AddDecimal(_cellFontBoldItalic, _refundCancelFeeTotal[code], false);
				table.AddDecimal(_cellFontBoldItalic, _refundTotal[code], false);
				table.AddDecimal(_cellFontBoldItalic, _refundCommission[code], false);

				if (ShowAviaHandling)
					table.AddDecimal(_cellFontBoldItalic, _refundCommissionDiscount[code], false);

				table.AddDecimal(_cellFontBoldItalic, _refundServiceFee[code], false);

				if (ShowAviaHandling)
					table.AddDecimal(_cellFontBoldItalic, _refundHandling[code], false);

				table.AddDecimal(_cellFontBoldItalic, _refundDiscount[code], false);
				table.AddDecimal(_cellFontBoldItalic, _refundProfit[code], false);
				table.AddDecimal(_cellFontBoldItalic, _refundGrandTotal[code], false);

				table.AddCell();
				table.AddCell();

				table.DefaultCell.Border = Rectangle.NO_BORDER;
			}

			_document.Add(table);
		}

		private void AddTotalBalance()
		{
			foreach (var code in _currencies)
			{
				if (!_saleGrandTotal.ContainsKey(code))
					_saleGrandTotal.Add(code, 0);
				if (!_saleFare.ContainsKey(code))
					_saleFare.Add(code, 0);
				if (!_saleFeesTotal.ContainsKey(code))
					_saleFeesTotal.Add(code, 0);
				if (!_saleTotal.ContainsKey(code))
					_saleTotal.Add(code, 0);
				if (!_saleCommission.ContainsKey(code))
					_saleCommission.Add(code, 0);
				if (!_saleCommissionDiscount.ContainsKey(code))
					_saleCommissionDiscount.Add(code, 0);
				if (!_saleServiceFee.ContainsKey(code))
					_saleServiceFee.Add(code, 0);
				if (!_saleHandling.ContainsKey(code))
					_saleHandling.Add(code, 0);
				if (!_saleDiscount.ContainsKey(code))
					_saleDiscount.Add(code, 0);
				if (!_saleProfit.ContainsKey(code))
					_saleProfit.Add(code, 0);

				if (!_refundGrandTotal.ContainsKey(code))
					_refundGrandTotal.Add(code, 0);
				if (!_refundFare.ContainsKey(code))
					_refundFare.Add(code, 0);
				if (!_refundFeesTotal.ContainsKey(code))
					_refundFeesTotal.Add(code, 0);
				if (!_refundCancelFeeTotal.ContainsKey(code))
					_refundCancelFeeTotal.Add(code, 0);
				if (!_refundTotal.ContainsKey(code))
					_refundTotal.Add(code, 0);
				if (!_refundCommission.ContainsKey(code))
					_refundCommission.Add(code, 0);
				if (!_refundCommissionDiscount.ContainsKey(code))
					_refundCommissionDiscount.Add(code, 0);
				if (!_refundServiceFee.ContainsKey(code))
					_refundServiceFee.Add(code, 0);
				if (!_refundHandling.ContainsKey(code))
					_refundHandling.Add(code, 0);
				if (!_refundDiscount.ContainsKey(code))
					_refundDiscount.Add(code, 0);
				if (!_refundProfit.ContainsKey(code))
					_refundProfit.Add(code, 0);
			}

			var widths = ShowAviaHandling ? _hasHandlingWidths : _noHandlingWidths;

			var table = new Table(widths.Length)
			{
				Cellspacing = 0,
				Cellpadding = 2,
				Border = Rectangle.NO_BORDER,
				DefaultCellBorder = Rectangle.NO_BORDER,
				DefaultHorizontalAlignment = Element.ALIGN_RIGHT,
				Widths = widths,
				Width = 100,
				TableFitsPage = true
			};

			var totalCaption = ReportRes.AgentReport_TotalBalance + "     ";

			for (var i = 0; i < _currencies.Count; i++)
			{
				table.AddCell(new Cell(true) { Border = Rectangle.NO_BORDER });
				table.AddCell(new Cell(true) { Border = Rectangle.NO_BORDER });

				if (i == _currencies.Count - 1)
					table.DefaultCellBorder = Rectangle.BOTTOM_BORDER;

				var code = _currencies[i];

				var text = totalCaption + code;
				totalCaption = string.Empty;

				var cell = AddCell(table, text, _cellFontBold);
				cell.Colspan = 2;
				cell.BorderWidth = 1.5f;

				table.AddCell(new Cell(true));

				cell = AddCell(table, (_saleFare[code] + _refundFare[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				cell = AddCell(table, (_saleFeesTotal[code] + _refundFeesTotal[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				cell = AddCell(table, (_refundCancelFeeTotal[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				cell = AddCell(table, (_saleTotal[code] + _refundTotal[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				cell = AddCell(table, (_saleCommission[code] + _refundCommission[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				if (ShowAviaHandling)
				{
					cell = AddCell(table, (_saleCommissionDiscount[code] + _refundCommissionDiscount[code]).ToMoneyString(), _cellFontBoldItalic);
					cell.BorderWidth = 1.5f;
				}

				cell = AddCell(table, (_saleServiceFee[code] + _refundServiceFee[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				if (ShowAviaHandling)
				{
					cell = AddCell(table, (_saleHandling[code] + _refundHandling[code]).ToMoneyString(), _cellFontBoldItalic);
					cell.BorderWidth = 1.5f;
				}

				cell = AddCell(table, (_saleDiscount[code] + _refundDiscount[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				cell = AddCell(table, (_saleProfit[code] + _refundProfit[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				cell = AddCell(table, (_saleGrandTotal[code] + _refundGrandTotal[code]).ToMoneyString(), _cellFontBoldItalic);
				cell.BorderWidth = 1.5f;

				table.AddCell(new Cell(true) { Border = Rectangle.NO_BORDER });
				table.AddCell(new Cell(true) { Border = Rectangle.NO_BORDER });
			}

			_document.Add(table);
		}

		private static readonly float[] _noHandlingWidths =
		{
			11.5f, 11f, 11f, 10f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 3f, 7f
		};

		private static readonly float[] _hasHandlingWidths =
		{
			11.5f, 11f, 11f, 10f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 5.5f, 3f, 7f
		};

		private PdfPTable GetDocumentsTable(IEnumerable<Product> products, string title)
		{
			var widths = ShowAviaHandling ? _hasHandlingWidths : _noHandlingWidths;

			var table = new PdfPTable(widths.Length);

			table.SetWidths(widths);
			table.WidthPercentage = 100;
			table.DefaultCell.PaddingTop = 1;

			var titleCell = new PdfPCell(table.DefaultCell);
			titleCell.AddElement(new Chunk(title, _cellFontBold));
			titleCell.Colspan = ShowAviaHandling ? 18 : 16;
			titleCell.Border = Rectangle.NO_BORDER;
			table.AddCell(titleCell);

			table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

			table.AddCell(_cellFontBold, ReportRes.AgentReport_Document);

			table.AddCell(_cellFontBold, ReportRes.Common_Itinerary);
			table.AddCell(_cellFontBold, DomainRes.Common_PassengerName);
			table.AddCell(_cellFontBold, DomainRes.Common_Customer);
			table.AddCell(_cellFontBold, DomainRes.Common_Fare);
			table.AddCell(_cellFontBold, DomainRes.Common_FeesTotal);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_CancelFee);
			table.AddCell(_cellFontBold, DomainRes.Product_Total);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_Commission);
			if (ShowAviaHandling)
				table.AddCell(_cellFontBold, ReportRes.AgentReport_CommissionDiscount);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_ServiceFee);
			if (ShowAviaHandling)
				table.AddCell(_cellFontBold, ReportRes.AgentReport_Handling);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_Discount);
			//table.AddCell(_cellFontBold, DomainRes.AviaDocument_Profit);
			table.AddCell(_cellFontBold, DomainRes.Common_TicketingIataOffice);
			table.AddCell(_cellFontBold, DomainRes.Product_GrandTotal);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_PaymentType_Short);
			table.AddCell(_cellFontBold, ReportRes.AgentReport_OrderDue);

			table.HeaderRows = 2;

			var invalidCellFont = new Font(_cellFont);
			invalidCellFont.SetColor(255, 0, 0);

			foreach (var product in products)
			{
				var type = product.Type.ToString();

				var ticket = product as AviaTicket;
				var refund = product as AviaRefund;
				var mco = product as AviaMco;

				if (ticket != null && ticket.ReissueFor != null || mco != null && mco.ReissueFor != null)
					type = ReportRes.AgentReport_Reissue;

				var itinerary = new StringBuilder();
				AviaTicket tkt = null;

				if (ticket != null)
					tkt = ticket;
				else if (refund != null && refund.RefundedDocument != null && refund.RefundedDocument.Type == ProductType.AviaTicket)
					tkt = (AviaTicket) refund.RefundedDocument;

				if (tkt != null)
				{
					itinerary.Append(tkt.Itinerary);

					const string dateFormat = "dd.MM.yy";

					if (tkt.Departure.HasValue)
					{
						var firstDeparture = tkt.Departure.Value.Date;
						itinerary.AppendFormat(" {0}", firstDeparture.ToString(dateFormat));

						if (tkt.LastDeparture.HasValue)
						{
							var lastDepature = tkt.LastDeparture.Value.Date;

							if (firstDeparture != lastDepature)
								itinerary.AppendFormat("-{0}", lastDepature.ToString(dateFormat));
						}
					}
				}

				table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

				table.DefaultCell.BackgroundColor = product.RequiresProcessing ? new Color(0xF5, 0xF5, 0xF5) : Color.WHITE;

				table.AddCell(_cellFont, type + " " + product.Name);
				table.AddCell(_cellFont, itinerary.ToString());
				table.AddCell(_cellFont, product.PassengerName);

				if (product.Order != null)
					table.AddCell(_cellFont, product.Order.Customer.Name + "\n" + product.Order.Number);
				else if (product.Customer != null)
					table.AddCell(_cellFont, product.Customer.Name);
				else
					table.AddCell();

				table.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;

				if (product.IsVoid)
				{
					table.AddCell(_cellFont, ReportRes.AgentReport_Voided, Element.ALIGN_LEFT);

					table.AddCell();
					table.AddCell();
					table.AddCell();
					table.AddCell();
					table.AddCell();
					table.AddCell();
					table.AddCell();
					table.AddCell();
					table.AddCell();
					table.AddCell();

					if (ShowAviaHandling)
					{
						table.AddCell();
						table.AddCell();
					}
				}
				else
				{
					var sign = product.IsRefund ? -1 : 1;

					table.AddCell(_cellFont, _invalidCellFont, (product.EqualFare ?? product.Fare)*sign, DefaultCurrency, UseDefaultCurrencyForInput);
					table.AddCell(_cellFont, _invalidCellFont, product.FeesTotal*sign, DefaultCurrency, UseDefaultCurrencyForInput);
					table.AddCell(_cellFont, _invalidCellFont, product.IsRefund ? product.CancelFee : null, DefaultCurrency, UseDefaultCurrencyForInput);
					table.AddCell(_cellFont, _invalidCellFont, product.Total*sign, DefaultCurrency, UseDefaultCurrencyForInput);
					table.AddCell(_cellFont, _invalidCellFont, product.Commission*sign, DefaultCurrency, UseDefaultCurrencyForInput);

					if (ShowAviaHandling)
						table.AddCell(_cellFont, _invalidCellFont, product.CommissionDiscount*sign, DefaultCurrency, UseDefaultCurrencyForInput);

					table.AddCell(_cellFont, _invalidCellFont, (product is AviaRefund ? ((AviaRefund) product).ServiceTotal : product.ServiceFee)*sign, DefaultCurrency, UseDefaultCurrencyForInput);

					if (ShowAviaHandling)
						table.AddCell(_cellFont, _invalidCellFont, product.Handling*sign, DefaultCurrency, UseDefaultCurrencyForInput);

					table.AddCell(_cellFont, _invalidCellFont, product.Discount*sign, DefaultCurrency, UseDefaultCurrencyForInput);
					//table.AddCell(_cellFont, _invalidCellFont, product.Profit * sign, DefaultCurrency, UseDefaultCurrencyForInput);
					table.AddCell(_cellFont, product.TicketingIataOffice, Element.ALIGN_CENTER);
					table.AddCell(_cellFont, _invalidCellFont, product.GrandTotal * sign, DefaultCurrency, UseDefaultCurrencyForInput);
					table.AddCell(_cellFont, ToShortDisplayPaymentType(product.PaymentType), Element.ALIGN_CENTER);

					if (product.Order != null)
						table.AddCell(_cellFont, _invalidCellFont, product.Order.TotalDue, DefaultCurrency, UseDefaultCurrencyForInput);
					else
						table.AddCell();
				}
			}

			return table;
		}

		private static string ToShortDisplayPaymentType(PaymentType paymentType)
		{
			switch (paymentType)
			{
				case (PaymentType.CreditCard):
					return "CC";

				case (PaymentType.Check):
					return "CH";

				case (PaymentType.Cash):
					return "C";

				case (PaymentType.Exchange):
					return "E";

				case (PaymentType.Invoice):
					return "I";

				case (PaymentType.WithoutPayment):
					return "W";

				default:
					return paymentType.ToDisplayString().Substring(0, 1);
			}
		}

		private static void AddSum(List<string> currencies, Dictionary<string, decimal> sum, Money money)
		{
			if (money == null)
				return;

			var code = money.Currency.Code;

			if (!currencies.Contains(code))
				currencies.Add(code);

			if (!sum.ContainsKey(code))
				sum.Add(code, 0);

			sum[code] += money.Amount;
		}

		private static Cell AddCell(Table table, string text, Font font)
		{
			var cell = new Cell(new Chunk(text, font))
			{
				Leading = font.Size
			};

			table.AddCell(cell);

			return cell;
		}

		private List<Party> GetDocumentOwners()
		{
			var owners = new List<Party>();

			foreach (var document in Products)
				if (document.Owner != null && !owners.Contains(document.Owner))
					owners.Add(document.Owner);

			return owners;
		}

		private static readonly List<Product> _emptyDocumentList = new List<Product>();

		private Document _document;

		private static readonly Font _cellFont;
		private static readonly Font _invalidCellFont;
		private static readonly Font _cellFontBold;
		private static readonly Font _cellFontBoldItalic;

		private readonly List<Product> _ticketMcoDocuments = new List<Product>();
		private readonly List<Product> _refundDocuments = new List<Product>();

		private int _ticketCount;
		private int _refundCount;
		private int _mcoCount;
		private int _reissueCount;

		private readonly List<string> _currencies = new List<string>();
		private readonly List<string> _saleCurrencies = new List<string>();
		private readonly List<string> _refundCurrencies = new List<string>();

		private readonly Dictionary<string, decimal> _saleGrandTotal = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleFare = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleFeesTotal = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleTotal = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleCommission = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleCommissionDiscount = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleServiceFee = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleHandling = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleDiscount = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _saleProfit = new Dictionary<string, decimal>();

		private readonly Dictionary<string, decimal> _refundGrandTotal = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundFare = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundFeesTotal = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundCancelFeeTotal = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundTotal = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundCommission = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundCommissionDiscount = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundServiceFee = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundHandling = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundDiscount = new Dictionary<string, decimal>();
		private readonly Dictionary<string, decimal> _refundProfit = new Dictionary<string, decimal>();

		private readonly Dictionary<string, Dictionary<PaymentType, decimal>> _incomeTotal =
			new Dictionary<string, Dictionary<PaymentType, decimal>>();

		private readonly Dictionary<string, Dictionary<PaymentType, decimal>> _expenseTotal =
			new Dictionary<string, Dictionary<PaymentType, decimal>>();

		private readonly Dictionary<string, Dictionary<PaymentType, decimal>> _extraChargeTotal =
			new Dictionary<string, Dictionary<PaymentType, decimal>>();

		private readonly Font _defaultFont = PdfUtility.GetFont(PdfUtility.Times, 10, false, false);
		private readonly Font _defaultFontBold = PdfUtility.GetFont(PdfUtility.Times, 10, true, false);
	}
}