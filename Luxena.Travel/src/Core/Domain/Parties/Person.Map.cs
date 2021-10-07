using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;


namespace Luxena.Travel.Domain.Mapping
{

	public class PersonMap : SubEntityMapping<Person>
	{
		public PersonMap()
		{
			DiscriminatorValue(PartyType.Person);

			Property(x => x.Birthday, m => m.Type<UtcKindDateType>());
			Property(x => x.MilesCardsString, m => m.Length(200));

			ManyToOne(x => x.Organization);

			Property(x => x.Title, m => m.Length(100));

			BagAggregate(x => x.Passports, i => i.Owner);

			BagAggregate(x => x.MilesCards, i => i.Owner);
		}
	}

}
