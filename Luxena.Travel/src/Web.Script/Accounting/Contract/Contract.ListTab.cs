namespace Luxena.Travel
{

	partial class ContractListTab
	{


		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Number.ToColumn(false, 120),
				se.IssueDate,
				se.Customer,
				se.DiscountPc,
				se.Note.ToColumn(true),
			});
		}

	}

}