//using Luxena.Base.Data.NHibernate.Mapping;

//using NHibernate;
//using NHibernate.Mapping.ByCode.Conformist;


//namespace Luxena.Travel.Domain.NHibernate.Mapping.Configuration
//{

//	public class SystemShutdownMap : ClassMapping<SystemShutdown>
//	{
//		public SystemShutdownMap()
//		{
//			Id(x => x.Id, Uuid.Mapping);

//			Property(x => x.CreatedBy, m =>
//			{
//				m.NotNullable(true);
//				m.Length(32);
//			});

//			Property(x => x.CreatedOn, m => m.NotNullable(true));

//			Property(x => x.Note, m =>
//			{
//				m.NotNullable(true);
//				m.Type(NHibernateUtil.StringClob);
//			});

//			Property(x => x.LaunchPlannedOn, m => m.NotNullable(true));
//		}
//	}

//}