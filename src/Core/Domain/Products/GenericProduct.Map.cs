using Luxena.Base.Data.NHibernate.Mapping;


namespace Luxena.Travel.Domain
{

	public class GenericProductMap : SubEntityMapping<GenericProduct>
	{
		public GenericProductMap()
		{
			DiscriminatorValue(ProductType.GenericProduct);

			ManyToOne(x => x.GenericType);

			Property(x => x.Number);
			Property(x => x.StartDate);
			Property(x => x.FinishDate);
		}
	}

}