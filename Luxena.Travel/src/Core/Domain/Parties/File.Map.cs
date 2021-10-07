using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Mapping
{
	public class FileMap : ClassMapping<File>
	{
		public FileMap()
		{
			// !!! There was a bug in NH3.1 with Cache.ReadWrite() and LazyLoad fields

			Id(x => x.Id, Uuid.Mapping);

			Property(x => x.FileName, m => m.NotNullable(true));
			Property(x => x.TimeStamp, m => m.NotNullable(true));
			Property(x => x.Content, m =>
			{
				m.Type(NHibernateUtil.BinaryBlob);
				m.NotNullable(true);
				m.Lazy(true);
			});

			ManyToOne(x => x.UploadedBy);
			ManyToOne(x => x.Party);
		}
	}
}
