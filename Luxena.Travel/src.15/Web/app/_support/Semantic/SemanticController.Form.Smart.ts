module Luxena
{

	export var smartTarget = ko.observable();
	export var smartTitle = ko.observable<string>();
	export var smartViewScope = ko.observable();
	//export var smartButtons = ko.observable();
	export var smartVisible = ko.observable(false);

	export function getSmartPopoverOptions(): DevExpress.ui.dxPopoverOptions
	{
		return {
			target: <any>smartTarget,
			title: <any>smartTitle,
			visible: <any>smartVisible,

			position: "bottom",
			width: 800,
			showTitle: true,
			showCloseButton: true,
			closeOnBackButton: true,
			//animation: {
			//	show: {
			//		type: "fade",
			//		from: 0,
			//		to: 1
			//	},
			//	hide: {},
			//},
			onShown: (e) =>
			{
				//$log("repaint");
				//e.component.repaint();
				//var content = $("#smartcontent");
				//$log(content);
				//var scroll = content.dxScrollView("instance");
				//$log(scroll);
				//scroll["refresh"]();
			},
		};
	}

	export interface ISmartFormControllerConfig extends IViewFormControllerConfig
	{
		id: any;
		refreshMaster?: () => void;
	}


	export class SmartFormController extends FormController<ISmartFormControllerConfig>
	{
		smartMode = true;

		getMenuItems(): any[]
		{
			return undefined;
		}

		getScope(): any
		{
			var cfg = this.config;
			const se = cfg.entity;

			cfg.view = cfg.view || cfg.view !== null && (cfg.form || se);
			cfg.edit = cfg.edit || cfg.edit !== null && (cfg.form || se);

			const scope = super.getScope();

			const id = this.id;
			const btns = scope.buttons = [];

			cfg.view && btns.push(cfg.view.toViewButton(id));
			cfg.edit && btns.push(cfg.edit.toEditButton(id));
			cfg.edit && btns.push(cfg.edit.toDeleteButton(id, () =>
			{
				cfg.refreshMaster();
				smartVisible(false);
			}));

			cfg.actions && cfg.actions.forEach(action => btns.push(action.toButton({ id: id })));

			return scope;
		}

		show(target)
		{
			smartVisible(false);
			smartTarget(target);

			var scope = this.getScope();

			this.loadData(() =>
			{
				smartTitle(scope.title);
				smartViewScope(scope);
				smartVisible(true);
			});
		}

	}

}