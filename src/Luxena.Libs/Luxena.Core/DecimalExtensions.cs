namespace Luxena
{

	public static class DecimalExtensions
	{
		static DecimalExtensions()
		{
			DecimalFormat = "#,#0.#####";
			MoneyFormat = "N{0}";
		}

		public static string DecimalFormat { get; set; }

		public static string MoneyFormat { get; set; }

		public static string GetMoneyFormat(int precision)
		{
			return string.Format(MoneyFormat, precision);
		}

		public static string ToDecimalString(this decimal value)
		{
			return value.ToString(DecimalFormat);
		}

		public static string ToMoneyString(this decimal value, bool allowSpaces = true)
		{
			return ToMoneyString(value, 2, allowSpaces);
		}


		public static string ToMoneyString(this decimal value, int precision, bool allowSpaces = true)
		{
			var s = value.ToString(GetMoneyFormat(precision));

			if (!allowSpaces)
				s = s?.Replace(((char)160).ToString(), "").Replace(" ", "");
			else
				s = s?.Replace((char)160, ' ');


			return s;
		}

	}

}