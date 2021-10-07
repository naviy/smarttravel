module Luxena.Views
{

	registerEntityControllers(sd.Person, se => ({
		list: [
			se.PartyTags.field().compact().width(90),
			se.Name,
			se.Title,
			se.Contacts,
			se.Addresses,
		],

		view: sd.tabPanel().card().items(
			sd.col().icon(se) /*otitle(r => r.Name)*/.items(
				//se.Name.hidden(),
				se.NameForDocuments.field().header3(),
				se.Title.field().unlabel(),
				sd.hr(),
				sd.row(
					sd.col(
						se.PartyTags.field().unlabel(),
						se.Organization,
						se.ReportsTo,
						se.BonusCardNumber,
						se.DefaultBankAccount,
						se.Birthday,
						se.Note.field().labelAsHeader()
					).length(7),
					sd.col(
						se.Contacts.clone().labelAsHeader().unlabelItems(),
						se.Addresses.clone().labelAsHeaderItems()
					).length(5)
				),

				sd.hr2(),
				sd.gheader(se.Passports),
				se.Passports.field().unlabel()
					.items((se: IPassportSemantic) => [se.Owner, se.Number, se.Name, se.Citizenship, se.ExpiredOn, ])
				,//.gridController({ inline: true, useExport: false }),
				sd.hr2(),
				sd.gheader(se.MilesCards),
				se.MilesCards.field().unlabel()
					.items((se: IMilesCardSemantic) => [se.Number, se.Organization])
			//.gridController({ inline: true, useExport: false })
			),

			////se.StatisticsTab.clone()

			se.OrderedTab.clone(),
			se.BalanceTab.clone(),
			se.ProvidedProductTab.clone(),
			se.HistoryTab.clone()
		),

		smart: () => sd.tabPanel(

			sd.col().icon(se).items(
				se.Name.header2().icon(se),
				se.LegalName.header3(),
				sd.er(),
				sd.row(
					se.PartyTags.field().unlabel().length(5),
					se.Contacts.unlabelItems().length(7)
				)
			),

			se.HistoryTab.clone()
		),


		edit: () => sd.tabPanel().card().items(

			sd.col().icon(se).items(
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

	
	registerEntityControllers(sd.Employee, se => ({
		list: [
			se.Organization.field().groupIndex(0),
			se.PartyTags.field().compact().width(90),
			se.Name,
			se.Title,
			se.Contacts,
			se.Addresses,
		],
		form: sd.Person,
	}));

}