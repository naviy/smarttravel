module Luxena
{

	export abstract class SemanticComponent<TComponent> extends SemanticObject<TComponent>
	{
		_controller: SemanticController;
		_containerId: string;

		_visible = true;
		_rowMode: boolean;


		clone(): TComponent
		{
			return $.extend({}, this);
		}

		addItemsToController(ctrl: SemanticController, action?: (sm: SemanticMember, sc: TComponent) => void)
		{
		}

		loadFromData(model: any, data: any): void
		{
		}


		toGridColumns(): DevExpress.ui.dxDataGridColumn[]
		{
			return [];
		}

		toGridTotalItems(): any[]
		{
			return [];
		}

		abstract render(container: JQuery);
		
	}

}