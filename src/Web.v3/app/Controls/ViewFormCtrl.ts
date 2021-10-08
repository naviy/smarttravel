/// <reference path='../../Scripts/typings/angularjs/angular.d.ts' />
/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />


module Controls
{

	export interface IViewFormScope extends ng.IScope, Domain.IEntityForm
	{
		app: IAppModule;
		viewPage: App.ViewPage;

		load: () => void;
		refresh: () => void;
		loading: boolean;
		needReload: boolean;

		canEdit: () => boolean;
		openEdit: () => void;
	}

	export interface IViewFormConfig extends Domain.IEntityFormConfig
	{
	}

	export function ViewFormCtrl(cfg: IViewFormConfig)
	{
		return ($scope: IViewFormScope) => ConfigureViewForm($scope, cfg);
	}

	export function ConfigureViewForm($scope: IViewFormScope, cfg: IViewFormConfig)
	{
		$scope.app = app;
		ConfigureViewFormService($scope, cfg);
	}

	function ConfigureViewFormService($scope: IViewFormScope, cfg: IViewFormConfig)
	{
		$scope.id = $scope.viewPage.id;
		$scope.r = null;

		if (cfg.service.title)
			$scope.viewPage.title = () => cfg.service.title($scope, $scope.r);

		$scope.needReload = false;
		$scope.loading = false;

		$scope.load = () => cfg.service.loadView($scope, cfg);

		$scope.refresh = () => $scope.needReload = true;

		$scope.$watch('needReload', () => 
		{
			if ($scope.needReload)
				$scope.load();
		});

		$scope.canEdit = () => cfg.service.canEdit($scope, $scope.r);
		$scope.openEdit = () => cfg.service.openEdit({ scope: $scope, r: $scope.r, target: $scope.viewPage });

		$scope.load();
	}

}