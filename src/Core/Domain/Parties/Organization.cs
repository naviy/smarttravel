using System.ComponentModel.DataAnnotations;

using Luxena.Domain;




namespace Luxena.Travel.Domain
{



	//===g






	[RU("Организация", "Организации")]
	public partial class Organization : Party
	{

		//---g



		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Organization> sm)
		{
			sm.For(a => a.Code)
				.RU("Код предприятия (ЕДРПОУ)")
				.MaxLength(10);
		}



		//---g



		public override PartyType Type => PartyType.Organization;


		[RU("Данная организация является Авиакомпанией")]
		public virtual bool IsAirline { get; set; }

		[RU("IATA код")]
		[MaxLength(2)]
		public virtual string AirlineIataCode { get; set; }

		[RU("Prefix код")]
		[MaxLength(3)]
		public virtual string AirlinePrefixCode { get; set; }

		[RU("Требование паспортных данных")]
		public virtual AirlinePassportRequirement AirlinePassportRequirement { get; set; }


		[RU("Для проживания")]
		public virtual bool IsAccommodationProvider { get; set; }

		[RU("Для автобусных билетов")]
		public virtual bool IsBusTicketProvider { get; set; }

		[RU("Для аренды авто")]
		public virtual bool IsCarRentalProvider { get; set; }

		[RU("Для ж/д билетов")]
		public virtual bool IsPasteboardProvider { get; set; }

		[RU("Для туров (готовых)")]
		public virtual bool IsTourProvider { get; set; }

		[RU("Для трансферов")]
		public virtual bool IsTransferProvider { get; set; }

		[RU("Для дополнительных услуг")]
		public virtual bool IsGenericProductProvider { get; set; }

		[RU("Данная организация является Провайдером услуг")]
		public virtual bool IsProvider => 
			IsAccommodationProvider || IsBusTicketProvider || IsCarRentalProvider || //IsInsuranceProvider ||
			IsPasteboardProvider || IsTourProvider || IsTransferProvider || IsGenericProductProvider;


		[RU("Данная организация является Страховой компанией")]
		public virtual bool IsInsuranceCompany { get; set; }

		[RU("Данная организация является Роуминг-оператором")]
		public virtual bool IsRoamingOperator { get; set; }



		//---g



		public virtual string ToAirlineString()
		{
			return $"{AirlineIataCode} - {Name}";
		}



		//---g

		

		public new class Service : Service<Organization>
		{

		}




		//---g

	}






	//===g



}