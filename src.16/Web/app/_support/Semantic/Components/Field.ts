module Luxena
{

	export class Field extends SemanticComponent<Field>
	{
		_members: SemanticMembers<any>;
		_components: SemanticComponent<any>[];
		_type: SemanticFieldType;

		_dependencies: SemanticObject<any>[] = [];

		_widgetOptions: { [typeName: string]: Object; } = {};

		readonly = false;
		sortOrder: string;

		_isHidden: boolean;
		_isReserved: boolean;
		_isCompact: boolean;

		//_isCard: boolean;
		_isHeader1: boolean;
		_isHeader2: boolean;
		_isHeader3: boolean;
		_isHeader4: boolean;
		_isHeader5: boolean;
		_isImpotent: boolean;

		_useBorderLeft: boolean;
		_useBorderRight: boolean;
		_useBorderTop: boolean;
		_useBorderBottom: boolean;

		_groupIndex: number;

		_width: number;
		_height: number;

		_rowClasses: string[] = [];


		constructor(public _member: SemanticMember)
		{
			super();

			this._entity = _member._entity;
			this._name = _member._name;
			this._icon = _member._icon;
			this._title = _member._title;
			this._type = _member._type;

			_member.prepare();
		}


		//#region Setters

		items(...members: SemanticMembers<any>[])
		{
			this._members = members;
			return this;
		}

		dependencies(...value: SemanticObject<any>[])
		{
			if (value)
			{
				value.forEach(a =>
				{
					if (this._dependencies.indexOf(a) < 0)
						this._dependencies.push(a);
				});
			}

			return this;
		}

		addRowClass(...classes: string[])
		{
			const rowClasses = this._rowClasses;
			classes.forEach(cls => 
			{
				if (rowClasses.indexOf(cls) < 0)
					rowClasses.push(cls);
			});    

			return this;
		}

		hidden()
		{
			this._isHidden = true;
			this._columnVisible = false;
			this._selectRequired = true;
			return this;
		}

		reserved(value?: boolean)
		{
			this._isReserved = true;
			this._columnVisible = false;
			return this;
		}

		compact(value?: boolean)
		{
			this._isCompact = value !== false;
			return this;
		}
		//card(value?: boolean)
		//{
		//	this._isCard = value !== false;
		//	return this;
		//}

		header1(value?: boolean)
		{
			this._isHeader1 = value !== false;
			this._unlabel = true;
			return this;
		}

		header2(value?: boolean)
		{
			this._isHeader2 = value !== false;
			this._unlabel = true;
			return this;
		}

		header3(value?: boolean)
		{
			this._isHeader3 = value !== false;
			//this._useBorderBottom = true;
			this._unlabel = true;
			return this;
		}

		header4(value?: boolean)
		{
			this._isHeader4 = value !== false;
			this._unlabel = true;
			return this;
		}

		header5(value?: boolean)
		{
			this._isHeader5 = value !== false;
			this._unlabel = true;
			return this;
		}

		width(value: number)
		{
			this._width = value;
			return this;
		}

		height(value: number)
		{
			this._height = value;
			return this;
		}

		impotent(value?: boolean)
		{
			this._isImpotent = value !== false;
			return this;
		}


		borderLeft(value?: boolean)
		{
			this._useBorderLeft = value !== false;
			return this;
		}

		borderRight(value?: boolean)
		{
			this._useBorderRight = value !== false;
			return this;
		}

		borderTop(value?: boolean)
		{
			this._useBorderTop = value !== false;
			return this;
		}

		borderBottom(value?: boolean)
		{
			this._useBorderBottom = value !== false;
			return this;
		}

		groupIndex(value: number)
		{
			this._groupIndex = value;
			return this;
		}

		ungroup()
		{
			this._groupIndex = -1;
			return this;
		}

		//#endregion


		//#region Widget Options

		widgetOptions(typeName: string, options: Object)
		{
			const exists = this._widgetOptions[typeName];
			this._widgetOptions[typeName] = exists ? $.extend(exists, options) : options;
			return this;
		}


		chartController = (options: ChartControllerConfigExt) =>
			this.widgetOptions("ChartController", options);

		gridController = (options: GridControllerConfigExt) =>
			this.widgetOptions("GridController", options);

		textBox = (options: DevExpress.ui.dxTextBoxOptions) =>
			this.widgetOptions("dxTextBox", options);

		numberBox = (options: DevExpress.ui.dxTextBoxOptions) =>
			this.widgetOptions("dxNumberBox", options);
		

		//#endregion
		

		//#region Data & Model

		getSelectFieldNames(): string[]
		{
			const names = this._type.getSelectFieldNames(this);

			return names;
		}

		getExpandFieldNames(): string[]
		{
			return this._type.getExpandFieldNames(this);
		}


		loadFromData(model: any, data: any): void
		{
			this._type.loadFromData(this, model, data);
		}

		saveToData(model: any, data: any): void
		{
			this._type.saveToData(this, model, data);
		}

		removeFromData(data: any): void
		{
			this._type.removeFromData(this, data);
		}


		getModelValue(): any
		{
			const model = this._controller.model;
			return !model ? undefined : model[this._name];
		}

		setModelValue(value: any): void
		{
			const model = this._controller.model;
			if (model)
				this._type.setModel(model, this._name, value);
		}

		setModelValueDefault(value: any): void
		{
			const model = this._controller.model;
			if (model && !ko.unwrap(model[this._name]))
				this._type.setModel(model, this._name, value);
		}

		//#endregion


		isComposite()
		{
			return this._type._isComposite;
		}

		addItemsToController(action?: (sm: SemanticMember, sc: Field) => void)
		{
			const ctrl = this._controller;
			ctrl.fields.push(this);

			const comps = ctrl.components;

			this._dependencies.forEach(d =>
			{
				if (!d || comps.filter(a => a._name === d._name).length) return;

				if (isField(d))
					ctrl.addComponent(d, this, null);
				else if (d instanceof SemanticMember)
					ctrl.addComponent(d.hidden(), this, null);
			});
		}

		toGridColumns(): DevExpress.ui.dxDataGridColumn[]
		{
			return this._type.toGridColumns(this);
		}

		toGridTotalItems(): any[]
		{
			return this._type.toGridTotalItems(this);
		}

		renderDisplayStatic(container: JQuery, data)
		{
			this._type.renderDisplayStatic(this, container, data);
		}

		render(container: JQuery)
		{
			const sf = this;
			const sm = sf._member;

			if (sf._isHidden)
				return;

			//if (sm._required)
			//	sf.impotent();

			sf.renderField(container, {
				title: (sm._title || "") + (sm._description ? ": " + sm._description : ""),
				fieldLabel: sf._title,
				isReadOnly: sm._isReadOnly,
			});
		}

		renderField(container: JQuery, cfg: {
			title?: string;
			fieldLabel?;
			isReadOnly?: boolean;
		})
		{
			const sf = this;
			//const sm = sf._member;
			const type = sf._type;

			if (sf._mustPureRender)
			{
				container.addClass(this._rowClasses.join(" "));
				sf._type.pureRender(sf, container);
				return;
			}


			const title = this._title = cfg.title || this._title || "";
			let fieldLabel = cfg.fieldLabel || title || "";

			type.prerender(sf);

			var rowEl = $("<div>").addClass(this._rowClasses.join(" "));

			if (title)
				rowEl.attr("title", title);

			const useLabel = fieldLabel && !this._unlabel && !sf._labelAsHeader;
			if (!useLabel)
				rowEl.addClass("field-label-none");

			rowEl.addClass("dx-field");

			if (fieldLabel)
			{
				if (this._isImpotent)
					fieldLabel = `<b>${fieldLabel}</b>`;

				if (sf._labelAsHeader)
				{
					$("<div>")
						.addClass("dx-fieldset-header")
						.html(fieldLabel)
						.appendTo(rowEl);
				}
				else if (useLabel)
				{
					if (sf._hideLabel)
						rowEl.addClass("field-label-hide");
					else if (!sf._unlabel)
						$("<div>")
							.addClass("dx-field-label")
						//.addClass("field-label-indent")
							.addClass(sf._indentLabel ? "field-label-indent" : "")
							.html(fieldLabel)// + ":")
							.appendTo(rowEl);
				}
			}

			let valueEl = $("<div>")
				.addClass(sf._controller.editMode && !cfg.isReadOnly ? "dx-field-value" : "dx-field-value-static")
				.appendTo(rowEl);

			if (this._isHeader1)
				valueEl = $(`<h1>`).appendTo(valueEl);
			else if (this._isHeader2)
				valueEl = $(`<h2>`).appendTo(valueEl);
			else if (this._isHeader3)
				valueEl = $(`<h3>`).appendTo(valueEl);
			else if (this._isHeader4)
				valueEl = $(`<h4>`).appendTo(valueEl);
			else if (this._isHeader5)
				valueEl = $(`<h5>`).appendTo(valueEl);

			if (this._useBorderLeft)
				rowEl.addClass("field-border-left");
			if (this._useBorderRight)
				rowEl.addClass("field-border-right");
			if (this._useBorderTop)
				rowEl.addClass("field-border-top");
			if (this._useBorderBottom)
				rowEl.addClass("field-border-bottom");

			sf._type.render(sf, valueEl, rowEl);

			//if (sf._isCard || sm._isCard)
			//	rowEl = $(`<div class="card card-fieldset">`).append($(`<div class="dx-fieldset">`).append(rowEl));

			rowEl.appendTo(container);
		}


		getIconHtml(icon?: string, withTitle?: boolean)
		{
			return this._member.getIconHtml(this._icon, withTitle);
		}

		getLength()
		{
			const l = this._member.getLength();

			if (this._length)
				l.length = this._length;

			return l;
		}

		getWidth(length?: number): number
		{
			const sf = this;
			const sm = sf._member;

			if (sf._width)
				return sf._width;
			if (sm._width)
				return sm._width;

			if (!length)
				length = sm.getLength().length;

			if (!length)
				return undefined;

			if (length < 2)
				length = 2;

			const cfg = (<GridController>sf._controller).config;
			const type = sf._type;

			return (
				14
				+ Math.round((type && type.charWidth || SemanticFieldType.charWidth) * length)
				+ (cfg.useFilter && cfg.useFilterRow && type && type.allowFiltering && sm._allowFiltering ? 12 + (type.addColumnFilterWidth || 0) : 0)
			);
		}

	}


	export function isField(o: SemanticObject<any>): o is Field
	{
		return o instanceof Field;
	}

}