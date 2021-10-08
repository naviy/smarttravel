/// <reference path='../../Scripts/typings/angularjs/angular.d.ts' />
/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />
var Controls;
(function (Controls) {
    function ViewFormCtrl(cfg) {
        return function ($scope) {
            return ConfigureViewForm($scope, cfg);
        };
    }
    Controls.ViewFormCtrl = ViewFormCtrl;

    function ConfigureViewForm($scope, cfg) {
        $scope.app = app;
        ConfigureViewFormService($scope, cfg);
    }
    Controls.ConfigureViewForm = ConfigureViewForm;

    function ConfigureViewFormService($scope, cfg) {
        $scope.id = $scope.viewPage.id;
        $scope.r = null;

        if (cfg.service.title)
            $scope.viewPage.title = function () {
                return cfg.service.title($scope, $scope.r);
            };

        $scope.needReload = false;
        $scope.loading = false;

        $scope.load = function () {
            return cfg.service.loadView($scope, cfg);
        };

        $scope.refresh = function () {
            return $scope.needReload = true;
        };

        $scope.$watch('needReload', function () {
            if ($scope.needReload)
                $scope.load();
        });

        $scope.canEdit = function () {
            return cfg.service.canEdit($scope, $scope.r);
        };
        $scope.openEdit = function () {
            return cfg.service.openEdit({ scope: $scope, r: $scope.r, target: $scope.viewPage });
        };

        $scope.load();
    }
})(Controls || (Controls = {}));
//# sourceMappingURL=ViewFormCtrl.js.map
