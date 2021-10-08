using Ext;


namespace Luxena.Travel
{

	public partial class OrganizationEditForm
	{

		protected override void CreateControls()
		{
			Form.add(TabPanel(450, new Component[]
			{
				TabMainPane(se._title, new object[]
				{
					se.Name,
					se.LegalName,
					se.ReportsTo,

					se.Code.ToField(-2, delegate(FormMember m)
					{
						m.Required(AppManager.SystemConfiguration.IsOrganizationCodeRequired);
					}),

					se.IsCustomer,
					se.CanNotBeCustomer,
					se.IsSupplier,
					se.DefaultBankAccount,

					se.Phones,
					se.Emails,
					se.Fax,
					se.WebAddress,
				}),

				TabPane(se.Note._title, new object[]
				{
					se.Note.ToField(526, delegate(FormMember m)
					{
						m.HideLabel();
						m.Height(398);
					}),
				}),

				AddressPanel(se, 450),

				TabMainPane("Провайдер", new object[]
				{
					EmptyRow(),
					se.IsBusTicketProvider.ToField(-3),
					se.IsCarRentalProvider.ToField(-3),
					se.IsPasteboardProvider.ToField(-3),
					se.IsAccommodationProvider.ToField(-3),
					se.IsTourProvider.ToField(-3),
					se.IsTransferProvider.ToField(-3),
					se.IsGenericProductProvider.ToField(-3),
				}),

				TabMainPane(DomainRes.Airline, new object[]
				{
					EmptyRow(),
					se.IsAirline.ToField(-3),
					EmptyRow(),
					se.AirlineIataCode,
					se.AirlinePrefixCode,
					se.AirlinePassportRequirement,
				}),

				TabMainPane(DomainRes.Common_Misc, new object[]
				{
					EmptyRow(),
					se.IsInsuranceCompany.ToField(-3),
					se.IsRoamingOperator.ToField(-3),
				}),

				BonusPanel(se),

			}));
		}

	}

}
