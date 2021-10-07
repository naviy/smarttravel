module Luxena.FieldTypes
{

	export class Chart extends CollectionFieldType
	{
		static Chart = new Chart();

		_chartMode = true;
		_isComposite = true;

		render(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const cfg = <ChartControllerConfig>this.getControllerConfig(sf, "ChartController", { fixed: true });
			const scope = new ChartController(cfg).getScope();

			sf._controller.widgets[sf.uname()] = scope;

			valueEl.append(
				`<div data-bind="dxChart: widgets.${sf.uname() }.chartOptions"></div>`
			);
		}
		
	}
	
}