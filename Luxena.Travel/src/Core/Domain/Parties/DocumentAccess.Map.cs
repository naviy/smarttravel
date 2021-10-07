using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Mapping
{
	public class DocumentAccessMap : Entity2Mapping<DocumentAccess>
	{
		public DocumentAccessMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			ManyToOne(x => x.Owner);
			ManyToOne(x => x.Person, m => m.NotNullable(true));

			Property(x => x.FullDocumentControl, m => m.NotNullable(true));
		}
	}
}