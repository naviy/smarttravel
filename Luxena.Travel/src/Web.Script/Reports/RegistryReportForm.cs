using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.form;

using LxnBase;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using ComboBox = Ext.form.ComboBox;
using Field = Ext.form.Field;


namespace Luxena.Travel.Reports
{
	public class RegistryReportForm : BaseEditForm
	{
		public RegistryReportForm()
		{
			Window.cls += " registry-report";

			Window.setTitle(Res.RegistryReportForm_Title);

			CreateFormItems();

			_createReportButton = Form.addButton(Res.Common_CreateReport, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			Fields = new Field[]
			{
				_fromField,
				_toField,
				_paymentTypeField,
				_paymentFormField,
				_airlineSelector.Widget,
				_reportTypeField,
				_onlyWithInvoices
			};

			ComponentSequence = new Component[]
			{
				_fromField,
				_toField,
				_paymentFormField,
				_airlineSelector.Widget,
				_reportTypeField,
				_onlyWithInvoices,
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
			_fromField = new DateField(new DateFieldConfig()
				.fieldLabel(Res.FromDate_Text)
				.format("d.m.Y")
				.ToDictionary());

			_toField = new DateField(new DateFieldConfig()
				.fieldLabel(Res.ToDate_Text)
				.format("d.m.Y")
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
						new object[] { PaymentType.WithoutPayment, DomainRes.PaymentType_WithoutPayment },
						new object[] { PaymentType.Unknown, EnumUtility.ToString(typeof (PaymentType), PaymentType.Unknown) }
					})
					.ToDictionary()))
				.mode("local")
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true)
				.width(180)
				.ToDictionary());

			_paymentFormField = new ComboBox(new ComboBoxConfig()
				.fieldLabel("Форма оплаты")
				.store(new ArrayStore(new ArrayStoreConfig()
					.fields(new string[] { "Id", "Name" })
					.data(new object[]
					{
						new object[] { PaymentForm.CashInOrder, "ПКО" },
						new object[] { PaymentForm.CashOutOrder, "РКО" },
						new object[] { PaymentForm.WireTransfer, EnumUtility.Localize(typeof (PaymentForm), PaymentForm.WireTransfer, typeof(DomainRes)) },
						new object[] { PaymentForm.Check, EnumUtility.Localize(typeof (PaymentForm), PaymentForm.Check, typeof(DomainRes)) },
						new object[] { PaymentForm.Electronic, EnumUtility.Localize(typeof (PaymentForm), PaymentForm.Electronic, typeof(DomainRes)) },
					})
					.ToDictionary()))
				.mode("local")
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true)
				.width(180)
				.ToDictionary());

			_airlineSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Airline)
				.allowEdit(false)
				.allowCreate(false)
				.allowBlank(true)
				.fieldLabel(DomainRes.Airline)
				.width(180)
				//.listeners(new Dictionary("changeValue", new AnonymousDelegate(CheckDocumentCount)))
				.selectOnFocus(true)
				.forceSelection(true));


			_reportTypeField = new ComboBox(new ComboBoxConfig()
				.fieldLabel(Res.ReportType_Text)
				.store(new ArrayStore(new ArrayStoreConfig()
					.fields(new string[] { "Id", "Name" })
					.data(new object[]
					{
						new object[] { ReportType.Excel, EnumUtility.ToString(typeof (ReportType), ReportType.Excel) },
						new object[] { ReportType.Pdf, EnumUtility.ToString(typeof (ReportType), ReportType.Pdf) },
					})
					.ToDictionary()))
				.mode("local")
				.width(60)
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true)
				.value(ReportType.Excel)
				.ToDictionary());

			_onlyWithInvoices = new Checkbox(new CheckboxConfig()
				.hideLabel(true)
				.boxLabel("Включать только документы со счетами / квитанциями")
				.ToDictionary());

			Form.add(_fromField);
			Form.add(_toField);
			Form.add(_paymentTypeField);
			Form.add(_paymentFormField);
			Form.add(_airlineSelector.Widget);
			Form.add(_reportTypeField);
			Form.add(_onlyWithInvoices);
		}

		protected override void OnSave()
		{
			Dictionary @params = new Dictionary();

			@params["from"] = GetDate(_fromField);
			@params["to"] = GetDate(_toField);
			@params["paymentType"] = _paymentTypeField.getValue();
			@params["paymentForm"] = _paymentFormField.getValue();
			@params["onlyWithInvoices"] = _onlyWithInvoices.getValue();

			object o = _airlineSelector.GetObjectInfo() != null ? _airlineSelector.GetObjectInfo().Id : null;

			if (o != null)
				@params["airline"] = o;


			ReportType value = (ReportType)(object)_reportTypeField.getValue();
			@params["reportType"] = value;
			string extention = value == ReportType.Pdf ? "pdf" : "xls";

			string url = string.Format("reports/registry/Registry_report_{0}.{1}", Date.Now.Format("Y-m-d_H-i"), extention);

			ReportLoader.Load(url, @params);

			CompleteSave(null);
		}

		private static string GetDate(DateField field)
		{
			if ((string)((Field)field).getValue() == string.Empty)
				return null;

			Date dateTime = field.getValue();

			return Script.IsNullOrUndefined(dateTime) ? null : dateTime.Format("Y-m-d");
		}

		private DateField _fromField;
		private DateField _toField;
		private ComboBox _paymentTypeField;
		private ComboBox _paymentFormField;
		private ObjectSelector _airlineSelector;
		private ComboBox _reportTypeField;
		private Checkbox _onlyWithInvoices;

		private readonly Button _createReportButton;
		private readonly Button _cancelButton;
	}
}