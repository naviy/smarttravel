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


	function renderMainMenuItem(data: any, index, containerEl: JQuery)
	{
		containerEl.append($(`
<div class="main-menu-item"${data.action ? ` data-bind="dxAction: '#${data.action}'"` : ""} title="${data.title || data.text}">
	<i class="fa fa-${data.icon} fa-3x"></i>
</div>`));
		//<br><small>${data.title || ""}</small>
	}

	function renderMainMenuSubItem(data: any, index, containerEl: JQuery)
	{
		containerEl.append($(`
<div class="main-menu-subitem">
	<i class="fa fa-${data.icon} fa-2x"></i>
	<h4>${data.title || data.text}</h4>
</div>`));
	}

	function renderMainMenuCompactSubItem(data: any, index, containerEl: JQuery)
	{
		containerEl.append($(`
<div class="main-menu-subitem compact">
	<i class="fa fa-${data.icon}"></i>
	<h4>${data.title || data.text}</h4>
</div>`));
	}

	//export function getToolMenuOptions(items: IMenuItem[], itemTemplate?, subItemTemplate?)
	//{
	//		items.forEach(a =>
	//		{
	//			if (!a.items) return;
	//			a.items.forEach(b =>
	//			{
	//				b.template = b.template || subItemTemplate || itemTemplate || renderMainMenuSubItem;
	//			});
	//		});

	//	return <DevExpress.ui.dxMenuOptions>{
	//		dataSource: items,
	//		showFirstSubmenuMode: "onHover",
	//		showSubmenuMode: "onHover",
	//		hideSubmenuOnMouseLeave: true,

	//		itemTemplate: itemTemplate || renderMainMenuSubItem,

	//		onItemClick: e =>
	//		{
	//			var item = <IMenuItem>e.itemData;
	//			item.onClick && item.onClick(e);
	//		},
	//	};
	//}


	export function getMainMenuOptions(): DevExpress.ui.dxMenuOptions
	{
		const items = $.map(Luxena.config.menu, menu =>
		{
			const item = $.extend({}, menu, {
				template: renderMainMenuItem,
				items: toMenuItems(menu.items),
			});

			if (item.items)
				item.items.forEach(b => b.template = b.template ||
					(item.items.length <= 10 ? renderMainMenuSubItem : renderMainMenuCompactSubItem)
				);

			return item;
		});

		return <DevExpress.ui.dxMenuOptions>{
			dataSource: items,
			showFirstSubmenuMode: "onHover",
			showSubmenuMode: "onHover",
			hideSubmenuOnMouseLeave: true,
			orientation: "vertical",

			onItemClick: e =>
			{
				var item = <IMenuItem>e.itemData;
				item.onClick && item.onClick(e);
			},
		};
	}


	export function getFormAccordionToolbarOptions($data): DevExpress.ui.dxToolbarOptions
	{
		const se = <SemanticEntity>$data.entity;

		var icon = $data.icon || se && se._icon;
		var iconHtml = icon && `<i class="fa fa-${icon}"></i>&nbsp; ` || "";

		const items = [
			{
				location: "before",
				html: `<div class="dx-toolbar-label">${iconHtml}${$data.title || se && se._titles || ""}</div>`
			}
		];

		const viewItems = $data.scope && $data.scope.viewToolbarItems && $data.scope.viewToolbarItems();
		if (viewItems && viewItems.length > 2)
			items.push(...viewItems.slice(2));

		return { items: items, };
	}

}