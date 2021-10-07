namespace Luxena.Travel
{

	public partial class OrganizationListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Name.ToColumn(false, 150, se.GetNameRenderer()),
				se.LegalName.ToColumn(false, 150, se.GetNameRenderer()),
				se.Code.ToColumn(false, 80, se.GetNameRenderer()),

				se.ReportsTo.ToColumn(true),
				se.IsCustomer.ToColumn(true),
				se.CanNotBeCustomer.ToColumn(true),
				se.IsSupplier.ToColumn(true),

				se.BonusCardNumber.ToColumn(true),
				se.BonusAmount.ToColumn(true),

				se.IsAirline,
				se.AirlineIataCode.ToColumn(true),
				se.AirlinePrefixCode.ToColumn(true),
				se.AirlinePassportRequirement.ToColumn(true),

				se.IsProvider,
				se.IsAccommodationProvider.ToColumn(true),
				se.IsBusTicketProvider.ToColumn(true),
				se.IsCarRentalProvider.ToColumn(true),
//				se.IsInsuranceProvider.ToColumn(true),
				se.IsPasteboardProvider.ToColumn(true),
				se.IsTourProvider.ToColumn(true),
				se.IsTransferProvider.ToColumn(true),
				se.IsGenericProductProvider.ToColumn(true),

				se.IsInsuranceCompany,

				se.Phone1,
				se.Phone2,
				se.Email1,
				se.Email2,
			});
		}

	}

}