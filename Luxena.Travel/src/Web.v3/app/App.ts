/// <reference path='../Scripts/typings/angularjs/angular.d.ts' />

declare var window: Window;
declare var document: Document;
declare var $log: any;// (any) => any;
declare var $logb: any;//(any, any) => any;
declare var $loge: () => void;
declare var encodeURIComponent: (url: string) => string;

declare var parseInt;


interface IAppModule extends ng.IModule
{
	views?: IViewModule;
	openView?: (path: App.IViewPath) => App.ViewPage;
	path?: (value?: string) => string;
	$digest?: ($scope: any) => void;

	$http?: ng.IHttpService;
	$modal? ;
	$timeout?: ng.ITimeoutService;
}

interface IViewModule extends ng.IModule
{
}


var app: IAppModule = angular.module('app', ['ui.bootstrap', 'app.views']);

app.views = angular.module('app.views', []);

app.$digest = $scope =>
{
	if ($scope.$$phase || $scope.$root.$$phase) return;
	$scope.$digest();
};


module App
{

	export interface IViewportScope extends ng.IScope
	{
		app?: IAppModule;
		viewPages?: ViewPage[];
	}


	export function ViewportCtrl(
		$scope: IViewportScope,
		/*REQUIRED*/ $route,
		$http: ng.IHttpService,
		$location: ng.ILocationService,
		$modal,
		$timeout: ng.ITimeoutService) 
	{
		app.$http = $http;
		app.$modal = $modal;
		app.$timeout = $timeout;

		$scope.app = app;
		$scope.viewPages = ViewPage.rootAll;
		$scope.$on('$routeChangeSuccess', (e, current) =>
		{
			//$log(current);
			//$log.trace();
			//$logb('$routeChangeSuccess ' + (current && current['viewName']), () =>
			//{
			current && ViewPage.open({ name: current['viewName'], id: current.params['id'] });
			//});
		});

		app.path = (value?) =>
		{
			//$logb('path "' + value + '"', () =>
			if (!value)
				return $location.path();
			$location.path(value);
			//);
		};

		app.openView = (path: IViewPath) =>
		{
			return $logb('app.openView # ' + path.name, () =>
			{
			$log(path);
			if (!path) return null;

			var viewPage = ViewPage.open(path);
			if (viewPage)
			{
				if (path.target && !path.part)
					app.path(path.name + (path.id ? '/' + path.id : ''));
				app.$digest($scope);
			}

			return viewPage;
			});
		};

		ViewPage.skipChangePath = true;
		ViewPage.open({ name: 'index' });
	}


	export interface IViewPath
	{
		name: string;
		id?: any;
		params?: any;
		target?: ViewPage;
		part?: {
			name: string;
			path: IViewPath;
		};
	}

	export interface IViewPart
	{
		name: string;
		viewPages: ViewPage[];
		last?: ViewPage;
	}

	export class ViewPage
	{
		closable: boolean = true;

		constructor(
			public key: string,
			public viewName: string,
			public id: any = null,
			public params: any = {},
			public active: boolean = true)
		{
			this.tagName = viewName.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
		}

		tagName: string;
		isActivated: boolean;

		title() { return this.key; }
		tooltip() { return this.title(); }
		load() { }

		parent: ViewPage;
		parts: { [id: string]: IViewPart; } = {};
		dependencies: string[];


		static open(path: IViewPath): ViewPage
		{

			//return $logb('ViewPage.open', () =>
			//{
			//$log('path: ', path);
			var viewPage: ViewPage = null;
			var parentViewPage: ViewPage = null;
			var part: IViewPart;
			var key = '';
			while (path)
			{
				if (path.id && path.id.Id) path.id = path.id.Id;
				path.params = path.params || {};

				key += (key && '/') + path.name + '#' + (path.id || null);

				viewPage = ViewPage.allByKey[key];

				if (!viewPage)
				{
					//$log(path.name, ': new page');
					ViewPage.allByKey[key] = viewPage = new ViewPage(key, path.name, path.id, path.params);
					if (!path.target)
					{
						ViewPage.all.push(viewPage);
						if (part)
							part.viewPages.push(viewPage);
						else
							ViewPage.rootAll.push(viewPage);
					}
					else
					{
						if (part)
						{
							path.target.close();
							ViewPage.all.push(viewPage);
							part.viewPages.push(viewPage);
						}
						else
						{
							path.target.close(false);
							ViewPage.all.push(viewPage);

							var i = ViewPage.rootAll.indexOf(path.target);
							if (i >= 0)
								ViewPage.rootAll[i] = viewPage;
							else
								ViewPage.rootAll.push(viewPage);

							ViewPage.skipChangePath = true;
						}
					}

					if (part)
						part.last = part.viewPages[part.viewPages.length - 1];
				}
				else
				{
					//$log(path.name, ': activate');
					angular.extend(viewPage.params, path.params);

					if (!viewPage.active)
						viewPage.active = true;
				}

				viewPage.parent = parentViewPage;
				parentViewPage = viewPage;

				if (!path.part || !path.part.name || !path.part.path) break;

				part = viewPage.parts[path.part.name] = { name: path.part.name, viewPages: [] };

				path = path.part.path;
			}


			//$log('ViewPage.all', ViewPage.all);
			//$log('ViewPage.rootAll', ViewPage.rootAll);
			return viewPage;
			//});
		}

		activate()
		{
			//$logb('activate ' + this.key, () =>
			//{
			if (ViewPage.skipChangePath)
				ViewPage.skipChangePath = false;
			else
			{
				//if (this.isActivated)
				app.path(this.viewName + (this.id ? '/' + this.id : ''));
				//else
				//	this.isActivated = true;
			}
			//});
		}

		close(removeFromRootAll: boolean = true)
		{
			var i = ViewPage.all.indexOf(this);
			if (i > -1)
				ViewPage.all.splice(i, 1);

			delete ViewPage.allByKey[this.key];

			if (removeFromRootAll)
			{
				i = ViewPage.rootAll.indexOf(this);
				if (i > -1)
				{
					ViewPage.rootAll.splice(i, 1);
					//ViewPage.all[i - 1].active = true;
				}
			}

			for (var partName in this.parts)
			{
				var part = this.parts[partName];
				for (var j = 0, page; page = part.viewPages[j++];)
				{
					page.close();
				}
			}
		}


		loadParts()
		{
			if (!this.parts) return;

			for (var partName in this.parts)
			{
				var part = this.parts[partName];
				for (var i = 0, page; page = part.viewPages[i++];)
				{
					page.load();
				}
			}
		}

		loadDependencies()
		{
			//$logb('loadDependencies', () =>
			//{
			var me = this;
			var parent = me.parent;
			if (parent)
			{
				parent.load();
				parent.loadDependencies();
			}

			if (me.dependencies)
			{
				for (var i = 0, dependency; dependency = me.dependencies[i++];)
				{
					for (var j = 0, page; page = ViewPage.all[j++];)
					{
						if (page.key.indexOf(dependency) < 0) continue;
						page.load();
					}
				}
			}
			//});
		}

		static all: ViewPage[] = [];
		static rootAll: ViewPage[] = [];
		static allByKey: { [id: string]: ViewPage; } = {};
		static skipChangePath: boolean = false;
	}


	app.views.directive('viewPane', function ($parse, $compile)
	{
		return {
			restrict: 'E',
			scope: { viewPage: '=' },

			link: function (scope, element, attrs)
			{
				$log('view-pane.scope: ', scope);
				element.html('<div><' + scope.viewPage.tagName + ' /></div>\n');
				$compile(element.contents())(scope);
			},

			replace: true
		};
	});

}