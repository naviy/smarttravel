using NHibernate.Mapping.ByCode.Conformist;


namespace Luxena.Travel.Domain.Avia
{
	public class PrintFileMap : SubclassMapping<PrintFile>
	{
		public PrintFileMap()
		{
			DiscriminatorValue("Print");

			Property(x => x.FilePath, m => m.Length(255));

			Property(x => x.UserName, m => m.Length(32));

			Property(x => x.Office, m => m.Length(100));
		}
	}
}