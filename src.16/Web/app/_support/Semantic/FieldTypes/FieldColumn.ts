module Luxena.FieldTypes
{

	export class FieldColumn extends CompositeFieldType
	{

		static FieldColumn = new FieldColumn();
		

		toGridColumns(sf: Field)
		{
			const sm = sf._member;

			var maxWidth = 0;

			sf._components.forEach(sc2 =>
			{
				if (isField(sc2))
				{
					const width = sc2.getWidth();
					if (width > maxWidth)
						maxWidth = width;
				}
			});

			const col = <DevExpress.ui.dxDataGridColumn>{
				allowFiltering: true,
				allowGrouping: false,
				allowSearch: true,
				allowSorting: false,
				caption: sf._title || sm._title || "",
				width: maxWidth,
				dataField: "Id",
			};

			col.cellTemplate = (cell: JQuery, cellInfo) =>
			{
				var data = cellInfo.data;
				sf._components.forEach(sc2 =>
				{
					const div = $(`<div title="${sc2._title}">`).appendTo(cell);
					sc2.renderDisplayStatic(div, data);
				});
			};

			col.calculateFilterExpression = (value, operation) =>
			{
				var filter = [];

				sf._components.forEach(sc2 =>
				{
					if (isField(sc2) && !sc2._type._isComposite)
					{
						const f = sc2._member.getFilterExpr(value, operation);
						if (f && f.length)
						{
							if (filter.length)
								filter.push("or");
							filter.push(f);
						}
					}
				});

				return filter;
			}

			return [col];
		}


		prerender(sf: Field)
		{
			if (sf._unlabel === undefined)
				sf._unlabel = true;
		}

		//pureRender(sf: Field, container: JQuery)
		//{
		//	const valueEl = sf._height
		//		? $(`<div data-bind="dxScrollView: { showScrollbar: 'always', height: ${sf._height} }">`)
		//		: $(`<div>`);

		//	sf._type.render(sf, valueEl, valueEl);

		//	valueEl.appendTo(container);
		//}

		render(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const unlabelItems = sf._unlabelItems || !sf._unlabel && !sf._labelAsHeader;
			
			var mustPureRender = sf._controller.viewMode && unlabelItems;
			if (mustPureRender)
			{
				rowEl.addClass("field-label-none");
				//valueEl.addClass("dx-field-value-static");
			}
			
			if (sf._height)
				valueEl = $(`<div data-bind="dxScrollView: { showScrollbar: 'always', height: ${sf._height} }">`)
					.appendTo(valueEl);

			sf._components.forEach(sc2 =>
			{
				if (sf._indentLabelItems)
					sc2.indentLabel();

				if (sf._labelAsHeaderItems)
					sc2.labelAsHeader();
				else if (unlabelItems)
				{
					sc2.unlabel();
					if (mustPureRender)
						sc2._mustPureRender = true;
				}
				else if (sf._hideLabelItems)
					sc2.hideLabel();

				sc2.render(valueEl);
			});

			if (!sf._controller.editMode)
				this.renderDisplayVisible(sf, valueEl, rowEl);
		}

	}

}