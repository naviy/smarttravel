module Luxena
{

	export interface FormControllerConfig extends SemanticControllerConfig
	{
		id?: any;
		args?: any[];

		members?: SemanticMembers<any>;

		onLoaded? (): void;

		//viewMenuItems?: Array<any>;
		
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

		getScope(): any
		{
			const se = this._entity;

			this.title(se._title || se._name);

			this.scope = $.extend(super.getScope(), {

				r: this.model,

				deferRenderingOptions:
				{
					renderWhen: this.modelIsReady.promise(),
					showLoadIndicator: true,
					animation: "stagger-3d-drop",
					staggerItemSelector: ".dx-field",// ".card, .card-accordion, .dx-accordion-item",
				},

				loadingOptions:
				{
					delay: 500,
					message: this.loadingMessage,
					visible: this.modelIsLoading,
				},
			});

			return this.scope;
		}


		//protected addMembers()
		//{
		//	this.addComponents(this.config.members, null, "fields");
		//}


		protected addMembers()
		{
			this.addComponents(this.config.members, null, "fields");
			super.addMembers();
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

		
		loadData(onLoaded?: () => void)
		{
			const cfg = this.config; 
			//this.loadingMessage("Загрузка...");
			//this.modelIsLoading(true);

			const dsConfig = this.dataSourceConfig = this.getDataSourceConfig();
			dsConfig.filter = [cfg.entity._lookupFields.id, "=", this.getId()];

			//$log(dsConfig);

			const ds = new DevExpress.data.DataSource(dsConfig);

			ds.load()
				.done(data => { this.loadFromData(data[0], true); onLoaded && onLoaded(); })
				.fail(() => this.loadFromData({}, true));
		}

		refresh()
		{
			this.loadData();
			this.details.forEach(a => a.refresh());
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