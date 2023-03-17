using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.form;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using Action = Ext.Action;
using ActionConfig = Ext.ActionConfig;
using FormAction = Ext.form.Action;
using ComboBox = LxnBase.UI.Controls.ComboBox;
using Field = Ext.form.Field;
using Record = Ext.data.Record;




namespace Luxena.Travel
{



	//===g






	public class PaymentEditForm : BaseClassEditForm
	{

		//---g



		static PaymentEditForm()
		{
			FormsRegistry.RegisterEdit(ClassNames.CashInOrderPayment, EditObject);
			FormsRegistry.RegisterEdit(ClassNames.CashOutOrderPayment, EditObject);
			FormsRegistry.RegisterEdit(ClassNames.WireTransfer, EditObject);
			FormsRegistry.RegisterEdit(ClassNames.CheckPayment, EditObject);
			FormsRegistry.RegisterEdit(ClassNames.ElectronicPayment, EditObject);
		}



		public static void EditObject(EditFormArgs args)
		{
			ConfigManager.GetEditConfig(args.Type,
				delegate (ItemConfig config)
				{
					PaymentService.GetPaymentSystems(
						delegate (object data)
						{
							PaymentEditForm form = new PaymentEditForm(args, config, (Reference[])data);
							form.Open();
						},
						null);

				});
		}



		private PaymentEditForm(EditFormArgs args, ItemConfig config, Reference[] paymentSystems) : base(args, config)
		{
			_paymentSystems = paymentSystems;

			Window.addClass("payment-edit");
		}



		//---g



		private PaymentDto Payment
		{
			get { return (PaymentDto)Instance; }
		}



		//---g



		protected override void LoadInstance(AjaxCallback onLoaded)
		{
			DomainService.Get(Args.Type, Args.IdToLoad, onLoaded, null);
			//			PaymentService.GetPayment(Args.IdToLoad, onLoaded, null);
		}



		protected override Field[] AddFields()
		{
			_dateField = CreateEditor("Date");

			_documentNumberField = (TextField)CreateEditor("DocumentNumber");
			_documentNumberField.fieldLabel = GetDocumentNumberFieldText();
			_documentNumberField.width = 166;

			if (Args.Type == ClassNames.CashInOrderPayment)
			{
				_documentNumberField.emptyText = Res.Auto;
				_documentNumberField.emptyClass = "auto-text";
			}
			else if (Args.Type == ClassNames.ElectronicPayment)
			{
				_authorizationCodeField = (TextField)CreateEditor("AuthorizationCode");
				_authorizationCodeField.width = 166;

				_paymentSystemField = new ComboBox(new ComboBoxConfig()
					.store(new JsonStore(new JsonStoreConfig()
						.fields(new string[] { "Id", "Name" })
						.data(_paymentSystems)
						.ToDictionary()))
					.mode("local")
					.editable(false)
					.displayField("Name")
					.valueField("Id")
					.triggerAction("all")
					.selectOnFocus(true)
					.fieldLabel(DomainRes.ElectronicPayment_PaymentSystem)
					.allowBlank(true)
					.ToDictionary());
			}

			string label = Args.Type == ClassNames.WireTransfer ? DomainRes.InvoiceType_Invoice : DomainRes.InvoiceType_Receipt;

			_invoiceSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.valueProperties(new string[] { "Order", "BillTo", "TotalDue", "VatDue", "Owner", "BankAccount" })
				.setDataProxy(OrderService.SuggestInvoicesProxy(Args.Type))
				.customActions(GetInvoiceActions())
				.fieldLabel(label));

			_orderSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Order)
				.valueProperties(new string[] { "Customer", "BillTo", "TotalDue", "VatDue", "Owner", "BankAccount" })
				.fieldLabel(DomainRes.Order));

			_payerSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Party)
				.allowCreate(false)
				.setDataProxy(PartyService.SuggestCustomersProxy())
				.fieldLabel(DomainRes.Payment_Payer)
				.width(230)
				.allowBlank(false));

			_amountField = (MoneyControl)CreateEditor("Amount");
			_amountField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_vatField = (MoneyControl)CreateEditor("Vat");

			if (Args.Type != ClassNames.WireTransfer)
				_receivedFromField = ControlFactory.CreateEditor(GetFieldConfig("ReceivedFrom"));

			_ownerField = ControlFactoryExt.CreateOwnerControl(230, false);
			_bankAccountField = ControlFactoryExt.CreateBankAccountControl(230);

			_assignedTo = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Person)
				.allowCreate(false)
				.setDataProxy(GenericService.SuggestProxy("Person"))
				.fieldLabel(DomainRes.Common_AssignedTo)
				.width(230));

			_noteField = ControlFactory.CreateEditor(GetFieldConfig("Note"));
			_noteField.addClass("note");

			if (Args.Type == ClassNames.CashInOrderPayment)
			{
				_savePostedCheckBox = new Checkbox(
					new CheckboxConfig()
						.labelSeparator(string.Empty)
						.boxLabel(Res.CashInOrderPayment_SavePosted.ToLowerCase())
						.ToDictionary());

				_savePostedCheckBox.setValue(true);
			}

			ArrayList fields = new ArrayList();
			fields.AddRange(new object[]
			{
				_dateField,
				_documentNumberField
			});

			if (Args.Type == ClassNames.ElectronicPayment)
				fields.AddRange(new object[]
				{
					_authorizationCodeField,
					_paymentSystemField
				});

			fields.AddRange(new object[]
			{
				_invoiceSelector.Widget,
				_orderSelector.Widget,
				_payerSelector.Widget,
				_amountField,
				_vatField
			});

			if (_receivedFromField != null)
				fields.Add(_receivedFromField);

			fields.AddRange(new object[] { _ownerField, _assignedTo.Widget, _bankAccountField, _noteField });

			if (_savePostedCheckBox != null)
				fields.Add(_savePostedCheckBox);

			Form.add(fields);

			return (Field[])fields;
		}



		private Action[] GetInvoiceActions()
		{
			Reference[] invoices = (Reference[])GetArgsValue("Invoices");

			if (!Script.Boolean(invoices) || invoices.Length == 0)
				return null;

			ArrayList actions = new ArrayList();

			for (int i = 0; i < invoices.Length; ++i)
				actions.Add(GetInvoiceAction(invoices[i]));

			return (Action[])actions;
		}



		private Action GetInvoiceAction(Reference invoice)
		{
			return new Action(new ActionConfig()
				.text(invoice.Name)
				.handler(new AnonymousDelegate(delegate
				{
					_skipOrderChanges = true;
					_skipInvoiceChanges = true;
					try
					{
						_payerSelector.SetValue(GetArgsValue("Payer"));
						_orderSelector.SetValue(GetArgsValue("Order"));
						_invoiceSelector.SetValue(invoice);
					}
					finally
					{
						_skipOrderChanges = false;
						_skipInvoiceChanges = false;
					}
				}))
				.ToDictionary());
		}



		protected override void InitComponentSequence(Field[] fields, Button[] buttons)
		{
			ArrayList list = new ArrayList();
			list.AddRange(fields);
			list.AddRange(buttons);

			list.Remove(_noteField);

			ComponentSequence = (Component[])list;
		}



		private void OnInvoiceChanged(Field objthis, object newvalue, object oldvalue)
		{

			if (newvalue == null || _skipInvoiceChanges)
				return;


			Record record = ((Store)((ComboBox)objthis).store).getById((string)((Array)newvalue)[Reference.IdPos]);

			_skipOrderChanges = true;

			_orderSelector.SetValue(record.get("Order"));

			_skipOrderChanges = false;

			if (Payment == null)
				SetDataFromOrder(record);

		}



		private void OnOrderChanged(Field objthis, object newvalue, object oldvalue)
		{

			if (newvalue == null || _skipOrderChanges)
				return;

			if (oldvalue == null || ((Array)oldvalue)[0] != ((Array)newvalue)[0])
			{
				_skipInvoiceChanges = true;

				_invoiceSelector.SetValue(null);

				_skipInvoiceChanges = false;
			}

			if (Payment == null)
			{
				Record record = ((Store)((ComboBox)objthis).store).getById((string)((Array)newvalue)[0]);

				SetDataFromOrder(record);
			}

		}



		private void SetDataFromOrder(Record record)
		{

			object[] customer = (object[])record.get("Customer");
			object[] billTo = (object[])record.get("BillTo");

			object[] totalDue = (object[])record.get("TotalDue");
			object[] vatDue = (object[])record.get("VatDue");

			object[] owner = (object[])record.get("Owner");

			object[] bankAccount = (object[])record.get("BankAccount");


			_payerSelector.SetValue(billTo ?? customer);

			_amountField.setValue(GetMoney(totalDue));
			_vatField.setValue(GetMoney(vatDue));

			if (AppManager.AllowSetDocumentOwner)
				_ownerField.setValue(owner);

			_bankAccountField.setValue(bankAccount);


		}



		private static MoneyDto GetMoney(object money)
		{

			if (money == null)
				return null;

			if (!(money is Array))
				return (MoneyDto)money;

			object[] data = (object[])money;

			MoneyDto dto = new MoneyDto();
			dto.Amount = (decimal)data[0];

			dto.Currency = new Reference();
			dto.Currency.Name = (string)data[1];
			dto.Currency.Id = data[2];
			dto.Currency.Type = ClassNames.Currency;


			return dto;

		}



		private string GetDocumentNumberFieldText()
		{

			switch (Args.Type)
			{
				case ClassNames.WireTransfer:
					return Res.WireTransfer_DocumentName;

				case ClassNames.CashInOrderPayment:
					return Res.CashInOrderPayment_DocumentName;

				case ClassNames.CashOutOrderPayment:
					return Res.CashOutOrderPayment_DocumentName;

				case ClassNames.CheckPayment:
					return Res.CheckPayment_DocumentName;

				case ClassNames.ElectronicPayment:
					return Res.ElectronicPayment_DocumentName;
			}


			return null;

		}



		protected override void SetFieldValues()
		{

			_dateField.setValue(GetInstancePropertyValue("Date") ?? Date.Today);

			if (Payment != null || Args.FieldValues != null)
			{
				_documentNumberField.setValue(GetInstancePropertyValue("DocumentNumber"));
				_payerSelector.SetValue(GetInstancePropertyValue("Payer"));
				_amountField.setValue((MoneyDto)GetInstancePropertyValue("Amount"));
				_vatField.setValue((MoneyDto)GetInstancePropertyValue("Vat"));

				if (_receivedFromField != null)
					_receivedFromField.setValue(GetInstancePropertyValue("ReceivedFrom"));

				if (_authorizationCodeField != null)
					_authorizationCodeField.setValue(GetInstancePropertyValue("AuthorizationCode"));

				if (_paymentSystemField != null)
					_paymentSystemField.setValue(GetInstancePropertyValue("PaymentSystem"));

				_invoiceSelector.SetValue(GetInstancePropertyValue("Invoice"));
				_orderSelector.SetValue(GetInstancePropertyValue("Order"));

				_ownerField.setValue(GetInstancePropertyValue("Owner"));
				_bankAccountField.setValue(GetInstancePropertyValue("BankAccount"));
				_assignedTo.SetValue(GetInstancePropertyValue("AssignedTo"));
				_noteField.setValue(GetInstancePropertyValue("Note"));
			}
			else
			{
				_dateField.setValue(Date.Today);
			}

			_orderSelector.Widget.on("changeValue", new FieldChangeDelegate(OnOrderChanged));
			_invoiceSelector.Widget.on("changeValue", new FieldChangeDelegate(OnInvoiceChanged));
		}



		protected override void OnSave()
		{
			if (Payment == null || IsModified())
			{
				StartSave();
				DomainService.Update(Args.Type, GetPayment(), Args.RangeRequest, CompleteSave, FailSave);
			}
			else
				Cancel();
		}


		protected override void OnSaved(object result)
		{
			PaymentDto dto = (PaymentDto)((ItemResponse)result).Item;

			if (!LocalMode && Payment == null && !Script.IsNullOrUndefined(Args.FieldValues))
				MessageFactory.ObjectUpdatedMsg(InstanceConfig.ListCaption, ObjectLink.Render(dto.Id, dto.Number, Args.Type), Args.IsNew);

			base.OnSaved(result);
		}



		private PaymentDto GetPayment()
		{

			PaymentDto dto = new PaymentDto();

			if (Payment != null)
			{
				dto.Id = Payment.Id;
				dto.Version = Payment.Version;
			}

			dto.Date = (Date)_dateField.getValue();

			dto.DocumentNumber = GetStringValue(_documentNumberField);

			if (_authorizationCodeField != null)
				dto.AuthorizationCode = GetStringValue(_authorizationCodeField);

			if (_paymentSystemField != null)
				dto.PaymentSystem = _paymentSystemField.GetObjectInfo();

			dto.Payer = _payerSelector.GetObjectInfo();
			dto.Amount = _amountField.getValue();
			dto.Vat = _vatField.getValue();

			if (_receivedFromField != null)
				dto.ReceivedFrom = GetStringValue(_receivedFromField);

			dto.Note = GetStringValue(_noteField);

			dto.Invoice = _invoiceSelector.GetObjectInfo();
			dto.Order = _orderSelector.GetObjectInfo();
			dto.Owner = _ownerField.GetObjectInfo();
			dto.BankAccount = _bankAccountField.GetObjectInfo();
			dto.AssignedTo = _assignedTo.GetObjectInfo();


			if (_savePostedCheckBox == null || _savePostedCheckBox.getValue())
				dto.SavePosted = true;


			return dto;

		}



		private MoneyDto CalculateVat()
		{

			MoneyDto originalAmount = (MoneyDto)GetArgsValue("Amount");
			MoneyDto originalVat = (MoneyDto)GetArgsValue("Vat");

			MoneyDto currentAmount = _amountField.getValue();

			if (originalAmount != null && originalVat != null && currentAmount != null)
			{
				MoneyDto vat = MoneyDto.GetZeroMoney();
				vat.Amount = originalVat.Amount * currentAmount.Amount / originalAmount.Amount;

				return vat;
			}


			return null;

		}



		private void OnAmountChange(Field decimalField, object newValue, object oldValue)
		{

			if (Payment != null)
				return;

			MoneyDto vat = CalculateVat();

			if (vat != null)
				_vatField.setValue(vat);

		}



		private static string GetStringValue(Field field)
		{
			string value = (string)field.getValue();

			if (string.IsNullOrEmpty(value))
				return null;

			return value;
		}



		private Field _dateField;
		private TextField _documentNumberField;
		private ObjectSelector _payerSelector;
		private MoneyControl _amountField;
		private MoneyControl _vatField;
		private Field _receivedFromField;
		private Field _noteField;
		private ComboBox _ownerField;
		private ComboBox _bankAccountField;

		private ObjectSelector _assignedTo;
		private ObjectSelector _orderSelector;
		private ObjectSelector _invoiceSelector;
		private Checkbox _savePostedCheckBox;
		private bool _skipOrderChanges;
		private bool _skipInvoiceChanges;
		private TextField _authorizationCodeField;
		private ComboBox _paymentSystemField;
		private readonly Reference[] _paymentSystems;



		//---g

	}






	//===g



}