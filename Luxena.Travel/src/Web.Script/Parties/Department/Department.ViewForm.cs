using LxnBase.UI;

using Luxena.Travel.Services;


namespace Luxena.Travel
{
	public class DepartmentViewForm : PartyViewForm
	{
		public DepartmentViewForm(string tabId, object departmentId) : base(tabId, departmentId, ClassNames.Department)
		{
			ListCaption = DomainRes.Department_Caption_List;
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string) id, delegate(string tabId) { return new DepartmentViewForm(tabId, id); });
		}

		protected override PartyModel CreateModel()
		{
			return new DepartmentModel();
		}

		protected override void GetParty(object id)
		{
			PartyService.GetDepartment(id, SetParty, delegate { Close(); });
		}
	}
}
