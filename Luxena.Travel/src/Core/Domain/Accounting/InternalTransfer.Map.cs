using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Accounting
{
	public class InternalTransferMap : Entity2Mapping<InternalTransfer>
	{
		public InternalTransferMap()
		{
			Property(x => x.Number, m =>
			{
				m.Length(20);
				m.NotNullable(true);
				m.Unique(true);
			});

			Property(x => x.Date, m =>
			{
				m.Type<UtcKindDateType>();
				m.NotNullable(true);
			});

			ManyToOne(x => x.FromParty, m => { m.NotNullable(true); m.Access(Accessor.Field); });
			ManyToOne(x => x.FromOrder, m => m.Access(Accessor.Field));

			ManyToOne(x => x.ToParty, m => { m.NotNullable(true); m.Access(Accessor.Field); });
			ManyToOne(x => x.ToOrder, m => m.Access(Accessor.Field));

			Property(x => x.Amount, m => m.NotNullable(true));
		}
	}
}