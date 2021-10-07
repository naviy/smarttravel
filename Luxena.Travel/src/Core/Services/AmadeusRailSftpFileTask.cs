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

		protected override string GetPrivateKeyFilePath()
		{
			return ConfigurationManager.AppSettings["amadeus-rail-sftp"].ResolvePath();
		}
		
		protected override SftpClient NewSftpClient()
		{
			return new SftpClient("contentinn.com", 27144, UserName, PrivateKeyFile);
		}

		protected override IEnumerable<SftpFile> LoadFiles(SftpClient sftp)
		{
			return sftp.ListDirectory("/XML");
		}

	}


}