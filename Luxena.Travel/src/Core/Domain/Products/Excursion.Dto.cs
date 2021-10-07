using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class ExcursionDto : ProductDto
	{

		public ProductPassengerDto[] Passengers { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public string TourName { get; set; }

	}


	public partial class ExcursionContractService 
		: ProductContractService<Excursion, Excursion.Service, ExcursionDto>
	{
		public ExcursionContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Passengers = NewPassengers(r.Passengers);

				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;
				c.TourName = r.TourName;
			};

			EntityFromContract += (r, c) =>
			{
				r.StartDate = c.StartDate + db;
				r.FinishDate = c.FinishDate + db;
				r.TourName = c.TourName + db;

				dc.ProductPassenger.Update(
					r, r.Passengers, c.Passengers,
					r.AddPassenger, r.RemovePassenger
				);
			};
		}
	}

}