module Luxena.Ui
{

	var moneyProgressCount = 0;

	export function moneyProgress(se: SemanticEntity, valueMember: SemanticMember, targetMember: SemanticMember)
	{
		if (!valueMember)
			throw Error("Не указан valueMember");
		if (!valueMember._isMoney)
			throw Error("valueMember должен быть типа Money");

		return fieldRow2(se, 
			(() =>
			{
				var displayTarget = ko.observable();
				var displayValue = ko.observable();
				var displayMinValue = ko.observable();
				var displayMaxValue = ko.observable();
				var displayColor = ko.observable();
				var displayCurrency = ko.observable();
				var displaySubvalues = ko.observable();

				return <ISemanticFieldRowConfig2<SemanticEntity>>{
					title: valueMember,
					members: () => [targetMember, valueMember.clone(<any>{ _selectRequired: true }), ],

					width: 150,

					cellTemplate: (sc, cell, cellInfo) =>
					{
						var r = cellInfo.data;

						var targetData = r[targetMember._name];
						var valueData = r[valueMember._name];

						var target: number = targetData && targetData.Amount;
						var value: number = valueData && valueData.Amount;


						var color = Math.abs(value - target) <= 0.01 ? "#82e583" : value >= target ? "#e55253" : undefined;

						cell
							.addClass("bullet")
							.dxBullet(<DevExpress.viz.sparklines.dxBulletOptions><any>
							{
								startScaleValue: Math.min(0, target, value),
								endScaleValue: Math.max(0, target, value),
								value: value,
								target: target,
								color: color,
								size: {
									width: 140,
									height: 25
								},
								tooltip:
								{
									enabled: false,
								}
							});

						//cell.text(totalAmount + " " + paidAmount);
					},

					loadFromData: (sc, model, data) =>
					{
						var targetData = data[targetMember._name];
						var valueData = data[valueMember._name];

						var target: number = targetData && targetData.Amount;
						var value: number = valueData && valueData.Amount;
						var color = Math.abs(value - target) <= 0.01 ? "#82e583" : value >= target ? "#e55253" : "#ebdd8f";

						//$alert(target);

						displayTarget(target);
						displayValue(value);
						displayMinValue(Math.min(0, target, value));
						displayMaxValue(Math.max(0, target, value));
						displayColor(color);
						displayCurrency(targetData && targetData.CurrencyId || "");
						displaySubvalues([target]);
					},


					renderDisplay: (sc, container) =>
					{
						var model = sc._controller.model;
						var name = "moneyProgress" + (moneyProgressCount++);

						model[name] = <DevExpress.viz.gauges.dxCircularGaugeOptions><any>
						{
							//geometry: { startAngle: 180, endAngle: 0 },
							scale: {
								startValue: displayMinValue,
								endValue: displayMaxValue,
								label: {
									format: "n0",
									//customizeText: arg => (arg.valueText + " " + displayCurrency())
								},
							},
							valueIndicator: {
								//type: 'rangebar',
								baseValue: 0
							},
							subvalueIndicator: {
								type: "textcloud",
								text: {
									format: "n0",
									customizeText: arg => (arg.valueText + " " + displayCurrency()),
								}
							},
							rangeContainer: {
								backgroundColor: displayColor,
							},
							value: displayValue,
							subvalues: displaySubvalues,
						};


						$("<div>")
							.height(200)
							.attr("data-bind", "dxCircularGauge: r." + name)
							.appendTo(container);
					},


					//renderDisplay1: (sc, container) =>
					//{
					//	var model = sc.controller.model;
					//	var name = "moneyProgress" + (moneyProgressCount++);

					//	model[name] = <DevExpress.viz.gauges.dxLinearGaugeOptions><any>
					//	{
					//		geometry: { orientation: 'vertical' },
					//		scale: {
					//			startValue: displayMinValue,
					//			endValue: displayMaxValue,
					//			label: {
					//				format: "n0",
					//				customizeText: arg => (arg.valueText + " " + displayCurrency())
					//			}
					//		},

					//		valueIndicator: {
					//			baseValue: 0,
					//			color: displayColor,
					//		},
					//		rangeContainer: {
					//			palette: 'pastel',
					//			//ranges: [
					//			//	{ startValue: 50, endValue: 90 },
					//			//	{ startValue: 90, endValue: 130 },
					//			//	{ startValue: 130, endValue: 150 },
					//			//]
					//		},
					//		title: {
					//			text: valueMember._title,
					//			//font: { size: 16 }
					//		},
					//		value: displayValue
					//	};


					//	$("<div>")
					//		//.height(200)
					//		.attr("data-bind", "dxLinearGauge: r." + name)
					//		.appendTo(container);
					//},
				}
			})()
			);
	}

}