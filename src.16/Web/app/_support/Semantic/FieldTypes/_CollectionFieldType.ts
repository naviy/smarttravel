module Luxena.FieldTypes
{

	export abstract class CollectionFieldType extends SemanticFieldType
	{

		getSelectFieldNames(sf: Field) { return []; }
		
		loadFromData(sf: Field, model: any, data: any) { }

		saveToData(sf: Field, model: any, data: any) { }

		removeFromData(sf: Field | SemanticMember, data: any) { }


		addItemsToController(sf: Field, ctrl: SemanticController, action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void)
		{
			const sm = sf._member;

			if (!sm._collectionItemEntity)// || !sm._collectionItemMasterMember)
				throw Error(`Свойство ${sm._entity._name}.${sm._name} не является коллекцией`);

			const gse = sm._collectionItemEntity();

			if (!sf._title || sf._title === gse._names)
				sf._title = gse._titles || gse._title || gse._names || gse._name;

			if (!sf._icon)
				sf._icon = sm._icon || gse._icon;
		}
		

		getControllerConfig(sf: Field, controllerName: string, defaultConfig)
		{
			const sm = sf._member;

			const ctrl = <FormController<any>>sf._controller;
			const gse = sm._collectionItemEntity();

			const masterMember = sm._collectionItemMasterMember && sm._collectionItemMasterMember(gse);
			let gcfg = <CollectionControllerConfig>sf._widgetOptions[controllerName] || defaultConfig;

			gcfg = $.extend({}, gcfg, <CollectionControllerConfig>{
				entity: gse,
				master: ctrl,
				defaults: masterMember ? [[masterMember, ctrl.getId()]] : undefined,
				members: sf._members || sf._member._members,
			});

			return gcfg;
		}

	}

}