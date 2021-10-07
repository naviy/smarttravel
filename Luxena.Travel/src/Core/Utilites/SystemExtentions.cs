using System;
using System.IO;


namespace Luxena.Travel
{

	public static class SystemExtentions
	{

		public static string FullMessage(this Exception ex)
		{
			string s = null;

			while (ex != null)
			{
				s += (s.Yes() ? "\r\n" : null) + ex.Message;
				ex = ex.InnerException;
			}

			return s;
		}


		public static bool IsSubclassOfRawGeneric(this Type me, Type generic)
		{
			while (me != null && me != typeof(object))
			{
				var cur = me.IsGenericType ? me.GetGenericTypeDefinition() : me;
				
				if (generic == cur)
					return true;

				me = me.BaseType;
			}

			return false;
		}

		public static string ToDateString(this DateTime me)
		{
			return me.ToString("dd.MM.yyyy");
		}
		public static string AsDateString(this DateTime? me)
		{
			return me?.ToString("dd.MM.yyyy");
		}

		public static byte[] ToBytes(this Stream stream)
		{
			stream.Seek(0, SeekOrigin.Begin);
			var buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);
			stream.Close();

			return buffer;
		}
	}

}
