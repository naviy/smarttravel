module Luxena
{

	export class NumericSemanticType extends SemanticType
	{
		static Float: NumericSemanticType = new NumericSemanticType();
		static Int: NumericSemanticType = new NumericSemanticType();
		static Percent: NumericSemanticType = new NumericSemanticType();

		allowGrouping = false;
		length = 10;
		dataType = "number";


		toGridColumns(sf: SemanticField): DevExpress.ui.dxDataGridColumn[]
		{
			var sm = sf.member;

			var col = this.toStdGridColumn(sf);

			col.calculateCellValue = r => r[sm._name] || null;

			if (sm._precision)
			{
				col.format = "n" + (sm._precision || "");
				col.precision = sm._precision;
			}

			return [col];
		}

		toGridTotalItems(sf: SemanticField)
		{
			const sm = sf.member;
			if (!sm._useTotalSum) return [];

			return [{
				column: sm._name,
				summaryType: "sum",
				displayFormat: "{0}",
				valueFormat: "n" + (sm._precision || 0),
			}];
		}


		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			const sm = sf.member;

			if (!sf._controller.editMode)
				rowEl.attr("data-bind", "visible: r." + sf._name);

			$("<div style='max-width: 98px'></div>")
				.css("text-align", "right")
				.attr("data-bind", `text: Globalize.format(r.${sf._name}(), 'n${sm._precision || ""}')`)
				.appendTo(valueEl);
		}



		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl = $("<div style='max-width: 164px'></div>")
				.appendTo(valueEl);

			this.appendTextBoxEditor(sf, valueEl, "dxNumberBox", <DevExpress.ui.dxNumberBoxOptions>{
				value: sf.getModelValue(),
			});
		}


	}

}