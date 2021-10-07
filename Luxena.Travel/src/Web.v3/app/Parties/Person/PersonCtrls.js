/// <reference path="../PartyServices.ts" />
var Domain;
(function (Domain) {
    Domain.PersonListCtrl = Controls.GridCtrl({
        service: Domain.Person
    });

    Domain.PersonViewCtrl = Controls.ViewFormCtrl({
        service: Domain.Person
    });

    Domain.PersonEditCtrl = Controls.EditFormCtrl({
        service: Domain.Person,
        parts: {
            Passports: { name: 'personPassportList' }
        },
        dependencies: ['personList', 'organizationEmployeeList']
    });

    Domain.PersonPassportListCtrl = Controls.GridCtrl({
        service: Domain.Passport,
        masterRow: true,
        listParams: function (mr) {
            return ({ personId: mr.Id });
        },
        listApi: 'api/persons/passportList'
    });
})(Domain || (Domain = {}));
//# sourceMappingURL=PersonCtrls.js.map
