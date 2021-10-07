/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />
/// <reference path="../Controls/GridCtrl.ts" />
/// <reference path="../Controls/EditFormCtrl.ts" />
/// <reference path="../Controls/FormGridCtrl.ts" />
/// <reference path="../Controls/ViewFormCtrl.ts" />


module Domain
{

	//#region PartyContact

	export interface IPartyContact
	{
		Type: string;
		Text: string;

		icon?: string;
		hint?: string;
		prependText?: string;
		appendText?: string;
	}

	export class PartyContactService extends EntityService
	{
		extend(contact: IPartyContact): IPartyContact
		{
			function set(icon, hint, prependText?, appendText?)
			{
				contact.icon = icon;
				contact.hint = hint || '';
				contact.prependText = prependText || '';
				contact.appendText = appendText || '';
			}

			var text = contact.Text;

			switch (contact.Type.toLowerCase())
			{
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
					set('globe', 'Сайт: ' + text,
						'<a href="' + (text.indexOf('://') > 0 ? text : 'http://' + text) + '" target="_blank">',
						'</a>');
					break;
			}

			return contact;
		}


		toHtml(c: IPartyContact, tag: string = 'span', attrs = null): string
		{
			this.extend(c);

			return (
				'<' + tag + ' style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis;' +
				(attrs && attrs.maxWidth && (' max-width: ' + attrs.maxWidth) || '')
				+ '">' +
				'<span tooltip="' +
				c.hint + '" tooltip-popup-delay="500">' +
				c.prependText + '<i class="fa fa-' + c.icon + '" style="display: inline-block; width: 1.2em"></i> ' + c.Text + c.appendText +
				'</span>' +
				'</' + tag + '>'
				);
		}

		toHtmlList(contacts: IPartyContact[], attrs = null): string
		{
			if (!contacts) return '';

			var html = [];
			for (var i = 0, c; c = contacts[i++];)
			{
				html.push(db.PartyContact.toHtml(c, 'div', attrs));
			}
			return html.join('');
		}
	}

	//#endregion
	


	export var Party = new EntityService('party', {
		names: 'parties'
	});

	export var PartyContact = new PartyContactService();

	export var Organization = new EntityService('organization', {
		title1: (scope, r) => r && (r.LegalName || r.Name)
	});

	export var Person = new EntityService('person', {
		title1: (scope, r) => r && (r.LegalName || r.Name)
	});

	export var Passport = new EntityService('passport', {
		title1: (scope, r) => r && (r.Number)
	});

}