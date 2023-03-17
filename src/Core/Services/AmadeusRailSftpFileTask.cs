using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using Common.Logging;

using Luxena.Travel.Domain;

using Renci.SshNet;
using Renci.SshNet.Sftp;




namespace Luxena.Travel.Services
{



	public class AmadeusRailSftpFileTask : AmadeusSftpFileTaskBase<AmadeusXmlFile>
	{


		protected static PrivateKeyFile PrivateKeyFile;


		public string UserName { get; set; }

		public string Password { get; set; }



		private PrivateKeyFile NewPrivateKeyFile()
		{
			var path = ConfigurationManager.AppSettings["amadeus-rail-sftp"].ResolvePath();
			_log.Info($"Private Key File Path: {path}");

			return new PrivateKeyFile(path, Password);
		}


		protected override SftpClient NewSftpClient()
		{
			PrivateKeyFile = PrivateKeyFile ?? NewPrivateKeyFile();

			return new SftpClient("contentinn.com", 27144, UserName, PrivateKeyFile);
		}

		protected override IEnumerable<SftpFile> LoadFiles(SftpClient sftp)
		{
			return sftp.ListDirectory("/XML");
		}


	}



}