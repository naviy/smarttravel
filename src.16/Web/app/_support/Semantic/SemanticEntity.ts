module Luxena
{

	import Button = Components.Button;


	export class SemanticEntity extends SemanticObject<SemanticEntity>
	{
		_className: string;
		_isAbstract: boolean;

		_store: DevExpress.data.ODataStore;
		_isDomainFunction: boolean;
		_lookupStore: DevExpress.data.ODataStore;
		_saveStore: DevExpress.data.ODataStore;
		_lookupFields: { id: string; name: string; } = { id: "Id", name: "Name" };

		_nameMember: SemanticMember;

		_members: SemanticMember[] = [];
		//_actions: SemanticEntityAction[] = [];

		_getDerivedEntities: () => SemanticEntity[];
		_getRootEntity: () => SemanticEntity;
		_getBaseEntity: () => SemanticEntity;

		_listUri: EntityUri;
		_viewUri: EntityUri;
		_smartUri: EntityUri;
		_editUri: EntityUri;

		_lookupItemTemplate: (r, index: Number, container: JQuery) => void | string;


		_isBig: boolean;
		_isSmall: boolean;
		_isWide: boolean;

		_isEntity: boolean;
		_isEntityQuery: boolean;
		_isQueryParams: boolean;
		_isQueryResult: boolean;
		_isDomainAction: boolean;

		/** Сохраняется контроллерами id последнего просмотриваемого/редактируемого entity */
		_lastId: any;

		_textIconHtml = "";

		_titleMenuItems: MenuItems;

		_entityStatusGetter: (r) => string;

		//#region Actions

		listAction = this.action()
			.onExecute((btn: Button) => btn._entity.openList());

		backToListAction = this.action()
			.title("Вернуться к списку")
			.icon("list")
			.onExecute((btn: Button) => btn._entity.openList());

		viewAction = this.action()
			.title("Подробнее")
			.icon("fa fa-search")
			.default()
			.onExecute((btn: Button) => btn._entity.openView(btn.getId()));

		viewSingletonAction = this.action()
			.default()
			.onExecute((btn: Button) => btn._entity.openView("single"));

		newAction = this.action()
			.title("Добавить")
			.icon("fa fa-plus")
			.success()
			.onExecute((btn: Button) => btn._entity.openNew(btn._controller.defaults));

		editAction = this.action()
			.title("Изменить")
			.icon("fa fa-pencil")
			.success()
			.onExecute((btn: Button) => btn._entity.openEdit(btn.getId()));

		editSingletonAction = this.action()
			.success()
			.onExecute((btn: Button) => btn._entity.openEdit("single"));

		deleteAction = this.action()
			.title("Удалить")
			.icon("fa fa-trash")
			.danger()
			.onExecute((btn: Button) => btn._entity.delete(btn.getId()));

		refreshAction = this.action()
			.title("Обновить")
			.icon("refresh")
			.onExecute((btn: Button) => btn._controller.refresh());

		//#endregion


		init(): void
		{
			this.initMembers();

			this.listAction.iconAndTitle(this._icon, this._titles || this._title || this._names || this._name);
			this.viewSingletonAction.iconAndTitle(this._icon, this._title || this._name);
			this.editSingletonAction.iconAndTitle(this._icon, this._title || this._name);
		}

		initMembers(): void
		{
			const entityPositions: SemanticMember[] = [];
			const entityNames: SemanticMember[] = [];
			const entityDates: SemanticMember[] = [];

			for (let name in this)
			{
				if (!this.hasOwnProperty(name) || (<string>name).indexOf("_") === 0) continue;

				const sm = this[name];

				if (sm instanceof SemanticObject)
				{
					sm._entity = this;
					sm._name = name;

					if (sm instanceof SemanticMember)
					{
						sm._title = sm._title || sm._name;

						if (sm._isEntityPosition)
							entityPositions.push(sm);
						if (sm._isEntityName)
							entityNames.push(sm);
						else if (sm._isEntityDate)
							entityDates.push(sm);
					}
				}
			}

			this._nameMember = this._nameMember || this[this._lookupFields.name];

			if (entityPositions.length)
			{
				entityPositions.forEach(a => a._sortOrder = "asc");
			}
			else if (entityDates.length)
			{
				entityDates.forEach(a => a._sortOrder = "desc");
				entityNames.forEach(a => a._sortOrder = "desc");
			}
			else
			{
				entityNames.forEach(a => a._sortOrder = "asc");
			}
		}


		titleMenuItems(titleMenuItems: MenuItems): SemanticEntity
		{
			this._titleMenuItems = titleMenuItems;
			return this;
		}

		getTitleMenuItems()
		{
			let items = this._titleMenuItems;
			let baseEntity: SemanticEntity = this;

			while (!items)
			{
				if (!baseEntity._getBaseEntity) break;

				baseEntity = baseEntity._getBaseEntity();
				if (!baseEntity) break;

				items = baseEntity._titleMenuItems;
			}

			if (items)
			{
				const i = items.indexOf(this);
				if (i >= 0)
				{
					items = items.slice(0);
					items.splice(i, 1);
				}
			}

			return items;
		}


		member0(): SemanticMember
		{
			const m = new SemanticMember();
			m._entity = this;
			return m;
		}

		member(original?: SemanticMember): SemanticMember
		{
			const m = original ? original.clone() : new SemanticMember();
			m._entity = this;

			this._members.push(m);

			return m;
		}

		collection<TEntity extends SemanticEntity>(
			collectionItemEntity: () => TEntity,
			collectionItemMasterMember?: SemanticMember | ((se: TEntity) => SemanticMember),
			setter?: (sm: SemanticMember) => void)
		{
			const m = this.member0();
			m._type = FieldTypes.Grid.Grid;
			m._collectionItemEntity = collectionItemEntity;


			if (collectionItemMasterMember instanceof SemanticMember)
			{
				var collectionItemMasterMember_ = collectionItemMasterMember;
				collectionItemMasterMember = <any>(() => collectionItemMasterMember_);
			}

			m._collectionItemMasterMember = <any>collectionItemMasterMember;

			setter && setter(m);

			return m;
		}

		action()
		{
			return new SemanticEntityAction(this);
		}


		//#region Setters

		icon(value: string): SemanticEntity
		{
			this._icon = value;
			this._textIconHtml = getTextIconHtml(value);
			return this;
		}

		entityStatus(value: (r) => string)
		{
			this._entityStatusGetter = value;
			return this;
		}
		

		big(value?: boolean): SemanticEntity
		{
			this._isBig = value !== false;
			return this;
		}

		small(value?: boolean): SemanticEntity
		{
			this._isSmall = value !== false;
			return this;
		}

		wide(value?: boolean): SemanticEntity
		{
			this._isWide = value !== false;
			return this;
		}


		///#endregion


		getTitle(data: any): string
		{
			if (!data) return undefined;

			var title = ko.unwrap(data[this._lookupFields.name]);

			return title ? this._title + " " + title : this._title;
		}


		//#region Navigations

		listView = () => this._names;
		viewView = () => this._name;
		editView = () => this._name + "Edit";

		resolveListUri(action?: SemanticEntity | EntityUri)
		{
			if (action === null)
				return null;

			if (action instanceof SemanticEntity)
				return action.resolveListUri();

			if (action === undefined)
				action = this._listUri || { uri: { view: this.listView() } };

			return action;
		}

		resolveViewUri(action?: SemanticEntity | EntityUri, formEntity?: SemanticEntity)
		{
			if (action === null)
				return null;

			if (action instanceof SemanticEntity)
				return action.resolveViewUri();

			if (action === undefined)
			{
				formEntity = formEntity || this;
				action = formEntity._viewUri || { uri: { view: formEntity.viewView() } };
			}

			return action;
		}

		resolveEditUri(action?: SemanticEntity | EntityUri, formEntity?: SemanticEntity)
		{
			if (action === null)
				return null;

			if (action instanceof SemanticEntity)
				return action.resolveEditUri();

			if (action === undefined)
			{
				formEntity = formEntity || this;
				action = formEntity._editUri || { uri: { view: formEntity.editView() } };
			}

			return action;
		}


		openList()
		{
			openEntityUri(this.resolveListUri());
		}

		openView(id)
		{
			openEntityUri(this.resolveViewUri(), id);
		}

		openNew(defaults?)
		{
			const action = this.resolveEditUri();
			action.defaults = defaults;
			openEntityUri(action);
		}

		openEdit(id)
		{
			openEntityUri(this.resolveEditUri(), id);
		}

		showSmart: (target, cfg: SmartFormControllerConfig) => void;

		toggleSmart(target, cfg: SmartFormControllerConfig)
		{
			if (!this.showSmart) return;

			if (smartVisible() && smartTarget() === target)
				smartVisible(false);
			else
			{
				this.showSmart(target, cfg);
			}
		}

		//#endregion


		save(id: any, data: Object)
		{
			var store = this._saveStore || this._store;

			if (id)
				return store.update(id, data);
			else
				return store.insert(data).done((e, newId) =>
				{
					this._lastId = newId;
				});
		}

		loadDefaults(data: any, select?: string[])
		{
			data = $.extend({ Version: -1 }, data);

			const store = this._store;

			let url = store["_byKeyUrl"]("");
			if (select && select.length)
				url += "?$select=" + select.join(",").replace(",$usecalculated", "");

			return $.when(store["_sendRequest"](url, "PUT", null, data));
		}

		recalc(prms: ISemanticEntityRecalc)
		{
			const store = this._saveStore || this._store;
			const select = prms.select;
			const data = $.extend({ Version: -1 }, prms.data);
			if (prms.propertyName)
				data.LastChangedPropertyName = prms.propertyName;

			let url = store["_byKeyUrl"](prms.id);
			if (select && select.length)
				url += "?$select=" + select.join(",").replace(",$usecalculated", "");

			var d = $.Deferred();

			$.when(store["_sendRequest"](url, "PATCH", null, data))
				.done(newData => d.resolve(prms.id, newData))
				.fail(d.reject, <any>d);

			return d.promise();
		}

		delete(id: any)
		{
			var store = this._saveStore || this._store;
			return store.remove(id);
		}

		//toTab()
		//{
		//	return {
		//		icon: "fa fa-" + this._icon,
		//		title: this._title || this._name,
		//		template: this._name,
		//	}
		//}

		//toTabs()
		//{
		//	return {
		//		icon: "fa fa-" + this._icon,
		//		title: this._titles || this._title || this._names || this._name,
		//		template: this._names || this._name,
		//	}
		//}

		applyToThisAndDerived(action: (se: SemanticEntity) => void)
		{
			if (!action) return;

			action(this);

			const deriveds = this._getDerivedEntities && this._getDerivedEntities();
			if (!deriveds) return;

			deriveds.forEach(se => se.applyToThisAndDerived(action));
		}


		//#region Components

		col(...members: SemanticMembers<any>[])
		{
			return this.member0().col(members);
		}

		row(...members: SemanticMembers<any>[])
		{
			return this.member0().row(members);
		}

		grid(
			masterMember?: SemanticMember | ((se) => SemanticMember),
			setter?: (sm: SemanticMember) => void
		)
		{
			return this.collection(() => this, <any>masterMember, setter).field();
		}


		chart(
			masterMember: SemanticMember | ((se) => SemanticMember),
			setter?: (sm: SemanticMember) => void
		)
		{
			const sf = this.collection(() => this, masterMember, setter).field();
			sf._type = FieldTypes.Chart.Chart;
			return sf;
		}

		//#endregion
	}


	export function $doForDerived<TEntity extends SemanticEntity>(se: TEntity, action: (se: TEntity) => void)
	{
		se.applyToThisAndDerived(action);
	}


	export interface ISemanticEntity extends SemanticEntity
	{
	}

	export interface ISemanticEntityRecalc
	{
		id: any;
		data: any;
		select?: string[];
		propertyName?: string;
	}


	export function isEntity(obj: Object): obj is SemanticEntity
	{
		return obj instanceof SemanticEntity;
	}

}