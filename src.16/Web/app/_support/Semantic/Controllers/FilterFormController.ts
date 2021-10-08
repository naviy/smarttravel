module Luxena
{


	export interface IFilterFormControllerConfig extends EditFormControllerConfig
	{
		filter?: (model: any) => Array<any[]|SemanticMember>;
	}


	export class FilterFormController extends BaseEditFormController<IFilterFormControllerConfig>
	{
		defaultContainerId = "filterFields";

		filterMode = true;

		filter = ko.observable();


		getScope(): any
		{
			var scope = super.getScope();

			this.apply();

			scope.applyFilter = () => this.apply();

			scope.findButton = {
				text: "Начать поиск",
				icon: "search",
				onClick: () => this.apply(),
				type: "default"
			};

			var collapsed = false;
			var collapseIcon = ko.observable("fa fa-arrow-left");
			scope.filterPanelWidth = ko.observable(440);

			scope.collapseButton = {
				hint: "Свернуть панель фильтра",
				icon: collapseIcon,
				onClick: () =>
				{
					collapsed = !collapsed;
					scope.filterPanelWidth(collapsed ? 55 : 440);
					collapseIcon(collapsed ? "fa fa-arrow-right" : "fa fa-arrow-left");
				},
			};

			return scope;
		}

		getScopeWithGrid(gridScope): any
		{
			var scope = this.getScope();

			if (gridScope.getScope)
				gridScope = gridScope.getScope();

			scope.titleMenuItems = gridScope.titleMenuItems,

			scope.viewToolbarItems = gridScope.viewToolbarItems;
			scope.title = gridScope.title;
			scope.gridOptions = gridScope.gridOptions;

			return scope;
		}


		getMenuItems(): any[]
		{
			return [{
				icon: "refresh",
				text: "Обновить",
				onExecute: () => this.apply(),
			}];
		}


		loadData()
		{
			if (!this.model["__isLoaded"])
			{
				this.loadFromData({});
				this.model["__isLoaded"] = true;
			}
			else
			{
				this.applyModel();
			}

			this.apply();
		}


		apply()
		{
			const cfg = this.config;

			let filter = cfg.filter && cfg.filter(this.model) || this.members;
			filter = prepareFilterExpression(this.model, filter);
			
			this.filter(filter);
		}

	}



	export function prepareFilterExpression(model: any, list: any[])
	{
		if (!list || !list.length)
			return undefined;

		//$logb("prepareFilterExpression");

		var result = [];
		list.forEach(a =>
		{
			if ($.isArray(a))
				a = prepareFilterExpression(model, a);
			else if (a instanceof SemanticMember)
				a = (<SemanticMember>a).filter(model);
			//$log(" => ", a);
			if (a !== undefined)
				result.push(a);
		});

		//$log("result:", result);

		var result2 = [];

		var priorIsOperation = true;

		result.forEach((item, i) =>
		{
			var isOperation = item === "and" || item === "or";

			if (!isOperation || (!priorIsOperation && i < result.length - 1))
			{
				result2.push(item);

				priorIsOperation = isOperation;
			}
		});

		if (priorIsOperation && result2.length)
			result2.pop();

		result2 = result2.length ? result2 : undefined;

		//$loge();

		return result2;
	};

}