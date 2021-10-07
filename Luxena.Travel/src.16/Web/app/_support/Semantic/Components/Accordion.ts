module Luxena.Components
{

	export class Accordion extends TabControl
	{
		//static Accordion = new Accordion();

		render(container: JQuery)
		{
			this.renderTabs(container, "dxAccordion", <DevExpress.ui.dxAccordionOptions>{
				collapsible: true,
			});
		}
	}

}