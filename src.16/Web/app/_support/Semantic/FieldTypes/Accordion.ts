//module Luxena.FieldTypes
//{

//	export class Accordion extends CompositeFieldType
//	{

//		static Accordion = new Accordion();


//		prerender(sf: Field)
//		{
//			if (sf._unlabel === undefined)
//				sf._unlabel = true;
//		}

//		render(sf: Field, valueEl: JQuery, rowEl: JQuery)
//		{
//			const unlabelItems = sf._itemUnlabel || !sf._unlabel && !sf._labelAsHeader;

//			var mustPureRender = sf._controller.viewMode && unlabelItems;
//			if (mustPureRender)
//			{
//				rowEl.addClass("field-label-none");
//				valueEl.addClass("dx-field-value-static");
//			}

//			const accEl = $(`<div data-bind="dxAccordion: { collapsible: true, items: [{ title: '${sf._title || ""}'${sf._icon ? ", icon: 'fa fa-" + sf._icon : ""} }] }"></div>`);
//			var itemEl = $(`<div data-options="dxTemplate: { name: 'item' } "></div>`).appendTo(accEl);


//			sf._components.forEach(sc2 =>
//			{
//				if (sf._itemLabelAsHeader)
//					sc2.labelAsHeader();
//				else if (unlabelItems)
//				{
//					sc2.unlabel();
//					if (mustPureRender)
//						sc2._mustPureRender = true;
//				}

//				sc2.render(itemEl);
//			});

//			valueEl.append(accEl);
//		}
//	}

//}