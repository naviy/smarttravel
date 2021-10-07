using System;

namespace LxnBase
{
	public class NumberUtility
	{
		public static bool IsNumber(object obj)
		{
			return Type.GetScriptType(obj) == "number" && Number.IsFinite((Number) obj);
		}
	}
}