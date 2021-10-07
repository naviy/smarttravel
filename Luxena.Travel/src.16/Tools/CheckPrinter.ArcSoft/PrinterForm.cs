#define Local
//#define AtlasTour
//#define BSV



using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Net.Http;
using System.Text;

using Luxena;
using Luxena.Travel.Domain;

using NLog;


namespace client.NET
{

	public partial class PrinterForm : Form
	{
		public dynamic Driver;
		private bool _driverIsStarted;
		public string TravelODataUri;
		public string TravelUri;

		private readonly Logger _logger = LogManager.GetLogger("Printer");


		public PrinterForm()
		{
			InitializeComponent();


			ePort.SelectedIndex = 2;

#if Local
			TravelUri = "http://v15.bsv.travel.luxena.com";
			//TravelUri = "http://travel15";
			eOrderNumber.Text = "O.15-00794";
#endif

#if AtlasTour
			TravelUri = "http://v15.atlastour.travel.luxena.com";
			eModel.SelectedIndex = 5;
#endif

#if BSV
			TravelUri = "http://v15.bsv.travel.luxena.com";
			eModel.SelectedIndex = 1;
			ePort.SelectedIndex = 3;
#endif

			TravelODataUri = TravelUri + "/odata.usecalculated";


			eCheckType2.SelectedIndex = 0;
			ePayType2.SelectedIndex = 0;

			eBaudRate.SelectedIndex = 0;
			eCheckType.SelectedIndex = 0;
			ePayType.SelectedIndex = 0;
			ePeriodicRepType.SelectedIndex = 0;
			eReportType.SelectedIndex = 0;
			eRow.SelectedIndex = 0;
			eTaxtype.SelectedIndex = 0;

			var tabs = tabControl1.TabPages;
			tabs.Remove(tabPage1);
			tabs.Remove(tabPage2);
			tabs.Remove(tabPage4);
			tabs.Remove(tabPage5);
			tabs.Remove(tabPage6);
		}


		private bool Do(string operationTitle, Func<bool> action, bool useThrow = false, string successText = null)
		{
			var now = DateTime.Now.ToString("HH:mm:ss.f");

			eStatus.BackColor = Color.AliceBlue;
			eStatus.Text = now + " ...";

			var log = new StringWrapper();
			log *= "";

			var success = false;

			try
			{
				log *= "[" + now + "] " + operationTitle;
				_logger.Info(operationTitle);

				success = action();

				if (success)
				{
					log *= "Команда выполнена успешно";
				}
				else
				{
					log *= "ERROR. Ошибка выполнения команды";
					log *= "---------------------------------------------------";
					log *= "LastError = " + Driver.LastError;
					log *= "LastErrorText = " + Driver.LastErrorText;
					log *= "---------------------------------------------------";
					log *= "LastErrorEx = " + Driver.LastErrorEx;
					log *= "LastErrorExText = " + Driver.LastErrorExText;
					log *= "---------------------------------------------------";
					log *= "Номер чека = " + Driver.LastReceiptNum;
					log *= "Номер смены = " + Driver.SmenNum;
					_logger.Error(log);
				}

			}
			catch (Exception ex)
			{
				_logger.Error(ex);
				log *= "EXCEPTION";
				log *= ex.Message;
				log *= ex.StackTrace;
			}

			txtLog.AppendText(log);

			eStatus.BackColor = success ? Color.LightGreen : Color.Pink;
			var now2 = DateTime.Now.ToString("HH:mm:ss.f");
			eStatus.Text = (now == now2 ? now : now + " ... " + now2) + "    " +
				(success ? successText ?? operationTitle : operationTitle);

			if (!success && useThrow)
				throw new Exception("");

			return success;
		}

		void Log(string msg)
		{
			txtLog.AppendText("[" + DateTime.Now.ToString("HH:mm:ss:f") + "] ");
			txtLog.AppendText(msg);
			txtLog.AppendText("\r\n");
		}

		void Log(string fmt, params string[] args)
		{
			txtLog.AppendText("[" + DateTime.Now.ToString("HH:mm:ss:f") + "] ");
			txtLog.AppendText(string.Format(fmt, args));
		}


		#region Events from Demo

		private void cmdCreateDriver_Click(object sender, EventArgs e)
		{
			Do("Создание драйвера", () =>
			{
				var driverType = Type.GetTypeFromProgID("ArtSoft.FiscalPrinter");
				Driver = Activator.CreateInstance(driverType);
				return true;
			});
		}

		private void cmdConnect_Click(object sender, EventArgs e)
		{
			OpenDriver();
		}

		private void cmdDisconnect_Click(object sender, EventArgs e)
		{
			Do("Disconnect", () => Driver.closePort());
		}

		private void cmdZReport_Click(object sender, EventArgs e)
		{
			if (!ShowQuestion("Вы действительно хотите распечатать Z-отчёт?")) return;

			if (!OpenDriver()) return;
			Do("ZReport", () => Driver.printZReport(), successText: "Распечатан Z-отчёт");
		}

		private void cmdXReport_Click(object sender, EventArgs e)
		{
			if (!ShowQuestion("Вы действительно хотите распечатать X-отчёт?")) return;

			if (!OpenDriver()) return;
			Do("XReport", () => Driver.printXReport(), successText: "Распечатан X-отчёт");
		}

		private static bool ShowQuestion(string text)
		{
			return MessageBox.Show(text, null, MessageBoxButtons.YesNo) == DialogResult.Yes;
		}

		private void cmdOpenCheck_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("Открытие чека", () =>
			{
				var suc2 = Driver.beginFiscalReceipt(eCheckType.SelectedIndex, eCashierName.Text);
				if (suc2) return true;
				Driver.printRecVoid();
				return Driver.beginFiscalReceipt(eCheckType.SelectedIndex, eCashierName.Text);
			});
		}

		private void cmdSale_Click(object sender, EventArgs e)
		{
			Do("Sale", () => Driver.printRecItem(
				eName.Text,
				ePrice.Text.As().Double,
				eQty.Text.As().Double,
				eTaxtype.SelectedIndex + 1,
				eDiscount.Text.As().Double,
				eDiscountPr.Text.As().Double
			));
		}

		private void cmdComment_Click(object sender, EventArgs e)
		{
			Do("Comment", () => Driver.printText(eComment.Text));
		}

		private void cmdDisc_Click(object sender, EventArgs e)
		{
			Do("TotalDiscount", () => Driver.printRecSubtotalAdjustment(
				eTotalDiscount.Text.As().Double,
				eTotalDiscountPc.Text.As().Double
			));
		}

		private void cmdTotal_Click(object sender, EventArgs e)
		{
			Do("Total", () => Driver.printRecTotal(
				eSum.Text.As().Double,
				ePayType.SelectedIndex
			));
		}

		private void cmdCloseCheck_Click(object sender, EventArgs e)
		{
			Do("CloseCheck", () => Driver.endFiscalReceipt());
		}

		private void cmdCancelCheck_Click(object sender, EventArgs e)
		{
			Do("CancelCheck", () => Driver.printRecVoid());
		}

		private void cmdNullCheck_Click(object sender, EventArgs e)
		{
			Do("NullCheck", () => Driver.printNullReceipt());
		}

		private void cmdCopyCheck_Click(object sender, EventArgs e)
		{
			if (!ShowQuestion("Распечатать копию чека?")) return;

			Do("CopyCheck", () => Driver.printDuplicateReceipt(), successText: "Распечатана копия чека");
		}

		private void cmdOpenDrawer_Click(object sender, EventArgs e)
		{
			Do("OpenDrawer", () => Driver.openCashDrawer());
		}

		private void cmdInOut_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("InOut", () => Driver.printRecCash(eInOutSum.Text.As().Double));
		}

		private void cmdReports_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("Reports", () => Driver.printReport(eReportType.SelectedIndex));
		}

		private void cmdPeriodicReports_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("PeriodicReports", () => Driver.printPeriodicReport(ePeriodicRepType.SelectedIndex, eBegin.Text, eEnd.Text));
		}

		private void cmdOpenNonFiscal_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("OpenNonFiscal", () => Driver.beginNonFiscal());
		}

		private void cmdCloseNonFiscal_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("CloseNonFiscal", () => Driver.endNonFiscal());
		}

		private void cmdPrintNonFiscal_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("PrintNonFiscal", () => Driver.printNonFiscalText(txtNonFiscalText.Text));
		}

		private void cmdDisplay_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("Display", () => Driver.displayText(
				eDispName1.Text,
				eSumDisp.Text.As().Double
			));
		}

		private void cmdDisplay2_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("Display2", () => Driver.displayTextAt(eRow.SelectedIndex + 1, eDispName2.Text));
		}

		private void cmdDisplayClear_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("DisplayClear", () => Driver.clearText());
		}

		#endregion


		public string AuthorizationCookie;
		public string UserName;

		public bool LoadAuthorization(string userName, string password)
		{
			var url = TravelUri + "/login";

			var parameters = new Dictionary<string, string>
			{
				{ "UserName", userName },
				{ "Password", password },
				{ "JsonResult", "true" },
			};

			var client = new HttpClient();

			var request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Content = new FormUrlEncodedContent(parameters);

			var response = client.SendAsync(request).Result;//.ConfigureAwait(false);

			var responseBody = response.Content.ReadAsByteArrayAsync().Result;

			var respBodyEncoded = Encoding.UTF8.GetString(responseBody);

			if (respBodyEncoded != "true")
				return false;

			AuthorizationCookie = response.Headers.By("Set-Cookie").One();
			UserName = userName;

			return true;
		}


		private bool OpenDriver()
		{
			if (!_driverIsStarted)
			{
				_driverIsStarted = Do("Подключение к регистратору", () =>
				{
					if (Driver == null)
					{
						var driverType = Type.GetTypeFromProgID("ArtSoft.FiscalPrinter");
						Driver = Activator.CreateInstance(driverType);
					}

					Driver.start(eModel.SelectedIndex);
					//Alert("start openPort");
					var success = (bool)Driver.openPort((string)ePort.SelectedItem, eBaudRate.SelectedIndex);
					//Alert("end openPort");
					return success;
				});

				return _driverIsStarted;
			}

			return true;
		}

		void Alert(string msg)
		{
			MessageBox.Show(msg, null, MessageBoxButtons.OK);
		}


		private Action _printOrder;

		private void cmdPrintOrder_Click(object sender, EventArgs e)
		{
			_printOrder?.Invoke();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			txtLog.Clear();
		}

		private void cmdCancelCheck2_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("CancelCheck", () => Driver.printRecVoid(), successText: "Чек отменён");
		}

		private void cmdNullCheck2_Click(object sender, EventArgs e)
		{
			if (!ShowQuestion("Распечатать нулевой чек?")) return;

			if (!OpenDriver()) return;
			Do("Нулевой чек", () => Driver.printNullReceipt(), successText: "Распечатан нулевой чек");
		}

		private void eOrderNumber_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
				cmdLoadOrder_Click(sender, e);
		}

		private void cmdInOut2_Click(object sender, EventArgs e)
		{
			if (!OpenDriver()) return;
			Do("InOut", () => Driver.printRecCash(eInOutSum2.Text.As().Double), successText: "Совершён внос/вынос");
		}

		private void PrinterForm_Shown(object sender, EventArgs e)
		{
#if Local
			if (!LoadAuthorization("denis.sahoshko", "idkfa"))
			{
				MessageBox.Show("Not authorized!", "Error", MessageBoxButtons.OK);
			}
#else
			var loginForm = new LoginForm(this);
			if (loginForm.ShowDialog() != DialogResult.OK)
				Application.Exit();
#endif
		}

		private void cmdAdmin_Click(object sender, EventArgs e)
		{
			var tabs = tabControl1.TabPages;
			tabs.Add(tabPage1);
			tabs.Add(tabPage2);
			tabs.Add(tabPage4);
			tabs.Add(tabPage5);
			tabs.Add(tabPage6);
		}



		private void cmdLoadOrder_Click(object sender, EventArgs e)
		{
			if (!cmdLoadOrder.Enabled) return;

			cmdLoadOrder.Enabled = false;
			cmdPrintOrder.Enabled = false;
			Cursor = Cursors.WaitCursor;

			try
			{

				if (AuthorizationCookie.No())
					return;

				var db = new Default.Container(new Uri(TravelODataUri));

				db.BuildingRequest += (o, ee) => ee.Headers.Add("Cookie", AuthorizationCookie);

				var personId = (from a in db.Users where a.Name == UserName select new { a.PersonId }).One().As(a => a.PersonId);
				if (personId.No())
				{
					Log(eOrderInfo.Text = "Пользователь " + UserName + " не найден.");
					return;
				}


				var orderNumber = eOrderNumber.Text;
				if (orderNumber.No())
					throw new Exception("Необходимо указать номер заказа.");

				var orders = (
					from a in db.Orders
					where a.Number == orderNumber
					select new
					{
						a.Id,
						a.Number,
						a.IssueDate,
						Customer = a.Customer.Name,
						Owner = a.Owner.Name,
						a.Total,
					}
				).ToList();

				if (orders.No())
				{
					Log(eOrderInfo.Text = "Заказ с номером " + orderNumber + " не найден.");
					return;
				}

				var order = orders.One();

				eOrderInfo.Text = $"Номер: {order.Number}\r\nДата выпуска: {order.IssueDate.ToString("dd.MM.yyy")}\r\nЗаказчик: {order.Customer}\r\nОтветственный: {order.Owner}\r\nСумма заказа: {order.Total.Amount:n2}\r\n\r\n";

#if AtlasTour
				var productTypeWithServiceFee = new[] { ProductType.Pasteboard, };
#endif

				var checkType = eCheckType2.SelectedIndex;
				var useRefund = checkType == 1;
				var loadAllItems = eLoadAllOrderItems.Checked;
				var orderItemsQuery =
					from a in db.OrderItems
					where
						a.OrderId == order.Id &&
						(loadAllItems || a.Product == null || useRefund == a.Product.IsRefund)
					orderby a.Position
					select new
					{
						a.Id,
						a.Position,
						ProductId = a.Product.Id,
						IsFullDocument = a.LinkType == OrderItemLinkType.FullDocument,
						IsProductData = a.LinkType == OrderItemLinkType.ProductData,
						IsServiceFee = a.LinkType == OrderItemLinkType.ServiceFee,
						a.Text,
						ProductName = a.CheckNameUA,
						ProductType = a.Product.Type,
						a.Product.IsReservation,
						a.Quantity,
						Total = a.Total.Amount,
						GrandTotal = a.GrandTotal.Amount,
						ProductGrandTotal = a.Product.GrandTotal.Amount,
						IsMenual = a.Product.Originator == GdsOriginator.Unknown,
						TaxRate = a.LinkType == OrderItemLinkType.ServiceFee ? a.Product.TaxRateOfServiceFee : a.Product.TaxRateOfProduct,
					};

				Log(orderItemsQuery.ToString());
				var orderItems0 = orderItemsQuery.ToList();

				var orderItems = (
					from a in orderItems0
#if AtlasTour
					where !a.IsServiceFee
						|| productTypeWithServiceFee.Contains(a.ProductType) && a.TaxMode != CheckItemTaxMode.WithoutVAT
						|| a.TaxMode == CheckItemTaxMode.WithoutVATAndServiceFeeVAT

					let taxType = !a.IsReservation && !a.IsServiceFee || (a.ProductType == ProductType.AviaTicket && a.IsMenual) ? 5 : 1,

					let price = Math.Abs(
						a.IsServiceFee || a.TaxMode == CheckItemTaxMode.WithoutVATAndServiceFeeVAT || productTypeWithServiceFee.Contains(a.ProductType)
							? a.GrandTotal ?? 0
							: a.ProductGrandTotal ?? a.GrandTotal ?? 0
					),
#else
					where a.TaxRate != TaxRate.None

					let taxType = a.TaxRate == TaxRate.Default
						? !a.IsServiceFee ? 5 : 1
						: (int)a.TaxRate
					let price = Math.Abs(
						a.IsProductData &&
						orderItems0.Any(b => b.ProductId == a.ProductId && b.IsServiceFee && b.TaxRate == TaxRate.None)
							? a.ProductGrandTotal ?? a.GrandTotal ?? 0  // вместе с сервисным сбором
							: a.GrandTotal ?? 0 // Без сервисного сбора
					)
#endif

					select new
					{
						a.Id,
						a.Position,
						a.ProductType,
						ProductName = a.ProductName.As(b => b.Replace("(возврат)", "")),
						a.Quantity,
						Price = price,
						Taxtype = taxType,
						Vat = taxType == 1 ? Math.Round(price * 20m / (100m + 20m), 2) : 0,
					}
				).ToList();


				if (order.Total.Amount != null)
					eTotal2.Text = orderItems.Sum(a => a.Price).ToString("n2");

				var sb = new StringWrapper();

				foreach (var a in orderItems)
				{
					sb.AppendFormat("#{0} {1}   {2:n2}   {3}\r\n",
						a.Position, a.ProductName, a.Price,
						a.Taxtype == 1 ? $"А (НДС 20% = {a.Vat:n2} UAH)" : a.Taxtype == 2 ? "Б" : a.Taxtype == 5 ? "Д (Без НДС)" : a.Taxtype.ToString()
					);
				}

				var orderItemsText = sb.ToString();
				eOrderInfo.Text += orderItemsText;

				_printOrder = () =>
				{
					cmdLoadOrder.Enabled = false;
					cmdPrintOrder.Enabled = false;
					Cursor = Cursors.WaitCursor;

					var opened = false;
					var printVisible = true;

					try
					{
						var payAmount = eTotal2.Text.As().Decimal;
						var payType = ePayType2.SelectedIndex;

						Log("Печать чека для заказа " + orderNumber);

#if !Local
						if (!OpenDriver()) return;

						opened = Do("Открытие чека", () =>
						{
							var suc2 = Driver.beginFiscalReceipt(checkType, eCashierName2.Text);
							if (suc2) return true;

							Driver.printRecVoid();

							return Driver.beginFiscalReceipt(checkType, eCashierName2.Text);
						}, useThrow: true);

						if (!opened) return;

						var success = true;

						orderItems.ForEach((a, i) =>
						{
							if (!success) return;

							success = Do(
								"Печать позиции #" + i,
								() => Driver.printRecItem(a.ProductName, a.Price, a.Quantity, a.Taxtype, 0, 0),
								useThrow: true
							);
						});

						if (!success) return;

						success = Do("Total", () => Driver.printRecTotal((double)payAmount, payType), useThrow: true);
						if (!success) return;

						success = Do("Закрытие чека", () => Driver.endFiscalReceipt(), useThrow: true, successText: "Распечатан фискальный чек");
						if (!success) return;

						var checkNumber = Driver == null ? null : ((int)Driver.LastReceiptNum).ToString();
#else
						var checkNumber = "test";
#endif

						db.AddToOrderChecks(new OrderCheck
						{
							Id = "",
							//Id = Guid.NewGuid().ToString("N"),
							Date = DateTimeOffset.Now,
							OrderId = order.Id,
							PersonId = personId,
							CheckType = checkType == 0 ? CheckType.Sale : CheckType.Return,
							CheckNumber = checkNumber,
							Currency = "UAH",
							CheckAmount = orderItems.Sum(a => a.Price),
							CheckVat = orderItems.Sum(a => a.Vat),
							PayAmount = payAmount,
							PaymentType = (CheckPaymentType)payType,
							Description = orderItemsText,
						});

						db.SaveChanges();

						eOrderNumber.Text = null;
						eTotal2.Text = null;
						eOrderInfo.Text = null;

						_printOrder = null;
						printVisible = false;
					}
					catch (Exception ex)
					{
						cmdPrintOrder.Enabled = true;

						if (ex.Message.Yes())
						{
							var msg = ex.FullMessage();
							MessageBox.Show(msg, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
							Log(msg + "\r\n" + ex.StackTrace);
						}

						if (opened)
							Driver.printRecVoid();
					}
					finally
					{
						cmdPrintOrder.Enabled = printVisible;
						cmdLoadOrder.Enabled = true;
						Cursor = Cursors.Default;
					}
				};

				cmdPrintOrder.Enabled = true;
			}
			catch (Exception ex)
			{
				if (ex.Message != "")
				{
					var msg = ex.FullMessage();
					MessageBox.Show(msg, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					Log(msg + "\r\n" + ex.StackTrace);
				}
			}
			finally
			{
				cmdLoadOrder.Enabled = true;
				Cursor = Cursors.Default;
			}

		}

	}

}