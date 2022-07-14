using System;
using System.Collections;
using System.Collections.Generic;

using Ext;
using Ext.form;
using Ext.menu;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using Item = Ext.menu.Item;
using OrderClass = LxnTravel.Accounting.Order.Order;




namespace Luxena.Travel
{



	//===g






	public class OrderViewForm : BaseClassViewForm
	{

		//---g



		static OrderViewForm()
		{
			FormsRegistry.RegisterView(ClassNames.Order, ViewObject);
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string)id, delegate (string tabId) { return new OrderViewForm(tabId, id, type); });
		}

		public OrderViewForm(string tabId, object id, string type) : base(tabId, id, type)
		{
		}



		//---g



		private OrderDto Order
		{
			get { return (OrderDto)Instance; }
		}



		//---g



		protected override void initComponent()
		{

			cls = "order-view";
			layout = null;

			base.initComponent();


			_titleLabel = new Label(new LabelConfig()
				.cls("title")
				.ToDictionary());


			_orderProperties = GetOrderProperiesControl();

			_orderFinances = GetOrderFinancesControl();

			_orderItems = GetOrderItemsControl();

			_payments = GetPaymentsControl();

			_transfers = GetTransfersControl();


			Panel contentPanel = new Panel(new PanelConfig()
				.items(new object[]
				{
					_titleLabel,
					_orderProperties,
					_orderFinances,
					_orderItems,
					_payments,
					_transfers
				})
				.border(false)
				.cls("content-panel")
				.ToDictionary());

			add(contentPanel);

		}



		protected override void OnLoad()
		{

			setTitle(Order.Number);

			RefreshOrderTitle();

			_orderProperties.LoadInstance(Order);
			_orderFinances.LoadInstance(Order);
			_orderItems.LoadCollection(Order.Items);
			_payments.LoadCollection(Order.Payments);
			_transfers.LoadCollection(Order.Transfers);


			if (!Order.CanCreateTransfer)
				_createInternalTransferItem.hide();

		}



		private void RefreshOrderTitle()
		{

			_voidButton.setText(Order.IsVoid ? Res.Restore_Action.ToLowerCase() : Res.Void_Action.ToLowerCase());

			string str = DomainRes.Order;

			if (Order.IsVoid)
				str += " (" + DomainRes.Common_IsVoid.ToLowerCase() + ")";

			_titleLabel.setText(str);

			_titleLabel.removeClass("open-unpaid");
			_titleLabel.removeClass("open-paid");
			_titleLabel.removeClass("voided");

			string labelClass = null;

			if (Order.IsVoid)
				labelClass = "voided";
			else if (Order.DeliveryBalance < 0)
				labelClass = "open-unpaid";
			else if (Order.Total != null && Order.Total.Amount != 0 && Order.TotalDue != null && Order.TotalDue.Amount != 0)
				labelClass = "open-unpaid";
			else if (Order.DeliveryBalance > 0)
				labelClass = "open-paid";

			if (!string.IsNullOrEmpty(labelClass))
				_titleLabel.addClass(labelClass);

			_registerPaymentButton.setDisabled(Order.IsVoid);

		}



		protected override void GetInstance()
		{
			OrderService.GetOrder(Id, Load, delegate { Tabs.Close(this); });
		}



		protected override void OnInitToolBar(ArrayList toolbarItems)
		{

			_issueInvoiceButton = new Button(new ButtonConfig()
				.text(Res.Order_IssueInvoice_Action.ToLowerCase())
				.handler(new AnonymousDelegate(IssueInvoice))
				.ToDictionary());

			_issueReceiptButton = new Button(new ButtonConfig()
				.text(Res.Order_IssueReceipt_Action.ToLowerCase())
				.handler(new AnonymousDelegate(IssueReceipt))
				.ToDictionary());

			_issueConsignmentButton = new Button(new ButtonConfig()
				.text(Res.IssueConsignmentAction.ToLowerCase())
				.handler(new AnonymousDelegate(IssueConsignment))
				.ToDictionary());

			_issueCompletionCertificateButton = new Button(new ButtonConfig()
				.text(Res.Order_IssueCompletionCertificate_Action.ToLowerCase())
				.handler(new AnonymousDelegate(IssueCompletionCertificate))
				.ToDictionary());

			Item createCashInOrder = new Item(new ItemConfig()
				.text(Res.Payment_CreateCashInOrder_Action)
				.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.CashInOrderPayment); }))
				.ToDictionary());

			Item createCashOutOrder = new Item(new ItemConfig()
				.text(Res.Payment_CreateCashOutOrder_Action)
				.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.CashOutOrderPayment); }))
				.ToDictionary());

			Item createNonCashPayment = new Item(new ItemConfig()
				.text(Res.Payment_CreateNonCashPayment_Action)
				.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.WireTransfer); }))
				.ToDictionary());

			Item createCheckPayment = new Item(new ItemConfig()
				.text(Res.Payment_CreateCheckPayment_Action)
				.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.CheckPayment); }))
				.ToDictionary());

			Item createCreditCardPayment = new Item(new ItemConfig()
				.text(Res.Payment_CreateElecronicPayment_Action)
				.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.ElectronicPayment); }))
				.ToDictionary());

			_createInternalTransferItem = new Item(new ItemConfig()
				.text(Res.Payment_CreateInternalTransfer_Action)
				.handler(new AnonymousDelegate(delegate { CreateInternalTransfer(); }))
				.ToDictionary());

			_registerPaymentButton = new Button(new ButtonConfig()
				.text(Res.Payment_CreatePayment_Action)
				.menu(new Menu(new MenuConfig()
					.items(new object[]
					{
						createCashInOrder,
						createCashOutOrder,
						createNonCashPayment,
						createCheckPayment,
						createCreditCardPayment,
						_createInternalTransferItem
					})
					.ToDictionary()))
				.ToDictionary());

			_addTaskButton = new Button(new ButtonConfig()
				.text(Res.Task_Add)
				.handler(new AnonymousDelegate(AddTask))
				.ToDictionary());

			_voidButton = new Button(new ButtonConfig()
				.handler(new AnonymousDelegate(delegate { SetIsVoid(!Order.IsVoid); }))
				.ToDictionary());

			int index = 0;

			toolbarItems.Insert(index++, _issueInvoiceButton);
			toolbarItems.Insert(index++, _issueReceiptButton);
			toolbarItems.Insert(index++, _registerPaymentButton);
			toolbarItems.Insert(index++, _issueConsignmentButton);
			toolbarItems.Insert(index++, _issueCompletionCertificateButton);
			toolbarItems.Insert(index++, _addTaskButton);
			toolbarItems.Insert(index++, "-");
			toolbarItems.Insert(index, _voidButton);
		}



		private PropertyListControl GetOrderProperiesControl()
		{

			PropertyListControlConfig config = new PropertyListControlConfig()

				.SetListItems(new PropertyItem[]
				{

					new PropertyItem(DomainRes.Common_Number, "Number"),
					new PropertyItem(DomainRes.Order_IssueDate, "IssueDate").SetPropertyType(PropertyType.Date),
					new PropertyItem(DomainRes.Common_Customer, "Customer").SetPropertyType(PropertyType.ObjectInfo),
					new PropertyItem(DomainRes.Common_BillTo, "BillTo").SetPropertyType(PropertyType.ObjectInfo).SetHideIsEmpty(true),
					new PropertyItem(DomainRes.Common_ShipTo, "ShipTo").SetPropertyType(PropertyType.ObjectInfo).SetHideIsEmpty(true),
					new PropertyItem(DomainRes.Common_Intermediary, "Intermediary").SetPropertyType(PropertyType.ObjectInfo).SetHideIsEmpty(true),
					new PropertyItem(DomainRes.Common_AssignedTo, "AssignedTo"),
					new PropertyItem(DomainRes.Common_Owner, "Owner"),
					new PropertyItem("Банковский счёт", "BankAccount").SetPropertyType(PropertyType.ObjectInfo).SetHideIsEmpty(true),
					new PropertyItem(DomainRes.Order_IsPublic, "IsPublic").SetHideIsEmpty(true).SetRenderer(new PropertyItemRenderDelegate(
						delegate(PropertyItem propertyItem, object value, jQueryObject container)
						{
							container.Html((bool) value ? Res.OrderView_IsPublic : Res.OrderView_IsNotPublic);
						})
					),
					new PropertyItem("Разрешить добавление билетов<br/>в заказ даже в закрытом периоде", "AllowAddProductsInClosedPeriod").SetPropertyType(PropertyType.Bool).SetHideIsEmpty(true),
					new PropertyItem(DomainRes.Order_IsSubjectOfPaymentsControl, "IsSubjectOfPaymentsControl").SetPropertyType(PropertyType.Bool).SetHideIsEmpty(true),

					new PropertyItem("Списано бонусов", "BonusSpentAmount").SetPropertyType(PropertyType.Number).SetHideIsEmpty(true),
					new PropertyItem("Получатель бонусов", "BonusRecipient").SetPropertyType(PropertyType.ObjectInfo).SetHideIsEmpty(true),
					new PropertyItem("Дата начисления бонусов", "BonusDate").SetPropertyType(PropertyType.Date).SetHideIsEmpty(true),

					new PropertyItem(DomainRes.Common_Note, "Note").SetHideIsEmpty(true),
					new PropertyItem(DomainRes.Order_Invoices, "Invoices").SetHideIsEmpty(true)
						.SetRenderer(new PropertyItemRenderDelegate(
							delegate(PropertyItem propertyItem, object value, jQueryObject container)
							{
								RenderInvoices((InvoiceDto[]) value, container);
							})),
					new PropertyItem(DomainRes.Task_Caption_List, "Tasks").SetHideIsEmpty(true)
						.SetRenderer(new PropertyItemRenderDelegate(
							delegate(PropertyItem propertyItem, object value, jQueryObject container)
							{
								RenderTasks((TaskDto[]) value, container);
							}))
				})

				.SetCssClass("order-properties")

			;


			return new PropertyListControl(config);

		}



		private PropertyListControl GetOrderFinancesControl()
		{

			PropertyListControlConfig config = new PropertyListControlConfig()

				.SetListItems(new PropertyItem[]
				{

					new PropertyItem(DomainRes.Common_Discount, "Discount").SetPropertyType(PropertyType.Money),

					new PropertyItem(DomainRes.Common_Total, "Total").SetPropertyType(PropertyType.Money),

					new PropertyItem(DomainRes.Order_Vat, "Vat").SetPropertyType(PropertyType.Money),

					new PropertyItem(DomainRes.Order_Paid, "Paid").SetPropertyType(PropertyType.Money),

					new PropertyItem(DomainRes.Order_TotalDue, "TotalDue").SetPropertyType(PropertyType.Money)
						.SetRowCssClass(delegate(PropertyItem item, object value)
						{
							MoneyDto moneyDto = (MoneyDto) value;

							if (moneyDto != null && moneyDto.Amount != 0)
								return "unpaid-amount";

							return null;
						})
					,


					new PropertyItem(DomainRes.Order_DeliveryBalance, "DeliveryBalance")

						.SetPropertyType(PropertyType.Number)

						.SetRowCssClass(delegate(PropertyItem item, object value)
						{

							decimal balance = (decimal)value;


							if (balance < 0)
								return "text-red";


							MoneyDto totalDue = Order.TotalDue;


							if (Script.IsValue(totalDue) && totalDue.Amount > 0)
								return "text-orange";


							return balance > 0 ? "text-green" : null;

						})


						.SetRenderer(new PropertyItemRenderDelegate(delegate(PropertyItem propertyItem, object value, jQueryObject container)
						{

							decimal balance = (decimal)value;

							MoneyDto total = Order.Total;

							string currency = total != null ? total.Currency.Name : "";


							container.Html(balance.Format("N2") + " " + currency);

						})),

				})

				.SetCssClass("common-finances")
			;


			return new PropertyListControl(config);

		}



		private static PropertyGridControl GetOrderItemsControl()
		{

			PropertyItem[] propertyItems =
			{
				new PropertyItem(Res.OrderItem_Text, "Text").SetWidth(500).SetRenderer(new PropertyGridRenderDelegate(RenderItemText)),
				new PropertyItem(Res.Common_Quantity, "Quantity").SetPropertyType(PropertyType.Number).SetWidth(80).SetCssClass("center-align"),
				new PropertyItem(Res.Common_Price, "Price").SetPropertyType(PropertyType.Money).SetWidth(100).SetCssClass("right-align"),
				new PropertyItem(DomainRes.Common_Amount, "Total").SetPropertyType(PropertyType.Money).SetWidth(100).SetCssClass("right-align"),
				new PropertyItem(DomainRes.OrderItem_Consignment, "Consignment").SetPropertyType(PropertyType.ObjectInfo).SetCssClass("center-align")
			};


			PropertyGridControlConfig config = new PropertyGridControlConfig()
				.SetListItems(propertyItems)
				.SetUseListCountColumn(true)
				.SetCssClass("order-items")
				.SetGridTitle(Res.OrderItems);


			return new PropertyGridControl(config);

		}



		private static PropertyGridControl GetPaymentsControl()
		{

			PropertyItem[] propertyItems =
			{
				new PropertyItem(DomainRes.Payment, "Number").SetCssClass("center-align").SetRenderer(new PropertyGridRenderDelegate(
					delegate(object instance, object value, int index, jQueryObject container)
					{
						PaymentDto payment = (PaymentDto) instance;

						container.Append(ObjectLink.Render(payment.Id, payment.Number, GetPaymentClassName(payment)));
					})),
				new PropertyItem(DomainRes.Common_Date, "Date").SetPropertyType(PropertyType.Date).SetCssClass("center-align"),
				new PropertyItem(DomainRes.Payment_PostedOn, "PostedOn").SetPropertyType(PropertyType.Date).SetCssClass("center-align"),
				new PropertyItem(Res.Payment_DocumentNumber, "DocumentNumber").SetRenderer(new PropertyGridRenderDelegate(
					delegate(object instance, object value, int index, jQueryObject container)
					{
						PaymentDto payment = (PaymentDto) instance;

						if (payment.DocumentNumber == null)
							return;

						string text = null;
						switch (payment.PaymentForm)
						{
							case PaymentForm.WireTransfer:
								text = Res.WireTransfer_DocumentName_Format;
								break;

							case PaymentForm.Check:
								text = Res.CheckPayment_DocumentName_Format;
								break;

							case PaymentForm.CashInOrder:
								text = Res.CashInOrderPayment_DocumentName_Format;
								break;

							case PaymentForm.CashOutOrder:
								text = Res.CashOutOrderPayment_DocumentName_Format;
								break;

							case PaymentForm.Electronic:
								text = Res.ElectronicPayment_DocumentName_Format;
								break;
						}

						if (payment.PaymentForm != PaymentForm.Electronic)
							text = string.Format(text, payment.DocumentNumber);
						else
							text = string.Format(text, payment.PaymentSystem != null ?
								payment.PaymentSystem.Name + ", " : string.Empty, payment.DocumentNumber);

						if (payment.PaymentForm != PaymentForm.CashInOrder && payment.PaymentForm != PaymentForm.CashOutOrder)
							container.Append(text);
						else
						{
							jQueryObject div = jQuery.FromHtml("<div class='cashorder-link'></div>");
							jQueryObject link = jQuery.FromHtml(@"<span></span>").Html(text);

							GenericTwoArgDelegate func = delegate(object id, object number) { link.Click(delegate { ReportPrinter.GetCashOrder(id, (string) number); }); };

							func.Invoke(payment.Id, payment.DocumentNumber);

							container.Append(div.Append(link));
						}
					})),
				new PropertyItem(DomainRes.Payment_Payer, "Payer").SetPropertyType(PropertyType.ObjectInfo),
				new PropertyItem(DomainRes.Payment_RegisteredBy, "RegisteredBy").SetPropertyType(PropertyType.String).SetCssClass("center-align"),
				new PropertyItem(DomainRes.Common_Owner, "Owner").SetPropertyType(PropertyType.ObjectInfo),
				new PropertyItem("Банковский счёт", "BankAccount").SetPropertyType(PropertyType.ObjectInfo),
				new PropertyItem(DomainRes.Payment_Amount, "Amount").SetPropertyType(PropertyType.Money).SetCssClass("right-align")
			};

			PropertyGridControlConfig config = new PropertyGridControlConfig()
				.SetListItems(propertyItems)
				.SetUseListCountColumn(true)
				.SetCssClass("payments")
				.SetRowCssClass(GetPaymentCss)
				.SetGridTitle(Res.Payments);

			return new PropertyGridControl(config);
		}



		private static PropertyGridControl GetTransfersControl()
		{

			PropertyItem[] propertyItems =
			{
				new PropertyItem(DomainRes.Common_Number, "Transfer").SetPropertyType(PropertyType.ObjectInfo),
				new PropertyItem(DomainRes.Common_Date, "Date").SetPropertyType(PropertyType.Date),
				new PropertyItem(DomainRes.Party, "Party").SetPropertyType(PropertyType.ObjectInfo),
				new PropertyItem(DomainRes.Order, "Order").SetPropertyType(PropertyType.ObjectInfo),
				new PropertyItem(DomainRes.Common_Amount, "Amount").SetPropertyType(PropertyType.Number).SetCssClass("right-align")
			};

			PropertyGridControlConfig config = new PropertyGridControlConfig()
				.SetListItems(propertyItems)
				.SetCssClass("payments")
				.SetGridTitle(DomainRes.InternalTransfer_Caption_List);

			return new PropertyGridControl(config);

		}



		private static string GetPaymentClassName(PaymentDto payment)
		{

			switch (payment.PaymentForm)
			{
				case PaymentForm.CashInOrder:
					return ClassNames.CashInOrderPayment;

				case PaymentForm.CashOutOrder:
					return ClassNames.CashOutOrderPayment;

				case PaymentForm.Check:
					return ClassNames.CheckPayment;

				case PaymentForm.WireTransfer:
					return ClassNames.WireTransfer;

				case PaymentForm.Electronic:
					return ClassNames.ElectronicPayment;
			}


			return null;

		}



		private static string GetPaymentCss(object value)
		{

			PaymentDto dto = (PaymentDto)value;


			if (dto.IsVoid)
				return "textColor-gray";


			if (!Script.IsValue(dto.PostedOn))
				return "textColor-red";


			return null;

		}



		private static void RenderItemText(object instance, object value, int index, jQueryObject container)
		{

			string text = (string)value;
			OrderItemDto item = (OrderItemDto)instance;

			Reference source = null;


			if (item.Product != null)
				source = Reference.Copy(item.Product);


			if (source != null && Script.IsValue(source.Name))
			{
				bool hasNumber = source.Name.Match(new RegularExpression(@"^\d{3}-\d{5,15}$")) != null;

				if (!hasNumber || text.IndexOf(source.Name) == -1)
				{
					if (!hasNumber)
						source.Name = Res.OrderViewForm_Reservation;

					int endPos = text.IndexOf("\n") != -1 ? text.IndexOf("\n") - 1 : text.Length;
					text = text.Insert(endPos, string.Format(" ({0})", ObjectLink.RenderInfo(source)));
				}
				else
					text = text.Replace(source.Name, ObjectLink.RenderInfo(source));
			}


			container.Append(text);

		}



		private void RenderTasks(TaskDto[] tasks, jQueryObject container)
		{

			OrderClass.Tasks.RenderTo(container.GetElement(0), new Dictionary(
				"tasks", tasks,
				"taskClick", new Action<TaskDto>(delegate (TaskDto task)
				{
					TaskStatus newStatus = task.Status == TaskStatus.Closed ? TaskStatus.Open : TaskStatus.Closed;

					TaskService.ChangeStatus(new object[] { task.Id }, newStatus, null,
						delegate (object result)
						{
							TaskDto dto = (TaskDto)result;

							dto.Type = ClassNames.Task;

							for (int i = 0; i < tasks.Length; ++i)
								if (tasks[i].Id == task.Id)
								{
									tasks[i] = dto;
									_orderProperties.LoadInstance(Order);
									MessageRegister.Info(DomainRes.Task_Caption_List, string.Format(Res.TaskStatusChanged_Msg, dto.Number,
										EnumUtility.Localize(typeof(TaskStatus), dto.Status, typeof(DomainRes)).ToLowerCase()));
									break;
								}
						},
						null);
				})
			));

		}



		private void RenderInvoices(InvoiceDto[] invoices, jQueryObject container)
		{

			if (invoices == null || invoices.Length == 0)
				return;


			foreach (InvoiceDto invoice in invoices)
			{

				container.Append(

					jQuery.FromHtml("<div class='issued-invoice'></div>")

					.Data("invoice", invoice)

					.Append(
						jQuery.FromHtml(@"<span class='invoice-link'></span>")
						.Html(invoice.Number)
						.Click(delegate (jQueryEvent e)
						{
							InvoiceDto dto = (InvoiceDto)jQuery.FromElement(e.Target).Parent().GetDataValue("invoice");

							ReportPrinter.GetOrderDocument(dto.Id, dto.Number, dto.Type, dto.FileExtension);
						})
					)

					.Append(string.Format(
						Res.OrderView_IssuedInvoice_Html, invoice.IssueDate.Format("d.m.y"),
						MoneyDto.ToMoneyFullString(invoice.Total)
					))

					.Append(

						jQuery.FromHtml(@"<a href='javascript:void(0)' class='delete-action'></a>")

						.Click(delegate (jQueryEvent e)
						{

							jQueryObject parent = jQuery.FromElement(e.Target).Parent();

							InvoiceDto dto = (InvoiceDto)parent.GetDataValue("invoice");


							MessageBoxWrap.Confirm(

								Res.Confirmation,

								string.Format(
									dto.Type == InvoiceType.Invoice ? Res.Invoice_Delete_Confirmation : Res.Receipt_Delete_Confirmation,
									dto.Number
								),

								delegate (string button, string text)
								{

									if (button != "yes")
										return;

									OrderService.DeleteInvoice(dto.Id,
										delegate
										{
											parent.Remove();

											((List<InvoiceDto>)(object)invoices).Remove(dto);

											MessageRegister.Info(Res.Invoice_List, string.Format(dto.Type == InvoiceType.Invoice ? Res.Invoice_Deleted_Message : Res.Receipt_Deleted_Message, dto.Number));
										},
										null
									);

								}
							);

						})

					)

					.Append(string.Format(
						"<div class='invoice-details'><span class='timestamp'>{0}</span><span class='issued-by'>{1}</span></div>",
						invoice.TimeStamp.Format("d.m.y H:i"),
						invoice.IssuedBy.Name)
					))
				;

			}

		}



		private void IssueInvoice()
		{

			ArrayList numbers = new ArrayList();

			foreach (InvoiceDto dto in Order.Invoices)
			{
				if (dto.Type == InvoiceType.Invoice && !numbers.Contains(dto.Number))
					numbers.Add(dto.Number);
			}

			InvoiceIssueForm form = new InvoiceIssueForm(Order.Id, Order.Owner, Order.BankAccount, (string[])numbers, InvoiceType.Invoice);

			form.Saved += delegate (object result) { OnIssueDocument((InvoiceDto)result); };

			form.Open();

		}



		private void IssueCompletionCertificate()
		{

			ArrayList numbers = new ArrayList();

			foreach (InvoiceDto dto in Order.Invoices)
			{
				if (dto.Type == InvoiceType.CompletionCertificate && !numbers.Contains(dto.Number))
					numbers.Add(dto.Number);
			}


			InvoiceIssueForm form = new InvoiceIssueForm(Order.Id, Order.Owner, Order.BankAccount, (string[])numbers, InvoiceType.CompletionCertificate);

			form.Saved += delegate (object result) { OnIssueDocument((InvoiceDto)result); };


			form.Open();

		}



		private void IssueReceipt()
		{
			OrderService.IssueReceipt(Order.Id,
				delegate (object result) { OnIssueDocument((InvoiceDto)result); }, null);
		}



		private void SetIsVoid(bool value)
		{

			OrderService.SetIsVoid(Order.Id, value,
				delegate
				{
					Order.IsVoid = value;

					RefreshOrderTitle();
				},
				null
			);

		}



		private void OnIssueDocument(InvoiceDto invoice)
		{

			string format = invoice.Type == InvoiceType.Receipt ? Res.ReceiptIssued_Msg : Res.InvoiceIssued_Msg;

			MessageRegister.Info(DomainRes.Order_Caption_List, string.Format(format, invoice.Number));

			ArrayList list = new ArrayList();
			list.Add(invoice);
			list.AddRange(Order.Invoices);

			Order.Invoices = (InvoiceDto[])list;

			_orderProperties.LoadInstance(Order);

			ReportPrinter.GetOrderDocument(invoice.Id, invoice.Number, invoice.Type, invoice.FileExtension);

		}



		private void CreatePayment(string type)
		{

			ArrayList invoices = new ArrayList();

			foreach (InvoiceDto invoice in Order.Invoices)
			{
				if ((type == ClassNames.WireTransfer && invoice.Type == InvoiceType.Invoice) ||
					(type != ClassNames.WireTransfer && invoice.Type == InvoiceType.Receipt))
					invoices.Add(Reference.Create(ClassNames.Invoice, invoice.Number, invoice.Id));
			}


			Dictionary values = new Dictionary(
				"Payer", Order.Customer,
				"Amount", Order.TotalDue,
				"Vat", Order.VatDue,
				"Order", Reference.Create(ClassNames.Order, Order.Number, Order.Id),
				"Invoices", invoices,
				"Owner", Order.Owner,
				"BankAccount", Order.BankAccount
			);


			FormsRegistry.EditObject(type, null, values,
				delegate (object arg1)
				{
					PaymentDto dto = (PaymentDto)((ItemResponse)arg1).Item;

					OrderService.GetOrder(Id,
						delegate (object result)
						{
							Load(result);

							if (type == ClassNames.CashInOrderPayment)
								ReportPrinter.GetCashOrder(dto.Id, dto.DocumentNumber);
						}
						,
						delegate { Tabs.Close(this); }
						);
				},
				null
			);

		}



		private void CreateInternalTransfer()
		{

			Reference order = new Reference();
			order.Id = Order.Id;
			order.Name = Order.Number;
			order.Type = ClassNames.Order;

			decimal amount = Order.TotalDue.Amount;


			Dictionary values = new Dictionary(
				"FromParty", Order.Customer,
				"ToParty", Order.Customer
			);


			if (amount < 0)
			{
				values["FromOrder"] = order;
				values["Amount"] = -amount;
			}
			else
			{
				values["ToOrder"] = order;
				values["Amount"] = amount;
			}


			FormsRegistry.EditObject(ClassNames.InternalTransfer, null, values,
				delegate
				{
					OrderService.GetOrder(Id,
						delegate (object result) { Load(result); },
						delegate { Tabs.Close(this); });
				},
				null
			);

		}



		private void IssueConsignment()
		{

			ConsignmentService.GetConsignmentItems(new object[] { Order.Id }, delegate (object result)
			{

				ConsignmentItemsDto consignmentItemsDto = (ConsignmentItemsDto)result;

				if (consignmentItemsDto.Items.Length > 0)
				{

					Dictionary values = new Dictionary("IsModified", true,
						"Supplier", consignmentItemsDto.Supplier,
						"Acquirer", consignmentItemsDto.Acquirer,
						"Discount", consignmentItemsDto.Discount,
						"IssueDate", consignmentItemsDto.IssueDate,
						"Items", Dictionary.GetDictionary(consignmentItemsDto.Items));

					FormsRegistry.EditObject(ClassNames.Consignment, null, values,
						delegate (object formResult)
						{
							GetInstance();
							ConsignmentDto dto = (ConsignmentDto)((ItemResponse)formResult).Item;
							string message = string.Format(Res.Issued_Consignment, dto.Number);
							MessageRegister.Info(DomainRes.Order_Caption_List, message);
						},
						null);
				}
				else
				{
					MessageBoxWrap.Show(new Dictionary("title", BaseRes.Error, "msg", Res.InvoiceItemsInConsignments_Warn,
						"icon", MessageBox.WARNING, "buttons", MessageBox.OK));
				}

			}, null);

		}



		private void AddTask()
		{

			Dictionary values = new Dictionary(
				"Order", Reference.Create(ClassNames.Order, Order.Number, Order.Id),
				"AssignedTo", Order.AssignedTo,
				"Status", TaskStatus.Open
			);


			FormsRegistry.EditObject(ClassNames.Task, null, values, delegate (object result)
			{
				TaskDto dto = (TaskDto)((ItemResponse)result).Item;
				dto.Type = ClassNames.Task;
				((List<TaskDto>)(object)Order.Tasks).Insert(0, dto);
				_orderProperties.LoadInstance(Order);
			}, null);

		}



		private Label _titleLabel;

		private Button _issueInvoiceButton;
		private Button _issueCompletionCertificateButton;
		private Button _issueReceiptButton;
		private Button _registerPaymentButton;
		private Button _issueConsignmentButton;
		private Button _addTaskButton;
		private Button _voidButton;

		private PropertyListControl _orderProperties;
		private PropertyListControl _orderFinances;
		private PropertyGridControl _orderItems;
		private PropertyGridControl _payments;
		private PropertyGridControl _transfers;
		private Item _createInternalTransferItem;

	}

}