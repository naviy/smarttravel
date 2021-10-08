using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.NHibernate.Mapping.Configuration
{

	public class InternalIdentityMap : SubEntityMapping<InternalIdentity>
	{
		public InternalIdentityMap()
		{
			DiscriminatorValue("Internal");
		}
	}

}
