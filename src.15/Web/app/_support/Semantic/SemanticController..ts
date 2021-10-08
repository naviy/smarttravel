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
		entityTitle?: SemanticTitle<any>;

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
		components: SemanticComponent<any>[] = [];
		fields: SemanticField[] = [];
		members: SemanticMember[] = [];
		containerIds: string[] = [];
		
		gridMode: boolean;
		viewMode: boolean;
		smartMode: boolean;
		editMode: boolean;
		filterMode: boolean;

		model: Object;
		modelIsExternal: boolean;
		scope: any;

		constructor(cfg: SemanticControllerConfig)
		{
			this.config = cfg;
			this.model = cfg.model || {};
			this.modelIsExternal = !!cfg.model;
			var se = cfg.entity;

			cfg.list = cfg.list === null ? null : cfg.list || se;
			cfg.form = cfg.form === null ? null : cfg.form || se;
			cfg.view = cfg.view === null ? null : cfg.view || cfg.form;
			cfg.smart = cfg.smart === null ? null : cfg.smart || (cfg.view === null ? null : cfg.view || cfg.form);
			cfg.edit = cfg.edit === null ? null : cfg.edit || cfg.form;

			//cfg.listAction = cfg.listAction !== undefined && cfg.entity.resolveListAction(cfg.viewAction) || cfg.list.resolveListAction();
			//cfg.viewAction = cfg.viewAction !== undefined && cfg.entity.resolveViewAction(cfg.viewAction) || cfg.view.resolveViewAction();
			//cfg.editAction = cfg.editAction !== undefined && cfg.entity.resolveEditAction(cfg.editAction) || cfg.edit.resolveEditAction();
		}

		abstract getMenuItems(): Array<any>;

		addComponent(item: SemanticObject<any>, containerId?: string, action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void): SemanticComponent<any>
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

				if (sm && this.members.indexOf(sm) >= 0) return null;

				sc = new SemanticField(sm);
			}
			else if (item instanceof SemanticEntityAction)
			{
				sc = new SemanticButton(item);
			}
			else if (item instanceof SemanticComponent)
			{
				sc = item;
				sm = sc instanceof SemanticField && sc.member;

				if (sm && this.members.indexOf(sm) >= 0) return null;

				sc = sc.clone();
			}

			if (!sc) return null;

			sc._controller = this;
			sc._containerId = containerId;
			this.components.push(sc);

			if (sm && this.members.indexOf(sm) < 0)
				this.members.push(sm);

			sc.addItemsToController(this, action);

			action && action(sm, sc);

			return sc;
		}


		addComponents(items: SemanticMembers<any>, containerId: string, action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void)
		{
			if (!items) return [];

			var components: SemanticComponent<any>[] = [];

			var item: any = items;

			if ($.isFunction(items))
				item = (<any>items)(this.config.entity);

			if (items instanceof SemanticEntity)
				item = items._members;

			if ($.isArray(item))
			{
				const list2 = <SemanticObject<any>[]>item;
				list2.forEach(a => a && components.push(this.addComponent(a, containerId, action)));
			}
			else if (item instanceof SemanticObject)
			{
				components.push(this.addComponent(item, containerId, action));
			}
			else
			{
				const items2 = items;

				for (let containerId2 in items2)
				{
					if (!items2.hasOwnProperty(containerId2)) continue;

					//$log(containerId2, ": ", items2[containerId2]);
					components.push.apply(components,
						this.addComponents(items2[containerId2], containerId2, action));
				}
			}

			return components;
		}


		getField(sm: SemanticMember)
		{
			return this.fields.filter(a => a.member === sm)[0];
		}

		modelValue(sm: SemanticMember, value?: any): any
		{
			var field = this.getField(sm);
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
			var cfg = this.config;

			var select: string[] = ["Id"];
			var expand: string[] = [];

			var usecalculated = false;

			var nameMember = cfg.entity._nameMember;

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
				var sm = sf.member;

				if (!sf._visible && !sm._selectRequired) return;

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

			var ds: DevExpress.data.DataSourceOptions = {
				store: cfg.entity._store,
				select: select,
				expand: expand,
			};

			return ds;
		}


		abstract getId();

		viewShown() { }
		viewHidden() { }


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


	export interface IEntityAction
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


	export function openAction(action: IEntityAction, id?: Object, actionOptions?: Object): void
	{
		if (!action || !action.uri) return;


		var uri = $.extend({ id: id }, action.uri);

		if (!id && action.defaults)
			uri = $.extend(uri, action.defaults);

		smartVisible(false);

		app.navigate(uri, actionOptions || action.options);
	}

}