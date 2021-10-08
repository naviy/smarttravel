namespace Luxena
{

	public static class HashCodeUtility
	{

		public static int GetHashCode(object obj)
		{
			return obj == null ? 0 : obj.GetHashCode();
		}

		public static int GetHashCode(object obj1, params object[] objs)
		{
			var result = GetHashCode(obj1);

			foreach (var obj in objs)
				result = unchecked(31 * result + GetHashCode(obj));

			return result;
		}

		public static int GetHashCode(int hashCode1, params int[] hashCodes)
		{
			var result = hashCode1;

			foreach (var hashCode in hashCodes)
				result = unchecked(31 * result + hashCode);

			return result;
		}

	}

}