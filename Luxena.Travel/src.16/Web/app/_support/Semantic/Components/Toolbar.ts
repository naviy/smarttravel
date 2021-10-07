module Luxena.Components
{

	export type Buttons = (Button | SemanticEntityAction)[];


	export class Toolbar extends SemanticComponent<Toolbar>
	{
		_items: Buttons;
		_buttons: Button[];


		addItemsToController()
		{
			this._buttons = <any>this._controller.addComponents(this._items, this, null);
		}

		items(...value: Buttons)
		{
			this._items = value;
			return this;
		}

		render(container: JQuery)
		{
			this._controller.widgets[this.uname()] = <DevExpress.ui.dxToolbarOptions>{
				items: this._buttons.map(btn =>
				{
					return btn.toolbarItemOptions ? btn.toolbarItemOptions() : btn;
				}),
			};

			container.append(`<div data-bind="dxToolbar: widgets.${this.uname()}"></div>`);
		}

	}

}