using Chilkat;
using Luxena.Travel.Domain;




namespace Luxena.Travel.Services
{


	public class AmadeusAviaSftpFileTask : AmadeusSftpFileTaskBase<AirFile>
	{

		public static SshKey PrivateKeyFile;


		public string UserName { get; set; }

		public string Password { get; set; }



		private SshKey NewPrivateKeyFile()
		{

			var ppkContent = db.AmadeusAviaSftpRsaKey.GetLastPPK();

			var sshKey = new SshKey { Password = Password };

			sshKey.FromPuttyPrivateKey(ppkContent);


			if (sshKey.LastMethodSuccess)
				return sshKey;


			_log.Error(sshKey.LastErrorText);
			return null;

		}



		protected override SFtp NewSftpClient()
		{

			PrivateKeyFile = PrivateKeyFile ?? NewPrivateKeyFile();

			if (PrivateKeyFile == null)
				return null;


			var sftp = new SFtp();

			var success1 =
				sftp.Connect("ftp.bmp.viaamadeus.com", 22) &&
				sftp.AuthenticatePk(UserName, PrivateKeyFile) &&
				sftp.InitializeSftp()
			;


			return sftp;

		}



		protected override void DoImportFiles(SFtp sftp)
		{
			ImportFilesFromDirectory(sftp, "FullAccess");
		}


	}



}