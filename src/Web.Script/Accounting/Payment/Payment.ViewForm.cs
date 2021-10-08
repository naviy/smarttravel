using System;
using System.Collections;

using Ext;
using Ext.form;

using jQueryApi;

using LxnBase;
using LxnBase.UI;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using Action = Ext.Action;
using ActionConfig = Ext.ActionConfig;


namespace Luxena.Travel
{
	public class PaymentViewForm : BaseClassViewForm
	{
		static PaymentViewForm()
		{
			FormsRegistry.RegisterView(ClassNames.CashInOrderPayment, ViewObject);
			FormsRegistry.RegisterView(ClassNames.CashOutOrderPayment, ViewObject);
			FormsRegistry.RegisterView(ClassNames.WireTransfer, ViewObject);
			FormsRegistry.RegisterView(ClassNames.CheckPayment, ViewObject);
			FormsRegistry.RegisterView(ClassNames.ElectronicPayment, ViewObject);
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string) id, delegate(string tabId) { return new PaymentViewForm(tabId, id, type); });
		}

		public PaymentViewForm(string tabId, object id, string type) : base(tabId, id, type)
		{
		}

		private PaymentDto Payment
		{
			get { return (PaymentDto) Instance; }
		}

		protected override void OnInitToolBar(ArrayList toolbarItems)
		{
			toolbarItems.RemoveAt(1);

			_voidAction = new Action(new ActionConfig()
				.text(Res.Void_Action.ToLowerCase())
				.handler(new AnonymousDelegate(VoidPayment))
				.ToDictionary());

			_postAction = new Action(new ActionConfig()
				.text(Res.Posted_Action.ToLowerCase())
				.handler(new AnonymousDelegate(PostPayment))
				.ToDictionary());

			toolbarItems.Insert(0, _voidAction);
			toolbarItems.Insert(1, _postAction);
			toolbarItems.Insert(2, new ToolbarSeparator());
		}

		protected override void initComponent()
		{
			cls = "payment-view";
			layout = null;

			base.initComponent();

			_titleLabel = new Label(new LabelConfig()
				.cls("title")
				.ToDictionary());

			_paymentPropertiesControl = GetPaymentPropertiesControl();
			_paymentAmountControl = GetPaymentAmountControl();

			Panel contentPanel = new Panel(new PanelConfig()
				.items(new object[]
				{
					_titleLabel,
					_paymentPropertiesControl,
					_paymentAmountControl
				})
				.border(false)
				.cls("content-panel")
				.ToDictionary());

			add(contentPanel);
		}

		protected override void UpdateActionsStatus()
		{
			SetActionStatus(_voidAction, Payment.Permissions.CanUpdate);
			SetActionStatus(_postAction, Payment.Permissions.CanUpdate);

			_voidAction.setText(Payment.IsVoid ? Res.Restore_Action.ToLowerCase() : Res.Void_Action.ToLowerCase());

			if (!Script.IsNullOrUndefined(Payment.PostedOn) || Payment.IsVoid)
				_postAction.disable();
			else
				_postAction.enable();

			base.UpdateActionsStatus();
		}

		protected override void GetInstance()
		{
			DomainService.Get(_type, Id, Load, delegate { Tabs.Close(this); });
		}

		protected override void OnLoad()
		{
			setTitle(Payment.Number);

			_titleLabel.removeClass("voided");
			_titleLabel.removeClass("nonposted");

			string text = DomainRes.Payment;

			if (Payment.IsVoid)
			{
				_titleLabel.addClass("voided");

				text = string.Format("{0} ({1})", text, Res.Payment_Voided);
			}
			else if (Script.IsNullOrUndefined(Payment.PostedOn))
			{
				_titleLabel.addClass("nonposted");

				text = string.Format("{0} ({1})", text, Res.Payment_NonPosted);
			}

			_titleLabel.setText(text);

			_paymentPropertiesControl.LoadInstance(Payment);
			_paymentAmountControl.LoadInstance(Payment);

			CompositeElement element = (CompositeElement) _paymentPropertiesControl.body.select(".doc-number .fieldLabel");
			if (element.getCount() != 0)
				element.first().dom.InnerHTML = GetDocumentNumberText();
		}

		private void VoidPayment()
		{
			MessageFactory.VoidingConfirmation(Res.Payment_Void_Confirmation, null, null,
				Res.Payment_Restore_Confirmation, null, null, Payment.IsVoid, 1,
				delegate
				{
					PaymentService.ChangeVoidStatus(new object[] { Payment.Id }, null,
						delegate(object result)
						{
							ItemListResponse response = (ItemListResponse) result;
							PaymentDto dto = (PaymentDto) response.Items[0];

							Load(dto);

							MessageFactory.VoidCompletedMsg(DomainRes.Payment,
								Res.Document_VoidDocument_Msg, null,
								Res.Document_RestoreDocument_Msg, null,
								dto.IsVoid, 1, dto.Number, null);
						}, null);
				});
		}

		private void PostPayment()
		{
			PaymentService.PostPayments(new object[] { Payment.Id }, null,
				delegate(object result)
				{
					ItemListResponse response = (ItemListResponse) result;
					PaymentDto dto = (PaymentDto) response.Items[0];

					Load(dto);

					MessageRegister.Info(DomainRes.Payment_Caption_List, string.Format(Res.PaymentView_Posted, Payment.Number));
				},
				null);
		}

		private PropertyListControl GetPaymentPropertiesControl()
		{
			PropertyListControlConfig config = new PropertyListControlConfig()
				.SetListItems(new PropertyItem[]
				{
					new PropertyItem(DomainRes.Common_Number, "Number"),
					new PropertyItem(DomainRes.Common_Date, "Date").SetPropertyType(PropertyType.Date),
					new PropertyItem(DomainRes.Payment_PostedOn, "PostedOn").SetPropertyType(PropertyType.Date),
					new PropertyItem(DomainRes.Payment_Payer, "Payer").SetPropertyType(PropertyType.ObjectInfo),
					new PropertyItem(DomainRes.Common_AssignedTo, "AssignedTo"),
					new PropertyItem(DomainRes.Payment_RegisteredBy, "RegisteredBy"),
					new PropertyItem(DomainRes.Payment_ReceivedFrom, "ReceivedFrom").SetHideIsEmpty(true),
					new PropertyItem(Res.Payment_Invoice, "Invoice").SetPropertyType(PropertyType.ObjectInfo).SetHideIsEmpty(true),
					new PropertyItem(DomainRes.Order, "Order").SetPropertyType(PropertyType.ObjectInfo).SetHideIsEmpty(true),
					new PropertyItem(Res.PaymentView_Document, "DocumentNumber").SetCssClass("doc-number").SetHideIsEmpty(true).SetRenderer(new PropertyItemRenderDelegate(
						delegate(PropertyItem propertyItem, object value, jQueryObject container)
						{
							PaymentDto payment = Payment;

							if (value == null)
								return;

							string text = payment.DocumentNumber;

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
					new PropertyItem(DomainRes.ElectronicPayment_AuthorizationCode, "AuthorizationCode").SetHideIsEmpty(true),
					new PropertyItem(DomainRes.ElectronicPayment_PaymentSystem, "PaymentSystem").SetPropertyType(PropertyType.ObjectInfo).SetHideIsEmpty(true),
					new PropertyItem(DomainRes.Common_Note, "Note").SetHideIsEmpty(true)
				})
				.SetCssClass("payment-properties");

			return new PropertyListControl(config);
		}

		private static PropertyListControl GetPaymentAmountControl()
		{
			PropertyListControlConfig config = new PropertyListControlConfig()
				.SetListItems(new PropertyItem[]
				{
					new PropertyItem(DomainRes.Payment_PaymentForm, "PaymentForm").SetPropertyType(PropertyType.EnumType).SetEnumType(typeof (PaymentForm)),
					new PropertyItem(DomainRes.Payment_Amount, "Amount").SetPropertyType(PropertyType.Money).SetCssClass("amount-value"),
					new PropertyItem(DomainRes.Payment_Vat, "Vat").SetPropertyType(PropertyType.Money).SetCssClass("amount-value")
				})
				.SetCssClass("payment-amount");

			return new PropertyListControl(config);
		}

		private string GetDocumentNumberText()
		{
			string text = null;

			switch (Payment.PaymentForm)
			{
				case PaymentForm.WireTransfer:
					text = Res.WireTransfer_DocumentName;
					break;

				case PaymentForm.Check:
					text = Res.CheckPayment_DocumentName;
					break;

				case PaymentForm.CashInOrder:
					text = Res.CashInOrderPayment_DocumentName;
					break;

				case PaymentForm.CashOutOrder:
					text = Res.CashOutOrderPayment_DocumentName;
					break;

				case PaymentForm.Electronic:
					text = Res.ElectronicPayment_DocumentName;
					break;
			}

			return text + ":";
		}

		private PropertyListControl _paymentPropertiesControl;
		private PropertyListControl _paymentAmountControl;
		private Label _titleLabel;
		private Action _voidAction;
		private Action _postAction;
	}
}