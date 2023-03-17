using System.Collections.Generic;
using System.Configuration;
using System.IO;

using Luxena.Travel.Domain;

using Renci.SshNet;
using Renci.SshNet.Sftp;




namespace Luxena.Travel.Services
{


	public class AmadeusAviaSftpFileTask : AmadeusSftpFileTaskBase<AirFile>
	{

		public static PrivateKeyFile PrivateKeyFile;


		public string UserName { get; set; }

		public string Password { get; set; }



		private PrivateKeyFile NewPrivateKeyFile()
		{

			var oppkContent = db.AmadeusAviaSftpRsaKey.GetLastOPPK();

			var oppkStream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(oppkContent));

			return new PrivateKeyFile(oppkStream, Password);

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