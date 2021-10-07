module Luxena.Views
{

	registerEntityControllers(sd.Customer, se => ({
		
		list: [
			se.Type,
			se.Name,
		],

	}));


}