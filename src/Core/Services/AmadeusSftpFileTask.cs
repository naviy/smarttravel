//using System.Collections.Generic;
//using System.Configuration;
//using Chilkat;
//using Luxena.Travel.Domain;




//namespace Luxena.Travel.Services
//{


//	// Depricated
//	public class AmadeusSftpFileTask : AmadeusSftpFileTaskBase<AirFile>
//	{


//		protected static SshKey PrivateKeyFile;


//		public string UserName { get; set; }

//		public string Password { get; set; }



//		private SshKey NewPrivateKeyFile()
//		{
//			var path = ConfigurationManager.AppSettings["amadeus-sftp"].ResolvePath();
//			_log.Info($"Private Key File Path: {path}");

//			return new SshKey(path, Password);
//		}



//		protected override SFtp NewSftpClient()
//		{
//			PrivateKeyFile = PrivateKeyFile ?? NewPrivateKeyFile();

//			return new SFtp("ftp.bmp.viaamadeus.com", 22, UserName, PrivateKeyFile);
//		}



//		protected override IEnumerable<SftpFile> LoadFiles(SFtp sftp)
//		{
//			return sftp.ListDirectory("/FullAccess");
//		}


//	}



//}