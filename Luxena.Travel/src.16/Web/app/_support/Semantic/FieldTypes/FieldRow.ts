module Luxena.FieldTypes
{

	export class FieldRow extends CompositeFieldType
	{

		static FieldRow = new FieldRow();

		_memberColumnVisible = true;
		

		toGridColumns(sf: Field): DevExpress.ui.dxDataGridColumn[]
		{
			return [];
		}

		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			container.addClass("field-row");

			sf._components.forEach(sc2 =>
			{
				sc2._rowMode = true;

				const span = $("<span>").appendTo(container);

				sc2.renderDisplayStatic(span, data);
			});

		}

		renderDisplay(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			var hasComposite = false;
			sf._components.forEach(sc2 => hasComposite = hasComposite ||
				sc2._isComposite || isField(sc2) && (sc2._type._isComposite)
			);

			// ReSharper disable once ConditionIsAlwaysConst
			if (hasComposite)
				this.renderEditor(sf, valueEl, rowEl);
			else
			{
				this.renderDisplayBind(sf, valueEl, rowEl);
				this.renderDisplayVisible(sf, valueEl, rowEl);
			}
		}


		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const multirows = sf._unlabel && sf._controller.viewMode;
			const tag = multirows ? `<div>` : `<span>`;

			if (!multirows)
				valueEl.addClass("field-row");


			sf._components.forEach(sc2 =>
			{
				sc2._rowMode = true;

				const span = $(tag).appendTo(valueEl);

				if (isField(sc2))
					sc2._type.render(sc2, span, span);
				else
					sc2.render(span);
			});
		}

		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			interface Column { sc: SemanticComponent<any>, length: number, width?: number } 

			const getMinColumnWidth = (col: Column) => isField(col.sc) ? 2 : 1;

			var cols: Column[] = [];
			var totalLength = 0;

			sf._components.forEach(sc2 =>
			{
				//if (sc2._parent !== sf) return;
				const length = sc2.getLength().length || 10;
				totalLength += length;
				cols.push({ sc: sc2, length: length });
			});
			if (!cols.length) return;

			var totalWidth = 0;
			
			cols.forEach(col =>
			{
				totalWidth += col.width = Math.max(getMinColumnWidth(col), Math.round(12 * col.length / totalLength));
			});

			var totalWidth2 = 0;
			cols.forEach(col =>
			{
				totalWidth2 += col.width = Math.max(getMinColumnWidth(col), Math.round(12 * col.width / totalWidth));
			});

			for (let i = 0; i < totalWidth2 - 12; i++)
			{
				var maxCol = cols[0];
				cols.forEach(col =>
				{
					// ReSharper disable ClosureOnModifiedVariable
					if (maxCol.width < col.width)
						maxCol = col;
					// ReSharper restore ClosureOnModifiedVariable
				});
				maxCol.width--;
			}

			var row = $(`<div class="row">`);

			cols.forEach(col =>
			{
				const cell = $(`<div class="col s${col.width}">`).appendTo(row);
				var sc2 = col.sc;
				sc2._rowMode = true;
				if (isField(sc2))
				{
					sc2._type.prerender(sc2);
					sc2._type.render(sc2, cell, rowEl);
				}
				else
					sc2.render(cell);
			});

			valueEl.append(row);
		}

	}

}