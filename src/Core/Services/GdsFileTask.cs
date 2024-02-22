using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

using Castle.MicroKernel.SubSystems.Conversion;

using Common.Logging;

using Luxena.Travel.Domain;

using static System.Net.WebRequestMethods;




namespace Luxena.Travel.Services
{



	//===g






	public class GdsFileTask<TGdsFile> : ITask
		where TGdsFile : GdsFile, new()
	{

		//---g



		public Domain.Domain db { get; set; }


		//bool ITask.IsStarted { get; set; }

		public AccessMode AccessMode { get; set; }

		public string LocalPath { get; set; }

		public string ArchiveFolder { get; set; }

		public string Uri { get; set; }
		public bool EnableSsl { get; set; }

		public bool UseProxy { get; set; }

		public bool UseProxyDefaultCredentials { get; set; }

		public string ProxyUserName { get; set; }

		public string ProxyUserPassword { get; set; }

		public string OfficeCode { get; set; }

		public string OfficeIata { get; set; }

		public string[] ReplicationUrls { get; set; }


		public PrintImportService PrintImportService { get; set; }


		public List<GdsFileTaskReimport> Reimports { get; set; }



		//---g



		private readonly ILog _log = LogManager.GetLogger("Luxena.Travel.Services." + typeof(TGdsFile).Name + "Task");

		private static readonly CultureInfo _culture = CultureInfo.GetCultureInfo("en-US");



		//---g



		public void Execute()
		{
			CheckNewFiles();
		}



		private void CheckNewFiles()
		{
			_log.Debug("Check new files...");

			try
			{
				if (typeof(TGdsFile) != typeof(PrintFile))
					ImportFiles();
				else
					ImportPrintFiles();
			}
			catch (Exception ex)
			{
				_log.Error(ex);
			}
		}



		private void ImportFiles()
		{

			var files = GetFileList();


			foreach (var file in files)
			{

				try
				{

					file.Do((TktFile a) =>
					{
						a.OfficeCode = OfficeCode;
						a.OfficeIata = OfficeIata;
					});


					_log.Info($"Import file {file.Name}...");
					_log.Debug("\tLoad file content...");


					LoadFileContent(file);

					if (file.Content.No())
						continue;


					var reimport = Reimports.By(a => file.Content.Contains(a.OfficeCode));

					if (reimport != null)
						file.SaveToInboxFolder(reimport.InboxPath);
					else
						db.GdsFile.AddFile(file);


					if (ArchiveFolder.Yes())
					{
						using (var streamWriter = new StreamWriter(Path.Combine(ArchiveFolder.ResolvePath(), file.Name)))
							streamWriter.Write(file.Content);
					}


					DeleteFile(file.Name);

				}
				catch (Exception ex)
				{
					_log.Error(ex);
				}

			}

		}



		private void ImportPrintFiles()
		{

			if (!Directory.Exists(LocalPath))
				return;


			foreach (var filePath in Directory.GetFiles(LocalPath))
			{
				try
				{
					_log.Info($"Import file {Path.GetFileName(filePath)}...");

					db.Commit(() => PrintImportService.ImportTicketsLocally(filePath));

					System.IO.File.Delete(filePath);
				}
				catch (Exception ex)
				{
					_log.Error(ex);
				}
			}

		}



		private IEnumerable<TGdsFile> GetFileList()
		{

			_log.Debug("Get file list...");

			var files = new List<TGdsFile>();


			if (AccessMode == AccessMode.Remote)
			{
				using (var response = CreateFtpResponse(Uri, Ftp.ListDirectoryDetails))
				{
					var responseStream = response.GetResponseStream();
					if (responseStream == null) return files;

					using (var reader = new StreamReader(responseStream))
					{
						while (!reader.EndOfStream)
						{
							var file = ParseFileInfo(reader.ReadLine());

							if (file != null)
								files.Add(file);
						}
					}
				}
			}
			else
			{
				if (!Directory.Exists(LocalPath))
					return files;

				foreach (var file in Directory.GetFiles(LocalPath))
				{
					var gdsFile = (TGdsFile)Activator.CreateInstance(typeof(TGdsFile));

					gdsFile.Name = Path.GetFileName(file);

					gdsFile.TimeStamp = System.IO.File.GetCreationTime(file);

					files.Add(gdsFile);
				}
			}


			return files;

		}



		private FtpWebResponse CreateFtpResponse(string uri, string method)
		{

			var request = (FtpWebRequest)WebRequest.Create(uri);
			request.EnableSsl = EnableSsl;
			request.Method = method;


			if (UseProxy)
			{

				var proxy = new WebProxy();

				if (UseProxyDefaultCredentials)
					proxy.UseDefaultCredentials = true;
				else
					proxy.Credentials = new NetworkCredential(ProxyUserName, ProxyUserPassword);

				request.Proxy = proxy;
			}


			return (FtpWebResponse)request.GetResponse();

		}



		private TGdsFile ParseFileInfo(string ftpListLine)
		{

			var fileListStyle = GetFileListStyle(ftpListLine);


			switch (fileListStyle)
			{
				case FileListStyle.Windows:
					return ParseWindowsFileInfo(ftpListLine);

				case FileListStyle.Unix:
					return ParseUnixFileInfo(ftpListLine);
			}


			throw new FormatException("Directory list is in unknown format");

		}



		private static FileListStyle GetFileListStyle(string ftpListLine)
		{

			if (Regex.IsMatch(ftpListLine, "^[-d][-rwxs]{9}.*$"))
				return FileListStyle.Unix;


			if (Regex.IsMatch(ftpListLine, @"^\d{2}-\d{2}-\d{2}.*$"))
				return FileListStyle.Windows;


			return FileListStyle.Unknown;

		}



		private static TGdsFile ParseWindowsFileInfo(string ftpListLine)
		{

			return new TGdsFile
			{
				Name = ftpListLine.Substring(39),
				TimeStamp = DateTime.Parse(ftpListLine.Substring(0, 17), _culture)
			};

		}



		private TGdsFile ParseUnixFileInfo(string ftpListLine)
		{

			const string pattern =
				@"^(?<dir>[\-ld])(?<permission>([\-r][\-w][\-xs]){3})\s+(?<filecode>\d+)\s+(?<owner>\w+)\s+(?<group>\w+)\s+(?<size>\d+)\s+(?<timestamp>((?<month>\w{3})\s+(?<day>\d{1,2})\s+(?<hour>\d{1,2}):(?<minute>\d{2}))|((?<month>\w{3})\s+(?<day>\d{1,2})\s+(?<year>\d{4})))\s+(?<name>.+)$";

			var regex = new Regex(pattern);
			var match = regex.Match(ftpListLine);

			var dir = match.Groups["dir"].Value;

			if (dir == "d")
				return null;


			var gdsFile = (TGdsFile)Activator.CreateInstance(typeof(TGdsFile));

			gdsFile.Name = match.Groups["name"].Value;
			gdsFile.TimeStamp = GetFileTimestamp();


			return gdsFile;


			DateTime GetFileTimestamp()
			{

				try
				{

					var year = match.Groups["year"].Value;
					var month = match.Groups["month"].Value;
					var day = match.Groups["day"].Value.TrimStart('0');
					var hour = match.Groups["hour"].Value;
					var minute = match.Groups["minute"].Value;



					if (year.Yes())
					{
						return DateTime.ParseExact($"{month} {day} {year}", "MMM d yyyy", _culture);
					}


					var timestamp = DateTime.ParseExact($"{month} {day} {hour}:{minute}", "MMM d HH:mm", _culture);

					if (timestamp.Date > DateTime.Today)
						timestamp = timestamp.AddYears(-1);


					return timestamp;

				}
				catch (Exception ex)
				{
					_log.Error(ex);
					return DateTime.Now;
				}

			}

		}



		private void LoadFileContent(GdsFile file)
		{

			if (AccessMode == AccessMode.Remote)
			{
				using (var response = CreateFtpResponse($"{Uri}/{file.Name}", Ftp.DownloadFile))
				{
					var stream = response.GetResponseStream();
					if (stream == null) return;

					using (var reader = new StreamReader(stream))
						file.Content = reader.ReadToEnd();
				}
			}
			else
			{
				using (var stream = new StreamReader(Path.Combine(LocalPath, file.Name)))
					file.Content = stream.ReadToEnd();
			}

		}



		private void DeleteFile(string fileName)
		{

			_log.Debug($"\nDeleting file {fileName}...");

			if (AccessMode == AccessMode.Remote)
			{
				using (var response = CreateFtpResponse($"{Uri}/{fileName}", Ftp.DeleteFile))
				{
					_log.Debug($"\nFile {fileName} deleted (status: {response.StatusDescription.Trim()})");
				}
			}
			else
			{
				System.IO.File.Delete(Path.Combine(LocalPath, fileName));
			}

			_log.Debug($"\nFile {fileName} deleted");

		}




		private enum FileListStyle
		{
			Windows,
			Unix,
			Unknown
		}




		//---g

	}






	//===g






	public enum AccessMode
	{
		Remote,
		Local
	}






	[Convertible]
	public class GdsFileTaskReimport
	{
		public string OfficeCode { get; set; }
		public string InboxPath { get; set; }
	}






	//===g



}