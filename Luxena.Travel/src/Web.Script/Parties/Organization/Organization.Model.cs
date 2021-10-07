using System;
using System.Runtime.CompilerServices;

using jQueryApi;

using KnockoutApi;


namespace Luxena.Travel
{

	public class OrganizationModel : PartyModel
	{
		public OrganizationModel()
		{
			IsOrganization = true;

			InfoTitle = Res.OrganizationView_Info;

			InfoPrompt = Ko.DependentObservable<bool>(
				delegate
				{
					return Phone1.GetValue() == null && Phone2.GetValue() == null && Email1.GetValue() == null && Email2.GetValue() == null &&
						WebAddress.GetValue() == null && ActualAddress.GetValue() == null && LegalAddress.GetValue() == null && Code.GetValue() == null;
				});
		}

		[PreserveCase]
		public Observable<string> Code = Ko.Observable<string>();

		[PreserveCase]
		public ObservableArray<PartyDto> Departments = Ko.ObservableArray<PartyDto>();

		[PreserveCase]
		public ObservableArray<PersonDto> Employees = Ko.ObservableArray<PersonDto>();

		[PreserveCase]
		public Action<jQueryEvent> AddEmployee;

		[PreserveCase]
		public Action<jQueryEvent> AddDepartment;
	}

}