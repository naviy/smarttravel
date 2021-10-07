using System;

using Ext.form;
using Ext.util;

using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;


namespace Luxena.Travel
{

	public class NumericSemanticType : SemanticType
	{
		public NumericSemanticType(int decimalPrecision)
		{
			DecimalPrecision = decimalPrecision;
		}

		public int DecimalPrecision;


		public override string GetString(SemanticMember sm, object value)
		{
			if (!Script.IsValue(value)) return "";

			decimal d = (decimal)value;
			if (Script.IsValue(d))
				value = d.Format("N" + DecimalPrecision);

			return value.ToString();
		}

		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			NumberFieldConfig config = new NumberFieldConfig()
				.width(ControlFactory.DefaultNumberFieldWidth)
				.allowDecimals(DecimalPrecision > 0)
				.decimalPrecision(DecimalPrecision);

			InitTextFieldConfig(form, sm, width, config);

			FormMember member = form.Members.Add2(form, sm, config, initMember);

			return member.SetField(DecimalPrecision <= 0
				? new NumberField(config.ToDictionary())
				: new DecimalField(config.ToDictionary())
			);
		}

		public override void ToColumn(SemanticMember sm, Ext.grid.ColumnConfig cfg)
		{
			cfg.renderer(new RenderDelegate(delegate(object value)
			{
				if (!Script.IsValue(value)) return "";

				decimal d = (decimal)value;
				if (Script.IsValue(d))
					value = d.Format("N" + DecimalPrecision);

				return "<div style='text-align: right'>" + value + "</div>";
			}));
		}
		
	}


	public static partial class ViewTypes
	{
		public static SemanticType Int32 = new NumericSemanticType(0);
		public static SemanticType Float2 = new NumericSemanticType(2);
		public static SemanticType Float4 = new NumericSemanticType(4);
	}

	public partial class SemanticMember
	{
		public SemanticMember Int32()
		{
			_type = ViewTypes.Int32;
			return this;
		}

		public SemanticMember Float2()
		{
			_type = ViewTypes.Float2;
			return this;
		}

		public SemanticMember Float4()
		{
			_type = ViewTypes.Float4;
			return this;
		}
	}

}