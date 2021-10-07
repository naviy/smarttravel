using System;
using System.Collections;

using Ext;
using Ext.form;

using jQueryApi;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;


namespace Luxena.Travel
{
	public class ConsignmentEditForm : BaseClassEditForm
	{
		static ConsignmentEditForm()
		{
			FormsRegistry.RegisterEdit(ClassNames.Consignment, EditObject);
		}

		private ConsignmentEditForm(EditFormArgs args, ItemConfig itemConfig) : base(args, itemConfig)
		{
			Window.setWidth(550);
		}

		protected override void LoadInstance(AjaxCallback onLoaded)
		{
			ConsignmentService.GetConsignment(Args.IdToLoad, onLoaded, null);
		}

		protected override void SetFieldValues()
		{
			if (Consignment == null)
			{
				if (!Script.IsNullOrUndefined(Args.FieldValues))
				{
					if (Args.FieldValues.ContainsKey("IsModified"))
						_itemsGrid.IsModified = true;

					if (Args.FieldValues.ContainsKey("Number"))
						_numberField.setValue(Args.FieldValues["Number"]);

					if (Args.FieldValues.ContainsKey("Discount"))
						_discount.setValue((MoneyDto) Args.FieldValues["Discount"]);

					if (Args.FieldValues.ContainsKey("Acquirer"))
						_acquirerField.Widget.setValue(Args.FieldValues["Acquirer"]);

					if (Args.FieldValues.ContainsKey("Supplier"))
						_supplierField.Widget.setValue(Args.FieldValues["Supplier"]);

					if (Args.FieldValues.ContainsKey("TotalSupplied"))
						_totalSuppliedField.setValue(Args.FieldValues["TotalSupplied"]);

					if (Args.FieldValues.ContainsKey("IssueDate"))
						_issueDateField.setValue(Args.FieldValues["IssueDate"]);

					if (Args.FieldValues.ContainsKey("Items"))
					{
						_itemsGrid.SetInitialData((object[]) Args.FieldValues["Items"]);
					}

					ConsignmentFinanceDataChanged();
				}

				return;
			}

			_numberField.setValue(Consignment.Number);
			_issueDateField.setValue(Consignment.IssueDate);
			_supplierField.SetValue(Consignment.Supplier);
			_acquirerField.SetValue(Consignment.Acquirer);
			_totalSuppliedField.setValue(Consignment.TotalSupplied);
			_discount.setValue(Consignment.Discount);
			_grandTotal.setValue(Consignment.GrandTotal);
			_vat.setValue(Consignment.Vat);
			_itemsGrid.SetInitialData(Consignment.Items);
		}

		protected override void OnSave()
		{
			if (IsModified() || _itemsGrid.IsModified)
				ConsignmentService.UpdateConsignment(GetConsignment(), Args.RangeRequest, CompleteSave, null);
			else
				Cancel();
		}

		protected override void OnSaved(object result)
		{
			ConsignmentDto consignment = ((ConsignmentDto) ((ItemResponse) result).Item);

			if (!Script.IsNullOrUndefined(consignment))
				ReportPrinter.GetLastIssuedConsignment(consignment.Id, consignment.Number);

			base.OnSaved(result);
		}

		protected override Field[] AddFields()
		{
			_numberField = new TextField(new TextFieldConfig()
				.selectOnFocus(true)
				.width(93)
				.fieldLabel(DomainRes.Consignment_Number)
				.ToDictionary());

			_issueDateField = ControlFactory.CreateEditor(GetFieldConfig("IssueDate"));
			_issueDateField.setValue(Date.Today);

			_discount = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Consignment_Discount).SetAmountChangeHandler(DiscountChanged));
			_grandTotal = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Consignment_GrandTotal));
			_vat = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Consignment_Vat));
			_vat.DecimalField.disable();
			_grandTotal.DecimalField.disable();
			_vat.DecimalField.readOnly = true;


			_supplierField = new ObjectSelector((ObjectSelectorConfig) new ObjectSelectorConfig()
				.setClass("Party")
				.allowCreate(false)
				.setDataProxy(PartyService.SuggestCustomersProxy())
				.fieldLabel(DomainRes.Consignment_Supplier)
				.selectOnFocus(true)
				.forceSelection(true)
				.width(170)
				.allowBlank(false));

			_acquirerField = new ObjectSelector((ObjectSelectorConfig) new ObjectSelectorConfig()
				.setClass("Party")
				.allowCreate(false)
				.setDataProxy(PartyService.SuggestCustomersProxy())
				.fieldLabel(DomainRes.Consignment_Acquirer)
				.selectOnFocus(true)
				.forceSelection(true)
				.width(170)
				.allowBlank(false));

			_totalSuppliedField = ControlFactory.CreateEditor(GetFieldConfig("TotalSupplied"));
			_totalSuppliedField.setWidth(170);

			_itemsGrid = new ConsignmentItemGridControl(200, true);
			_itemsGrid.FinanceDataChanged += ConsignmentFinanceDataChanged;

			ArrayList fields = new ArrayList();

			fields.Add(_supplierField.Widget);
			fields.Add(_acquirerField.Widget);
			fields.Add(_totalSuppliedField);
			fields.Add(_numberField);
			fields.Add(_issueDateField);
			fields.Add(_discount);
			fields.Add(_grandTotal);
			fields.Add(_vat);

			Panel pnlAttributes = new Panel(new PanelConfig()
				.items(new Component[]
					{
					new Panel(new PanelConfig()
						.items(new Component[] { _supplierField.Widget, _acquirerField.Widget, _totalSuppliedField })
						.layout("form")
						.ToDictionary()),
					new Panel(new PanelConfig()
						.items(new Component[] { _numberField,  _issueDateField })
						.layout("form")
						.style("float: right;")
						.ToDictionary())
					}
				)
				.layout("column")
				.width(500)
				.ToDictionary());

			Panel pnlItems = new Panel(new PanelConfig()
				.items(new Component[] { _itemsGrid })
				.layout("form")
				.cls("positions")
				.ToDictionary());

			Panel pnlFinance = new Panel(new PanelConfig()
				.items(new Component[] { _discount, _grandTotal, _vat })
				.cls("consignment-finance")
				.layout("form")
				.autoWidth(true)
				.ToDictionary());

			Form.add(pnlAttributes);
			Form.add(pnlItems);
			Form.add(pnlFinance);

			return (Field[]) fields;
		}

		private ConsignmentDto Consignment
		{
			get { return (ConsignmentDto) Instance; }
		}

		private static void EditObject(EditFormArgs args)
		{
			ConfigManager.GetEditConfig(args.Type,
				delegate(ItemConfig config)
				{
					ConsignmentEditForm form = new ConsignmentEditForm(args, config);
					form.Open();
				});
		}

		private ConsignmentDto GetConsignment()
		{
			ConsignmentDto dto = new ConsignmentDto();

			if (Consignment != null)
			{
				dto.Id = Consignment.Id;
				dto.Version = Consignment.Version;
			}

			dto.Number = _numberField.getValue().ToString();

			if (dto.Number == string.Empty)
				dto.Number = null;

			dto.TotalSupplied = _totalSuppliedField.getValue().ToString();

			if (dto.TotalSupplied == string.Empty)
				dto.TotalSupplied = null;

			dto.Supplier = _supplierField.GetObjectInfo();
			dto.Acquirer = _acquirerField.GetObjectInfo();
			dto.IssueDate = (Date) _issueDateField.getValue();
			dto.Discount = _discount.getValue();
			dto.GrandTotal = _grandTotal.getValue();
			dto.Vat = _vat.getValue();

			OrderItemDto[] gridItems = _itemsGrid.Items;
			dto.Items = new OrderItemDto[gridItems.Length];
			for (int i = 0; i < gridItems.Length; i++)
			{
				dto.Items[i] = new OrderItemDto();
				dto.Items[i].Id = gridItems[i].Id;
				dto.Items[i].Version = gridItems[i].Version;
				dto.Items[i].Text = gridItems[i].Text;
				dto.Items[i].GrandTotal = MoneyDto.CopyMoney(gridItems[i].GrandTotal);
			}

			return dto;
		}

		private void ConsignmentFinanceDataChanged()
		{
			_discount.setValue(_itemsGrid.Discount);
			_grandTotal.setValue(_itemsGrid.GrandTotal);
			_vat.setValue(_itemsGrid.Vat);
		}

		private void DiscountChanged(Field decimalField, object newValue, object oldValue)
		{
			MoneyDto grandTotal = _grandTotal.getValue();
			grandTotal.Amount += (decimal) oldValue - (decimal) newValue;
			_grandTotal.setValue(grandTotal);

			MoneyDto vat = _vat.getValue();

			decimal oldDiscountVat = (decimal) oldValue - ((decimal) oldValue*100)/(100 + AppManager.SystemConfiguration.VatRate);
			decimal newDiscountVat = (decimal) newValue - ((decimal) newValue*100)/(100 + AppManager.SystemConfiguration.VatRate);

			vat.Amount += oldDiscountVat - newDiscountVat;

			if (vat.Amount < 0)
				vat.Amount = 0;

			_vat.setValue(vat);
		}

		private Field _issueDateField;
		private Field _numberField;
		private ObjectSelector _supplierField;
		private ObjectSelector _acquirerField;
		private Field _totalSuppliedField;
		private MoneyControl _discount;
		private MoneyControl _grandTotal;
		private MoneyControl _vat;
		private ConsignmentItemGridControl _itemsGrid;
	}
}