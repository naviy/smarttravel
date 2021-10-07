using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class SimCardDto : ProductDto
	{

		public string PassengerName { get; set; }

		public virtual Person.Reference Passenger { get; set; }

		public string Number { get; set; }

		public bool IsSale { get; set; }

	}


	public partial class SimCardContractService 
		: ProductContractService<SimCard, SimCard.Service, SimCardDto>
	{
		public SimCardContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PassengerName = r.PassengerName;
				c.Passenger = r.Passenger;
				c.Number = r.Number;
				c.IsSale = r.IsSale;
			};

			EntityFromContract += (r, c) =>
			{
				r.PassengerName = c.PassengerName + db;
				r.Passenger = c.Passenger + db;
				r.Number = c.Number + db;
				r.IsSale = c.IsSale + db;
			};
		}
	}

}