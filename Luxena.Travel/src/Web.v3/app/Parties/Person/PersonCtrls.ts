/// <reference path="../PartyServices.ts" />


module Domain
{

	export var PersonListCtrl = Controls.GridCtrl({
		service: Person
	});

	export var PersonViewCtrl = Controls.ViewFormCtrl({
		service: Person
	});

	export var PersonEditCtrl = Controls.EditFormCtrl({
		service: Person,
		parts: {
			Passports: { name: 'personPassportList', },
		},
		dependencies: ['personList', 'organizationEmployeeList'],
	});

	export var PersonPassportListCtrl = Controls.GridCtrl({
		service: Passport,

		masterRow: <any>true,

		listParams: mr => ({ personId: mr.Id }),
		listApi: 'api/persons/passportList',  
	});

}