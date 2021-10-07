using System.Collections;

using LxnBase.Data;
using LxnBase.UI;

using Luxena.Travel.Services;


namespace Luxena.Travel
{
	public class OrganizationViewForm : PartyViewForm
	{
		public OrganizationViewForm(string tabId, object organizationId) : base(tabId, organizationId, ClassNames.Organization)
		{
			ListCaption = DomainRes.Organization_Caption_List;
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string) id, delegate(string tabId) { return new OrganizationViewForm(tabId, id); });
		}

		protected override PartyModel CreateModel()
		{
			return new OrganizationModel();
		}

		protected override void InitModel()
		{
			base.InitModel();

			((OrganizationModel) Model).AddEmployee = delegate { AddDetail(ClassNames.Person, "Organization"); };
			((OrganizationModel) Model).AddDepartment = delegate { AddDetail(ClassNames.Department, "Organization"); };
		}

		protected override void GetParty(object id)
		{
			PartyService.GetObject1(ClassNames.Organization, id, SetParty, delegate { Close(); });
		}
	}

}