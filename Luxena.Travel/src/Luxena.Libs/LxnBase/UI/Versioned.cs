using System;
using System.Runtime.CompilerServices;


namespace LxnBase.UI
{
	public sealed class Versioned : Record
	{
		[PreserveCase]
		public int Version;
	}
}