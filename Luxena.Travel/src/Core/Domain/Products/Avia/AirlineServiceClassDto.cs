using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class AirlineServiceClassDto : EntityContract
	{

		public Organization.Reference Airline { get; set; }

		public string Code { get; set; }

		public int ServiceClass { get; set; }

	}


	public partial class AirlineServiceClassContractService : EntityContractService<AirlineServiceClass, AirlineServiceClass.Service, AirlineServiceClassDto>
	{
		public AirlineServiceClassContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Airline = r.Airline;
				c.Code = r.Code;
				c.ServiceClass = (int)r.ServiceClass;
			};


			EntityFromContract += (r, c) =>
			{
				r.Airline = c.Airline + db;
				r.Code = c.Code + db;
				r.ServiceClass = (ServiceClass)c.ServiceClass + db;
			};
		}
	}

}