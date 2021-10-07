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

		menu?: IMenuItem[];

		//commandMapping?: {
		//	[containderId: string]: {
		//		defaults?: DevExpress.framework.dxCommandOptions;
		//		commands?: DevExpress.framework.dxCommandOptions;
		//	}
		//};

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


	export type MenuItems = Array<Components.Button | SemanticEntity | SemanticEntityAction | IMenuItem>;

	export interface IMenuItem
	{
		icon?: string;
		text?: string;
		hint?: string;
		template?: any;
		onClick?: any;
		
		items?: IMenuItem[];
	}

	export function toMenuItems(subitems: MenuItems, submenuTemplate?: any)
	{
		if (!subitems) return null;

		var buttons: IMenuItem[] = [];

		subitems.forEach(subitem =>
		{
			if (!subitem) return;

			var btn: IMenuItem;

			if (subitem instanceof Components.Button)
				btn = subitem.buttonOptions();
			else if (subitem instanceof SemanticEntityAction)
				btn = subitem.button().buttonOptions();
			else if (subitem instanceof SemanticEntity)
				btn = subitem.listAction.button().buttonOptions();
			else
				btn = $.extend({}, subitem);

			if (submenuTemplate)
				btn.template = submenuTemplate;
			//btn.template = submenuTemplate || "item";

			buttons.push(btn);
		});

		return buttons;
	}


}