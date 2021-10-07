module Luxena.Views
{

	registerEntityControllers(sd.Country, se => ({

		members: [
			se.TwoCharCode,
			se.ThreeCharCode,
			se.Name,
		],

		viewScope: ctrl => ({
			tabs: [
				se.Airports.toTab(ctrl, a => [a.Code, a.Name, a.Settlement, ]),
			]
		}),

	}));

}