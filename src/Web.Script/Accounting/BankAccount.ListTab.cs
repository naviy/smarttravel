namespace Luxena.Travel
{



	public partial class BankAccountListTab
	{

		protected override void CreateColumnConfigs()
		{

			AddColumns(new object[]
			{

				se.Name,
				se.CompanyDetails.ToColumn(false, 150),
				se.TotalSuffix.ToColumn(false, 100),
				se.Description,
				se.IsDefault.ToColumn(false, 90),
				se.DisallowVat.ToColumn(false, 90),
				se.Note.ToColumn(true),

				se.CreatedOn.ToColumn(true),
				se.CreatedBy.ToColumn(true),
				se.ModifiedOn.ToColumn(true),
				se.ModifiedBy.ToColumn(true),

			});

		}

	}



}