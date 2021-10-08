/// <reference path="../PartyServices.ts" />
var Domain;
(function (Domain) {
    Domain.OrganizationListCtrl = Controls.GridCtrl({
        service: Domain.Organization
    });

    Domain.OrganizationViewCtrl = Controls.ViewFormCtrl({
        service: Domain.Organization
    });

    Domain.OrganizationEditCtrl = Controls.EditFormCtrl({
        service: Domain.Organization,
        parts: {
            Departments: { name: 'organizationDepartmentList' },
            Employees: { name: 'organizationEmployeeList' }
        },
        dependencies: ['organizationList']
    });

    Domain.OrganizationEmployeeListCtrl = Controls.GridCtrl({
        service: Domain.Person,
        masterRow: true,
        listParams: function (mr) {
            return ({ organizationId: mr.Id });
        },
        listApi: 'api/organizations/personList',
        deleteApi: 'api/persons/removeFromOrganization/{id}'
    });
})(Domain || (Domain = {}));
//# sourceMappingURL=OrganizationCtrls.js.map
