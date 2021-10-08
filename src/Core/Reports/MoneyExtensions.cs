using System;
using System.Collections.Generic;
using System.Text;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public static class MoneyExtensions
	{
		public static string ToWords(this Money money)
		{
			var male = !_femaleCurrencies.Exists(s => s == money.Currency.Code);

			return ToWords(money, male);
		}

		public static string ToWords(Money money, bool isCurrencyGenderMale)
		{
			var val = Math.Round(money.Amount, 2, MidpointRounding.AwayFromZero);

			var isNegative = false;

			if (val < 0)
			{
				val = -val;
				isNegative = true;
			}

			var integer = (int) val;
			var fraction = (int) ((val - integer) * 100);

			var currencyCode = money.Currency.Code;

			var currencyOne = GetMoneyAmountPart(currencyCode, "One");
			var currencyTwo = GetMoneyAmountPart(currencyCode, "Two");
			var currencyFive = GetMoneyAmountPart(currencyCode, "Five");

			var currencyCopeckOne = GetMoneyAmountPart(currencyCode, "Minor_One");
			var currencyCopeckTwo = GetMoneyAmountPart(currencyCode, "Minor_Two");
			var currencyCopeckFive = GetMoneyAmountPart(currencyCode, "Minor_Five");

			var builder = new StringBuilder();

			if (integer == 0)
				builder.Append("0");

			if (integer % 1000 != 0)
				AppendPart(builder, TriadToString(integer, isCurrencyGenderMale, currencyOne, currencyTwo, currencyFive));
			else
				AppendPart(builder, currencyFive);

			integer /= 1000;
			InsertPart(builder, TriadToString(integer, false, CommonRes.Thousand_One, CommonRes.Thousand_Two, CommonRes.Thousand_Five));

			integer /= 1000;
			InsertPart(builder, TriadToString(integer, true, CommonRes.Million_One, CommonRes.Million_Two, CommonRes.Million_Five));

			integer /= 1000;
			InsertPart(builder, TriadToString(integer, true, CommonRes.Billion_One, CommonRes.Billion_Two, CommonRes.Billion_Five));

			if (isNegative)
				InsertPart(builder, CommonRes.Minus);

			AppendPart(builder, fraction.ToString("00"));
			AppendPart(builder, GetNumberText(fraction, currencyCopeckOne, currencyCopeckTwo, currencyCopeckFive));

			builder[0] = char.ToUpper(builder[0]);

			return builder.ToString();
		}

		private static string TriadToString(int value, bool male, string one, string two, string five)
		{
			var unities = GetUnities();

			if (!male)
			{
				unities[1] = CommonRes.One1;
				unities[2] = CommonRes.Two1;
			}

			var triad = value % 1000;

			if (triad == 0)
				return string.Empty;

			var builder = new StringBuilder(GetHundreds()[triad / 100]);

			if (triad % 100 < 20)
				AppendPart(builder, unities[triad % 100]);
			else
			{
				AppendPart(builder, GetTens()[triad % 100 / 10]);
				AppendPart(builder, unities[triad % 10]);
			}

			AppendPart(builder, GetNumberText(triad, one, two, five));

			return builder.ToString();
		}

		public static string GetNumberText(int val, string one, string two, string five)
		{
			var t = (val % 100 > 20) ? val % 10 : val % 20;

			switch (t)
			{
				case 1:
					return one;
				case 2:
				case 3:
				case 4:
					return two;
				default:
					return five;
			}
		}

		private static void InsertPart(StringBuilder builder, string part)
		{
			if (part.No())
				return;

			const string separator = " ";

			if (builder.Length > 0)
				builder.Insert(0, separator);

			builder.Insert(0, part);
		}

		private static void AppendPart(StringBuilder builder, string part)
		{
			if (part.No())
				return;

			const string separator = " ";

			if (builder.Length > 0)
				builder.Append(separator);

			builder.Append(part);
		}

		public static string[] GetHundreds()
		{
			return new[]
			{
				string.Empty, CommonRes.Hundred, CommonRes.TwoHundred, CommonRes.ThreeHundred, CommonRes.FourHundred,
				CommonRes.FiveHundred, CommonRes.SixHundred, CommonRes.SevenHundred, CommonRes.EightHundred, CommonRes.NineHundred
			};
		}

		public static string[] GetTens()
		{
			return new[]
			{
				string.Empty, CommonRes.Ten, CommonRes.Twenty, CommonRes.Thirty, CommonRes.Forty, CommonRes.Fifty,
				CommonRes.Sixty, CommonRes.Seventy, CommonRes.Eighty, CommonRes.Ninety
			};
		}

		/// <summary>
		/// </summary>
		/// <param name="gender">Род: 0 - мужской, 1 - женский, 2 - средний</param>
		/// <returns></returns>
		public static string[] GetUnities(int gender = 0)
		{
			switch (gender)
			{
				case 0: return new[]
				{
					string.Empty, CommonRes.One, CommonRes.Two, CommonRes.Three, CommonRes.Four, CommonRes.Five,
					CommonRes.Six, CommonRes.Seven, CommonRes.Eight, CommonRes.Nine, CommonRes.Ten,
					CommonRes.Eleven, CommonRes.Twelve, CommonRes.Thirteen, CommonRes.Fourteen, CommonRes.Fifteen,
					CommonRes.Sixteen, CommonRes.Seventeen, CommonRes.Eighteen, CommonRes.Nineteen
				};
				case 1: return new[]
				{
					string.Empty, CommonRes.One1, CommonRes.Two1, CommonRes.Three, CommonRes.Four, CommonRes.Five,
					CommonRes.Six, CommonRes.Seven, CommonRes.Eight, CommonRes.Nine, CommonRes.Ten,
					CommonRes.Eleven, CommonRes.Twelve, CommonRes.Thirteen, CommonRes.Fourteen, CommonRes.Fifteen,
					CommonRes.Sixteen, CommonRes.Seventeen, CommonRes.Eighteen, CommonRes.Nineteen
				};
				case 2: return new[]
				{
					string.Empty, CommonRes.One2, CommonRes.Two2, CommonRes.Three, CommonRes.Four, CommonRes.Five,
					CommonRes.Six, CommonRes.Seven, CommonRes.Eight, CommonRes.Nine, CommonRes.Ten,
					CommonRes.Eleven, CommonRes.Twelve, CommonRes.Thirteen, CommonRes.Fourteen, CommonRes.Fifteen,
					CommonRes.Sixteen, CommonRes.Seventeen, CommonRes.Eighteen, CommonRes.Nineteen
				};
			}

			throw new NotImplementedException();
		}

		private static string GetMoneyAmountPart(string currencyCode, string part)
		{
			return CommonRes.ResourceManager.GetString(currencyCode + "_" + part, CommonRes.Culture);
		}

		private static readonly List<string> _femaleCurrencies = new List<string> { "UAH" };
	}
}