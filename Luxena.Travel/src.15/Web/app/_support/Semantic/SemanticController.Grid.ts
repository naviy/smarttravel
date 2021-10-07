module Luxena
{

	export interface IGridControllerConfigExt
	{
		title?: string;

		filter?: any;

		inline?: boolean;
		fixed?: boolean;

		useFilter?: boolean;
		useFilterRow?: boolean;
		useSearch?: boolean;

		useGrouping?: boolean;
		usePaging?: boolean;
		useSorting?: boolean;

		wide?: boolean;

		columnsIsStatic?: boolean;

		gridOptions?: DevExpress.ui.dxDataGridOptions;

		fullHeight?: boolean;
	}

	export interface IGridControllerConfig extends SemanticControllerConfig, IGridControllerConfigExt
	{
		members: SemanticMembers<any>;
		master?: FormController<FormControllerConfig>;

		defaults?: Array<[SemanticMember, any]>;
	}


	export interface ISemanticRowData
	{
		_smartEntity: SemanticEntity;
		_viewEntity: SemanticEntity;
		_editEntity: SemanticEntity;
	}


	export class GridController extends SemanticController
	{
		config: IGridControllerConfig;
		scope: any;
		grid: DevExpress.ui.dxDataGrid;

		editDefaults;

		selectedRowKeys: KnockoutObservableArray<any>;

		private _pagerIsRepainted: boolean;


		constructor(cfg: IGridControllerConfig)
		{
			this.gridMode = true;

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

					sm._type.saveToData(new SemanticField(sm), model, defaults);
				});

				this.editDefaults = defaults;
			}

			// ReSharper disable RedundantComparisonWithBoolean
			cfg.inline = cfg.inline === true;
			cfg.fixed = cfg.fixed === true;
			cfg.useFilter = cfg.useFilter !== false;
			cfg.useFilterRow = cfg.useFilterRow !== false;
			cfg.useGrouping = cfg.useGrouping !== false;
			cfg.usePaging = cfg.usePaging !== false;
			cfg.useSearch = cfg.useSearch !== false;
			cfg.useSorting = cfg.useSorting !== false;
			cfg.columnsIsStatic = cfg.columnsIsStatic === true;
			// ReSharper restore RedundantComparisonWithBoolean

			if (cfg.inline)
			{
				cfg.useFilterRow = false;
				cfg.useGrouping = false;
				cfg.useSearch = false;
				cfg.columnsIsStatic = true;
			}

			if (cfg.entity._isSmall)
			{
				cfg.useFilterRow = false;
				cfg.useGrouping = false;
			}

			super(cfg);

			this.selectedRowKeys = ko.observableArray([]);

			this.createMembers();

			if (cfg.master)
				cfg.master.addDetails(this);
		}

		getScope(): any
		{
			var ctrl = this;
			var cfg = this.config;
			var se = cfg.entity;

			return this.scope = {
				icon: se._icon,
				title: se._titles || se._title || se._names || se._name,

				titleMenuItems: toMenuSubitems(se.getTitleMenuItems()),
				viewMenuItems: ctrl.getMenuItems(),

				gridOptions: ctrl.getGridOptions(),

				viewShown: () => this.viewShown(),
				viewHidden: () => this.viewHidden(),
			};
		}

		createMembers()
		{
			const cfg = this.config;
			const se = cfg.entity;
			const members = cfg.members; 
			
			//$log(cfg.entity._name);
			//$log(members(cfg.entity));

			this.addComponents(members, "columns", (sm, sc) => sc._visible = sm._columnVisible);
			if (se instanceof EntitySemantic)
			{
				this.addComponent(se.Id);

				if (se instanceof Entity2Semantic)
				{
					this.addComponents([
						se.CreatedOn,
						se.CreatedBy,
						se.ModifiedOn,
						se.ModifiedBy,
					], "columns");
				}
			}
		}


		viewShown()
		{
			if (!this.grid) return;

			this.selectByLastId(true);
		}

		viewHidden()
		{
			smartVisible(false);
		}

		getId(data?): Object
		{
			if (data)
				return data[this.config.entity._referenceFields.id];

			var keys = this.selectedRowKeys();
			return keys[0];
		}

		getGridOptions(): DevExpress.ui.dxDataGridOptions
		{
			const cfg = this.config;

			const options = $.extend(<DevExpress.ui.dxDataGridOptions>{
				dataSource: this.getDataSource(),

				columnChooser: { enabled: !cfg.columnsIsStatic },
				allowColumnReordering: !cfg.columnsIsStatic,
				allowColumnResizing: true,
				hoverStateEnabled: true,

				groupPanel: {
					visible: cfg.useGrouping,
					emptyPanelText: "Для группировки по колонке перетащите сюда её заголовок."
				},

				filterRow: {
					visible: cfg.useFilter && cfg.useFilterRow && !cfg.fixed
				},

				searchPanel: {
					visible: cfg.useFilter && cfg.useSearch && !cfg.fixed,
					width: 240,
				},

				paging: {
					enabled: cfg.usePaging && !cfg.fixed,
					pageSize: cfg.inline ? 10 : 30,
				},

				scrolling: {
					mode: cfg.inline || !cfg.usePaging || cfg.fixed ? "standard" : "virtual",
				},

				selection: {
					mode: "single",
				},

				sorting: { mode: cfg.useSorting ? "multiple" : "none" },

				"export": {
					enabled: !cfg.inline,
					fileName: cfg.entity._names,
					//allowExportSelectedData: true,
					excelFilterEnabled: true,
					excelWrapTextEnabled: true,
				},


				onInitialized: e =>
				{
					this.grid = e.component;
				},

				onSelectionChanged: e =>
				{
					this.selectedRowKeys(e.selectedRowKeys);
				},

				onRowClick: e =>
				{
					if (e.rowType !== "data") return;

					var data = e.data;

					data._smartEntity.toggleSmart(e.rowElement[0], {
						id: this.getId(data),
						view: data._viewEntity,
						edit: data._editEntity,
						refreshMaster: () => this.grid && this.grid.refresh(),
					});
				},


				onContentReady: e =>
				{
					if (!this._pagerIsRepainted)
					{
						this._pagerIsRepainted = true;
						const accordionId = this.scope.accordionId;

						if (accordionId)
						{
							const accordion = $("#" + accordionId).dxAccordion("instance");
							const selectedItems = <any>accordion.option("selectedItems");
							accordion.option("selectedItems", []);
							accordion.option("selectedItems", selectedItems);
						}

						if (this.config.fullHeight)
						{
							this.grid.option("height", "100%");
						}
					}

					this.selectByLastId();
				},

			}, cfg.gridOptions);
			this.appendComponentToGridOptions(options);

			return options;
		}


		appendComponentToGridOptions(options: DevExpress.ui.dxDataGridOptions)
		{
			//var cfg = this.config;

			var columns: DevExpress.ui.dxDataGridColumn[] = [];
			var totalItems = [], groupItems = [];


			const entityPositions: SemanticField[] = [];
			const entityNames: SemanticField[] = [];
			const entityDates: SemanticField[] = [];

			this.components.forEach(sc =>
			{
				if (sc instanceof SemanticField)
				{
					const sm = sc.member;

					if (sm._isEntityPosition)
						entityPositions.push(sc);
					if (sm._isEntityName)
						entityNames.push(sc);
					else if (sm._isEntityDate)
						entityDates.push(sc);
				}
			});

			if (entityPositions.length)
			{
				entityPositions.forEach(a => a.sortOrder = "asc");
			}
			else if (entityDates.length)
			{
				entityDates.forEach(a => a.sortOrder = "desc");
				entityNames.forEach(a => a.sortOrder = "desc");
			}
			else
			{
				entityNames.forEach(a => a.sortOrder = "asc");
			}


			this.components.forEach(sc =>
			{
				var cols = sc.toGridColumns();
				cols.forEach(a => columns.push(a));

				var items = sc.toGridTotalItems();
				items.forEach(a =>
				{
					totalItems.push(a);
					groupItems.push($.extend({
						showInGroupFooter: true,
						alignByColumn: true,
					}, a));
				});
			});

			options.columns = columns;

			if (totalItems.length)
				options.summary = {
					groupItems: groupItems,
					totalItems: totalItems,
				};
		}


		getDataSource(): DevExpress.data.DataSource
		{
			const options = this.getDataSourceConfig();
			var cfg = this.config;

			let ofilter: KnockoutObservable<any>;

			if (cfg.filter)
				options.filter = cfg.filter;
			else if (cfg.defaults)
			{
				const filter = cfg.defaults.map(a => a[0].getFilterExpr(a[1]));
				if (filter.length)
					options.filter = filter;
			}
			else if (cfg.master instanceof FilterFormController)
			{
				ofilter = (<FilterFormController>cfg.master).filter;
				//				options.filter = ofilter;
				options.filter = ko.unwrap(ofilter);
			}

			if (cfg.entity._isQueryResult || cfg.fixed)
			{
				delete options.expand;
				delete options.select;
			}

			options.map = (data: ISemanticRowData) =>
			{
				var se = cfg.entity._isAbstract && sd.entityByOData(data) || cfg.entity;

				data._viewEntity = cfg.view === cfg.entity ? se : cfg.view;
				data._smartEntity = cfg.smart === cfg.entity ? se : cfg.smart;
				data._editEntity = cfg.edit === cfg.entity ? se : cfg.edit;

				return data;
			};

			var ds = new DevExpress.data.DataSource(options);

			if (ofilter)
				ofilter.subscribe(newFilter =>
				{
					ds.filter(newFilter);
					//					$log(newFilter);
					this.grid && this.grid.refresh();
				});

			return ds;
		}

		selectByLastId(onViewShown?: boolean)
		{
			const id = this.config.entity._lastId;
			if (!id || !this.grid) return;

			if (onViewShown)
			{
				this.grid.refresh();
			}
			else
			{
				this.config.entity._lastId = null;

				const keys = this.grid.getSelectedRowKeys();

				if (!keys.length || keys.length === 1 && keys[0] !== id)
				{
					this.grid.selectRows([id], false);
				}

			}
		}

		getMenuItems(): Array<any>
		{
			const cfg = this.config;
			const items = [];

			cfg.edit && items.push(cfg.edit.toAddButton(this.editDefaults));

			items.push(cfg.entity.toRefreshButton(() => this.grid && this.grid.refresh()));

			return items;
		}

	}

}