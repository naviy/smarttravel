namespace Luxena.Components_
{

	export type dxDataGridColumn = DevExpress.ui.dxDataGridColumn;


	export class Grid extends Component$<GridOptions, GridOptionsSetter>
	{
		constructor()
		{
			super();
			this._options = new GridOptions();
			this._optionsSetter = new GridOptionsSetter(this._options);
		}


		////#region Render

		private _columnFields: Component[];
		private _columns: dxDataGridColumn[] = [];

		prepare()
		{
			super.prepare();

			const o = this._options;
			if (!o.columns) return;

			const columnFields = this._columnFields = [];
			const columns = this._columns = [];

			for (let col of o.columns)
			{
				let field: Component;
				if (Semantic.isMember(col))
					field = col.fieldBox();
				else if (isComponent(col))
					field = col;
				else
					field = new GridColumnField(col);

				columnFields.push(field);
				columns.push(...field.toGridColumns());
			}
		}
		
		loadOptions()
		{
			return DataSet.concatLoadOptions(this._columnFields);
		}

		render()
		{
			const o = this._options;
			this._columns[0].sortOrder = "asc";

			const gridOptions = <DevExpress.ui.dxDataGridOptions>$.extend(
				<DevExpress.ui.dxDataGridOptions>{
					columns: this._columns,
					dataSource: this._dataSet.dataSource(),

					//allowColumnReordering: !o.columnsIsStatic,
					allowColumnResizing: true,
					//allowGrouping: o.useGrouping,
					columnAutoWidth: true,
					//columnChooser: { enabled: !o.columnsIsStatic },
					//height: o.height,
					hoverStateEnabled: true,
					//rowAlternationEnabled: true,
					showRowLines: true,
					//showColumnHeaders: cfg.useHeader,
					showColumnLines: false,
					wordWrapEnabled: true,

					pager: {
						showInfo: true,
						showNavigationButtons: true,
					},

					selection: {
						allowSelectAll: false,
						mode: "single",
					},

					scrolling: {
						preloadEnabled: true,
					},

					//onRowClick: e => $alert(e),
					onSelectionChanged: e => this._dataSet.select(e.selectedRowsData)
				}, 
				o.gridOptions
			);

			return $div().dxDataGrid(gridOptions);
		}

		////#endregion

	}


	class GridColumnField extends InternalComponent
	{

		constructor(public _column: DevExpress.ui.dxDataGridColumn)
		{
			super();
		}
		
		loadOptions()
		{
			return {
				select: [this._column.dataField],
			};
		}

		toGridColumns(): DevExpress.ui.dxDataGridColumn[]
		{
			return [this._column];
		}

	}

}