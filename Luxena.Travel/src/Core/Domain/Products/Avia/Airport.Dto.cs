using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class AirportDto : Entity3Contract
	{

		public string Code { get; set; }

		public Country.Reference Country { get; set; }

		public string Settlement { get; set; }

		public string LocalizedSettlement { get; set; }

		public double? Latitude { get; set; }

		public double? Longitude { get; set; }

	}


	public partial class AirportContractService : Entity3ContractService<Airport, Airport.Service, AirportDto>
	{
		public AirportContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Code = r.Code;
				c.Country = r.Country;
				c.Settlement = r.Settlement;
				c.LocalizedSettlement = r.LocalizedSettlement;
				c.Latitude = r.Latitude;
				c.Longitude = r.Longitude;
			};

			EntityFromContract += (r, c) =>
			{
				r.Code = c.Code + db;
				r.Country = c.Country + db;
				r.Settlement = c.Settlement + db;
				r.LocalizedSettlement = c.LocalizedSettlement + db;
				r.Latitude = c.Latitude + db;
				r.Longitude = c.Longitude + db;
			};
		}
	}

}