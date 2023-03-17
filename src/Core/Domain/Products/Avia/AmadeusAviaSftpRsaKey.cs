using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

using Luxena.Domain;
using Luxena.Travel.Services;




namespace Luxena.Travel.Domain
{



	//===g






	[RU("RSA-ключ для авиабилетов из Amadeus", "RSA-ключи для авиабилетов из Amadeus")]
	[SupervisorPrivileges]
	public partial class AmadeusAviaSftpRsaKey : Entity2
	{

		//---g



		//[EntityName, RU("Текст PPK"), Required, Text(20)]
		//public virtual string PPK { get; set; }

		[RU("Текст OPPK"), Required, Text(20)]
		public virtual string OPPK { get; set; }


		//[RU("Имя пользователя SFTP")]
		//public virtual string SftpUserName { get; set; }

		//[RU("Пароль RSA-ключа")]
		//public virtual string KeyPassword { get; set; }



		//---g



		//public override Entity Resolve(Domain db)
		//{

		//	if (!db.IsNew(this))
		//		return this;


		//	if (Name.No())
		//	{
		//		if (IataCode.Yes())
		//			return db.AmadeusAviaSftpRsaKey.By(a => a.IataCode == IataCode);

		//		Name = IataCode;
		//	}


		//	return db.AmadeusAviaSftpRsaKey.Save(this);

		//}



		//---g



		public class Service : Entity2Service<AmadeusAviaSftpRsaKey>
		{

			public Service()
			{
				
				//Inserting += r =>
				//{

				//	var priorKey = Query.OrderByDescending(a => a.CreatedOn).FirstOrDefault();


				//	if (priorKey != null)
				//	{
				//		r.SftpUserName = priorKey.SftpUserName;
				//		r.KeyPassword = priorKey.KeyPassword;
				//	}

				//};

				Modified += r =>
				{
					AmadeusAviaSftpFileTask.PrivateKeyFile = null;
				};

				Deleted += r =>
				{
					AmadeusAviaSftpFileTask.PrivateKeyFile = null;
				};

			}



			public string GetLastOPPK()
			{
				return Query.OrderByDescending(a => a.CreatedOn).FirstOrDefault()?.OPPK;
			}

		}



		//---g

	}






	//===g



}