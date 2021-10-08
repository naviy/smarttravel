using System;

using Ext.form;

using LxnBase.UI.AutoControls;

using Luxena.Travel.Controls;


namespace Luxena.Travel
{

	public class MoneySemanticType : SemanticType
	{
		public readonly bool UseDefaults;
		public readonly bool UseCurrencyReadOnly;

		public MoneySemanticType(bool useDefaults, bool useCurrencyReadOnly)
		{
			UseDefaults = useDefaults;
			UseCurrencyReadOnly = useCurrencyReadOnly;
		}

		public override string GetString(SemanticMember sm, object value)
		{
			if (!Script.IsValue(value)) return "";

			string moneyStr;

			object[] values = value as object[];

			if (values != null)
				moneyStr = string.Format("{0} {1}", ((decimal)values[0]).Format("N2"), values[1]);
			else
			{
				MoneyDto dto = (MoneyDto)value;

				moneyStr = string.Format("{0} {1}", dto.Amount.Format("N2"), dto.Currency.Name);
			}

			return moneyStr;
		}


		public override void ToColumn(SemanticMember sm, Ext.grid.ColumnConfig cfg)
		{
			cfg.width(90);

			cfg.renderer(new RenderDelegate(delegate(object value)
			{
				if (!Script.IsValue(value)) return "";

				return string.Format("<div style='text-align: right'>{0}</div>", GetString(sm, value));
			}));
		}

		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			MoneyControlConfig config = UseDefaults ? MoneyControlConfig.DefaultConfig("") : new MoneyControlConfig();
			config.SetIsCurrencyReadOnly(UseCurrencyReadOnly);

			InitFieldConfig(form, sm, width, config);

			FormMember member = form.Members.Add2(form, sm, config, initMember);

			MoneyControl field = new MoneyControl(config);

			return UseDefaults
				? member.SetField(field)
				: member.SetField(field, new Field[] { field.DecimalField, field.CurrencySelector.Widget });
		}
	}


	public static partial class ViewTypes
	{
		public static SemanticType Money = new MoneySemanticType(false, false);
		public static SemanticType DefaultMoney = new MoneySemanticType(true, false);
		public static SemanticType DefaultOnlyMoney = new MoneySemanticType(true, true);
	}

	public partial class SemanticMember
	{
		public bool IsMoney;

		public SemanticMember Money()
		{
			_type = ViewTypes.Money;
			IsMoney = true;
			return this;
		}

		public SemanticMember DefaultMoney()
		{
			_type = ViewTypes.DefaultMoney;
			IsMoney = true;
			return this;
		}

		public SemanticMember DefaultOnlyMoney()
		{
			_type = ViewTypes.DefaultOnlyMoney;
			IsMoney = true;
			return this;
		}
	}

}