using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;




namespace Luxena.Travel.Domain.Avia
{



	public class AmadeusAviaSftpRsaKeyMap : Entity2Mapping<AmadeusAviaSftpRsaKey>
	{

		public AmadeusAviaSftpRsaKeyMap()
		{

			Cache(x => x.Usage(CacheUsage.ReadWrite));

			//Property(x => x.SftpUserName, m => { m.NotNullable(true); });
			//Property(x => x.KeyPassword, m => { m.NotNullable(true); });
			//Property(x => x.PPK, m => { m.NotNullable(true); });
			Property(x => x.OPPK, m => { m.NotNullable(true); });

		}

	}



}