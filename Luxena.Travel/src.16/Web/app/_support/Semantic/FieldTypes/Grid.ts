module Luxena.FieldTypes
{

	export interface IGridTypeConfig
	{
		gridConfig: GridControllerConfigExt;
	}

	export class Grid extends CollectionFieldType
	{
		static Grid = new Grid();

		_gridMode = true;
		_isComposite = true;

		render(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const cfg = <GridControllerConfig>this.getControllerConfig(sf, "GridController", { inline: true });

			//var totalCount = ko.observable(0);

			//cfg.onTotalCountChange = (ctrl, newCount) => totalCount(newCount);

			//sf.otitle(() => (sf._titles || sf._title || sf._names || sf._name) + " (" + totalCount() + ")");

			const scope = new GridController(cfg).getScope();

			sf._controller.widgets[sf.uname()] = scope;

			valueEl.append(
				`<div data-bind="dxDataGrid: widgets.${sf.uname() }.gridOptions"></div>`
			);
		}

	}

}