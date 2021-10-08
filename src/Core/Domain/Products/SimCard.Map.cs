using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class SimCardMap : SubEntityMapping<SimCard>
	{
		public SimCardMap()
		{
			DiscriminatorValue(ProductType.SimCard);

			Property(x => x.Number, m => m.Length(16));
			Property(x => x.IsSale, m => m.NotNullable(true));
		}
	}

}