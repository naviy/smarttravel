module Luxena
{

	export type SemanticEntityActionExecute =
	string |
	SemanticEntity |
	((prms?: any) => void | string | { view: string | SemanticEntity });

	export interface SemanticEntityActionExecuteParams
	{
		id: any;
		_delta?: any;
		_save?: boolean;
		_resync?: boolean;
		_select?: string[];
		_expand?: string[];
		_onExecuteDone?: (data: any) => void;
		[index: string]: any;
	}

	export class SemanticEntityAction extends SemanticObject<SemanticEntityAction>
	{
		_getParams: (prms) => Object;

		_buttonType: string;
		_onExecute: SemanticEntityActionExecute;
		_navigateOptions;


		constructor(se: SemanticEntity, getParams?: (prms: SemanticEntityActionExecuteParams) => Object)
		{
			super();

			this._entity = se;
			this._getParams = getParams;

			if (se)
			{
				this._names = this._name = se._name;
				this._icon = se._icon;
				this._title = se._shortTitle || se._title;
			}
		}
	

		//#region Setters

		onExecute(value: SemanticEntityActionExecute, navigateOptions?)
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

		
		getExecuter(prms?)
		{
			prms = prms || {};

			let onExecute = <any>ko.unwrap(this._onExecute);

			if (this._getParams)
				prms = this._getParams(prms) || prms || {};

			if (!onExecute)
				onExecute = () => this.exec(prms);
			else if ($.isFunction(onExecute))
				onExecute = onExecute(prms);

			if ($.isFunction(onExecute))
				return onExecute;

			if (!prms.view)
			{
				if (typeof onExecute === "string")
					prms.view = onExecute;
				else if (onExecute instanceof SemanticEntity)
					prms.view = onExecute.editView();
			}
			else if (prms.view instanceof SemanticEntity)
				prms.view = prms.view.editView();

			const action = () => app.navigate(prms, this._navigateOptions);

			return action;
		}

		exec(prms: SemanticEntityActionExecuteParams)
		{
			const store = this._entity._store;
			let url = store["_byKeyUrl"](prms.id || "") + "/Default." + this._name;

			const select = prms._select;
			if (select && select.length)
				url += `?$select=${select.join(",").replace(",$usecalculated", "")}&$expand=${(prms._expand || []).join(",")}`;

			var d = $.Deferred();

			if (typeof prms._delta !== "string")
				prms._delta = JSON.stringify(prms._delta);

			const onExecuteDone = prms._onExecuteDone;

			prms = $.extend({}, prms);

			delete prms.id;
			delete prms._select;
			delete prms._expand;
			delete prms._onExecuteDone;

			$.when(store["_sendRequest"](url, "POST", null, prms))
				.done(newData => d.resolve(prms.id, newData))
				.done(onExecuteDone)
				.fail(d.reject, <any>d)
				.fail(showError);

			return d.promise();
		}


		toButton(prms?: SemanticEntityActionExecuteParams)
		{
			const executer = this.getExecuter(prms);

			return <DevExpress.ui.dxButtonOptions>{
				icon: this._icon,
				text: this._title,
				hint: this._description,
				type: this._buttonType || "normal",

				onClick: executer,
				onExecute: executer,
			};
		}
	}

}