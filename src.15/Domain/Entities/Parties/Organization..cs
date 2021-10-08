using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Организация", "Организации"), Icon("group")]
	public partial class Organization : Party
	{

		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<Organization> sm)
		{
			sm.For(a => a.Code)
				.RU("Код предприятия (ЕДРПОУ)")
				.MaxLength(10);
		}

		public override PartyType Type => PartyType.Organization;


		[RU("Авиакомпания"), Icon(typeof(AviaDocument))]
		public bool IsAirline { get; set; }

		[RU("IATA код")]
		[MaxLength(2)]
		public string AirlineIataCode { get; set; }

		[RU("Prefix код")]
		[MaxLength(3)]
		public string AirlinePrefixCode { get; set; }

		public AirlinePassportRequirement AirlinePassportRequirement { get; set; }

		[RU("Провайдер проживания"), Icon(typeof(Accommodation))]
		public bool IsAccommodationProvider { get; set; }

		[RU("Провайдер автобусных билетов"), Icon(typeof(BusTicket))]
		public bool IsBusTicketProvider { get; set; }

		[RU("Провайдер аренды авто"), Icon(typeof(CarRental))]
		public bool IsCarRentalProvider { get; set; }

		[RU("Провайдер ж/д билетов"), Icon(typeof(Pasteboard))]
		public bool IsPasteboardProvider { get; set; }

		[RU("Провайдер турпакетов"), Icon(typeof(Tour))]
		public bool IsTourProvider { get; set; }

		[RU("Провайдер трансферов"), Icon(typeof(Transfer))]
		public bool IsTransferProvider { get; set; }

		[RU("Провайдер дополнительных услуг"), Icon(typeof(GenericProduct))]
		public bool IsGenericProductProvider { get; set; }

		[RU("Провайдером услуг")]
		public bool IsProvider { get; set; }

		[RU("Страховая компания"), Icon(typeof(InsuranceCompany))]
		public bool IsInsuranceCompany { get; set; }

		[RU("Роуминг-оператор"), Icon(typeof(RoamingOperator))]
		public bool IsRoamingOperator { get; set; }


		public virtual ICollection<MilesCard> MilesCards { get; set; }

		public virtual ICollection<Department> Departments { get; set; }
		public int DepartmentCount => Departments?.Count ?? 0;

		[RU("Сотрудники")]
		public virtual ICollection<Person> Employees { get; set; }
		public int EmployeeCount => Employees?.Count ?? 0;

		public virtual ICollection<AirlineServiceClass> AirlineServiceClasses { get; set; }

		//public virtual ICollection<Product> Products_Producer { get; set; }

		//public virtual ICollection<Product> Products_Provider { get; set; }


		public string ToAirlineString() => 
			$"{AirlineIataCode} - {Name}";


		public override void Calculate()
		{
			base.Calculate();

			IsProvider = 
				IsAccommodationProvider || IsBusTicketProvider || IsCarRentalProvider ||
				IsPasteboardProvider || IsTourProvider || IsTransferProvider || IsGenericProductProvider;

			IsSupplier |= IsProvider;
		}

	}


	partial class Domain
	{
		public DbSet<Organization> Organizations { get; set; }
	}

}