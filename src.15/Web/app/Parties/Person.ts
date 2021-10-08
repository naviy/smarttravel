module Luxena.Views
{

	registerEntityControllers(sd.Person, se => ({
		
		list: [
			se.Name,
			se.IsCustomer,
			se.IsSupplier,
		],

		form: () => ({
			"fields1": [
				se.Name,
				se.LegalName,
				se.Title,
				se.IsCustomer,
				se.IsSupplier,
				se.Organization,
				se.ReportsTo,
			],

			"fields2": [
				se.BonusCardNumber,
				se.Note.lineCount(5),
				se.DefaultBankAccount,
			],

			"Contacts1": se.Contacts,
			"Contacts2": se.Addresses,
		}),

		viewScope: ctrl => ({
			tabs: [
				se.Contacts.toTab(),
				se.MilesCards.toTab(ctrl, a => [a.Number, a.Organization]),
				se.Passports.toTab(ctrl, a => [a.Number, a.Name, a.Citizenship]),
			]
		}),

		editScope: ctrl => ({
			tabs: [
				se.Contacts.toTab(),
			]
		}),

	}));


	registerEntityControllers(sd.MilesCard, se => [se.Owner, se.Number, se.Organization, ]);


	registerEntityControllers(sd.Passport, se => ({

		list: [
			se.Number,
			se.Owner,
			se.LastName,
			se.FirstName,
			se.MiddleName,
			se.Citizenship,
		],

		view: [
			se.Owner,
			se.Number,
			se.Name,
			se.Citizenship,
			se.Birthday,
			se.Gender,
			se.IssuedBy,
			se.ExpiredOn,
			se.Note,
			se.AmadeusString,
			se.GalileoString,
		],

		edit: [
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
		],

	}));

}