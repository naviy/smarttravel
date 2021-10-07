module Luxena.Views
{
	registerEntityControllers(sd.Agent, se => ({
		list: [
			se.Name,
		],

		view: {
			"fields1": [
				se.Name,
				se.LegalName,
				se.Title,
				se.Organization,
				se.ReportsTo,
			],

			"fields2": [
				se.Note,
			],

			"Contacts1": se.Contacts,
			"Contacts2": se.Addresses,
		},

		viewScope: ctrl => ({
			tabs: [
				se.Contacts.toTab(),
				se.GdsAgents.toTab(ctrl),

				//ctrl.toDetailListTab(
				//	sd.Product,
				//	se => se.Seller,
				//	se => [
				//		se.IssueDate,
				//		se.Type,
				//		se.Name,
				//		se.Itinerary,
				//		se.Total,
				//		se.ServiceFee,
				//		se.GrandTotal,
				//	], {
				//		title: "Проданные билеты",
				//		inline: true,
				//	}
				//	),

				//ctrl.toDetailListTab(
				//	sd.Product,
				//	se => se.Booker,
				//	se => [
				//		se.IssueDate,
				//		se.Type,
				//		se.Name,
				//		se.Itinerary,
				//		se.Total,
				//		se.ServiceFee,
				//		se.GrandTotal,
				//	], {
				//		title: "Забронированные билеты",
				//		inline: true,
				//	}
				//),
			]
		}),

		edit: sd.Person,

	}));



}