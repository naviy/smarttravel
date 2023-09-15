using System;
using System.Collections.Generic;
using System.IO;
using Chilkat;
using Common.Logging;

using Luxena.Travel.Domain;

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



		protected abstract SFtp NewSftpClient();



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

				if (sftp == null)
					return;

				if (!sftp.LastMethodSuccess)
				{
					_log.Error(sftp.LastErrorText);
					return;
				}


				DoImportFiles(sftp);

				sftp.Disconnect();

			}

		}



		protected abstract void DoImportFiles(SFtp sftp);


		protected void ImportFilesFromDirectory(SFtp sftp, string dirName)
		{

			var dirHandle = sftp.OpenDir(dirName);

			try
			{

				if (!CheckLastMethodSuccess())
					return;


				var dirListing = sftp.ReadDir(dirHandle);

				if (!CheckLastMethodSuccess())
					return;


				var fileCount = dirListing.NumFilesAndDirs;

				for (var i = 0; i < fileCount; i++)
				{

					var sfile = dirListing.GetFileObject(i);

					if (!sfile.IsRegular)
						continue;

					try
					{


						var gdsFile = new TGdsFile
						{
							Name = sfile.Filename,
							TimeStamp = sfile.LastModifiedTime,
						};


						var fileName = dirName + "/" + sfile.Filename;

						_log.Info($"Import file {gdsFile.Name}...");


						var sfileHandle = sftp.OpenFile(fileName, "readOnly", "openExisting");

						if (!CheckLastMethodSuccess())
							continue;

						_log.Debug("\tLoad file content...");


						try
						{

							gdsFile.Content = sftp.ReadFileText(sfileHandle, sfile.Size32, "ansi");

							if (CheckLastMethodSuccess())
							{
								SaveGdsFile(gdsFile);

								sftp.RemoveFile(fileName);
								CheckLastMethodSuccess();
							}

						}
						finally
						{
							sftp.CloseHandle(sfileHandle);
							CheckLastMethodSuccess();
						}

					}
					catch (Exception ex)
					{
						_log.Error(ex);
					}


				}


			}

			finally
			{
				if (!sftp.CloseHandle(dirHandle))
				{
					_log.Error(sftp.LastErrorText);
				}
			}



			bool CheckLastMethodSuccess()
			{
				if (sftp.LastMethodSuccess)
					return true;

				_log.Error(sftp.LastErrorText);
				return false;
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