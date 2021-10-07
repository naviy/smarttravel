using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Avia
{
	public class TktFileMap : SubclassMapping<TktFile>
	{
		public TktFileMap()
		{
			DiscriminatorValue("Tkt");

			Property(x => x.OfficeCode, m => m.Length(20));
			Property(x => x.OfficeIata, m => m.Length(10));
		}
	}
}