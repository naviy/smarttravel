using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class AirplaneModelDto : Entity3Contract
	{

		public string IataCode { get; set; }

		public string IcaoCode { get; set; }

	}


	public partial class AirplaneModelContractService : Entity3ContractService<AirplaneModel, AirplaneModel.Service, AirplaneModelDto>
	{
		public AirplaneModelContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.IataCode = r.IataCode;
				c.IcaoCode = r.IcaoCode;
			};

			EntityFromContract += (r, c) =>
			{
				r.IataCode = c.IataCode + db;
				r.IcaoCode = c.IcaoCode + db;
			};
		}
	}

}