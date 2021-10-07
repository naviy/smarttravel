using System;
using System.Text;


namespace Luxena
{

	public class StringWrapper
	{

		public StringBuilder Builder { get; private set; }

		public StringWrapper()
		{
			Builder = new StringBuilder();
		}

		public StringWrapper(StringBuilder builder)
		{
			Builder = builder ?? new StringBuilder();
		}

		public StringWrapper(string text)
			: this()
		{
			Builder.Append(text);
		}


		public override string ToString()
		{
			return Builder.ToString();
		}


		public static implicit operator StringBuilder(StringWrapper a)
		{
			return a != null ? a.Builder : null;
		}

		public static implicit operator StringWrapper(StringBuilder a)
		{
			return a != null ? new StringWrapper(a) : null;
		}

		public static implicit operator string(StringWrapper a)
		{
			return a != null ? a.Builder.ToString() : null;
		}

		public static implicit operator StringWrapper(string a)
		{
			return a != null ? new StringWrapper(a) : null;
		}

		public static implicit operator int(StringWrapper a)
		{
			return a != null ? a.Builder.Length : 0;
		}


		#region Add

		public static StringWrapper operator +(StringWrapper a, string b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, bool b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, sbyte b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, byte b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, char b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, short b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, int b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, long b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, float b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);

		}

		public static StringWrapper operator +(StringWrapper a, double b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, decimal b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, ushort b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, uint b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, ulong b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, object b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}

		public static StringWrapper operator +(StringWrapper a, char[] b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.Append(b);
		}


		public static string operator +(string a, StringWrapper b)
		{
			if (b == null) return a;
			if (a == null) return b;

			return a + b.ToString();
		}

		#endregion


		#region Multi (AppendLine)

		public static StringWrapper operator *(StringWrapper a, string b)
		{
			if (a == null) a = new StringWrapper();
			return a.Builder.AppendLine(b);
		}

		public static StringWrapper operator *(StringWrapper a, bool b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, sbyte b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, byte b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, char b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, short b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, int b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, long b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, float b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, double b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, decimal b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, ushort b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, uint b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, ulong b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, object b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		public static StringWrapper operator *(StringWrapper a, char[] b)
		{
			if (a == null) a = new StringWrapper();
			a.Builder.Append(b);
			return a.Builder.AppendLine();
		}

		#endregion


		public StringWrapper AppendJoin(string separator, params string[] values)
		{
			var isNext = false;
			foreach (var value in values)
			{
				if (value.No()) continue;

				if (isNext)
					Builder.Append(separator);
				else
					isNext = true;

				Builder.Append(value);
			}

			return this;
		}

		public StringWrapper AppendJoin(string separator, params Func<string>[] getValues)
		{
			var isNext = false;
			foreach (var getValue in getValues)
			{
				if (getValue == null) continue;
				var value = getValue();
				if (value.No()) continue;

				if (isNext)
					Builder.Append(separator);
				else
					isNext = true;

				Builder.Append(value);
			}

			return this;
		}


		public StringWrapper AppendFormat(string fmt, params object[] args)
		{
			Builder.AppendFormat(fmt, args);
			return this;
		}

	}

}
