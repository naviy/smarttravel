namespace Luxena.Travel
{

	public partial class PassportListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Owner,
				se.Number,
				se.FirstName,
				se.MiddleName,
				se.LastName,
				se.Citizenship,
				se.Birthday,
				se.Gender,
				se.IssuedBy,
				se.ExpiredOn,
				se.Note,			
			});
		}

	}

}