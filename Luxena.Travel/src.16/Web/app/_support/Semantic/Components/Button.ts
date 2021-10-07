module Luxena.Components
{

	export class Button extends SemanticComponent<Button>
	{
		_getParams: (prms) => Object;

		_buttonType: string;
		_onExecute: EntityActionExecuteEvent;
		_navigateOptions;
		_onExecuteDone: (btn: Button, prms) => void;

		protected _items: (Button | SemanticEntityAction)[];
		protected _buttons: Button[];

		getId() { return this._controller.getId(); }

		protected _isSmall: boolean;
		protected _location: string;
		protected _template: (data, index: number, container: JQuery) => void;


		constructor(public _action?: SemanticEntityAction)
		{
			super();

			if (_action)
			{
				this._entity = _action._entity;
				
				this._name = _action._name + "Button";
				this._icon = _action._icon;
				this._title = _action._title;
				this._description = _action._description;

				this._buttonType = _action._buttonType;
				this._onExecute = _action._onExecute;
				this._navigateOptions = _action._navigateOptions;
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

		onExecuteDone(value: (btn: Button, prms) => void)
		{
			this._onExecuteDone = value;
			return this;
		}

		navigateOptions(value)
		{
			this._navigateOptions = value;
			return this;
		}

		items(...value: Buttons)
		{
			this._items = value;
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

		back()
		{
			this._buttonType = "back";
			this._title = "Назад";
			this._onExecute = () => app.back();
			return this;
		}


		small(value?: boolean)
		{
			this._isSmall = value !== false;
			return this;
		}

		location(value: string)
		{
			if (value === "left")
				value = "before";
			else if (value === "right")
				value = "after";

			this._location = value;
			return this;
		}

		left()
		{
			this._location = "before";
			return this;
		}

		right()
		{
			this._location = "after";
			return this;
		}

		template(value: (data, index: number, container: JQuery) => void)
		{
			this._template = value;
			return this;
		}

		//#endregion


		addItemsToController()
		{
			this._buttons = <any>this._controller.addComponents(this._items, this, null);
		}

		getExecuter(prms?: EntityActionParams)
		{
			prms = prms || {};

			if (this._getParams)
				$.extend(prms, this._getParams(prms));

			const onExecute = this._onExecute;

			if (!onExecute)
				return () => this.execODataAction(prms);
			else if ($.isFunction(onExecute))
				return () =>
				{
					var result = (<any>onExecute)(this, prms);
					if (this._onExecuteDone)
						if (result && $.isFunction(result.done))
							result.done(() => this._onExecuteDone(this, prms));
						else
							this._onExecuteDone(this, prms);
				};

			if (!prms.view)
			{
				if (typeof onExecute === "string")
					prms.view = onExecute;
				else if (isEntity(onExecute))
					prms.view = onExecute.editView();
			}
			else if (isEntity(prms.view))
				prms.view = (<SemanticEntity>prms.view).editView();

			const action = () => app.navigate(prms, this._navigateOptions);

			return action;
		}

		execODataAction(prms: EntityActionParams)
		{
			const ctrl = this._controller;
			const fctrl = ctrl instanceof FormController ? ctrl : null;
			const ectrl = ctrl instanceof EditFormController ? ctrl : null;

			if (ectrl)
				ectrl.isRecalculating = true;

			if (fctrl)
			{
				fctrl.loadingMessage(this._title + "...");
				fctrl.modelIsLoading(true);
			}

			const store = this._entity._store;
			let id = this.getId();
			let url = store["_byKeyUrl"](id || "") + "/Default." + this._name;

			const select = <any>fctrl.dataSourceConfig.select;
			const expand = ctrl.viewMode ? <any>fctrl.dataSourceConfig.expand : undefined;

			if (select && select.length)
				url += `?$select=${select.join(",").replace(",$usecalculated", "") }&$expand=${(expand || []).join(",") }`;

			var d = $.Deferred();

			const qprms = $.extend({}, prms);

			if ($.isFunction(qprms._delta))
				qprms._delta = qprms._delta();
			
			if (ectrl)
				qprms._delta = JSON.stringify(ectrl.saveToData());
			else if (ctrl.viewMode)
				qprms._save = true;

			$.when(store["_sendRequest"](url, "POST", null, qprms))
				.done(newData => d.resolve(qprms.id, newData))
				.done(data =>
				{
					if (fctrl && (ctrl.editMode || ctrl.viewMode))
						fctrl.loadFromData(data);
				})
				.fail(d.reject, <any>d)
				.fail(showError)
				.always(() =>
				{
					fctrl && fctrl.modelIsLoading(false);
					if (ectrl)
						ectrl.isRecalculating = false;
				});

			return d.promise();
		}


		render(container: JQuery)
		{
			const ctrl = this._controller;

			ctrl.widgets[this.uname()] = this.buttonOptions();

			container.append(`<div data-bind="dxButton: widgets.${this.uname() }">`);
		}


		buttonOptions(prms?: EntityActionParams)
		{
			return <DevExpress.ui.dxButtonOptions>{
				icon: this._icon,
				text: this._isSmall ? undefined : this._title,
				hint: this._title + ($as(this._description, a => ": " + a) || ""),
				type: this._buttonType || "normal",

				onClick: this.getExecuter(prms),
			};
		}

		dropDownMenuItemOptions(prms?: EntityActionParams)
		{
			return <DevExpress.ui.dxButtonOptions>{
				icon: this._icon,
				text: this._title,
				hint: this._title + ($as(this._description, a => ": " + a) || ""),
				onClick: this.getExecuter(prms),
			};
		}

		toolbarItemOptions(prms?: EntityActionParams): any
		{
			const options: any = {
				location: this._location,
			};

			if (this._template)
			{
				options.template = this._template;
			}
			else if (this._buttons && this._buttons.length)
			{
				options.widget = "dropDownMenu";
				options.options = { items: this._buttons.map(a => a.dropDownMenuItemOptions(prms)), };
			}
			else
			{
				options.widget = "button";
				options.options = this.buttonOptions(prms);
			};

			return options;
		}
	}

}
