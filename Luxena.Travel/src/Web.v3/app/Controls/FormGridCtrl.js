/// <reference path='../../Scripts/typings/angularjs/angular.d.ts' />
/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />
var Controls;
(function (Controls) {
    function FormGridCtrl(cfg) {
        return function ($scope) {
            ConfigureFormGrid($scope, cfg);
        };
    }
    Controls.FormGridCtrl = FormGridCtrl;

    function ConfigureFormGrid($scope, cfg) {
        $scope.app = app;

        $scope.rows = function () {
            return $scope.$parent['r'] && cfg.rows($scope.$parent['r']);
        };

        $scope.canView = function (r) {
            return cfg.service.canView($scope, r);
        };
        $scope.openView = function (r) {
            return cfg.service.openView({ scope: $scope, r: r });
        };
        $scope.canNew = function () {
            return cfg.service.canNew($scope);
        };
        $scope.openNew = function () {
            return cfg.service.openNew({ scope: $scope, params: cfg.newParams && cfg.newParams($scope.$parent['r']) });
        };
        $scope.canLink = function () {
            return cfg.service.canLink($scope, $scope.$parent['r']);
        };
        $scope.openLink = function () {
            return cfg.service.openLink({ scope: $scope, mr: $scope.$parent['r'], params: cfg.linkParams && cfg.linkParams($scope.$parent['r']) });
        };
        $scope.canDelete = function (r) {
            return cfg.service.canDelete($scope, r);
        };

        $scope.delete = function (r) {
            cfg.service.delete($scope.$parent, r.Id, {
                deleteApi: cfg.deleteApi,
                callback: function () {
                    var rows = $scope.rows();
                    if (!rows)
                        return;
                    var i = rows.indexOf(r);
                    if (i < 0)
                        return;
                    rows.splice(i, 1);
                }
            });
        };
    }
    Controls.ConfigureFormGrid = ConfigureFormGrid;
})(Controls || (Controls = {}));
//# sourceMappingURL=FormGridCtrl.js.map
