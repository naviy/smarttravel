using System;


namespace Luxena.Travel
{
	public partial class AviaDocumentDto
	{
		public static string NumberToString(long number)
		{
			return Script.IsNullOrUndefined(number) ? string.Empty : number.ToString().PadLeft(10, '0');
		}
	}
}