using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Common.Logging;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using Luxena.Base;
using Luxena.Base.Services;
using Luxena.Travel.Domain;

using ImportExceptions = Luxena.Travel.Parsers.Exceptions;


namespace Luxena.Travel.Services
{
	public class PrintImportService : BaseService
	{

		public Domain.Domain db { get; set; }

		public string CommunicatorFilePath { get; set; }

		public IDictionary<string, string> UserMapping { get; set; }


		public virtual bool ImportTickets(string user, string password, string office, byte[] exportFilesZip, out string output)
		{
			output = null;

			var path = GetImportPath(user);

			try
			{
				if (exportFilesZip == null || exportFilesZip.Length == 0)
				{
					_log.Warn("Export files zip is null or his length equal 0.");

					output = ImportExceptions.PrintImport_ExportEmptyFile;

					return false;
				}

				ExtractFiles(path, exportFilesZip);

				if (db.User.By(user, password) != null)
					return Import(user, office, path, out output);

				_log.Warn(string.Format("Invalid user name or password: {0} {1}", user, password));

				output = ImportExceptions.PrintImport_InvalidUserName;

				BackupFiles(path, user, true);

				DeleteFiles(path);

				return false;
			}
			catch (Exception ex)
			{
				_log.Error(ex);

				return false;
			}
		}

		public virtual bool ImportTicketsLocally(string path)
		{
			try
			{
				var userName = ResolveUserName(path);
				var importPath = GetImportPath(userName);

				Directory.CreateDirectory(importPath);

				new ZipExtractor(path).ExtractTo(importPath);

				string output;
				return Import(userName, null, importPath, out output);
			}
			catch (Exception ex)
			{
				_log.Error(ex);

				return false;
			}
		}

		public virtual bool GetUpdate(string currentVersion, out byte[] update)
		{
			update = null;

			if (CommunicatorFilePath.No() || !System.IO.File.Exists(CommunicatorFilePath))
				return false;

			var versionInfo = FileVersionInfo.GetVersionInfo(CommunicatorFilePath);

			if (versionInfo.FileVersion == currentVersion)
				return false;

			using (var fileStream = new FileStream(CommunicatorFilePath, FileMode.Open, FileAccess.Read))
			using (var stream = new MemoryStream())
			using (var zipStream = new DeflaterOutputStream(stream))
			{
				var buffer = new byte[fileStream.Length];
				fileStream.Read(buffer, 0, buffer.Length);
				fileStream.Close();

				zipStream.Write(buffer, 0, buffer.Length);
				zipStream.Finish();
				zipStream.Flush();

				update = stream.ToArray();
			}

			return true;
		}

		private bool Import(string userName, string office, string importPath, out string output)
		{
			output = null;

			var backupPath = BackupFiles(importPath, userName, false);

			_log.Info(string.Format("Import print file {0}...", importPath));

			var file = GetPrintFile(backupPath, userName, office);

			try
			{
				db.GdsFile.AddFile(file, out output);
			}
			catch (Exception ex)
			{
				_log.Error(ex);
			}
			finally
			{
				TransactionManager.Close();

				DeleteFiles(importPath);
			}

			return true;
		}

		private static string GetImportPath(string user)
		{
			const string importDirectoryPath = "printFiles";
			var endDirectory = string.Format(@"{0:yyyy}\{0:yyyy-MM-dd}\{0:yyyy-MM-dd_HH-mm-ss}_{1}_{2}", DateTime.Now, user, Guid.NewGuid());

			return Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, importDirectoryPath), endDirectory);
		}

		private static void ExtractFiles(string path, byte[] exportFilesZip)
		{
			_log.Debug(string.Format("Extract files into {0}", path));

			Directory.CreateDirectory(path);
			new ZipExtractor(exportFilesZip).ExtractTo(path);
		}

		private static string BackupFiles(string sourcePath, string user, bool badFiles)
		{
			var backupPath = sourcePath;

			if (badFiles)
				backupPath = Path.Combine(Path.Combine(sourcePath, ".."), string.Format("{0:yyyy-MM-dd_HH-mm-ss}_BAD_{1}_{2}", DateTime.Now, user, Guid.NewGuid()));

			backupPath += ".zip";

			_log.Debug(string.Format("Backup files to {0}", backupPath));

			new ZipCompressor().Compress(sourcePath, backupPath);

			return backupPath;
		}

		private static void DeleteFiles(string path)
		{
			_log.Debug(string.Format("Deleting print files from {0}...", path));

			Directory.Delete(path, true);

			_log.Info(string.Format("Files were deleted from {0}", path));
		}

		private static PrintFile GetPrintFile(string path, string user, string office)
		{
			return new PrintFile
			{
				Name = user,
				TimeStamp = DateTime.Now.AsUtc(),
				UserName = user,
				Office = office,
				FilePath = path,
				Content = string.Format("User: {0}{1}Stored data: {2}", user, Environment.NewLine, path)
			};
		}

		private string ResolveUserName(string path)
		{
			var fileName = Path.GetFileNameWithoutExtension(path);

			if (fileName == null) return null;

			var pos = fileName.IndexOf("_", StringComparison.Ordinal);
			var userName = pos > 0 ? fileName.Substring(pos + 1) : string.Empty;

			if (UserMapping != null && UserMapping.ContainsKey(userName))
				return UserMapping[userName];

			return userName;

		}

		private static readonly ILog _log = LogManager.GetLogger(typeof(PrintImportService));
	}
}