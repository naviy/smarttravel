module Luxena.Views
{

	registerEntityControllers(sd.Passport, se => ({

		list: [
			se.Number,
			se.Owner,
			se.Name,
			se.Citizenship,
			se.Birthday,
			se.Gender.field().compact().length(3),
			se.ExpiredOn,
		],

		view: sd.tabPanel(
			sd.col().icon(se).items(
				se.Number.header2(),
				se.Owner.header3(),
				sd.hr(),
				se.Name,
				se.Citizenship,
				se.Birthday,
				se.Gender,
				se.IssuedBy,
				se.ExpiredOn,
				se.Note.field()._labelAsHeader,
				sd.hr(),
				se.AmadeusString,
				se.GalileoString
			),
			se.HistoryTab
		),

		edit: [
			se.Owner,
			se.Number,
			se.LastName,
			se.FirstName,
			se.MiddleName,
			se.Citizenship,
			se.Birthday,
			se.Gender,
			se.IssuedBy,
			se.ExpiredOn,
			se.Note,
		],

	}));

}