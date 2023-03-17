using System.Collections.Generic;
using System.Configuration;

using Luxena.Travel.Domain;

using Renci.SshNet;
using Renci.SshNet.Sftp;




namespace Luxena.Travel.Services
{


	// Depricated
	public class AmadeusSftpFileTask : AmadeusSftpFileTaskBase<AirFile>
	{


		protected static PrivateKeyFile PrivateKeyFile;


		public string UserName { get; set; }

		public string Password { get; set; }



		private PrivateKeyFile NewPrivateKeyFile()
		{
			var path = ConfigurationManager.AppSettings["amadeus-sftp"].ResolvePath();
			_log.Info($"Private Key File Path: {path}");

			return new PrivateKeyFile(path, Password);
		}



		protected override SftpClient NewSftpClient()
		{
			PrivateKeyFile = PrivateKeyFile ?? NewPrivateKeyFile();

			return new SftpClient("ftp.bmp.viaamadeus.com", 22, UserName, PrivateKeyFile);
		}



		protected override IEnumerable<SftpFile> LoadFiles(SftpClient sftp)
		{
			return sftp.ListDirectory("/FullAccess");
		}


	}



}