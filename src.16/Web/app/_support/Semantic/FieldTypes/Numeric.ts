module Luxena.FieldTypes
{

	export class Numeric extends SemanticFieldType
	{
		static Float = new Numeric();
		static Int = new Numeric();
		static Percent = new Numeric();

		allowGrouping = false;
		length = 10;
		dataType = "number";
		chartDataType = "numeric";


		initMember(sm: SemanticMember)
		{
			if (!sm._format)
				sm._format = "n";
		}

		toGridColumns(sf: Field): DevExpress.ui.dxDataGridColumn[]
		{
			var sm = sf._member;

			const col = this.toStdGridColumn(sf);

			col.calculateCellValue = r => r[sm._name] || null;

			if (sm._precision)
			{
				col.format = "n" + (sm._precision || "");
				col.precision = sm._precision;
			}

			return [col];
		}

		toGridTotalItems(sf: Field)
		{
			const sm = sf._member;
			if (!sm._useTotalSum) return [];

			return [{
				column: sm._name,
				summaryType: "sum",
				displayFormat: "{0}",
				valueFormat: "n" + (sm._precision || 0),
			}];
		}


		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			let value = data[sf._name];
			if (!value) return;

			value = Globalize.format(ko.unwrap(value), "n" + (sf._member._precision || ""));
			if (sf._member._kind === SemanticMemberKind.Important)
				value = `<b>${value}</b>`;

			container.append(
				//`<div style="max-width: 98px; text-align: right">${sf._member.getIconHtml()}${value}</div>`
				`${sf._member.getIconHtml()}${value}`
			);
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const iconHtml = sf._member.getIconHtml();
			if (iconHtml)
				valueEl.append(
					`<div style="max-width: 98px; text-align: right" data-bind="html: '${iconHtml}' + Globalize.format(r.${sf._name}(), 'n${sf._member._precision || ""}')"> </div>`
				);

			else
			valueEl.append(
				`<div style="max-width: 98px; text-align: right" data-bind="text: Globalize.format(r.${sf._name}(), 'n${sf._member._precision || ""}')"></div>`
			);
		}


		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl = $("<div style='max-width: 164px'></div>")
				.appendTo(valueEl);

			this.appendTextBoxEditor(sf, valueEl, "dxNumberBox", <DevExpress.ui.dxNumberBoxOptions>{
				value: sf.getModelValue(),
			});
		}

	}

}