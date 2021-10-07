namespace Luxena.Components_
{

	export class FieldBoxOptions extends ComponentOptions
	{
		allowFiltering: boolean;
		allowGrouping: boolean;
		allowSorting: boolean;
		isCalculated: boolean;
	}


	export class FieldOptionsBoxSetter<TOptions extends FieldBoxOptions>
		extends ComponentOptionsSetter<TOptions>
	{
		//calculate()
		//{
		//	super.calculate();

		//	if (!this.isCalculated)
		//	{
		//		this._allowFiltering = false;
		//		this._allowGrouping = false;
		//		this._allowSorting = false;
		//	}
		//}


		allowFiltering(value?: boolean)
		{
			this._options.allowFiltering = value !== false;
			return this;
		}

		allowGrouping(value?: boolean)
		{
			this._options.allowGrouping = value !== false;
			return this;
		}

		allowSorting(value?: boolean)
		{
			this._options.allowSorting = value !== false;
			return this;
		}

		isCalculated(value?: boolean)
		{
			this._options.isCalculated = value !== false;
			return this;
		}

	}
	
}