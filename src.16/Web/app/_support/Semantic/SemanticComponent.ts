module Luxena
{

	export abstract class SemanticComponent<TComponent> extends SemanticObject<TComponent>
	{
		_controller: SemanticController;
		_containerId: string;
		_parent: SemanticComponent<any>;
		_otitleGetter: (model) => any;
		_otitle: KnockoutComputed<string>;
		_badgeGetter: (model) => any;
		_badge: KnockoutComputed<string>;

		_rowMode: boolean;

		_visible = true;
		_columnVisible = true;
		_selectRequired = true;
		_hideLabel: boolean;
		_hideLabelItems: boolean;
		_unlabel: boolean;
		_unlabelItems: boolean;
		_indentLabel: boolean;
		_indentLabelItems: boolean;
		_labelAsHeader: boolean;
		_labelAsHeaderItems: boolean;
		_mustPureRender: boolean;

		_length: number;

		_isComposite: boolean;

		clone(): TComponent
		{
			const clone = Object.create(this.constructor.prototype);

			for (let attr in this)
			{
				if (this.hasOwnProperty(attr))
					clone[attr] = this[attr];
			}
			
			return clone;
		}

		addItemsToController(action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void)
		{
		}

		loadFromData(model: any, data: any): void
		{
		}

		isComposite()
		{
			return this._isComposite;
		} 

		toGridColumns(): DevExpress.ui.dxDataGridColumn[]
		{
			return [];
		}

		toGridTotalItems(): any[]
		{
			return [];
		}

		abstract render(container: JQuery);

		renderDisplayStatic(container: JQuery, data)
		{
			this.render(container);
		}

		getLength()
		{
			return { length: this._length, min: <number>undefined, max: <number>undefined };
		}


		
		//toTab()
		//{
		//	return {
		//		title: this._title,
		//		template: this._name,
		//	};
		//}


		otitle(): KnockoutComputed<string>;
		otitle(getter: (model) => any): TComponent;
		otitle(getter?: (model) => any): any
		{
			if (getter)
			{
				this._otitleGetter = getter;
				return this;
			}

			if (!this._otitleGetter)
				return null;

			if (!this._otitle)
				this._otitle = ko.computed(() => ko.unwrap(this._otitleGetter(this._controller.model)));

			return this._otitle;
		}

		badge(): KnockoutComputed<string>;
		badge(getter: (model) => any): TComponent;
		badge(getter?: (model) => any): any {
			if (getter) {
				this._badgeGetter = getter;
				return this;
			}

			if (!this._badgeGetter)
				return null;

			if (!this._badge)
				this._badge = ko.computed(() => ko.unwrap(this._badgeGetter(this._controller.model)));

			return this._badge;
		}
		//#region Setters

		columnVisible(value?: boolean)
		{
			this._columnVisible = value !== false;
			return <TComponent><any>this;
		}

		hideLabel(value?: boolean)
		{
			this._hideLabel = value !== false;
			return <TComponent><any>this;
		}

		hideLabelItems(value?: boolean)
		{
			this._hideLabelItems = value !== false;
			return <TComponent><any>this;
		}

		indentLabel(value?: boolean)
		{
			this._indentLabel = value !== false;
			return <TComponent><any>this;
		}

		indentLabelItems(value?: boolean)
		{
			this._indentLabelItems = value !== false;
			return <TComponent><any>this;
		}

		labelAsHeader(value?: boolean)
		{
			this._labelAsHeader = value !== false;
			return <TComponent><any>this;
		}

		labelAsHeaderItems(value?: boolean)
		{
			this._labelAsHeaderItems = value !== false;
			return <TComponent><any>this;
		}

		length(value: number)
		{
			this._length = value;
			return <TComponent><any>this;
		}

		unlabel(value?: boolean)
		{
			this._unlabel = value !== false;
			return <TComponent><any>this;
		}

		unlabelItems(value?: boolean)
		{
			this._unlabelItems = value !== false;
			return <TComponent><any>this;
		}

		//#endregion

	}


	export interface ISemanticFieldComponentConfig2<TEntity extends ISemanticEntity>// extends ISemanticFieldRowConfig
	{
		name?: string;
		title?: SemanticTitle;
		members: SemanticMembers<TEntity>;
	}


	export function newFieldComponent<TEntity extends ISemanticEntity, TComponent extends SemanticComponent<any>>(
		se: TEntity,
		cfg: ISemanticFieldComponentConfig2<TEntity>,
		creater: () => TComponent
	): TComponent
	{

		if (cfg.name)
		{
			se.applyToThisAndDerived(dse =>
			{
				const sc = creater();

				sc._entity = dse;
				sc._name = cfg.name;
				sc.title(cfg.title);

				dse[cfg.name] = sc;
			});

			return null;
		}
		else
		{
			const sc = creater();

			sc._entity = se;
			sc.title(cfg.title);

			return sc;
		}
	}

}