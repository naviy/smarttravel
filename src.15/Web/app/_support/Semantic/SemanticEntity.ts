module Luxena
{

	export class SemanticEntity extends SemanticObject<SemanticEntity>
	{
		_className: string;
		_isAbstract: boolean;

		_store: DevExpress.data.ODataStore;
		_lookupStore: DevExpress.data.ODataStore;
		_saveStore: DevExpress.data.ODataStore;
		_referenceFields: { id: string; name: string; } = { id: "Id", name: "Name" };

		_nameMember: SemanticMember;

		_members: SemanticMember[] = [];
		//_actions: SemanticEntityAction[] = [];

		_getDerivedEntities: () => SemanticEntity[];
		_getRootEntity: () => SemanticEntity;
		_getBaseEntity: () => SemanticEntity;

		_listAction: IEntityAction;
		_viewAction: IEntityAction;
		_smartAction: IEntityAction;
		_editAction: IEntityAction;

		_lookupItemTemplate: (r, index: Number, container: JQuery) => void | string;


		_isBig: boolean;
		_isSmall: boolean;
		_isWide: boolean;

		_isEntity: boolean;
		_isEntityQuery: boolean;
		_isQueryParams: boolean;
		_isQueryResult: boolean;
		_isDomainAction: boolean;

		_listController: (params: any, viewInfo) => any;
		_viewController: (params: { id: any }, viewInfo) => any;
		_editController: (params: { id: any }, viewInfo) => any;

		/** Сохраняется контроллерами id последнего просмотриваемого/редактируемого entity */
		_lastId: any;

		_textIconHtml: string = "";

		_titleMenuItems: Array<SemanticEntity | MenuSubitemOptions>;

		titleMenuItems(titleMenuItems: Array<SemanticEntity | MenuSubitemOptions>): SemanticEntity
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
				var i = items.indexOf(this);
				if (i >= 0)
				{
					items = items.slice(0);
					items.splice(i, 1);
				}
			}

			return items;
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
			collectionItemMasterMember: (se: TEntity) => SemanticMember,
			setter?: (col: SemanticCollectionMember<TEntity>) => void)
		{
			const m = new SemanticCollectionMember<TEntity>();
			m._entity = this;
			m._collectionItemEntity = collectionItemEntity;
			m._collectionItemMasterMember = collectionItemMasterMember;

			setter && setter(m);

			this._members.push(m);

			return m;
		}

		action(getParams?: (prms) => Object)
		{
			return new SemanticEntityAction(this, getParams);
		}

		toAction(getParams?: (prms) => Object)
		{
			return new SemanticEntityAction(this, getParams);
		}

		init(): void
		{
			this.initMembers();
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

			this._nameMember = this._nameMember || this[this._referenceFields.name];

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


		///#region Setters

		icon(value: string): SemanticEntity
		{
			this._icon = value;
			this._textIconHtml = getTextIconHtml(value);
			return this;
		}


		//title(value: string): SemanticEntity
		//{
		//	this._title = value;
		//	return this;
		//}

		//localizeTitle(value: ILocalization): SemanticEntity
		//{
		//	this._localizeTitle(value);
		//	return this;
		//}


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

			var title = ko.unwrap(data[this._referenceFields.name]);

			return title ? this._title + " " + title : this._title;
		}


		listView = () => this._names;
		viewView = () => this._name;
		editView = () => this._name + "Edit";

		resolveListAction(action?: SemanticEntity | IEntityAction)
		{
			if (action === null)
				return null;

			if (action instanceof SemanticEntity)
				return action.resolveListAction();

			if (action === undefined)
				action = this._listAction || { uri: { view: this.listView() } };

			return action;
		}

		resolveViewAction(action?: SemanticEntity | IEntityAction, formEntity?: SemanticEntity)
		{
			if (action === null)
				return null;

			if (action instanceof SemanticEntity)
				return action.resolveViewAction();

			if (action === undefined)
			{
				formEntity = formEntity || this;
				action = formEntity._viewAction || { uri: { view: formEntity.viewView() } };
			}

			return action;
		}

		resolveEditAction(action?: SemanticEntity | IEntityAction, formEntity?: SemanticEntity)
		{
			if (action === null)
				return null;

			if (action instanceof SemanticEntity)
				return action.resolveEditAction();

			if (action === undefined)
			{
				formEntity = formEntity || this;
				action = formEntity._editAction || { uri: { view: formEntity.editView() } };
			}

			return action;
		}


		navigateToList()
		{
			var action = this.resolveListAction();
			app.navigate(action.uri);
		}


		showSmart: (target, cfg: ISmartFormControllerConfig) => void;

		toggleSmart(target, cfg: ISmartFormControllerConfig)
		{
			if (!this.showSmart) return;

			if (smartVisible() && smartTarget() === target)
				smartVisible(false);
			else
			{
				this.showSmart(target, cfg);
			}
		}


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


		toListMenuItem(onExecute?: (e) => void)
		{
			const title = this._titles || this._title || this._names || this._name;

			return <MenuSubitemOptions>{
				icon: this._icon,
				text: title,
				title: title,
				description: this._description,
				url: this.resolveListAction().uri.view,
				onExecute: onExecute,

				isList: true,
			};
		}

		toViewMenuItem(id?)
		{
			const title = this._title || this._name;

			return <MenuSubitemOptions>{
				icon: this._icon,
				text: title,
				title: title,
				description: this._description,
				url: this.resolveViewAction().uri.view + (id ? "/" + id : ''),
			};
		}

		toEditMenuItem(id?)
		{
			const title = this._title || this._name;

			return <MenuSubitemOptions>{
				icon: this._icon,
				text: title,
				title: title,
				description: this._description,
				url: this.editView() + (id ? "/" + id : ''),
			};
		}



		toListButton()
		{
			var action = () => openAction(this.resolveListAction());

			return {
				icon: "list",
				text: "Перейти к списку",
				onClick: action,
				onExecute: action,
			};
		}

		toViewButton(id)
		{
			var action = () => openAction(this.resolveViewAction(), id);

			return {
				icon: "fa fa-search",
				text: "Подробнее",
				type: "default",
				onClick: action,
				onExecute: action,
			};
		}

		toAddMenuItem(defaults?: Object): MenuSubitemOptions
		{
			var title = this._title;

			var action = () =>
			{
				var a = this.resolveEditAction();
				a.defaults = defaults;
				openAction(a);
			};

			return {
				icon: this._icon,
				text: title,
				title: title,
				description: this._description,
				onClick: action,
				onExecute: action,
			};
		}

		toAddButton(defaults?)
		{
			var addCmd = <any>{
				icon: "plus",
				text: "Добавить",
			};

			if (!this._isAbstract)
			{
				addCmd.onExecute = this.toAddMenuItem(defaults).onExecute;
				addCmd.onClick = addCmd.onExecute;
			}
			else
			{
				var addItems = [];

				const deriveds = this._getDerivedEntities && this._getDerivedEntities();

				deriveds && deriveds.forEach(a =>
				{
					if (a._isAbstract) return;
					addItems.push(a.toAddMenuItem(defaults));
				});

				addCmd.items = addItems;
			}

			return addCmd;
		}


		toEditButton(id)
		{
			var action = () => openAction(this.resolveEditAction(), id);

			return {
				icon: "edit",
				text: "Изменить",
				type: "success",
				onClick: action,
				onExecute: action,
			};
		}

		toDeleteButton(id, done: () => void)
		{
			var action = () => this.delete(id).done(done);

			return {
				icon: "trash",
				text: "Удалить",
				type: "danger",
				onClick: action,
				onExecute: action,
			};
		}

		toRefreshButton(resresh: () => void)
		{
			return {
				icon: "refresh",
				text: "Обновить",
				onClick: resresh,
				onExecute: resresh,
			}
		}

		toTab()
		{
			return {
				icon: "fa fa-" + this._icon,
				title: this._title || this._name,
				template: this._name,
			}
		}

		toTabs()
		{
			return {
				icon: "fa fa-" + this._icon,
				title: this._titles || this._title || this._names || this._name,
				template: this._names || this._name,
			}
		}

		applyToThisAndDerived(action: (se: SemanticEntity) => void)
		{
			if (!action) return;

			action(this);

			var derived = this._getDerivedEntities && this._getDerivedEntities();
			if (!derived) return;

			derived.forEach(se => action(se));
		}

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


}