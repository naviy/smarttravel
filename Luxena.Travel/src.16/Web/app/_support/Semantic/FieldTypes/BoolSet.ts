module Luxena.FieldTypes
{

	export class BoolSet extends CompositeFieldType
	{
		static BoolSet = new BoolSet();

		_isComposite = true;
		allowFiltering = false;
		allowGrouping = false;
		allowSorting = false;
		addColumnFilterWidth = -8;
	

		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			var html = [];

			if (sf._isCompact)
			{
				container
					.addClass("cell-boolset")
					.css("text-align", "right");

				sf._components.forEach(sc2 =>
				{
					if (!isField(sc2)) return;

					const value = data[sc2._name];
					if (!value) return;
					html.push(sc2.getIconHtml(null, true));
				});
			}
			else
			{
				html.push(`<div class="chips">`);

				sf._components.forEach(sc2 =>
				{
					if (!isField(sc2)) return;

					const value = data[sc2._name];
					if (!value) return;

					html.push(`<div class="chip">${sc2.getIconHtml()}${sc2._title}</div>`);
				});

				html.push(`</div>`);
			}

			container.append(html.join(""));
		}

		getDisplayValueVisible(sf: Field, model)
		{
			return () =>
			{
				var visible = false;

				sf._components.forEach(sc2 =>
				{
					if (isField(sc2))
						visible = visible || model[sc2._name] && model[sc2._name]();
				});

				return visible;
			};
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.addClass("chips");
			sf._components.forEach(sc2 =>
			{
				if (!isField(sc2)) return;
				$(`<div class="chip" data-bind="visible: r.${sc2._name}">${sc2.getIconHtml()}${sc2._title}</div>`)
					.appendTo(valueEl);
			});
		}

		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const sm = sf._member;

			var model = sf._controller.model;

			var values = ko.observableArray<SemanticMember>();
			sf._components.forEach(sc2 =>
			{
				if (!isField(sc2)) return;
				const sm2 = (<Field>sc2)._member;

				var mvalue = sm2.getModel(model);
				if (mvalue)
				{
					mvalue.subscribe(newValue =>
					{
						if (newValue)
						{
							const i = values.indexOf(sm2);
							if (i < 0)
								values.push(sm2);
						}
						else
						{
							//if (i >= 0)
							values.remove(sm2);
						}
					});
				}

				if (mvalue.peek())
					values.push(sm2);
			});


			values.subscribe<KnockoutArrayChange<SemanticMember>[]>(changes => changes.forEach(change =>
			{
				//$log(change.value._name, ko.unwrap(model[change.value._name]), change.status === "added");
				change.value.setModel(model, change.status === "added");
			}), null, "arrayChange");

			this.appendTextBoxEditor(sf, valueEl, "dxTagBox", <DevExpress.ui.dxTagBoxOptions>{
				values: <any>values,
				items: sm._members,
				//displayExpr: "_title",
				showClearButton: true,
				itemTemplate: (item: SemanticMember, index, container: JQuery) =>
					item.getIconHtml() + item._title,
				tagTemplate: (item: SemanticMember, index, container: JQuery) =>
					item.getIconHtml() + item._title,
			});
		}

	}
}