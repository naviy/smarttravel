namespace Luxena.Components_
{

	export abstract class BaseStringBox<
		TOptions extends BaseStringBoxOptions,
		TOptionsSetter extends BaseStringBoxOptionsSetter<any>
	>
		extends FieldBox$<TOptions, TOptionsSetter>
	{
		_dataType = "string";

		setDataValue(data: any, value: any)
		{
			if (data)
				data[this._options.name] = $clip(value);
		}
	}

	export abstract class BaseStringBoxOptions extends FieldBoxOptions
	{
	}

	export abstract class BaseStringBoxOptionsSetter<TOptions extends BaseStringBoxOptions>
		extends FieldOptionsBoxSetter<TOptions>
	{
	}


	export class StringBox extends BaseStringBox<StringBoxOptions, StringBoxOptionsSetter>
	{
		constructor()
		{
			super();
			this._options = new StringBoxOptions();
			this._optionsSetter = new StringBoxOptionsSetter(this._options);
		}

		renderDisplay()
		{
			const value = this.dataValue();
			return !value ? null : $(`<span>${value}</span>`);
		}

		renderEditor()
		{
			const o = this._options;

			return $div().dxTextBox(<DevExpress.ui.dxTextBoxOptions>{
				value: this.dataValue(),
				placeholder: o.title,
			});
		}
	}

	export class StringBoxOptions extends BaseStringBoxOptions
	{
	}

	export class StringBoxOptionsSetter extends BaseStringBoxOptionsSetter<StringBoxOptions>
	{
	}

}