using System;
using System.Diagnostics;
using System.IO;


namespace Luxena
{

	public static class SystemExtensions
	{

		public static string FullMessage(this Exception ex, string separator = "\r\n")
		{
			if (ex == null) return null;

			var s = new StringWrapper(ex.Message);

			ex = ex.InnerException;

			while (ex != null)
			{
				s += separator;
				s += ex.Message;
				ex = ex.InnerException;
			}

			return s;
		}

		public static T OfType<T>(this Exception ex)
			where T: Exception
		{
			if (ex == null) return null;

			while (ex != null)
			{
				var ex2 = ex as T;
				if (ex2 != null)
					return ex2;
				ex = ex.InnerException;
			}

			return null;
		}

		public static string ToHexString(this byte[] bytes)
		{
			var len = bytes.Length;

			var c = new char[len * 2];

			for (var i = 0; i < len; i++)
			{
				var b = (byte)(bytes[i] >> 4);

				c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);

				b = (byte)(bytes[i] & 0xF);

				c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
			}

			return new string(c);
		}

		public static Stream SaveToFile(this Stream stream, string path)
		{
			if (stream == null) return null;

			stream.Seek(0, SeekOrigin.Begin);

			using (var fileStream = File.OpenWrite(path))
			{
				stream.CopyTo(fileStream);
			}

			return stream;
		}

	}

}
