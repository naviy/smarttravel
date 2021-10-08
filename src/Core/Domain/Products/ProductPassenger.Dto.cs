using Luxena.Base.Serialization;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class ProductPassengerDto : EntityContract
	{

		public string PassengerName { get; set; }

		public virtual Person.Reference Passenger { get; set; }

	}

	public partial class ProductPassengerContractService : EntityContractService<ProductPassenger, ProductPassenger.Service, ProductPassengerDto>
	{
		public ProductPassengerContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PassengerName = r.PassengerName;
				c.Passenger = r.Passenger;
			};

			EntityFromContract += (r, c) =>
			{
				r.PassengerName = c.PassengerName + db;
				r.Passenger = c.Passenger + db;
			};
		}
	}

}
