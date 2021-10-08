using System.Data.Entity;


namespace Luxena.Travel.Domain
{

	public partial class InternalIdentity : Identity
	{

	}


	partial class Domain
	{
		public DbSet<InternalIdentity> InternalIdentities { get; set; }
	}

}