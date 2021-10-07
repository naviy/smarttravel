using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public enum ProductType
	{

		[Localization(typeof(AviaTicket))]
		AviaTicket = 0,

		[Localization(typeof(AviaRefund))]
		AviaRefund = 1,

		[Localization(typeof(AviaMco))]
		AviaMco = 2,
	

		[Localization(typeof(Pasteboard))]
		Pasteboard = 3,

		[Localization(typeof(SimCard))]
		SimCard = 4,

		[Localization(typeof(Isic))]
		Isic = 5,

		[Localization(typeof(Excursion))]
		Excursion = 6,

		[Localization(typeof(Tour))]
		Tour = 7,

		[Localization(typeof(Accommodation))]
		Accommodation = 8,

		[Localization(typeof(Transfer))]
		Transfer = 9,

		[Localization(typeof(Insurance))]
		Insurance = 10,

		[Localization(typeof(CarRental))]
		CarRental = 11,

		[Localization(typeof(GenericProduct))]
		GenericProduct = 12,

		[Localization(typeof(BusTicket))]
		BusTicket = 13,


		[Localization(typeof(PasteboardRefund))]
		PasteboardRefund = 14,

		[Localization(typeof(InsuranceRefund))]
		InsuranceRefund = 15,

		[Localization(typeof(BusTicketRefund))]
		BusTicketRefund = 16,
	}


	public enum ProductTypes
	{
		MaxValue = ProductType.BusTicketRefund
	}

	partial class Product
	{
		public virtual bool IsAviaDocument 
		{ 
			get { return Type == ProductType.AviaTicket || Type == ProductType.AviaRefund || Type == ProductType.AviaMco; } 
		}
		public virtual bool IsAviaTicket { get { return Type == ProductType.AviaTicket; } }
		public virtual bool IsAviaRefund { get { return Type == ProductType.AviaRefund; } }
		public virtual bool IsAviaMco { get { return Type == ProductType.AviaMco; } }

		public virtual bool IsPasteboard { get { return Type == ProductType.Pasteboard; } }
		public virtual bool IsPasteboardRefund { get { return Type == ProductType.PasteboardRefund; } }
		public virtual bool IsSimCard { get { return Type == ProductType.SimCard; } }
		public virtual bool IsIsic { get { return Type == ProductType.Isic; } }
		public virtual bool IsExcursion { get { return Type == ProductType.Excursion; } }
		public virtual bool IsTour { get { return Type == ProductType.Tour; } }
		public virtual bool IsAccommodation { get { return Type == ProductType.Accommodation; } }
		public virtual bool IsTransfer { get { return Type == ProductType.Transfer; } }
		public virtual bool IsInsurance { get { return Type == ProductType.Insurance; } }
		public virtual bool IsInsuranceRefund { get { return Type == ProductType.InsuranceRefund; } }
		public virtual bool IsCarRental { get { return Type == ProductType.CarRental; } }
		public virtual bool IsGenericProduct { get { return Type == ProductType.GenericProduct; } }
		public virtual bool IsBusTicket { get { return Type == ProductType.BusTicket; } }
		public virtual bool IsBusTicketRefund { get { return Type == ProductType.BusTicketRefund; } }


		public static bool TypeIsRefund(ProductType type)
		{
			return type == ProductType.AviaRefund || type == ProductType.PasteboardRefund;
		}
	}

}