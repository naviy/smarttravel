module Luxena
{

	export class BoolSemanticType extends SemanticType
	{
		static Bool: BoolSemanticType = new BoolSemanticType();
	
		dataType = "boolean";
		nullable = false;

		loadFromData(sf: SemanticField, model: any, data: any): void
		{
			this.setModel(model, sf, !!data[sf._name]);
		}


		getFieldLabel(sf: SemanticField)
		{
			return "";
		}

		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (!sf._controller.editMode)
				rowEl.attr("data-bind", `visible: r.${sf._name}`);

			valueEl.append($(
				`<div data-bind="dxCheckBox: { value: r.${sf._name}, text: '${sf.member._title}', readOnly: true, }"></div>`
			));

		}

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			this.appendEditor(sf, valueEl, "dxCheckBox", <DevExpress.ui.dxCheckBoxOptions>{
				value: sf.getModelValue(),
				text: sf.member._title,
			});
		}
			
	}

}