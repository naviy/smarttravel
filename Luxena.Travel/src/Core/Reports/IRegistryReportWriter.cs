namespace Luxena.Travel.Reports
{
	public interface IRegistryReportWriter
	{
		void Close();

		void WriteReportTitle(string title);
		void WriteFilterString(string filter);

		void SetTextAlign(TextAlign align);
		void SetTextFont(int size, bool bold, bool italic);
		void SetCellBorder(BorderStyle border);
		void SetCellColor(int red, int green, int blue);

		void BeginDocumentTable();
		void EndDocumentTableHeaders();
		void EndDocumentTableContent();
		void BeginDocumentTableRow();
		void WriteDocumentTableCell(string text);
		void WriteDocumentTableCell(string text, bool isValid);
		void EndDocumentTable();

		void WriteFooterText(string text);
		void WriteOperationsTable(string[,] data);
		void BeginFooter();
		void EndFooter();
	}
}