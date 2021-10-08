using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;


namespace Luxena.Travel.Domain.Avia
{
	public sealed class GdsFileMap : Entity3Mapping<GdsFile>
	{
		public GdsFileMap()
		{
			Discriminator(x =>
			{
				x.Length(10);
				x.Type(NHibernateUtil.String);
				x.Column("class");
			});

			Property(x => x.TimeStamp, m => m.NotNullable(true));
			Property(x => x.Content, m => { m.NotNullable(true); m.Type(NHibernateUtil.StringClob); });
			Property(x => x.ImportResult, m => m.NotNullable(true));
			Property(x => x.ImportOutput, m => m.Type(NHibernateUtil.StringClob));
			Property(x => x.FileType, m => m.NotNullable(true));
		}
	}
}
