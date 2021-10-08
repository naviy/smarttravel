module Luxena.Views
{

	registerEntityControllers(sd.Identity, se => ({
		list: [
			se.Name,
			se.Description,
		],
	}));


	registerEntityControllers(sd.InternalIdentity, se => ({
		members: [
			se.Name,
			se.Description,
		],
	}));


	registerEntityControllers(sd.User, se => ({

		list: [
			se.Person,
			se.Name,
			se.Roles,
			se.Description,
		],

		view: [
			se.Person,
			se.Name,
			se.Roles,
			se.Description,
		],

		edit: [
			se.Person,
			sd.hr(),
			se.Name,
			se.NewPassword,
			se.ConfirmPassword,
			sd.er(),
			se.Roles,
			sd.er(),
			se.Description,
		],
	}));

}