module Luxena
{

	export enum ChartColorMode { Default, NegativePositive }

	export interface ChartControllerConfigExt extends CollectionControllerConfig
	{
		chartOptions?: DevExpress.viz.charts.dxChartOptions;

		argument?: SemanticMember;
		value?: SemanticMember | SemanticMember[];

		type?: string;
		colorMode?: ChartColorMode;

		height?: number;
		tooltip?: boolean;
		zoom?: boolean;
	}

	export interface ChartControllerConfig extends ChartControllerConfigExt
	{
	}


	export class ChartController extends CollectionController<ChartControllerConfig>
	{
		config: ChartControllerConfig;
		scope: any;
		defaults;


		constructor(cfg: ChartControllerConfig)
		{
			this.chartMode = true;

			if (!cfg.members)
			{
				cfg.members = [];
				if (cfg.argument) cfg.members.push(cfg.argument);
				if (cfg.value) cfg.members.push(cfg.value);
			}

			super(cfg);

			this.addComponents(cfg.members, null, "columns");
		}

		getId() { Error("Not Implement"); }

		getScope(): any
		{
			return $do(super.getScope(), a =>
				a.chartOptions = this.getChartOptions()
			);
		}


		private _argumentDataMap: (data) => void;
		private _valueDataMap: (data) => void;

		dataMap(data: ISemanticRowData)
		{
			super.dataMap(data);

			data["$data"] = data;

			this._argumentDataMap && this._argumentDataMap(data);
			this._valueDataMap && this._valueDataMap(data);

			return data;
		}

		getChartOptions(): DevExpress.viz.charts.dxChartOptions
		{
			const cfg = this.config;
			const arg = cfg.argument;

			let values = <SemanticMember[]>(cfg.value instanceof SemanticMember ? [cfg.value] : cfg.value);
			if (!values.length) values = null;

			if (!this.fields.length)
			{
				if (arg) this.fields.push(arg.field());
				if (values && values.length) this.fields.push(...values.map(a => a.field()));
			}

			const options = <DevExpress.viz.charts.dxChartOptions>$.extend({
				dataSource: this.getDataSourceConfig(),
				loadingIndicator: { show: true },
			}, cfg.chartOptions);


			if (cfg.height)
			{
				if (!options.size)
					options.size = {};
				options.size.height = cfg.height;
			}

			if (cfg.zoom)
			{
				options.zoomingMode = "all";
				options.scrollingMode = "all";
				options.scrollBar = $.extend(options.scrollBar, {
					visible: true,
					width: 3,
					position: "bottom",
					opacity: 0.5,
				});
			}

			var commonSeries = options.commonSeriesSettings;
			if (!commonSeries)
				commonSeries = options.commonSeriesSettings = {};

			if (!commonSeries.type && cfg.type)
				commonSeries.type = cfg.type;


			//#region argument

			if (arg)
			{
				let argName = arg._name;
				const ref = arg.getLookupEntity();

				let argAxis = options.argumentAxis;
				if (!argAxis)
					options.argumentAxis = argAxis = {};

				if (!commonSeries.argumentField)
				{
					commonSeries.argumentField = argName;
					if (ref)
						commonSeries.argumentField += "_" + ref._lookupFields.name;

					if (arg._type.chartDataType)
					{
						if (!options.argumentAxis)
							options.argumentAxis = {};

						options.argumentAxis.argumentType = arg._type.chartDataType;
					}
				}

				//if (!ref)
				//	argAxis.grid = $.extend(argAxis.grid, { visible: true });

				if (ref)
				{
					let nameName = ref._lookupFields.name;
					let idName = ref._lookupFields.id;

					this._argumentDataMap = data =>
					{
						const argData = data[argName];
						if (!argData) return;
						data[argName + "_" + idName] = argData[idName];
						data[argName + "_" + nameName] = argData[nameName];
					}

					if (options.rotated === undefined)
					{
						options.rotated = true;
						argAxis.inverted = true;
					}

					if (!commonSeries.type)
						commonSeries.type = "bar";

					if (!commonSeries.tagField)
						commonSeries.tagField = "$data"; //argName + "_" + idName;

					if (!options.onPointClick)
						options.onPointClick = e =>
						{
							const data = e.target.tag as ISemanticRowData;
							const argData = data[argName];
							const id = argData[idName];
							if (!id) return;

							e.target.hideTooltip();

							const ref2 = ref._isAbstract && sd.entityByOData(argData, ref) || ref;
							ref2.toggleSmart(e.target.graphic.element, {
								id: id,
								view: ref2,
								edit: ref2,
								//refreshMaster: () => this.grid && this.grid.refresh(),
							});
						};
				}
			}

			//#endregion


			//#region value

			if (!options.series)
				options.series = [];

			var panes: { [name: string]: DevExpress.viz.charts.Pane; } = {};
			if ($.isArray(options.panes))
			{
				options.panes.forEach(a =>
					panes[a.name] = a
				);
			}

			values && values.forEach((val, valueIndex) =>
			{
				const varName = val._name;

				let format = val._format; // + (val._precision || "");

				var series = options.series[valueIndex];
				if (!series)
					series = options.series[valueIndex] = {};

				if (!series.name)
					series.name = val._title || val._name;

				if (!series.valueField)
				{
					series.valueField = varName;

					if (val._type instanceof FieldTypes.Money)
					{
						series.valueField += "_Amount";
						this._valueDataMap = r => r[varName + "_Amount"] = r[varName].Amount;
						format = "n";
					}
				}

				if (!options.legend)
					options.legend = { visible: false };

				var pane = panes[series.pane];
				if (series.pane && !pane)
				{
					if (!options.panes)
						options.panes = [];

					options.panes.push(panes[series.pane] = pane = { name: series.pane });
				}

				if (!options.valueAxis)
					options.valueAxis = [];

				let valueAxis: DevExpress.viz.charts.ChartValueAxis;

				if (pane)
				{
					valueAxis = options.valueAxis.filter(a => a.pane === pane.name)[0];
					if (!valueAxis)
						options.valueAxis.push(valueAxis = { pane: pane.name });
				}
				else
				{
					valueAxis = options.valueAxis[0];
					if (!valueAxis)
						options.valueAxis.push(valueAxis = {});
				}

				if (!valueAxis.title)
					valueAxis.title = { text: val._title };

				if (val._type.chartDataType)
					valueAxis.valueType = val._type.chartDataType;

				if (cfg.colorMode === ChartColorMode.NegativePositive && valueAxis.valueType === "numeric")
					options.customizePoint = function ()
					{
						// ReSharper disable once SuspiciousThisUsage
						const value = this.value;
						if (value > 0)
							return { color: '#859666', hoverStyle: { color: '#859666' } };
						else
							return { color: '#BA4D51', hoverStyle: { color: '#BA4D51' } };
					};


				if (format)
				{
					let label = valueAxis.label;
					if (!label)
						valueAxis.label = label = {};

					if (format === "n" && !series.point)
						series.point = { visible: false };

					if (!label.format)
						label.format = format === "n" ? "n0" : format;
				}

			});

			//#endregion


			if (arg && cfg.tooltip !== false)
			{
				if (!options.tooltip)
					options.tooltip = { enabled: true };

				if (values && values.length)
				{
					let val = values[0];

					options.tooltip.customizeTooltip = (args: any) =>
					{
						var argument = args.argument;
						var argFormat = arg._format || arg._type.format;
						if (argFormat)
							argument = Globalize.format(argument, argFormat);
						
						let value = args.point.value;
						if (val._format)
							value = Globalize.format(value, val._format);

						const html = [
							`<table><tr><td>${arg._title}: &nbsp;</td><td><b>${argument}</b></td></tr>`,
							`<tr><td>${args.seriesName}: &nbsp;</td><td><b>${value}</b></td></tr>`,
							`</table>`,
						];

						return { html: html.join(""), };
					};
				}
			}

			return options;
		}

	}

}
