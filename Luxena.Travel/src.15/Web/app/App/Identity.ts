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
			se.Password,
		],

		edit: [
			se.Person,
			se.Name,
			se.NewPassword,
			se.ConfirmPassword,
			se.Roles,
			se.Description,
		],
	}));

}