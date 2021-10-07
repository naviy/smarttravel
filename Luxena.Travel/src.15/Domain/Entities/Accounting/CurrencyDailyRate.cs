using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Курс валюты", "Курсы валют")]
	[SupervisorPrivileges]
	public partial class CurrencyDailyRate : Entity2
	{

		[EntityName, EntityDate, Patterns.Date, Required, Unique]
		public DateTime Date { get; set; }


		[RU("UAH/EUR"), Patterns.CurrencyRate]
		public decimal? UAH_EUR { get; set; }

		[RU("UAH/RUB"), Patterns.CurrencyRate]
		public decimal? UAH_RUB { get; set; }

		[RU("UAH/USD"), Patterns.CurrencyRate]
		public decimal? UAH_USD { get; set; }

		[RU("RUB/EUR"), Patterns.CurrencyRate]
		public decimal? RUB_EUR { get; set; }

		[RU("RUB/USD"), Patterns.CurrencyRate]
		public decimal? RUB_USD { get; set; }

		[RU("EUR/USD"), Patterns.CurrencyRate]
		public decimal? EUR_USD { get; set; }

	}


	partial class Domain
	{
		public DbSet<CurrencyDailyRate> CurrencyDailyRates { get; set; }


		public decimal? GetCurrencyRate(DateTimeOffset date, string fromCurrencyId, string toCurrencyId)
		{
			return Database.SqlQuery<decimal?>(
				"select get_currency_rate({0}, {1}, {2})",
				date, fromCurrencyId, toCurrencyId
			).FirstOrDefault();
		}

	}

}
