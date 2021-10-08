using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

using Common.Logging;

using Luxena.Travel.Domain;

using Renci.SshNet;
using Renci.SshNet.Sftp;
// ReSharper disable UnusedMember.Global




namespace Luxena.Travel.Services
{


	public abstract class AmadeusSftpFileTaskBase<TGdsFile> : ITask
		where TGdsFile : GdsFile, new()
	{

		protected AmadeusSftpFileTaskBase()
		{
			_log = LogManager.GetLogger(GetType());
		}


		// bool ITask.IsStarted { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public string ArchiveFolder { get; set; }

		public List<GdsFileTaskReimport> Reimports { get; set; }

		public Domain.Domain db { get; set; }

		// ReSharper disable StaticMemberInGenericType
		protected static PrivateKeyFile PrivateKeyFile;
		// ReSharper restore StaticMemberInGenericType

		protected readonly ILog _log;


		public void Execute()
		{
			_log.Debug("Check new files...");

			try
			{
				ImportFiles();
			}
			catch (Exception ex)
			{
				_log.Error(ex);
			}
		}


		protected abstract string GetPrivateKeyFilePath();

		protected virtual PrivateKeyFile NewPrivateKeyFile()
		{
			var path = GetPrivateKeyFilePath();
			_log.Info($"Private Key File Path: {path}");

			var keyFile = new PrivateKeyFile(path, Password);
			//_log.Info($"Private Key File: {keyFile}");

			return keyFile;
		}

		protected abstract SftpClient NewSftpClient();

		protected abstract IEnumerable<SftpFile> LoadFiles(SftpClient sftp);



		private void ImportFiles()
		{

			//_log.Info("ImportFiles...");
			//_log.Info($"PrivateKeyFile: {PrivateKeyFile}");
			
			PrivateKeyFile = PrivateKeyFile ?? NewPrivateKeyFile();

			if (ArchiveFolder?.Contains("~") ?? false)
				ArchiveFolder = ArchiveFolder.ResolvePath();


			using (var sftp = NewSftpClient())
			{

				sftp.Connect();

				var sfiles = LoadFiles(sftp).ToList();

				foreach (var sfile in sfiles.Where(a => a.IsRegularFile).OrderBy(a => a.Name))
				{

					try
					{

						_log.Info($"Import file {sfile.Name}...");
						_log.Debug("\tLoad file content...");

						var file = new TGdsFile { Name = sfile.Name, TimeStamp = sfile.LastWriteTime };

						using (var stream = sftp.Open(sfile.FullName, FileMode.Open))
						using (var reader = new StreamReader(stream))
							file.Content = reader.ReadToEnd();

						if (file.Content.No()) 
							continue;


						var reimport = Reimports.By(a => file.Content.Contains(a.OfficeCode));

						if (reimport != null)
							file.SaveToInboxFolder(reimport.InboxPath);
						else
							db.GdsFile.AddFile(file);


						if (ArchiveFolder.Yes())
						{
							using (var sw = new StreamWriter(Path.Combine(ArchiveFolder, file.Name)))
								sw.Write(file.Content);
						}

						sfile.Delete();

					}
					catch (Exception ex)
					{
						_log.Error(ex);
					}

				}


				sftp.Disconnect();

			}
		}

	}


}