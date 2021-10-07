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
			sd.row(se.FromOrder, se.FromParty),
			sd.row(se.ToOrder, se.ToParty),
			se.Amount,
		],

	}));

}