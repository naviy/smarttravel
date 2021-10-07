var Domain;
(function (Domain) {
    var EntityService = (function () {
        function EntityService(name, cfg) {
            this.name = name;
            var me = this;
            me.name = name;
            if (cfg)
                angular.extend(me, cfg);

            if (!me.names)
                me.names = name + 's';
            me.initApi();
            me.initViewNames();
        }
        EntityService.prototype.initApi = function () {
            var me = this;
            if (!me.api)
                me.api = 'api/' + me.names;
            if (!me.listApi)
                me.listApi = me.api + '/list';
            if (!me.viewApi)
                me.viewApi = me.api + '/view/{id}';
            if (!me.editApi)
                me.editApi = me.api + '/edit/{id}';
            if (!me.saveApi)
                me.saveApi = me.api + '/save';
            if (!me.deleteApi)
                me.deleteApi = me.api + '/delete';
            if (!me.suggestApi)
                me.suggestApi = me.api + '/suggest';
        };

        EntityService.prototype.load = function ($scope, url, callback) {
            $scope.loading = true;

            return app.$http.get(url, { params: $scope.viewPage.params }).success(function (data) {
                if (callback)
                    callback(data);

                $scope.needReload = false;
                $scope.loading = false;
                app.$digest($scope);
            }).error(function (msg) {
                $scope.loading = false;
                app.$digest($scope);
                $log.error(msg);
            });
        };

        EntityService.prototype.suggest = function (search, cfg) {
            if (!search)
                return null;

            search = encodeURIComponent(search);
            var api = cfg && cfg.suggestApi || this.suggestApi;

            return app.$http.get(api + '/' + search).then(function (response) {
                return response.data;
            });
        };

        EntityService.prototype.save = function ($scope, url, callback) {
            $scope.saving = true;
            var isNew = !$scope.r || $scope.r.isNew || false;

            return app.$http.post(url, $scope.r).success(function (data) {
                if (callback)
                    callback(data, isNew);

                if ($scope.viewPage)
                    $scope.viewPage.loadDependencies();

                $scope.saving = false;
                app.$digest($scope);
                $log.success('Record saved');
            }).error(function (msg) {
                $scope.saving = false;
                app.$digest($scope);
                $log.error(msg);
            });
        };

        EntityService.prototype.delete = function ($scope, id, cfg) {
            $scope.loading = true;

            var api = cfg && cfg.deleteApi || this.deleteApi;
            var post = api.indexOf('{id}') ? app.$http.get(api.replace('{id}', id), null) : app.$http.post(api, { id: id });

            return post.success(function () {
                if (cfg.callback)
                    cfg.callback();
                $scope.needReload = false;
                $scope.loading = false;
                app.$digest($scope);
            }).error(function (msg) {
                $scope.loading = false;
                app.$digest($scope);
                $log.error(msg);
            });
        };

        //#endregion
        //#region List
        EntityService.prototype.loadList = function ($scope, cfg) {
            $scope.loading = true;

            var prms = {
                pageSize: $scope.pageSize,
                skip: $scope.needReload ? null : $scope.loadedCount,
                sorts: [$scope.sort],
                searchText: $scope.searchText
            };
            if (cfg.listParams && cfg.masterRow) {
                var mr = cfg.masterRow($scope);
                if (mr)
                    angular.extend(prms, cfg.listParams(mr));
            }

            return app.$http.post(cfg && cfg.listApi || this.listApi, prms).success(function (data) {
                if ($scope.needReload)
                    $scope.data.splice(0, $scope.data.length);
                $scope.totalCount = data.TotalCount;

                if (data) {
                    if (data.Data)
                        data = data.Data;
                    for (var i = 0, r; r = data[i++];) {
                        $scope.data.push(r);
                    }
                }
                $scope.loadedCount = $scope.data.length;

                $scope.needReload = false;
                $scope.loading = false;
                app.$digest($scope);
            }).error(function (msg) {
                $scope.loading = false;
                app.$digest($scope);
                $log.error(msg);
            });
        };

        EntityService.prototype.listDelete = function ($scope, ids, cfg) {
            $scope.loading = true;

            //Fake: delete is buggy
            return app.$http.post(cfg && cfg.deleteApi || this.deleteApi, {
                Ids: ids
            }).success(function (data) {
                var recs = $scope.data;
                for (var i = 0, id; id = data.Ids[i++];) {
                    for (var j = 0, r; r = recs[j]; j++) {
                        if (r.Id == id) {
                            recs.splice(j--, 1);

                            break;
                        }
                    }
                }

                $scope.totalCount = data.totalCount;
                $scope.loadedCount = $scope.data.length;

                $scope.needReload = false;
                $scope.loading = false;
                app.$digest($scope);
            }).error(function (msg) {
                $scope.loading = false;
                app.$digest($scope);
                $log.error(msg);
            });
        };

        EntityService.prototype.rowDelete = function ($scope, id, cfg) {
            var deleteApi = cfg && cfg.deleteApi || this.deleteApi;
            if (!deleteApi)
                return null;

            deleteApi = deleteApi.replace('{id}', id);

            $scope.loading = true;

            return app.$http.post(deleteApi, {
                Ids: [id]
            }).success(function (data) {
                var recs = $scope.data;
                for (var j = 0, r; r = recs[j]; j++) {
                    if (r.Id == id) {
                        recs.splice(j--, 1);

                        break;
                    }
                }

                $scope.totalCount = data.totalCount;
                $scope.loadedCount = $scope.data.length;

                $scope.needReload = false;
                $scope.loading = false;
                app.$digest($scope);
                $scope.viewPage.loadDependencies();
            }).error(function (msg) {
                $scope.loading = false;
                app.$digest($scope);
                $log.error(msg);
            });
        };

        //#endregion
        //#region ViewForm
        EntityService.prototype.loadView = function ($scope, cfg) {
            var api = cfg && cfg.viewApi || this.viewApi;
            api = api.replace('{id}', $scope.id);
            return this.load($scope, api, function (data) {
                return $scope.r = data;
            });
        };

        //#endregion
        //#region EditForm
        EntityService.prototype.loadEdit = function ($scope, cfg) {
            var api = cfg && cfg.editApi || this.editApi;
            api = api.replace('{id}', $scope.id || 'null');
            return this.load($scope, api, function (data) {
                if (!data || data == 'null')
                    data = {};
                if (!data.Id)
                    data.isNew = true;

                $scope.r = data;
            });
        };

        EntityService.prototype.saveEdit = function ($scope, cfg) {
            var api = cfg && cfg.saveApi || this.saveApi;
            return this.save($scope, api, function (data, isNew) {
                $scope.r = data;
                if (isNew) {
                    var viewPage = $scope.viewPage;
                    app.openView({ name: viewPage.viewName, id: data.Id, target: viewPage });
                }
            });
        };

        EntityService.prototype.initViewNames = function () {
            var me = this;
            if (!me.listViewName)
                me.listViewName = me.name + 'List';
            if (!me.linkViewName)
                me.linkViewName = me.name + 'List';
            if (!me.viewFormViewName)
                me.viewFormViewName = me.name;
            if (!me.newFormViewName)
                me.newFormViewName = me.name + 'Edit';
            if (!me.editFormViewName)
                me.editFormViewName = me.name + 'Edit';

            if (!me.canList)
                me.canList = function () {
                    return false;
                };
            if (!me.canLink)
                me.canLink = function () {
                    return false;
                };
            if (!me.canView)
                me.canView = function () {
                    return false;
                };
            if (!me.canNew)
                me.canNew = function () {
                    return false;
                };
            if (!me.canEdit)
                me.canEdit = function () {
                    return false;
                };
            if (!me.canDelete)
                me.canDelete = function () {
                    return false;
                };
        };

        EntityService.prototype.canList = function (rootScope, r) {
            return true;
        };
        EntityService.prototype.canView = function (rootScope, r) {
            return true;
        };
        EntityService.prototype.canNew = function (rootScope) {
            return true;
        };
        EntityService.prototype.canLink = function (rootScope, mr) {
            return true;
        };
        EntityService.prototype.canEdit = function (rootScope, r) {
            return true;
        };
        EntityService.prototype.canDelete = function (rootScope, r) {
            return true;
        };

        EntityService.prototype.openLink = function (e) {
            if (!this.canLink(e.scope, e.mr))
                return;

            var me = this;

            var modalInstance = app.$modal.open({
                templateUrl: 'Domain.LinkDialog.html',
                controller: function ($scope, $modalInstance) {
                    $scope.viewPage = new App.ViewPage(null, me.linkViewName);
                    $scope.ok = function () {
                        return $modalInstance.close($scope.getSelectedIds());
                    };
                    $scope.cancel = function () {
                        return $modalInstance.dismiss('cancel');
                    };
                }
            });

            modalInstance.result.then(function (selectedItem) {
                //scope.selected = selectedItem;
            });
        };

        EntityService.prototype.openView = function (e) {
            if (this.canView(e.scope, e.r))
                app.openView({
                    name: this.viewFormViewName,
                    id: e.r.Id,
                    target: e.target
                });
        };

        EntityService.prototype.openNew = function (e) {
            if (this.canNew(e.scope))
                app.openView({
                    name: this.newFormViewName,
                    params: e.params,
                    target: e.target
                });
        };

        EntityService.prototype.openEdit = function (e) {
            if (this.canEdit(e.scope, e.r))
                app.openView({
                    name: this.editFormViewName,
                    id: e.r.Id,
                    target: e.target
                });
        };
        return EntityService;
    })();
    Domain.EntityService = EntityService;
    ;
})(Domain || (Domain = {}));

var db = Domain;
//# sourceMappingURL=Domain.js.map
