using System;


namespace Luxena
{

	public static class SystemExtensions
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

	}

}
