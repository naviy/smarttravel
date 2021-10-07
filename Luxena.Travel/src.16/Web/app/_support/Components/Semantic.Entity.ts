namespace Luxena.Semantic
{

	export class Entity
	{
		//_name: string;
		_store: DevExpress.data.Store;

		stringMember(name: string, optionsGetter?: (o: Components_.StringBoxOptionsSetter) => void)
		{
			const o = new Components_.StringBoxOptions();
			o.name = name;

			const oSetter = new Components_.StringBoxOptionsSetter(o);
			optionsGetter && optionsGetter(oSetter);

			const sm = new Member$<
				Components_.StringBox,
				Components_.StringBoxOptions,
				Components_.StringBoxOptionsSetter
			>(
				o, () => new Components_.StringBox()
			);

			sm._entity = this;

			return sm;
		}

	}

}