using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;


namespace Luxena.Travel.Domain.Avia
{

	public class AviaMcoMap : SubEntityMapping<AviaMco>
	{
		public AviaMcoMap()
		{
			DiscriminatorValue(ProductType.AviaMco);

			Property(x => x.Description, m => m.Type(NHibernateUtil.StringClob));
			ManyToOne(x => x.InConnectionWith);
		}
	}

}