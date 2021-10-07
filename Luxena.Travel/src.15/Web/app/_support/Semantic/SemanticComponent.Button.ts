module Luxena
{

	export class SemanticButton extends SemanticComponent<SemanticButton>
	{
		_name: string;

		constructor(public _action: SemanticEntityAction)
		{
			super();
			this._name = _action._name;
		}

		render(container: JQuery)
		{
			const ctrl = <FormController<any>>this._controller;

			const prms: SemanticEntityActionExecuteParams = {
				id: ctrl.getId(),
				_resync: ctrl.editMode || ctrl.viewMode,
				_save: ctrl.viewMode,
				_select: <any>ctrl.dataSourceConfig.select,
			};

			if (ctrl.viewMode)
				prms._expand = <any>ctrl.dataSourceConfig.expand;

			if (prms._resync)
				prms._onExecuteDone = data => ctrl.loadFromData(data);

			ctrl.widgets[this._name] = this._action.toButton(prms);

			container.append(`<div data-bind="dxButton: $root.widgets.${this._name}">`);
		}

	}

}


module Luxena.Ui
{

	export var emptyRow = () => new SemanticEmptyFieldRow();

}