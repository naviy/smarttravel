module Luxena.Views
{

	registerEntityControllers(sd.Department, se => ({

		list: () => [
			se.PartyTags.field().compact().width(90),
			se.Name,
			se.Organization,//.groupIndex(0),
			se.Contacts,
			se.Addresses,
		],

		view: () =>
		{

			//#region mainTab

			const mainTab = sd.col().icon(se).items(
				se.NameForDocuments.header3(),
				se.Organization.header4(),
				sd.hr(),
				sd.row(
					sd.col(
						se.PartyTags.field().unlabel(),
						se.ReportsTo,
						se.DefaultBankAccount,
						se.Note.field().labelAsHeader()
					).length(7),
					sd.col(
						se.Contacts.clone().labelAsHeader().unlabelItems(),
						se.Addresses.clone().labelAsHeaderItems()
					).length(5)
				)
			);

			//#endregion


			return sd.tabPanel().card().items(

				mainTab,

				//se.StatisticsTab.clone()
				se.OrderedTab.clone(),
				se.BalanceTab.clone(),
				se.ProvidedProductTab.clone(),

				se.HistoryTab.clone()
			);
		},


		smart: () => sd.tabPanel(

			sd.col().icon(se).items(
				se.Name.header2().icon(se),
				se.LegalName.header3(),
				sd.er(),
				sd.row(
					se.PartyTags.field().unlabel().length(5),
					se.Contacts.unlabelItems().length(7)
				)
			),//.height(200),

			se.HistoryTab.clone()
		),

		smartConfig: {
			contentWidth: 600
		},


		edit: () => sd.tabPanel().card().items(

			sd.col().title("Общее").items(
				se.Name,
				se.LegalName,
				se.PartyTags,
				se.Organization,
				se.ReportsTo,
				se.DefaultBankAccount,
				se.Note.lineCount(6).field().labelAsHeader()
			),

			sd.col().title(se.Contacts).items(
				se.EditContacts.clone(),
				se.Addresses.clone().labelAsHeaderItems()
			)

		)


	}));

}