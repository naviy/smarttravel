module Luxena
{

	export interface CollectionControllerConfig extends SemanticControllerConfig
	{
		members?: SemanticMembers<any>;
		master?: FormController<FormControllerConfig>;
		defaults?: Array<[SemanticMember, any]>;

		title?: string;

		filter?: any;
		//KnockoutObservable<any> |
		//Array<[string, string, any] | string> |
		//((ctrl: CollectionController<any>) => Array<[string, string, any] | string>);

		fixed?: boolean;
	}

	export interface ISemanticRowData
	{
		_smartEntity: SemanticEntity;
		_viewEntity: SemanticEntity;
		_editEntity: SemanticEntity;
		[name: string]: any;
	}

	export abstract class CollectionController<TConfig extends CollectionControllerConfig>
		extends SemanticController
	{
		config: TConfig;
		scope: any;

		constructor(cfg: ChartControllerConfig)
		{
			this.chartMode = true;

			if (!cfg.members)
				cfg.members = cfg.entity._members;

			if (cfg.defaults && cfg.defaults.length)
			{
				var defaults = {};
				cfg.defaults.forEach(a =>
				{
					var sm = a[0];
					var model = {};
					model[sm._name] = a[1];

					sm.field().saveToData(model, defaults);
				});

				this.defaults = defaults;
			}

			super(cfg);

			if (cfg.master)
				cfg.master.addDetails(this);
		}

		getScope(): any
		{
			const se = this._entity;
			this.title(se._titles || se._title || se._names || se._name);

			return super.getScope();
		}

		masterId()
		{
			const master = this.config.master;
			return master && master.getId();
		}

		prepareFilter(filter: any[])
		{
			if (!filter) return filter;

			for (var i = 0, len = filter.length; i < len; i++)
			{
				const f = filter[i];
				if (f instanceof SemanticMember)
					filter[i] = f.getFilterExpr(null)[0];
				else if ($.isArray(f))
					this.prepareFilter(f);
			}

			return filter;
		}

		getDataSourceConfig()
		{
			const options = super.getDataSourceConfig();
			const cfg = this.config;
			const se = this._entity;

			if (cfg.filter)
			{
				let filter = <any>cfg.filter;

				if (!ko.isObservable(filter))
				{
					if ($.isFunction(filter))
						filter = filter(this);
					filter = this.prepareFilter(filter);
				}

				options.filter = filter;
				//$log(options.filter);
			}
			else if (cfg.defaults)
			{
				if (se._isDomainFunction)
				{
					const params = options["customQueryParams"] = {};
					cfg.defaults.forEach(def =>
						params[def[0].getFilterExpr(null)[0]] = def[1]
					);
				}
				else
				{
					const filter = cfg.defaults.map(a => a[0].getFilterExpr(a[1]));
					if (filter.length)
						options.filter = filter;
				}
			}
			else if (cfg.master instanceof FilterFormController)
			{
				const ofilter = (<FilterFormController>cfg.master).filter;
				options.filter = ko.unwrap(ofilter);
			}

			options.map = data => this.dataMap(data);

			if (cfg.entity._isQueryResult || cfg.fixed)
			{
				delete options.expand;
				delete options.select;
			}

			return options;
		}


		dataMap(data: ISemanticRowData)
		{
			const cfg = this.config;
			const se = cfg.entity._isAbstract && sd.entityByOData(data) || cfg.entity;

			data._viewEntity = cfg.view === cfg.entity ? se : cfg.view;
			data._smartEntity = cfg.smart === cfg.entity ? se : cfg.smart;
			data._editEntity = cfg.edit === cfg.entity ? se : cfg.edit;

			return data;
		}

	}

}