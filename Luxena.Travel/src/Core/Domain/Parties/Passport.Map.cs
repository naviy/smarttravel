using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Mapping
{
	public class PassportMap : Entity2Mapping<Passport>
	{
		public PassportMap()
		{
			Cache(x => x.Usage(CacheUsage.ReadWrite));

			ManyToOne(x => x.Owner, m => m.NotNullable(true));

			Property(x => x.Number, m => { m.Length(20); m.UniqueKey("business-key"); m.NotNullable(true); });

			Property(x => x.FirstName);

			Property(x => x.MiddleName);

			Property(x => x.LastName);

			ManyToOne(x => x.Citizenship);

			Property(x => x.Birthday, m => m.Type<UtcKindDateType>());

			Property(x => x.Gender);

			ManyToOne(x => x.IssuedBy, m => m.UniqueKey("business-key"));

			Property(x => x.ExpiredOn, m => m.Type<UtcKindDateType>());

			Property(x => x.Note);
		}
	}
}