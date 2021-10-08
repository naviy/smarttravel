using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{
	public class AirlineMonthCommissionMap : Entity2Mapping<AirlineMonthCommission>
	{
		public AirlineMonthCommissionMap()
		{
			ManyToOne(x => x.Airline, m => m.NotNullable(true));
			Property(x => x.DateFrom, m => m.NotNullable(true));
			Property(x => x.DateTo, x => x.NotNullable(true));
			Property(x => x.CommissionPc);
		}
	}
}
