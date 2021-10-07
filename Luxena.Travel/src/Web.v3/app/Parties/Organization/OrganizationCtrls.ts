/// <reference path="../PartyServices.ts" />


module Domain
{

	export var OrganizationListCtrl = Controls.GridCtrl({
		service: Organization
	});

	export var OrganizationViewCtrl = Controls.ViewFormCtrl({
		service: Organization
	});

	export var OrganizationEditCtrl = Controls.EditFormCtrl({
		service: Organization,
		parts: {
			Departments: { name: 'organizationDepartmentList' },
			Employees: { name: 'organizationEmployeeList' },
		},
		dependencies: ['organizationList'],
	});

	export var OrganizationEmployeeListCtrl = Controls.GridCtrl({
		service: Person,

		masterRow: <any>true,

		listParams: mr => ({ organizationId: mr.Id }),
		listApi: 'api/organizations/personList',

		deleteApi: 'api/persons/removeFromOrganization/{id}',
	});

}