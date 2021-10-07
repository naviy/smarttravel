module Luxena.Views
{

	registerEntityControllers(sd.MilesCard, se => ({
		list: [se.Number, se.Owner, se.Organization, ],
		form: [se.Owner, se.Number, se.Organization, ],
	}));

}