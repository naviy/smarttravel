using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class TransferDto : ProductDto
	{

		public ProductPassengerDto[] Passengers { get; set; }

		public DateTime StartDate { get; set; }

	}


	public partial class TransferContractService 
		: ProductContractService<Transfer, Transfer.Service, TransferDto>
	{
		public TransferContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Passengers = NewPassengers(r.Passengers);

				c.StartDate = r.StartDate;
			};

			EntityFromContract += (r, c) =>
			{
				r.StartDate = c.StartDate + db;

				dc.ProductPassenger.Update(
					r, r.Passengers, c.Passengers,
					r.AddPassenger, r.RemovePassenger
				);
			};
		}
	}

}