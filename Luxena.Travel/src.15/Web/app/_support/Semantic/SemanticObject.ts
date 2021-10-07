module Luxena
{

	export type SemanticTitle<TEntity extends SemanticEntity> =
		string |
		SemanticObject<any> |
		((se: TEntity) => SemanticObject<any>) |
		((data: any) => string);

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

		icon(value: string): TObject
		{
			this._icon = value;
			return <any>this;
		}

		localizeTitle(value: ILocalization): TObject
		{
			this._localizeTitle(value);
			return <any>this;
		}

		title(title: SemanticTitle<any>): TObject
		{
			title = ko.unwrap(<any>title);

			if ($.isFunction(title))
			{
				title = (<any>title)(this["_entity"] || this["entity"]);
			}

			if (title instanceof SemanticMember)
			{
				title = (<SemanticMember>title)._title;
			}

			this._title = <string>title;

			return <any>this;
		}

		description(value: string): TObject
		{
			this._description = value;
			return <any>this;
		}

		//#endregion

	}


	export interface ILocalization
	{
		en?: string; ens?: string; enDesc?: string; enShort?: string;
		ru: string; rus?: string; ru2?: string; ru5?: string; ruDesc?: string; ruShort?: string;
		ua?: string; uas?: string; ua2?: string; ua5?: string; uaDesc?: string; uaShort?: string;
	}

}