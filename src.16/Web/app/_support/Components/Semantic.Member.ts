namespace Luxena.Semantic
{

	export type Member = Member$<any, any, any>;


	export class Member$<
		TComponent extends Components_.Component$<any, any>,
		TOptions extends Components_.ComponentOptions,
		TOptionsSetter extends Components_.ComponentOptionsSetter<any>
	>
	{
		_entity: Entity;

		constructor(
			public _options: TOptions,
			public _newFieldBox: () => TComponent
		) { }

		fieldBox(setter?: (o: TOptionsSetter) => void)
		{
			return <TComponent>this._newFieldBox()
				.options(setter)
				.appendOptions(this._options);
		}

		formField(
			formFieldSetter?: (o: Components_.FormFieldOptionsSetter) => void,
			fieldBoxSetter?: (o: TOptionsSetter) => void
		)
		{
			return <Components_.FormField>new Components_.FormField()
				.field(this.fieldBox(fieldBoxSetter))
				.options(formFieldSetter);
		}

	}


	export function isMember(a): a is Member$<any, any, any>
	{
		return a instanceof Member$;
	}
	
}