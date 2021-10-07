namespace Luxena.Travel.Domain
{

	partial class Identity
	{

		public class Service<TIdentity> : EntityService<TIdentity>
			where TIdentity: Identity
		{
	

		}

		public partial class Service : EntityService<Identity>
		{
			
		}

	}

}