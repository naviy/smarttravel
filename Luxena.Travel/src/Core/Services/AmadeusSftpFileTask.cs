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


	public class AmadeusSftpFileTask : AmadeusSftpFileTaskBase<AirFile>
	{
		
		protected override string GetPrivateKeyFilePath()
		{
			return ConfigurationManager.AppSettings["amadeus-sftp"].ResolvePath();
		}

		protected override SftpClient NewSftpClient()
		{
			return new SftpClient("ftp.bmp.viaamadeus.com", 22, UserName, PrivateKeyFile);
		}

		protected override IEnumerable<SftpFile> LoadFiles(SftpClient sftp)
		{
			return sftp.ListDirectory("/FullAccess");
		}

	}


}