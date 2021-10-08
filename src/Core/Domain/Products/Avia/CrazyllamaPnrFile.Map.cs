using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain
{

	public class CrazyllamaPnrFileMap : SubclassMapping<CrazyllamaPnrFile>
	{
		public CrazyllamaPnrFileMap()
		{
			DiscriminatorValue("CrazyllamaPnr");
		}
	}

}