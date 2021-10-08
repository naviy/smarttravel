module Luxena.Components
{

	export abstract class TabControl extends Container<TabControl>
	{
		_isCard: boolean;


		card(value?: boolean)
		{
			this._isCard = value !== false;
			return this;
		}

		renderTabs(container: JQuery, widgetClassName: string, options)
		{
			const ctrl = this._controller;
			const widgets = ctrl.widgets;


			let accEl = $(`<div data-bind="${widgetClassName}: widgets.${this.uname() }"></div>`);

			this._components.forEach((sc2, i) =>
			{
				var hdiv2 = isField(sc2) && sc2._type._gridMode ? `style="padding: 10px"`
					: sc2 instanceof TabControl ? `style="padding-top: 20px"`
					: `class="card-fieldset"`;

				var itemEl =
					$(`<div data-bind="with: $parent" ${hdiv2}>`).appendTo(
						$(`<div data-options="dxTemplate: { name: 'item${i}' }"></div>`)
							.appendTo(accEl)
					);

				sc2.unlabel();
				sc2._mustPureRender = true;

				sc2.render(itemEl);
			});

			options.items = this._components.map((tab, i) =>
			{
				const widget = widgets[tab.uname()];
				return {
					title: tab.otitle() || tab._title,
					badge: tab.badge(),
					icon: tab._icon ? "fa fa-" + tab._icon : "",
					template: "item" + i,
					visible: !widget || widget.valueVisible,
				}
			});
			
			widgets[this.uname()] = options;

			if (this._isCard)
				accEl = $(`<div class="card-accordion">`).append(accEl);

			container.append(accEl);
		}

	}

}