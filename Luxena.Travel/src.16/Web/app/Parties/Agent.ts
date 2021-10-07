module Luxena.Views
{

	registerEntityControllers(sd.Agent, se => ({

		list: () => [
			se.PartyTags.field().compact().width(90),
			se.Name,
			se.Title,
			se.Contacts,
			se.Addresses,
		],

		view: () => sd.tabPanel().card().items(

			//sd.Product.totalGrid1(se => se.Seller),
			sd.col().icon(se).items(
				se.NameForDocuments.field().header3(),
				se.Title.field().unlabel(),
				sd.hr(),
				sd.row(
					sd.col(
						se.PartyTags.field().unlabel(),
						se.Organization,
						se.ReportsTo,
						se.Birthday,
						se.Note.field().labelAsHeader()
					).length(7),
					sd.col(
						se.Contacts.clone().labelAsHeader().unlabelItems(),
						se.Addresses.clone().labelAsHeaderItems()
					).length(5)
				),

				sd.hr2(),
				sd.gheader(sd.GdsAgent._titles),
				se.GdsAgents.field().unlabel()
					.items((se: IGdsAgentSemantic) => [se.Origin, se.Code, se.OfficeCode, se.Office, ])
				,//.gridController({ inline: true, useExport: false }),
				sd.hr2(),
				sd.gheader(sd.User._titles),
				$as(sd.User, se => se.grid(se.Person).unlabel()
					.items(() => [se.Name, se.Active])
				)
				//.gridController({ inline: true, useExport: false })
			),

			sd.Product.totalTab1("Продано", se => se.Seller, sd.ProductBySeller_TotalByIssueDate),

			sd.Product.totalTab1("Забронировано", se => se.Booker, sd.ProductByBooker_TotalByIssueDate),

			sd.Product.totalTab1("Тикетировано", se => se.Ticketer, sd.ProductByTicketer_TotalByIssueDate),

			////se.StatisticsTab.clone()

			se.HistoryTab.clone()
		),

		//view: {
		//	"fields1": [
		//		se.Name,
		//		se.LegalName,
		//		se.Title,
		//		se.Organization,
		//		se.ReportsTo,
		//	],

		//	"fields2": [
		//		se.Note,
		//	],

		//	"Contacts1": se.Contacts,
		//	"Contacts2": se.Addresses,
		//},

		viewScope: ctrl => ({
			tabs: [
				//se.Contacts.toTab(),
				//se.GdsAgents.toGridTab(ctrl),

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