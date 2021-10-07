module Luxena
{

	export interface GridControllerConfigExt extends CollectionControllerConfig
	{
		inline?: boolean;
		listMode?: boolean;

		useFilter?: boolean;
		useFilterRow?: boolean;
		useSearch?: boolean;

		useExport?: boolean;
		useHeader?: boolean;
		useGrouping?: boolean;
		usePaging?: boolean;
		usePager?: boolean;
		useSorting?: boolean;

		wide?: boolean;
		small?: boolean;

		columnsIsStatic?: boolean;

		gridOptions?: DevExpress.ui.dxDataGridOptions;

		height?: number;
		fullHeight?: boolean;

		entityStatus?: (data) => string;

		onTotalCountChange?: (ctrl: GridController, totalCount: number) => void;
	}

	export interface GridControllerConfig extends GridControllerConfigExt
	{
	}


	export class GridController extends CollectionController<GridControllerConfig>
	{
		grid: DevExpress.ui.dxDataGrid;

		selectedRowKeys: KnockoutObservableArray<any>;


		constructor(cfg: GridControllerConfig)
		{
			this.gridMode = true;

			if (cfg.inline)
			{
				//cfg.useExport = cfg.useExport === true;
				cfg.useFilterRow = cfg.useFilterRow === true;
				cfg.useGrouping = cfg.useGrouping === true;
				cfg.useSearch = cfg.useSearch === true;
				cfg.columnsIsStatic = cfg.columnsIsStatic !== false;
				cfg.usePager = cfg.usePager !== false;
			}

			if (cfg.listMode)
			{
				cfg.useExport = cfg.useExport === true;
				cfg.useHeader = cfg.useHeader === true;
				cfg.useFilterRow = cfg.useFilterRow === true;
				cfg.useGrouping = cfg.useGrouping === true;
				cfg.useSearch = cfg.useSearch === true;
				cfg.columnsIsStatic = cfg.columnsIsStatic !== false;

				if (!cfg.usePager && cfg.height === undefined)
					cfg.height = 300;
			}

			if (cfg.fixed)
			{
				cfg.useFilter = cfg.useFilter === true;
				cfg.useSearch = cfg.useSearch === true;
				cfg.usePager = cfg.usePager !== false;
			}

			if (cfg.small || cfg.entity._isSmall)
			{
				cfg.useFilterRow = cfg.useFilterRow === true;
				cfg.useGrouping = cfg.useGrouping === true;
			}

			// ReSharper disable RedundantComparisonWithBoolean
			cfg.inline = cfg.inline === true;
			cfg.fixed = cfg.fixed === true;
			cfg.useExport = cfg.useExport !== false;
			cfg.useHeader = cfg.useHeader !== false;
			cfg.useFilter = cfg.useFilter !== false;
			cfg.useFilterRow = cfg.useFilterRow !== false;
			cfg.useGrouping = cfg.useGrouping !== false;
			cfg.usePager = cfg.usePager === true;
			cfg.usePaging = cfg.usePaging !== false || cfg.usePager;
			cfg.useSearch = cfg.useSearch !== false;
			cfg.useSorting = cfg.useSorting !== false;
			cfg.columnsIsStatic = cfg.columnsIsStatic === true;
			// ReSharper restore RedundantComparisonWithBoolean
			
			super(cfg);

			this.selectedRowKeys = ko.observableArray([]);

			this.createMembers();
		}

		getScope()
		{
			return $do(super.getScope(), a=>
				a.gridOptions = this.getGridOptions()
			);
		}

		createMembers()
		{
			const cfg = this.config;
			const se = cfg.entity;
			const members = cfg.members;

			this.addComponents(members, null, "columns", (sm, sc) => sc._visible = sc._columnVisible && sm._columnVisible);

			if (se instanceof EntitySemantic)
			{
				this.addComponent(se.Id, null);

				if (se instanceof Entity2Semantic)
				{
					this.addComponents([
						se.CreatedOn,
						se.CreatedBy,
						se.ModifiedOn,
						se.ModifiedBy,
					], null, "columns");
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
				return data[this.config.entity._lookupFields.id];

			var keys = this.selectedRowKeys();
			return keys[0];
		}

		getGridOptions(): DevExpress.ui.dxDataGridOptions
		{
			const cfg = this.config;
			const se = this._entity;

			const options = <DevExpress.ui.dxDataGridOptions>$.extend(<DevExpress.ui.dxDataGridOptions>{
				dataSource: this.getDataSource(),

				allowColumnReordering: !cfg.columnsIsStatic,
				allowColumnResizing: true,
				allowGrouping: cfg.useGrouping,
				columnAutoWidth: true,
				columnChooser: { enabled: !cfg.columnsIsStatic },
				hoverStateEnabled: true,
				//rowAlternationEnabled: true,
				showRowLines: true,
				showColumnLines: false,
				wordWrapEnabled: true,
				height: cfg.height,

				groupPanel: {
					visible: cfg.useGrouping,
				},

				filterRow: {
					visible: cfg.useFilter && cfg.useFilterRow,
				},

				searchPanel: {
					visible: cfg.useFilter && cfg.useSearch,
					width: 240,
				},

				paging: {
					enabled: cfg.usePaging,
					pageSize: cfg.usePager ? 10 : 30,
				},

				scrolling: {
					mode: cfg.usePager || cfg.inline || cfg.fixed ? "standard" : "virtual",
				},

				selection: {
					mode: "single",
				},

				showColumnHeaders: cfg.useHeader,

				sorting: { mode: cfg.useSorting ? "multiple" : "none" },

				"export": {
					enabled: cfg.useExport,//!cfg.inline,
					fileName: cfg.entity._names,
					//allowExportSelectedData: true,
					excelFilterEnabled: true,
					excelWrapTextEnabled: true,
				},

				onInitialized: e =>
				{
					this.grid = e.component;
					//$log(e.element[0].outerHTML);
					//this.appendButtons(e.element.find(".dx-datagrid-header-panel"));
				},

				onSelectionChanged: e =>
				{
					this.selectedRowKeys(e.selectedRowKeys);
				},

				onRowClick: e =>
				{
					if (e.rowType !== "data") return;

					if (this.config.master && this.config.master.smartMode)
						return;

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
					//if (!this._pagerIsRepainted)
					//{
					//	this._pagerIsRepainted = true;
					//	const accordionId = this.scope.accordionId;

					//	if (accordionId)
					//	{
					//		const accordion = $("#" + accordionId).dxAccordion("instance");
					//		const selectedItems = <any>accordion.option("selectedItems");
					//		accordion.option("selectedItems", []);
					//		accordion.option("selectedItems", selectedItems);
					//	}

					//	if (this.config.fullHeight)
					//	{
					//		this.grid.option("height", "100%");
					//	}
					//}

					if (cfg.onTotalCountChange)
						cfg.onTotalCountChange(this, e.component.totalCount());

					this.appendButtons(e.element);

					this.selectByLastId();
				},

			}, cfg.gridOptions);

			let entityStatus = cfg.entityStatus || se._entityStatusGetter;

			if (entityStatus)
				options.onCellPrepared = (e: any) =>
				{
					if (e.rowType !== "data") return;

					let rowClass = entityStatus(e.data);
					if (!rowClass) return;

					if (rowClass === "error")
						rowClass = "cell-state-error";
					else if (rowClass === "success")
						rowClass = "cell-state-success";
					else if (rowClass === "warning")
						rowClass = "cell-state-warning";
					else if (rowClass === "disabled")
						rowClass = "cell-state-disabled";

					e.cellElement.addClass(rowClass);
				};

			this.appendComponentsToGridOptions(options);
			
			return options;
		}

		appendComponentsToGridOptions(options: DevExpress.ui.dxDataGridOptions)
		{
			var cfg = this.config;

			var columns: DevExpress.ui.dxDataGridColumn[] = [];
			var totalItems = [], groupItems = [];


			const entityPositions: Field[] = [];
			const entityNames: Field[] = [];
			const entityDates: Field[] = [];

			this.components.forEach(sc =>
			{
				if (isField(sc))
				{
					const sm = sc._member;

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


			this.components.forEach(sc2 =>
			{
				var cols = sc2.toGridColumns();

				if (sc2._parent)
					cols.forEach(c => c.visible = false);

				if (cols.length && isField(sc2))
				{
					if (sc2._member._isEntityName)
						cols[0].width = 0;
					if (cfg.useGrouping === false)
						delete cols[0].groupIndex;
				}

				columns.push(...cols);

				var items = sc2.toGridTotalItems();
				items.forEach(a =>
				{
					totalItems.push(a);
					groupItems.push($.extend({
						showInGroupFooter: true,
						alignByColumn: true,
					}, a));
				});
			});

			//$log(columns);
			options.columns = columns;

			if (totalItems.length)
				options.summary = {
					groupItems: groupItems,
					totalItems: totalItems,
				};
		}

		refresh()
		{
			this.grid && this.grid.refresh();
		}

		getDataSource(): DevExpress.data.DataSource
		{
			const options = this.getDataSourceConfig();
			const cfg = this.config;

			var ds = new DevExpress.data.DataSource(options);

			if (cfg.master instanceof FilterFormController)
			{
				const ofilter: KnockoutObservable<any> =
					(<FilterFormController>cfg.master).filter;
				ofilter.subscribe(newFilter =>
				{
					ds.filter(newFilter);
					this.grid && this.grid.refresh();
				});
			}

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

		appendButtons(container: JQuery)
		{
			const cfg = this.config;

			if (!cfg.master) return;

			const header = container.find(".dx-datagrid-header-panel");
			if (container.find("#app-buttons").length) return;

			const btns = $(`<div id="app-buttons" class="pull-right">`);

			$(`<div>`).appendTo(btns).dxButton(
				(this.addComponent(this._entity.refreshAction.button().small(), null, "buttons") as Components.Button).buttonOptions()
			);

			if (cfg.edit)
			{
				const newBtn = cfg.edit.newAction.button();
				if (this.config.master && this.config.master.smartMode)
					newBtn.small();

				$(`<div>`).appendTo(btns).dxButton(
					(this.addComponent(newBtn, null, "buttons") as Components.Button).buttonOptions()
				);
			}

			header.append(btns);
		}

		getToolbarItems()
		{
			const cfg = this.config;

			const items = super.getToolbarItems();

			if (!cfg.master)
			{
				if (cfg.edit)
					items.push(cfg.edit.newAction.button().right());
				items.push(this._entity.refreshAction.button().small().right());
			}

			return items;
		}


	}

}