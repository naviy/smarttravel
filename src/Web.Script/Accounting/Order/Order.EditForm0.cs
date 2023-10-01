using System;
using System.Collections;
using System.Collections.Generic;

using Ext;
using Ext.form;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.Controls;

using Luxena.Travel.Cfg;
using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using ComboBox = LxnBase.UI.Controls.ComboBox;
using Field = Ext.form.Field;




namespace Luxena.Travel
{



	//===g






	public class OrderEditForm : BaseClassEditForm
	{

		//---g



		static OrderEditForm()
		{
			FormsRegistry.RegisterEdit(ClassNames.Order, EditObject);
		}



		public static void EditObject(EditFormArgs args)
		{

			ConfigManager.GetEditConfig(args.Type,
				delegate (ItemConfig config)
				{
					OrderEditForm form = new OrderEditForm(args, config);
					form.Open();
				})
			;

		}



		//---g



		private OrderEditForm(EditFormArgs args, ItemConfig config)
			: base(args, config)
		{
			Window.addClass("order-edit");
		}



		//---g



		public bool SeparateServiceFee
		{
			get { return _separateServiceFee.getValue(); }
		}


		public Reference Customer
		{
			get { return _customer.GetObjectInfo(); }
			set { _customer.SetValue(value); }
		}


		public bool CanChangeVat
		{
			get { return GetCustomActionStatus("ChangeVat").Visible; }
		}


		private OrderDto Order
		{
			get { return (OrderDto)Instance; }
		}



		//---g



		protected override void LoadInstance(AjaxCallback onLoaded)
		{
			OrderService.GetOrder(Args.IdToLoad, onLoaded, null);
		}



		//---g



		protected override Field[] AddFields()
		{

			InitFields();

			InitLayout();


			ArrayList fields = new ArrayList();


			fields.AddRange(new object[]
			{
				_issueDate,
				_number,
				_customer.Widget,
				_billTo.Widget,
				_shipTo.Widget,
				_intermediary.Widget,
				_owner,
				_assignedTo.Widget,
				_bankAccount,
				_bonusDate,
				_bonusSpentAmount,
				_bonusRecipient.Widget,
				_isPublic,
				_allowAddProductsInClosedPeriod,
				_isSubjectOfPaymentsControl,
				_note,
				_discount,
				_total
			});


			if (CanChangeVat)
			{
				fields.Add(_vat);
				fields.Add(_useServiceFeeOnlyInVat);
			}


			return (Field[])fields;

		}



		private void InitFields()
		{

			_issueDate = (DateField)CreateEditor("IssueDate");
			_issueDate.setValue(Date.Today);
			
			_number = (TextField)CreateEditor("Number");
			_number.allowBlank = true;

			_customer = ControlFactoryExt.CreateCustomerControl(DomainRes.Common_Customer, 200, false);

			_billTo = ControlFactoryExt.CreateCustomerControlWithText(DomainRes.Common_BillTo, 200, true);

			_shipTo = ControlFactoryExt.CreateCustomerControl(DomainRes.Common_ShipTo, 200, true);

			_intermediary = ControlFactoryExt.CreateCustomerControl(DomainRes.Common_Intermediary, 200, true);

			_owner = ControlFactoryExt.CreateOwnerControl(200, false);

			_assignedTo = ControlFactoryExt.CreateAssignedToControl(DomainRes.Common_AssignedTo, 200, false);
			_assignedTo.SetValue(AppManager.CurrentPerson);

			_bankAccount = ControlFactoryExt.CreateBankAccountControl(200);

			_bonusDate = new DateField(new DateFieldConfig()
				.fieldLabel("Дата начисления")
				.labelStyle("width: 140px")
				.ToDictionary());

			_bonusSpentAmount = new DecimalField(new NumberFieldConfig()
				.fieldLabel("Списано бонусов")
				.labelStyle("width: 140px")
				.ToDictionary());

			_bonusRecipient = new ObjectSelector(
				(ObjectSelectorConfig)ControlFactoryExt.CreateCustomerConfig("Получатель бонусов", 200, true)
				.labelStyle("width: 140px")
			);

			_isPublic = new Checkbox(new CheckboxConfig()
				.boxLabel(DomainRes.Order_IsPublic)
				.ToDictionary());

			_allowAddProductsInClosedPeriod = new Checkbox(new CheckboxConfig()
				.boxLabel("Разрешить добавление билетов, даже если заказ находится в закрытом периоде")
				.ToDictionary());

			_isSubjectOfPaymentsControl = new Checkbox(new CheckboxConfig()
				.boxLabel(DomainRes.Order_IsSubjectOfPaymentsControl)
				.ToDictionary());

			_isSubjectOfPaymentsControl.setValue(true);

			_separateServiceFee = new Checkbox(new CheckboxConfig()
				.boxLabel(Res.Order_SeparateServiceFee)
				.ToDictionary());

			_separateServiceFee.setValue(true);
			_separateServiceFee.setVisible(AppManager.SystemConfiguration.AviaOrderItemGenerationOption == AviaOrderItemGenerationOption.ManualSetting);

			_discount = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_Discount, true));
			_total = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_Total, true));
			_vat = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Order_Vat, true));

			_useServiceFeeOnlyInVat = new Checkbox(new CheckboxConfig()
				.boxLabel(DomainRes.Order_UseServiceFeeOnlyInVat)
				.ToDictionary()
			);
			_useServiceFeeOnlyInVat.setValue(AppManager.SystemConfiguration.Order_UseServiceFeeOnlyInVat);
			_useServiceFeeOnlyInVat.on("check", new CheckboxCheckDelegate(OnUseServiceFeeOnlyInVatChange));

			_vatString = new BoxComponent(new BoxComponentConfig()
				.cls("x-form-item vat")
				.ToDictionary());

			_itemsControl = new OrderItemGridControl(227);
			_itemsControl.UseServiceFeeOnlyInVat = AppManager.SystemConfiguration.Order_UseServiceFeeOnlyInVat;
			_itemsControl.FinanceDataChanged += OrderFinanceDataChanged;
			_itemsControl.SetParentForm(this);

			_note = new TextArea(new TextAreaConfig()
				.fieldLabel(DomainRes.Common_Note)
				.width(220)
				.height(98)
				.ToDictionary());

		}



		private void OnUseServiceFeeOnlyInVatChange(Field field, bool newValue)
		{
			_itemsControl.UseServiceFeeOnlyInVat = newValue;
		}



		private void InitLayout()
		{

			Panel attributes = new Panel(new PanelConfig()
				.items(new Component[]
					{
						new Panel(new PanelConfig()
							.items(new object[]
							{
								_issueDate,
								_number,
								_customer.Widget,
								_billTo.Widget,
								_shipTo.Widget,
								_intermediary.Widget,
								_note,
							})
							.layout("form")
							.cls("attributes")
							.ToDictionary()),
						new Panel(new PanelConfig()
							.items(new Component[]
							{
								_owner,
								_assignedTo.Widget,
								_bankAccount,
								_isPublic,
								_allowAddProductsInClosedPeriod,
								_isSubjectOfPaymentsControl,
								_separateServiceFee,
								_bonusSpentAmount,
								_bonusRecipient.Widget,
								_bonusDate,
							})
							.layout("form")
							.cls("attributes")
							.ToDictionary())
					}
				)
				.layout("column")
				.ToDictionary());


			Panel orderItems = new Panel(new PanelConfig()
				.items(new Component[] { _itemsControl })
				.cls("items")
				.ToDictionary()
			);


			List<Component> financeItems = new List<Component>(_discount, _total);


			if (CanChangeVat)
			{
				financeItems.Add(_vat);
				financeItems.Add(_useServiceFeeOnlyInVat);
			}


			Panel finances = new Panel(new PanelConfig()
				.items(financeItems)
				.cls("totals")
				.autoWidth(true)
				.layout("form")
				.ToDictionary()
			);

			Form.add(attributes);
			Form.add(orderItems);
			Form.add(finances);

		}



		protected override void SetFieldValues()
		{

			if (Order == null)
			{

				if (!Script.IsNullOrUndefined(Args.FieldValues))
				{

					if (Args.FieldValues.ContainsKey("SeparateServiceFee"))
					{
						_separateServiceFee.setValue((bool)Args.FieldValues["SeparateServiceFee"]);
					}


					if (Args.FieldValues.ContainsKey("AviaDocuments"))
					{
						_itemsControl.TryAddDocuments((object[])Args.FieldValues["AviaDocuments"]);
					}


					if (Args.FieldValues.ContainsKey("Customer"))
					{
						_customer.SetValue(Args.FieldValues["Customer"]);
					}

				}


				_issueDate.setValue(Date.Today);


				return;
			}


			_issueDate.setValue(Order.IssueDate);
			_number.setValue(Order.Number);

			_customer.SetValue(Order.Customer);

			if (Order.BillTo != null && Order.BillTo.Id == null)
				Order.BillTo.Id = "";

			_billTo.SetValue(Order.BillTo);

			_shipTo.SetValue(Order.ShipTo);
			_intermediary.SetValue(Order.Intermediary);

			_itemsControl.SetInitialData((string)Order.Id, Order.Items);
			_discount.setValue(Order.Discount);
			_total.setValue(Order.Total);


			if (CanChangeVat)
			{
				_vat.setValue(Order.Vat);
				_useServiceFeeOnlyInVat.setValue(Order.UseServiceFeeOnlyInVat);
			}
			else
				UpdateVatString(Order.Vat);


			_owner.setValue(Order.Owner);
			_assignedTo.SetValue(Order.AssignedTo);
			_bankAccount.setValue(Order.BankAccount);


			if (!Order.CanChangeAssignedTo)
				_assignedTo.Widget.setDisabled(true);


			_bonusDate.setValue(Order.BonusDate);
			_bonusSpentAmount.setValue(Order.BonusSpentAmount);
			_bonusRecipient.SetValue(Order.BonusRecipient);

			_isPublic.setValue(Order.IsPublic);
			_allowAddProductsInClosedPeriod.setValue(Order.AllowAddProductsInClosedPeriod);
			_isSubjectOfPaymentsControl.setValue(Order.IsSubjectOfPaymentsControl);

			_note.setValue(Order.Note);

		}



		protected override void OnSave()
		{

			if (IsModified() || _itemsControl.IsModified || Order.BillTo != null && _billTo.Text != Order.BillTo.Name)
			{
				OrderService.UpdateOrder(GetOrder(), Args.RangeRequest, OnSaveComplete, null);
			}
			else
			{
				Cancel();
			}

		}



		private void OnSaveComplete(object result)
		{

			ItemResponse response = (ItemResponse)result;

			if (response.Errors == null)
			{
				CompleteSave(response);

				return;
			}


			MessageFactory.DocumentsAlreadyAddedToOrder(Res.Order_SaveError_Msg_Title, Res.Order_CannotRestoreOrder_Msg, (OrderItemDto[])response.Errors);

		}



		protected override void OnSaved(object result)
		{

			if (Script.IsNullOrUndefined(Args.RangeRequest))
			{
				OrderDto dto = (OrderDto)((ItemResponse)result).Item;

				MessageRegister.Info(InstanceConfig.ListCaption,
					string.Format("{0} {1}", (Args.IsNew ? BaseRes.Created : BaseRes.Updated), ObjectLink.Render(dto.Id, dto.Number, ClassNames.Order)));
			}


			base.OnSaved(result);

		}



		private OrderDto GetOrder()
		{

			OrderDto dto = new OrderDto();


			if (Order != null)
			{
				dto.Id = Order.Id;
				dto.Version = Order.Version;
			}


			dto.IssueDate = _issueDate.getValue();
			dto.Number = _number.getValue().ToString();

			dto.Customer = _customer.GetObjectInfo();
			dto.BillTo = _billTo.GetObjectInfo();


			if (dto.BillTo == null && !string.IsNullOrEmpty(_billTo.Text))
				dto.BillTo = new Reference();

			if (dto.BillTo != null)
				dto.BillTo.Name = _billTo.Text;


			dto.ShipTo = _shipTo.GetObjectInfo();
			dto.Intermediary = _intermediary.GetObjectInfo();

			dto.Items = _itemsControl.Items;

			dto.Discount = _discount.getValue();
			dto.Total = _total.getValue();

			dto.Vat = CanChangeVat ? _vat.getValue() : _itemsControl.Vat;
			dto.UseServiceFeeOnlyInVat = _useServiceFeeOnlyInVat.getValue();

			dto.Owner = _owner.GetObjectInfo();
			dto.AssignedTo = _assignedTo.GetObjectInfo();
			dto.BankAccount = _bankAccount.GetObjectInfo();

			dto.BonusDate = _bonusDate.getValue();
			dto.BonusSpentAmount = (Number)_bonusSpentAmount.getValue();
			dto.BonusRecipient = _bonusRecipient.GetObjectInfo();

			dto.IsPublic = _isPublic.getValue();
			dto.AllowAddProductsInClosedPeriod = _allowAddProductsInClosedPeriod.getValue();
			dto.IsSubjectOfPaymentsControl = _isSubjectOfPaymentsControl.getValue();

			string note = (string)_note.getValue();
			dto.Note = string.IsNullOrEmpty(note) ? null : note;


			return dto;

		}



		private void OrderFinanceDataChanged()
		{

			_discount.setValue(_itemsControl.Discount);
			_total.setValue(_itemsControl.GrandTotal);

			if (CanChangeVat)
				_vat.setValue(_itemsControl.Vat);
			else
				UpdateVatString(_itemsControl.Vat);

		}



		private void UpdateVatString(MoneyDto vat)
		{

			string html = string.Format(
				@"<label class='x-form-item-label'>{0}:</label>
				<div class='x-form-element'>
					<div class='text'>
						<div class='amount'>{1}</div>
						<div class='currency'>{2}</div>
					</div>
				</div>
				<div class='x-form-clear-left'></div>",
				DomainRes.Order_Vat, vat.Amount.Format("N2"), vat.Currency.Name);


			if (_vatString.rendered)
				_vatString.getEl().update(html);
			else
				_vatString.autoEl = new Dictionary("html", html);

		}



		//---g



		private OrderItemGridControl _itemsControl;

		private DateField _issueDate;
		private TextField _number;

		private ObjectSelector _customer;
		private ObjectSelector _billTo;
		private ObjectSelector _shipTo;
		private ObjectSelector _intermediary;

		private ComboBox _owner;
		private ObjectSelector _assignedTo;
		private ComboBox _bankAccount;
		private Checkbox _isPublic;
		private Checkbox _allowAddProductsInClosedPeriod;
		private Checkbox _isSubjectOfPaymentsControl;
		private Checkbox _separateServiceFee;
		private Checkbox _useServiceFeeOnlyInVat;

		private TextArea _note;

		private MoneyControl _total;
		private MoneyControl _discount;
		private MoneyControl _vat;
		private BoxComponent _vatString;

		private DateField _bonusDate;
		private DecimalField _bonusSpentAmount;
		private ObjectSelector _bonusRecipient;



		//---g

	}






	//===g



}