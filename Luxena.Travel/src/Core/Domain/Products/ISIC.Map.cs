using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class IsicMap : SubEntityMapping<Isic>
	{
		public IsicMap()
		{
			DiscriminatorValue(ProductType.Isic);

			Property(x => x.CardType, m => m.NotNullable(true));
			Property(x => x.Number1, m => { m.Length(12); m.NotNullable(true); });
			Property(x => x.Number2, m => { m.Length(1); m.NotNullable(true); });
		}
	}

}