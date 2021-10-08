using System.IO;

using ICSharpCode.SharpZipLib.Zip;


namespace Luxena.Base
{
	public class ZipCompressor
	{
		public int CompressionLevel
		{
			get { return _compressionLevel; }
			set { _compressionLevel = value; }
		}

		public void Compress(string sourcePath, string targetPath)
		{
			if (!(Directory.Exists(sourcePath) && NameIsValid(targetPath)))
				return;

			string[] files = Directory.GetFiles(sourcePath);

			using (var outputStream = new ZipOutputStream(File.Create(targetPath)))
			{
				outputStream.SetLevel(_compressionLevel);

				foreach (string fullPath in files)
				{
					using (FileStream fileStream = File.OpenRead(fullPath))
					{
						byte[] buffer = new byte[fileStream.Length];
						fileStream.Read(buffer, 0, buffer.Length);

						var entry = new ZipEntry(Path.GetFileName(fullPath));

						outputStream.PutNextEntry(entry);

						outputStream.Write(buffer, 0, buffer.Length);
					}
				}
			}
		}

		private static bool NameIsValid(string name)
		{
			return !string.IsNullOrEmpty(name) && name.IndexOfAny(Path.GetInvalidPathChars()) < 0;
		}

		private int _compressionLevel = 5;
	}
}