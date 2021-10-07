module Luxena.FieldTypes
{

	export class Lookup extends SemanticFieldType
	{

		static Reference = new Lookup();


		getSelectFieldNames(sf: Field): string[]
		{
			return sf._controller.editMode
				? [sf._name + "Id"]
				: [];
		}
		
		getExpandFieldNames(sf: Field): string[]
		{
			const sm = sf._member;
			const ref = sm.getLookupEntity();
			const refs = ref._lookupFields;

			if (sf._controller.editMode)
				return [];
			else
				return [`${sm._name}($select=${refs.id},${refs.name})`];
		}

		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			return [sm._name + "Id", "=", value];
		}


		loadFromData(sf: Field, model: any, data: any): void
		{
			const name = sf._name;
			let value: any = {};

			if (sf._controller.editMode)
			{
				let id = data[name + "Id"] || data[name];
				if (id && id.Id)
					id = id.Id;

				value = id;
			}
			else
			{
				const newValue = data[name];

				for (let prop in newValue)
				{
					if (!newValue.hasOwnProperty(prop)) continue;

					value[prop] = ko.observable(newValue[prop]);
				}
			}

			this.setModel(model, sf, value);
		}

		saveToData(sf: Field, model: any, data: any): void
		{
			const id = ko.unwrap(model[sf._name]);
			data[sf._name + "Id"] = id || null;
		}

		removeFromData(sf: Field, data: any): void
		{
			delete data[sf._name];
			delete data[sf._name + "Id"];
		}


		toGridColumns(sf: Field): DevExpress.ui.dxDataGridColumn[]
		{
			var sm = sf._member;
			const col = this.toStdGridColumn(sf);
			var ref = sm.getLookupEntity();

			var refs = ref._lookupFields;
			col.dataField += "." + refs.name;

			col.calculateFilterExpression = (filterValue, selectedFilterOperation) =>
				[sm._name + "." + refs.name, selectedFilterOperation || "contains", filterValue];

			col.cellTemplate = (cell: JQuery, cellInfo) =>
			{
				if (cellInfo.column.groupIndex !== undefined) return;

				this.renderDisplayStatic(sf, cell, cellInfo.data, ref);
			}

			col.groupCellTemplate = (cell: JQuery, cellInfo) =>
			{
				if (cellInfo.data.items && cellInfo.data.items.length)
				{
					const v = cellInfo.data.items[0][sf._name];
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


		renderDisplayStatic(sf: Field, container: JQuery, data, ref?: SemanticEntity)
		{
			const value = data[sf._name];
			if (!value) return;

			renderReferenceDisplay(sf, container, value, ref);
		}

		getDisplayValueVisible(sf: Field, model)
		{
			return () =>
			{
				var value = model[sf._name];
				return value && value().Id;
			};
		}

		renderDisplayVisible(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			rowEl.attr("data-bind", `visible: r.${sf._name} && r.${sf._name}().Id`);
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const ctrl = sf._controller;
			ctrl.widgets[sf.uname()] = {
				renderer: referenceDisplayRendererByData(sf, ctrl.model),
			}

			$("<span>")
				.attr("data-bind", `renderer: widgets.${sf.uname()}.renderer`)
				.appendTo(valueEl);
		}

		
		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const sm = sf._member;
			const ref = sm.getLookupEntity();
			const refs = ref._lookupFields;

			var defaultIconHtml = ref._textIconHtml;

			let itemTemplate;
			if (ref._lookupItemTemplate)
			{
				itemTemplate = (data, index, container) =>
				{
					var result = ref._lookupItemTemplate(data, index, container);
					if (typeof result === "string" && result.indexOf("<") >= 0)
						container.html(result);
				};
			}
			else
			{
				itemTemplate = (r, index, itemContainer: JQuery) => itemContainer.html(r._iconHtml + r[refs.name]);
			}

			const options = <any>{
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
				options.title = sm._title;
				options.showPopupTitle = false;
				options.cleanSearchOnOpening = false;
				options.pageLoadMode = "scrollBottom";

				this.appendTextBoxEditor(sf, valueEl, "dxLookup", options);
			}
		}

	}


	export function referenceDisplayRendererByData(sf: Field, data)
	{
		return container =>
		{
			if (!data) return;

			const value = ko.unwrap(data[sf._name]);
			renderReferenceDisplay(sf, container, value);
		};
	}

	export function renderReferenceDisplay(sf: Field, container: JQuery, v, ref?: SemanticEntity)
	{
		if (!v) return;

		ref = ref || sf._member.getLookupEntity();
		if (!ref) return;

		var ref2 = sd.entityByOData(v, ref);
		const refs = ref2._lookupFields;

		var id = ko.unwrap(v[refs.id]);
		let name = ko.unwrap(v[refs.name]);
		if (!id && !name) return;

		const iconHtml = (ref2 ? ref2._textIconHtml : null) || ref._textIconHtml;

		if (sf._member._kind === SemanticMemberKind.Important)
			name = `<b>${name}</b>`;

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

}