using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;


namespace Luxena.Travel.Domain.Accounting
{
	public class ClosedPeriodMap : Entity2Mapping<ClosedPeriod>
	{
		public ClosedPeriodMap()
		{
			Property(x => x.DateFrom, m => { m.Type<UtcKindDateType>(); m.NotNullable(true); });
			Property(x => x.DateTo, m => { m.Type<UtcKindDateType>(); m.NotNullable(true); });
			Property(x => x.PeriodState, m => m.NotNullable(true));
		}
	}
}
