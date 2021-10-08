using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class CurrencyDailyRateMap : Entity2Mapping<CurrencyDailyRate>
	{

		public CurrencyDailyRateMap()
		{
			Property(a => a.Date, m => m.NotNullable(true));
			Property(a => a.UAH_EUR);
			Property(a => a.UAH_RUB);
			Property(a => a.UAH_USD);
			Property(a => a.RUB_EUR);
			Property(a => a.RUB_USD);
			Property(a => a.EUR_USD);
		}

	}

}