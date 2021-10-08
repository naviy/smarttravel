/// <reference path='../../Scripts/typings/angularjs/angular.d.ts' />
/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />
var Controls;
(function (Controls) {
    var FormSection = (function () {
        function FormSection(element, title, description) {
            this.element = element;
            this.title = title;
            this.description = description;
        }
        FormSection.prototype.activate = function (sections) {
            for (var i = 0, section; section = sections[i++];) {
                if (section == this)
                    section.menuElement && section.menuElement.addClass('active');
else
                    section.menuElement && section.menuElement.removeClass('active');
            }
        };

        FormSection.prototype.open = function () {
            var totalScroll = this.element.position().top;
            var container = this.element.closest('.editform-container');
            container.animate({ scrollTop: totalScroll }, 400);
        };
        return FormSection;
    })();
    Controls.FormSection = FormSection;

    function EditFormCtrl(cfg) {
        return function ($scope) {
            return ConfigureEditForm($scope, cfg);
        };
    }
    Controls.EditFormCtrl = EditFormCtrl;

    function ConfigureEditForm($scope, cfg) {
        $scope.app = app;
        $scope.db = Domain;

        ConfigureEditFormService($scope, cfg);
        ConfigureEditFormSave($scope, cfg);
        ConfigureEditFormValidation($scope, cfg);
        ConfigureEditFormSections($scope, cfg);
        ConfigureEditFormParts($scope, cfg);
    }
    Controls.ConfigureEditForm = ConfigureEditForm;

    function ConfigureEditFormService($scope, cfg) {
        $scope.id = $scope.viewPage.id;
        $scope.r = null;
        $scope.viewPage.dependencies = cfg.dependencies;

        if (cfg.title)
            $scope.viewPage.title = function () {
                return cfg.title($scope, $scope.r);
            };
else if (cfg.title1 && cfg.service.title)
            $scope.viewPage.title = function () {
                return cfg.service.title($scope, $scope.r, cfg.title1);
            };
else if (cfg.service.title)
            $scope.viewPage.title = function () {
                return cfg.service.title($scope, $scope.r);
            };

        $scope.needReload = false;
        $scope.loading = false;

        $scope.load = function () {
            return cfg.service.loadEdit($scope, cfg).success(function () {
                return $scope.viewPage.loadParts();
            });
        };

        $scope.refresh = function () {
            return $scope.needReload = true;
        };

        $scope.$watch('needReload', function () {
            if ($scope.needReload)
                $scope.load();
        });

        $scope.load();

        $scope.openView = function () {
            return cfg.service.openView({ scope: $scope, r: $scope.r, target: $scope.viewPage });
        };
    }

    function ConfigureEditFormSave($scope, cfg) {
        $scope.saving = false;
        $scope.save = function () {
            return cfg.service.saveEdit($scope, cfg);
        };
    }

    function ConfigureEditFormValidation($scope, cfg) {
        $scope.fieldErrorAsText = function ($error) {
            var s = [];

            if ($error.email)
                s.push('<b>E-mail address is invalid.</b>');
            if ($error.number)
                s.push('<b>Number has invalid format.</b>');
            if ($error.required)
                s.push('<b>The field is required.</b>');
            if ($error.url)
                s.push('<b>Web address is invalid.</b>');

            if (s.length)
                return s.join('<br/><br/>');
            return null;
        };
    }

    function ConfigureEditFormSections($scope, cfg) {
        $scope.sections = [];

        $scope.addSection = function (section) {
            section.index = $scope.sections.length;
            $scope.sections.push(section);
            return section;
        };
    }

    function ConfigureEditFormParts($scope, cfg) {
        if (!cfg.parts)
            return;

        for (var partName in cfg.parts) {
            var part = cfg.parts[partName];

            //$log(part);
            app.openView({
                name: $scope.viewPage.viewName,
                id: $scope.viewPage.id,
                part: { name: partName, path: part }
            });
        }
    }

    app.views.directive('formSection', function () {
        return {
            restrict: 'E',
            transclude: true,
            replace: true,
            scope: {},
            link: function (scope, element, attrs) {
                scope.section = scope.$parent.addSection(new FormSection(element, attrs.title, attrs.description));
            },
            template: '<div style="padding-top: 15px">' + '<div class="panel-body" ng-transclude></div>' + '<div style="border-bottom: 1px solid #dddddd"></div>' + '</div>'
        };
    });

    app.views.directive('sectionScrollSpy', function () {
        return {
            restrict: 'A',
            link: function (scope, elem, attrs) {
                $(elem).scroll(function () {
                    for (var i = scope.sections.length, section; section = scope.sections[--i];) {
                        if (section.element.offset().top <= section.menuElement.offset().top) {
                            section.activate(scope.sections);
                            break;
                        }
                    }
                });
            }
        };
    });

    app.views.directive('sectionSpy', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var section = scope.$eval(attrs.sectionSpy);

                if (section) {
                    section.menuElement = element;

                    if (section.index == 0)
                        element.addClass('active');

                    element.click(function () {
                        return section.open();
                    });
                }
            }
        };
    });
})(Controls || (Controls = {}));
//# sourceMappingURL=EditFormCtrl.js.map
