namespace Luxena.Components_
{

	export class ComponentOptions
	{
		//__component: Component;
		name: string;

		icon: string;
		title: string;
		titles: string;
		title2: string;
		title5: string;
		badge: string;
		description: string;
		shortTitle: string;

		//otitle: KnockoutComputed<string>;
		//titleGetter: (model) => any;

		//obadge: KnockoutComputed<string>;
		//badgeGetter: (model) => any;

		editMode: boolean;

		visible = true;
		columnVisible = true;
		selectRequired = true;
		length: number;
	}


	export interface IComponentOptions extends ComponentOptions
	{
	}


	export class ComponentOptionsSetter<TOptions extends ComponentOptions>
	{
		constructor(protected _options: TOptions) { }


		//calculate() { }

		//model()
		//{
		//	return this.__component._controller.model;
		//}

		name(value: string)
		{
			this._options.name = value;
			return this;
		}


		icon(value: string | SemanticObject<any>, title?: string)
		{
			const op = this._options;

			if (value instanceof SemanticObject)
				op.icon = value._icon || "sticky-note-o";
			else if (value === "props")
				op.icon = "sticky-note-o"; //"info"
			else
				op.icon = <any>value;

			if (title)
				op.title = title;

			return this;
		}

		_localizeTitle(value: ILocalization)
		{
			const op = this._options;
			const lng = Luxena.language;

			op.title = value[lng] || value.ru || op.title;
			op.titles = value[lng + "s"] || value.rus || op.titles;
			op.title2 = value[lng + "2"] || value.ru2 || op.title2;
			op.title5 = value[lng + "5"] || value.ru5 || op.title5;
			op.description = value[lng + "Desc"] || value.ruDesc || op.description;
			op.shortTitle = value[lng + "Short"] || value.ruShort || op.shortTitle;
		}

		localizeTitle(value: ILocalization)
		{
			this._localizeTitle(value);
			return this;
		}

		ru(value: string)
		{
			this._localizeTitle({ ru: value });
			return this;
		}

		en(value: string)
		{
			this._localizeTitle({ en: value });
			return this;
		}

		ua(value: string)
		{
			this._localizeTitle({ ua: value });
			return this;
		}

		title(title: string | Semantic.Member | Component)
		{
			const o = this._options;

			if (isString(title))
				o.title = title;
			else if (Semantic.isMember(title) || isComponent(title))
			{
				o.title = title._options.title;
				if (!o.icon)
					o.icon = title._options.icon;
			}

			return this;
		}

		titles(se: SemanticEntity)
		{
			const op = this._options;
			op.icon = se._icon;
			op.title = se._titles;
			return this;
		}

		//titlePrefix(value: string)
		//{
		//	this._title = (value || "") + (this._title || "");
		//	return <TObject><any>this;
		//}

		//titlePostfix(value: string)
		//{
		//	this._title = (this._title || "") + (value || "");
		//	return <TObject><any>this;
		//}

		//otitle(): KnockoutComputed<string>;
		//otitle(getter: (model) => any): TOptions;
		//otitle(getter?: (model) => any): any
		//{
		//	if (getter)
		//	{
		//		this._titleGetter = getter;
		//		return this;
		//	}

		//	if (!this._titleGetter)
		//		return this._title;

		//	if (!this._otitle)
		//		return this._otitle = ko.computed(() => ko.unwrap(this._titleGetter(this.model())));

		//	return this._otitle;
		//}


		badge(value: string)
		{
			this._options.badge = value;
			return this;
		}

		//obadge(): KnockoutComputed<string>;
		//obadge(getter: (model) => any): TOptions;
		//obadge(getter?: (model) => any): any
		//{
		//	if (getter)
		//	{
		//		this._badgeGetter = getter;
		//		return this;
		//	}

		//	if (!this._badgeGetter)
		//		return this._badge;

		//	if (!this._obadge)
		//		return this._obadge = ko.computed(() => ko.unwrap(this._badgeGetter(this.model())));

		//	return this._obadge;
		//}


		description(value: string)
		{
			this._options.description = value;
			return this;
		}


		editMode(value?: boolean)
		{
			this._options.editMode = value !== false;
			return this;
		}

		visible(value?: boolean)
		{
			this._options.visible = value !== false;
			return this;
		}

		columnVisible(value?: boolean)
		{
			this._options.columnVisible = value !== false;
			return this;
		}

		length(value: number)
		{
			this._options.length = value;
			return this;
		}

	}

}