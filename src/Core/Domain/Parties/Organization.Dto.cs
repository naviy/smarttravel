namespace Luxena.Travel.Domain
{

	public partial class OrganizationDto : PartyDto
	{

		public string Code { get; set; }


		public bool IsAccommodationProvider { get; set; }

		public bool IsBusTicketProvider { get; set; }

		public bool IsCarRentalProvider { get; set; }

//		public bool IsInsuranceProvider { get; set; }

		public bool IsPasteboardProvider { get; set; }

		public bool IsTourProvider { get; set; }

		public bool IsTransferProvider { get; set; }

		public bool IsGenericProductProvider { get; set; }


		public bool IsAirline { get; set; }

		public string AirlineIataCode { get; set; }

		public string AirlinePrefixCode { get; set; }

		public int AirlinePassportRequirement { get; set; }


		public bool IsInsuranceCompany { get; set; }

		public bool IsRoamingOperator { get; set; }

		public DepartmentListDetailDto[] Departments { get; set; }

		public PersonListDetailDto[] Employees { get; set; }
	}


	public partial class OrganizationContractService
		: PartyContractService<Organization, Organization.Service, OrganizationDto>
	{
		public OrganizationContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Code = r.Code;

				c.IsAccommodationProvider = r.IsAccommodationProvider;
				c.IsBusTicketProvider = r.IsBusTicketProvider;
				c.IsCarRentalProvider = r.IsCarRentalProvider;
//				c.IsInsuranceProvider = r.IsInsuranceProvider;
				c.IsPasteboardProvider = r.IsPasteboardProvider;
				c.IsTourProvider = r.IsTourProvider;
				c.IsTransferProvider = r.IsTransferProvider;
				c.IsGenericProductProvider = r.IsGenericProductProvider;

				c.IsAirline = r.IsAirline;
				c.AirlineIataCode = r.AirlineIataCode;
				c.AirlinePrefixCode = r.AirlinePrefixCode;
				c.AirlinePassportRequirement = (int)r.AirlinePassportRequirement;

				c.IsInsuranceCompany = r.IsInsuranceCompany;
				c.IsRoamingOperator = r.IsRoamingOperator;

				c.Employees = dc.PersonListDetail.New(db.Person.ListBy(a => a.Organization == r));
				c.Departments = dc.DepartmentListDetail.New(db.Department.ListBy(a => a.Organization == r));
			};

			EntityFromContract += (r, c) =>
			{
				r.Code = c.Code + db;

				r.IsAccommodationProvider = c.IsAccommodationProvider + db;
				r.IsBusTicketProvider = c.IsBusTicketProvider + db;
				r.IsCarRentalProvider = c.IsCarRentalProvider + db;
//				r.IsInsuranceProvider = c.IsInsuranceProvider + db;
				r.IsPasteboardProvider = c.IsPasteboardProvider + db;
				r.IsTourProvider = c.IsTourProvider + db;
				r.IsTransferProvider = c.IsTransferProvider + db;
				r.IsGenericProductProvider = c.IsGenericProductProvider + db;

				r.IsAirline = c.IsAirline + db;
				r.AirlineIataCode = c.AirlineIataCode + db;
				r.AirlinePrefixCode = c.AirlinePrefixCode + db;
				r.AirlinePassportRequirement = (AirlinePassportRequirement)c.AirlinePassportRequirement + db;

				r.IsInsuranceCompany = c.IsInsuranceCompany + db;
				r.IsRoamingOperator = c.IsRoamingOperator + db;
			};
		}
	}

}