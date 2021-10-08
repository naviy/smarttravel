using System;

using Ext;
using Ext.form;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;

using Luxena.Travel.Controls;


namespace Luxena.Travel
{
	public class OrderItemEditForm : BaseClassEditForm
	{
		static OrderItemEditForm()
		{
			FormsRegistry.RegisterEdit(ClassNames.OrderItem, EditObject);
		}

		private OrderItemEditForm(EditFormArgs args, ItemConfig itemConfig)
			: base(args, itemConfig)
		{
			args.Mode = LoadMode.Local;
			Window.addClass("orderitem-edit");
		}

		public static void EditObject(EditFormArgs args)
		{
			ConfigManager.GetEditConfig(args.Type,
				delegate (ItemConfig config)
				{
					OrderItemEditForm form = new OrderItemEditForm(args, config);
					form.Open();
				});
		}


		public OrderItemDto r { get { return (OrderItemDto)Instance; } }


		protected override Field[] AddFields()
		{
			_textField = CreateEditor("Text");

			Button addProductTextButton = Form.addButton("Добавить название услуги", new AnonymousDelegate(AddProductText));

			Panel panel1 = new Panel(new PanelConfig()
				.items(new object[] { addProductTextButton })
				.cls("add-product-text-button")
				.ToDictionary());


			_priceField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.OrderItem_Price, true));
			_priceField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));


			if (!AppManager.SystemConfiguration.UseDefaultCurrencyForInput)
				_priceField.CurrencySelector.Widget.on("change", new FieldChangeDelegate(OnCurrencyChange));


			_quantityField = (NumberField)CreateEditor("Quantity");
			_quantityField.on("change", new FieldChangeDelegate(OnAmountChange));
			_quantityField.setValue(1);
			_quantityField.allowBlank = false;
			_quantityField.cls = "number";


			_discountField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.OrderItem_Discount, true));
			_discountField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			if (!AppManager.SystemConfiguration.UseDefaultCurrencyForInput)
				_discountField.CurrencySelector.Widget.on("change", new FieldChangeDelegate(OnCurrencyChange));

			MoneyDto money = new MoneyDto();
			money.Currency = AppManager.SystemConfiguration.DefaultCurrency;


			_grandTotalField = new DisplayField(new DateFieldConfig()
				.fieldLabel(DomainRes.OrderItem_GrandTotal)
				.value(MoneyDto.ToMoneyFullString(money))
				.ToDictionary());


			_hasVatField = CreateEditor("HasVat");


			Field[] fields =
			{
				_textField,
				_priceField,
				_quantityField,
				_discountField,
				_grandTotalField,
				_hasVatField
			};


			Form.add(new object[] {
				_textField,
				panel1,
				_priceField,
				_quantityField,
				_discountField,
				_grandTotalField,
				_hasVatField
			});


			return fields;
		}


		private void AddProductText()
		{
			string value = _textField.getValue() as string;

			if (string.IsNullOrEmpty(value))
				value = "";
			else
				value += " ";

			_textField.setValue(value + r.ProductText);
		}


		private void OnAmountChange(Field field, object newvalue, object oldvalue)
		{
			MoneyDto money = CalculateGrandTotal();

			_grandTotalField.setValue(MoneyDto.ToMoneyFullString(money));
		}

		private void OnCurrencyChange(Field objthis, object newvalue, object oldvalue)
		{
			if (objthis == _priceField.CurrencySelector.Widget)
				_discountField.CurrencySelector.SetValue(_priceField.Currency);
			else
				_priceField.CurrencySelector.SetValue(_discountField.Currency);

			MoneyDto money = CalculateGrandTotal();

			_grandTotalField.setValue(MoneyDto.ToMoneyFullString(money));
		}

		private bool CanCaluculate()
		{
			bool res =
				_priceField.getValue() != null &&
				_discountField.getValue() != null &&
				_quantityField.getValue() != null &&
				(string)_quantityField.getValue() != string.Empty;


			return res && Reference.Equals(_priceField.Currency, _discountField.Currency);
		}

		private MoneyDto CalculateGrandTotal()
		{
			if (!CanCaluculate())
				return MoneyDto.GetZeroMoney();

			MoneyDto money = new MoneyDto();
			money.Amount = _priceField.Amount * (int)_quantityField.getValue() - _discountField.Amount;
			money.Currency = _priceField.Currency;

			return money;
		}

		protected override void LoadInstance(AjaxCallback onLoaded)
		{
		}

		protected override void SetFieldValues()
		{
			if (r == null) return;

			_textField.setValue(r.Text);
			_priceField.setValue(r.Price);
			_quantityField.setValue(r.Quantity);
			_discountField.setValue(r.Discount);
			_grandTotalField.setValue(MoneyDto.ToMoneyFullString(r.GrandTotal));

			_hasVatField.setValue(r.HasVat);
		}

		protected override void OnSave()
		{
			OrderItemDto dto = new OrderItemDto();
			if (r != null)
			{
				dto.LinkType = r.LinkType;
				dto.Product = r.Product;
			}
			dto.Text = (string)_textField.getValue();
			dto.Price = _priceField.getValue() ?? MoneyDto.GetZeroMoney();
			dto.Quantity = (int)_quantityField.getValue();
			dto.Discount = _discountField.getValue() ?? MoneyDto.GetZeroMoney();
			dto.GrandTotal = CalculateGrandTotal() ?? MoneyDto.GetZeroMoney();
			dto.Total = MoneyDto.GetZeroMoney(dto.Price.Currency);
			dto.Total.Amount = dto.Price.Amount * dto.Quantity;

			dto.TaxedTotal = MoneyDto.CopyMoney(dto.Total);

			dto.HasVat = (bool)_hasVatField.getValue();

			CompleteSave(ItemResponse.Create(dto));
		}

		private Field _textField;
		private MoneyControl _priceField;
		private NumberField _quantityField;
		private MoneyControl _discountField;
		private DisplayField _grandTotalField;
		private Field _hasVatField;
	}
}