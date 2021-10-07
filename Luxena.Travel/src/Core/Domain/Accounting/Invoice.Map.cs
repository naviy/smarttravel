using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Accounting
{
	public class InvoiceMap : ClassMapping<Invoice>
	{
		public InvoiceMap()
		{
			Id(x => x.Id, Uuid.Mapping);

			Version(x => x.Version, m => { });

			Property(x => x.Number, m => m.NotNullable(true));
			Property(x => x.Agreement, m => m.Length(255));
			Property(x => x.Type, m => m.NotNullable(true));
			Property(x => x.IssueDate, m => { m.Type<UtcKindDateType>(); m.NotNullable(true); });
			Property(x => x.TimeStamp, m => m.NotNullable(true));
			Property(x => x.Content, m => { m.NotNullable(true); m.Lazy(true); });

			ManyToOne(x => x.Order);

			ManyToOne(x => x.IssuedBy);

			Component(x => x.Total);
			Component(x => x.Vat);
		}
	}
}