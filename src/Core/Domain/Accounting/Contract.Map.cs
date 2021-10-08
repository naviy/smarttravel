using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class ContractMap : Entity2Mapping<Contract>
	{
		public ContractMap()
		{
			ManyToOne(x => x.Customer);
			Property(x => x.Number);
			Property(x => x.IssueDate);
			Property(x => x.DiscountPc);
			Property(x => x.Note);
		}
	}

}