/// <reference path="../PartyServices.ts" />
var Domain;
(function (Domain) {
    Domain.PassportEditCtrl = Controls.EditFormCtrl({
        service: Domain.Passport,
        dependencies: ['personPassportList']
    });
})(Domain || (Domain = {}));
//# sourceMappingURL=PassportCtrls.js.map
