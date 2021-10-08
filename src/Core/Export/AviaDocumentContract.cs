using System.Linq;
using System.Runtime.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract]
	public abstract class AviaDocumentContract : ProductContract
	{

		[DataMember]
		public string AirlineIataCode { get; set; }
		[DataMember]
		public string AirlinePrefixCode { get; set; }
		[DataMember]
		public string AirlineName { get; set; }
		[DataMember]
		public PartyReference Airline { get; set; }
		[DataMember]
		public string Number { get; set; }
		[DataMember]
		public string ConjunctionNumbers { get; set; }
		[DataMember]
		public PartyReference Passenger { get; set; }
		[DataMember]
		public string Itinerary { get; set; }
		[DataMember]
		public string PaymentForm { get; set; }
		[DataMember]
		public string PaymentDetails { get; set; }
		[DataMember]
		public string BookerOffice { get; set; }
		[DataMember]
		public string BookerCode { get; set; }
		[DataMember]
		public PartyReference Booker { get; set; }
		[DataMember]
		public string TicketerOffice { get; set; }
		[DataMember]
		public string TicketerCode { get; set; }
		[DataMember]
		public PartyReference Ticketer { get; set; }
		[DataMember]
		public string TicketingIataOffice { get; set; }
		[DataMember]
		public bool IsTicketerRobot { get; set; }
		[DataMember]
		public GdsOriginator Originator { get; set; }
		[DataMember]
		public ProductOrigin Origin { get; set; }
		[DataMember]
		public string AirlinePnrCode { get; set; }
		[DataMember]
		public string Remarks { get; set; }
		[DataMember]
		public AviaDocumentFeeContract[] Fees { get; set; }


		//===


		protected AviaDocumentContract() { }

		protected AviaDocumentContract(AviaDocument r)
			: base(r)
		{
			AirlineIataCode = r.AirlineIataCode;
			AirlinePrefixCode = r.AirlinePrefixCode;
			AirlineName = r.AirlineName;
			Airline = r.Producer;
			Number = r.Number;
			ConjunctionNumbers = r.ConjunctionNumbers;
			Passenger = r.Passengers.One()?.Passenger;
			Itinerary = r.Itinerary;
			PaymentForm = r.PaymentForm;
			PaymentDetails = r.PaymentDetails;
			BookerOffice = r.BookerOffice;
			BookerCode = r.BookerCode;
			Booker = r.Booker;
			TicketerOffice = r.TicketerOffice;
			TicketerCode = r.TicketerCode;
			Ticketer = r.Ticketer;
			TicketingIataOffice = r.TicketingIataOffice;
			IsTicketerRobot = r.IsTicketerRobot;
			Originator = r.Originator;
			Origin = r.Origin;
			AirlinePnrCode = r.AirlinePnrCode;
			Remarks = r.Remarks;
			Fees = r.Fees.Select(a => new AviaDocumentFeeContract(a)).ToArray();
		}


		//===


		public override void AssignTo(Domain.Domain db, Product rr)
		{
			base.AssignTo(db, rr);
			var r = (AviaDocument) rr;

			r.AirlineIataCode = AirlineIataCode;
			r.AirlinePrefixCode = AirlinePrefixCode;
			r.AirlineName = AirlineName;
			r.Producer = db.Organization.ByName(Airline?.Text);
			r.Number = Number;
			r.ConjunctionNumbers = ConjunctionNumbers;
			r.Passenger = db.Person.ByName(Passenger?.Text);
			r.Itinerary = Itinerary;
			r.PaymentForm = PaymentForm;
			r.PaymentDetails = PaymentDetails;
			r.BookerOffice = BookerOffice;
			r.BookerCode = BookerCode;
			r.Booker = db.Person.ByName(Booker?.Text);
			r.TicketerOffice = TicketerOffice;
			r.TicketerCode = TicketerCode;
			r.Ticketer = db.Person.ByName(Ticketer?.Text);
			r.TicketingIataOffice = TicketingIataOffice;
			r.IsTicketerRobot = IsTicketerRobot;
			r.Originator = Originator;
			r.Origin = Origin;
			r.AirlinePnrCode = AirlinePnrCode;
			r.Remarks = Remarks;
			r.Fees = Fees?.Select(a => new AviaDocumentFee
			{
				Document = r,
				Code = a.Code,
				//Amount = new Money(a.Currency, a.Amount),
			}).ToList();
		}


	}


}