/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />
/// <reference path="../Controls/GridCtrl.ts" />
/// <reference path="../Controls/EditFormCtrl.ts" />
/// <reference path="../Controls/FormGridCtrl.ts" />
/// <reference path="../Controls/ViewFormCtrl.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Domain;
(function (Domain) {
    var PartyContactService = (function (_super) {
        __extends(PartyContactService, _super);
        function PartyContactService() {
            _super.apply(this, arguments);
        }
        PartyContactService.prototype.extend = function (contact) {
            function set(icon, hint, prependText, appendText) {
                contact.icon = icon;
                contact.hint = hint || '';
                contact.prependText = prependText || '';
                contact.appendText = appendText || '';
            }

            var text = contact.Text;

            switch (contact.Type.toLowerCase()) {
                case 'email':
                    set('envelope', 'E-mail: ' + text, '<a href="mailto:' + text + '">', '</a>');
                    break;
                case 'fax':
                    set('print', 'Факс: ' + text);
                    break;
                case 'phone':
                    set('phone', 'Телефон: ' + text);
                    break;
                case 'site':
                    set('globe', 'Сайт: ' + text, '<a href="' + (text.indexOf('://') > 0 ? text : 'http://' + text) + '" target="_blank">', '</a>');
                    break;
            }

            return contact;
        };

        PartyContactService.prototype.toHtml = function (c, tag, attrs) {
            if (typeof tag === "undefined") { tag = 'span'; }
            if (typeof attrs === "undefined") { attrs = null; }
            this.extend(c);

            return ('<' + tag + ' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;' + (attrs && attrs.maxWidth && (' max-width: ' + attrs.maxWidth) || '') + '">' + '<span tooltip="' + c.hint + '" tooltip-popup-delay="500">' + c.prependText + '<i class="fa fa-' + c.icon + '" style="display: inline-block; width: 1.2em"></i> ' + c.Text + c.appendText + '</span>' + '</' + tag + '>');
        };

        PartyContactService.prototype.toHtmlList = function (contacts, attrs) {
            if (typeof attrs === "undefined") { attrs = null; }
            if (!contacts)
                return '';

            var html = [];
            for (var i = 0, c; c = contacts[i++];) {
                html.push(db.PartyContact.toHtml(c, 'div', attrs));
            }
            return html.join('');
        };
        return PartyContactService;
    })(Domain.EntityService);
    Domain.PartyContactService = PartyContactService;

    //#endregion
    Domain.Party = new Domain.EntityService('party', {
        names: 'parties'
    });

    Domain.PartyContact = new PartyContactService();

    Domain.Organization = new Domain.EntityService('organization', {
        title1: function (scope, r) {
            return r && (r.LegalName || r.Name);
        }
    });

    Domain.Person = new Domain.EntityService('person', {
        title1: function (scope, r) {
            return r && (r.LegalName || r.Name);
        }
    });

    Domain.Passport = new Domain.EntityService('passport', {
        title1: function (scope, r) {
            return r && (r.Number);
        }
    });
})(Domain || (Domain = {}));
//# sourceMappingURL=PartyServices.js.map
