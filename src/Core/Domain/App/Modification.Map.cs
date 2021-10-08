using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;


namespace Luxena.Travel.Domain
{

	public class ModificationMap : EntityMapping<Modification>
	{

		public ModificationMap()
		{
			//Mutable(false);
			//Cache(x => x.Usage(CacheUsage.ReadOnly));

			Id(x => x.Id, m =>
			{
				m.Type((IIdentifierType)NHibernateUtil.Int32);
				m.Generator(Generators.Identity);
			});

			Version(x => x.Version, m => { });

			Property(x => x.TimeStamp);
			Property(x => x.Author, m => m.Length(32));
			Property(x => x.Type);
			Property(x => x.InstanceType, m => m.Length(250));
			Property(x => x.InstanceId, m => m.Length(32));
			Property(x => x.InstanceString, m => m.Length(250));
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