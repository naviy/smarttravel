using System;
using System.Linq;

using Luxena.Base.Domain;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Автобусный билет", "Автобусные билеты")]
	public partial class BusTicket : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<BusTicket> se)
		{
			se.For(a => a.RefundedProduct)
				.Suggest<BusTicket>();

			se.For(a => a.ReissueFor)
				.Suggest<BusTicket>();

			se.For(a => a.Provider)
				.Suggest<BusTicketProvider>();
		}


		public override ProductType Type => ProductType.BusTicket;

		public override string Name => Number ?? PnrCode ?? DomainRes.BusTicket;

		public override string PassengerName
		{
			get => GetPassengerName();
			set => SetPassengerName(value);
		}

		[Patterns.Passenger]
		public virtual Person Passenger
		{
			get => GetPassenger();
			set => SetPassenger(value);
		}


		[Patterns.Number, EntityName2]
		public virtual string Number { get; set; }

		[RU("Начальная станция"), Patterns.DeparturePlace]
		public virtual string DeparturePlace { get; set; }

		[Patterns.DepartureDate]
		public virtual DateTime? DepartureDate { get; set; }

		[Patterns.DepartureTime]
		public virtual string DepartureTime { get; set; }


		[RU("Конечная станция"), Patterns.ArrivalPlace]
		public virtual string ArrivalPlace { get; set; }

		[Patterns.ArrivalDate]
		public virtual DateTime? ArrivalDate { get; set; }

		[Patterns.ArrivalTime]
		public virtual string ArrivalTime { get; set; }


		[RU("Номер места")]
		public virtual string SeatNumber { get; set; }


		public override Entity Resolve(Domain db)
		{
			base.Resolve(db);

			if (IsVoid)
			{
				var r0 = db.BusTicket.By(a => a.Number == Number);

				if (r0 != null)
				{
					r0.IsVoid = true;
					return r0;
				}
			}


			var r = this;

			if (db.BusTicket.IsExists(r))
				throw new DomainException(string.Format(Exceptions.ImportGdsFile_DocumentAlreadyExists, r));

			if (r.Provider != null)
				r.Provider = db.Organization.By(a => a.Name == r.Provider.Name && a.IsBusTicketProvider);

			//if (r.PaymentType == PaymentType.Unknown)
			//	db.Configuration.Pasterboard_DefaultPaymentType.Do(a => r.PaymentType = a);


			return r;
		}



		public new partial class Service : Service<BusTicket>
		{

			public bool IsExists(BusTicket r)
			{
				if (r == null || r.Number.No())
					return false;

				var id = Query
					.Where(a =>
						a.Number == r.Number &&
						a.Type == r.Type &&
						a.SeatNumber == r.SeatNumber &&
						(r.Id == null || a.Id != r.Id)
					)
					.Select(a => a.Id)
					.FirstOrDefault();

				return id != null;
			}

		}

	}


	public partial class BusTicketRefund : BusTicket
	{
		public override ProductType Type => ProductType.BusTicketRefund;

		public override bool IsRefund => true;

		public override string Name => string.Format(DomainRes.ProductRefund_NameFormat, base.Name);




		public new partial class Service : Service<BusTicketRefund> { }
	}

}
