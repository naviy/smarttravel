/// <reference path='../../Scripts/typings/angularjs/angular.d.ts' />
/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />


module Controls
{

	export interface IFormGridScope extends ng.IScope
	{
		app: IAppModule;
		viewPage: App.ViewPage;

		rows: () => any[];

		canView: (r?) => boolean;
		openView: (r) => void;
		canNew: () => boolean;
		openNew: () => void;
		canLink: () => boolean;
		openLink: () => void;
		canDelete: (r?) => void;
		delete: (r) => void;
	}

	export interface IFormGridConfig
	{
		rows: (r) => any[];
		service: Domain.EntityService;

		newParams: (mr) => any;
		linkParams: (mr) => any;
		deleteApi: string;
	}


	export function FormGridCtrl(cfg: IFormGridConfig)
	{
		return function ($scope: IFormGridScope)
		{
			ConfigureFormGrid($scope, cfg);
		};
	}

	export function ConfigureFormGrid($scope: IFormGridScope, cfg: IFormGridConfig)
	{
		$scope.app = app;

		$scope.rows = () => $scope.$parent['r'] && cfg.rows($scope.$parent['r']);

		$scope.canView = (r?) => cfg.service.canView($scope, r);
		$scope.openView = r => cfg.service.openView({ scope: $scope, r: r });
		$scope.canNew = () => cfg.service.canNew($scope);
		$scope.openNew = () => cfg.service.openNew({ scope: $scope, params: cfg.newParams && cfg.newParams($scope.$parent['r']) });
		$scope.canLink = () => cfg.service.canLink($scope, $scope.$parent['r']);
		$scope.openLink = () => cfg.service.openLink({ scope: $scope, mr: $scope.$parent['r'], params: cfg.linkParams && cfg.linkParams($scope.$parent['r']) });
		$scope.canDelete = (r?) => cfg.service.canDelete($scope, r);

		
		$scope.delete = (r) =>
		{
			cfg.service.delete(<any>$scope.$parent, r.Id, {
				deleteApi: cfg.deleteApi,
				callback: () =>
				{
					var rows = $scope.rows();
					if (!rows) return;
					var i = rows.indexOf(r);
					if (i < 0) return;
					rows.splice(i, 1);
				}
			});
		};
	}



}