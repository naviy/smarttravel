module Luxena
{

	export var smartTarget = ko.observable();
	export var smartTitle = ko.observable<string>();
	export var smartViewScope = ko.observable();
	export var smartVisible = ko.observable(false);
	export var smartWidth = ko.observable<number>();
	//export var smartButtons = ko.observableArray();

	export function getSmartPopoverOptions()
	{
		return <DevExpress.ui.dxPopoverOptions>{
			target: <any>smartTarget,
			title: <any>smartTitle,
			visible: <any>smartVisible,
			//buttons: <any>smartButtons,

			position: "bottom",
			width: smartWidth,
			//showTitle: true,
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
		};
	}


	export interface SmartFormControllerConfigExt extends ViewFormControllerConfig
	{
		refreshMaster?: () => void;
		contentWidth?: number;
	}

	export interface SmartFormControllerConfig extends SmartFormControllerConfigExt
	{
		id: any;
	}


	export class SmartFormController extends FormController<SmartFormControllerConfig>
	{
		smartMode = true;

		getToolbarItems()
		{
			const cfg = this.config;
			const se = this._entity;

			return [
				cfg.view && cfg.view.viewAction.button(),
				cfg.edit && cfg.edit.editAction.button().small().right(),
				sd.button(
					cfg.edit && cfg.edit.deleteAction,
					se.refreshAction
				).right(),
			];
		}

		//getBottomToolbarItems()
		//{
		//	const cfg = this.config;
		//	const se = this._entity;

		//	const buttons = [
		//		cfg.view && cfg.view.viewAction.button().right(),
		//		cfg.edit && cfg.edit.editAction.button().small().right(),
		//		sd.button(
		//			cfg.edit && cfg.edit.deleteAction.button().small(),
		//			se.refreshAction.button().small()
		//		).right(),
		//	].map(btn => $.extend(btn.toolbarItemOptions(), { toolbar: "bottom" }));

		//	$log(buttons);

		//	return buttons;
		//}

		show(target)
		{
			const cfg = this.config;

			smartVisible(false);
			smartTarget(target);

			var scope = this.getScope();
			
			this.loadData(() =>
			{
				smartWidth(cfg.contentWidth ? cfg.contentWidth + 40 : 500);
				smartTitle(/*this.iconHtml() +*/this.title());
				//smartButtons(this.getBottomToolbarItems());
				smartViewScope(scope);
				smartVisible(true);
			});
		}

	}

}