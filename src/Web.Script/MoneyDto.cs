using System;
using System.Runtime.CompilerServices;

using LxnBase.Data;


namespace Luxena.Travel
{

	public partial class MoneyDto
	{

		public static MoneyDto Create(object money)
		{
			if (money == null) return null;

			if (!(money is Array))
				return (MoneyDto)money;

			object[] data = (object[])money;

			MoneyDto dto = new MoneyDto();

			dto.Amount = (decimal)data[0];
			dto.Currency = Reference.Create(ClassNames.Currency, (string)data[1], data[2]);

			return dto;
		}

		public static MoneyDto CopyMoney(MoneyDto source)
		{
			if (source == null || source.Currency == null)
				return null;

			MoneyDto dto = new MoneyDto();
			dto.Amount = source.Amount;

			dto.Currency = Reference.Copy(source.Currency);

			return dto;
		}

		public static string DecimalToMoneyString(decimal val)
		{
			return Script.IsNullOrUndefined(val) ? null : val.Format("N2");
		}

		public static string ToMoneyString(MoneyDto dto)
		{
			return dto == null ? null : dto.Amount.Format("N2");
		}

		public static string ToMoneyFullString(MoneyDto dto)
		{
			return dto == null ? null : string.Format("{0} {1}", ToMoneyString(dto), dto.Currency.Name);
		}

		[AlternateSignature]
		public extern static MoneyDto GetZeroMoney();

		public static MoneyDto GetZeroMoney(Reference currency)
		{
			MoneyDto dto = new MoneyDto();

			dto.Amount = 0;
			dto.Currency = Reference.Copy(Script.IsNullOrUndefined(currency) ? AppManager.SystemConfiguration.DefaultCurrency : currency);

			return dto;
		}

	}

}