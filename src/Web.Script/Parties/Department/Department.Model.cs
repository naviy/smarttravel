using System.Runtime.CompilerServices;

using KnockoutApi;


namespace Luxena.Travel
{
	public class DepartmentModel : PartyModel
	{
		public DepartmentModel()
		{
			IsDepartment = true;

			InfoTitle = Res.DepartmentView_Info;

			InfoPrompt = Ko.DependentObservable<bool>(
				delegate
				{
					return Phone1.GetValue() == null && Phone2.GetValue() == null && Email1.GetValue() == null && Email2.GetValue() == null &&
						WebAddress.GetValue() == null && ActualAddress.GetValue() == null && LegalAddress.GetValue() == null;
				});

			Organization.SetValue(new OrganizationModel());
		}

		[PreserveCase]
		public Observable<OrganizationModel> Organization = Ko.Observable<OrganizationModel>();
	}
}