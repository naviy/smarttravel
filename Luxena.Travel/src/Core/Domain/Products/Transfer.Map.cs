using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class TransferMap : SubEntityMapping<Transfer>
	{
		public TransferMap()
		{
			DiscriminatorValue(ProductType.Transfer);

			Property(x => x.StartDate, m => m.NotNullable(true));
		}
	}

}