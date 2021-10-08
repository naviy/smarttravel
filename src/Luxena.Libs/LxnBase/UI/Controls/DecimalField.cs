using System;

using Ext.form;


namespace LxnBase.UI.Controls
{
	public class DecimalField : NumberField
	{
		public DecimalField(object config) :  base(config)
		{
			decimalSeparator = ",";
			baseChars = "0123456789.";
			if (!Script.IsValue(decimalPrecision) || decimalPrecision == 0)
				decimalPrecision = 2;
//			decimalPrecision = 4;
		}

#pragma warning disable 108,114
// ReSharper disable InconsistentNaming
		public void setValue(object v)
// ReSharper restore InconsistentNaming
#pragma warning restore 108,114
		{
			if (Script.IsNullOrUndefined(v))
				v = "";
			else
			{
				v = v.GetType() == typeof (Number) ? v : Number.Parse(((string) v).Replace(decimalSeparator, "."));
				int decimals = Math.Pow(10, decimalPrecision);
				v = Number.IsNaN((Number)v) ? "" : ((Number)(Math.Round((Number)v * decimals) / decimals)).ToFixed(decimalPrecision).Replace(".", decimalSeparator);
			}

			value = v;
			setRawValue(v);

			if (rendered)
				validate();
		}
	}
}