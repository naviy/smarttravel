using System;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Luxena.Travel.Parsers
{
	internal class Utility
	{
		private Utility()
		{
		}

		public static DateTime ParseExactDateTime(string s, string format)
		{
			return DateTime.ParseExact(s.Trim(), format, _culture);
		}

		public static DateTime ParseSegmentDateTime(string s, DateTime baseDate)
		{
			s = s.Trim().Replace(" ", "0");

			string pattern;
			DateTime result;


			if (s.Length == 13)
			{
				pattern = "ddMMMyyyyHHmm";
				result = DateTime.ParseExact(s, pattern, _culture);
			}
			else if (s.Length == 5)
			{
				pattern = "yyyyddMMM";

				s = !s.StartsWith("29FEB") || DateTime.IsLeapYear(baseDate.Year) ?
					baseDate.Year + s :
					(baseDate.Year + 1) + s;

				result = DateTime.ParseExact(s, pattern, _culture);
			}
			else if (s.Length >= 7)
			{
				if (s.EndsWith("A") || s.EndsWith("P"))
				{
					pattern = "yyyyddMMMhhmmt";
				}
				else
				{
					if (s.Substring(5, 2) == "24")
						s = s.Substring(0, 5) + "00" + s.Substring(7);
					pattern = "yyyyddMMMHHmm";
				}
				s = !s.StartsWith("29FEB") || DateTime.IsLeapYear(baseDate.Year) ?
					baseDate.Year + s :
					(baseDate.Year + 1) + s;

				result = DateTime.ParseExact(s, pattern, _culture);
			}
			else
			{
				throw new Exception("Invalid date in segment " + s);
			}

			
			if (result < baseDate)
				result = result.AddYears(1);

			return result;
		}

		public static decimal ParseDecimal(string str)
		{
			return decimal.Parse(str, _culture);
		}

		public static bool TryParseDecimal(string str, out decimal result)
		{
			return decimal.TryParse(str, NumberStyles.Any, _culture, out result);
		}

		public static int ParseInt(string str)
		{
			return int.Parse(str, _culture);
		}

		public static bool MatchPassengers(string passenger1, string passenger2)
		{
			const string pattern = @"(?<passenger>.*[^\s])\s?(MR|MRS|MSTR|MISS)$";

			if (passenger1 == passenger2)
				return true;

			var regex = new Regex(pattern);

			if (regex.IsMatch(passenger1))
				passenger1 = regex.Match(passenger1).Groups["passenger"].Value;

			if (regex.IsMatch(passenger2))
				passenger2 = regex.Match(passenger2).Groups["passenger"].Value;

			if (passenger1 == passenger2)
				return true;

			return false;
		}

		public static string ResolveCurrencyCode(string code)
		{
			switch (code)
			{
				case UahStringLocal:
					return UahCode;
				case UsdStringLocal:
					return UsdCode;
				case LocalString:
					return UahCode;

				case UahStringTtl1:
					return UahCode;
				case UahStringTtl2:
					return UahCode;
				case UahStringE1:
					return UahCode;
				case UahStringE2:
					return UahCode;

				case UsdStringTtl1:
					return UsdCode;
				case UsdStringTtl2:
					return UsdCode;
				case UsdStringE1:
					return UsdCode;
				case UsdStringE2:
					return UsdCode;

				default:
					return code;
			}
		}

		private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;

		private const string UahStringLocal = "ÃÐÍ";
		private const string UsdStringLocal = "ÄÎË";
		private const string LocalString = "Local";
		private const string UahStringTtl1 = "TTL UAH";
		private const string UahStringTtl2 = "TTLUAH";
		private const string UsdStringTtl1 = "TTL USD";
		private const string UsdStringTtl2 = "TTLUSD";

		private const string UahStringE1 = "EUAH";
		private const string UahStringE2 = "E UAH";
		private const string UsdStringE1 = "EUSD";
		private const string UsdStringE2 = "E USD";

		public const string UsdCode = "USD";
		public const string UahCode = "UAH";
	}
}