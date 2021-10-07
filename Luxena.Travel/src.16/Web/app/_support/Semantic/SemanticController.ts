module Luxena
{

	//export type SemanticComponentOrMember = SemanticComponent<any> | SemanticMember;
	//export type SemanticEntityTitle = SemanticObject<any> | string | ((data: any) => string);

	//export type SemanticComponentOrMemberList = any;
	//SemanticEntity |
	//SemanticComponentOrMember[] |
	//((se: SemanticEntity) => SemanticComponentOrMember[]) |
	//{ [containerId: string]: SemanticComponentOrMember[] }
	//((se: SemanticEntity) => { [containerId: string]: SemanticComponentOrMember[] })
	//;


	export interface SemanticControllerConfig
	{
		entity?: SemanticEntity;
		model?: Object;
		entityTitle?: SemanticTitle;

		list?: SemanticEntity;
		form?: SemanticEntity;
		view?: SemanticEntity;
		smart?: SemanticEntity;
		edit?: SemanticEntity;

		//listAction?: IEntityAction;
		//viewAction?: IEntityAction;
		//editAction?: IEntityAction;
	}


	export abstract class SemanticController
	{
		config: SemanticControllerConfig;
		_entity: SemanticEntity;
		components: SemanticComponent<any>[] = [];
		fields: Field[] = [];
		members: SemanticMember[] = [];
		containerIds: string[] = [];
		defaults;

		chartMode: boolean;
		gridMode: boolean;
		viewMode: boolean;
		smartMode: boolean;
		editMode: boolean;
		filterMode: boolean;

		iconHtml = ko.observable<string>();
		title = ko.observable<string>();

		model: Object;
		modelIsExternal: boolean;
		scope: any;

		containers: {
			[name: string]: {
				renderer: (containerEl: JQuery) => void;
			};
		} = {};

		details: SemanticController[] = [];

		widgets: { [uname: string]: any; } = {};


		constructor(cfg: SemanticControllerConfig)
		{
			this.config = cfg;
			const se = this._entity = cfg.entity;
			this.model = cfg.model || {};
			this.modelIsExternal = !!cfg.model;

			cfg.list = cfg.list === null ? null : cfg.list || se;
			cfg.form = cfg.form === null ? null : cfg.form || se;
			cfg.view = cfg.view === null ? null : cfg.view || cfg.form;
			cfg.smart = cfg.smart === null ? null : cfg.smart || (cfg.view === null ? null : cfg.view || cfg.form);
			cfg.edit = cfg.edit === null ? null : cfg.edit || cfg.form;

			//cfg.listAction = cfg.listAction !== undefined && cfg.entity.resolveListAction(cfg.viewAction) || cfg.list.resolveListAction();
			//cfg.viewAction = cfg.viewAction !== undefined && cfg.entity.resolveViewAction(cfg.viewAction) || cfg.view.resolveViewAction();
			//cfg.editAction = cfg.editAction !== undefined && cfg.entity.resolveEditAction(cfg.editAction) || cfg.edit.resolveEditAction();
		}


		addComponent(
			item: SemanticObject<any> | string,
			parent: SemanticComponent<any>,
			containerId?: string,
			action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void,
			addedComponents?: SemanticComponent<any>[]
		): SemanticComponent<any>
		{
			if (!item)
			{
				const cfg = <any>this.config;
				$error("entity: ", cfg.entity && cfg.entity._name || cfg.entity, ", members: ", cfg.members, ", config: ", cfg);
				throw Error("Попытка добавить несуществующий SemanticComponent в SemanticController (containerId: " + containerId + ")");
			}
			let sc: SemanticComponent<any> = null;
			let sm: SemanticMember = null;

			if (containerId && this.containerIds.indexOf(containerId) < 0)
				this.containerIds.push(containerId);

			//$log(item);
			//$log(item["name"] || item["_name"]);

			if (item instanceof SemanticMember)
			{
				sm = item;

				//if (sm && this.members.indexOf(sm) >= 0) return null;

				sc = sm.field();
			}
			else if (typeof item === "string")
			{
				sc = new Components.Html(item);
			}
			else if (item instanceof SemanticEntityAction)
			{
				sc = new Components.Button(item);
			}
			else if (item instanceof SemanticComponent)
			{
				sc = item;
				sm = sc instanceof Field && sc._member;
				
				//if (sm && this.members.indexOf(sm) >= 0) return null;

				//sc = sc.clone();
			}

			if (!sc) return null;

			sc._controller = this;
			sc._entity = this._entity;
			sc._containerId = containerId;
			sc._parent = parent;
			this.components.push(sc);

			addedComponents && addedComponents.push(sc);

			//if (sm && this.members.indexOf(sm) < 0 && isField(sc))
			if (sm && isField(sc))
			{
				sc._type.addItemsToController(sc, this, action);
				this.members.push(sm);
			}


			sc.addItemsToController(action);

			action && action(sm, sc);

			return sc;
		}


		addComponents(
			items: SemanticMembers<any>,
			parent: SemanticComponent<any>,
			containerId: string,
			action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void,
			addedComponents?: SemanticComponent<any>[]
		)
		{
			if (!items) return [];

			var components: SemanticComponent<any>[] = [];

			let item: any = items;

			if ($.isArray(item) && item.length === 1)
				item = item[0];

			if ($.isFunction(item))
				item = item(this.config.entity);

			if (item instanceof SemanticEntity)
				item = item._members;

			if ($.isArray(item))
			{
				const list2 = <SemanticObject<any>[]>item;
				list2.forEach(a => a && this.addComponent(a, parent, containerId, action, components));
			}
			else if (item instanceof SemanticObject)
			{
				this.addComponent(item, parent, containerId, action, components);
			}
			else
			{
				const items2 = item;

				for (let containerId2 in items2)
				{
					if (!items2.hasOwnProperty(containerId2)) continue;

					this.addComponents(items2[containerId2], parent, containerId2, action, components);
				}
			}

			return components;
		}

		addDetails(details: SemanticController)
		{
			this.details.push(details);
		}


		getFieldByMember(sm: SemanticMember)
		{
			return this.fields.filter(a => a._member === sm)[0];
		}

		modelValue(sm: SemanticMember, value?: any): any
		{
			const field = this.getFieldByMember(sm);
			if (!field) return undefined;

			if (value !== undefined)
			{
				field.setModelValue(value);
				return value;
			}
			else
				return field.getModelValue();
		}


		getDataSourceConfig(): DevExpress.data.DataSourceOptions
		{
			const cfg = this.config;

			var select: string[] = ["Id"];
			var expand: string[] = [];

			var usecalculated = false;

			let nameMember = cfg.entity._nameMember;

			if ((<any>cfg.entityTitle) instanceof SemanticMember)
				nameMember = <SemanticMember>cfg.entityTitle;

			if (nameMember && select.indexOf(nameMember._name) < 0)
			{
				select.push(nameMember._name);
				if (nameMember._isCalculated)
					usecalculated = true;
			}

			this.fields.forEach(sf =>
			{
				var sm = sf._member;

				if (!sf._visible && !sf._selectRequired && !sm._selectRequired) return;

				var fields = sf.getSelectFieldNames();
				fields.forEach(a =>
				{
					if (select.indexOf(a) < 0)
						select.push(a);
				});

				var expandFields = sf.getExpandFieldNames();
				expandFields.forEach(a =>
				{
					if (expand.indexOf(a) < 0)
						expand.push(a);
				});

				if (sm._isCalculated)
					usecalculated = true;
			});

			if (usecalculated)
				select.push("$usecalculated");

			return <DevExpress.data.DataSourceOptions>{
				store: cfg.entity._store,
				select: select,
				expand: expand,
			};
		}


		abstract getId();

		//getEntityActionParams()
		//{
		//	return <EntityActionParams>{
		//		id: this.getId(),
		//		_controller: this,
		//		_entity: this.config.entity,
		//	};

		//	//const ectrl = ctrl instanceof EditFormController ? ctrl : null;

		//	//const prms: EntityActionParams = {
		//	//	id: ctrl.getId(),
		//	//	_controller: ctrl,
		//	//	_resync: ctrl.editMode || ctrl.viewMode,
		//	//	_save: ctrl.viewMode,
		//	//	_select: <any>ctrl.dataSourceConfig.select,
		//	//	_delta: !ectrl ? undefined : () => ectrl.saveToData(),
		//	//};

		//	//if (ctrl.viewMode)
		//	//	prms._expand = <any>ctrl.dataSourceConfig.expand;

		//	//if (prms._resync)
		//	//	prms._onExecuteDone = data => ctrl.loadFromData(data);
		//}


		getScope(): any
		{
			const se = this._entity;

			this.iconHtml(se._textIconHtml);

			this.addMembers();
			this.createContainers();

			return this.scope = {
				controller: this,
				containers: this.containers,
				widgets: this.widgets,
				viewShown: () => this.viewShown(),
				viewHidden: () => this.viewHidden(),
			};
		}

		protected addMembers()
		{
			this.addComponents(this.getToolbarItems(), null, "toolbarItems");
			const toolbarItems = this.components.filter(sc => sc._containerId === "toolbarItems");
			this.addComponent(sd.toolbar(<any>toolbarItems), null, "toolbar");
		}

		protected createContainers(): void
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

		getToolbarItems(): Components.Buttons
		{
			return [
				new Components.Button().back().left(),
				new Components.Button().template((data, index, container) =>
				{
					var h = $(`<h1>${ko.unwrap(this.iconHtml) }<b data-bind="text: title"></b></h1>`).appendTo(container);
					ko.applyBindings({ title: this.title }, h[0]);
				})
			];
		}

		viewShown() { }
		viewHidden() { }

		refresh() { }

		getEntityTitle(data): string
		{
			const cfg = this.config;
			const entityTitle = <any>cfg.entityTitle;
			const se = cfg.entity;

			if (!entityTitle)
				return se.getTitle(data) || se._title;

			if ($.isFunction(entityTitle))
				return entityTitle(data) || se._title;

			if (entityTitle instanceof SemanticMember)
			{
				const title = ko.unwrap(data[entityTitle._name]);
				return title ? se._title + " " + title : se._title;
			}

			return entityTitle + "";
		}

	}

	export interface EntityUri
	{
		uri: {
			view: string;
			id?: any;
		};

		defaults?: Object;

		options?: {
			root?: boolean;
			target?: string;
			direction?: string;
		};
	}

	export function openEntityUri(action: EntityUri, id?: Object, actionOptions?: Object): void
	{
		if (!action || !action.uri) return;

		let uri = $.extend({ id: id }, action.uri);

		if (!id && action.defaults)
			uri = $.extend(uri, action.defaults);

		smartVisible(false);

		app.navigate(uri, actionOptions || action.options);
	}

}