using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class GenericProductTypeMap : Entity2Mapping<GenericProductType>
	{

		public GenericProductTypeMap()
		{
			Property(x => x.Name, m => { m.Length(100); m.NotNullable(true); });
		}

	}

}