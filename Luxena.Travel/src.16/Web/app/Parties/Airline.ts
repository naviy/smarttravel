module Luxena
{
	sd.Airline._lookupItemTemplate = r =>`<b class="span-40">${r.IataCode}</b>${r.Name}`;
}


module Luxena.Views
{

	registerEntityControllers(sd.Airline, se => ({

		list: [
			se.AirlineIataCode,
			se.AirlinePrefixCode,
			se.Name,
		],

		form: () => ({

			"fields1": [
				se.Name,
				se.LegalName,
				se.Code,
			],

			"fields2": [
				se.AirlineIataCode,
				se.AirlinePrefixCode,
				se.AirlinePassportRequirement,
			],

			"fields3": [
				se.Note,
			],

			"Contacts1": se.Contacts,
			"Contacts2": se.Addresses,
		}),


		viewScope: ctrl => ({
			tabs: [
				//se.Contacts.toTab(),
				//se.AirlineServiceClasses.toTab(ctrl, a => [a.Code, a.ServiceClass, ]),
				//se.MilesCards.toTab(ctrl, a => [a.Owner, a.Number, ]),
			]
		}),

		editScope: ctrl => ({
			tabs: [
				//se.Contacts.toTab(),
			]
		}),
		
	}));

}