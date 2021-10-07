module Luxena
{

	export class DateSemanticType extends SemanticType
	{
		static Date: DateSemanticType = new DateSemanticType("dd.MM.yyyy", 10);
		static MonthAndYear: DateSemanticType = new DateSemanticType("monthAndYear", 10);
		static QuarterAndYear: DateSemanticType = new DateSemanticType("quarterAndYear", 10);
		static Year: DateSemanticType = new DateSemanticType("dd.MM.yyyy", 10);

		static DateTime: DateSemanticType = new DateSemanticType("dd.MM.yyyy H:mm", 15);
		static DateTime2: DateSemanticType = new DateSemanticType("dd.MM.yyyy H:mm:ss", 18);
		static Time: DateSemanticType = new DateSemanticType("H:mm", 5);
		static Time2: DateSemanticType = new DateSemanticType("H:mm:ss", 8);


		constructor(format: string, length: number)
		{
			super();

			this.format = format;
			this.length = length;
			this.charWidth = SemanticType.digitWidth;
			this.addColumnFilterWidth = length <= 10 ? 42 : 0;
			this.dataType = "date";
			this.chartDataType = "datetime";
		}


		loadFromData(sf: SemanticField, model: any, data: any): void
		{
			var value = data[sf._name];
			if (typeof value === "string")
			{
				value = value.replace(/T.+/, "");
				value = new Date(value);
			}

			this.setModel(model, sf, value);
		}


		toGridColumns(sf: SemanticField): DevExpress.ui.dxDataGridColumn[]
		{
			var se = sf._entity;
			var sm = sf.member;

			var col = this.toStdGridColumn(sf);

			if (se._isBig && sm._isEntityDate)
				col.groupIndex = 0;

			return [col];
		}

		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (!sf._controller.editMode)
				rowEl.attr("data-bind", "visible: r." + sf._name);

			var ref = sf.member.getReference();

			valueEl.attr("data-bind", "text: Globalize.format(r." + sf._name + "(), '" + this.format + "')");
		}

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl = $("<div>").appendTo(valueEl);

			let options = <DevExpress.ui.dxDateBoxOptions>{
				value: sf.getModelValue(),
				format: "date",
				formatString: this.format,
				showClearButton: !sf.member._required || sf._controller.filterMode,
			};


			if (this.format === "monthAndYear")
			{
				options.formatString = "MMMM yyyy";
				options.maxZoomLevel = "year";
				options.minZoomLevel = "year";
			}


			this.appendEditor(sf, valueEl, "dxDateBox", options);
		}

	}

}