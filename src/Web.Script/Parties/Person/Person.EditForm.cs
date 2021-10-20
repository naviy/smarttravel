using Ext;


namespace Luxena.Travel
{

	public partial class PersonEditForm
	{

		protected override void CreateControls()
		{
			Panel mainPanel = TabMainPane(se._title, new object[]
			{
				se.Name,
				se.LegalName,
				se.Title.ToField(-3),
				se.Signature,

				se.IsCustomer,
				se.CanNotBeCustomer,
				se.IsSupplier,
				se.DefaultBankAccount,

				se.Phones,
				se.Emails,
				se.Fax,
				se.WebAddress,

				se.Birthday,
				se.Organization.ToField(-3),
				se.ReportsTo,
			});

			Panel notePanel = TabPane(se.Note._title, new object[]
			{
				se.Note.ToField(526, delegate(FormMember m)
				{
					m.HideLabel();
					m.Height(486);
				}),
			});

			Form.add(TabPanel(540, new Component[]
			{
				mainPanel,
				notePanel,
				AddressPanel(se, 540),
				TabFitPane(DomainRes.MilesCard_Caption_List, se.GridMember("MilesCards", new MilesCardGridControl())),
				TabFitPane(DomainRes.Passport_Caption_List, se.GridMember("Passports", new PassportGridControl())),
				BonusPanel(se),
			}));
		}

	}

}
