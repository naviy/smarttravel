module Luxena
{

	export class TextAreaSemanticType extends SemanticType
	{

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			var sm = sf.member;

			this.appendTextBoxEditor(sf, valueEl, "dxTextArea", <DevExpress.ui.dxTextAreaOptions>{
				value: sf.getModelValue(),
				height: sm._lineCount ? <any>(sm._lineCount * 46 - 10) : undefined,
			});
		}

	}


	export class CodeTextAreaSemanticType extends TextAreaSemanticType
	{
		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (!sf._controller.editMode)
				rowEl.attr("data-bind", `visible: r.${sf._name}`);

			valueEl.attr("data-bind", `html: "<pre>" + r.${sf._name}() + "</pre>"`);
		}

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			var sm = sf.member;

			this.appendTextBoxEditor(sf, valueEl, "dxTextArea", <DevExpress.ui.dxTextAreaOptions>{
				value: sf.getModelValue(),
				height: sm._lineCount ? <any>(sm._lineCount * 64 - 16 - 12) : undefined,
			});
		}
	}


	export class PasswordSemanticType extends SemanticType
	{

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			this.appendTextBoxEditor(sf, valueEl, "dxTextBox", <DevExpress.ui.dxTextBoxOptions>{
				value: sf.getModelValue(),
				maxLength: 20,
				mode: "password",
			});
		}

	}

	export class TextSemanticType extends SemanticType
	{
		static String: TextSemanticType = new TextSemanticType();
		static Text: TextSemanticType = new TextAreaSemanticType();
		static CodeText: TextSemanticType = new CodeTextAreaSemanticType();
		static Password: TextSemanticType = new PasswordSemanticType();


		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			return [sm._name, operation || "contains", value];
		}


		toGridColumns(sf: SemanticField): DevExpress.ui.dxDataGridColumn[]
		{
			var sm = sf.member;
			var se = sm._entity;

			var col = this.toStdGridColumn(sf);

			if (sm._isEntityName)
			{
				//var cellIconHtml = getCellIconHtml(se._icon);

				col.cellTemplate = (cell: JQuery, cellInfo) =>
				{
					if (cellInfo.column.groupIndex !== undefined) return;

					var data = <ISemanticRowData>cellInfo.data;

					var v = data[sf._name];
					if (!v) return;

					var id = data[se._referenceFields.id];

					$(`<a class="dx-link" href="#${data._viewEntity._name}/${id}"><b>${v}</b></a>`).appendTo(cell);
				};
			}

			return [col];
		}

		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (!sf._controller.editMode)
				rowEl.attr("data-bind", "visible: r." + sf._name);

			valueEl.addClass("pre");
			valueEl.attr("data-bind", "text: r." + sf._name);
		}

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			this.appendTextBoxEditor(sf, valueEl, "dxTextBox", <DevExpress.ui.dxTextBoxOptions>{
				value: sf.getModelValue(),
			});
		}

	}

}