/// <reference path="../PartyServices.ts" />


module Domain
{

	export var PassportEditCtrl = Controls.EditFormCtrl({
		service: Passport,
		dependencies: ['personPassportList'],
	});

}