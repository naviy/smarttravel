using System.Collections;
using System.Collections.Generic;


namespace Luxena.Base.Text
{
	public sealed class ToStringComparer : IComparer
	{
		public static readonly ToStringComparer Instance = new ToStringComparer();

		private ToStringComparer()
		{
		}

		public int Compare(object x, object y)
		{
			if (x == y)
				return 0;

			if (x == null)
				return -1;

			if (y == null)
				return 1;

			return string.Compare(x.ToString(), y.ToString());
		}
	}

	public sealed class ToStringComparer<T> : IComparer<T>
	{
		public static readonly ToStringComparer<T> Instance = new ToStringComparer<T>();

		private ToStringComparer()
		{
		}

		public int Compare(T x, T y)
		{
			if (Equals(x, y))
				return 0;

			if (Equals(x, default(T)))
				return -1;

			if (Equals(y, default(T)))
				return 1;

			return string.Compare(x.ToString(), y.ToString());
		}
	}
}