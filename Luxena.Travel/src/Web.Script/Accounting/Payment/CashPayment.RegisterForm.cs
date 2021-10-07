using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.form;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.Controls;

using Luxena.Travel.Cfg;
using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using ComboBox = LxnBase.UI.Controls.ComboBox;
using Field = Ext.form.Field;


namespace Luxena.Travel
{
	public class CashPaymentRegisterFormArgs
	{
		public readonly Reference Payer;
		public readonly MoneyDto Amount;
		public readonly MoneyDto Vat;
		public readonly object[] DocumentIds;

		public CashPaymentRegisterFormArgs(Reference payer, MoneyDto amount, MoneyDto vat, object[] documentIds)
		{
			Payer = payer;
			Amount = amount;
			Vat = vat;
			DocumentIds = documentIds;
		}
	}

	public class CashPaymentRegisterForm : BaseEditForm
	{
		public CashPaymentRegisterForm(CashPaymentRegisterFormArgs args)
		{
			_args = args;
		}

		public void Open()
		{
			AppService.GetDocumentOwners(
				delegate(object result)
				{
					_owners = (Reference[])result;

					Init();

					Window.show();
				}, null);
		}

		private void Init()
		{
			_saveButton = Form.addButton(BaseRes.Save, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			Fields = AddFields();

			ArrayList list = new ArrayList();
			list.AddRange(Fields);
			list.Add(_saveButton);
			list.Add(_cancelButton);

			ComponentSequence = (Component[]) list;

			Window.setTitle(Res.CashPaymentRegisterForm_Title);
			Window.addClass("cashPaymentForm");
		}

		private Field[] AddFields()
		{
			_date = new DateField(new DateFieldConfig()
				.fieldLabel(DomainRes.Common_Date)
				.format("d.m.Y")
				.selectOnFocus(true)
				.allowBlank(false)
				.ToDictionary());

			_payerField = new ObjectSelector((ObjectSelectorConfig) new ObjectSelectorConfig()
				.setClass(ClassNames.Party)
				.selectOnFocus(true)
				.allowBlank(false)
				.width(230)
				.fieldLabel(DomainRes.Payment_Payer));

			_numberField = new TextField(new TextFieldConfig()
				.fieldLabel(Res.CashInOrderPayment_DocumentName)
				.selectOnFocus(true)
				.allowBlank(true)
				.width(230)
				.ToDictionary());

			_amountField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Payment_Amount, true).SetAllowBlank(false));
			_amountField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_vatField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Payment_Vat , true));

			_receivedFromField = new TextField(new TextFieldConfig()
				.fieldLabel(DomainRes.Payment_ReceivedFrom)
				.selectOnFocus(true)
				.allowBlank(true)
				.width(230)
				.ToDictionary());

			if (AppManager.SystemConfiguration.AviaOrderItemGenerationOption == AviaOrderItemGenerationOption.ManualSetting)
			{
				_separateServiceFee = new Checkbox(new CheckboxConfig()
					.boxLabel(Res.Order_SeparateServiceFee)
					.ToDictionary());

				_separateServiceFee.setValue(true);
			}

			if (_owners.Length > 1)
			{
				_ownerSelector = new ComboBox(new ComboBoxConfig()
					.name("Office")
					.store(new JsonStore(new JsonStoreConfig()
						.data(_owners)
						.fields(new string[] { "Id", "Name" })
						.ToDictionary()))
					.mode("local")
					.editable(false)
					.displayField("Name")
					.valueField("Id")
					.triggerAction("all")
					.selectOnFocus(true)
					.fieldLabel(DomainRes.Common_Owner)
					.width(170)
					.allowBlank(false)
					.ToDictionary());
			}

			_noteField = new TextArea(new TextAreaConfig()
				.name("Note")
				.fieldLabel(DomainRes.Common_Note)
				.selectOnFocus(true)
				.allowBlank(true)
				.height(70)
				.width(230)
				.ToDictionary());

			_date.setValue(Date.Today);
			_payerField.SetValue(_args.Payer);
			_amountField.setValue(_args.Amount);
			_vatField.setValue(_args.Vat);

			_createConsignment = new Checkbox(new CheckboxConfig()
					.boxLabel(Res.CashPaymentRegisterForm_DoCreateConsignment.ToLowerCase())
					.ToDictionary());

			_savePosted = new Checkbox(new CheckboxConfig()
						.labelSeparator(string.Empty)
						.boxLabel(Res.CashInOrderPayment_SavePosted.ToLowerCase())
						.value(true)
						.ToDictionary());

			_savePosted.setValue(true);

			ArrayList fields = new ArrayList();
			fields.Add(_date);
			fields.Add(_payerField.Widget);
			fields.Add(_numberField);
			fields.Add(_amountField);
			fields.Add(_vatField);
			fields.Add(_receivedFromField);

			ArrayList formItems = new ArrayList();
			formItems.AddRange((object[]) fields);

			if (_ownerSelector != null)
			{
				fields.Add(_ownerSelector);
				formItems.Add(_ownerSelector);
			}

			formItems.Add(_noteField);

			if (_separateServiceFee != null)
			{
				fields.Add(_separateServiceFee);
				formItems.Add(_separateServiceFee);
			}

			fields.Add(_createConsignment);
			formItems.Add(_createConsignment);

			fields.Add(_savePosted);
			formItems.Add(_savePosted);

			Form.add(new Panel(new PanelConfig()
				.items(formItems)
				.layout("form")
				.ToDictionary()));

			return (Field[]) fields;
		}
		
		protected override bool OnValidate()
		{
			if (_amountField.getValue() == null)
				return false;

			return true;
		}

		protected override void OnSave()
		{
			PaymentService.CreateCashPayment(GetRequest(), CompleteSave, null);
		}

		protected override void OnSaved(object result)
		{
			CashPaymentResponse response = (CashPaymentResponse) result;
			PaymentDto payment = response.Payment;
			ConsignmentDto consignment = response.Consignment;

			string msg = string.Format(Res.QuickCashPayment_Msg, ObjectLink.Render(payment.Id, payment.Number, ClassNames.CashInOrderPayment), ObjectLink.RenderInfo(payment.Order));

			if (!Script.IsNullOrUndefined(consignment))
				msg += string.Format(Res.QuickCashPayment_Consiment_Msg, ObjectLink.Render(consignment.Id, consignment.Number, ClassNames.Consignment));

			MessageRegister.Info(DomainRes.Payment_Caption_List, msg);

			ReportPrinter.GetCashOrder(payment.Id, payment.Number);

			if (!Script.IsNullOrUndefined(consignment))
				ReportPrinter.GetLastIssuedConsignment(consignment.Id, consignment.Number);

			base.OnSaved(result);
		}

		private CashInOrderPaymentRequest GetRequest()
		{
			CashInOrderPaymentRequest request = new CashInOrderPaymentRequest();

			request.Date = (Date)_date.getValue();
			request.Payer = _payerField.GetObjectInfo();

			string number = (string)_numberField.getValue();
			request.Number = !string.IsNullOrEmpty(number) ? number : null;

			request.Amount = _amountField.getValue();
			request.PaymentVat = _vatField.getValue();

			request.Owner = _owners.Length == 1 ? Reference.Copy(_owners[0]) : _ownerSelector.GetObjectInfo();

			string text = (string) _receivedFromField.getValue();

			if (text != string.Empty)
				request.ReceivedFrom = text;

			text = (string)_noteField.getValue();

			if (text != string.Empty)
				request.Note = text;

			request.IsPosted = _savePosted.getValue();

			switch (AppManager.SystemConfiguration.AviaOrderItemGenerationOption)
			{
				case AviaOrderItemGenerationOption.SeparateServiceFee:
					request.SeparateServiceFee = true;
					break;
				case AviaOrderItemGenerationOption.AlwaysOneOrderItem:
					request.SeparateServiceFee = false;
					break;
				case AviaOrderItemGenerationOption.ManualSetting:
					request.SeparateServiceFee = _separateServiceFee.getValue();
					break;
			}

			request.DocumentIds = _args.DocumentIds;
			request.CreateConsignment = _createConsignment.getValue();
			
			return request;
		}

		private void OnAmountChange(Field decimalField, object newValue, object oldValue)
		{
			if (!Script.IsNullOrUndefined(_amountField.getValue()))
			{
				_vatField.setValue(CalculateVat());
			}
		}

		private MoneyDto CalculateVat()
		{
			if (_args.Vat == null || _args.Amount == null || _amountField.getValue() == null) return null;

			MoneyDto recalculatedVat = new MoneyDto();
			recalculatedVat.Amount = (_amountField.getValue().Amount*_args.Vat.Amount)/_args.Amount.Amount;
			recalculatedVat.Currency = _args.Vat.Currency;
			
			return recalculatedVat;
		}

		private readonly CashPaymentRegisterFormArgs _args;

		private Field _date;
		private ObjectSelector _payerField;
		private TextField _numberField;
		private MoneyControl _amountField;
		private MoneyControl _vatField;
		private Field _receivedFromField;
		private Checkbox _separateServiceFee;
		private ComboBox _ownerSelector;
		private Field _noteField;
		private Checkbox _createConsignment;

		private Reference[] _owners;

		private Button _saveButton;
		private Button _cancelButton;
		private Checkbox _savePosted;
	}
}