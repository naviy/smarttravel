using NPOI.SS.UserModel;


namespace Luxena.Travel.Reports
{
	public static class FontExtensions
	{
		public static IFont Size(this IFont font, short points)
		{
			font.FontHeightInPoints = points;

			return font;
		}

		public static IFont Bold(this IFont font, short weight = (short) 700)
		{
			font.Boldweight = weight;

			return font;
		}
	}
}