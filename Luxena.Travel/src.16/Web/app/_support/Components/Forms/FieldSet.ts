namespace Luxena.Components_
{

	//export type Container = Container$<any, any>;


	export class FieldSet extends Container$<
		FieldSetOptions,
		FieldSetOptionsSetter,
		Semantic.Member | Component | string
	>
	{
		//private _widget: JQuery;
		constructor()
		{
			super();

			this._options = new FieldSetOptions();
			this._optionsSetter = new FieldSetOptionsSetter(this._options);
		}

		prepare()
		{
			super.prepare();

			this._renderOnRepaint = !this._options.editMode;
		}

		itemToComponent(item)
		{
			if (Semantic.isMember(item))
				return new FormField().field(item);

			if (isString(item))
				return new FieldSetHeader().options(o => o.title(item));

			return item;
		}
		

		render()
		{
			return $logb(`${this.__name}.render()`, () =>
			{
				if (!this.data())
					return null;

				return super.render()
					.addClass("dx-fieldset");
			});
		}


		//loadFromData(data)
		//{
		//	const widget = this._widget;
		//	if (widget)
		//		widget.toggleClass("dx-state-invisible", !data);

		//	super.loadFromData(data);
		//}
	}

	
	export class FieldSetOptions extends ContainerOptions
	{

	}
	

	export class FieldSetOptionsSetter extends ContainerOptionsSetter<FieldSetOptions>
	{

	}

	

	class FieldSetHeader extends InternalComponent
	{
		render()
		{
			return $logb(`${this.__name}.render()`, () =>
			{
				const o = this._options;
				return $(`<div class="dx-fieldset-header">${o.title}</div>`);
			});
		}
	}

}