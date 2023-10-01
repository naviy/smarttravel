using Luxena.Base.Data.NHibernate.Mapping;




namespace Luxena.Travel.Domain
{

	
	
	public class BankAccountMap : Entity3DMapping<BankAccount>
	{
		public BankAccountMap()
		{

			Property(x => x.Name);
			Property(x => x.IsDefault);
			Property(x => x.CompanyDetails);
			Property(x => x.Note);

		}
	}



}