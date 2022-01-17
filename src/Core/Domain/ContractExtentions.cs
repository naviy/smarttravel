using System.Diagnostics;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{


	#region AccommodationDto
		
	partial class Contracts
	{
		public AccommodationContractService Accommodation { [DebuggerStepThrough] get { return ResolveService(ref _accommodationDto); } }
		private AccommodationContractService _accommodationDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AccommodationDto r, RangeRequest prms)
		{
			return Accommodation.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AccommodationDto r, string typeName, RangeRequest prms)
		{
			return Accommodation.Update(r, typeName, prms);
		}
	}

	partial class AccommodationDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

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

		*/
	}

	partial class AccommodationContractService
	{
		/*
		public AccommodationContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Passengers = r.Passengers; ??? is array
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
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Passengers = c.Passengers + db; ??? is array
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
			};
		}
		*/
	}

	#endregion
	

	#region AccommodationTypeDto
		
	partial class Contracts
	{
		public AccommodationTypeContractService AccommodationType { [DebuggerStepThrough] get { return ResolveService(ref _accommodationTypeDto); } }
		private AccommodationTypeContractService _accommodationTypeDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AccommodationTypeDto r, RangeRequest prms)
		{
			return AccommodationType.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AccommodationTypeDto r, string typeName, RangeRequest prms)
		{
			return AccommodationType.Update(r, typeName, prms);
		}
	}

	partial class AccommodationTypeDto
	{
		/*
		*/
	}

	partial class AccommodationTypeContractService
	{
		/*
		public AccommodationTypeContractService()
		{
			ContractFromEntity += (r, c) =>
			{
			};
		
			EntityFromContract += (r, c) =>
			{
			};
		}
		*/
	}

	#endregion
	

	#region AirlineCommissionPercentsDto
		
	partial class Contracts
	{
		public AirlineCommissionPercentsContractService AirlineCommissionPercents { [DebuggerStepThrough] get { return ResolveService(ref _airlineCommissionPercentsDto); } }
		private AirlineCommissionPercentsContractService _airlineCommissionPercentsDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AirlineCommissionPercentsDto r, RangeRequest prms)
		{
			return AirlineCommissionPercents.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AirlineCommissionPercentsDto r, string typeName, RangeRequest prms)
		{
			return AirlineCommissionPercents.Update(r, typeName, prms);
		}
	}

	partial class AirlineCommissionPercentsDto
	{
		/*
		public Organization.Reference Airline { get; set; }

		public Decimal Domestic { get; set; }

		public Decimal International { get; set; }

		public Decimal InterlineDomestic { get; set; }

		public Decimal InterlineInternational { get; set; }

		*/
	}

	partial class AirlineCommissionPercentsContractService
	{
		/*
		public AirlineCommissionPercentsContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Airline = r.Airline;
				c.Domestic = r.Domestic;
				c.International = r.International;
				c.InterlineDomestic = r.InterlineDomestic;
				c.InterlineInternational = r.InterlineInternational;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Airline = c.Airline + db;
				r.Domestic = c.Domestic + db;
				r.International = c.International + db;
				r.InterlineDomestic = c.InterlineDomestic + db;
				r.InterlineInternational = c.InterlineInternational + db;
			};
		}
		*/
	}

	#endregion
	

	#region AirlineMonthCommissionDto
		
	partial class Contracts
	{
		public AirlineMonthCommissionContractService AirlineMonthCommission { [DebuggerStepThrough] get { return ResolveService(ref _airlineMonthCommissionDto); } }
		private AirlineMonthCommissionContractService _airlineMonthCommissionDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AirlineMonthCommissionDto r, RangeRequest prms)
		{
			return AirlineMonthCommission.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AirlineMonthCommissionDto r, string typeName, RangeRequest prms)
		{
			return AirlineMonthCommission.Update(r, typeName, prms);
		}
	}

	partial class AirlineMonthCommissionDto
	{
		/*
		public Organization.Reference Airline { get; set; }

		public DateTime DateFrom { get; set; }

		public DateTime? DateTo { get; set; }

		public Nullable`1 CommissionPc { get; set; }

		*/
	}

	partial class AirlineMonthCommissionContractService
	{
		/*
		public AirlineMonthCommissionContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Airline = r.Airline;
				c.DateFrom = r.DateFrom;
				c.CommissionPc = r.CommissionPc;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Airline = c.Airline + db;
				r.DateFrom = c.DateFrom + db;
				r.CommissionPc = c.CommissionPc + db;
			};
		}
		*/
	}

	#endregion
	

	#region AirlineServiceClassDto
		
	partial class Contracts
	{
		public AirlineServiceClassContractService AirlineServiceClass { [DebuggerStepThrough] get { return ResolveService(ref _airlineServiceClassDto); } }
		private AirlineServiceClassContractService _airlineServiceClassDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AirlineServiceClassDto r, RangeRequest prms)
		{
			return AirlineServiceClass.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AirlineServiceClassDto r, string typeName, RangeRequest prms)
		{
			return AirlineServiceClass.Update(r, typeName, prms);
		}
	}

	partial class AirlineServiceClassDto
	{
		/*
		public Organization.Reference Airline { get; set; }

		public string Code { get; set; }

		public int ServiceClass { get; set; }

		*/
	}

	partial class AirlineServiceClassContractService
	{
		/*
		public AirlineServiceClassContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Airline = r.Airline;
				c.Code = r.Code;
				c.ServiceClass = (int)r.ServiceClass;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Airline = c.Airline + db;
				r.Code = c.Code + db;
				r.ServiceClass = (ServiceClass)c.ServiceClass + db;
			};
		}
		*/
	}

	#endregion
	

	#region AirplaneModelDto
		
	partial class Contracts
	{
		public AirplaneModelContractService AirplaneModel { [DebuggerStepThrough] get { return ResolveService(ref _airplaneModelDto); } }
		private AirplaneModelContractService _airplaneModelDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AirplaneModelDto r, RangeRequest prms)
		{
			return AirplaneModel.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AirplaneModelDto r, string typeName, RangeRequest prms)
		{
			return AirplaneModel.Update(r, typeName, prms);
		}
	}

	partial class AirplaneModelDto
	{
		/*
		public string IataCode { get; set; }

		public string IcaoCode { get; set; }

		*/
	}

	partial class AirplaneModelContractService
	{
		/*
		public AirplaneModelContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.IataCode = r.IataCode;
				c.IcaoCode = r.IcaoCode;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.IataCode = c.IataCode + db;
				r.IcaoCode = c.IcaoCode + db;
			};
		}
		*/
	}

	#endregion
	

	#region AirportDto
		
	partial class Contracts
	{
		public AirportContractService Airport { [DebuggerStepThrough] get { return ResolveService(ref _airportDto); } }
		private AirportContractService _airportDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AirportDto r, RangeRequest prms)
		{
			return Airport.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AirportDto r, string typeName, RangeRequest prms)
		{
			return Airport.Update(r, typeName, prms);
		}
	}

	partial class AirportDto
	{
		/*
		public string Code { get; set; }

		public Country.Reference Country { get; set; }

		public string Settlement { get; set; }

		public string LocalizedSettlement { get; set; }

		public Nullable`1 Latitude { get; set; }

		public Nullable`1 Longitude { get; set; }

		*/
	}

	partial class AirportContractService
	{
		/*
		public AirportContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Code = r.Code;
				c.Country = r.Country;
				c.Settlement = r.Settlement;
				c.LocalizedSettlement = r.LocalizedSettlement;
				c.Latitude = r.Latitude;
				c.Longitude = r.Longitude;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Code = c.Code + db;
				r.Country = c.Country + db;
				r.Settlement = c.Settlement + db;
				r.LocalizedSettlement = c.LocalizedSettlement + db;
				r.Latitude = c.Latitude + db;
				r.Longitude = c.Longitude + db;
			};
		}
		*/
	}

	#endregion
	

	#region AviaDocumentDto
		
	partial class Contracts
	{
		public AviaDocumentContractService AviaDocument { [DebuggerStepThrough] get { return ResolveService(ref _aviaDocumentDto); } }
		private AviaDocumentContractService _aviaDocumentDto;
	}

	#endregion
	

	#region AviaDocumentProcessDto
		
	partial class Contracts
	{
		public AviaDocumentProcessContractService AviaDocumentProcess { [DebuggerStepThrough] get { return ResolveService(ref _aviaDocumentProcessDto); } }
		private AviaDocumentProcessContractService _aviaDocumentProcessDto;
	}

	#endregion
	

	#region AviaMcoDto
		
	partial class Contracts
	{
		public AviaMcoContractService AviaMco { [DebuggerStepThrough] get { return ResolveService(ref _aviaMcoDto); } }
		private AviaMcoContractService _aviaMcoDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AviaMcoDto r, RangeRequest prms)
		{
			return AviaMco.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AviaMcoDto r, string typeName, RangeRequest prms)
		{
			return AviaMco.Update(r, typeName, prms);
		}
	}

	partial class AviaMcoDto
	{
		/*
		public int Type { get; set; }

		public string Description { get; set; }

		public AviaDocument.Reference InConnectionWith { get; set; }

		*/
	}

	partial class AviaMcoContractService
	{
		/*
		public AviaMcoContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Description = r.Description;
				c.InConnectionWith = r.InConnectionWith;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Description = c.Description + db;
				r.InConnectionWith = c.InConnectionWith + db;
			};
		}
		*/
	}

	#endregion
	

	#region AviaMcoProcessDto
		
	partial class Contracts
	{
		public AviaMcoProcessContractService AviaMcoProcess { [DebuggerStepThrough] get { return ResolveService(ref _aviaMcoProcessDto); } }
		private AviaMcoProcessContractService _aviaMcoProcessDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AviaMcoProcessDto r, RangeRequest prms)
		{
			return AviaMcoProcess.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AviaMcoProcessDto r, string typeName, RangeRequest prms)
		{
			return AviaMcoProcess.Update(r, typeName, prms);
		}
	}

	partial class AviaMcoProcessDto
	{
		/*
		public int Type { get; set; }

		public string Description { get; set; }

		public AviaDocument.Reference InConnectionWith { get; set; }

		*/
	}

	partial class AviaMcoProcessContractService
	{
		/*
		public AviaMcoProcessContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Description = r.Description;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Description = c.Description + db;
			};
		}
		*/
	}

	#endregion
	

	#region AviaRefundDto
		
	partial class Contracts
	{
		public AviaRefundContractService AviaRefund { [DebuggerStepThrough] get { return ResolveService(ref _aviaRefundDto); } }
		private AviaRefundContractService _aviaRefundDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AviaRefundDto r, RangeRequest prms)
		{
			return AviaRefund.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AviaRefundDto r, string typeName, RangeRequest prms)
		{
			return AviaRefund.Update(r, typeName, prms);
		}
	}

	partial class AviaRefundDto
	{
		/*
		public int Type { get; set; }

		public bool IsRefund { get; set; }

		public AviaDocument.Reference RefundedDocument { get; set; }

		public string Itinerary { get; set; }

		*/
	}

	partial class AviaRefundContractService
	{
		/*
		public AviaRefundContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = r.Type;
				c.IsRefund = r.IsRefund;
				c.RefundedDocument = r.RefundedDocument;
				c.Itinerary = r.Itinerary;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = (ProductType)c.Type + db; !!! property is non-writable
				// r.IsRefund = c.IsRefund + db; !!! property is non-writable
				// r.RefundedDocument = c.RefundedDocument + db; !!! property is non-writable
				r.Itinerary = c.Itinerary + db;
			};
		}
		*/
	}

	#endregion
	

	#region AviaRefundProcessDto
		
	partial class Contracts
	{
		public AviaRefundProcessContractService AviaRefundProcess { [DebuggerStepThrough] get { return ResolveService(ref _aviaRefundProcessDto); } }
		private AviaRefundProcessContractService _aviaRefundProcessDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AviaRefundProcessDto r, RangeRequest prms)
		{
			return AviaRefundProcess.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AviaRefundProcessDto r, string typeName, RangeRequest prms)
		{
			return AviaRefundProcess.Update(r, typeName, prms);
		}
	}

	partial class AviaRefundProcessDto
	{
		/*
		public int Type { get; set; }

		public bool IsRefund { get; set; }

		public AviaDocument.Reference RefundedDocument { get; set; }

		public string Itinerary { get; set; }

		*/
	}

	partial class AviaRefundProcessContractService
	{
		/*
		public AviaRefundProcessContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.ServiceFeePenalty = r.ServiceFeePenalty;
				c.RefundServiceFee = r.RefundServiceFee;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.ServiceFeePenalty = c.ServiceFeePenalty + db;
				r.RefundServiceFee = c.RefundServiceFee + db;
			};
		}
		*/
	}

	#endregion
	

	#region AviaTicketDto
		
	partial class Contracts
	{
		public AviaTicketContractService AviaTicket { [DebuggerStepThrough] get { return ResolveService(ref _aviaTicketDto); } }
		private AviaTicketContractService _aviaTicketDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AviaTicketDto r, RangeRequest prms)
		{
			return AviaTicket.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AviaTicketDto r, string typeName, RangeRequest prms)
		{
			return AviaTicket.Update(r, typeName, prms);
		}
	}

	partial class AviaTicketDto
	{
		/*
		public int Type { get; set; }

		public string Itinerary { get; set; }

		public DateTime? Departure { get; set; }

		public DateTime? Arrival { get; set; }

		public DateTime? LastDeparture { get; set; }

		public string FareBasises { get; set; }

		public bool Domestic { get; set; }

		public bool Interline { get; set; }

		public string SegmentClasses { get; set; }

		public string Endorsement { get; set; }

		public bool IsManual { get; set; }

		public IList`1 Segments { get; set; }

		public Money FareTotal { get; set; }

		public IList`1 PenalizeOperations { get; set; }

		*/
	}

	partial class AviaTicketContractService
	{
		/*
		public AviaTicketContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Segments = r.Segments; ??? is array
				// c.PenalizeOperations = r.PenalizeOperations; ??? is array
				// c.PassportRequired = r.PassportRequired; !!! unknown property
				// c.PassportValidationResult = r.PassportValidationResult; !!! unknown property
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Segments = c.Segments + db; ??? is array
				// r.PenalizeOperations = c.PenalizeOperations + db; ??? is array
				// r.PassportRequired = c.PassportRequired + db; !!! unknown property
				// r.PassportValidationResult = c.PassportValidationResult + db; !!! unknown property
			};
		}
		*/
	}

	#endregion
	

	#region AviaTicketProcessDto
		
	partial class Contracts
	{
		public AviaTicketProcessContractService AviaTicketProcess { [DebuggerStepThrough] get { return ResolveService(ref _aviaTicketProcessDto); } }
		private AviaTicketProcessContractService _aviaTicketProcessDto;

		[DebuggerStepThrough]
		public ItemResponse Update(AviaTicketProcessDto r, RangeRequest prms)
		{
			return AviaTicketProcess.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(AviaTicketProcessDto r, string typeName, RangeRequest prms)
		{
			return AviaTicketProcess.Update(r, typeName, prms);
		}
	}

	partial class AviaTicketProcessDto
	{
		/*
		public int Type { get; set; }

		public string Itinerary { get; set; }

		public DateTime? Departure { get; set; }

		public DateTime? Arrival { get; set; }

		public DateTime? LastDeparture { get; set; }

		public string FareBasises { get; set; }

		public bool Domestic { get; set; }

		public bool Interline { get; set; }

		public string SegmentClasses { get; set; }

		public string Endorsement { get; set; }

		public bool IsManual { get; set; }

		public IList`1 Segments { get; set; }

		public Money FareTotal { get; set; }

		public IList`1 PenalizeOperations { get; set; }

		*/
	}

	partial class AviaTicketProcessContractService
	{
		/*
		public AviaTicketProcessContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.PenalizeOperations = r.PenalizeOperations; ??? is array
				// c.SegmentCount = r.SegmentCount; !!! unknown property
				c.Passenger = r.Passenger;
				// c.SuggestPassenger = r.SuggestPassenger; !!! unknown property
				c.GdsPassport = r.GdsPassport;
				c.GdsPassportStatus = r.GdsPassportStatus;
				// c.Passport = r.Passport; !!! unknown property
				// c.PassportRequired = r.PassportRequired; !!! unknown property
				// c.PassportValidationResult = r.PassportValidationResult; !!! unknown property
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.PenalizeOperations = c.PenalizeOperations + db; ??? is array
				// r.SegmentCount = c.SegmentCount + db; !!! unknown property
				r.Passenger = c.Passenger + db;
				// r.SuggestPassenger = c.SuggestPassenger + db; !!! unknown property
				r.GdsPassport = c.GdsPassport + db;
				r.GdsPassportStatus = (GdsPassportStatus)c.GdsPassportStatus + db;
				// r.Passport = c.Passport + db; !!! unknown property
				// r.PassportRequired = c.PassportRequired + db; !!! unknown property
				// r.PassportValidationResult = c.PassportValidationResult + db; !!! unknown property
			};
		}
		*/
	}

	#endregion
	

	#region BankAccountDto
		
	partial class Contracts
	{
		public BankAccountContractService BankAccount { [DebuggerStepThrough] get { return ResolveService(ref _bankAccountDto); } }
		private BankAccountContractService _bankAccountDto;

		[DebuggerStepThrough]
		public ItemResponse Update(BankAccountDto r, RangeRequest prms)
		{
			return BankAccount.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(BankAccountDto r, string typeName, RangeRequest prms)
		{
			return BankAccount.Update(r, typeName, prms);
		}
	}

	partial class BankAccountDto
	{
		/*
		public bool IsDefault { get; set; }

		public string Note { get; set; }

		*/
	}

	partial class BankAccountContractService
	{
		/*
		public BankAccountContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.IsDefault = r.IsDefault;
				c.Note = r.Note;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.IsDefault = c.IsDefault + db;
				r.Note = c.Note + db;
			};
		}
		*/
	}

	#endregion
	

	#region BusTicketDto
		
	partial class Contracts
	{
		public BusTicketContractService BusTicket { [DebuggerStepThrough] get { return ResolveService(ref _busTicketDto); } }
		private BusTicketContractService _busTicketDto;

		[DebuggerStepThrough]
		public ItemResponse Update(BusTicketDto r, RangeRequest prms)
		{
			return BusTicket.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(BusTicketDto r, string typeName, RangeRequest prms)
		{
			return BusTicket.Update(r, typeName, prms);
		}
	}

	partial class BusTicketDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

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

		*/
	}

	partial class BusTicketContractService
	{
		/*
		public BusTicketContractService()
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
		*/
	}

	#endregion
	

	#region BusTicketRefundDto
		
	partial class Contracts
	{
		public BusTicketRefundContractService BusTicketRefund { [DebuggerStepThrough] get { return ResolveService(ref _busTicketRefundDto); } }
		private BusTicketRefundContractService _busTicketRefundDto;

		[DebuggerStepThrough]
		public ItemResponse Update(BusTicketRefundDto r, RangeRequest prms)
		{
			return BusTicketRefund.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(BusTicketRefundDto r, string typeName, RangeRequest prms)
		{
			return BusTicketRefund.Update(r, typeName, prms);
		}
	}

	partial class BusTicketRefundDto
	{
		/*
		public int Type { get; set; }

		public bool IsRefund { get; set; }

		public string Name { get; set; }

		*/
	}

	partial class BusTicketRefundContractService
	{
		/*
		public BusTicketRefundContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = r.Type;
				c.IsRefund = r.IsRefund;
				c.Name = r.Name;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = (ProductType)c.Type + db; !!! property is non-writable
				// r.IsRefund = c.IsRefund + db; !!! property is non-writable
				// r.Name = c.Name + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region CarRentalDto
		
	partial class Contracts
	{
		public CarRentalContractService CarRental { [DebuggerStepThrough] get { return ResolveService(ref _carRentalDto); } }
		private CarRentalContractService _carRentalDto;

		[DebuggerStepThrough]
		public ItemResponse Update(CarRentalDto r, RangeRequest prms)
		{
			return CarRental.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(CarRentalDto r, string typeName, RangeRequest prms)
		{
			return CarRental.Update(r, typeName, prms);
		}
	}

	partial class CarRentalDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public string CarBrand { get; set; }

		*/
	}

	partial class CarRentalContractService
	{
		/*
		public CarRentalContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Passengers = r.Passengers; ??? is array
				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;
				c.CarBrand = r.CarBrand;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Passengers = c.Passengers + db; ??? is array
				r.StartDate = c.StartDate + db;
				r.FinishDate = c.FinishDate + db;
				r.CarBrand = c.CarBrand + db;
			};
		}
		*/
	}

	#endregion
	

	#region CashInOrderPaymentDto
		
	partial class Contracts
	{
		public CashInOrderPaymentContractService CashInOrderPayment { [DebuggerStepThrough] get { return ResolveService(ref _cashInOrderPaymentDto); } }
		private CashInOrderPaymentContractService _cashInOrderPaymentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(CashInOrderPaymentDto r, RangeRequest prms)
		{
			return CashInOrderPayment.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(CashInOrderPaymentDto r, string typeName, RangeRequest prms)
		{
			return CashInOrderPayment.Update(r, typeName, prms);
		}
	}

	partial class CashInOrderPaymentDto
	{
		/*
		public int PaymentForm { get; set; }

		public string DocumentUniqueCode { get; set; }

		*/
	}

	partial class CashInOrderPaymentContractService
	{
		/*
		public CashInOrderPaymentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.SavePosted = r.SavePosted;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.SavePosted = c.SavePosted + db;
			};
		}
		*/
	}

	#endregion
	

	#region CashOutOrderPaymentDto
		
	partial class Contracts
	{
		public CashOutOrderPaymentContractService CashOutOrderPayment { [DebuggerStepThrough] get { return ResolveService(ref _cashOutOrderPaymentDto); } }
		private CashOutOrderPaymentContractService _cashOutOrderPaymentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(CashOutOrderPaymentDto r, RangeRequest prms)
		{
			return CashOutOrderPayment.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(CashOutOrderPaymentDto r, string typeName, RangeRequest prms)
		{
			return CashOutOrderPayment.Update(r, typeName, prms);
		}
	}

	partial class CashOutOrderPaymentDto
	{
		/*
		public int PaymentForm { get; set; }

		public string DocumentUniqueCode { get; set; }

		*/
	}

	partial class CashOutOrderPaymentContractService
	{
		/*
		public CashOutOrderPaymentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.SavePosted = r.SavePosted;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.SavePosted = c.SavePosted + db;
			};
		}
		*/
	}

	#endregion
	

	#region CateringTypeDto
		
	partial class Contracts
	{
		public CateringTypeContractService CateringType { [DebuggerStepThrough] get { return ResolveService(ref _cateringTypeDto); } }
		private CateringTypeContractService _cateringTypeDto;

		[DebuggerStepThrough]
		public ItemResponse Update(CateringTypeDto r, RangeRequest prms)
		{
			return CateringType.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(CateringTypeDto r, string typeName, RangeRequest prms)
		{
			return CateringType.Update(r, typeName, prms);
		}
	}

	partial class CateringTypeDto
	{
		/*
		*/
	}

	partial class CateringTypeContractService
	{
		/*
		public CateringTypeContractService()
		{
			ContractFromEntity += (r, c) =>
			{
			};
		
			EntityFromContract += (r, c) =>
			{
			};
		}
		*/
	}

	#endregion
	

	#region CheckPaymentDto
		
	partial class Contracts
	{
		public CheckPaymentContractService CheckPayment { [DebuggerStepThrough] get { return ResolveService(ref _checkPaymentDto); } }
		private CheckPaymentContractService _checkPaymentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(CheckPaymentDto r, RangeRequest prms)
		{
			return CheckPayment.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(CheckPaymentDto r, string typeName, RangeRequest prms)
		{
			return CheckPayment.Update(r, typeName, prms);
		}
	}

	partial class CheckPaymentDto
	{
		/*
		public int PaymentForm { get; set; }

		*/
	}

	partial class CheckPaymentContractService
	{
		/*
		public CheckPaymentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PaymentForm = r.PaymentForm;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.PaymentForm = (PaymentForm)c.PaymentForm + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region ClosedPeriodDto
		
	partial class Contracts
	{
		public ClosedPeriodContractService ClosedPeriod { [DebuggerStepThrough] get { return ResolveService(ref _closedPeriodDto); } }
		private ClosedPeriodContractService _closedPeriodDto;

		[DebuggerStepThrough]
		public bool CanUpdate(ClosedPeriodDto r)
		{
			return ClosedPeriod.CanUpdate(r);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ClosedPeriodDto r, RangeRequest prms)
		{
			return ClosedPeriod.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ClosedPeriodDto r, string typeName, RangeRequest prms)
		{
			return ClosedPeriod.Update(r, typeName, prms);
		}
	}

	partial class ClosedPeriodDto
	{
		/*
		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }

		public int PeriodState { get; set; }

		*/
	}

	partial class ClosedPeriodContractService
	{
		/*
		public ClosedPeriodContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.DateFrom = r.DateFrom;
				c.DateTo = r.DateTo;
				c.PeriodState = r.PeriodState;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.DateFrom = c.DateFrom + db;
				r.DateTo = c.DateTo + db;
				r.PeriodState = (PeriodState)c.PeriodState + db;
			};
		}
		*/
	}

	#endregion
	

	#region ConsignmentDto
		
	partial class Contracts
	{
		public ConsignmentContractService Consignment { [DebuggerStepThrough] get { return ResolveService(ref _consignmentDto); } }
		private ConsignmentContractService _consignmentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(ConsignmentDto r, RangeRequest prms)
		{
			return Consignment.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ConsignmentDto r, string typeName, RangeRequest prms)
		{
			return Consignment.Update(r, typeName, prms);
		}
	}

	partial class ConsignmentDto
	{
		/*
		public string Number { get; set; }

		public DateTime IssueDate { get; set; }

		public Party.Reference Supplier { get; set; }

		public Party.Reference Acquirer { get; set; }

		public string AcquirerCode { get; set; }

		public Order.Reference Order { get; set; }

		public Money GrandTotal { get; set; }

		public Money Vat { get; set; }

		public Money Total { get; set; }

		public Money Discount { get; set; }

		public string TotalSupplied { get; set; }

		public IList`1 OrderItems { get; set; }

		public IList`1 IssuedConsignments { get; set; }

		*/
	}

	partial class ConsignmentContractService
	{
		/*
		public ConsignmentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.Supplier = r.Supplier;
				c.Acquirer = r.Acquirer;
				c.Order = r.Order;
				c.Vat = r.Vat;
				c.GrandTotal = r.GrandTotal;
				c.Total = r.Total;
				c.Discount = r.Discount;
				c.TotalSupplied = r.TotalSupplied;
				// c.Items = r.Items; !!! unknown property
				// c.IssuedConsignments = r.IssuedConsignments; ??? is array
				// c.Permissions = r.Permissions; !!! unknown property
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.IssueDate = c.IssueDate + db;
				r.Supplier = c.Supplier + db;
				r.Acquirer = c.Acquirer + db;
				r.Order = c.Order + db;
				r.Vat = c.Vat + db;
				r.GrandTotal = c.GrandTotal + db;
				// r.Total = c.Total + db; !!! property is non-writable
				r.Discount = c.Discount + db;
				r.TotalSupplied = c.TotalSupplied + db;
				// r.Items = c.Items + db; !!! unknown property
				// r.IssuedConsignments = c.IssuedConsignments + db; !!! property is non-writable
				// r.Permissions = c.Permissions + db; !!! unknown property
			};
		}
		*/
	}

	#endregion
	

	#region ContractDto
		
	partial class Contracts
	{
		public ContractContractService Contract { [DebuggerStepThrough] get { return ResolveService(ref _contractDto); } }
		private ContractContractService _contractDto;

		[DebuggerStepThrough]
		public ItemResponse Update(ContractDto r, RangeRequest prms)
		{
			return Contract.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ContractDto r, string typeName, RangeRequest prms)
		{
			return Contract.Update(r, typeName, prms);
		}
	}

	partial class ContractDto
	{
		/*
		public Organization.Reference Customer { get; set; }

		public string Number { get; set; }

		public DateTime? IssueDate { get; set; }

		public Decimal DiscountPc { get; set; }

		public string Note { get; set; }

		*/
	}

	partial class ContractContractService
	{
		/*
		public ContractContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Customer = r.Customer;
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.DiscountPc = r.DiscountPc;
				c.Note = r.Note;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Customer = c.Customer + db;
				r.Number = c.Number + db;
				r.IssueDate = c.IssueDate + db;
				r.DiscountPc = c.DiscountPc + db;
				r.Note = c.Note + db;
			};
		}
		*/
	}

	#endregion
	

	#region CountryDto
		
	partial class Contracts
	{
		public CountryContractService Country { [DebuggerStepThrough] get { return ResolveService(ref _countryDto); } }
		private CountryContractService _countryDto;

		[DebuggerStepThrough]
		public ItemResponse Update(CountryDto r, RangeRequest prms)
		{
			return Country.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(CountryDto r, string typeName, RangeRequest prms)
		{
			return Country.Update(r, typeName, prms);
		}
	}

	partial class CountryDto
	{
		/*
		public string TwoCharCode { get; set; }

		public string ThreeCharCode { get; set; }

		public string Note { get; set; }

		*/
	}

	partial class CountryContractService
	{
		/*
		public CountryContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.TwoCharCode = r.TwoCharCode;
				c.ThreeCharCode = r.ThreeCharCode;
				c.Note = r.Note;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.TwoCharCode = c.TwoCharCode + db;
				r.ThreeCharCode = c.ThreeCharCode + db;
				r.Note = c.Note + db;
			};
		}
		*/
	}

	#endregion
	

	#region CurrencyDto
		
	partial class Contracts
	{
		public CurrencyContractService Currency { [DebuggerStepThrough] get { return ResolveService(ref _currencyDto); } }
		private CurrencyContractService _currencyDto;

		[DebuggerStepThrough]
		public ItemResponse Update(CurrencyDto r, RangeRequest prms)
		{
			return Currency.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(CurrencyDto r, string typeName, RangeRequest prms)
		{
			return Currency.Update(r, typeName, prms);
		}
	}

	partial class CurrencyDto
	{
		/*
		public string Code { get; set; }

		public int NumericCode { get; set; }

		public string CyrillicCode { get; set; }

		*/
	}

	partial class CurrencyContractService
	{
		/*
		public CurrencyContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Code = r.Code;
				c.NumericCode = r.NumericCode;
				c.CyrillicCode = r.CyrillicCode;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Code = c.Code + db;
				r.NumericCode = c.NumericCode + db;
				r.CyrillicCode = c.CyrillicCode + db;
			};
		}
		*/
	}

	#endregion
	

	#region CurrencyDailyRateDto
		
	partial class Contracts
	{
		public CurrencyDailyRateContractService CurrencyDailyRate { [DebuggerStepThrough] get { return ResolveService(ref _currencyDailyRateDto); } }
		private CurrencyDailyRateContractService _currencyDailyRateDto;

		[DebuggerStepThrough]
		public ItemResponse Update(CurrencyDailyRateDto r, RangeRequest prms)
		{
			return CurrencyDailyRate.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(CurrencyDailyRateDto r, string typeName, RangeRequest prms)
		{
			return CurrencyDailyRate.Update(r, typeName, prms);
		}
	}

	partial class CurrencyDailyRateDto
	{
		/*
		public DateTime Date { get; set; }

		public Nullable`1 UAH_EUR { get; set; }

		public Nullable`1 UAH_RUB { get; set; }

		public Nullable`1 UAH_USD { get; set; }

		public Nullable`1 RUB_EUR { get; set; }

		public Nullable`1 RUB_USD { get; set; }

		public Nullable`1 EUR_USD { get; set; }

		*/
	}

	partial class CurrencyDailyRateContractService
	{
		/*
		public CurrencyDailyRateContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Date = r.Date;
				c.UAH_EUR = r.UAH_EUR;
				c.UAH_RUB = r.UAH_RUB;
				c.UAH_USD = r.UAH_USD;
				c.RUB_EUR = r.RUB_EUR;
				c.RUB_USD = r.RUB_USD;
				c.EUR_USD = r.EUR_USD;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Date = c.Date + db;
				r.UAH_EUR = c.UAH_EUR + db;
				r.UAH_RUB = c.UAH_RUB + db;
				r.UAH_USD = c.UAH_USD + db;
				r.RUB_EUR = c.RUB_EUR + db;
				r.RUB_USD = c.RUB_USD + db;
				r.EUR_USD = c.EUR_USD + db;
			};
		}
		*/
	}

	#endregion
	

	#region DepartmentDto
		
	partial class Contracts
	{
		public DepartmentContractService Department { [DebuggerStepThrough] get { return ResolveService(ref _departmentDto); } }
		private DepartmentContractService _departmentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(DepartmentDto r, RangeRequest prms)
		{
			return Department.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(DepartmentDto r, string typeName, RangeRequest prms)
		{
			return Department.Update(r, typeName, prms);
		}
	}

	partial class DepartmentDto
	{
		/*
		public int Type { get; set; }

		public Organization.Reference Organization { get; set; }

		*/
	}

	partial class DepartmentContractService
	{
		/*
		public DepartmentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Organization = r.Organization;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Organization = c.Organization + db;
			};
		}
		*/
	}

	#endregion
	

	#region DepartmentListDetailDto
		
	partial class Contracts
	{
		public DepartmentListDetailContractService DepartmentListDetail { [DebuggerStepThrough] get { return ResolveService(ref _departmentListDetailDto); } }
		private DepartmentListDetailContractService _departmentListDetailDto;

		[DebuggerStepThrough]
		public ItemResponse Update(DepartmentListDetailDto r, RangeRequest prms)
		{
			return DepartmentListDetail.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(DepartmentListDetailDto r, string typeName, RangeRequest prms)
		{
			return DepartmentListDetail.Update(r, typeName, prms);
		}
	}

	partial class DepartmentListDetailDto
	{
		/*
		public int Type { get; set; }

		public Organization.Reference Organization { get; set; }

		*/
	}

	partial class DepartmentListDetailContractService
	{
		/*
		public DepartmentListDetailContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = r.Type;
				c.Organization = r.Organization;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = (PartyType)c.Type + db; !!! property is non-writable
				r.Organization = c.Organization + db;
			};
		}
		*/
	}

	#endregion
	

	#region DocumentAccessDto
		
	partial class Contracts
	{
		public DocumentAccessContractService DocumentAccess { [DebuggerStepThrough] get { return ResolveService(ref _documentAccessDto); } }
		private DocumentAccessContractService _documentAccessDto;

		[DebuggerStepThrough]
		public ItemResponse Update(DocumentAccessDto r, RangeRequest prms)
		{
			return DocumentAccess.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(DocumentAccessDto r, string typeName, RangeRequest prms)
		{
			return DocumentAccess.Update(r, typeName, prms);
		}
	}

	partial class DocumentAccessDto
	{
		/*
		public Person.Reference Person { get; set; }

		public Party.Reference Owner { get; set; }

		public bool FullDocumentControl { get; set; }

		*/
	}

	partial class DocumentAccessContractService
	{
		/*
		public DocumentAccessContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Person = r.Person;
				c.Owner = r.Owner;
				c.FullDocumentControl = r.FullDocumentControl;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Person = c.Person + db;
				r.Owner = c.Owner + db;
				r.FullDocumentControl = c.FullDocumentControl + db;
			};
		}
		*/
	}

	#endregion
	

	#region DocumentOwnerDto
		
	partial class Contracts
	{
		public DocumentOwnerContractService DocumentOwner { [DebuggerStepThrough] get { return ResolveService(ref _documentOwnerDto); } }
		private DocumentOwnerContractService _documentOwnerDto;

		[DebuggerStepThrough]
		public ItemResponse Update(DocumentOwnerDto r, RangeRequest prms)
		{
			return DocumentOwner.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(DocumentOwnerDto r, string typeName, RangeRequest prms)
		{
			return DocumentOwner.Update(r, typeName, prms);
		}
	}

	partial class DocumentOwnerDto
	{
		/*
		public Party.Reference Owner { get; set; }

		public bool IsActive { get; set; }

		*/
	}

	partial class DocumentOwnerContractService
	{
		/*
		public DocumentOwnerContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Owner = r.Owner;
				c.IsActive = r.IsActive;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Owner = c.Owner + db;
				r.IsActive = c.IsActive + db;
			};
		}
		*/
	}

	#endregion
	

	#region ElectronicPaymentDto
		
	partial class Contracts
	{
		public ElectronicPaymentContractService ElectronicPayment { [DebuggerStepThrough] get { return ResolveService(ref _electronicPaymentDto); } }
		private ElectronicPaymentContractService _electronicPaymentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(ElectronicPaymentDto r, RangeRequest prms)
		{
			return ElectronicPayment.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ElectronicPaymentDto r, string typeName, RangeRequest prms)
		{
			return ElectronicPayment.Update(r, typeName, prms);
		}
	}

	partial class ElectronicPaymentDto
	{
		/*
		public int PaymentForm { get; set; }

		public string AuthorizationCode { get; set; }

		*/
	}

	partial class ElectronicPaymentContractService
	{
		/*
		public ElectronicPaymentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.AuthorizationCode = r.AuthorizationCode;
				c.PaymentSystem = r.PaymentSystem;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.AuthorizationCode = c.AuthorizationCode + db;
				r.PaymentSystem = c.PaymentSystem + db;
			};
		}
		*/
	}

	#endregion
	

	#region ExcursionDto
		
	partial class Contracts
	{
		public ExcursionContractService Excursion { [DebuggerStepThrough] get { return ResolveService(ref _excursionDto); } }
		private ExcursionContractService _excursionDto;

		[DebuggerStepThrough]
		public ItemResponse Update(ExcursionDto r, RangeRequest prms)
		{
			return Excursion.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ExcursionDto r, string typeName, RangeRequest prms)
		{
			return Excursion.Update(r, typeName, prms);
		}
	}

	partial class ExcursionDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public string TourName { get; set; }

		*/
	}

	partial class ExcursionContractService
	{
		/*
		public ExcursionContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Passengers = r.Passengers; ??? is array
				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;
				c.TourName = r.TourName;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Passengers = c.Passengers + db; ??? is array
				r.StartDate = c.StartDate + db;
				r.FinishDate = c.FinishDate + db;
				r.TourName = c.TourName + db;
			};
		}
		*/
	}

	#endregion
	

	#region FileDto
		
	partial class Contracts
	{
		public FileContractService File { [DebuggerStepThrough] get { return ResolveService(ref _fileDto); } }
		private FileContractService _fileDto;

		[DebuggerStepThrough]
		public ItemResponse Update(FileDto r, RangeRequest prms)
		{
			return File.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(FileDto r, string typeName, RangeRequest prms)
		{
			return File.Update(r, typeName, prms);
		}
	}

	partial class FileDto
	{
		/*
		public string FileName { get; set; }

		public DateTime TimeStamp { get; set; }

		public Byte[] Content { get; set; }

		public Person.Reference UploadedBy { get; set; }

		public Party.Reference Party { get; set; }

		*/
	}

	partial class FileContractService
	{
		/*
		public FileContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.FileName = r.FileName;
				c.TimeStamp = r.TimeStamp;
				c.UploadedBy = r.UploadedBy;
				c.Party = r.Party;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.FileName = c.FileName + db;
				r.TimeStamp = c.TimeStamp + db;
				r.UploadedBy = c.UploadedBy + db;
				r.Party = c.Party + db;
			};
		}
		*/
	}

	#endregion
	

	#region FlightSegmentDto
		
	partial class Contracts
	{
		public FlightSegmentContractService FlightSegment { [DebuggerStepThrough] get { return ResolveService(ref _flightSegmentDto); } }
		private FlightSegmentContractService _flightSegmentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(FlightSegmentDto r, RangeRequest prms)
		{
			return FlightSegment.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(FlightSegmentDto r, string typeName, RangeRequest prms)
		{
			return FlightSegment.Update(r, typeName, prms);
		}
	}

	partial class FlightSegmentDto
	{
		/*
		public AviaTicket.Reference Ticket { get; set; }

		public int Position { get; set; }

		public int Type { get; set; }

		public string FromAirportCode { get; set; }

		public string FromAirportName { get; set; }

		public Airport.Reference FromAirport { get; set; }

		public Country.Reference FromCountry { get; set; }

		public string ToAirportCode { get; set; }

		public string ToAirportName { get; set; }

		public Airport.Reference ToAirport { get; set; }

		public Country.Reference ToCountry { get; set; }

		public string CarrierIataCode { get; set; }

		public string CarrierPrefixCode { get; set; }

		public string CarrierName { get; set; }

		public Organization.Reference Carrier { get; set; }

		public Organization.Reference Operator { get; set; }

		public string FlightNumber { get; set; }

		public AirplaneModel.Reference Equipment { get; set; }

		public string ServiceClassCode { get; set; }

		public Nullable`1 ServiceClass { get; set; }

		public DateTime? DepartureTime { get; set; }

		public DateTime? ArrivalTime { get; set; }

		public string MealCodes { get; set; }

		public Nullable`1 MealTypes { get; set; }

		public int? NumberOfStops { get; set; }

		public string Luggage { get; set; }

		public string CheckInTerminal { get; set; }

		public string CheckInTime { get; set; }

		public string Duration { get; set; }

		public string ArrivalTerminal { get; set; }

		public string Seat { get; set; }

		public string FareBasis { get; set; }

		public bool Stopover { get; set; }

		public Nullable`1 Surcharges { get; set; }

		public bool IsInclusive { get; set; }

		public Nullable`1 Fare { get; set; }

		public Nullable`1 StopoverOrTransferCharge { get; set; }

		public bool IsSideTrip { get; set; }

		public Double Distance { get; set; }

		public Money Amount { get; set; }

		public Money CouponAmount { get; set; }

		*/
	}

	partial class FlightSegmentContractService
	{
		/*
		public FlightSegmentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.FromAirport = r.FromAirport;
				c.FromAirportName = r.FromAirportName;
				c.ToAirport = r.ToAirport;
				c.ToAirportName = r.ToAirportName;
				// c.CarrierCode = r.CarrierCode; !!! unknown property
				c.Carrier = r.Carrier;
				c.Operator = r.Operator;
				c.FlightNumber = r.FlightNumber;
				c.Equipment = r.Equipment;
				c.ServiceClassCode = r.ServiceClassCode;
				c.ServiceClass = r.ServiceClass;
				// c.ServiceClassName = r.ServiceClassName; !!! unknown property
				c.DepartureTime = r.DepartureTime;
				c.ArrivalTime = r.ArrivalTime;
				c.MealTypes = r.MealTypes;
				c.NumberOfStops = r.NumberOfStops;
				c.Luggage = r.Luggage;
				c.CheckInTerminal = r.CheckInTerminal;
				c.CheckInTime = r.CheckInTime;
				c.Duration = r.Duration;
				c.ArrivalTerminal = r.ArrivalTerminal;
				c.Seat = r.Seat;
				c.FareBasis = r.FareBasis;
				c.Position = r.Position;
				c.Stopover = r.Stopover;
				c.Type = r.Type;
				c.CouponAmount = r.CouponAmount;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.FromAirport = c.FromAirport + db;
				r.FromAirportName = c.FromAirportName + db;
				r.ToAirport = c.ToAirport + db;
				r.ToAirportName = c.ToAirportName + db;
				// r.CarrierCode = c.CarrierCode + db; !!! unknown property
				r.Carrier = c.Carrier + db;
				r.Operator = c.Operator + db;
				r.FlightNumber = c.FlightNumber + db;
				r.Equipment = c.Equipment + db;
				r.ServiceClassCode = c.ServiceClassCode + db;
				r.ServiceClass = c.ServiceClass + db;
				// r.ServiceClassName = c.ServiceClassName + db; !!! unknown property
				r.DepartureTime = c.DepartureTime + db;
				r.ArrivalTime = c.ArrivalTime + db;
				r.MealTypes = c.MealTypes + db;
				r.NumberOfStops = c.NumberOfStops + db;
				r.Luggage = c.Luggage + db;
				r.CheckInTerminal = c.CheckInTerminal + db;
				r.CheckInTime = c.CheckInTime + db;
				r.Duration = c.Duration + db;
				r.ArrivalTerminal = c.ArrivalTerminal + db;
				r.Seat = c.Seat + db;
				r.FareBasis = c.FareBasis + db;
				r.Position = c.Position + db;
				r.Stopover = c.Stopover + db;
				r.Type = (FlightSegmentType)c.Type + db;
				r.CouponAmount = c.CouponAmount + db;
			};
		}
		*/
	}

	#endregion
	

	#region GdsAgentDto
		
	partial class Contracts
	{
		public GdsAgentContractService GdsAgent { [DebuggerStepThrough] get { return ResolveService(ref _gdsAgentDto); } }
		private GdsAgentContractService _gdsAgentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(GdsAgentDto r, RangeRequest prms)
		{
			return GdsAgent.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(GdsAgentDto r, string typeName, RangeRequest prms)
		{
			return GdsAgent.Update(r, typeName, prms);
		}
	}

	partial class GdsAgentDto
	{
		/*
		public Person.Reference Person { get; set; }

		public int Origin { get; set; }

		public string Code { get; set; }

		public string OfficeCode { get; set; }

		public Organization.Reference Provider { get; set; }

		public Party.Reference Office { get; set; }

		public Organization.Reference LegalEntity { get; set; }

		*/
	}

	partial class GdsAgentContractService
	{
		/*
		public GdsAgentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Person = r.Person;
				c.Origin = (int)r.Origin;
				c.Code = r.Code;
				c.OfficeCode = r.OfficeCode;
				c.Provider = r.Provider;
				c.Office = r.Office;
				c.LegalEntity = r.LegalEntity;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Person = c.Person + db;
				r.Origin = (ProductOrigin)c.Origin + db;
				r.Code = c.Code + db;
				r.OfficeCode = c.OfficeCode + db;
				r.Provider = c.Provider + db;
				r.Office = c.Office + db;
				r.LegalEntity = c.LegalEntity + db;
			};
		}
		*/
	}

	#endregion
	

	#region GdsFileDto
		
	partial class Contracts
	{
		public GdsFileContractService GdsFile { [DebuggerStepThrough] get { return ResolveService(ref _gdsFileDto); } }
		private GdsFileContractService _gdsFileDto;

		[DebuggerStepThrough]
		public ItemResponse Update(GdsFileDto r, RangeRequest prms)
		{
			return GdsFile.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(GdsFileDto r, string typeName, RangeRequest prms)
		{
			return GdsFile.Update(r, typeName, prms);
		}
	}

	partial class GdsFileDto
	{
		/*
		public int FileType { get; set; }

		public DateTime TimeStamp { get; set; }

		public string Content { get; set; }

		public int ImportResult { get; set; }

		public string ImportOutput { get; set; }

		*/
	}

	partial class GdsFileContractService
	{
		/*
		public GdsFileContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.FileType = (int)r.FileType;
				c.TimeStamp = r.TimeStamp;
				c.Content = r.Content;
				c.ImportResult = (int)r.ImportResult;
				c.ImportOutput = r.ImportOutput;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.FileType = (GdsFileType)c.FileType + db;
				r.TimeStamp = c.TimeStamp + db;
				r.Content = c.Content + db;
				r.ImportResult = (ImportResult)c.ImportResult + db;
				r.ImportOutput = c.ImportOutput + db;
			};
		}
		*/
	}

	#endregion
	

	#region GenericProductDto
		
	partial class Contracts
	{
		public GenericProductContractService GenericProduct { [DebuggerStepThrough] get { return ResolveService(ref _genericProductDto); } }
		private GenericProductContractService _genericProductDto;

		[DebuggerStepThrough]
		public ItemResponse Update(GenericProductDto r, RangeRequest prms)
		{
			return GenericProduct.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(GenericProductDto r, string typeName, RangeRequest prms)
		{
			return GenericProduct.Update(r, typeName, prms);
		}
	}

	partial class GenericProductDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

		public GenericProductType.Reference GenericType { get; set; }

		public string Number { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		*/
	}

	partial class GenericProductContractService
	{
		/*
		public GenericProductContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Passengers = r.Passengers; ??? is array
				c.GenericType = r.GenericType;
				c.Number = r.Number;
				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Passengers = c.Passengers + db; ??? is array
				r.GenericType = c.GenericType + db;
				r.Number = c.Number + db;
				r.StartDate = c.StartDate + db;
				r.FinishDate = c.FinishDate + db;
			};
		}
		*/
	}

	#endregion
	

	#region GenericProductTypeDto
		
	partial class Contracts
	{
		public GenericProductTypeContractService GenericProductType { [DebuggerStepThrough] get { return ResolveService(ref _genericProductTypeDto); } }
		private GenericProductTypeContractService _genericProductTypeDto;

		[DebuggerStepThrough]
		public ItemResponse Update(GenericProductTypeDto r, RangeRequest prms)
		{
			return GenericProductType.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(GenericProductTypeDto r, string typeName, RangeRequest prms)
		{
			return GenericProductType.Update(r, typeName, prms);
		}
	}

	partial class GenericProductTypeDto
	{
		/*
		*/
	}

	partial class GenericProductTypeContractService
	{
		/*
		public GenericProductTypeContractService()
		{
			ContractFromEntity += (r, c) =>
			{
			};
		
			EntityFromContract += (r, c) =>
			{
			};
		}
		*/
	}

	#endregion
	

	#region InsuranceDto
		
	partial class Contracts
	{
		public InsuranceContractService Insurance { [DebuggerStepThrough] get { return ResolveService(ref _insuranceDto); } }
		private InsuranceContractService _insuranceDto;

		[DebuggerStepThrough]
		public ItemResponse Update(InsuranceDto r, RangeRequest prms)
		{
			return Insurance.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(InsuranceDto r, string typeName, RangeRequest prms)
		{
			return Insurance.Update(r, typeName, prms);
		}
	}

	partial class InsuranceDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

		public string Number { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		*/
	}

	partial class InsuranceContractService
	{
		/*
		public InsuranceContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Passengers = r.Passengers; ??? is array
				c.Number = r.Number;
				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Passengers = c.Passengers + db; ??? is array
				r.Number = c.Number + db;
				r.StartDate = c.StartDate + db;
				r.FinishDate = c.FinishDate + db;
			};
		}
		*/
	}

	#endregion
	

	#region InsuranceRefundDto
		
	partial class Contracts
	{
		public InsuranceRefundContractService InsuranceRefund { [DebuggerStepThrough] get { return ResolveService(ref _insuranceRefundDto); } }
		private InsuranceRefundContractService _insuranceRefundDto;

		[DebuggerStepThrough]
		public ItemResponse Update(InsuranceRefundDto r, RangeRequest prms)
		{
			return InsuranceRefund.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(InsuranceRefundDto r, string typeName, RangeRequest prms)
		{
			return InsuranceRefund.Update(r, typeName, prms);
		}
	}

	partial class InsuranceRefundDto
	{
		/*
		public int Type { get; set; }

		public bool IsRefund { get; set; }

		public string Name { get; set; }

		*/
	}

	partial class InsuranceRefundContractService
	{
		/*
		public InsuranceRefundContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = r.Type;
				c.IsRefund = r.IsRefund;
				c.Name = r.Name;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = (ProductType)c.Type + db; !!! property is non-writable
				// r.IsRefund = c.IsRefund + db; !!! property is non-writable
				// r.Name = c.Name + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region InternalTransferDto
		
	partial class Contracts
	{
		public InternalTransferContractService InternalTransfer { [DebuggerStepThrough] get { return ResolveService(ref _internalTransferDto); } }
		private InternalTransferContractService _internalTransferDto;

		[DebuggerStepThrough]
		public ItemResponse Update(InternalTransferDto r, RangeRequest prms)
		{
			return InternalTransfer.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(InternalTransferDto r, string typeName, RangeRequest prms)
		{
			return InternalTransfer.Update(r, typeName, prms);
		}
	}

	partial class InternalTransferDto
	{
		/*
		public string Number { get; set; }

		public DateTime Date { get; set; }

		public Order.Reference FromOrder { get; set; }

		public Party.Reference FromParty { get; set; }

		public Order.Reference ToOrder { get; set; }

		public Party.Reference ToParty { get; set; }

		public Decimal Amount { get; set; }

		*/
	}

	partial class InternalTransferContractService
	{
		/*
		public InternalTransferContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Date = r.Date;
				c.FromParty = r.FromParty;
				c.FromOrder = r.FromOrder;
				c.ToParty = r.ToParty;
				c.ToOrder = r.ToOrder;
				c.Amount = r.Amount;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Date = c.Date + db;
				r.FromParty = c.FromParty + db;
				r.FromOrder = c.FromOrder + db;
				r.ToParty = c.ToParty + db;
				r.ToOrder = c.ToOrder + db;
				r.Amount = c.Amount + db;
			};
		}
		*/
	}

	#endregion
	

	#region InvoiceDto
		
	partial class Contracts
	{
		public InvoiceContractService Invoice { [DebuggerStepThrough] get { return ResolveService(ref _invoiceDto); } }
		private InvoiceContractService _invoiceDto;

		[DebuggerStepThrough]
		public ItemResponse Update(InvoiceDto r, RangeRequest prms)
		{
			return Invoice.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(InvoiceDto r, string typeName, RangeRequest prms)
		{
			return Invoice.Update(r, typeName, prms);
		}
	}

	partial class InvoiceDto
	{
		/*
		public string Number { get; set; }

		public string Agreement { get; set; }

		public DateTime IssueDate { get; set; }

		public DateTime TimeStamp { get; set; }

		public int Type { get; set; }

		public Byte[] Content { get; set; }

		public Order.Reference Order { get; set; }

		public Party.Reference Customer { get; set; }

		public Party.Reference BillTo { get; set; }

		public string BillToName { get; set; }

		public Party.Reference ShipTo { get; set; }

		public bool IsOrderVoid { get; set; }

		public Party.Reference Owner { get; set; }

		public Person.Reference IssuedBy { get; set; }

		public Money Total { get; set; }

		public Money Vat { get; set; }

		public string FileExtension { get; set; }

		*/
	}

	partial class InvoiceContractService
	{
		/*
		public InvoiceContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.TimeStamp = r.TimeStamp;
				c.Order = r.Order;
				c.IssuedBy = r.IssuedBy;
				c.Total = r.Total;
				c.Vat = r.Vat;
				c.Type = r.Type;
				c.FileExtension = r.FileExtension;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.IssueDate = c.IssueDate + db;
				r.TimeStamp = c.TimeStamp + db;
				r.Order = c.Order + db;
				r.IssuedBy = c.IssuedBy + db;
				r.Total = c.Total + db;
				r.Vat = c.Vat + db;
				r.Type = (InvoiceType)c.Type + db;
				r.FileExtension = c.FileExtension + db;
			};
		}
		*/
	}

	#endregion
	

	#region IsicDto
		
	partial class Contracts
	{
		public IsicContractService Isic { [DebuggerStepThrough] get { return ResolveService(ref _isicDto); } }
		private IsicContractService _isicDto;

		[DebuggerStepThrough]
		public ItemResponse Update(IsicDto r, RangeRequest prms)
		{
			return Isic.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(IsicDto r, string typeName, RangeRequest prms)
		{
			return Isic.Update(r, typeName, prms);
		}
	}

	partial class IsicDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

		public Person.Reference Passenger { get; set; }

		public int CardType { get; set; }

		public string Number1 { get; set; }

		public string Number2 { get; set; }

		*/
	}

	partial class IsicContractService
	{
		/*
		public IsicContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PassengerName = r.PassengerName;
				c.Passenger = r.Passenger;
				c.CardType = (int)r.CardType;
				c.Number1 = r.Number1;
				c.Number2 = r.Number2;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.PassengerName = c.PassengerName + db;
				r.Passenger = c.Passenger + db;
				r.CardType = (IsicCardType)c.CardType + db;
				r.Number1 = c.Number1 + db;
				r.Number2 = c.Number2 + db;
			};
		}
		*/
	}

	#endregion
	

	#region IssuedConsignmentDto
		
	partial class Contracts
	{
		public IssuedConsignmentContractService IssuedConsignment { [DebuggerStepThrough] get { return ResolveService(ref _issuedConsignmentDto); } }
		private IssuedConsignmentContractService _issuedConsignmentDto;

		[DebuggerStepThrough]
		public ItemResponse Update(IssuedConsignmentDto r, RangeRequest prms)
		{
			return IssuedConsignment.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(IssuedConsignmentDto r, string typeName, RangeRequest prms)
		{
			return IssuedConsignment.Update(r, typeName, prms);
		}
	}

	partial class IssuedConsignmentDto
	{
		/*
		public string Number { get; set; }

		public DateTime TimeStamp { get; set; }

		public Byte[] Content { get; set; }

		public Consignment.Reference Consignment { get; set; }

		public Person.Reference IssuedBy { get; set; }

		*/
	}

	partial class IssuedConsignmentContractService
	{
		/*
		public IssuedConsignmentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.TimeStamp = r.TimeStamp;
				c.Consignment = r.Consignment;
				c.IssuedBy = r.IssuedBy;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.TimeStamp = c.TimeStamp + db;
				r.Consignment = c.Consignment + db;
				r.IssuedBy = c.IssuedBy + db;
			};
		}
		*/
	}

	#endregion
	

	#region MilesCardDto
		
	partial class Contracts
	{
		public MilesCardContractService MilesCard { [DebuggerStepThrough] get { return ResolveService(ref _milesCardDto); } }
		private MilesCardContractService _milesCardDto;

		[DebuggerStepThrough]
		public ItemResponse Update(MilesCardDto r, RangeRequest prms)
		{
			return MilesCard.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(MilesCardDto r, string typeName, RangeRequest prms)
		{
			return MilesCard.Update(r, typeName, prms);
		}
	}

	partial class MilesCardDto
	{
		/*
		public Person.Reference Owner { get; set; }

		public string Number { get; set; }

		public Organization.Reference Organization { get; set; }

		*/
	}

	partial class MilesCardContractService
	{
		/*
		public MilesCardContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.Organization = r.Organization;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.Organization = c.Organization + db;
			};
		}
		*/
	}

	#endregion
	

	#region ModificationDto
		
	partial class Contracts
	{
		public ModificationContractService Modification { [DebuggerStepThrough] get { return ResolveService(ref _modificationDto); } }
		private ModificationContractService _modificationDto;

		[DebuggerStepThrough]
		public ItemResponse Update(ModificationDto r, RangeRequest prms)
		{
			return Modification.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ModificationDto r, string typeName, RangeRequest prms)
		{
			return Modification.Update(r, typeName, prms);
		}
	}

	partial class ModificationDto
	{
		/*
		public DateTime TimeStamp { get; set; }

		public string Author { get; set; }

		public int Type { get; set; }

		public string InstanceType { get; set; }

		public string InstanceId { get; set; }

		public string InstanceString { get; set; }

		public string Comment { get; set; }

		public IDictionary`2 Items { get; set; }

		public string ItemsJson { get; set; }

		*/
	}

	partial class ModificationContractService
	{
		/*
		public ModificationContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.TimeStamp = r.TimeStamp;
				c.Author = r.Author;
				c.Type = (int)r.Type;
				c.InstanceType = r.InstanceType;
				c.InstanceId = r.InstanceId;
				c.InstanceString = r.InstanceString;
				c.Comment = r.Comment;
				c.ItemsJson = r.ItemsJson;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.TimeStamp = c.TimeStamp + db;
				r.Author = c.Author + db;
				r.Type = (ModificationType)c.Type + db;
				r.InstanceType = c.InstanceType + db;
				r.InstanceId = c.InstanceId + db;
				r.InstanceString = c.InstanceString + db;
				r.Comment = c.Comment + db;
				// r.ItemsJson = c.ItemsJson + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region OpeningBalanceDto
		
	partial class Contracts
	{
		public OpeningBalanceContractService OpeningBalance { [DebuggerStepThrough] get { return ResolveService(ref _openingBalanceDto); } }
		private OpeningBalanceContractService _openingBalanceDto;

		[DebuggerStepThrough]
		public ItemResponse Update(OpeningBalanceDto r, RangeRequest prms)
		{
			return OpeningBalance.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(OpeningBalanceDto r, string typeName, RangeRequest prms)
		{
			return OpeningBalance.Update(r, typeName, prms);
		}
	}

	partial class OpeningBalanceDto
	{
		/*
		public string Number { get; set; }

		public DateTime Date { get; set; }

		public Party.Reference Party { get; set; }

		public Decimal Balance { get; set; }

		*/
	}

	partial class OpeningBalanceContractService
	{
		/*
		public OpeningBalanceContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.Date = r.Date;
				c.Party = r.Party;
				c.Balance = r.Balance;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.Date = c.Date + db;
				r.Party = c.Party + db;
				r.Balance = c.Balance + db;
			};
		}
		*/
	}

	#endregion
	

	#region OrderDto
		
	partial class Contracts
	{
		public OrderContractService Order { [DebuggerStepThrough] get { return ResolveService(ref _orderDto); } }
		private OrderContractService _orderDto;

		[DebuggerStepThrough]
		public ItemResponse Update(OrderDto r, RangeRequest prms)
		{
			return Order.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(OrderDto r, string typeName, RangeRequest prms)
		{
			return Order.Update(r, typeName, prms);
		}
	}

	partial class OrderDto
	{
		/*
		public string Number { get; set; }

		public DateTime IssueDate { get; set; }

		public bool IsVoid { get; set; }

		public Party.Reference Customer { get; set; }

		public Party.Reference BillTo { get; set; }

		public string BillToName { get; set; }

		public Party.Reference ShipTo { get; set; }

		public Party.Reference Intermediary { get; set; }

		public Money Discount { get; set; }

		public Money Vat { get; set; }

		public bool UseServiceFeeOnlyInVat { get; set; }

		public Money Total { get; set; }

		public Money Paid { get; set; }

		public Money CheckPaid { get; set; }

		public Money WirePaid { get; set; }

		public Money CreditPaid { get; set; }

		public Money RestPaid { get; set; }

		public Money TotalDue { get; set; }

		public Money VatDue { get; set; }

		public bool IsPaid { get; set; }

		public Decimal DeliveryBalance { get; set; }

		public DateTime? BonusDate { get; set; }

		public Nullable`1 BonusSpentAmount { get; set; }

		public Party.Reference BonusRecipient { get; set; }

		public Person.Reference AssignedTo { get; set; }

		public BankAccount.Reference BankAccount { get; set; }

		public Party.Reference Owner { get; set; }

		public string Note { get; set; }

		public bool IsPublic { get; set; }

		public bool AllowAddProductsInClosedPeriod { get; set; }

		public bool IsSubjectOfPaymentsControl { get; set; }

		public bool SeparateServiceFee { get; set; }

		public IList`1 Items { get; set; }

		public IList`1 Invoices { get; set; }

		public int? InvoiceLastIndex { get; set; }

		public int? ConsignmentLastIndex { get; set; }

		public IList`1 Payments { get; set; }

		public IList`1 Tasks { get; set; }

		public Money ServiceFee { get; set; }

		public IList`1 OutgoingTransfers { get; set; }

		public IList`1 IncomingTransfers { get; set; }

		public string ConsignmentNumbers { get; set; }

		public string InvoiceNumbers { get; set; }

		public string FirstInvoiceNumber { get; set; }

		public IEnumerable`1 ConsignmentRefs { get; set; }

		*/
	}

	partial class OrderContractService
	{
		/*
		public OrderContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Type = r.Type; !!! unknown property
				// c.Text = r.Text; !!! unknown property
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.Customer = r.Customer;
				c.BillTo = r.BillTo;
				c.ShipTo = r.ShipTo;
				c.Intermediary = r.Intermediary;
				c.IsVoid = r.IsVoid;
				c.Discount = r.Discount;
				c.Vat = r.Vat;
				c.UseServiceFeeOnlyInVat = r.UseServiceFeeOnlyInVat;
				c.Total = r.Total;
				c.ServiceFee = r.ServiceFee;
				c.Paid = r.Paid;
				c.TotalDue = r.TotalDue;
				c.VatDue = r.VatDue;
				c.DeliveryBalance = r.DeliveryBalance;
				c.BonusDate = r.BonusDate;
				c.BonusSpentAmount = r.BonusSpentAmount;
				c.BonusRecipient = r.BonusRecipient;
				c.AssignedTo = r.AssignedTo;
				c.BankAccount = r.BankAccount;
				c.Owner = r.Owner;
				c.Note = r.Note;
				c.IsPublic = r.IsPublic;
				c.AllowAddProductsInClosedPeriod = r.AllowAddProductsInClosedPeriod;
				c.IsSubjectOfPaymentsControl = r.IsSubjectOfPaymentsControl;
				// c.Items = r.Items; ??? is array
				// c.Invoices = r.Invoices; ??? is array
				// c.Payments = r.Payments; ??? is array
				// c.Tasks = r.Tasks; ??? is array
				// c.Transfers = r.Transfers; !!! unknown property
				// c.Permissions = r.Permissions; !!! unknown property
				// c.CanCreateTransfer = r.CanCreateTransfer; !!! unknown property
				// c.CanChangeAssignedTo = r.CanChangeAssignedTo; !!! unknown property
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = c.Type + db; !!! unknown property
				// r.Text = c.Text + db; !!! unknown property
				r.Number = c.Number + db;
				r.IssueDate = c.IssueDate + db;
				// r.Customer = c.Customer + db; !!! property is not public
				r.BillTo = c.BillTo + db;
				r.ShipTo = c.ShipTo + db;
				r.Intermediary = c.Intermediary + db;
				r.IsVoid = c.IsVoid + db;
				// r.Discount = c.Discount + db; !!! property is not public
				// r.Vat = c.Vat + db; !!! property is not public
				r.UseServiceFeeOnlyInVat = c.UseServiceFeeOnlyInVat + db;
				// r.Total = c.Total + db; !!! property is not public
				// r.ServiceFee = c.ServiceFee + db; !!! property is non-writable
				// r.Paid = c.Paid + db; !!! property is non-writable
				// r.TotalDue = c.TotalDue + db; !!! property is non-writable
				// r.VatDue = c.VatDue + db; !!! property is non-writable
				// r.DeliveryBalance = c.DeliveryBalance + db; !!! property is non-writable
				r.BonusDate = c.BonusDate + db;
				r.BonusSpentAmount = c.BonusSpentAmount + db;
				r.BonusRecipient = c.BonusRecipient + db;
				r.AssignedTo = c.AssignedTo + db;
				r.BankAccount = c.BankAccount + db;
				r.Owner = c.Owner + db;
				r.Note = c.Note + db;
				r.IsPublic = c.IsPublic + db;
				r.AllowAddProductsInClosedPeriod = c.AllowAddProductsInClosedPeriod + db;
				r.IsSubjectOfPaymentsControl = c.IsSubjectOfPaymentsControl + db;
				// r.Items = c.Items + db; !!! property is non-writable
				// r.Invoices = c.Invoices + db; !!! property is non-writable
				// r.Payments = c.Payments + db; !!! property is non-writable
				// r.Tasks = c.Tasks + db; !!! property is non-writable
				// r.Transfers = c.Transfers + db; !!! unknown property
				// r.Permissions = c.Permissions + db; !!! unknown property
				// r.CanCreateTransfer = c.CanCreateTransfer + db; !!! unknown property
				// r.CanChangeAssignedTo = c.CanChangeAssignedTo + db; !!! unknown property
			};
		}
		*/
	}

	#endregion
	

	#region OrderItemDto
		
	partial class Contracts
	{
		public OrderItemContractService OrderItem { [DebuggerStepThrough] get { return ResolveService(ref _orderItemDto); } }
		private OrderItemContractService _orderItemDto;

		[DebuggerStepThrough]
		public ItemResponse Update(OrderItemDto r, RangeRequest prms)
		{
			return OrderItem.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(OrderItemDto r, string typeName, RangeRequest prms)
		{
			return OrderItem.Update(r, typeName, prms);
		}
	}

	partial class OrderItemDto
	{
		/*
		public bool IsFullDocument { get; set; }

		public bool IsProductData { get; set; }

		public bool IsServiceFee { get; set; }

		public Order.Reference Order { get; set; }

		public Product.Reference Product { get; set; }

		public Consignment.Reference Consignment { get; set; }

		public int Position { get; set; }

		public string Text { get; set; }

		public Nullable`1 LinkType { get; set; }

		public Money Price { get; set; }

		public int Quantity { get; set; }

		public Money Total { get; set; }

		public Money Discount { get; set; }

		public Money GrandTotal { get; set; }

		public Money GivenVat { get; set; }

		public Money TaxedTotal { get; set; }

		public bool HasVat { get; set; }

		public Money ServiceFee { get; set; }

		public bool IsDelivered { get; set; }

		public bool IsForceDelivered { get; set; }

		*/
	}

	partial class OrderItemContractService
	{
		/*
		public OrderItemContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Order = r.Order;
				c.Consignment = r.Consignment;
				c.Product = r.Product;
				c.LinkType = r.LinkType;
				c.Text = r.Text;
				// c.ProductText = r.ProductText; !!! unknown property
				c.Price = r.Price;
				c.Quantity = r.Quantity;
				c.Total = r.Total;
				c.Discount = r.Discount;
				c.GrandTotal = r.GrandTotal;
				c.GivenVat = r.GivenVat;
				c.TaxedTotal = r.TaxedTotal;
				c.HasVat = r.HasVat;
				c.IsForceDelivered = r.IsForceDelivered;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Order = c.Order + db;
				r.Consignment = c.Consignment + db;
				r.Product = c.Product + db;
				r.LinkType = c.LinkType + db;
				r.Text = c.Text + db;
				// r.ProductText = c.ProductText + db; !!! unknown property
				r.Price = c.Price + db;
				r.Quantity = c.Quantity + db;
				// r.Total = c.Total + db; !!! property is non-writable
				r.Discount = c.Discount + db;
				r.GrandTotal = c.GrandTotal + db;
				r.GivenVat = c.GivenVat + db;
				r.TaxedTotal = c.TaxedTotal + db;
				r.HasVat = c.HasVat + db;
				r.IsForceDelivered = c.IsForceDelivered + db;
			};
		}
		*/
	}

	#endregion
	

	#region OrganizationDto
		
	partial class Contracts
	{
		public OrganizationContractService Organization { [DebuggerStepThrough] get { return ResolveService(ref _organizationDto); } }
		private OrganizationContractService _organizationDto;

		[DebuggerStepThrough]
		public ItemResponse Update(OrganizationDto r, RangeRequest prms)
		{
			return Organization.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(OrganizationDto r, string typeName, RangeRequest prms)
		{
			return Organization.Update(r, typeName, prms);
		}
	}

	partial class OrganizationDto
	{
		/*
		public int Type { get; set; }

		public bool IsAirline { get; set; }

		public string AirlineIataCode { get; set; }

		public string AirlinePrefixCode { get; set; }

		public int AirlinePassportRequirement { get; set; }

		public bool IsAccommodationProvider { get; set; }

		public bool IsBusTicketProvider { get; set; }

		public bool IsCarRentalProvider { get; set; }

		public bool IsPasteboardProvider { get; set; }

		public bool IsTourProvider { get; set; }

		public bool IsTransferProvider { get; set; }

		public bool IsGenericProductProvider { get; set; }

		public bool IsProvider { get; set; }

		public bool IsInsuranceCompany { get; set; }

		public bool IsRoamingOperator { get; set; }

		*/
	}

	partial class OrganizationContractService
	{
		/*
		public OrganizationContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Code = r.Code;
				c.IsAccommodationProvider = r.IsAccommodationProvider;
				c.IsBusTicketProvider = r.IsBusTicketProvider;
				c.IsCarRentalProvider = r.IsCarRentalProvider;
				c.IsPasteboardProvider = r.IsPasteboardProvider;
				c.IsTourProvider = r.IsTourProvider;
				c.IsTransferProvider = r.IsTransferProvider;
				c.IsGenericProductProvider = r.IsGenericProductProvider;
				c.IsAirline = r.IsAirline;
				c.AirlineIataCode = r.AirlineIataCode;
				c.AirlinePrefixCode = r.AirlinePrefixCode;
				c.AirlinePassportRequirement = (int)r.AirlinePassportRequirement;
				c.IsInsuranceCompany = r.IsInsuranceCompany;
				c.IsRoamingOperator = r.IsRoamingOperator;
				// c.Departments = r.Departments; !!! unknown property
				// c.Employees = r.Employees; !!! unknown property
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Code = c.Code + db;
				r.IsAccommodationProvider = c.IsAccommodationProvider + db;
				r.IsBusTicketProvider = c.IsBusTicketProvider + db;
				r.IsCarRentalProvider = c.IsCarRentalProvider + db;
				r.IsPasteboardProvider = c.IsPasteboardProvider + db;
				r.IsTourProvider = c.IsTourProvider + db;
				r.IsTransferProvider = c.IsTransferProvider + db;
				r.IsGenericProductProvider = c.IsGenericProductProvider + db;
				r.IsAirline = c.IsAirline + db;
				r.AirlineIataCode = c.AirlineIataCode + db;
				r.AirlinePrefixCode = c.AirlinePrefixCode + db;
				r.AirlinePassportRequirement = (AirlinePassportRequirement)c.AirlinePassportRequirement + db;
				r.IsInsuranceCompany = c.IsInsuranceCompany + db;
				r.IsRoamingOperator = c.IsRoamingOperator + db;
				// r.Departments = c.Departments + db; !!! unknown property
				// r.Employees = c.Employees + db; !!! unknown property
			};
		}
		*/
	}

	#endregion
	

	#region PartyInvoiceDto
		
	partial class Contracts
	{
		public PartyInvoiceContractService PartyInvoice { [DebuggerStepThrough] get { return ResolveService(ref _partyInvoiceDto); } }
		private PartyInvoiceContractService _partyInvoiceDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PartyInvoiceDto r, RangeRequest prms)
		{
			return PartyInvoice.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PartyInvoiceDto r, string typeName, RangeRequest prms)
		{
			return PartyInvoice.Update(r, typeName, prms);
		}
	}

	partial class PartyInvoiceDto
	{
		/*
		public string Number { get; set; }

		public string Agreement { get; set; }

		public DateTime IssueDate { get; set; }

		public DateTime TimeStamp { get; set; }

		public int Type { get; set; }

		public Byte[] Content { get; set; }

		public Order.Reference Order { get; set; }

		public Party.Reference Customer { get; set; }

		public Party.Reference BillTo { get; set; }

		public string BillToName { get; set; }

		public Party.Reference ShipTo { get; set; }

		public bool IsOrderVoid { get; set; }

		public Party.Reference Owner { get; set; }

		public Person.Reference IssuedBy { get; set; }

		public Money Total { get; set; }

		public Money Vat { get; set; }

		public string FileExtension { get; set; }

		*/
	}

	partial class PartyInvoiceContractService
	{
		/*
		public PartyInvoiceContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.Type = r.Type;
				c.TimeStamp = r.TimeStamp;
				c.Order = r.Order;
				c.Total = r.Total;
				c.Vat = r.Vat;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.IssueDate = c.IssueDate + db;
				r.Type = (InvoiceType)c.Type + db;
				r.TimeStamp = c.TimeStamp + db;
				r.Order = c.Order + db;
				r.Total = c.Total + db;
				r.Vat = c.Vat + db;
			};
		}
		*/
	}

	#endregion
	

	#region PartyOrderDto
		
	partial class Contracts
	{
		public PartyOrderContractService PartyOrder { [DebuggerStepThrough] get { return ResolveService(ref _partyOrderDto); } }
		private PartyOrderContractService _partyOrderDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PartyOrderDto r, RangeRequest prms)
		{
			return PartyOrder.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PartyOrderDto r, string typeName, RangeRequest prms)
		{
			return PartyOrder.Update(r, typeName, prms);
		}
	}

	partial class PartyOrderDto
	{
		/*
		public string Number { get; set; }

		public DateTime IssueDate { get; set; }

		public bool IsVoid { get; set; }

		public Party.Reference Customer { get; set; }

		public Party.Reference BillTo { get; set; }

		public string BillToName { get; set; }

		public Party.Reference ShipTo { get; set; }

		public Party.Reference Intermediary { get; set; }

		public Money Discount { get; set; }

		public Money Vat { get; set; }

		public bool UseServiceFeeOnlyInVat { get; set; }

		public Money Total { get; set; }

		public Money Paid { get; set; }

		public Money CheckPaid { get; set; }

		public Money WirePaid { get; set; }

		public Money CreditPaid { get; set; }

		public Money RestPaid { get; set; }

		public Money TotalDue { get; set; }

		public Money VatDue { get; set; }

		public bool IsPaid { get; set; }

		public Decimal DeliveryBalance { get; set; }

		public DateTime? BonusDate { get; set; }

		public Nullable`1 BonusSpentAmount { get; set; }

		public Party.Reference BonusRecipient { get; set; }

		public Person.Reference AssignedTo { get; set; }

		public BankAccount.Reference BankAccount { get; set; }

		public Party.Reference Owner { get; set; }

		public string Note { get; set; }

		public bool IsPublic { get; set; }

		public bool AllowAddProductsInClosedPeriod { get; set; }

		public bool IsSubjectOfPaymentsControl { get; set; }

		public bool SeparateServiceFee { get; set; }

		public IList`1 Items { get; set; }

		public IList`1 Invoices { get; set; }

		public int? InvoiceLastIndex { get; set; }

		public int? ConsignmentLastIndex { get; set; }

		public IList`1 Payments { get; set; }

		public IList`1 Tasks { get; set; }

		public Money ServiceFee { get; set; }

		public IList`1 OutgoingTransfers { get; set; }

		public IList`1 IncomingTransfers { get; set; }

		public string ConsignmentNumbers { get; set; }

		public string InvoiceNumbers { get; set; }

		public string FirstInvoiceNumber { get; set; }

		public IEnumerable`1 ConsignmentRefs { get; set; }

		*/
	}

	partial class PartyOrderContractService
	{
		/*
		public PartyOrderContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Type = r.Type; !!! unknown property
				// c.Text = r.Text; !!! unknown property
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.Total = r.Total;
				c.Paid = r.Paid;
				c.TotalDue = r.TotalDue;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = c.Type + db; !!! unknown property
				// r.Text = c.Text + db; !!! unknown property
				r.Number = c.Number + db;
				r.IssueDate = c.IssueDate + db;
				// r.Total = c.Total + db; !!! property is not public
				// r.Paid = c.Paid + db; !!! property is non-writable
				// r.TotalDue = c.TotalDue + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region PartyProductDto
		
	partial class Contracts
	{
		public PartyProductContractService PartyProduct { [DebuggerStepThrough] get { return ResolveService(ref _partyProductDto); } }
		private PartyProductContractService _partyProductDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PartyProductDto r, RangeRequest prms)
		{
			return PartyProduct.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PartyProductDto r, string typeName, RangeRequest prms)
		{
			return PartyProduct.Update(r, typeName, prms);
		}
	}

	partial class PartyProductDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public DateTime IssueDate { get; set; }

		public string PureNumber { get; set; }

		public Organization.Reference Producer { get; set; }

		public Organization.Reference Provider { get; set; }

		public Product.Reference ReissueFor { get; set; }

		public Product.Reference RefundedProduct { get; set; }

		public string PassengerName { get; set; }

		public IList`1 Passengers { get; set; }

		public ProductPassengerDto[] PassengerDtos { get; set; }

		public bool IsRefund { get; set; }

		public bool IsReservation { get; set; }

		public bool IsProcessed { get; set; }

		public bool MustBeUnprocessed { get; set; }

		public bool IsVoid { get; set; }

		public bool RequiresProcessing { get; set; }

		public bool IsDelivered { get; set; }

		public bool IsPaid { get; set; }

		public Party.Reference Customer { get; set; }

		public Order.Reference Order { get; set; }

		public Party.Reference Intermediary { get; set; }

		public Country.Reference Country { get; set; }

		public string PnrCode { get; set; }

		public string TourCode { get; set; }

		public Person.Reference Booker { get; set; }

		public string BookerOffice { get; set; }

		public string BookerCode { get; set; }

		public Person.Reference Ticketer { get; set; }

		public string TicketerOffice { get; set; }

		public string TicketerCode { get; set; }

		public string TicketingIataOffice { get; set; }

		public bool IsTicketerRobot { get; set; }

		public Person.Reference Seller { get; set; }

		public Party.Reference Owner { get; set; }

		public Organization.Reference LegalEntity { get; set; }

		public Money Fare { get; set; }

		public Money Fare_EUR { get; set; }

		public Money Fare_USD { get; set; }

		public Money EqualFare { get; set; }

		public Money EqualFare_EUR { get; set; }

		public Money EqualFare_USD { get; set; }

		public Money FeesTotal { get; set; }

		public Money FeesTotal_EUR { get; set; }

		public Money FeesTotal_USD { get; set; }

		public Money ConsolidatorCommission { get; set; }

		public Money Total { get; set; }

		public Money Total_EUR { get; set; }

		public Money Total_USD { get; set; }

		public Money BookingFee { get; set; }

		public Money CancelFee { get; set; }

		public Money CancelFee_EUR { get; set; }

		public Money CancelFee_USD { get; set; }

		public Money Vat { get; set; }

		public Money Vat_EUR { get; set; }

		public Money Vat_USD { get; set; }

		public Money Commission { get; set; }

		public Money Commission_EUR { get; set; }

		public Money Commission_USD { get; set; }

		public Money CommissionDiscount { get; set; }

		public Money CommissionDiscount_EUR { get; set; }

		public Money CommissionDiscount_USD { get; set; }

		public Money ServiceFee { get; set; }

		public Money ServiceFee_EUR { get; set; }

		public Money ServiceFee_USD { get; set; }

		public Money Handling { get; set; }

		public Money Handling_EUR { get; set; }

		public Money Handling_USD { get; set; }

		public Money HandlingN { get; set; }

		public Money HandlingN_EUR { get; set; }

		public Money HandlingN_USD { get; set; }

		public Money Discount { get; set; }

		public Money Discount_EUR { get; set; }

		public Money Discount_USD { get; set; }

		public Money BonusDiscount { get; set; }

		public Money BonusDiscount_EUR { get; set; }

		public Money BonusDiscount_USD { get; set; }

		public Money BonusAccumulation { get; set; }

		public Money BonusAccumulation_EUR { get; set; }

		public Money BonusAccumulation_USD { get; set; }

		public Money RefundServiceFee { get; set; }

		public Money RefundServiceFee_EUR { get; set; }

		public Money RefundServiceFee_USD { get; set; }

		public Money ServiceFeePenalty { get; set; }

		public Money ServiceFeePenalty_EUR { get; set; }

		public Money ServiceFeePenalty_USD { get; set; }

		public Money ServiceTotal { get; set; }

		public Money GrandTotal { get; set; }

		public Money GrandTotal_EUR { get; set; }

		public Money GrandTotal_USD { get; set; }

		public Nullable`1 CancelCommissionPercent { get; set; }

		public Money CancelCommission { get; set; }

		public Money CancelCommission_EUR { get; set; }

		public Money CancelCommission_USD { get; set; }

		public Nullable`1 CommissionPercent { get; set; }

		public Money TotalToTransfer { get; set; }

		public Money Profit { get; set; }

		public Money ExtraCharge { get; set; }

		public int PaymentType { get; set; }

		public int TaxRateOfProduct { get; set; }

		public int TaxRateOfServiceFee { get; set; }

		public string Note { get; set; }

		public int Originator { get; set; }

		public int Origin { get; set; }

		public GdsFile.Reference OriginalDocument { get; set; }

		public string ProducerOrProviderAirlineIataCode { get; set; }

		public string TextForOrderItem { get; set; }

		public bool IsAviaDocument { get; set; }

		public bool IsAviaTicket { get; set; }

		public bool IsAviaRefund { get; set; }

		public bool IsAviaMco { get; set; }

		public bool IsPasteboard { get; set; }

		public bool IsPasteboardRefund { get; set; }

		public bool IsSimCard { get; set; }

		public bool IsIsic { get; set; }

		public bool IsExcursion { get; set; }

		public bool IsTour { get; set; }

		public bool IsAccommodation { get; set; }

		public bool IsTransfer { get; set; }

		public bool IsInsurance { get; set; }

		public bool IsInsuranceRefund { get; set; }

		public bool IsCarRental { get; set; }

		public bool IsGenericProduct { get; set; }

		public bool IsBusTicket { get; set; }

		public bool IsBusTicketRefund { get; set; }

		*/
	}

	partial class PartyProductContractService
	{
		/*
		public PartyProductContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = (String)r.Type;
				c.Name = r.Name;
				c.IsRefund = r.IsRefund;
				c.IssueDate = r.IssueDate;
				c.Provider = r.Provider;
				c.Order = r.Order;
				c.Seller = r.Seller;
				c.Owner = r.Owner;
				c.Fare = r.Fare;
				c.Total = r.Total;
				c.ServiceFee = r.ServiceFee;
				c.GrandTotal = r.GrandTotal;
				// c.Itinerary = r.Itinerary; !!! unknown property
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = (ProductType)c.Type + db; !!! property is non-writable
				r.Name = c.Name + db;
				// r.IsRefund = c.IsRefund + db; !!! property is non-writable
				r.IssueDate = c.IssueDate + db;
				r.Provider = c.Provider + db;
				// r.Order = c.Order + db; !!! property is not public
				r.Seller = c.Seller + db;
				r.Owner = c.Owner + db;
				r.Fare = c.Fare + db;
				r.Total = c.Total + db;
				r.ServiceFee = c.ServiceFee + db;
				r.GrandTotal = c.GrandTotal + db;
				// r.Itinerary = c.Itinerary + db; !!! unknown property
			};
		}
		*/
	}

	#endregion
	

	#region PassportDto
		
	partial class Contracts
	{
		public PassportContractService Passport { [DebuggerStepThrough] get { return ResolveService(ref _passportDto); } }
		private PassportContractService _passportDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PassportDto r, RangeRequest prms)
		{
			return Passport.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PassportDto r, string typeName, RangeRequest prms)
		{
			return Passport.Update(r, typeName, prms);
		}
	}

	partial class PassportDto
	{
		/*
		public Person.Reference Owner { get; set; }

		public string Number { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public Country.Reference Citizenship { get; set; }

		public DateTime? Birthday { get; set; }

		public Nullable`1 Gender { get; set; }

		public Country.Reference IssuedBy { get; set; }

		public DateTime? ExpiredOn { get; set; }

		public string Note { get; set; }

		public string AmadeusString { get; set; }

		public string GalileoString { get; set; }

		*/
	}

	partial class PassportContractService
	{
		/*
		public PassportContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.FirstName = r.FirstName;
				c.MiddleName = r.MiddleName;
				c.LastName = r.LastName;
				c.Owner = r.Owner;
				c.Gender = r.Gender;
				// c.GenderString = r.GenderString; !!! unknown property
				c.Citizenship = r.Citizenship;
				c.IssuedBy = r.IssuedBy;
				// c.IssuedByCode = r.IssuedByCode; !!! unknown property
				c.Birthday = r.Birthday;
				c.ExpiredOn = r.ExpiredOn;
				// c.ExpiredDays = r.ExpiredDays; !!! unknown property
				c.Note = r.Note;
				c.AmadeusString = r.AmadeusString;
				c.GalileoString = r.GalileoString;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.FirstName = c.FirstName + db;
				r.MiddleName = c.MiddleName + db;
				r.LastName = c.LastName + db;
				r.Owner = c.Owner + db;
				r.Gender = c.Gender + db;
				// r.GenderString = c.GenderString + db; !!! unknown property
				r.Citizenship = c.Citizenship + db;
				r.IssuedBy = c.IssuedBy + db;
				// r.IssuedByCode = c.IssuedByCode + db; !!! unknown property
				r.Birthday = c.Birthday + db;
				r.ExpiredOn = c.ExpiredOn + db;
				// r.ExpiredDays = c.ExpiredDays + db; !!! unknown property
				r.Note = c.Note + db;
				// r.AmadeusString = c.AmadeusString + db; !!! property is non-writable
				// r.GalileoString = c.GalileoString + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region PasteboardDto
		
	partial class Contracts
	{
		public PasteboardContractService Pasteboard { [DebuggerStepThrough] get { return ResolveService(ref _pasteboardDto); } }
		private PasteboardContractService _pasteboardDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PasteboardDto r, RangeRequest prms)
		{
			return Pasteboard.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PasteboardDto r, string typeName, RangeRequest prms)
		{
			return Pasteboard.Update(r, typeName, prms);
		}
	}

	partial class PasteboardDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

		public Person.Reference Passenger { get; set; }

		public string Number { get; set; }

		public string DeparturePlace { get; set; }

		public DateTime? DepartureDate { get; set; }

		public string DepartureTime { get; set; }

		public string ArrivalPlace { get; set; }

		public DateTime? ArrivalDate { get; set; }

		public string ArrivalTime { get; set; }

		public string Itinerary { get; set; }

		public string TrainNumber { get; set; }

		public string CarNumber { get; set; }

		public string SeatNumber { get; set; }

		public int ServiceClass { get; set; }

		*/
	}

	partial class PasteboardContractService
	{
		/*
		public PasteboardContractService()
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
		*/
	}

	#endregion
	

	#region PasteboardRefundDto
		
	partial class Contracts
	{
		public PasteboardRefundContractService PasteboardRefund { [DebuggerStepThrough] get { return ResolveService(ref _pasteboardRefundDto); } }
		private PasteboardRefundContractService _pasteboardRefundDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PasteboardRefundDto r, RangeRequest prms)
		{
			return PasteboardRefund.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PasteboardRefundDto r, string typeName, RangeRequest prms)
		{
			return PasteboardRefund.Update(r, typeName, prms);
		}
	}

	partial class PasteboardRefundDto
	{
		/*
		public int Type { get; set; }

		public bool IsRefund { get; set; }

		public string Name { get; set; }

		*/
	}

	partial class PasteboardRefundContractService
	{
		/*
		public PasteboardRefundContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = r.Type;
				c.IsRefund = r.IsRefund;
				c.Name = r.Name;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = (ProductType)c.Type + db; !!! property is non-writable
				// r.IsRefund = c.IsRefund + db; !!! property is non-writable
				// r.Name = c.Name + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region PaymentDto
		
	partial class Contracts
	{
		public PaymentContractService Payment { [DebuggerStepThrough] get { return ResolveService(ref _paymentDto); } }
		private PaymentContractService _paymentDto;
	}

	#endregion
	

	#region PaymentSystemDto
		
	partial class Contracts
	{
		public PaymentSystemContractService PaymentSystem { [DebuggerStepThrough] get { return ResolveService(ref _paymentSystemDto); } }
		private PaymentSystemContractService _paymentSystemDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PaymentSystemDto r, RangeRequest prms)
		{
			return PaymentSystem.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PaymentSystemDto r, string typeName, RangeRequest prms)
		{
			return PaymentSystem.Update(r, typeName, prms);
		}
	}

	partial class PaymentSystemDto
	{
		/*
		*/
	}

	partial class PaymentSystemContractService
	{
		/*
		public PaymentSystemContractService()
		{
			ContractFromEntity += (r, c) =>
			{
			};
		
			EntityFromContract += (r, c) =>
			{
			};
		}
		*/
	}

	#endregion
	

	#region PenalizeOperationDto
		
	partial class Contracts
	{
		public PenalizeOperationContractService PenalizeOperation { [DebuggerStepThrough] get { return ResolveService(ref _penalizeOperationDto); } }
		private PenalizeOperationContractService _penalizeOperationDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PenalizeOperationDto r, RangeRequest prms)
		{
			return PenalizeOperation.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PenalizeOperationDto r, string typeName, RangeRequest prms)
		{
			return PenalizeOperation.Update(r, typeName, prms);
		}
	}

	partial class PenalizeOperationDto
	{
		/*
		public AviaTicket.Reference Ticket { get; set; }

		public int Type { get; set; }

		public int Status { get; set; }

		public string Description { get; set; }

		*/
	}

	partial class PenalizeOperationContractService
	{
		/*
		public PenalizeOperationContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = r.Type;
				c.Status = r.Status;
				c.Description = r.Description;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Type = (PenalizeOperationType)c.Type + db;
				r.Status = (PenalizeOperationStatus)c.Status + db;
				r.Description = c.Description + db;
			};
		}
		*/
	}

	#endregion
	

	#region PersonDto
		
	partial class Contracts
	{
		public PersonContractService Person { [DebuggerStepThrough] get { return ResolveService(ref _personDto); } }
		private PersonContractService _personDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PersonDto r, RangeRequest prms)
		{
			return Person.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PersonDto r, string typeName, RangeRequest prms)
		{
			return Person.Update(r, typeName, prms);
		}
	}

	partial class PersonDto
	{
		/*
		public int Type { get; set; }

		public string MilesCardsString { get; set; }

		public DateTime? Birthday { get; set; }

		public Organization.Reference Organization { get; set; }

		public string Title { get; set; }

		public IList`1 Passports { get; set; }

		public IList`1 MilesCards { get; set; }

		*/
	}

	partial class PersonContractService
	{
		/*
		public PersonContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Title = r.Title;
				c.Birthday = r.Birthday;
				c.Organization = r.Organization;
				// c.Passports = r.Passports; ??? is array
				// c.MilesCards = r.MilesCards; ??? is array
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Title = c.Title + db;
				r.Birthday = c.Birthday + db;
				r.Organization = c.Organization + db;
				// r.Passports = c.Passports + db; !!! property is non-writable
				// r.MilesCards = c.MilesCards + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region PersonListDetailDto
		
	partial class Contracts
	{
		public PersonListDetailContractService PersonListDetail { [DebuggerStepThrough] get { return ResolveService(ref _personListDetailDto); } }
		private PersonListDetailContractService _personListDetailDto;

		[DebuggerStepThrough]
		public ItemResponse Update(PersonListDetailDto r, RangeRequest prms)
		{
			return PersonListDetail.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(PersonListDetailDto r, string typeName, RangeRequest prms)
		{
			return PersonListDetail.Update(r, typeName, prms);
		}
	}

	partial class PersonListDetailDto
	{
		/*
		public int Type { get; set; }

		public string MilesCardsString { get; set; }

		public DateTime? Birthday { get; set; }

		public Organization.Reference Organization { get; set; }

		public string Title { get; set; }

		public IList`1 Passports { get; set; }

		public IList`1 MilesCards { get; set; }

		*/
	}

	partial class PersonListDetailContractService
	{
		/*
		public PersonListDetailContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Title = r.Title;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Title = c.Title + db;
			};
		}
		*/
	}

	#endregion
	

	#region ProductDto
		
	partial class Contracts
	{
		public ProductContractService Product { [DebuggerStepThrough] get { return ResolveService(ref _productDto); } }
		private ProductContractService _productDto;
	}

	#endregion
	

	#region ProductPassengerDto
		
	partial class Contracts
	{
		public ProductPassengerContractService ProductPassenger { [DebuggerStepThrough] get { return ResolveService(ref _productPassengerDto); } }
		private ProductPassengerContractService _productPassengerDto;

		[DebuggerStepThrough]
		public ItemResponse Update(ProductPassengerDto r, RangeRequest prms)
		{
			return ProductPassenger.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ProductPassengerDto r, string typeName, RangeRequest prms)
		{
			return ProductPassenger.Update(r, typeName, prms);
		}
	}

	partial class ProductPassengerDto
	{
		/*
		public Product.Reference Product { get; set; }

		public string PassengerName { get; set; }

		public Person.Reference Passenger { get; set; }

		*/
	}

	partial class ProductPassengerContractService
	{
		/*
		public ProductPassengerContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PassengerName = r.PassengerName;
				c.Passenger = r.Passenger;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.PassengerName = c.PassengerName + db;
				r.Passenger = c.Passenger + db;
			};
		}
		*/
	}

	#endregion
	

	#region ProfileDto
		
	partial class Contracts
	{
		public ProfileContractService Profile { [DebuggerStepThrough] get { return ResolveService(ref _profileDto); } }
		private ProfileContractService _profileDto;

		[DebuggerStepThrough]
		public ItemResponse Update(ProfileDto r, RangeRequest prms)
		{
			return Profile.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(ProfileDto r, string typeName, RangeRequest prms)
		{
			return Profile.Update(r, typeName, prms);
		}
	}

	partial class ProfileDto
	{
		/*
		public Person.Reference Person { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public bool Active { get; set; }

		public bool IsAdministrator { get; set; }

		public bool IsSupervisor { get; set; }

		public bool IsAgent { get; set; }

		public bool IsCashier { get; set; }

		public bool IsAnalyst { get; set; }

		public bool IsSubAgent { get; set; }

		public bool AllowCustomerReport { get; set; }

		public bool AllowRegistryReport { get; set; }

		public bool AllowUnbalancedReport { get; set; }

		public string SessionId { get; set; }

		public string Roles { get; set; }

		*/
	}

	partial class ProfileContractService
	{
		/*
		public ProfileContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Person = r.Person;
				// c.Login = r.Login; !!! unknown property
				c.Roles = r.Roles;
				// c.GdsAgents = r.GdsAgents; !!! unknown property
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Person = c.Person + db;
				// r.Login = c.Login + db; !!! unknown property
				// r.Roles = c.Roles + db; !!! property is non-writable
				// r.GdsAgents = c.GdsAgents + db; !!! unknown property
			};
		}
		*/
	}

	#endregion
	

	#region SimCardDto
		
	partial class Contracts
	{
		public SimCardContractService SimCard { [DebuggerStepThrough] get { return ResolveService(ref _simCardDto); } }
		private SimCardContractService _simCardDto;

		[DebuggerStepThrough]
		public ItemResponse Update(SimCardDto r, RangeRequest prms)
		{
			return SimCard.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(SimCardDto r, string typeName, RangeRequest prms)
		{
			return SimCard.Update(r, typeName, prms);
		}
	}

	partial class SimCardDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

		public Person.Reference Passenger { get; set; }

		public string Number { get; set; }

		public bool IsSale { get; set; }

		*/
	}

	partial class SimCardContractService
	{
		/*
		public SimCardContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PassengerName = r.PassengerName;
				c.Passenger = r.Passenger;
				c.Number = r.Number;
				c.IsSale = r.IsSale;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.PassengerName = c.PassengerName + db;
				r.Passenger = c.Passenger + db;
				r.Number = c.Number + db;
				r.IsSale = c.IsSale + db;
			};
		}
		*/
	}

	#endregion
	

	#region SystemConfigurationDto
		
	partial class Contracts
	{
		public SystemConfigurationContractService SystemConfiguration { [DebuggerStepThrough] get { return ResolveService(ref _systemConfigurationDto); } }
		private SystemConfigurationContractService _systemConfigurationDto;

		[DebuggerStepThrough]
		public ItemResponse Update(SystemConfigurationDto r, RangeRequest prms)
		{
			return SystemConfiguration.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(SystemConfigurationDto r, string typeName, RangeRequest prms)
		{
			return SystemConfiguration.Update(r, typeName, prms);
		}
	}

	partial class SystemConfigurationDto
	{
		/*
		public DateTime? ModifiedOn { get; set; }

		public string ModifiedBy { get; set; }

		public Organization.Reference Company { get; set; }

		public string CompanyName { get; set; }

		public string CompanyDetails { get; set; }

		public Country.Reference Country { get; set; }

		public Currency.Reference DefaultCurrency { get; set; }

		public bool UseDefaultCurrencyForInput { get; set; }

		public Decimal VatRate { get; set; }

		public int AmadeusRizUsingMode { get; set; }

		public bool IsPassengerPassportRequired { get; set; }

		public int AviaOrderItemGenerationOption { get; set; }

		public bool AllowAgentSetOrderVat { get; set; }

		public bool UseAviaDocumentVatInOrder { get; set; }

		public int AviaDocumentVatOptions { get; set; }

		public string AccountantDisplayString { get; set; }

		public string IncomingCashOrderCorrespondentAccount { get; set; }

		public bool SeparateDocumentAccess { get; set; }

		public bool AllowOtherAgentsToModifyProduct { get; set; }

		public bool IsOrganizationCodeRequired { get; set; }

		public bool UseConsolidatorCommission { get; set; }

		public Money DefaultConsolidatorCommission { get; set; }

		public bool UseAviaHandling { get; set; }

		public bool UseBonuses { get; set; }

		public int DaysBeforeDeparture { get; set; }

		public Person.Reference BirthdayTaskResponsible { get; set; }

		public bool IsOrderRequiredForProcessedDocument { get; set; }

		public DateTime? MetricsFromDate { get; set; }

		public bool ReservationsInOfficeMetrics { get; set; }

		public bool McoRequiresDescription { get; set; }

		public string NeutralAirlineCode { get; set; }

		public bool Order_UseServiceFeeOnlyInVat { get; set; }

		public int Consignment_NumberMode { get; set; }

		public int Invoice_NumberMode { get; set; }

		public bool InvoicePrinter_ShowVat { get; set; }

		public string InvoicePrinter_FooterDetails { get; set; }

		public DateTime? DrctWebService_LoadedOn { get; set; }

		public DateTime? GalileoWebService_LoadedOn { get; set; }

		public DateTime? GalileoRailWebService_LoadedOn { get; set; }

		public DateTime? GalileoBusWebService_LoadedOn { get; set; }

		public DateTime? TravelPointWebService_LoadedOn { get; set; }

		public bool Consignment_SeparateBookingFee { get; set; }

		public Nullable`1 Pasterboard_DefaultPaymentType { get; set; }

		public bool Ticket_NoPrintReservations { get; set; }

		*/
	}

	partial class SystemConfigurationContractService
	{
		/*
		public SystemConfigurationContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.ModifiedOn = r.ModifiedOn;
				c.ModifiedBy = r.ModifiedBy;
				c.Company = r.Company;
				c.CompanyName = r.CompanyName;
				c.CompanyDetails = r.CompanyDetails;
				c.Country = r.Country;
				c.DefaultCurrency = r.DefaultCurrency;
				c.UseDefaultCurrencyForInput = r.UseDefaultCurrencyForInput;
				c.VatRate = r.VatRate;
				c.AmadeusRizUsingMode = (int)r.AmadeusRizUsingMode;
				c.IsPassengerPassportRequired = r.IsPassengerPassportRequired;
				c.AviaOrderItemGenerationOption = (int)r.AviaOrderItemGenerationOption;
				c.AllowAgentSetOrderVat = r.AllowAgentSetOrderVat;
				c.UseAviaDocumentVatInOrder = r.UseAviaDocumentVatInOrder;
				c.AviaDocumentVatOptions = (int)r.AviaDocumentVatOptions;
				c.AccountantDisplayString = r.AccountantDisplayString;
				c.IncomingCashOrderCorrespondentAccount = r.IncomingCashOrderCorrespondentAccount;
				c.SeparateDocumentAccess = r.SeparateDocumentAccess;
				c.AllowOtherAgentsToModifyProduct = r.AllowOtherAgentsToModifyProduct;
				c.IsOrganizationCodeRequired = r.IsOrganizationCodeRequired;
				c.UseConsolidatorCommission = r.UseConsolidatorCommission;
				c.DefaultConsolidatorCommission = r.DefaultConsolidatorCommission;
				c.UseAviaHandling = r.UseAviaHandling;
				c.UseBonuses = r.UseBonuses;
				c.DaysBeforeDeparture = r.DaysBeforeDeparture;
				c.BirthdayTaskResponsible = r.BirthdayTaskResponsible;
				c.IsOrderRequiredForProcessedDocument = r.IsOrderRequiredForProcessedDocument;
				c.MetricsFromDate = r.MetricsFromDate;
				c.ReservationsInOfficeMetrics = r.ReservationsInOfficeMetrics;
				c.McoRequiresDescription = r.McoRequiresDescription;
				c.NeutralAirlineCode = r.NeutralAirlineCode;
				c.Order_UseServiceFeeOnlyInVat = r.Order_UseServiceFeeOnlyInVat;
				c.Invoice_NumberMode = (int)r.Invoice_NumberMode;
				c.Consignment_NumberMode = (int)r.Consignment_NumberMode;
				c.InvoicePrinter_ShowVat = r.InvoicePrinter_ShowVat;
				c.InvoicePrinter_FooterDetails = r.InvoicePrinter_FooterDetails;
				c.DrctWebService_LoadedOn = r.DrctWebService_LoadedOn;
				c.GalileoWebService_LoadedOn = r.GalileoWebService_LoadedOn;
				c.GalileoRailWebService_LoadedOn = r.GalileoRailWebService_LoadedOn;
				c.GalileoBusWebService_LoadedOn = r.GalileoBusWebService_LoadedOn;
				c.TravelPointWebService_LoadedOn = r.TravelPointWebService_LoadedOn;
				c.Consignment_SeparateBookingFee = r.Consignment_SeparateBookingFee;
				c.Pasterboard_DefaultPaymentType = r.Pasterboard_DefaultPaymentType;
				c.Ticket_NoPrintReservations = r.Ticket_NoPrintReservations;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.ModifiedOn = c.ModifiedOn + db;
				r.ModifiedBy = c.ModifiedBy + db;
				r.Company = c.Company + db;
				// r.CompanyName = c.CompanyName + db; !!! property is non-writable
				r.CompanyDetails = c.CompanyDetails + db;
				r.Country = c.Country + db;
				r.DefaultCurrency = c.DefaultCurrency + db;
				r.UseDefaultCurrencyForInput = c.UseDefaultCurrencyForInput + db;
				r.VatRate = c.VatRate + db;
				r.AmadeusRizUsingMode = (AmadeusRizUsingMode)c.AmadeusRizUsingMode + db;
				r.IsPassengerPassportRequired = c.IsPassengerPassportRequired + db;
				r.AviaOrderItemGenerationOption = (AviaOrderItemGenerationOption)c.AviaOrderItemGenerationOption + db;
				r.AllowAgentSetOrderVat = c.AllowAgentSetOrderVat + db;
				r.UseAviaDocumentVatInOrder = c.UseAviaDocumentVatInOrder + db;
				r.AviaDocumentVatOptions = (AviaDocumentVatOptions)c.AviaDocumentVatOptions + db;
				r.AccountantDisplayString = c.AccountantDisplayString + db;
				r.IncomingCashOrderCorrespondentAccount = c.IncomingCashOrderCorrespondentAccount + db;
				r.SeparateDocumentAccess = c.SeparateDocumentAccess + db;
				r.AllowOtherAgentsToModifyProduct = c.AllowOtherAgentsToModifyProduct + db;
				r.IsOrganizationCodeRequired = c.IsOrganizationCodeRequired + db;
				r.UseConsolidatorCommission = c.UseConsolidatorCommission + db;
				r.DefaultConsolidatorCommission = c.DefaultConsolidatorCommission + db;
				r.UseAviaHandling = c.UseAviaHandling + db;
				r.UseBonuses = c.UseBonuses + db;
				r.DaysBeforeDeparture = c.DaysBeforeDeparture + db;
				r.BirthdayTaskResponsible = c.BirthdayTaskResponsible + db;
				r.IsOrderRequiredForProcessedDocument = c.IsOrderRequiredForProcessedDocument + db;
				r.MetricsFromDate = c.MetricsFromDate + db;
				r.ReservationsInOfficeMetrics = c.ReservationsInOfficeMetrics + db;
				r.McoRequiresDescription = c.McoRequiresDescription + db;
				r.NeutralAirlineCode = c.NeutralAirlineCode + db;
				r.Order_UseServiceFeeOnlyInVat = c.Order_UseServiceFeeOnlyInVat + db;
				r.Invoice_NumberMode = (InvoiceNumberMode)c.Invoice_NumberMode + db;
				r.Consignment_NumberMode = (InvoiceNumberMode)c.Consignment_NumberMode + db;
				r.InvoicePrinter_ShowVat = c.InvoicePrinter_ShowVat + db;
				r.InvoicePrinter_FooterDetails = c.InvoicePrinter_FooterDetails + db;
				r.DrctWebService_LoadedOn = c.DrctWebService_LoadedOn + db;
				r.GalileoWebService_LoadedOn = c.GalileoWebService_LoadedOn + db;
				r.GalileoRailWebService_LoadedOn = c.GalileoRailWebService_LoadedOn + db;
				r.GalileoBusWebService_LoadedOn = c.GalileoBusWebService_LoadedOn + db;
				r.TravelPointWebService_LoadedOn = c.TravelPointWebService_LoadedOn + db;
				r.Consignment_SeparateBookingFee = c.Consignment_SeparateBookingFee + db;
				r.Pasterboard_DefaultPaymentType = c.Pasterboard_DefaultPaymentType + db;
				r.Ticket_NoPrintReservations = c.Ticket_NoPrintReservations + db;
			};
		}
		*/
	}

	#endregion
	

	#region TaskDto
		
	partial class Contracts
	{
		public TaskContractService Task { [DebuggerStepThrough] get { return ResolveService(ref _taskDto); } }
		private TaskContractService _taskDto;

		[DebuggerStepThrough]
		public ItemResponse Update(TaskDto r, RangeRequest prms)
		{
			return Task.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(TaskDto r, string typeName, RangeRequest prms)
		{
			return Task.Update(r, typeName, prms);
		}
	}

	partial class TaskDto
	{
		/*
		public string Number { get; set; }

		public string Subject { get; set; }

		public string Description { get; set; }

		public Party.Reference RelatedTo { get; set; }

		public Order.Reference Order { get; set; }

		public Party.Reference AssignedTo { get; set; }

		public int Status { get; set; }

		public DateTime? DueDate { get; set; }

		public bool Overdue { get; set; }

		public IList`1 Comments { get; set; }

		public bool CanModify { get; set; }

		*/
	}

	partial class TaskContractService
	{
		/*
		public TaskContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Type = r.Type; !!! unknown property
				// c.Text = r.Text; !!! unknown property
				c.Number = r.Number;
				c.Subject = r.Subject;
				c.Description = r.Description;
				c.RelatedTo = r.RelatedTo;
				c.Order = r.Order;
				c.AssignedTo = r.AssignedTo;
				c.Status = r.Status;
				c.DueDate = r.DueDate;
				c.Overdue = r.Overdue;
				c.CanModify = r.CanModify;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Type = c.Type + db; !!! unknown property
				// r.Text = c.Text + db; !!! unknown property
				r.Number = c.Number + db;
				r.Subject = c.Subject + db;
				r.Description = c.Description + db;
				r.RelatedTo = c.RelatedTo + db;
				// r.Order = c.Order + db; !!! property is not public
				r.AssignedTo = c.AssignedTo + db;
				r.Status = (TaskStatus)c.Status + db;
				r.DueDate = c.DueDate + db;
				// r.Overdue = c.Overdue + db; !!! property is non-writable
				// r.CanModify = c.CanModify + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region TourDto
		
	partial class Contracts
	{
		public TourContractService Tour { [DebuggerStepThrough] get { return ResolveService(ref _tourDto); } }
		private TourContractService _tourDto;

		[DebuggerStepThrough]
		public ItemResponse Update(TourDto r, RangeRequest prms)
		{
			return Tour.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(TourDto r, string typeName, RangeRequest prms)
		{
			return Tour.Update(r, typeName, prms);
		}
	}

	partial class TourDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

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

		*/
	}

	partial class TourContractService
	{
		/*
		public TourContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Passengers = r.Passengers; ??? is array
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
				// r.Passengers = c.Passengers + db; ??? is array
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
			};
		}
		*/
	}

	#endregion
	

	#region TransferDto
		
	partial class Contracts
	{
		public TransferContractService Transfer { [DebuggerStepThrough] get { return ResolveService(ref _transferDto); } }
		private TransferContractService _transferDto;

		[DebuggerStepThrough]
		public ItemResponse Update(TransferDto r, RangeRequest prms)
		{
			return Transfer.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(TransferDto r, string typeName, RangeRequest prms)
		{
			return Transfer.Update(r, typeName, prms);
		}
	}

	partial class TransferDto
	{
		/*
		public int Type { get; set; }

		public string Name { get; set; }

		public string PassengerName { get; set; }

		public DateTime StartDate { get; set; }

		*/
	}

	partial class TransferContractService
	{
		/*
		public TransferContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				// c.Passengers = r.Passengers; ??? is array
				c.StartDate = r.StartDate;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.Passengers = c.Passengers + db; ??? is array
				r.StartDate = c.StartDate + db;
			};
		}
		*/
	}

	#endregion
	

	#region UserDto
		
	partial class Contracts
	{
		public UserContractService User { [DebuggerStepThrough] get { return ResolveService(ref _userDto); } }
		private UserContractService _userDto;

		[DebuggerStepThrough]
		public ItemResponse Update(UserDto r, RangeRequest prms)
		{
			return User.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(UserDto r, string typeName, RangeRequest prms)
		{
			return User.Update(r, typeName, prms);
		}
	}

	partial class UserDto
	{
		/*
		public Person.Reference Person { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

		public bool Active { get; set; }

		public bool IsAdministrator { get; set; }

		public bool IsSupervisor { get; set; }

		public bool IsAgent { get; set; }

		public bool IsCashier { get; set; }

		public bool IsAnalyst { get; set; }

		public bool IsSubAgent { get; set; }

		public bool AllowCustomerReport { get; set; }

		public bool AllowRegistryReport { get; set; }

		public bool AllowUnbalancedReport { get; set; }

		public string SessionId { get; set; }

		public string Roles { get; set; }

		*/
	}

	partial class UserContractService
	{
		/*
		public UserContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Person = r.Person;
				c.Password = r.Password;
				c.ConfirmPassword = r.ConfirmPassword;
				c.Active = r.Active;
				c.IsAdministrator = r.IsAdministrator;
				c.IsSupervisor = r.IsSupervisor;
				c.IsAgent = r.IsAgent;
				c.IsCashier = r.IsCashier;
				c.IsAnalyst = r.IsAnalyst;
				c.IsSubAgent = r.IsSubAgent;
				c.AllowCustomerReport = r.AllowCustomerReport;
				c.AllowRegistryReport = r.AllowRegistryReport;
				c.AllowUnbalancedReport = r.AllowUnbalancedReport;
				c.SessionId = r.SessionId;
				c.Roles = r.Roles;
			};
		
			EntityFromContract += (r, c) =>
			{
				r.Person = c.Person + db;
				r.Password = c.Password + db;
				r.ConfirmPassword = c.ConfirmPassword + db;
				r.Active = c.Active + db;
				r.IsAdministrator = c.IsAdministrator + db;
				r.IsSupervisor = c.IsSupervisor + db;
				r.IsAgent = c.IsAgent + db;
				r.IsCashier = c.IsCashier + db;
				r.IsAnalyst = c.IsAnalyst + db;
				r.IsSubAgent = c.IsSubAgent + db;
				r.AllowCustomerReport = c.AllowCustomerReport + db;
				r.AllowRegistryReport = c.AllowRegistryReport + db;
				r.AllowUnbalancedReport = c.AllowUnbalancedReport + db;
				r.SessionId = c.SessionId + db;
				// r.Roles = c.Roles + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	

	#region WireTransferDto
		
	partial class Contracts
	{
		public WireTransferContractService WireTransfer { [DebuggerStepThrough] get { return ResolveService(ref _wireTransferDto); } }
		private WireTransferContractService _wireTransferDto;

		[DebuggerStepThrough]
		public ItemResponse Update(WireTransferDto r, RangeRequest prms)
		{
			return WireTransfer.Update(r, prms);
		}

		[DebuggerStepThrough]
		public ItemResponse Update(WireTransferDto r, string typeName, RangeRequest prms)
		{
			return WireTransfer.Update(r, typeName, prms);
		}
	}

	partial class WireTransferDto
	{
		/*
		public int PaymentForm { get; set; }

		*/
	}

	partial class WireTransferContractService
	{
		/*
		public WireTransferContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PaymentForm = r.PaymentForm;
			};
		
			EntityFromContract += (r, c) =>
			{
				// r.PaymentForm = (PaymentForm)c.PaymentForm + db; !!! property is non-writable
			};
		}
		*/
	}

	#endregion
	
}