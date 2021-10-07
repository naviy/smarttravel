module Luxena
{

	export class SemanticEmptyFieldRow extends SemanticComponent<SemanticEmptyFieldRow>
	{
		render(container: JQuery)
		{
			container.append(`<div class="dx-field"><div class="dx-field-value-static">&nbsp;</div></div>`);
		}
	}

}


module Luxena.Ui
{

	export var emptyRow = () => new SemanticEmptyFieldRow();

}