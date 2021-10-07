module Domain
{

	export interface IEntityContainer
	{
		viewPage: App.ViewPage;
		loading: boolean;
		needReload: boolean;
	}

	export interface IEntityList extends IEntityContainer
	{
		data: any[];
		totalCount: number;
		loadedCount: number;

		pageSize: number;

		load: () => void;
		refresh: () => void;

		canDelete: (r?) => boolean;
		delete?: () => void;

		searchText?: string;

		sort?: {
			name: string;
			descending: boolean;
		};
	}

	export interface IEntityListConfig
	{
		service: EntityService;

		masterRow?: ($scope) => any;

		listParams?: (mr) => any;
		listApi?: string;

		newParams?: (mr) => any;

		deleteApi?: string;

		sort?: any; 	//{ name: string; descending: string; }
	}


	export interface IEntityForm extends IEntityContainer
	{
		id: any;
		r: any;
		saving?: boolean;
	}

	export interface IEntityFormConfig
	{
		service: EntityService;
		viewApi?: string;
		editApi?: string;
		saveApi?: string;
		deleteApi?: string;

		title?: (scope: any, r: any) => string;
		title1?: (scope: any, r: any) => string;
	}


	export interface IEntityServiceConfig
	{
		names?: string;

		api?: string;
		listApi?: string;
		viewApi?: string;
		editApi?: string;
		postApi?: string;
		saveApi?: string;
		deleteApi?: string;
		suggestApi?: string;
	}


	export class EntityService
	{
		constructor(public name?: string, cfg?: IEntityServiceConfig)
		{
			var me = this;
			me.name = name;
			if (cfg)
				angular.extend(me, cfg);

			if (!me.names) me.names = name + 's';
			me.initApi();
			me.initViewNames();
		}

		names: string;

		titles: (scope) => string;
		title: (scope, r, title1?: (scope, r) => string) => string;
		title1: (scope, r) => string;


		//#region Api

		api: string;
		listApi: string;
		viewApi: string;
		editApi: string;
		saveApi: string;
		deleteApi: string;
		suggestApi: string;

		initApi()
		{
			var me = this;
			if (!me.api) me.api = 'api/' + me.names;
			if (!me.listApi) me.listApi = me.api + '/list';
			if (!me.viewApi) me.viewApi = me.api + '/view/{id}';
			if (!me.editApi) me.editApi = me.api + '/edit/{id}';
			if (!me.saveApi) me.saveApi = me.api + '/save';
			if (!me.deleteApi) me.deleteApi = me.api + '/delete';
			if (!me.suggestApi) me.suggestApi = me.api + '/suggest';
		}

		load($scope: IEntityContainer, url: string, callback: (any) => void): ng.IHttpPromise<any>
		{
			$scope.loading = true;

			return app.$http.get(url, { params: $scope.viewPage.params }).success(data =>
			{
				if (callback)
					callback(data);

				$scope.needReload = false;
				$scope.loading = false;
				app.$digest($scope);
			}).error(msg =>
				{
					$scope.loading = false;
					app.$digest($scope);
					$log.error(msg);
				});
		}

		suggest(search: string, cfg?: IEntityServiceConfig)
		{
			if (!search) return null;

			search = encodeURIComponent(search);
			var api = cfg && cfg.suggestApi || this.suggestApi;

			return app.$http.get(api + '/' + search).then(response => response.data);
		}

		save($scope: IEntityForm, url: string, callback: (r: any, isNew: boolean) => void): ng.IHttpPromise<any>
		{
			$scope.saving = true;
			var isNew = !$scope.r || $scope.r.isNew || false;

			return app.$http.post(url, $scope.r).success(data =>
			{
				if (callback)
					callback(data, isNew);

				if ($scope.viewPage)
					$scope.viewPage.loadDependencies();

				$scope.saving = false;
				app.$digest($scope);
				$log.success('Record saved');
			}).error(msg =>
			{
				$scope.saving = false;
				app.$digest($scope);
				$log.error(msg);
			});
		}

		delete($scope: IEntityForm, id, cfg?: { deleteApi?: string; callback: () => void; }): ng.IHttpPromise<any>
		{
			$scope.loading = true;


			var api = cfg && cfg.deleteApi || this.deleteApi;
			var post = api.indexOf('{id}')
				? app.$http.get(api.replace('{id}', id), null)
			//Fake: delete is buggy
				: app.$http.post(api, { id: id });

			return post.success(() =>
			{
				if (cfg.callback)
					cfg.callback();
				$scope.needReload = false;
				$scope.loading = false;
				app.$digest($scope);
			}).error(msg =>
				{
					$scope.loading = false;
					app.$digest($scope);
					$log.error(msg);
				});
		}

		//#endregion


		//#region List

		loadList($scope: IEntityList, cfg?: IEntityListConfig): ng.IHttpPromise<any>
		{
			$scope.loading = true;

			var prms = {
				pageSize: $scope.pageSize,
				skip: $scope.needReload ? null : $scope.loadedCount,
				sorts: [$scope.sort],
				searchText: $scope.searchText
			};
			if (cfg.listParams && cfg.masterRow)
			{
				var mr = cfg.masterRow($scope);
				if (mr)
					angular.extend(prms, cfg.listParams(mr));
			}

			return app.$http.post(cfg && cfg.listApi || this.listApi, prms).success(data =>
			{
				if ($scope.needReload)
					$scope.data.splice(0, $scope.data.length);
				$scope.totalCount = data.TotalCount;

				if (data)
				{
					if (data.Data)
						data = data.Data;
					for (var i = 0, r; r = data[i++];)
					{
						$scope.data.push(r);
					}
				}
				$scope.loadedCount = $scope.data.length;

				$scope.needReload = false;
				$scope.loading = false;
				app.$digest($scope);
			}).error((msg) =>
				{
					$scope.loading = false;
					app.$digest($scope);
					$log.error(msg);
				});
		}

		listDelete($scope: IEntityList, ids: any[], cfg?: IEntityListConfig): ng.IHttpPromise<any>
		{
			$scope.loading = true;

			//Fake: delete is buggy
			return app.$http.post(cfg && cfg.deleteApi || this.deleteApi, {
				Ids: ids
			}).success(data =>
				{
					var recs = $scope.data;
					for (var i = 0, id; id = data.Ids[i++];)
					{
						for (var j = 0, r; r = recs[j]; j++)
						{
							if (r.Id == id)
							{
								recs.splice(j--, 1);
								//$log('delete: ', recs.length);
								break;
							}
						}
					}

					$scope.totalCount = data.totalCount;
					$scope.loadedCount = $scope.data.length;

					$scope.needReload = false;
					$scope.loading = false;
					app.$digest($scope);
				}).error(msg =>
				{
					$scope.loading = false;
					app.$digest($scope);
					$log.error(msg);
				});
		}

		rowDelete($scope: IEntityList, id: any, cfg?: IEntityListConfig): ng.IHttpPromise<any>
		{
			var deleteApi = cfg && cfg.deleteApi || this.deleteApi;
			if (!deleteApi) return null;

			deleteApi = deleteApi.replace('{id}', id);

			$scope.loading = true;

			return app.$http.post(deleteApi, {
				Ids: [ id ]
			}).success(data =>
				{
					var recs = $scope.data;
					for (var j = 0, r; r = recs[j]; j++)
					{
						if (r.Id == id)
						{
							recs.splice(j--, 1);
							//$log('delete: ', recs.length);
							break;
						}
					}

					$scope.totalCount = data.totalCount;
					$scope.loadedCount = $scope.data.length;

					$scope.needReload = false;
					$scope.loading = false;
					app.$digest($scope);
					$scope.viewPage.loadDependencies();
				}).error(msg =>
				{
					$scope.loading = false;
					app.$digest($scope);
					$log.error(msg);
				});
		}

		//#endregion


		//#region ViewForm

		loadView($scope: IEntityForm, cfg?: IEntityFormConfig): ng.IHttpPromise<any>
		{
			var api = cfg && cfg.viewApi || this.viewApi;
			api = api.replace('{id}', $scope.id);
			return this.load($scope, api, data => $scope.r = data);
		}

		//#endregion


		//#region EditForm

		loadEdit($scope: IEntityForm, cfg?: IEntityFormConfig): ng.IHttpPromise<any>
		{
			var api = cfg && cfg.editApi || this.editApi;
			api = api.replace('{id}', $scope.id || 'null');
			return this.load($scope, api, data =>
			{
				if (!data || data == 'null') data = {};
				if (!data.Id)
					data.isNew = true;

				$scope.r = data;
			});
		}

		saveEdit($scope: IEntityForm, cfg?: IEntityFormConfig): ng.IHttpPromise<any>
		{
			var api = cfg && cfg.saveApi || this.saveApi;
			return this.save($scope, api, (data, isNew) =>
			{
				$scope.r = data;
				if (isNew)
				{
					var viewPage: App.ViewPage = $scope.viewPage;
					app.openView({ name: viewPage.viewName, id: data.Id, target: viewPage });
				}
			});
		}

		//#endregion


		//#region Views

		listViewName: string;
		linkViewName: string;
		viewFormViewName: string;
		newFormViewName: string;
		editFormViewName: string;

		initViewNames()
		{
			var me = this;
			if (!me.listViewName) me.listViewName = me.name + 'List';
			if (!me.linkViewName) me.linkViewName = me.name + 'List';
			if (!me.viewFormViewName) me.viewFormViewName = me.name;
			if (!me.newFormViewName) me.newFormViewName = me.name + 'Edit';
			if (!me.editFormViewName) me.editFormViewName = me.name + 'Edit';

			if (!me.canList) me.canList = () => false;
			if (!me.canLink) me.canLink = () => false;
			if (!me.canView) me.canView = () => false;
			if (!me.canNew) me.canNew = () => false;
			if (!me.canEdit) me.canEdit = () => false;
			if (!me.canDelete) me.canDelete = () => false;
		}

		canList(rootScope: ng.IScope, r?) { return true; }
		canView(rootScope: ng.IScope, r?) { return true; }
		canNew(rootScope: ng.IScope) { return true; }
		canLink(rootScope: ng.IScope, mr) { return true; }
		canEdit(rootScope: ng.IScope, r?) { return true; }
		canDelete(rootScope: ng.IScope, r?) { return true; }

		openLink(e: { scope: ng.IScope; mr; params? ; target?: App.ViewPage; })
		{
			if (!this.canLink(e.scope, e.mr)) return;

			var me = this;

			var modalInstance = app.$modal.open({
				templateUrl: 'Domain.LinkDialog.html',
				controller: function ($scope, $modalInstance)
				{
					$scope.viewPage = new App.ViewPage(null, me.linkViewName);
					$scope.ok = () => $modalInstance.close($scope.getSelectedIds());
					$scope.cancel = () => $modalInstance.dismiss('cancel');
				}
			});

			modalInstance.result.then(selectedItem =>
			{
				//scope.selected = selectedItem;
			});

		}

		openView(e: { scope: ng.IScope; r; target?: App.ViewPage; })
		{
			if (this.canView(e.scope, e.r))
				app.openView({
					name: this.viewFormViewName,
					id: e.r.Id,
					target: e.target
				});
		}

		openNew(e: { scope: ng.IScope; params? ; target?: App.ViewPage; })
		{
			if (this.canNew(e.scope))
				app.openView({
					name: this.newFormViewName,
					params: e.params,
					target: e.target
				});
		}

		openEdit(e: { scope: ng.IScope; r; target?: App.ViewPage; })
		{
			if (this.canEdit(e.scope, e.r))
				app.openView({
					name: this.editFormViewName,
					id: e.r.Id,
					target: e.target
				});
		}

		//#endregion

	};

}


var db = Domain;
