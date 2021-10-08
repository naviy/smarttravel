using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class TourDto : ProductDto
	{

		public ProductPassengerDto[] Passengers { get; set; }


		public DateTime StartDate { get; set; }

		public DateTime? FinishDate { get; set; }


		public string HotelName { get; set; }

		public string HotelOffice { get; set; }

		public string HotelCode { get; set; }


		public string PlacementName { get; set; }

		public string PlacementOffice { get; set; }

		public string PlacementCode { get; set; }


		public AccommodationType.Reference AccommodationType { get; set; }

		public CateringType.Reference CateringType { get; set; }


		public string AviaDescription { get; set; }

		public string TransferDescription { get; set; }

	}


	public partial class TourContractService : ProductContractService<Tour, Tour.Service, TourDto>
	{
		public TourContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Passengers = NewPassengers(r.Passengers);

				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;

				c.HotelName = r.HotelName;
				c.HotelOffice = r.HotelOffice;
				c.HotelCode = r.HotelCode;

				c.PlacementName = r.PlacementName;
				c.PlacementOffice = r.PlacementOffice;
				c.PlacementCode = r.PlacementCode;

				c.AccommodationType = r.AccommodationType;
				c.CateringType = r.CateringType;

				c.AviaDescription = r.AviaDescription;
				c.TransferDescription = r.TransferDescription;
			};

			EntityFromContract += (r, c) =>
			{
				r.StartDate = c.StartDate + db;
				r.FinishDate = c.FinishDate + db;

				r.HotelName = c.HotelName + db;
				r.HotelOffice = c.HotelOffice + db;
				r.HotelCode = c.HotelCode + db;

				r.PlacementName = c.PlacementName + db;
				r.PlacementOffice = c.PlacementOffice + db;
				r.PlacementCode = c.PlacementCode + db;

				r.AccommodationType = c.AccommodationType + db;
				r.CateringType = c.CateringType + db;

				r.AviaDescription = c.AviaDescription + db;
				r.TransferDescription = c.TransferDescription + db;

				dc.ProductPassenger.Update(
					r, r.Passengers, c.Passengers,
					r.AddPassenger, r.RemovePassenger
				);
			};
		}
	}

}