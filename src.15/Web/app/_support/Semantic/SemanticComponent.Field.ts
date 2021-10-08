module Luxena
{

	export class SemanticField extends SemanticComponent<SemanticField>
	{
		readonly = false;
		sortOrder: string;


		constructor(public member: SemanticMember)
		{
			super();

			this._entity = member._entity;
			this._name = member._name;

			member.prepare();
		}


		addItemsToController(ctrl: SemanticController, action?: (sm: SemanticMember, sc: SemanticField) => void)
		{
			ctrl.fields.push(this);
		}

		getSelectFieldNames(): string[]
		{
			var sm = this.member;

			var names = sm._type.getSelectFieldNames(this);

			//if (sm._dependencies)
			//	sm._dependencies.forEach(a =>
			//	{
			//		if (names.indexOf(a._name) < 0)
			//			names.push(a._name);
			//	});

			return names;
		}

		getExpandFieldNames(): string[]
		{
			return this.member._type.getExpandFieldNames(this);
		}


		loadFromData(model: any, data: any): void
		{
			this.member._type.loadFromData(this, model, data);
		}

		saveToData(model: any, data: any): void
		{
			this.member._type.saveToData(this, model, data);
		}

		removeFromData(data: any): void
		{
			this.member._type.removeFromData(this, data);
		}


		getModelValue(): any
		{
			var model = this._controller.model;
			return !model ? undefined : model[this._name];
		}

		setModelValue(value: any): void
		{
			var model = this._controller.model;
			if (model)
				this.member._type.setModel(model, this._name, value);
		}

		setModelValueDefault(value: any): void
		{
			var model = this._controller.model;
			if (model && !ko.unwrap(model[this._name]))
				this.member._type.setModel(model, this._name, value);
		}

		getFieldLabel()
		{
			return this.member._type.getFieldLabel(this);
		}

		getLength(): { length: number; min: number; max: number }
		{
			return this.member._type.getLength(this);
		}

		getWidth(length?: number): number
		{
			return this.member._type.getColumnWidth(this, length);
		}

	
		toGridColumns(): DevExpress.ui.dxDataGridColumn[]
		{
			return this.member._type.toGridColumns(this);
		}
		
		toGridTotalItems(): any[]
		{
			return this.member._type.toGridTotalItems(this);
		}

		render(container: JQuery)
		{
			const sf = this;
			const sm = sf.member;

			const rowEl = $("<div>")
				.addClass("dx-field");

			$("<div>")
				.addClass("dx-field-label")
				.attr("title", sm._title + (sm._description ? ": " + sm._description : ""))
				.text(sf.getFieldLabel())
				.appendTo(rowEl);

			var valueEl = $("<div>")
				.addClass(sf._controller.editMode && !sm._isReadOnly ? "dx-field-value" : "dx-field-value-static");

			sm._type.render(sf, valueEl, rowEl);

			valueEl.appendTo(rowEl);

			rowEl.appendTo(container);
		}

	}

}