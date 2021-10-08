using System;
using System.Collections;

using Ext.data;

using LxnBase.UI.AutoControls;

using Luxena.Travel.Services;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.Controls;

using ComboBox = Ext.form.ComboBox;
using Field = Ext.form.Field;


namespace Luxena.Travel.Reports
{
	public class CustomerReportForm : BaseEditForm
	{
		public CustomerReportForm()
		{
			Window.cls += " customer-report";

			Window.setTitle(Res.CustomerReportForm_Title);

			CreateFormItems();

			_createReportButton = Form.addButton(Res.Common_CreateReport, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			Fields = new Field[]
			{
				_customerSelector.Widget,
				_billToSelector.Widget,
				_passengerField,
				_airlineSelector.Widget,
				_paymentTypeField,
				_fromField,
				_toField,
				_unpayedDocuments,
				_includeDepartmentsCheckBox
			};

			ComponentSequence = new Component[]
			{
				_customerSelector.Widget,
				_billToSelector.Widget,
				_passengerField,
				_airlineSelector.Widget,
				_paymentTypeField,
				_fromField,
				_toField,
				_unpayedDocuments,
				_includeDepartmentsCheckBox,
				_createReportButton,
				_cancelButton
			};
		}

		public void Open()
		{
			Window.show();
		}

		private void CreateFormItems()
		{
			_customerSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Party)
				.allowEdit(false)
				.allowCreate(false)
				.setDataProxy(PartyService.SuggestCustomersProxy())
				//.allowBlank(false)
				.fieldLabel(DomainRes.Common_Customer)
				.width(200)
				.listeners(new Dictionary("changeValue", new AnonymousDelegate(CheckDocumentCount)))
				.selectOnFocus(true)
				.forceSelection(true));

			_billToSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Party)
				.allowEdit(false)
				.allowCreate(false)
				.setDataProxy(PartyService.SuggestCustomersProxy())
				//.allowBlank(false)
				.fieldLabel(DomainRes.Common_BillTo)
				.width(200)
				.listeners(new Dictionary("changeValue", new AnonymousDelegate(CheckDocumentCount)))
				.selectOnFocus(true)
				.forceSelection(true));

			_airlineSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Airline)
				.allowEdit(false)
				.allowCreate(false)
				.allowBlank(true)
				.fieldLabel(DomainRes.Airline)
				.width(200)
				.listeners(new Dictionary("changeValue", new AnonymousDelegate(CheckDocumentCount)))
				.selectOnFocus(true)
				.forceSelection(true));

			_passengerField = new TextField(new TextFieldConfig()
				.fieldLabel(DomainRes.Common_PassengerName)
				.listeners(new Dictionary("change", new AnonymousDelegate(CheckDocumentCount)))
				.width(200)
				.ToDictionary());

			_paymentTypeField = new ComboBox(new ComboBoxConfig()
				.fieldLabel(DomainRes.PaymentType)
				.store(new ArrayStore(new ArrayStoreConfig()
					.fields(new string[] { "Id", "Name" })
					.data(new object[]
					{
						new object[] { PaymentType.Cash, EnumUtility.ToString(typeof (PaymentType), PaymentType.Cash) },
						new object[] { PaymentType.Invoice, EnumUtility.ToString(typeof (PaymentType), PaymentType.Invoice) },
						new object[] { PaymentType.Check, EnumUtility.ToString(typeof (PaymentType), PaymentType.Check) },
						new object[] { PaymentType.CreditCard, EnumUtility.ToString(typeof (PaymentType), PaymentType.CreditCard) },
						new object[] { PaymentType.Exchange, EnumUtility.ToString(typeof (PaymentType), PaymentType.Exchange) },
					})
					.ToDictionary()))
				.mode("local")
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true)
				.listeners(new Dictionary("change", new AnonymousDelegate(CheckDocumentCount)))
				.ToDictionary());

			_fromField = new DateField(new DateFieldConfig()
				.fieldLabel(Res.FromDate_Text)
				.format("d.m.Y")
				.listeners(new Dictionary("change", new AnonymousDelegate(CheckDocumentCount)))
				.ToDictionary());

			_toField = new DateField(new DateFieldConfig()
				.fieldLabel(Res.ToDate_Text)
				.format("d.m.Y")
				.listeners(new Dictionary("change", new AnonymousDelegate(CheckDocumentCount)))
				.ToDictionary());

			_unpayedDocuments = new Checkbox(new CheckboxConfig()
				.boxLabel(Res.CustomerReportForm_IncludeUnpayedDocumentOnly)
				.handler(new AnonymousDelegate(CheckDocumentCount))
				.ToDictionary());

			_includeDepartmentsCheckBox = new Checkbox(new CheckboxConfig()
				.handler(new AnonymousDelegate(UpdateLabelCount))
				.listeners(new Dictionary("render", new AnonymousDelegate(
					delegate
					{
						Element el = (Element)Type.GetField(_includeDepartmentsCheckBox, "wrap");
						el.addClass("include-departments");
					})))
				.ToDictionary());

			_includeDepartmentsCheckBox.setDisabled(true);

			_totalCountBox = new BoxComponent(new BoxComponentConfig()
				.cls("total-count")
				.ToDictionary());

			UpdateLabelCount();

			Panel panel = new Panel(new PanelConfig()
				.items(new object[] { _includeDepartmentsCheckBox, _totalCountBox })
				.ToDictionary());

			Form.add(_customerSelector.Widget);
			Form.add(_billToSelector.Widget);
			Form.add(_passengerField);
			Form.add(_airlineSelector.Widget);
			Form.add(_paymentTypeField);
			Form.add(_fromField);
			Form.add(_toField);
			Form.add(_unpayedDocuments);
			Form.add(panel);
		}

		private void CheckDocumentCount()
		{
			Reference customer = _customerSelector.GetObjectInfo();
			Reference billTo = _billToSelector.GetObjectInfo();

			if (Script.IsNullOrUndefined(customer) && Script.IsNullOrUndefined(billTo))
			{
				UpdateLabelCount();

				return;
			}

			string passenger = (string)_passengerField.getValue();

			object airlineId = _airlineSelector.GetObjectId();

			object paymentType = null;

			object from = null;
			object to = null;

			object value = _paymentTypeField.getValue();

			if (!Script.IsNullOrUndefined(value) && !string.IsNullOrEmpty(value.ToString()))
				paymentType = (int)value;

			value = _fromField.getValue();

			if (!Script.IsNullOrUndefined(value) && !string.IsNullOrEmpty(value.ToString()))
				from = value;

			value = _toField.getValue();

			if (!Script.IsNullOrUndefined(value) && !string.IsNullOrEmpty(value.ToString()))
				to = value;

			ReportService.GetCustomerDocumentCount(
				customer != null ? customer.Id : null,
				billTo != null ? billTo.Id : null,
				passenger, airlineId, paymentType, from, to,
				_unpayedDocuments.getValue(),
				delegate(object result)
				{
					int[] counts = (int[])result;

					_customerDocCount = counts[0];
					_departmentDocCount = counts[1] - counts[0];

					UpdateLabelCount();

				}, null
			);
		}

		private void UpdateLabelCount()
		{
			int departmentCount = _departmentDocCount;
			int totalCount = _customerDocCount;

			if (_includeDepartmentsCheckBox.getValue())
				totalCount += _departmentDocCount;

			string text = StringUtility.GetNumberText(departmentCount, Res.CustomerReportForm_IncludeDepartments1,
				Res.CustomerReportForm_IncludeDepartments2, Res.CustomerReportForm_IncludeDepartments3);

			if (_includeDepartmentsCheckBox.rendered)
			{
				Element element = (Element)Type.GetField(_includeDepartmentsCheckBox, "wrap");
				Element label = (Element)element.child("label");

				label.dom.InnerHTML = text;
			}
			else
				_includeDepartmentsCheckBox.boxLabel = text;

			Reference customer = _customerSelector.GetObjectInfo();

			if (Script.IsNullOrUndefined(customer) || customer.Type != "Organization" || _departmentDocCount == 0)
				_includeDepartmentsCheckBox.disable();
			else
				_includeDepartmentsCheckBox.enable();

			text = string.Format(Res.CustomerReportForm_DocumentCount_Text, totalCount);

			if (_totalCountBox.rendered)
				_totalCountBox.getEl().update(text);
			else
				_totalCountBox.autoEl = new Dictionary("html", text);
		}

		protected override void OnSave()
		{
			string url = string.Format("reports/customer/Customer_report_{0}.pdf", Date.Now.Format("Y-m-d_H-i"));


			Dictionary prms = new Dictionary();

			object value = _customerSelector.GetObjectId();
			if (value != null)
				prms["customer"] = value;

			value = _billToSelector.GetObjectId();
			if (value != null)
				prms["billTo"] = value;

			value = _passengerField.getValue();

			if (!Script.IsNullOrUndefined(value) && (string)value != "")
				prms[PassengerParam] = value;

			value = _airlineSelector.GetObjectId();
			if (value != null)
				prms[AirlineParam] = value;

			value = _paymentTypeField.getValue();

			if (!Script.IsNullOrUndefined(value) && !string.IsNullOrEmpty(value.ToString()))
				prms[PaymenttypeParam] = value;

			value = _fromField.getValue();

			if (!Script.IsNullOrUndefined(value) && !string.IsNullOrEmpty(value.ToString()))
				prms[FromParam] = ((Date)value).Format("Y-m-d");

			value = _toField.getValue();

			if (!Script.IsNullOrUndefined(value) && !string.IsNullOrEmpty(value.ToString()))
				prms[ToParam] = ((Date)value).Format("Y-m-d");

			if (!_includeDepartmentsCheckBox.disabled && _includeDepartmentsCheckBox.getValue())
				prms[IncludedepartmentsParam] = "true";

			if (_unpayedDocuments.getValue())
				prms[UnpayedDocumentsOnly] = "true";

			ReportLoader.Load(url, prms);

			CompleteSave(null);
		}

		private ObjectSelector _customerSelector;
		private ObjectSelector _billToSelector;
		private DateField _fromField;
		private DateField _toField;
		private TextField _passengerField;
		private ObjectSelector _airlineSelector;
		private Field _paymentTypeField;
		private readonly Button _createReportButton;
		private readonly Button _cancelButton;
		private Checkbox _includeDepartmentsCheckBox;
		private Checkbox _unpayedDocuments;
		private BoxComponent _totalCountBox;

		private int _departmentDocCount;
		private int _customerDocCount;

		private const string PassengerParam = "passenger";
		private const string AirlineParam = "airline";
		private const string PaymenttypeParam = "paymentType";
		private const string FromParam = "from";
		private const string ToParam = "to";
		private const string IncludedepartmentsParam = "includeDepartments";
		private const string UnpayedDocumentsOnly = "unpayedDocumentsOnly";
	}
}