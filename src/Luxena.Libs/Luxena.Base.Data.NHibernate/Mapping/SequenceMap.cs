//using Luxena.Base.Domain;
//
//using NHibernate.Mapping.ByCode;
//using NHibernate.Mapping.ByCode.Conformist;
//
//
//namespace Luxena.Base.Data.NHibernate.Mapping
//{
//	public class SequenceMap : ClassMapping<Sequence>
//	{
//		public SequenceMap()
//		{
//			Mutable(false);
//
//			Id(x => x.Id, Uuid.Mapping);
//
//			Version(x => x.Version, m => { });
//
//			Property(x => x.Name, m =>
//			{
//				m.Length(100);
//				m.Access(Accessor.Field);
//				m.NotNullable(true);
//			});
//
//			Property(x => x.Discriminator, m =>
//			{
//				m.Length(100);
//				m.Access(Accessor.Field);
//			});
//
//			Property(x => x.Format, m =>
//			{
//				m.Length(100);
//				m.Access(Accessor.Field);
//				m.NotNullable(true);
//			});
//
//			Property(x => x.Timestamp, m =>
//			{
//				m.Access(Accessor.Field);
//				m.NotNullable(true);
//			});
//
//			Property(x => x.Current, m =>
//			{
//				m.Access(Accessor.Field);
//				m.NotNullable(true);
//			});
//		}
//	}
//}