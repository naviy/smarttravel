namespace Luxena.Travel
{

	public partial class UserListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Person,
				se.Active,
				se.Roles,
				se.Name,
				se.Description,

				se.IsAdministrator,
				se.IsSupervisor,
				se.IsAgent,
				se.IsCashier,
				se.IsAnalyst,
				se.IsSubAgent,

				se.AllowCustomerReport.ToColumn(true),
				se.AllowRegistryReport.ToColumn(true),
				se.AllowUnbalancedReport.ToColumn(true),

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

	}


	public partial class UserEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -4;
			Form.add(
				ColumnPanel(new object[]
				{
					MainDataPanel(new object[]
					{
						se.Person,
						EmptyRow(),
						se.Name,
						se.Password,
						se.ConfirmPassword,
						se.Description,
					}),

					MainDataPanel(new object[]
					{
						se.Active,
						EmptyRow(),
						se.IsAdministrator,
						se.IsSupervisor,
						se.IsAgent,
						se.IsCashier,
						se.IsAnalyst,
						se.IsSubAgent,
						EmptyRow(),
						se.AllowCustomerReport,
						se.AllowRegistryReport,
						se.AllowUnbalancedReport,
					})
				})

			);
		}

	}

}