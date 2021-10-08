using System.Data.Entity;


namespace Luxena.Travel.Domain
{

	public abstract partial class Identity : Entity3D
	{

	}

	partial class Domain
	{
		public DbSet<Identity> Identities { get; set; }
	}

}