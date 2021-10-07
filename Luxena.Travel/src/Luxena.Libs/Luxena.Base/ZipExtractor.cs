using System.IO;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;


namespace Luxena.Base
{
	public class ZipExtractor
	{
		public ZipExtractor(byte[] data)
		{
			_stream = new ZipInputStream(new MemoryStream(data));
		}

		public ZipExtractor(string targetPath)
		{
			if (File.Exists(targetPath))
				_stream = new ZipInputStream(File.OpenRead(targetPath));
		}

		public ZipExtractor(ZipInputStream stream)
		{
			_stream = stream;
		}

		public bool CreateEmptyDirectories
		{
			get { return _createEmptyDirectories; }
			set { _createEmptyDirectories = value; }
		}

		public int BufferSize
		{
			get { return _bufferSize; }
			set { _bufferSize = value; }
		}

		public void ExtractTo(string path)
		{
			ZipEntry entry;
			while ((entry = _stream.GetNextEntry()) != null)
				ExtractEntry(entry, path);
		}

		private static bool NameIsValid(string name)
		{
			return !string.IsNullOrEmpty(name) && name.IndexOfAny(Path.GetInvalidPathChars()) < 0;
		}

		private void ExtractEntry(ZipEntry entry, string path)
		{
			bool flag = NameIsValid(entry.Name) && entry.IsCompressionMethodSupported();

			string fullName = null;
			string filePath = null;
			if (flag)
			{
				string name;

				if (Path.IsPathRooted(entry.Name))
				{
					string pathRoot = Path.GetPathRoot(entry.Name);
					name = Path.Combine(Path.GetDirectoryName(entry.Name.Substring(pathRoot.Length)), Path.GetFileName(entry.Name));
				}
				else
				{
					name = entry.Name;
				}

				fullName = Path.Combine(path, name);
				filePath = Path.GetDirectoryName(Path.GetFullPath(fullName));

				flag = name.Length > 0;
			}

			if (flag && !Directory.Exists(filePath) && (!entry.IsDirectory || _createEmptyDirectories))
				try
				{
					Directory.CreateDirectory(filePath);
				}
				catch
				{
					flag = false;
				}

			if (flag && entry.IsFile)
				ExtractFileEntry(entry, fullName);
		}

		private void ExtractFileEntry(ZipEntry entry, string targetName)
		{
			using (FileStream destination = File.Create(targetName))
			{
				if (_buffer == null)
					_buffer = new byte[_bufferSize];

				StreamUtils.Copy(_stream, destination, _buffer);
			}

			File.SetLastWriteTime(targetName, entry.DateTime);
		}

		private readonly ZipInputStream _stream;
		private bool _createEmptyDirectories = true;
		private int _bufferSize = 0x1000;
		private byte[] _buffer;
	}
}