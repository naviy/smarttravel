module Luxena.FieldTypes
{

	export class DateTime extends SemanticFieldType
	{
		static Date = new DateTime("dd.MM.yyyy", 10);
		static MonthAndYear = new DateTime("monthAndYear", 10);
		static QuarterAndYear = new DateTime("quarterAndYear", 10);
		static Year = new DateTime("dd.MM.yyyy", 10);

		static DateTime = new DateTime("dd.MM.yyyy H:mm", 15);
		static DateTime2 = new DateTime("dd.MM.yyyy H:mm:ss", 18);
		static Time = new DateTime("H:mm", 5);
		static Time2 = new DateTime("H:mm:ss", 8);


		constructor(format: string, length: number)
		{
			super();

			this.format = format;
			this.length = length;
			this.charWidth = SemanticFieldType.digitWidth;
			this.addColumnFilterWidth = length <= 10 ? 42 : 0;
			this.dataType = "date";
			this.chartDataType = "datetime";
		}


		loadFromData(sf: Field, model: any, data: any): void
		{
			let value = data[sf._name];
			if (typeof value === "string")
			{
				value = value.replace(/T.+/, "");
				value = new Date(value);
			}

			this.setModel(model, sf, value);
		}


		toGridColumns(sf: Field): DevExpress.ui.dxDataGridColumn[]
		{
			const se = sf._entity;
			const sm = sf._member;
			const col = this.toStdGridColumn(sf);

			delete col.cellTemplate;

			if (col.groupIndex === undefined && se._isBig && sm._isEntityDate)
				col.groupIndex = 0;

			return [col];
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.attr("data-bind", "text: Globalize.format(r." + sf._name + "(), '" + this.format + "')");
		}

		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl = $("<div>").appendTo(valueEl);

			const options = <DevExpress.ui.dxDateBoxOptions>{
				value: sf.getModelValue(),
				format: "date",
				formatString: this.format,
				showClearButton: !sf._member._required || sf._controller.filterMode,
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