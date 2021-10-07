module Luxena
{

	export class LookupSemanticType extends SemanticType
	{

		static Reference: LookupSemanticType = new LookupSemanticType();

		isComplex = true;

		getSelectFieldNames(sf: SemanticField): string[]
		{
			return sf._controller.editMode
				? [sf._name + "Id"]
				: [];
		}
		
		getExpandFieldNames(sf: SemanticField): string[]
		{
			var sm = sf.member;
			var ref = sm.getReference();
			var refs = ref._referenceFields;

			if (sf._controller.editMode)
				return [];
			else
				return [`${sm._name}($select=${refs.id},${refs.name})`];
		}

		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			return [sm._name + "Id", "=", value];
		}


		loadFromData(sf: SemanticField, model: any, data: any): void
		{
			var name = sf._name;

			var value: any = {};

			if (sf._controller.editMode)
			{
				var id = data[name + "Id"] || data[name];
				if (id && id.Id)
					id = id.Id;

				value = id;
			}
			else
			{
				var newValue = data[name];

				for (var prop in newValue)
				{
					if (!newValue.hasOwnProperty(prop)) continue;

					value[prop] = ko.observable(newValue[prop]);
				}
			}

			this.setModel(model, sf, value);
		}

		saveToData(sf: SemanticField, model: any, data: any): void
		{
			var id = ko.unwrap(model[sf._name]);

			data[sf._name + "Id"] = id || null;
		}

		removeFromData(sf: SemanticField, data: any): void
		{
			delete data[sf._name];
			delete data[sf._name + "Id"];
		}


		toGridColumns(sf: SemanticField): DevExpress.ui.dxDataGridColumn[]
		{
			var sm = sf.member;
			const col = this.toStdGridColumn(sf);
			var ref = sm.getReference();

			var refs = ref._referenceFields;
			col.dataField += "." + refs.name;

			col.calculateFilterExpression = (filterValue, selectedFilterOperation) =>
				[sm._name + "." + refs.name, selectedFilterOperation || "contains", filterValue];

			col.cellTemplate = (cell: JQuery, cellInfo) =>
			{
				if (cellInfo.column.groupIndex !== undefined) return;

				renderReferenceDisplay(sf, cellInfo.data, cell, ref);
			}

			col.groupCellTemplate = (cell: JQuery, cellInfo) =>
			{
				if (cellInfo.data.items && cellInfo.data.items.length)
				{
					var v = cellInfo.data.items[0][sf._name];
					if (v && v[refs.id] && v[refs.name])
					{
						cell.html(`${sm._title}: <a href='#${ref._name}/${v[refs.id]}'>${v[refs.name]}</a>`);
						return;
					}
				}

				cell.html(sm._title + ": " + cellInfo.text);
			};

			return [col];
		}

		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (!sf._controller.editMode)
				rowEl.attr("data-bind", `visible: r.${sf._name} && r.${sf._name}().Id`);

			const span = $("<span>")
				.attr("data-bind", `renderer: Luxena.referenceDisplayRendererByData($element.sf, $data.r)`)
				.appendTo(valueEl);
			span[0]["sf"] = sf;
		}


		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			const sm = sf.member;
			const ref = sm.getReference();
			const refs = ref._referenceFields;

			var defaultIconHtml = ref._textIconHtml;

			let itemTemplate;
			if (ref._lookupItemTemplate)
			{
				itemTemplate = (data, index, container) =>
				{
					var result = ref._lookupItemTemplate(data, index, container);
					if (result)
					{
						if (typeof result === "string" && result.indexOf("<") >= 0)
							container.html(result);
						else
							return result;
					}
				};
			}
			else
			{
				itemTemplate = (r, index, itemContainer: JQuery) => itemContainer.html(r._iconHtml + r[refs.name]);
			}

			const options =<any>{
				value: sf.getModelValue(),
				dataSource: <any>{
					store: ref._lookupStore || ref._store,
					sort: refs.name,
					select: [refs.id, refs.name],
					map: r =>
					{
						var ref2 = sd.entityByOData(r, ref);
						r._iconHtml = (ref2 ? ref2._textIconHtml : null) || defaultIconHtml;
						return r;
					}
				},
				valueExpr: refs.id,
				displayExpr: refs.name,

				showClearButton: !sm._required,

				itemTemplate: itemTemplate,
			};


			if (ref._isSmall)
			{
				this.appendTextBoxEditor(sf, valueEl, "dxSelectBox", options);
			}
			else
			{
				options.dataSource.paginate = true;
				options.title = sm._title;
				options.showPopupTitle = false;
				options.cleanSearchOnOpening = false;

				this.appendTextBoxEditor(sf, valueEl, "dxLookup", options);
			}
		}
	}


	export function renderReferenceDisplay(sf: SemanticField, data, container: JQuery, ref?: SemanticEntity)
	{
		if (!data) return;

		const v = ko.unwrap(data[sf._name]);
		if (!v) return;

		ref = ref || sf.member.getReference();
		if (!ref) return;

		var ref2 = sd.entityByOData(v, ref);
		const refs = ref2._referenceFields;

		var id = ko.unwrap(v[refs.id]);
		const name = ko.unwrap(v[refs.name]);
		if (!id && !name) return;

		const iconHtml = (ref2 ? ref2._textIconHtml : null) || ref._textIconHtml;

		if (!id)
		{
			container.html(iconHtml + name);
		}
		else if (sf._controller.smartMode)
		{
			container.html(`<span>${iconHtml}<a href="#${ref2._name}/${id}" class="dx-link">${name}</a></span>`);
		}
		else
		{
			var span = $(`<span>${iconHtml}<a href="#${ref2._name}/${id}" class="dx-link">${name}</a></span>`);

			span.click(e =>
			{
				e.preventDefault();
				ref2.toggleSmart(span, { id: id });
			});

			span.appendTo(container);
		}
	}


	export function referenceDisplayRendererByData(sf: SemanticField, data)
	{
		return container => renderReferenceDisplay(sf, data, container);
	}

}