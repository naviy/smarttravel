using System.Collections.Generic;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Вид услуги"), Length(12)]
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


	partial class Product
	{

		public static Dictionary<ProductType, ProductType?> ProductTypesByRefundType = new Dictionary<ProductType, ProductType?>()
		{
			{ ProductType.AviaRefund, ProductType.AviaTicket },
			{ ProductType.PasteboardRefund, ProductType.Pasteboard },
			{ ProductType.InsuranceRefund, ProductType.Insurance },
			{ ProductType.BusTicketRefund, ProductType.BusTicket },
		};

		[NotUiMapped]
		public bool IsAviaDocument
		{
			get { return Type == ProductType.AviaTicket || Type == ProductType.AviaRefund || Type == ProductType.AviaMco; }
		}

		[NotUiMapped]
		public bool IsAviaTicket { get { return Type == ProductType.AviaTicket; } }

		[NotUiMapped]
		public bool IsAviaRefund { get { return Type == ProductType.AviaRefund; } }

		[NotUiMapped]
		public bool IsAviaMco { get { return Type == ProductType.AviaMco; } }


		[NotUiMapped]
		public bool IsPasteboard { get { return Type == ProductType.Pasteboard; } }

		[NotUiMapped]
		public bool IsPasteboardRefund { get { return Type == ProductType.PasteboardRefund; } }

		[NotUiMapped]
		public bool IsSimCard { get { return Type == ProductType.SimCard; } }

		[NotUiMapped]
		public bool IsIsic { get { return Type == ProductType.Isic; } }

		[NotUiMapped]
		public bool IsExcursion { get { return Type == ProductType.Excursion; } }

		[NotUiMapped]
		public bool IsTour { get { return Type == ProductType.Tour; } }

		[NotUiMapped]
		public bool IsAccommodation { get { return Type == ProductType.Accommodation; } }

		[NotUiMapped]
		public bool IsTransfer { get { return Type == ProductType.Transfer; } }

		[NotUiMapped]
		public bool IsInsurance { get { return Type == ProductType.Insurance; } }

		[NotUiMapped]
		public bool IsInsuranceRefund { get { return Type == ProductType.InsuranceRefund; } }

		[NotUiMapped]
		public bool IsCarRental { get { return Type == ProductType.CarRental; } }

		[NotUiMapped]
		public bool IsGenericProduct { get { return Type == ProductType.GenericProduct; } }

		[NotUiMapped]
		public bool IsBusTicket { get { return Type == ProductType.BusTicket; } }

		[NotUiMapped]
		public bool IsBusTicketRefund { get { return Type == ProductType.BusTicketRefund; } }

	}

}