using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class PasteboardDto : ProductDto
	{

		public string PassengerName { get; set; }

		public virtual Person.Reference Passenger { get; set; }


		public string Number { get; set; }

		public string DeparturePlace { get; set; }

		public DateTime? DepartureDate { get; set; }

		public string DepartureTime { get; set; }


		public string ArrivalPlace { get; set; }

		public DateTime? ArrivalDate { get; set; }

		public string ArrivalTime { get; set; }


		public string TrainNumber { get; set; }

		public string CarNumber { get; set; }

		public string SeatNumber { get; set; }

		public int ServiceClass { get; set; }


		public MoneyDto BookingFee { get; set; }
	}


	[DataContract]
	public partial class PasteboardRefundDto : PasteboardDto {}

	public abstract partial class PasteboardContractService<TPasteboard, TPasteboardService, TPasteboardDto>
		: ProductContractService<TPasteboard, TPasteboardService, TPasteboardDto>
		where TPasteboard : Pasteboard, new()
		where TPasteboardService : Pasteboard.Service<TPasteboard>
		where TPasteboardDto : PasteboardDto, new()

	{
		protected PasteboardContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PassengerName = r.PassengerName;
				c.Passenger = r.Passenger;

				c.Number = r.Number;
				c.DeparturePlace = r.DeparturePlace;
				c.DepartureDate = r.DepartureDate;
				c.DepartureTime = r.DepartureTime;

				c.ArrivalPlace = r.ArrivalPlace;
				c.ArrivalDate = r.ArrivalDate;
				c.ArrivalTime = r.ArrivalTime;

				c.TrainNumber = r.TrainNumber;
				c.CarNumber = r.CarNumber;
				c.SeatNumber = r.SeatNumber;

				c.ServiceClass = (int)r.ServiceClass;

				c.BookingFee = r.BookingFee;
			};

			EntityFromContract += (r, c) =>
			{
				r.PassengerName = c.PassengerName + db;
				r.Passenger = c.Passenger + db;

				r.Number = c.Number + db;

				r.DeparturePlace = c.DeparturePlace + db;
				r.DepartureDate = c.DepartureDate + db;
				r.DepartureTime = c.DepartureTime + db;

				r.ArrivalPlace = c.ArrivalPlace + db;
				r.ArrivalDate = c.ArrivalDate + db;
				r.ArrivalTime = c.ArrivalTime + db;

				r.TrainNumber = c.TrainNumber + db;
				r.CarNumber = c.CarNumber + db;
				r.SeatNumber = c.SeatNumber + db;
				
				r.ServiceClass = (PasteboardServiceClass)c.ServiceClass + db;

				r.BookingFee = c.BookingFee + db;
			};
		}
	}

	public partial class PasteboardContractService : PasteboardContractService<Pasteboard, Pasteboard.Service, PasteboardDto> { }

	public partial class PasteboardRefundContractService : PasteboardContractService<PasteboardRefund, PasteboardRefund.Service, PasteboardRefundDto> { }

}