namespace Luxena.Components_
{

	export class FormField extends Component$<FormFieldOptions, FormFieldOptionsSetter>
	{
		private _fieldGetter: Semantic.Member | Component;
		private _field: Component;
		private _display: JQuery;


		constructor()
		{
			super();
			this._options = new FormFieldOptions();
			this._optionsSetter = new FormFieldOptionsSetter(this._options);
		}

		field(fieldGetter: Semantic.Member | Component)
		{
			this._fieldGetter = fieldGetter;
			return this;
		}


		//loadFromData(data: any): void
		//{
		//	this._field.loadFromData(data);

		//	if (this._display && !this._options.editMode)
		//		this._display.toggleClass("dx-state-invisible", !this._field.hasValue());
		//}

		//saveToData(data: any): void
		//{
		//	this._field.saveToData(data);
		//}

		//removeFromData(data: any): void
		//{
		//	this._field.removeFromData(data);
		//}


		loadOptions()
		{
			return this._field.loadOptions();
		}

		prepare()
		{
			const field = this._field = toComponent(this._fieldGetter);
			this.prepareItem(field);
			$append(this._options, field._options);

			this._renderOnRepaint = !this._options.editMode;
		}

		render()
		{
			return $logb(`${this.__name}.render()`, () =>
			{
				const o = this._options;

				const el = this._display = $div("dx-field");

				if (o.title)
					el.append(`<div class="dx-field-label">${o.title}</div>`);

				const valueEl = $div(o.editMode ? "dx-field-value" : "dx-field-value-static").appendTo(el);

				if (!this._field.renderTo(valueEl))
					return null;

				return el;
			});
		}

	}

	
	export class FormFieldOptions extends ComponentOptions
	{

	}
	

	export class FormFieldOptionsSetter extends ComponentOptionsSetter<FormFieldOptions>
	{

	}

}