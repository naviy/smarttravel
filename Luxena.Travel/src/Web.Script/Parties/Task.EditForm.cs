namespace Luxena.Travel
{

	partial class TaskEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Number,
				se.Subject.ToField(-3),
				se.Description.ToField(-3),
				se.RelatedTo.ToField(-3),
				se.Order,
				se.AssignedTo.ToField(-3),
				se.Status,
				se.DueDate,
			}));
		}

	}

}