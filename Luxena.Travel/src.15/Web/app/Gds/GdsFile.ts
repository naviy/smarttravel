module Luxena.Views
{
	registerEntityControllers(sd.GdsFile, se => ({
		members: [
			se.TimeStamp,
			se.Name,
			se.FileType,
			se.ImportResult,
			se.ImportOutput,
			se.CreatedOn,
		],

		form: [
			se.TimeStamp,
			se.Name,
			se.FileType,
			se.ImportResult,
			se.ImportOutput,
			se.Content,
		],

		smart: [
			se.TimeStamp,
			se.Name,
			se.FileType,
			se.ImportResult,
			se.ImportOutput,
		],

		viewScope: ctrl => ({
			tabs: [
				se.Products.toTab(ctrl, a => [a.CreatedBy, a.Type, a.Name, ]),
			]
		}),

	}));

}