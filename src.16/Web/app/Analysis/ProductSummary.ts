module Luxena.Views
{
	import DxChartOptions = DevExpress.viz.charts.dxChartOptions;


	function registerProductTotalController<TEntity extends SemanticEntity>(se: TEntity, cfg: {
		argumentField: (se: TEntity) => SemanticMember;
		//argumentType?: string;
		//argumentFormat?: string;
		tagField?: (se: TEntity) => SemanticMember;
		rotated?: boolean;
		members?: (se: TEntity) => SemanticMember[];
		onPointClick?: (filterCtrl, arg, tag) => void;

		chartOptions?: (options: DxChartOptions, scope) => void;
	})
	{
		Views[se._names] = (...args) =>
		{
			var filterCtrl = NewProductFilterController(args);

			var p = sd.Product;

			var scope = filterCtrl.getScope();

			scope.viewMenuItems = [
				{
					icon: "refresh",
					text: "Обновить",
					onExecute: () => filterCtrl.apply(),
				}
			];

			scope.title = se._titles || se._title;

			var argumentMember = cfg.argumentField(se);
			var tagMember = cfg.tagField && cfg.tagField(se);
			var argumentFormat = argumentMember._format || argumentMember._type.format;

			var g = new GridController({
				entity: se,
				members: cfg.members,
				filter: filterCtrl.filter,
				fixed: true,
			}).getScope().gridOptions;


			scope = $.extend(scope, {

				title: scope.title,
				template: "chart",
				titleMenuItems: toMenuItems(se.getTitleMenuItems()),
				gridOptions: g,
				chartOptions: <DxChartOptions>{
					dataSource: {
						store: se._store,
						filter: filterCtrl.filter,
					},

					commonSeriesSettings: {
						argumentField: argumentMember._name,
						type: "stackedBar",
						label: {
							format: "fixedPoint",
							precision: 2,
						},
						point: {
							hoverMode: "allArgumentPoints",
						},
						tagField: tagMember && tagMember._name,
					},

					series: [
						{ valueField: "Total", name: p.Total._title, stack: "1", },
						{ valueField: "ServiceFee", name: p.ServiceFee._title, stack: "1", },
						{
							valueField: "GrandTotal",
							name: p.GrandTotal._title,
							stack: "2",
							label: {
								visible: true,
								connector: { visible: true, },
								position: "outside",
							},
						},
					],

					argumentAxis: {
						argumentType: argumentMember._type.chartDataType,
						tickInterval: 1,
						label: {
							format: argumentFormat,
						},
						inverted: cfg.rotated,
					},

					legend: {
						verticalAlignment: "bottom",
						horizontalAlignment: "center",
						itemTextPosition: "right",
					},

					loadingIndicator: {
						show: true,
					},

					//palette: "Harmony Light",
					palette: "Violet",

					resolveLabelOverlapping: "hide",

					rotated: cfg.rotated,

					valueAxis: [
						{
							title: {
								text: "Суммы",
							},

							label: {
								format: "fixedPoint",
								precision: 2,
							},
						}
					],

					scrollBar: {
						visible: true
					},

					scrollingMode: "all",
					zoomingMode: "all",

					title: scope.title,

					tooltip: {
						enabled: true,
						format: "fixedPoint",
						precision: 2,
						shared: true,
						//location: "edge",
						customizeTooltip: (point: any) =>
						{
							//$log(point);
							var argumentText = ko.format(point.originalArgument, argumentFormat);

							return {
								text: "<b>" + argumentText + "</b>:<br>" + point.valueText,
							};
						},
					},


					onPointClick: e =>
					{
						if (!cfg.onPointClick) return;

						//$log(e);
						cfg.onPointClick(filterCtrl, e.target.originalArgument, e.target.tag);
					},

				},

			});

			cfg.chartOptions && cfg.chartOptions(scope.chartOptions, scope);

			return scope;
		};
	}


	export var ProductSummaries = (...args) =>
	{
		var filterForm = NewProductFilterController(args);

		var se = sd.ProductSummary;

		var grid = new GridController({
			entity: se,
			form: sd.Product,
			master: filterForm,
			members: [
				se.IssueDate,
				se.Type,
				se.Name,
				se.Itinerary,
				se.Total,
				se.ServiceFee,
				se.GrandTotal,
			],

			fixed: true,
		});

		return filterForm.getScopeWithGrid(grid);
	};


	var fse = sd.ProductFilter;

	registerProductTotalController(sd.ProductTotalByYear, {
		argumentField: se => se.Year,

		members: se => [se.Year, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg) =>
		{
			filterCtrl.modelValue(fse.MinIssueDate, new Date(arg, 0));
			filterCtrl.modelValue(fse.MaxIssueDate, new Date(arg, 11, 31));

			sd.ProductTotalByMonth.openList();
		}
	});


	registerProductTotalController(sd.ProductTotalByQuarter, {
		argumentField: se => se.IssueDate,

		members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg) =>
		{
			var quarter = arg.getMonth() / 3 >> 0;
			var min = new Date(arg.getFullYear(), quarter * 3, 1);
			var max = new Date(arg.getFullYear(), quarter * 3 + 3, 0);

			filterCtrl.modelValue(fse.MinIssueDate, min);
			filterCtrl.modelValue(fse.MaxIssueDate, max);

			sd.ProductTotalByMonth.openList();
		}
	});


	registerProductTotalController(sd.ProductTotalByMonth, {
		argumentField: se => se.IssueDate,

		members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg) =>
		{
			var min = new Date(arg.getFullYear(), arg.getMonth(), 1);
			var max = new Date(arg.getFullYear(), arg.getMonth() + 1, 0);

			filterCtrl.modelValue(fse.MinIssueDate, min);
			filterCtrl.modelValue(fse.MaxIssueDate, max);

			sd.ProductTotalBySeller.openList();
		}
	});


	registerProductTotalController(sd.ProductTotalByDay, {
		argumentField: se => se.IssueDate,

		members: se => [se.IssueDate, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg) =>
		{
			filterCtrl.modelValue(fse.MinIssueDate, arg);
			filterCtrl.modelValue(fse.MaxIssueDate, arg);

			sd.ProductTotalBySeller.openList();
		},

		chartOptions: chart =>
		{
			chart.commonSeriesSettings.type = "bar";
			//chart.commonSeriesSettings.line = { point: { visible: false } };

			chart.series = [chart.series[2]];
			chart.series[0].label.visible = false;

			chart.tooltip.enabled = false;
		},
	});


	registerProductTotalController(sd.ProductTotalByType, {
		argumentField: se => se.TypeName,
		tagField: se => se.Type,
		rotated: true,

		members: se => [se.Rank, se.Type, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg, tag) =>
		{
			filterCtrl.modelValue(fse.Type, tag),

			sd.ProductTotalByMonth.openList();
		}
	});


	registerProductTotalController(sd.ProductTotalByProvider, {
		argumentField: se => se.ProviderName,
		tagField: se => se.Provider,
		rotated: true,

		members: se => [se.Rank, se.Provider, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg, tag) =>
		{
			filterCtrl.modelValue(fse.Provider, tag.Id),
			sd.ProductTotalByMonth.openList();
		}
	});


	registerProductTotalController(sd.ProductTotalBySeller, {
		argumentField: se => se.SellerName,
		tagField: se => se.Seller,
		rotated: true,

		members: se => [se.Rank, se.Seller, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg, tag) =>
		{
			filterCtrl.modelValue(fse.Seller, tag.Id),
			sd.ProductTotalByMonth.openList();
		}
	});


	registerProductTotalController(sd.ProductTotalByBooker, {
		argumentField: se => se.BookerName,
		tagField: se => se.Booker,
		rotated: true,

		members: se => [se.Rank, se.Booker, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg, tag) =>
		{
			filterCtrl.modelValue(fse.Booker, tag.Id),
			sd.ProductTotalByMonth.openList();
		}
	});


	registerProductTotalController(sd.ProductTotalByOwner, {
		argumentField: se => se.OwnerName,
		tagField: se => se.Owner,
		rotated: true,

		members: se => [se.Rank, se.Owner, se.Total, se.ServiceFee, se.GrandTotal, se.Note, ],

		onPointClick: (filterCtrl, arg, tag) =>
		{
			filterCtrl.modelValue(fse.Owner, tag.Id),
			sd.ProductTotalByMonth.openList();
		}
	});

}