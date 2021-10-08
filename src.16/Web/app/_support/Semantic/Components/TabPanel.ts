module Luxena.Components
{

	export class TabPanel extends TabControl
	{
		//static TabPanel = new TabPanel();

		render(container: JQuery)
		{
			this.renderTabs(container, "dxTabPanel", <DevExpress.ui.dxTabPanelOptions>{
				collapsible: true,
				//animationEnabled: true,
			});
		}
	}

}