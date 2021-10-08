module Luxena
{

	export interface FormControllerConfig extends SemanticControllerConfig
	{
		id?: any;
		args?: any[];

		members?: SemanticMembers<any>;

		onLoaded? (): void;

		viewMenuItems?: Array<any>;
		
		actions?: SemanticEntityAction[];

		///// <summary>Указывается селектор dxAccordion, если используется на форме</summary>
		//accordion?: string;
	}


	//export interface IFormController
	//{
	//	addMembers(container: JQuery|string, members: SemanticMember[]): void;
	//}


	export abstract class FormController<TConfig extends FormControllerConfig>
		extends SemanticController
	//implements IFormController
	{
		config: TConfig;
		params: any;
		//viewInfo: Object;
		id: Object;

		scope: any;

		data: Object;
		dataSourceConfig: DevExpress.data.DataSourceOptions;
		modelIsReady: JQueryDeferred<any>;
		modelIsLoading = ko.observable(false);
		loadingMessage = ko.observable(<string>undefined);

		containers: {
			[name: string]: {
				renderer: (containerEl: JQuery) => void;
			};
		} = {};

		widgets: { [memberName: string]: Object; } = {};


		title = ko.observable();

		details: SemanticController[] = [];

		defaultContainerId = "fields";


		constructor(cfg: TConfig)
		{
			super(cfg);

			if (!cfg.members)
				cfg.members = cfg.entity._members;

			this.params = cfg.args && cfg.args[0] || { id: cfg.id };
			this.id = this.params.id || null;
			//this.viewInfo = cfg.args[1];

			this.modelIsReady = $.Deferred();
		}


		getId() { return this.id; }


		addDetails(details: SemanticController)
		{
			this.details.push(details);
		}


		getScope(): any
		{
			const cfg = this.config;
			const se = cfg.entity;

			this.addComponents(cfg.members, this.defaultContainerId);
			this.createContainers();
			this.title(se._title || se._name);

			this.scope = {
				controller: this,

				r: this.model,
				icon: se._icon,
				title: this.title,
				containers: this.containers,

				widgets: this.widgets,

				deferRenderingOptions:
				{
					renderWhen: this.modelIsReady.promise(),
					showLoadIndicator: true,
					animation: "stagger-3d-drop",
					staggerItemSelector: ".dx-field, .dx-accordion-item",
				},

				loadingOptions:
				{
					visible: this.modelIsLoading,
					message: this.loadingMessage,
				},

				viewMenuItems: cfg.viewMenuItems || this.getMenuItems(),

				viewShown: () => this.viewShown(),
				viewHidden: () => this.viewHidden(),
			};

			return this.scope;
		}

		getRedirectUriToEntityType(entityTypeName: string)
		{
			return { view: entityTypeName, id: this.getId() };
		}

		viewShown()
		{
			super.viewShown();

			var cfg = this.config;
			var se = cfg.entity;

			se._lastId = this.getId();

			this.loadData();

			this.details.forEach(a => a.viewShown());
		}

		viewHidden()
		{
			super.viewHidden();
			this.details.forEach(a => a.viewHidden());
		}


		createContainers(): void
		{
			if (!this.modelIsExternal || !this.model["__isLoaded"])
			{
				this.fields.forEach(sf => sf.loadFromData(this.model, {}));
			}

			this.containerIds.forEach(containerId =>
			{
				this.containers[containerId] =
				{
					renderer: (containerEl: JQuery) =>
					{
						this.components.forEach(sc =>
						{
							if (sc._containerId === containerId)
								sc.render(containerEl);
						});
					},
				};
			});
		}

		loadData(onLoaded?: () => void)
		{
			var cfg = this.config;

			//this.loadingMessage("Загрузка...");
			//this.modelIsLoading(true);

			var dsConfig = this.dataSourceConfig = this.getDataSourceConfig();
			dsConfig.filter = [cfg.entity._referenceFields.id, "=", this.getId()];

			var ds = new DevExpress.data.DataSource(dsConfig);

			ds.load()
				.done(data => { this.loadFromData(data[0], true); onLoaded && onLoaded(); })
				.fail(() => this.loadFromData({}, true));
		}

		refresh()
		{
			this.loadData();
		}

		loadFromData(data?, resolveModel?: boolean): void
		{
			//$log("loadFromData", ko.unwrap2(this.model));

			this.data = data;

			if (data !== undefined)
			{
				this.components.forEach(sc => sc.loadFromData(this.model, data));
			}
			
			this.applyModel();

			if (resolveModel)
			{
				//this.modelIsLoading(false);
				this.modelIsReady.resolve();
			}
		}

		applyModel()
		{
			const cfg = this.config;
			let title = this.getEntityTitle(this.data);
			this.title(title);

			if (cfg.onLoaded)
				cfg.onLoaded();
		}

	}

}