module Luxena.FieldTypes
{

	export class Bool extends SemanticFieldType
	{
		static Bool: Bool = new Bool();
	
		dataType = "boolean";
		nullable = false;

		loadFromData(sf: Field, model: any, data: any): void
		{
			this.setModel(model, sf, !!data[sf._name]);
		}


		toGridColumns(sf: Field)
		{
			const col = this.toStdGridColumn(sf);
			delete col.cellTemplate;
			return [col];
		}


		prerender(sf: Field)
		{
			if (sf._hideLabel === undefined)
				sf._hideLabel = true;
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.append(
				`<div data-bind="dxCheckBox: { value: r.${sf._name}, text: '${sf._member._title}', readOnly: true, }"></div>`
			);
		}

		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			this.appendEditor(sf, valueEl, "dxCheckBox", <DevExpress.ui.dxCheckBoxOptions>{
				value: sf.getModelValue(),
				text: /*sf._member.getIconHtml() +*/ sf._member._title,
			});
		}
			
	}
}