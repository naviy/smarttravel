/// <reference path='../../Scripts/typings/angularjs/angular.d.ts' />
/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />
var Controls;
(function (Controls) {
    function GridCtrl(cfg) {
        return function ($scope) {
            return ConfigureGrid($scope, angular.copy(cfg));
        };
    }
    Controls.GridCtrl = GridCtrl;

    function ConfigureGrid($scope, cfg) {
        $scope.app = app;
        ConfigureGridService($scope, cfg);
        ConfigureGridSelection($scope, cfg);
        ConfigureGridSort($scope, cfg);
        ConfigureGridFilter($scope, cfg);
    }
    Controls.ConfigureGrid = ConfigureGrid;

    function ConfigureGridService($scope, cfg) {
        if (cfg.service.titles)
            $scope.viewPage.title = function () {
                return cfg.service.titles($scope);
            };

        $scope.data = [];
        $scope.loadedCount = 0;
        $scope.pageSize = 25;
        $scope.needReload = false;

        if (cfg.masterRow === true)
            cfg.masterRow = function () {
                return $scope['$parent'] && $scope['$parent']['r'];
            };

        if (cfg.masterRow)
            $scope.loading = true;

        $scope.load = function () {
            return cfg.service.loadList($scope, cfg);
        };
        $scope.refresh = function () {
            return $scope.needReload = true;
        };
        $scope.viewPage.load = function () {
            $scope.data.splice(0, $scope.data.length);
            $scope.load();
        };

        $scope.$watch('needReload', function () {
            if ($scope.needReload && !$scope.loading)
                $scope.load();
        });

        $scope.canDelete = function (r) {
            return cfg.service.canDelete($scope, r);
        };
        $scope.delete = function (r) {
            if (r)
                cfg.service.rowDelete($scope, r.Id, cfg);
else
                cfg.service.listDelete($scope, $scope.getSelectedIds(), cfg);
        };

        $scope.canView = function (r) {
            return cfg.service.canView($scope, r);
        };
        if (cfg.masterRow)
            $scope.openView = function (r) {
                return cfg.service.openEdit({ scope: $scope, r: r });
            };
else
            $scope.openView = function (r) {
                return cfg.service.openView({ scope: $scope, r: r });
            };
        $scope.canNew = function () {
            return cfg.service.canNew($scope);
        };
        $scope.openNew = function () {
            return cfg.service.openNew({
                scope: $scope,
                params: cfg.newParams && cfg.newParams(cfg.masterRow($scope)) || cfg.listParams && cfg.listParams(cfg.masterRow($scope))
            });
        };
    }

    function ConfigureGridSelection($scope, cfg) {
        $scope.selectedCount = function () {
            var count = 0;
            for (var i = 0, r; r = $scope.data[i++];) {
                if (r.selected)
                    count++;
            }
            return count;
        };

        $scope.toggleSelect = function (r) {
            if (r)
                r.selected = !r.selected;
else
                $scope.selectAll(!$scope.selectedCount());
        };

        $scope.selectAll = function (value) {
            if (typeof value === 'undefined')
                value = true;

            for (var i = 0, a; a = $scope.data[i++];) {
                a.selected = value;
            }
        };

        $scope.selectedCountLabel = function () {
            var count = $scope.selectedCount();
            return count ? '<span class="label label-info">' + count + '</span>' : '<span class="label label-default"><i class="fa fa-check"></i></span>';
        };

        $scope.getSelected = function () {
            var res = [];
            for (var i = 0, r; r = $scope.data[i++];) {
                if (r.selected)
                    res.push(r);
            }
            return res;
        };

        $scope.getSelectedIds = function () {
            var res = [];
            for (var i = 0, r; r = $scope.data[i++];) {
                if (r.selected)
                    res.push(r.Id);
            }
            return res;
        };
    }

    function ConfigureGridSort($scope, cfg) {
        $scope.sort = !cfg.sort ? { name: 'Name', descending: false } : typeof cfg.sort == 'string' ? $scope.sort = { name: cfg.sort, descending: false } : $scope.sort = cfg.sort;

        $scope.$watch('sort.name', function () {
            return $scope.needReload = true;
        });
        $scope.$watch('sort.descending', function () {
            return $scope.needReload = true;
        });

        $scope.sortBy = function (column) {
            if ($scope.sort.name == column) {
                $scope.sort.descending = !$scope.sort.descending;
            } else {
                $scope.sort.name = column;
                $scope.sort.descending = false;
            }
        };

        $scope.sortIcon = function (column) {
            return ($scope.sort.name != column ? 'fa fa-chevron-up text-muted' : $scope.sort.descending ? 'fa fa-chevron-down' : 'fa fa-chevron-up');
        };
    }

    function ConfigureGridFilter($scope, cfg) {
        var timer;

        $scope.$watch('searchText', function () {
            if (timer) {
                app.$timeout.cancel(timer);
            }
            if (!$scope.needReload)
                timer = app.$timeout(function () {
                    return $scope.needReload = true;
                }, 500);
        });
    }

    app.views.directive('sortable', function () {
        return {
            restrict: 'A',
            compile: function (element, attrs) {
                var text = '<span class="sortable-column" ng-click="sortBy(\'' + attrs.sortable + '\')">' + element.text() + ' <i ng-class="sortIcon(\'' + attrs.sortable + '\')"></i>' + '</span>';
                element.html(text);
            }
        };
    });
})(Controls || (Controls = {}));
//# sourceMappingURL=GridCtrl.js.map
