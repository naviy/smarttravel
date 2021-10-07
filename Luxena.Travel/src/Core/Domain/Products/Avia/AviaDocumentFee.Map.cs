using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain.Avia
{
	public class AviaDocumentFeeMap : Entity2Mapping<AviaDocumentFee>
	{
		public AviaDocumentFeeMap()
		{
			ManyToOne(x => x.Document);
			Property(x => x.Code, m => { m.NotNullable(true); m.Length(3); });

			Component(x => x.Amount);
		}
	}
}