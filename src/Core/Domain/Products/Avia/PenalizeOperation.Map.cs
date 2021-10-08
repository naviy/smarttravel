using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.Avia
{
	public class PenalizeOperationMap : Entity2Mapping<PenalizeOperation>
	{
		public PenalizeOperationMap()
		{
			ManyToOne(x => x.Ticket);
			Property(x => x.Type);
			Property(x => x.Status);
			Property(x => x.Description, m => m.Length(128));
		}
	}
}