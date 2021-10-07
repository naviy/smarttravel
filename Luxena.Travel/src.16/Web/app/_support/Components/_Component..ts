namespace Luxena.Components_
{

	export type Component = Component$<any, any>;


	export abstract class Component$<
		TOptions extends ComponentOptions,
		TOptionsSetter extends ComponentOptionsSetter<any>
		>
	{
		__name: string;

		_options: TOptions;
		protected _optionsSetter: TOptionsSetter;
		_dataSet: Components_.DataSet;

		protected _renderOnRepaint: boolean;
		protected _element: JQuery;
		protected _parent: JQuery;


		constructor()
		{
			this.__name = this.toString();
		}

		uname()
		{
			return this._uname ||
				(this._uname = (this["_name"] || "cmp") + "__" + Component$._unameIndex++);
		}
		private _uname: string;
		private static _unameIndex = 0;


		toString()
		{
			const name = this._options && this._options.name;
			const className = classNameOf(this);

			return name
				? `${name}: ${className}`
				: `${className}`;
		}

		options(setter: (op: TOptionsSetter) => void)
		{
			if (setter)
			{
				setter(this._optionsSetter);
				this.__name = this.toString();
			}

			return this;
		}

		appendOptions(o: TOptions)
		{
			if (o)
			{
				$append(this._options, o);
				this.__name = this.toString();
			}

			return this;
		}


		source(value: Components_.DataSet)
		{
			this._dataSet = value;
			value._components.register(this);
			return this;
		}


		//clone()
		//{
		//	const clone = Object.create(this.constructor.prototype);

		//	// ReSharper disable once MissingHasOwnPropertyInForeach
		//	for (let attr in Object.getOwnPropertyNames(this.constructor.prototype))
		//	{
		//		const value = this[attr];
		//		if (value !== undefined)
		//			clone[attr] = value;
		//	}

		//	return clone;
		//}

		//getLength()
		//{
		//	return { length: this._options.length, min: <number>undefined, max: <number>undefined };
		//}


		//#region Data & Model

		data()
		{
			const ds = this._dataSet;
			return !ds ? undefined : ds._current;
		}

		loadOptions(): IDataLoadOptions
		{
			return undefined;
		}

		filterExpr(value: any, operation?: string): [string, string, any]
		{
			return undefined;
		}

		load(): void
		{
		}

		save(): void
		{
		}

		//removeFromData(data: any): void
		//{
		//}

		hasValue(): boolean
		{
			return undefined;
		}


		//#endregion


		//#region Widgets

		toGridColumns(): DevExpress.ui.dxDataGridColumn[]
		{
			return [];
		}

		toGridTotalItems(): any[]
		{
			return [];
		}


		prepare() { }

		protected prepareItem(item: Component)
		{
			if (!item._dataSet)
				item._dataSet = this._dataSet;

			item.prepare();
		}


		repaint(parent?: JQuery)
		{
			$logb(`${this.__name}.repaint(${!!parent})`);
			const oldElement = this._element;
			if (!oldElement || this._renderOnRepaint)
			{
				if (!parent)
					parent = this._parent;
				else
					this._parent = parent;

				if (oldElement || parent)
				{
					const newElement = this.render();

					if (newElement)
						if (oldElement)
						{
							oldElement.replaceWith(newElement);
							$log(`replace`);
						}
						else
						{
							newElement.appendTo(parent);
							$log(`appendTo`);
						}
					else if (oldElement)
						oldElement.remove();

					this._element = newElement;
				}
			}
			else
				this.load();

			$loge();
		}

		renderTo(parent: JQuery): JQuery
		{
			this._parent = parent;
			const el = this._element = this.render();
			if (el)
			{
				el.appendTo(parent);
				$log(`appendTo`);
			}
			return el;
		}

		render(): JQuery
		{
			return null;
		}

		//refresh(): JQuery
		//{
		//	return null;
		//}

		//#endregion

	}


	export class InternalComponent extends Component$<ComponentOptions, ComponentOptionsSetter<ComponentOptions>>
	{
		constructor()
		{
			super();
			this._options = new ComponentOptions();
			this._optionsSetter = new ComponentOptionsSetter(this._options);
		}
	}


	export function isComponent(a): a is Component
	{
		return a instanceof Component$;
	}

	export function toComponent(item: Semantic.Member | Component): Component
	{
		if (Semantic.isMember(item))
			return item.fieldBox();

		return <Component>item;
	}

}