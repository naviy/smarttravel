using Ext;

using jQueryApi;

using LxnBase.UI;

using Luxena.Travel.Services;


namespace Luxena.Travel
{

	public class PersonViewForm : PartyViewForm
	{
		public PersonViewForm(string tabId, object personId) : base(tabId, personId, ClassNames.Person)
		{
			ListCaption = DomainRes.Person_Caption_List;
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string) id, delegate(string tabId) { return new PersonViewForm(tabId, id); });
		}

		protected override PartyModel CreateModel()
		{
			return new PersonModel();
		}

		protected override void GetParty(object id)
		{
			PartyService.GetPerson(id, SetParty, delegate { Close(); });
		}

		protected override void InitView()
		{
			base.InitView();

			View.Find(".pass-copy a")
				.Live("click",
					delegate(jQueryEvent e)
					{
						jQuery.FromElement(e.CurrentTarget).Next().FadeIn().Focus().Select();
					});

			View.Find(".pass-copy input")
				.Live("blur",
					delegate(jQueryEvent e)
					{
						jQuery.FromElement(e.CurrentTarget).FadeOut();
					})
				.Live("keydown",
					delegate(jQueryEvent e)
					{
						if (e.Which == EventObject.ESC || (e.Which == EventObject.C && e.CtrlKey))
							jQuery.FromElement(e.CurrentTarget).FadeOut();
					});
		}
	}

}