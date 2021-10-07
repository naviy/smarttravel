module Luxena.Layouts
{

	var layoutName = "agent";


	var layoutSets = DevExpress.framework.html.layoutSets;
	layoutSets[layoutName] = layoutSets[layoutName] || [];

	layoutSets[layoutName].push({
		platform: "generic",
		controller: new DevExpress.framework.html["DefaultLayoutController"]({
			name: layoutName,
		})
	});


	function renderLeftToolMenuItem(data: any, index, containerEl: JQuery)
	{
		containerEl.append($(`
<div class="main-menu-item single brd-right" title="${data.title || data.text || ""}">
	<i class="fa fa-${data.icon} fa-4x"></i>
</div>`));
	}

	function renderMainMenuItem(data: any, index, containerEl: JQuery)
	{
		containerEl.append($(`
<div class="main-menu-item"${data.action ? ` data-bind="dxAction: '#${data.action}'"` : ""}>
	<i class="fa fa-${data.icon} fa-3x"></i>
	<h4${data.description ? "" : ` class="title-only"`}>${data.title || data.text}</h4>
	<span>${data.description || ""}</span>
</div>`));
	}

	function renderMainMenuSubItem(data: any, index, containerEl: JQuery)
	{
		containerEl.append($(`
<div class="main-menu-subitem single">
	<i class="fa fa-${data.icon} fa-2x"></i>
	<h4>${data.title || data.text}</h4>
</div>`));
	}

	function renderToolMenuItem(data, index, containerEl: JQuery)
	{
		containerEl.append($(`
<div class="main-menu-item ${data.items ? "" : " single"} brd-left" title="${data.title || data.text || ""}">
	<i class="fa fa-${data.icon} fa-3x"></i>
</div>`));
	}


	export function getToolMenuOptions(items: MenuItemOptions[], itemTemplate?, subItemTemplate?)
	{
		if (items)
		{
			items.forEach(a =>
			{
				if (!a.items) return;
				a.items.forEach(b => b.template = b.template || subItemTemplate || itemTemplate || renderMainMenuItem);
			});
		}

		return <DevExpress.ui.dxMenuOptions>{
			dataSource: items,
			cssClass: "main-menu",
			showFirstSubmenuMode: "onHover",
			showSubmenuMode: "onHover",
			hideSubmenuOnMouseLeave: true,

			itemTemplate: itemTemplate || renderToolMenuItem,

			onItemClick: e =>
			{
				if (e.itemData.url)
					app.navigate(e.itemData.url);
				else if (e.itemData.onExecute)
					if (typeof e.itemData.onExecute === "string")
						app.navigate(e.itemData.onExecute);
					else
						e.itemData.onExecute(e);
			},
		};
	}


	export function getMainMenuOptions(): DevExpress.ui.dxMenuOptions
	{
		var menus = $.map(Luxena.config.menu, menu =>
			$.extend({}, menu, {
				template: renderMainMenuItem,
				items: toMenuSubitems(menu.items, renderMainMenuSubItem),
			})
		);

		var titleMenu = {
			icon: "navicon",
			items: menus,
			template: renderLeftToolMenuItem,
		};

		return getToolMenuOptions([titleMenu]);
	}

	export function getBackMenuOptions(): DevExpress.ui.dxMenuOptions
	{
		var backMenu = {
			icon: "remove",
			title: "Закрыть текущую страницу и вернуться на предыдущую",
			//			template: renderLeftToolMenuItem,
			onExecute: () => app.back(),
		};

		return getToolMenuOptions([backMenu]);
	}


	export function getTitleMenuOptions(viewMenuItems): DevExpress.ui.dxMenuOptions
	{
		viewMenuItems = [
			{
				template: "title",
				items: viewMenuItems,
			}
		];

		return getToolMenuOptions(viewMenuItems, renderMainMenuSubItem);
	}

	export function getViewMenuOptions(viewMenuItems): DevExpress.ui.dxMenuOptions
	{
		return getToolMenuOptions(viewMenuItems, null, renderMainMenuSubItem);
	}

}