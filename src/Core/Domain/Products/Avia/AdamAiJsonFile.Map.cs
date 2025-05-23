using NHibernate.Mapping.ByCode.Conformist;




namespace Luxena.Travel.Domain
{



	public class AdamAiJsonFileMap : SubclassMapping<AdamAiJsonFile>
	{
		public AdamAiJsonFileMap()
		{
			DiscriminatorValue("AdamAiJson");
		}
	}



}