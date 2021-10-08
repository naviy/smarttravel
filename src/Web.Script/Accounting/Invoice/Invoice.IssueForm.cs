using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.form;

using LxnBase;
using LxnBase.UI;

using Luxena.Travel.Services;

using ComboBox = Ext.form.ComboBox;
using Field = Ext.form.Field;




namespace Luxena.Travel
{



	//===g






	public class InvoiceIssueForm : BaseEditForm
	{

		//---g



		private readonly object _orderId;
		private readonly string[] _invoices;

		private DateField _issueDate;
		private Field _number;
		private ComboBox _formNumber;
		private Checkbox _showPayments;
		private InvoiceType _type;



		//---g



		public InvoiceIssueForm(object orderId, string[] invoices, InvoiceType type)
		{
			_orderId = orderId;
			_invoices = invoices;
			_type = type;
		}



		public void Open()
		{

			Button saveButton = Form.addButton(Res.Issue, new AnonymousDelegate(Save));

			Button cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			_issueDate = new DateField(new DateFieldConfig()
				.fieldLabel(Res.Invoice_IssueDate)
				.format("d.m.Y")
				.width(100)
				.value(Date.Today)
				.allowBlank(false)
				.ToDictionary())
			;


			if (_invoices == null || _invoices.Length == 0)
			{
				_number = new TextField(new TextFieldConfig()
					.fieldLabel(DomainRes.Common_Number)
					.emptyText(Res.Auto)
					.emptyClass("auto-text")
					.width(170)
					.ToDictionary())
				;
			}
			else
			{

				object[] data = new object[_invoices.Length];

				for (int i = 0; i < _invoices.Length; i++)
				{
					data[i] = new object[] { _invoices[i] };
				}


				_number = new ComboBox(new ComboBoxConfig()
					.fieldLabel(DomainRes.Common_Number)
					.store(new ArrayStore(new ArrayStoreConfig()
						.data(data)
						.fields(new string[] { "number" })
						.ToDictionary()))
					.mode("local")
					.displayField("number")
					.valueField("number")
					.tpl(new XTemplate("<tpl for=\".\"><div class='x-combo-list-item invoice-reissue-action'>" + Res.Invoice_Reissue + "{number}</div></tpl>"))
					.emptyText(Res.Auto)
					.emptyClass("auto-text")
					.width(170)
					.ToDictionary())
				;

			}


			_showPayments = new Checkbox(new CheckboxConfig()
				.boxLabel(Res.InvoiceIssueForm_ShowPaid)
				.ToDictionary())
			;


			_formNumber = new ComboBox(new ComboBoxConfig()
				.fieldLabel("Форма счёта")
				.store(new ArrayStore(new ArrayStoreConfig()
					.fields(new string[] { "number", "name" })
					.data(new object[]
					{
						new object[] { 1, "Форма 1" },
						new object[] { 2, "Форма 2" }
					})
					.ToDictionary()
				))
				.mode("local")
				.displayField("name")
				.valueField("number")
				.triggerAction("all")
				.selectOnFocus(true)
				.editable(false)
				.emptyText("по умолчанию")
				.emptyClass("auto-text")
				.width(170)
				.ToDictionary())
			;


			Component[] fields = { _issueDate, _number, _formNumber, _showPayments };

			Form.add(fields);

			ArrayList list = new ArrayList();

			list.AddRange(fields);
			list.AddRange(new Component[] { saveButton, cancelButton });

			ComponentSequence = (Component[])list;


			string title = _type == InvoiceType.CompletionCertificate
				? Res.IssueCompletionCertificate_Title
				: Res.IssueInvoice_Title
			;

			Window.setTitle(title);

			Window.setWidth(370);

			Window.addClass("invoice-issue");


			Window.show();

		}



		protected override void OnSave()
		{

			string number = (string)_number.getValue();

			if (number == string.Empty)
				number = null;


			if (_type == InvoiceType.CompletionCertificate)
			{
				OrderService.IssueCompletionCertificate(
					_orderId,
					number,
					_issueDate.getValue(),
					_showPayments.getValue(),
					CompleteSave, null
				);
			}
			else
			{
				OrderService.IssueInvoice(
					_orderId,
					number,
					_issueDate.getValue(),
					Number.ParseInt(_formNumber.getValue()),
					_showPayments.getValue(),
					CompleteSave, null
				);
			}

		}



		//---g

	}






	//===g



}