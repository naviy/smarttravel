using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Accounting
{
	public class IssuedConsignmentMap : ClassMapping<IssuedConsignment>
	{
		public IssuedConsignmentMap()
		{
			Id(x => x.Id, Uuid.Mapping);

			Property(x => x.Number);
			Property(x => x.TimeStamp, m=>m.NotNullable(true));
			Property(x => x.Content, m => { m.Type(NHibernateUtil.BinaryBlob); m.NotNullable(true); m.Lazy(true); });

			ManyToOne(x => x.Consignment);
			ManyToOne(x => x.IssuedBy);
		}
	}
}
