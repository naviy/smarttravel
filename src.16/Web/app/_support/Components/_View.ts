namespace Luxena.Components_
{

	export class View
		extends Container$<ContainerOptions, ContainerOptionsSetter<ContainerOptions>, Component>
	{
		

		private _dataSets: DataSet[] = [];

		dataSet(source: Semantic.Entity)
		{
			const ds = new EntityDataSet(source);
			this._dataSets.push(ds);
			return ds;
		}

		prepare()
		{
			super.prepare();
			this._dataSets.forEach(a=> a.prepare());
		}

		//getRenderer()
		//{
		//	return (container: JQuery) =>
		//	{
		//		this.prepare();
		//		this.render(container);
		//		//this.loadFromData(null);
		//	}
		//}


		fieldSet()
		{
			return new FieldSet();
		}

		formField(field: Semantic.Member | Component, setter: (op: FormFieldOptionsSetter) => void)
		{
			return <FormField>new FormField().field(field).options(setter);
		}

		grid()
		{
			return new Grid();
		}

	}

}