module Luxena.FieldTypes
{

	export class Enum extends SemanticFieldType
	{
		static Enum = new Enum();

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


		loadFromData(sf: Field, model: any, data: any): void
		{
			const sm = sf._member;

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


		saveToData(sf: Field, model: any, data: any): void
		{
			if (!sf._member._enumIsFlags)
			{
				super.saveToData(sf, model, data);
				return;
			}

			var values = ko.unwrap(model[sf._name]);

			data[sf._name] = values.join(", ");
		}


		toGridColumns(sf: Field): DevExpress.ui.dxDataGridColumn[]
		{
			var sm = sf._member;
			const col = this.toStdGridColumn(sf);

			if (sf._isCompact)
				col.alignment = "right";

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

				//col.cellTemplate = (cell, cellInfo) =>
				//	cell.html(getEnumNames(sm._enumType, cellInfo.value));

				//col.calculateCellValue = data => getEnumNames(sm._enumType, data[sm._name]);

				col.calculateGroupValue = data => sm._enumType._getEdm(data[sm._name]);

				col.groupCellTemplate = (cell: JQuery, cellInfo) =>
					cell.html(sm._title + ": &nbsp; " + getEnumNames(sm._enumType, cellInfo.value));
			}

			return [col];
		}

		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			let value = data[sf._name];
			if (!value) return;

			value = getEnumNames(sf._member._enumType, value, sf._isCompact);
			
			if (sf._member._kind === SemanticMemberKind.Important)
				value = `<b>${value}</b>`;

			container.addClass("nowrap");

			container.append(value);
		}

		getDisplayValueVisible(sf: Field, model)
		{
			var defValue = sf._member._enumType._array[0].Id;
			return () =>
			{
				var value = model[sf._name]();
				return value && value !== defValue;
			};
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.attr("data-bind", "html: Luxena.FieldTypes.getEnumNames(Luxena." + sf._member._enumType._name + ", r." + sf._name + ")");
		}

		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const sm = sf._member;

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


	export function getEnumNames(enumType, values, compact?: boolean)
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

			if (!item)
				names.push(value);
			else if (compact)
				names.push(item.TextIconHtml || item.Name);
			else
				names.push(item.TextIconHtml + item.Name);
		}

		return names.join(", ") || "";
	}

}