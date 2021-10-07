/// <reference path='../../Scripts/typings/angularjs/angular.d.ts' />
/// <reference path='../App.ts' />
/// <reference path='../Domain.ts' />


module Controls
{

	export interface IEditFormScope extends ng.IScope, Domain.IEntityForm
	{
		app: IAppModule;
		db: any;
		id: any;
		r: any;

		load: () => void;
		refresh: () => void;
		loading: boolean;
		needReload: boolean;

		save: () => void;
		saving: boolean;

		sections: FormSection[];
		addSection: (FormSection) => FormSection;
		//openSection: (FormSection) => void;

		fieldErrorAsText: (any) => string;

		openView: () => void;
	}

	export interface IEditFormConfig extends Domain.IEntityFormConfig
	{
		parts?: { [id: string]: App.IViewPath; };
		dependencies?: string[];
	}

	export class FormSection
	{
		constructor(
			public element: any,
			public title: string,
			public description)
		{
		}

		index: number;
		menuElement: any;

		activate(sections: FormSection[])
		{
			for (var i = 0, section; section = sections[i++];)
			{
				if (section == this)
					section.menuElement && section.menuElement.addClass('active');
				else
					section.menuElement && section.menuElement.removeClass('active');
			}
		}

		open()
		{
			var totalScroll = this.element.position().top;
			var container = this.element.closest('.editform-container');
			container.animate({ scrollTop: totalScroll }, 400);
		}
	}

	export function EditFormCtrl(cfg: IEditFormConfig)
	{
		return ($scope: IEditFormScope) => ConfigureEditForm($scope, cfg);
	}

	export function ConfigureEditForm($scope: IEditFormScope, cfg: IEditFormConfig)
	{
		$scope.app = app;
		$scope.db = Domain;
		
		ConfigureEditFormService($scope, cfg);
		ConfigureEditFormSave($scope, cfg);
		ConfigureEditFormValidation($scope, cfg);
		ConfigureEditFormSections($scope, cfg);
		ConfigureEditFormParts($scope, cfg);
	}

	function ConfigureEditFormService($scope: IEditFormScope, cfg: IEditFormConfig)
	{
		$scope.id = $scope.viewPage.id;
		$scope.r = null;
		$scope.viewPage.dependencies = cfg.dependencies;

		if (cfg.title)
			$scope.viewPage.title = () => cfg.title($scope, $scope.r);
		else if (cfg.title1 && cfg.service.title)
			$scope.viewPage.title = () => cfg.service.title($scope, $scope.r, cfg.title1);
		else if (cfg.service.title)
			$scope.viewPage.title = () => cfg.service.title($scope, $scope.r);

		$scope.needReload = false;
		$scope.loading = false;

		$scope.load = () => cfg.service.loadEdit($scope, cfg).success(() => $scope.viewPage.loadParts());

		$scope.refresh = () => $scope.needReload = true;

		$scope.$watch('needReload', () => 
		{
			if ($scope.needReload)
				$scope.load();
		});

		$scope.load();

		$scope.openView = () => cfg.service.openView({ scope: $scope, r: $scope.r, target: $scope.viewPage });
	}

	function ConfigureEditFormSave($scope: IEditFormScope, cfg: IEditFormConfig)
	{
		$scope.saving = false;
		$scope.save = () => cfg.service.saveEdit($scope, cfg);
	}

	function ConfigureEditFormValidation($scope: IEditFormScope, cfg: IEditFormConfig)
	{
		$scope.fieldErrorAsText = $error =>
		{
			var s: string[] = [];

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

	function ConfigureEditFormSections($scope: IEditFormScope, cfg: IEditFormConfig)
	{
		$scope.sections = [];

		$scope.addSection = (section: FormSection) => 
		{
			section.index = $scope.sections.length;
			$scope.sections.push(section);
			return section;
		};
	}

	function ConfigureEditFormParts($scope: IEditFormScope, cfg: IEditFormConfig)
	{
		if (!cfg.parts) return;

		for (var partName in cfg.parts)
		{
			var part = cfg.parts[partName];
			//$log(part);
			app.openView({
				name: $scope.viewPage.viewName,
				id: $scope.viewPage.id,
				part: { name: partName, path: part }
			});
		}
	}
	


	app.views.directive('formSection', function ()
	{
		return {
			restrict: 'E',
			transclude: true,
			replace: true,
			scope: {},

			link: function (scope, element, attrs)
			{
				scope.section = scope.$parent.addSection(new FormSection(element, attrs.title, attrs.description));
			},

			template:
			'<div style="padding-top: 15px">' +
			//'<div class="panel-heading">' +
			//	'<h3 class="panel-title">{{section.title}}</h3 > ' +
			//'</div> ' +
			'<div class="panel-body" ng-transclude></div>' +
			'<div style="border-bottom: 1px solid #dddddd"></div>' +
			'</div>'
		};
	});


	app.views.directive('sectionScrollSpy', function ()
	{
		return {
			restrict: 'A',

			link: function (scope: IEditFormScope, elem, attrs)
			{
				$(elem).scroll(function ()
				{
					for (var i = scope.sections.length, section: FormSection; section = scope.sections[--i];)
					{
						if (section.element.offset().top <= section.menuElement.offset().top)
						{
							section.activate(scope.sections);
							break;
						}
					}
				});
			}
		};
	});


	app.views.directive('sectionSpy', function ()
	{
		return {
			restrict: 'A',

			link: function (scope: IEditFormScope, element, attrs)
			{
				var section = scope.$eval(attrs.sectionSpy);

				if (section)
				{
					section.menuElement = element;

					if (section.index == 0)
						element.addClass('active');

					element.click(() => section.open());
				}
			}
		};
	});


}