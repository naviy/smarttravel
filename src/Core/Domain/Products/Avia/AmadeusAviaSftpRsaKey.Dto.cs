using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{



	public partial class AmadeusAviaSftpRsaKeyDto : EntityContract
	{

		//public string SftpUserName { get; set; }

		//public string KeyPassword { get; set; }

		//public string PPK { get; set; }

		public string OPPK { get; set; }

	}




	public partial class AmadeusAviaSftpRsaKeyContractService : EntityContractService<AmadeusAviaSftpRsaKey, AmadeusAviaSftpRsaKey.Service, AmadeusAviaSftpRsaKeyDto>
	{

		public AmadeusAviaSftpRsaKeyContractService()
		{

			ContractFromEntity += (r, c) =>
			{
				//c.SftpUserName = r.SftpUserName;
				//c.KeyPassword = r.KeyPassword;
				//c.PPK = r.PPK;
				c.OPPK = r.OPPK;
			};


			EntityFromContract += (r, c) =>
			{
				//r.SftpUserName = c.SftpUserName + db;
				//r.KeyPassword = c.KeyPassword + db;
				//r.PPK = c.PPK + db;
				r.OPPK = c.OPPK + db;
			};

		}

	}



}