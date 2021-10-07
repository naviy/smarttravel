module Luxena.Views
{
	registerEntityControllers(sd.InternalTransfer, se => ({

		list: [
			se.Number,
			se.Date,
			se.FromOrder,
			se.FromParty,
			se.ToOrder,
			se.ToParty,
			se.Amount,
		],

		form: [
			se.Number,
			se.Date,
			Ui.fieldRow(se, "/", ()=> [ se.FromOrder, se.FromParty, ]),
			Ui.fieldRow(se, "/",() => [se.ToOrder, se.ToParty, ]),
			se.Amount,
		],

	}));

}