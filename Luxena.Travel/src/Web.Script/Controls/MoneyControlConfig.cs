using System;
using System.Runtime.CompilerServices;

using Ext.form;
using Ext.menu;

using LxnBase;
using LxnBase.Data;


namespace Luxena.Travel.Controls
{
	public class MoneyControlConfig : FieldConfig
	{
		public MoneyControlConfig()
		{
			_args = new MoneyControlArgs();
			_args.AllowBlank = true;

			o["_args"] = _args;
		}

		[AlternateSignature]
		public static extern MoneyControlConfig DefaultConfig(string fieldLabel);

		public static MoneyControlConfig DefaultConfig(string fieldLabel, bool useDefautZero)
		{
			MoneyControlConfig config = (MoneyControlConfig) new MoneyControlConfig()
				.SetDefaultCurrency(AppManager.SystemConfiguration.DefaultCurrency)
				.fieldLabel(fieldLabel);

			if (AppManager.SystemConfiguration.UseDefaultCurrencyForInput)
				config.SetIsCurrencyReadOnly(true);

			if (!Script.IsNullOrUndefined(useDefautZero))
				config.SetInitValue(MoneyDto.GetZeroMoney());

			return config;
		}

		/*public MoneyDto InitValue
		{
			get { return _initValue; }
		}

		public ObjectInfo DefaultCurrency
		{
			get { return _defaultCurrency; }
		}

		public bool IsCurrencyReadonly
		{
			get { return _isCurrencyReadonly && _defaultCurrency != null; }
		}

		public MenuConfig MenuConfig
		{
			get { return _menuConfig; }
		}

		public FieldChangeDelegate AmountChangeHandler
		{
			get { return _amountChangeHandler; }
		}

		public bool AllowBlank
		{
			get { return _allowBlank; }
		}
*/
		public MoneyControlConfig SetInitValue(MoneyDto money)
		{
			_args.InitValue = MoneyDto.CopyMoney(money);

			return this;
		}

		public MoneyControlConfig SetDefaultCurrency(Reference currency)
		{
			_args.DefaultCurrency = Reference.Copy(currency);

			return this;
		}

		public MoneyControlConfig SetIsCurrencyReadOnly(bool readOnly)
		{
			_args.IsCurrencyReadonly = readOnly;

			return this;
		}

		public MoneyControlConfig SetOnChangeMenu(MenuConfig config)
		{
			_args.MenuConfig = config;

			return this;
		}

		public MoneyControlConfig SetAmountChangeHandler(FieldChangeDelegate onChange)
		{
			_args.AmountChangeHandler = onChange;

			return this;
		}

		public MoneyControlConfig SetAllowBlank(bool allowBlank)
		{
			_args.AllowBlank = allowBlank;

			return this;
		}

		private readonly MoneyControlArgs _args;
	}

	public sealed class MoneyControlArgs : Record
	{
		public MenuConfig MenuConfig;
		public MoneyDto InitValue;
		public Reference DefaultCurrency;
		public bool IsCurrencyReadonly;
		public FieldChangeDelegate AmountChangeHandler;
		public bool AllowBlank;
	}
}