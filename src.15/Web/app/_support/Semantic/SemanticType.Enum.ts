module Luxena
{

	export class EnumSemanticType extends SemanticType
	{
		static Enum: EnumSemanticType = new EnumSemanticType();

		allowGrouping = false;
		addColumnFilterWidth = -8;


		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			if ($.isArray(value))
			{
				var filter = [];
				value.forEach(a =>
				{
					filter.push([sm._name, operation || "=", sm._enumType._getEdm(a)]);
					filter.push("or");
				});
				filter.pop();

				if (!filter.length) filter = undefined;

				
				return <any>filter;
			}
			else
				return [sm._name, operation || "=", sm._enumType._getEdm(value)];
		}


		loadFromData(sf: SemanticField, model: any, data: any): void
		{
			const sm = sf.member;

			if (!sf._controller.editMode || !sm._enumIsFlags)
			{
				let value = data[sf._name];

				if (sf._controller.editMode && sm._required && !value)
					value = sm._enumType._array[0].Id;

				this.setModel(model, sf, value);
			}
			else
			{
				let values = data[sf._name];

				values = !values ? [] : values.split(",").map(a => a.trim());

				this.setModel(model, sf, values);
			}
		}


		saveToData(sf: SemanticField, model: any, data: any): void
		{
			if (!sf.member._enumIsFlags)
			{
				super.saveToData(sf, model, data);
				return;
			}

			var values = ko.unwrap(model[sf._name]);

			data[sf._name] = values.join(", ");
		}


		toGridColumns(sf: SemanticField): DevExpress.ui.dxDataGridColumn[]
		{
			var sm = sf.member;

			var col = this.toStdGridColumn(sf);

			if (sm._enumIsFlags)
			{
				col.allowFiltering = false;
				col.allowGrouping = false;
				col.allowSorting = false;
				col.calculateCellValue = data => getEnumNames(sm._enumType, data[sm._name]);
			}
			else
			{
				col.lookup = {
					dataSource: sm._enumList || sm._enumType._array,
					valueExpr: "Id",
					displayExpr: "Name",
					allowClearing: sm._required,
				};

				col.cellTemplate = (cell, cellInfo) =>
					cell.html(getEnumNames(sm._enumType, cellInfo.value));

				//col.calculateCellValue = data => getEnumNames(sm._enumType, data[sm._name]);

				col.calculateGroupValue = data => sm._enumType._getEdm(data[sm._name]);

				col.groupCellTemplate = (cell: JQuery, cellInfo) =>
					cell.html(sm._title + ": &nbsp; " + getEnumNames(sm._enumType, cellInfo.value));
			}

			return [col];
		}

		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (!sf._controller.editMode)
				rowEl.attr("data-bind", "visible: r." + sf._name);

			valueEl.attr("data-bind", "html: Luxena.getEnumNames(Luxena." + sf.member._enumType._name + ", r." + sf._name + ")");
		}

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			var sm = sf.member;

			if (sm._enumIsFlags)
			{
				this.appendTextBoxEditor(sf, valueEl, "dxTagBox", <DevExpress.ui.dxTagBoxOptions>{
					values: sf.getModelValue(),
					items: sm._enumType._array,
					valueExpr: "Id",
					displayExpr: "Name",
					showClearButton: !sm._required,
				});
			}
			else
			{
				this.appendTextBoxEditor(sf, valueEl, "dxSelectBox", <DevExpress.ui.dxSelectBoxOptions>{
					value: sf.getModelValue(),
					dataSource: sm._enumType._array,
					valueExpr: "Id",
					displayExpr: "Name",
					showClearButton: !sm._required,
				});
			}
		}

	}


	export function getEnumNames(enumType, values)
	{
		if (!values) return "";

		if ($.isFunction(values))
			values = values();

		if (!values) return "";
		
		if (typeof values === "string")
			values = values.split(",");

		var names = [];

		for (var i = 0, value; value = values[i++]; )
		{
			value = value.trim();
			var item = enumType._items[value];

			if (item)
				names.push(item.TextIconHtml + item.Name);
			else
				names.push(value);
		}

		return names.join(", ") || "";
	}

}