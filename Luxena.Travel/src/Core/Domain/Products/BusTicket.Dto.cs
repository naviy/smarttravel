using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class BusTicketDto : ProductDto
	{

		public string PassengerName { get; set; }

		public Person.Reference Passenger { get; set; }


		public string Number { get; set; }


		public string DeparturePlace { get; set; }

		public DateTime? DepartureDate { get; set; }

		public string DepartureTime { get; set; }


		public string ArrivalPlace { get; set; }

		public DateTime? ArrivalDate { get; set; }

		public string ArrivalTime { get; set; }


		public string SeatNumber { get; set; }

	}


	[DataContract]
	public partial class BusTicketRefundDto : BusTicketDto {}

	public abstract partial class BusTicketContractService<TBusTicket, TBusTicketService, TBusTicketDto>
		: ProductContractService<TBusTicket, TBusTicketService, TBusTicketDto>
		where TBusTicket : BusTicket, new()
		where TBusTicketService : BusTicket.Service<TBusTicket>
		where TBusTicketDto : BusTicketDto, new()
	{
		protected BusTicketContractService()
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

				c.SeatNumber = r.SeatNumber;
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

				r.SeatNumber = c.SeatNumber + db;
			};
		}

	}

	public partial class BusTicketContractService : BusTicketContractService<BusTicket, BusTicket.Service, BusTicketDto> { }

	public partial class BusTicketRefundContractService : BusTicketContractService<BusTicketRefund, BusTicketRefund.Service, BusTicketRefundDto> { }

}