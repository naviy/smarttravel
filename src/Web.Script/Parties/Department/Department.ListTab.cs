namespace Luxena.Travel
{

	public partial class DepartmentListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Name.ToColumn(false, 150, se.GetNameRenderer()),
				se.LegalName.ToColumn(false, 150, se.GetNameRenderer()),
				se.Code.ToColumn(false, 80, se.GetNameRenderer()),

				se.Organization,

				se.ReportsTo.ToColumn(true),
				se.IsCustomer.ToColumn(true),
				se.CanNotBeCustomer.ToColumn(true),
				se.IsSupplier.ToColumn(true),

				se.Phone1,
				se.Phone2,
				se.Email1,
				se.Email2,
			});
		}

	}

}