namespace Luxena.Travel
{

	partial class InvoiceEditForm
	{
		
		protected override void CreateControls()
		{
			Window.width = -1;

			Form.add(MainDataPanel(new object[]
			{
				se.Number,
				se.IssueDate,
				se.Type,
				se.IssuedBy,
				se.Total,
				se.Vat,
			}));
		}

	}

}