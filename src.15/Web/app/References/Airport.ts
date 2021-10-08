module Luxena.Views
{

	registerEntityControllers(sd.Airport, se => ({

		list: [
			se.Code,
			se.Name,
			se.Country,
			se.Settlement,
		],

		view: () => 
		({
			"fields1": [
				se.Code,
				se.Name,
				se.Country,
				se.Settlement,
				se.LocalizedSettlement,
			],
			"fields2": [
				se.Id,
				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			]
		}),

		edit: [
			se.Code,
			se.Name,
			se.Country,
			se.Settlement,
			se.LocalizedSettlement,
		],

	}));

}