namespace Luxena.Travel
{

	partial class ContractEditForm
	{
		
		protected override void CreateControls()
		{
			Window.width = -1;

			Form.add(MainDataPanel(new object[]
			{
				se.Number,
				se.IssueDate,
				se.Customer,
				se.DiscountPc,
				se.Note,
			}));
		}

	}

}