var app = angular.module('app', ['ui.bootstrap', 'app.views']);

app.views = angular.module('app.views', []);

app.$digest = function ($scope) {
    if ($scope.$$phase || $scope.$root.$$phase)
        return;
    $scope.$digest();
};

var App;
(function (App) {
    function ViewportCtrl($scope, /*REQUIRED*/ $route, $http, $location, $modal, $timeout) {
        app.$http = $http;
        app.$modal = $modal;
        app.$timeout = $timeout;

        $scope.app = app;
        $scope.viewPages = ViewPage.rootAll;
        $scope.$on('$routeChangeSuccess', function (e, current) {
            //$log(current);
            //$log.trace();
            //$logb('$routeChangeSuccess ' + (current && current['viewName']), () =>
            //{
            current && ViewPage.open({ name: current['viewName'], id: current.params['id'] });
            //});
        });

        app.path = function (value) {
            if (!value)
                return $location.path();
            $location.path(value);
            //);
        };

        app.openView = function (path) {
            return $logb('app.openView # ' + path.name, function () {
                $log(path);
                if (!path)
                    return null;

                var viewPage = ViewPage.open(path);
                if (viewPage) {
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
    App.ViewportCtrl = ViewportCtrl;

    var ViewPage = (function () {
        function ViewPage(key, viewName, id, params, active) {
            if (typeof id === "undefined") { id = null; }
            if (typeof params === "undefined") { params = {}; }
            if (typeof active === "undefined") { active = true; }
            this.key = key;
            this.viewName = viewName;
            this.id = id;
            this.params = params;
            this.active = active;
            this.closable = true;
            this.parts = {};
            this.tagName = viewName.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
        }
        ViewPage.prototype.title = function () {
            return this.key;
        };
        ViewPage.prototype.tooltip = function () {
            return this.title();
        };
        ViewPage.prototype.load = function () {
        };

        ViewPage.open = function (path) {
            //return $logb('ViewPage.open', () =>
            //{
            //$log('path: ', path);
            var viewPage = null;
            var parentViewPage = null;
            var part;
            var key = '';
            while (path) {
                if (path.id && path.id.Id)
                    path.id = path.id.Id;
                path.params = path.params || {};

                key += (key && '/') + path.name + '#' + (path.id || null);

                viewPage = ViewPage.allByKey[key];

                if (!viewPage) {
                    //$log(path.name, ': new page');
                    ViewPage.allByKey[key] = viewPage = new ViewPage(key, path.name, path.id, path.params);
                    if (!path.target) {
                        ViewPage.all.push(viewPage);
                        if (part)
                            part.viewPages.push(viewPage);
else
                            ViewPage.rootAll.push(viewPage);
                    } else {
                        if (part) {
                            path.target.close();
                            ViewPage.all.push(viewPage);
                            part.viewPages.push(viewPage);
                        } else {
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
                } else {
                    //$log(path.name, ': activate');
                    angular.extend(viewPage.params, path.params);

                    if (!viewPage.active)
                        viewPage.active = true;
                }

                viewPage.parent = parentViewPage;
                parentViewPage = viewPage;

                if (!path.part || !path.part.name || !path.part.path)
                    break;

                part = viewPage.parts[path.part.name] = { name: path.part.name, viewPages: [] };

                path = path.part.path;
            }

            //$log('ViewPage.all', ViewPage.all);
            //$log('ViewPage.rootAll', ViewPage.rootAll);
            return viewPage;
            //});
        };

        ViewPage.prototype.activate = function () {
            if (ViewPage.skipChangePath)
                ViewPage.skipChangePath = false;
else {
                //if (this.isActivated)
                app.path(this.viewName + (this.id ? '/' + this.id : ''));
                //else
                //	this.isActivated = true;
            }
            //});
        };

        ViewPage.prototype.close = function (removeFromRootAll) {
            if (typeof removeFromRootAll === "undefined") { removeFromRootAll = true; }
            var i = ViewPage.all.indexOf(this);
            if (i > -1)
                ViewPage.all.splice(i, 1);

            delete ViewPage.allByKey[this.key];

            if (removeFromRootAll) {
                i = ViewPage.rootAll.indexOf(this);
                if (i > -1) {
                    ViewPage.rootAll.splice(i, 1);
                    //ViewPage.all[i - 1].active = true;
                }
            }

            for (var partName in this.parts) {
                var part = this.parts[partName];
                for (var j = 0, page; page = part.viewPages[j++];) {
                    page.close();
                }
            }
        };

        ViewPage.prototype.loadParts = function () {
            if (!this.parts)
                return;

            for (var partName in this.parts) {
                var part = this.parts[partName];
                for (var i = 0, page; page = part.viewPages[i++];) {
                    page.load();
                }
            }
        };

        ViewPage.prototype.loadDependencies = function () {
            //$logb('loadDependencies', () =>
            //{
            var me = this;
            var parent = me.parent;
            if (parent) {
                parent.load();
                parent.loadDependencies();
            }

            if (me.dependencies) {
                for (var i = 0, dependency; dependency = me.dependencies[i++];) {
                    for (var j = 0, page; page = ViewPage.all[j++];) {
                        if (page.key.indexOf(dependency) < 0)
                            continue;
                        page.load();
                    }
                }
            }
            //});
        };

        ViewPage.all = [];
        ViewPage.rootAll = [];
        ViewPage.allByKey = {};
        ViewPage.skipChangePath = false;
        return ViewPage;
    })();
    App.ViewPage = ViewPage;

    app.views.directive('viewPane', function ($parse, $compile) {
        return {
            restrict: 'E',
            scope: { viewPage: '=' },
            link: function (scope, element, attrs) {
                $log('view-pane.scope: ', scope);
                element.html('<div><' + scope.viewPage.tagName + ' /></div>\n');
                $compile(element.contents())(scope);
            },
            replace: true
        };
    });
})(App || (App = {}));
//# sourceMappingURL=App.js.map
