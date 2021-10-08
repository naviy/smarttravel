namespace Luxena.Components_
{

	export class GridOptions extends ComponentOptions
	{
		//_entity: SemanticEntity;

		columns: Array<Semantic.Member | Component | DevExpress.ui.dxDataGridColumn>;
		gridOptions: DevExpress.ui.dxDataGridOptions;
	}


	export class GridOptionsSetter extends ComponentOptionsSetter<GridOptions>
	{

		columns(value: Array<Semantic.Member | Component | DevExpress.ui.dxDataGridColumn>)
		{
			this._options.columns = value;
			return this;
		}

		gridOptions(value: DevExpress.ui.dxDataGridOptions)
		{
			this._options.gridOptions = value;
			return this;
		}

	}	

}