module Luxena.Views
{

	registerEntityControllers(sd.Airport, se => ({

		list: [
			se.Code,
			se.Name,
			se.Country,
			se.Settlement,
		],

		form: [
			se.Code,
			se.Name,
			se.Country,
			se.Settlement,
			se.LocalizedSettlement,
		],

	}));

}