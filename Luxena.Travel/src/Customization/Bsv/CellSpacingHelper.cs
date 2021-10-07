using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Luxena.Travel.Bsv
{
	public class CellSpacingHelper : IPdfPCellEvent
	{
		public CellSpacingHelper(float cellSpacing)
		{
			_cellSpacing = cellSpacing;
		}

		public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
		{
			var x1 = position.GetLeft(0) + _cellSpacing;
			var x2 = position.GetRight(0) - _cellSpacing;
			var y1 = position.GetTop(0) - _cellSpacing;
			var y2 = position.GetBottom(0) + _cellSpacing;

			var canvas = canvases[PdfPTable.LINECANVAS];

			canvas.Rectangle(x1, y1, x2 - x1, y2 - y1);
			canvas.Stroke();
			canvas.ResetRGBColorStroke();
		}

		private readonly float _cellSpacing;
	}
}