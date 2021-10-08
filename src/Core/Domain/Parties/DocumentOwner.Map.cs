using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Mapping
{
	public class DocumentOwnerMap : ClassMapping<DocumentOwner>
	{
		public DocumentOwnerMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			Id(x => x.Id, Uuid.Mapping);

			Version(x => x.Version, m => { });

			ManyToOne(x => x.Owner, m =>
			{
				m.NotNullable(true);
				m.Unique(true);
			});
			Property(x => x.IsActive, m => m.NotNullable(true));
		}
	}
}
