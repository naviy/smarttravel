namespace Luxena.Components_
{

	//export type Container = Container$<any, any>;


	export abstract class Container$<
		TOptions extends ContainerOptions,
		TOptionsSetter extends ContainerOptionsSetter<any>,
		TItem
		>
		extends Component$<TOptions, TOptionsSetter>
	{

		_itemsGetter: TItem[];
		_items: Component[];


		items(itemsGetter: TItem[])
		{
			this._itemsGetter = itemsGetter;
			return this;
		}


		//loadFromData(data: any): void
		//{
		//	this._items.forEach(a => a.loadFromData(data));
		//}

		//saveToData(data: any): void
		//{
		//	this._items.forEach(a => a.saveToData(data));
		//}

		//removeFromData(data: any): void
		//{
		//	this._items.forEach(a => a.removeFromData(data));
		//}


		loadOptions()
		{
			return DataSet.concatLoadOptions(this._items);
		}

		prepare()
		{
			const items = this._items = [];

			for (let item_ of this._itemsGetter)
			{
				const item = this.itemToComponent(item_);
				items.push(item);
				this.prepareItem(item);
			}
		}

		itemToComponent(item: TItem): Component
		{
			return toComponent(<any>item);
		}


		render()
		{
			if (!this._items.length) return null;

			const el = $div();

			const itemOptions = this._options.itemOptions;

			for (const item of this._items)
			{
				$extend(item._options, itemOptions);
				item.renderTo(el);
			}

			return el;
		}

	}


	export class ContainerOptions extends ComponentOptions
	{
		itemOptions: IComponentOptions = <any>{};
	}


	export class ContainerOptionsSetter<TOptions extends ContainerOptions>
		extends ComponentOptionsSetter<TOptions>
	{
		editMode(value?: boolean)
		{
			const o = this._options;
			o.itemOptions.editMode = o.editMode = value !== false;
			return this;
		}
	}


	export class Container extends Container$<
		ContainerOptions,
		ContainerOptionsSetter<ContainerOptions>,
		Semantic.Member | Component
		>
	{
	}

}