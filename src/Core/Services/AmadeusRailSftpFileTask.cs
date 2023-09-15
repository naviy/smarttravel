using System.Configuration;
using Chilkat;

using Luxena.Travel.Domain;
using File = System.IO.File;




namespace Luxena.Travel.Services
{



	public class AmadeusRailSftpFileTask : AmadeusSftpFileTaskBase<AmadeusXmlFile>
	{


		protected static SshKey PrivateKeyFile;


		public string UserName { get; set; }

		public string Password { get; set; }



		private SshKey NewPrivateKeyFile()
		{

			var path = ConfigurationManager.AppSettings["amadeus-rail-sftp"].ResolvePath();
			_log.Info($"Private Key File Path: {path}");

			var ppkContent = File.ReadAllText(path);

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

			var success =
				sftp.Connect("contentinn.com", 27144) &&
				sftp.AuthenticatePk(UserName, PrivateKeyFile) &&
				sftp.InitializeSftp()
			;


			return sftp;

		}



		protected override void DoImportFiles(SFtp sftp)
		{
			ImportFilesFromDirectory(sftp, "XML");
		}


	}



}