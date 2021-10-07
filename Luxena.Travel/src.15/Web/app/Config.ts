module Luxena
{

	export var config: Config = {
		serverNamespace: "Luxena.Travel.Domain",

		layoutSet: "agent",

		endpoints:
		{
			db:
			{
				local: "odata/",
				production: "odata/"
			}
		},

		services:
		{
			db: {}
		},

	};


	export interface Config
	{
		serverNamespace?: string;

		startupView?: string;

		layoutSet?: string;

		menu?: Array<MenuItemOptions>;

		commandMapping?: {
			[containderId: string]: {
				defaults?: DevExpress.framework.dxCommandOptions;
				commands?: DevExpress.framework.dxCommandOptions;
			}
		};

		endpoints: {
			[key: string]: {
				local?: string;
				production?: string;
			}
		};

		services: {
			db: {
				entities?: {
					[entityName: string]: {
						key?: any;
						keyType?: any;
					}
				}
			}
		};

	}



	export interface MenuItemOptions
	{
		icon?: string;
		title?: string;
		description?: string;
		items?: Array<any>;
		onExecute?: any;
	}

	export interface MenuSubitemOptions
	{
		icon?: string;
		text?: string;
		title?: string;
		description?: string;
		onClick?: any;
		onExecute?: any;
		disabled?: boolean;
		visible?: boolean;
	}

	export function toMenuSubitems(subitems: Array<SemanticEntity|MenuSubitemOptions>, sudmenuTemplate?: any): Array<MenuSubitemOptions>
	{
		if (!subitems) return null;

		var result = [];

		subitems.forEach((subitem, a, b) =>
		{
			if (!subitem) return;

			if (subitem instanceof SemanticEntity)
				subitem = (<SemanticEntity>subitem).toListMenuItem();
			else
				subitem = $.extend({}, subitem);

			if (sudmenuTemplate)
				subitem["template"] = sudmenuTemplate;

			result.push(subitem);
		});

		return result;
	}


}