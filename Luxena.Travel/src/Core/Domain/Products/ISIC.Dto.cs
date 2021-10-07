using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class IsicDto : ProductDto
	{

		public string PassengerName { get; set; }

		public Person.Reference Passenger { get; set; }

		public int CardType { get; set; }

		public string Number1 { get; set; }

		public string Number2 { get; set; }

	}


	public partial class IsicContractService : ProductContractService<Isic, Isic.Service, IsicDto>
	{
		public IsicContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PassengerName = r.PassengerName;
				c.Passenger = r.Passenger;
				c.CardType = (int)r.CardType;
				c.Number1 = r.Number1;
				c.Number2 = r.Number2;
			};

			EntityFromContract += (r, c) =>
			{
				r.PassengerName = c.PassengerName + db;
				r.Passenger = c.Passenger + db;
				r.CardType = (IsicCardType)c.CardType;// + db;
				r.Number1 = c.Number1 + db;
				r.Number2 = c.Number2 + db;
			};
		}
	}

}