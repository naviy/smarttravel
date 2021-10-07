using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class InsuranceMap : SubEntityMapping<Insurance>
	{
		public InsuranceMap()
		{
			DiscriminatorValue(ProductType.Insurance);

			Property(x => x.Number);
			Property(x => x.StartDate);
			Property(x => x.FinishDate);
		}
	}

	public class InsuranceRefundMap : SubEntityMapping<InsuranceRefund>
	{
		public InsuranceRefundMap()
		{
			DiscriminatorValue(ProductType.InsuranceRefund);
		}
	}

}