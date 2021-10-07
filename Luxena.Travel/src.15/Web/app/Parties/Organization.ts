module Luxena.Views
{

	registerEntityControllers([
		sd.Organization,
		sd.InsuranceCompany,
		sd.RoamingOperator,
		sd.AccommodationProvider,
		sd.BusTicketProvider,
		sd.CarRentalProvider,
		sd.GenericProductProvider,
		sd.PasteboardProvider,
		sd.TourProvider,
		sd.TransferProvider,
	], se => ({

		list: [
			se.Name,
			se.IsCustomer,
			se.IsSupplier,
		],

		form: () => ({

			"fields1": [
				se.Name,
				se.LegalName,
				se.Code,
				se.IsCustomer,
				se.IsSupplier,
			],

			"fields2": [
				//se.ReportsTo,
				se.Note.lineCount(4),
				se.DefaultBankAccount,
			],

			"Contacts1": se.Contacts,
			"Contacts2": se.Addresses,

			"Providers1": [
				se.IsBusTicketProvider,
				se.IsCarRentalProvider,
				se.IsPasteboardProvider,
				se.IsAccommodationProvider,
				se.IsTourProvider,
				se.IsTransferProvider,
				se.IsGenericProductProvider,
			],

			"Providers2": [
				se.IsInsuranceCompany,
				se.IsRoamingOperator,
				se.IsAirline,
				se.AirlineIataCode,
				se.AirlinePrefixCode,
				se.AirlinePassportRequirement,
			],

		}),


		viewScope: ctrl => ({
			tabs: [
				se.Contacts.toTab(),

				{
					title: "Провайдер услуг",
					template: "Providers",
				},

				se.Persons.toTab(ctrl, a => [a.Name, ]),
				se.Departments.toTab(ctrl, a => [a.Name, ]),
			]
		}),

		editScope: ctrl => ({
			tabs: [
				se.Contacts.toTab(),
				{
					title: "Провайдер услуг",
					template: "Providers",
				},
			]
		}),

	}));


}