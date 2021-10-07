using System;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Курс валюты", "Курсы валют")]
	[SupervisorPrivileges]
	public partial class CurrencyDailyRate : Entity2
	{

		[EntityName, EntityDate, Patterns.Date]
		public virtual DateTime Date { get; set; }


		[RU("UAH/EUR"), Patterns.CurrencyRate]
		public virtual decimal? UAH_EUR { get; set; }

		[RU("UAH/RUB"), Patterns.CurrencyRate]
		public virtual decimal? UAH_RUB { get; set; }

		[RU("UAH/USD"), Patterns.CurrencyRate]
		public virtual decimal? UAH_USD { get; set; }

		[RU("RUB/EUR"), Patterns.CurrencyRate]
		public virtual decimal? RUB_EUR { get; set; }

		[RU("RUB/USD"), Patterns.CurrencyRate]
		public virtual decimal? RUB_USD { get; set; }

		[RU("EUR/USD"), Patterns.CurrencyRate]
		public virtual decimal? EUR_USD { get; set; }


		public class Service : Entity2Service<CurrencyDailyRate>
		{
			public Service()
			{
				Validating += r =>
				{
					if (Query.Any(a => a.Date == r.Date && a.Id != r.Id))
						throw new Exception("Уже введены курсы валют на данную дату. Измените дату, либо не сохраняйте текущие данные");
				};
			}
		}

	}


	public static class CurrencyDailyRateExtensions
	{

		static Money Convert(CurrencyDailyRate rate, Money money, string toCurrency, Func<string, decimal?> convert)
		{
			if (rate == null || money == null || money.Currency == null)
				return money.Clone();

			var k = convert((string)money.Currency.Id);

			if (k == null)
				return money.Clone();

			return new Money(toCurrency, money.Amount * (k ?? 1));
		}		

		public static Money ToEUR(this Money money, CurrencyDailyRate rate)
		{
			return Convert(rate, money, "EUR", fromCurrency => 
				fromCurrency == "RUB" ? 1 / rate.RUB_EUR :
				fromCurrency == "UAH" ? 1 / rate.UAH_EUR :
				fromCurrency == "USD" ? rate.EUR_USD :
				null
			);
		}

		public static Money ToRUB(this Money money, CurrencyDailyRate rate)
		{
			return Convert(rate, money, "RUB", fromCurrency => 
				fromCurrency == "EUR" ? rate.RUB_EUR :
				fromCurrency == "UAH" ? 1 / rate.UAH_RUB :
				fromCurrency == "USD" ? rate.RUB_USD :
				null
			);
		}

		public static Money ToUAH(this Money money, CurrencyDailyRate rate)
		{
			return Convert(rate, money, "UAH", fromCurrency => 
				fromCurrency == "EUR" ? rate.UAH_EUR :
				fromCurrency == "RUB" ? rate.UAH_EUR :
				fromCurrency == "USD" ? rate.UAH_EUR :
				null
			);
		}

		public static Money ToUSD(this Money money, CurrencyDailyRate rate)
		{
			return Convert(rate, money, "USD", fromCurrency => 
				fromCurrency == "EUR" ? 1 / rate.EUR_USD :
				fromCurrency == "RUB" ? 1 / rate.RUB_USD :
				fromCurrency == "UAH" ? 1 / rate.UAH_USD :
				null
			);
		}

		public static Money Convert(this CurrencyDailyRate rate, Money money, string toCurrency)
		{
			return Convert(rate, money, toCurrency, fromCurrency =>
				toCurrency == "EUR" ?
					fromCurrency == "RUB" ? 1 / rate.RUB_EUR :
					fromCurrency == "UAH" ? 1 / rate.UAH_EUR :
					fromCurrency == "USD" ? rate.EUR_USD :
					null
				: toCurrency == "RUB" ?
					fromCurrency == "EUR" ? rate.RUB_EUR :
					fromCurrency == "UAH" ? 1 / rate.UAH_RUB :
					fromCurrency == "USD" ? rate.RUB_USD :
					null
				: toCurrency == "UAH" ?
					fromCurrency == "EUR" ? rate.UAH_EUR :
					fromCurrency == "RUB" ? rate.UAH_EUR :
					fromCurrency == "USD" ? rate.UAH_EUR :
					null
				: toCurrency == "USD" ?
					fromCurrency == "EUR" ? 1 / rate.EUR_USD :
					fromCurrency == "RUB" ? 1 / rate.RUB_USD :
					fromCurrency == "UAH" ? 1 / rate.UAH_USD :
					null
				: null
			);
		}
	}

}
