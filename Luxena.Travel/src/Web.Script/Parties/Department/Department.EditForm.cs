using Ext;


namespace Luxena.Travel
{

	public partial class DepartmentEditForm
	{

		protected override void CreateControls()
		{
			Form.add(TabPanel(454, new Component[]
			{
				TabMainPane(se._title, new object[]
				{
					se.Name,
					se.LegalName,

					se.IsCustomer,
					se.CanNotBeCustomer,
					se.IsSupplier,
					se.DefaultBankAccount,

					se.Phones,
					se.Emails,
					se.Fax,
					se.WebAddress,

					se.Organization.ToField(-3),
					se.ReportsTo,
					se.Note,
				}),

				AddressPanel(se, 454),
			}));
		}

	}

}
