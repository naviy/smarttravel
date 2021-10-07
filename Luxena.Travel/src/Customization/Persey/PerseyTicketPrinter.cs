using iTextSharp.text;

using Luxena.Travel.Reports;


namespace Luxena.Travel.Persey
{
	public class PerseyTicketPrinter : DefaultTicketPrinter
	{
		protected override void InitDocument()
		{
			Document.SetBottomMargin(Document.BottomMargin + 40);
		}

		protected override void AddHeader() { }

		protected override void AddPageHeader()
		{
			var font = new Font(BaseFont, 11f);

			var table = NewRowTable(18f, 66f, 16f);

			table.AddCell(font, ReportRes.TicketPrinter_Header1, cell =>
			{
				cell.SetLeading(12, 0);
				cell.PaddingLeft = 8;
			});

			table.AddImage(ReportRes.TicketPrinter_LogoPath1, cell => cell.PaddingTop = -18);

			table.AddCell(font, ReportRes.TicketPrinter_Header2, cell => cell.SetLeading(12, 0));

			Document.Add(table);
		}

		protected override void AddFooter() { }

		protected override void AddPageFooter()
		{
			var font = new Font(BaseFont, 11f);

			var table = NewRowTable(10, 69, 21);
			table.DefaultCell.BorderTop().PaddingTop(10);

			table.AddImage(ReportRes.TicketPrinter_LogoPath2);
			table.AddCell(font, ReportRes.TicketPrinter_Footer1, cell => cell.Center().PaddingTop(20));
			table.AddImage(ReportRes.TicketPrinter_LogoPath3);

			ContentByte.AddToBottom(table);
		}


	}
}