using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class ExcursionMap : SubEntityMapping<Excursion>
	{
		public ExcursionMap()
		{
			DiscriminatorValue(ProductType.Excursion);

			Property(x => x.StartDate, m => m.NotNullable(true));
			Property(x => x.FinishDate);
			Property(x => x.TourName, m => m.NotNullable(true));
		}
	}

}