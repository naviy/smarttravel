using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.Avia
{
	public class AviaDocumentVoidingMap : Entity2Mapping<AviaDocumentVoiding>
	{
		public AviaDocumentVoidingMap()
		{
			ManyToOne(x => x.Document, m => m.NotNullable(true));

			Property(x => x.IsVoid, x => x.NotNullable(true));
			Property(x => x.TimeStamp, x => x.NotNullable(true));
			Property(x => x.AgentOffice, x => x.Length(20));
			Property(x => x.IataOffice, x => x.Length(10));
			Property(x => x.AgentCode, x => x.Length(20));

			ManyToOne(x => x.Agent);
		}
	}
}
