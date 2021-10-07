/// <reference path='../../Scripts/typings/angularjs/angular.d.ts' />
/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />


module Controls
{

	export interface IGridScope extends ng.IScope, Domain.IEntityList
	{
		app: IAppModule;

		selectedCount: () => number;
		toggleSelect: (r?) => void;
		selectAll: (value: boolean) => void;
		selectedCountLabel: () => string;
		getSelected: () => any[];
		getSelectedIds: () => any[];

		sortBy?: (column: string) => void;
		sortIcon?: (column: string) => string;

		canView: (r?) => boolean;
		openView: (r) => void;
		canNew: () => boolean;
		openNew: () => void;
	}

	export interface IGridConfig extends Domain.IEntityListConfig
	{
	}


	export function GridCtrl(cfg: IGridConfig)
	{
		return ($scope: IGridScope) => ConfigureGrid($scope, angular.copy(cfg));
	}

	export function ConfigureGrid($scope: IGridScope, cfg: IGridConfig)
	{
		$scope.app = app;
		ConfigureGridService($scope, cfg);
		ConfigureGridSelection($scope, cfg);
		ConfigureGridSort($scope, cfg);
		ConfigureGridFilter($scope, cfg);
	}


	function ConfigureGridService($scope: IGridScope, cfg: IGridConfig)
	{
		if (cfg.service.titles)
			$scope.viewPage.title = () => cfg.service.titles($scope);

		$scope.data = [];
		$scope.loadedCount = 0;
		$scope.pageSize = 25;
		$scope.needReload = false;

		//$log('ConfigureGridService.$scope: ', $scope);

		if (cfg.masterRow === <any>true)
			cfg.masterRow = () => $scope['$parent'] && $scope['$parent']['r'];

		if (cfg.masterRow)
			$scope.loading = true;

		$scope.load = () => cfg.service.loadList($scope, cfg);
		$scope.refresh = () => $scope.needReload = true;
		$scope.viewPage.load = () => { $scope.data.splice(0, $scope.data.length); $scope.load(); };

		$scope.$watch('needReload', () => 
		{
			if ($scope.needReload && !$scope.loading)
				$scope.load();
		});

		$scope.canDelete = (r?) => cfg.service.canDelete($scope, r);
		$scope.delete = (r?) =>
		{
			if (r)
				cfg.service.rowDelete($scope, r.Id, cfg);
			else
				cfg.service.listDelete($scope, $scope.getSelectedIds(), cfg);
		};


		$scope.canView = (r?) => cfg.service.canView($scope, r);
		if (cfg.masterRow)
			$scope.openView = r => cfg.service.openEdit({ scope: $scope, r: r });
		else
			$scope.openView = r => cfg.service.openView({ scope: $scope, r: r });
		$scope.canNew = () => cfg.service.canNew($scope);
		$scope.openNew = () => cfg.service.openNew({
			scope: $scope,
			params: cfg.newParams && cfg.newParams(cfg.masterRow($scope)) || cfg.listParams && cfg.listParams(cfg.masterRow($scope))
		});

	}

	function ConfigureGridSelection($scope: IGridScope, cfg: IGridConfig)
	{
		$scope.selectedCount = () =>
		{
			var count = 0;
			for (var i = 0, r; r = $scope.data[i++];)
			{
				if (r.selected)
					count++;
			}
			return count;
		};

		$scope.toggleSelect = (r?) =>
		{
			if (r)
				r.selected = !r.selected;
			else
				$scope.selectAll(!$scope.selectedCount());
		};

		$scope.selectAll = value =>
		{
			if (typeof value === 'undefined')
				value = true;

			for (var i = 0, a; a = $scope.data[i++];)
			{
				a.selected = value;
			}
		};

		$scope.selectedCountLabel = () =>
		{
			var count = $scope.selectedCount();
			return count
				? '<span class="label label-info">' + count + '</span>'
				: '<span class="label label-default"><i class="fa fa-check"></i></span>';
		};

		$scope.getSelected = () =>
		{
			var res = [];
			for (var i = 0, r; r = $scope.data[i++];)
			{
				if (r.selected)
					res.push(r);
			}
			return res;
		};

		$scope.getSelectedIds = () =>
		{
			var res = [];
			for (var i = 0, r; r = $scope.data[i++];)
			{
				if (r.selected)
					res.push(r.Id);
			}
			return res;
		};
	}

	function ConfigureGridSort($scope: IGridScope, cfg: IGridConfig)
	{
		$scope.sort =
		!cfg.sort ? { name: 'Name', descending: false }
		: typeof cfg.sort == 'string' ? $scope.sort = { name: cfg.sort, descending: false }
		: $scope.sort = cfg.sort;

		$scope.$watch('sort.name', () => $scope.needReload = true);
		$scope.$watch('sort.descending', () => $scope.needReload = true);

		$scope.sortBy = (column: string) =>
		{
			if ($scope.sort.name == column)
			{
				$scope.sort.descending = !$scope.sort.descending;
			} else
			{
				$scope.sort.name = column;
				$scope.sort.descending = false;
			}
		};

		$scope.sortIcon = (column: string) =>
		{
			return (
				$scope.sort.name != column ? 'fa fa-chevron-up text-muted'
				: $scope.sort.descending ? 'fa fa-chevron-down'
				: 'fa fa-chevron-up'
			);
		};
	}

	function ConfigureGridFilter($scope: IGridScope, cfg: IGridConfig)
	{
		var timer: ng.IPromise<any>;

		$scope.$watch('searchText', () =>
		{
			if (timer)
			{
				app.$timeout.cancel(timer);
			}
			if (!$scope.needReload)
				timer = app.$timeout(() => $scope.needReload = true, 500);
		});
	}


	app.views.directive('sortable', () =>
	{
		return {
			restrict: 'A',
			compile: (element, attrs) =>
			{
				var text =
					'<span class="sortable-column" ng-click="sortBy(\'' + attrs.sortable + '\')">' +
					element.text() + ' <i ng-class="sortIcon(\'' + attrs.sortable + '\')"></i>' +
					'</span>';
				element.html(text);
			}
		};
	});

}