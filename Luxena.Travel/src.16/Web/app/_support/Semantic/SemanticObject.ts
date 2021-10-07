module Luxena
{
	export type SemanticTitle =
		string |
		SemanticObject<any>;// |
		//((model: any) => () => string);

	export type SemanticMembers<TEntity extends SemanticEntity> = any;
		//SemanticEntity |
		//SemanticObject<any>[] |
		//((se: TEntity) => SemanticObject<any>[]) |
		//{ [containerId: string]: SemanticObject<any>[] | SemanticObject<any> } |
		//((se: TEntity) => { [containerId: string]: SemanticObject<any>[] | SemanticObject<any> })
;


	export class SemanticObject<TObject>
	{
		_entity: SemanticEntity;
		_name: string;
		_names: string;

		_icon: string;
		_title: string;
		_titles: string;
		_title2: string;
		_title5: string;
		_description: string;
		_shortTitle: string;

		name(value: string)
		{
			this._name = value;
			return <TObject><any>this;
		}

		private static _unameIndex = 0;
		private _uname:string;

		uname()
		{
			return this._uname || (this._uname = this._name + "__" + SemanticObject._unameIndex++);
		}

		_localizeTitle(value: ILocalization)
		{
			this._title = value[Luxena.language] || value.ru || this._title;
			this._titles = value[Luxena.language + "s"] || value.rus || this._titles;
			this._title2 = value[Luxena.language + "2"] || value.ru2 || this._title2;
			this._title5 = value[Luxena.language + "5"] || value.ru5 || this._title5;
			this._description = value[Luxena.language + "Desc"] || value.ruDesc || this._description;
			this._shortTitle = value[Luxena.language + "Short"] || value.ruShort || this._shortTitle;
		}


		//#region Setters

		icon(value: string|SemanticObject<any>)
		{
			if (value instanceof SemanticObject)
				this._icon = value._icon || "sticky-note-o";
			else if (value === "props")
				this._icon = "sticky-note-o"; //"info"
			else
				this._icon = <any>value;

			return <TObject><any>this;
		}

		iconAndTitle(icon: string, title: string)
		{
			this._icon = icon;
			this._title = title;
			return <TObject><any>this;
		}

		localizeTitle(value: ILocalization)
		{
			this._localizeTitle(value);
			return <TObject><any>this;
		}

		ru(value: string)
		{
			this._localizeTitle({ ru: value });
			return <TObject><any>this;
		}

		//en(value: string)
		//{
		//	this._localizeTitle({ en: value });
		//	return this;
		//}

		//ua(value: string)
		//{
		//	this._localizeTitle({ ua: value });
		//	return this;
		//}

		title(title: SemanticTitle)
		{
			title = ko.unwrap(<any>title);


			if (!this._icon && title instanceof SemanticObject && title._icon)
				this._icon = title._icon;

			this._title = semanticTitleToString(title);

			return <TObject><any>this;
		}

		titleForList(se: SemanticEntity) 
		{
			this._icon = se._icon;
			this._title = se._titles;
			return <TObject><any>this;
		}

		titlePrefix(value: string)
		{
			this._title = (value || "") + (this._title || "");
			return <TObject><any>this;
		}

		titlePostfix(value: string)
		{
			this._title = (this._title || "") + (value || "");
			return <TObject><any>this;
		}

		description(value: string)
		{
			this._description = value;
			return <TObject><any>this;
		}

		//#endregion


		getIconHtml(icon?: string, withTitle?: boolean)
		{
			icon = icon || this._icon;
			return icon ? `<i class="fa fa-${icon} text-icon"${withTitle ? ` title="${this._title}"` : ""}></i>` : "";
		}

	}


	export interface ILocalization
	{
		en?: string; ens?: string; enDesc?: string; enShort?: string;
		ru?: string; rus?: string; ru2?: string; ru5?: string; ruDesc?: string; ruShort?: string;
		ua?: string; uas?: string; ua2?: string; ua5?: string; uaDesc?: string; uaShort?: string;
	}


	export function semanticTitleToString(title: SemanticTitle)
	{
		title = ko.unwrap(<any>title);

		if (title instanceof SemanticObject)
		{
			const so = <SemanticObject<any>>title;
			title = so._title;

			if ((!title || title === so._name) && so instanceof SemanticMember)
			{
				//$do(so._lookupGetter && so._lookupGetter(), a => title = a._title);

				//// ReSharper disable once ConditionIsAlwaysConst
				//if (!title)
				$do(so._collectionItemEntity && so._collectionItemEntity(), a => title = a._titles || a._title);
			}

		}

		return <string>title;
	}

}