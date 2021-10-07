namespace Luxena.Travel
{

	public partial class BankAccountEditForm
	{

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Name.ToField(-3),
				se.Description.ToField(-3),
				se.IsDefault.ToField(-3),
				se.Note.ToField(-3),
			}));
		}

	}

}