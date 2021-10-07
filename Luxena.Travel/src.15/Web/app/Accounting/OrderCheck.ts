module Luxena.Views
{
	registerEntityControllers(sd.OrderCheck, se => ({

		members: [
			se.Date,
			se.Order,
			se.Person,
			se.CheckType,
			se.CheckNumber,
			se.Currency,
			se.CheckAmount,
			se.PayAmount,
			se.PaymentType,
			se.Description,
			se.CreatedOn,
		],

		edit: null,

	}));

}