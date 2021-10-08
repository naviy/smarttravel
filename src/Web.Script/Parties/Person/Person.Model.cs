using System;
using System.Runtime.CompilerServices;

using KnockoutApi;


namespace Luxena.Travel
{
	public class PersonModel : PartyModel
	{
		public PersonModel()
		{
			IsPerson = true;

			InfoTitle = Res.PersonView_Personal_Info;

			InfoPrompt = Ko.DependentObservable<bool>(
				delegate
				{
					return 
						Phone1.GetValue() == null && Phone2.GetValue() == null && 
						Email1.GetValue() == null && Email2.GetValue() == null &&
						WebAddress.GetValue() == null && ActualAddress.GetValue() == null && 
						LegalAddress.GetValue() == null &&
						(MilesCards.GetItems() == null || MilesCards.GetItems().Count == 0) && 
						(Passports.GetItems() == null || Passports.GetItems().Count == 0);
				});

			Organization.SetValue(new OrganizationModel());
		}

		[PreserveCase]
		public Observable<Date> Birthday = Ko.Observable<Date>();

		[PreserveCase]
		public ObservableArray<PassportDto> Passports = Ko.ObservableArray<PassportDto>();

		[PreserveCase]
		public ObservableArray<MilesCardDto> MilesCards = Ko.ObservableArray<MilesCardDto>();
		
		[PreserveCase]
		public Observable<OrganizationModel> Organization = Ko.Observable<OrganizationModel>();

		[PreserveCase]
		public Observable<string> Title = Ko.Observable<string>();

		[PreserveCase]
		public Observable<string> BonusCardNumber = Ko.Observable<string>();
	}
}