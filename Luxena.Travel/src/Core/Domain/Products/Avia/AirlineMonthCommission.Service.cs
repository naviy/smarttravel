using System;


namespace Luxena.Travel.Domain
{

	partial class AirlineMonthCommission
	{

		public class Service : Entity2Service<AirlineMonthCommission>
		{

			public Service()
			{
				Calculating += r =>
				{
					r.DateFrom = r.DateFrom.AddDays(1 - r.DateFrom.Day);
					r.DateTo = r.DateFrom.AddMonths(1).AddDays(-1);

					var id = r.Id;
					if (db.AirlineMonthCommission.Exists(a => a.Airline == r.Airline && a.DateFrom == r.DateFrom && (id == null || a.Id != id)))
						throw new Exception("Ранее уже были указанны суммы для данной авиакомпании за указанный период.");
				};
			}

		}

	}

}