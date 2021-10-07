using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Luxena
{

	public static class StringExtentions
	{

		#region Rest

		[DebuggerStepThrough]
		public static string Clip(this string me)
		{
			if (string.IsNullOrEmpty(me))
				return null;

			me = me.Trim();

			return string.IsNullOrEmpty(me) ? null : me;
		}

		[DebuggerStepThrough]
		public static string[] Clip(this string[] me)
		{
			if (me == null) return null;

			for (var i = 0; i < me.Length; i++)
			{
				me[i] = me[i].Clip();
			}

			return me;
		}

		[DebuggerStepThrough]
		public static string Ellipsis(this string me, int maxLength)
		{
			if (string.IsNullOrEmpty(me))
				return null;

			me = me.Trim();

			if (me.Length > maxLength)
				me = me.Substring(0, maxLength - 3) + "...";

			return string.IsNullOrEmpty(me) ? null : me;
		}

		[DebuggerStepThrough]
		public static string Join(this IEnumerable<string> me, string separator)
		{
			if (me == null) return null;

			return string.Join(separator, me);
		}

		[DebuggerStepThrough]
		public static string Join(this IEnumerable<object> me, string separator)
		{
			if (me == null) return null;

			return string.Join(separator, me.Select(a => a?.ToString()));
		}

		[DebuggerStepThrough]
		public static bool StartsWith(this string me, params string[] values)
		{
			if (string.IsNullOrEmpty(me))
				return false;

			foreach (var value in values)
			{
				if (me.StartsWith(value))
					return true;
			}

			return false;
		}

		[DebuggerStepThrough]
		public static string ToLowerFirst(this string me)
		{
			if (string.IsNullOrEmpty(me) || char.IsLower(me, 0))
				return me;

			return char.ToLowerInvariant(me[0]) + me.Substring(1);
		}

		[DebuggerStepThrough]
		public static string ToUpperFirst(this string me)
		{
			if (string.IsNullOrEmpty(me) || char.IsUpper(me, 0))
				return me;

			return char.ToUpperInvariant(me[0]) + me.Substring(1);
		}

		[DebuggerStepThrough]
		public static string TrimStart(this string me, params string[] trimStrings)
		{
			if (me.No() || trimStrings.No()) return me;

			var trimed = true;
			var startPos = 0;

			while (trimed && startPos < me.Length)
			{
				trimed = false;
				foreach (var trimString in trimStrings)
				{
					if (string.IsNullOrEmpty(trimString)) continue;

					if (startPos + trimString.Length >= me.Length) continue;
					if (me.Substring(startPos, trimString.Length) != trimString) continue;

					startPos += trimString.Length;
					trimed = true;
				}
			}

			return startPos == 0 ? me : me.Substring(startPos);
		}

		/// <summary>
		/// NEED TESTS!!!
		/// </summary>
		[DebuggerStepThrough]
		public static string TrimEnd(this string me, params string[] trimStrings)
		{
			if (me.No() || trimStrings.No()) return me;

			// TODO": test it
			var trimed = true;
			var endPos = me.Length - 1;

			while (trimed && endPos >= 0)
			{
				trimed = false;
				foreach (var trimString in trimStrings)
				{
					if (string.IsNullOrEmpty(trimString)) continue;

					if (endPos - trimString.Length <= 0 || me.Substring(endPos - trimString.Length + 1, trimString.Length) != trimString)
						continue;

					endPos -= trimString.Length;
					trimed = true;
				}
			}

			return
				endPos == me.Length - 1 ? me :
				endPos < 0 ? null :
				me.Substring(0, endPos + 1);
		}

		[DebuggerStepThrough]
		public static string Trim(this string me, params string[] trimStrings)
		{
			return me.TrimStart(trimStrings).TrimEnd(trimStrings);
		}

		#endregion


		[DebuggerStepThrough]
		public static string Left(this string me, int length)
		{
			return
				string.IsNullOrEmpty(me) || length <= 0 ? null :
				length >= me.Length ? me :
				me.Substring(0, length);
		}

		[DebuggerStepThrough]
		public static string Right(this string me, int length)
		{
			return
				string.IsNullOrEmpty(me) || length <= 0 ? null :
				length >= me.Length ? me :
				me.Substring(me.Length - length, length);
		}

	}

}
