using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Accounting
{

	public class WireTransferMap : SubclassMapping<WireTransfer>
	{
		public WireTransferMap()
		{
			DiscriminatorValue("WireTransfer");
		}
	}

}