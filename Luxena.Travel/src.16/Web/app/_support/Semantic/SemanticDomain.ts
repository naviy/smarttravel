module Luxena
{

	export class SemanticDomain
	{

		entity<TSemanticEntity extends SemanticEntity>(se: TSemanticEntity)
		{
			se.init();
			return se;
		}

		entityByOData(data, targetEntity?: SemanticEntity): SemanticEntity
		{
			if (targetEntity && !targetEntity._isAbstract)
				return targetEntity;

			let name = <string>ko.unwrap(data["_Type"] || data["@odata.type"]);
			name = name && name.replace(`#${config.serverNamespace}.`, "");

			if (name)
			{
				const entity = this[name];
				if (entity)
					return entity;
			}

			if (!targetEntity)
				return null;

			return targetEntity._getBaseEntity && targetEntity._getBaseEntity() || targetEntity;
		}


		//#region Components

		accordion(...items: SemanticMembers<any>[])
		{
			return new Components.Accordion().items(...items);
		}

		tabPanel(...items: SemanticMembers<any>[])
		{
			return new Components.TabPanel().items(...items);
		}

		tabCard(...items: SemanticMembers<any>[])
		{
			return new Components.TabPanel().items(...items).card();
		}


		card(...items: SemanticMembers<any>[])
		{
			return new Components.Card().items(items);
		}

		col(...items: SemanticMembers<any>[])
		{
			return new SemanticMember().col(items).field();
		}

		row(...items: SemanticMembers<any>[])
		{
			return new SemanticMember().row(items).field();
		}

		er = () =>
			`<div class="dx-field"><div class="dx-field-value-static">&nbsp;</div></div>`;

		header(title: SemanticTitle)
		{
			title = semanticTitleToString(title);
			return `<div class="dx-fieldset-header" style="text-align: center">${title}</div>`;
		}

		gheader(title: SemanticTitle)
		{
			title = semanticTitleToString(title);
			return `<div class="dx-fieldset-header" style="position: absolute; z-index: 999; padding-top: 12px">${title}</div>`;
		}

		hr = () => `<hr/>`;
		hr2 = () => `<div class="form-divider"><div></div><div></div></div>`;
		hr3 = () => `<hr class="gap"/>`;
		hr4 = () => `<hr class="gap4"/>`;

		button(...items: Components.Buttons)
		{
			return new Components.Button().items(...items);
		}

		toolbar(...items: Components.Buttons)
		{
			return new Components.Toolbar().items(...items);
		}

		//#endregion

	}

}