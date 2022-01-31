using System;
using System.ComponentModel;
using System.Linq;

using Luxena.Base.Domain;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public enum PasteboardServiceClass
	{
		[RU("1-й класс")]
		FirstClass,

		[RU("2-й класс")]
		SecondClass,

		[RU("люкс")]
		LuxuryCoupe,

		[RU("плацкарт")]
		ReservedSeat,

		[RU("купе")]
		Compartment,

		[RU("Неизвестно")]
		Unknown,

		[RU("Общий")]
		Сommon,
	}


	[RU("Ж/д билет", "Ж/д билеты")]
	public partial class Pasteboard : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Pasteboard> se)
		{
			se.For(a => a.RefundedProduct)
				.Suggest<Pasteboard>();

			se.For(a => a.ReissueFor)
				.Suggest<Pasteboard>();

			se.For(a => a.Provider)
				.Suggest<PasteboardProvider>();
		}


		public override ProductType Type => ProductType.Pasteboard;

		public override string Name => Number ?? PnrCode ?? "";

		public override string PassengerName { get => GetPassengerName();
			set => SetPassengerName(value);
		}

		[Patterns.Passenger]
		public virtual Person Passenger { get => GetPassenger();
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


		[RU("Маршрут")]
		public virtual string Itinerary
		{
			get => DeparturePlace.Yes() ? ArrivalPlace.Yes() ? DeparturePlace + " - " + ArrivalPlace : DeparturePlace : ArrivalPlace;
			set {  }
		}


		[RU("Номер поезда")]
		public virtual string TrainNumber { get; set; }

		[RU("Номер вагона")]
		public virtual string CarNumber { get; set; }

		[RU("Номер места")]
		public virtual string SeatNumber { get; set; }


		[Patterns.ServiceClass, DefaultValue(0)]
		public virtual PasteboardServiceClass ServiceClass { get; set; }


		public override Entity Resolve(Domain db)
		{
			base.Resolve(db);

			if (IsVoid)
			{
				var r0 = db.Pasteboard.By(a => a.Number == Number && a.SeatNumber == SeatNumber);

				if (r0 != null)
				{
					r0.IsVoid = true;
					return r0;
				}
			}

			var r = this;

			if (db.Pasteboard.IsExists(r))
				throw new DomainException(string.Format(Exceptions.ImportGdsFile_DocumentAlreadyExists, r));

			if (r.Provider != null)
				r.Provider = db.Organization.By(a => a.Name == r.Provider.Name && a.IsPasteboardProvider);

			if (r.PaymentType == PaymentType.Unknown)
				db.Configuration.Pasterboard_DefaultPaymentType.Do(a => r.PaymentType = a);

			return r;
		}


		public new partial class Service : Service<Pasteboard>
		{
			public Service()
			{
				Validating += r =>
				{
					if ((db.IsNew(r) && Query.Any(a => a.Number == r.Number && a.Type == r.Type && a.PassengerName == r.PassengerName && !a.IsVoid)) ||
						(!db.IsNew(r) && Query.Any(a => a.Number == r.Number && a.Type == r.Type && a.PassengerName == r.PassengerName && a.Id != r.Id && !a.IsVoid)))
						throw new Exception(r.GetType().Localization(db).Default + " " + Exceptions.Product_lt_product_number_passenger_key);
				};
			}

			public bool IsExists(Pasteboard r)
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

	[RU("Возврат ж/д билета", "Возвраты ж/д билетов")]
	public partial class PasteboardRefund : Pasteboard
	{
		public override ProductType Type => ProductType.PasteboardRefund;

		public override bool IsRefund => true;

		public override string Name => string.Format(DomainRes.ProductRefund_NameFormat, base.Name);

		public new partial class Service : Service<PasteboardRefund> { }
	}

}
