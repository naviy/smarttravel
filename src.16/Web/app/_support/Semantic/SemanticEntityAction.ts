module Luxena
{

	export interface EntityActionParams
	{
		view?: string | SemanticEntity;
		[index: string]: Object;
	}

	export type EntityActionExecuteEvent =
		string |
		SemanticEntity |
		((btn: Components.Button, prms) => void | JQueryPromise<any>);



	export class SemanticEntityAction extends SemanticObject<SemanticEntityAction>
	{
		_buttonType: string;
		_onExecute: EntityActionExecuteEvent;
		_navigateOptions;


		constructor(se: SemanticEntity)
		{
			super();

			this._entity = se;

			if (se)
			{
				this._names = this._name = se._name;
				this._icon = se._icon;
				this._title = se._shortTitle || se._title;
			}
		}
	

		//#region Setters

		onExecute(value: EntityActionExecuteEvent, navigateOptions?)
		{
			this._onExecute = value;

			if (navigateOptions)
				this._navigateOptions = navigateOptions;

			return this;
		}

		navigateOptions(value)
		{
			this._navigateOptions = value;
			return this;
		}

		normal()
		{
			this._buttonType = "normal";
			return this;
		}

		default()
		{
			this._buttonType = "default";
			return this;
		}

		success()
		{
			this._buttonType = "success";
			return this;
		}

		danger()
		{
			this._buttonType = "danger";
			return this;
		}

		//#endregion

		
		button()
		{
			return new Components.Button(this);
		}

	}

}