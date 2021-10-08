using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.NHibernate.Mapping.Configuration
{

	public class UserMap : SubEntityMapping<User>
	{
		public UserMap()
		{
			DiscriminatorValue("User");

			ManyToOne(x => x.Person, m => m.NotNullable(true));

			Property(x => x.Password, m => m.NotNullable(true));
			Property(x => x.Active, m => m.NotNullable(true));
			Property(x => x.IsAdministrator, m => m.NotNullable(true));
			Property(x => x.IsSupervisor, m => m.NotNullable(true));
			Property(x => x.IsAgent, m => m.NotNullable(true));
			Property(x => x.IsAnalyst, m => m.NotNullable(true));
			Property(x => x.IsCashier, m => m.NotNullable(true));
			Property(x => x.IsSubAgent, m => m.NotNullable(true));

			Property(x => x.AllowCustomerReport, m => m.NotNullable(true));
			Property(x => x.AllowRegistryReport, m => m.NotNullable(true));
			Property(x => x.AllowUnbalancedReport, m => m.NotNullable(true));

			Property(x => x.SessionId, m => m.OptimisticLock(false));
		}
	}

}