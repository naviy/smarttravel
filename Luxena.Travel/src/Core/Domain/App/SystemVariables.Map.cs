using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.NHibernate.Mapping.Configuration
{
	public class SystemVariablesMap : ClassMapping<SystemVariables>
	{
		public SystemVariablesMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			Id(x => x.Id, Uuid.Mapping);

			Version(x => x.Version, m => { });

			Property(x => x.ModifiedBy, m => m.Length(32));
			Property(x => x.ModifiedOn);

			Property(x => x.BirthdayTaskTimestamp, m => m.NotNullable(true));
		}
	}
}