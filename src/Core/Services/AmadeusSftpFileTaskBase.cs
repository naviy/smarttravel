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

		//---g



		protected AmadeusSftpFileTaskBase()
		{
			_log = LogManager.GetLogger(GetType());
		}



		//---g



		// bool ITask.IsStarted { get; set; }

		public string ArchiveFolder { get; set; }

		public List<GdsFileTaskReimport> Reimports { get; set; }

		public Domain.Domain db { get; set; }


		protected readonly ILog _log;



		//---g



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



		protected abstract SftpClient NewSftpClient();

		protected abstract IEnumerable<SftpFile> LoadFiles(SftpClient sftp);



		private void ImportFiles()
		{

			//_log.Info("ImportFiles...");
			//_log.Info($"PrivateKeyFile: {PrivateKeyFile}");
			

			if (ArchiveFolder?.Contains("~") ?? false)
			{
				ArchiveFolder = ArchiveFolder.ResolvePath();
			}


			using (var sftp = NewSftpClient())
			{

				sftp.Connect();


				var sfiles = LoadFiles(sftp).ToArray();


				foreach (var sfile in sfiles.Where(a => a.IsRegularFile).OrderBy(a => a.Name))
				{

					try
					{

						_log.Info($"Import file {sfile.Name}...");
						_log.Debug("\tLoad file content...");


						var content = LoadFileContent(sftp, sfile);

						if (content.No())
							continue;


						var file = new TGdsFile
						{
							Name = sfile.Name, 
							TimeStamp = sfile.LastWriteTime,
							Content = content,
						};
						
						SaveGdsFile(file);


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



		private static string LoadFileContent(SftpClient sftp, SftpFile sfile)
		{

			using (var stream = sftp.Open(sfile.FullName, FileMode.Open))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}

		}



		private void SaveGdsFile(GdsFile file)
		{

			var reimport = Reimports.By(a => file.Content.Contains(a.OfficeCode));

			if (reimport != null)
			{
				file.SaveToInboxFolder(reimport.InboxPath);
			}
			else
			{
				db.GdsFile.AddFile(file);
			}


			if (ArchiveFolder.Yes())
			{
				using (var sw = new StreamWriter(Path.Combine(ArchiveFolder, file.Name)))
				{
					sw.Write(file.Content);
				}
			}

		}



		//---g

	}






	//===g



}