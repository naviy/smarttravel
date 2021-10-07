using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.form;
using Ext.menu;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI.Controls;


namespace Luxena.Travel.Controls
{
	public class MoneyControl : Field
	{
		[AlternateSignature]
		// ReSharper disable once UnusedParameter.Local
		public extern MoneyControl(FieldConfig config);

		public MoneyControl(MoneyControlConfig config) : base(config.ToDictionary())
		{
		}

		protected override void initComponent()
		{
			if (!Script.IsValue(_args.DefaultCurrency))
			{
				if (Script.IsValue(_args.InitValue))
					_args.DefaultCurrency = Reference.Copy(_args.InitValue.Currency);
				else
					_args.DefaultCurrency = Reference.Copy(AppManager.SystemConfiguration.DefaultCurrency);
			}
			_decimalField = new DecimalField(new NumberFieldConfig()
				.cls("money-amount")
				.decimalSeparator(",")
				.baseChars("0123456789.")
				.selectOnFocus(true)
				.value(0)
				.listeners(new Dictionary(
					"focus", new AnonymousDelegate(delegate { fireEvent("focus"); }),
					"change", new FieldChangeDelegate(OnAmountChange)//,
					//"keypress", new TextFieldKeypressDelegate(FieldKeyPress)
				))
				.allowBlank(_args.AllowBlank)
				.ToDictionary());

			if (IsCurrencyReadOnly)
			{
				_currencyLabel = new Label(new LabelConfig()
					.text(_args.DefaultCurrency.Name)
					.cls("money-currency")
					.ToDictionary());
			}
			else
			{
				_currencySelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
					.setClass("Currency")
					.hideLabel(true)
					.cls("money-currency")
					.selectOnFocus(true)
					.allowBlank(_args.AllowBlank)
					.forceSelection(true));
			}

			if (Script.IsValue(_args.InitValue))
				setValue(_args.InitValue);
			else if (!IsCurrencyReadOnly && _args.DefaultCurrency != null)
				_currencySelector.SetValue(_args.DefaultCurrency);

			if (_args.MenuConfig != null)
			{
				_args.MenuConfig
					.cls("simple-menu")
					.listeners(new Dictionary("beforeshow", new MenuBeforeshowDelegate(
						delegate
						{
							_menu.tryActivate(0);
						})));

				_menu = new Menu(_args.MenuConfig.ToDictionary());
			}


			base.initComponent();
		}

		//private void FieldKeyPress(object sender, EventObject e)
		//{
		//	Script.Alert("FieldKeyPress");

		//	Component sibling = !IsCurrencyReadOnly ? _currencySelector.Widget.nextSibling() : _decimalField.nextSibling();

		//	if (sibling != null)
		//		sibling.focus();
		//}

		public DecimalField DecimalField { get { return _decimalField; } }

		public ObjectSelector CurrencySelector { get { return _currencySelector; } }

		public decimal Amount
		{
			get
			{
				MoneyDto dto = getValue();
				return dto != null ? dto.Amount : 0;
			}
			set
			{
				_decimalField.setValue(value);
			}
		}

		public Reference Currency
		{
			get
			{
				MoneyDto dto = getValue();
				return dto != null ? dto.Currency : null;
			}
			set
			{
				if (!IsCurrencyReadOnly)
					_currencySelector.SetValue(value);
				else
				{
					_currency = Reference.Copy(value);

					_currencyLabel.setText(value.Name);
				}
			}
		}

		private bool IsCurrencyReadOnly
		{
			get { return _args.IsCurrencyReadonly && _args.DefaultCurrency != null; }
		}

		protected override void onRender(object container, object position)
		{
			object pos = Script.IsUndefined(position) ? null : position;

			if (Script.IsNullOrUndefined(el))
			{
				ArrayList items = new ArrayList();
				items.Add(_decimalField);

				if (IsCurrencyReadOnly)
					items.Add(_currencyLabel);
				else
					items.Add(_currencySelector.Widget);

				PanelConfig config = new PanelConfig()
					.id(id)
					.renderTo(container)
					.items(items)
					.cls("money-control")
					.layout("column");

				if (IsCurrencyReadOnly)
					config.width(120);

				_panel = new Panel(config.ToDictionary());

				el = _panel.getEl();
			}

			base.onRender(container, pos);
		}

		public void Focus()
		{
			_decimalField.focus();
		}

		// ReSharper disable once CSharpWarnings::CS0108
		public void setVisible(bool visible)
		{
			_decimalField.setVisible(visible);

			if (!IsCurrencyReadOnly)
			{
				_currencySelector.Widget.setVisible(visible);

				if (_currencySelector.Widget.rendered)
					new Element(_currencySelector.Widget.getEl().findParent(".x-form-field-trigger-wrap")).setStyle("width", string.Empty);
			}

			base.setVisible(visible);
		}

		public void setValue(MoneyDto money)
		{
			if (Script.IsNullOrUndefined(money))
			{
				_decimalField.setValue(null);

				if (!IsCurrencyReadOnly)
					_currencySelector.SetValue(null);

				return;
			}

			Amount = money.Amount;
			Currency = money.Currency;
		}

		// ReSharper disable once CSharpWarnings::CS0108
		public MoneyDto getValue()
		{
			MoneyDto dto = new MoneyDto();

			if (string.IsNullOrEmpty(_decimalField.getValue().ToString()) ||
				(!IsCurrencyReadOnly && _currencySelector.GetObjectInfo() == null))
				return null;

			dto.Amount = (decimal) _decimalField.getValue();

			if (IsCurrencyReadOnly)
			{
				_currency = _currency ?? Reference.Copy(_args.DefaultCurrency);

				dto.Currency = _currency;
			}
			else
				dto.Currency = _currencySelector.GetObjectInfo();

			return dto;
		}

		private void OnAmountChange(Field objthis, object newvalue, object oldvalue)
		{
			if (_menu != null)
				_menu.show(_decimalField.getEl());

			if (_args.AmountChangeHandler != null)
				_args.AmountChangeHandler.Invoke(objthis, newvalue, oldvalue);
		}

		public bool IsDirty()
		{
			return (bool) Type.InvokeMethod(_decimalField, "isDirty") ||
				(!IsCurrencyReadOnly && (bool) Type.InvokeMethod(_currencySelector.Widget, "isDirty"));
		}

		public static bool CanCalculate(MoneyControl[] controls)
		{
			if (controls == null || controls.Length == 0)
				return false;

			Reference currency = controls[0].Currency;

			for (int i = 1; i < controls.Length; i++)
			{
				MoneyControl control = controls[i];

				if (!Reference.Equals(control.Currency, currency))
					return false;
			}

			return true;
		}

		[PreserveName]
		// ReSharper disable once UnassignedReadonlyField.Compiler
		private readonly MoneyControlArgs _args;

		private DecimalField _decimalField;
		private ObjectSelector _currencySelector;
		private Panel _panel;

		private Label _currencyLabel;

		private Menu _menu;
		private Reference _currency;
	}
}