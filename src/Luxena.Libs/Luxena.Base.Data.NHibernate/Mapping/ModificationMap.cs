using Luxena.Base.Domain;

using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Base.Data.NHibernate.Mapping
{
	public class ModificationMap : ClassMapping<Modification>
	{
		public ModificationMap()
		{
			Mutable(false);

			Cache(x => x.Usage(CacheUsage.ReadOnly));

			Id(x => x.Id, m => m.Generator(Generators.Identity));

			Property(x => x.TimeStamp, m => m.Index("modification_timestamp_idx")); // todo convention for indicies??

			Property(x => x.Author, m =>
			{
				m.Length(32);
				m.Index("modification_author_idx");
			});

			Property(x => x.Type);

			Property(x => x.InstanceType, m =>
			{
				m.Length(250);
				m.Index("modification_instancetype_idx");
			});

			Property(x => x.InstanceId, m =>
			{
				m.Type(NHibernateUtil.AnsiString);
				m.Length(32);
				m.Index("modification_instanceid_idx");
			});

			Property(x => x.InstanceString, m =>
			{
				m.Length(250);
				m.Index("modification_instancestring_idx");
			});

			Property(x => x.Comment, m => m.Type(NHibernateUtil.StringClob));

			Map(x => x.Items,
				m =>
				{
					m.Access(Accessor.Field);
					m.Key(k => k.ForeignKey("none"));
				},
				k => k.Element(e => e.Column("Property")),
				r => r.Element(e => e.Column("OldValue"))
			);
		}
	}
}