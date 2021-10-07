using Luxena.Base.Data.NHibernate.Mapping;



namespace Luxena.Travel.Domain.NHibernate.Mapping.Configuration
{


	public class UserVisitMap : EntityMapping<UserVisit>
	{
		public UserVisitMap()
		{
			Id(x => x.Id, Uuid.Mapping);
			Version(x => x.Version, m => { });

			ManyToOne(x => x.User, m => m.NotNullable(true));

			Property(x => x.StartDate, m => m.NotNullable(true));
			Property(x => x.IP, m => m.NotNullable(true));
			Property(x => x.SessionId, m => { m.Length(32); m.NotNullable(true); });
			Property(x => x.Request);
		}
	}



}