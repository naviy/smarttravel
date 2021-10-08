using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class CarRentalDto : ProductDto
	{

		public ProductPassengerDto[] Passengers { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public string CarBrand { get; set; }

	}


	public partial class CarRentalContractService : ProductContractService<CarRental, CarRental.Service, CarRentalDto>
	{
		public CarRentalContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Passengers = NewPassengers(r.Passengers);
				c.Provider = r.Provider;

				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;
				c.CarBrand = r.CarBrand;
			};

			EntityFromContract += (r, c) =>
			{
				r.Provider = c.Provider + db;

				r.StartDate = c.StartDate + db;
				r.FinishDate = c.FinishDate + db;
				r.CarBrand = c.CarBrand + db;

				dc.ProductPassenger.Update(
					r, r.Passengers, c.Passengers,
					r.AddPassenger, r.RemovePassenger
				);
			};
		}
	}

}