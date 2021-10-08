using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract(Name = "AviaTicket", Namespace = "")]
	[XmlType(TypeName = "AviaTicket")]
	public class AviaTicketContract : AviaDocumentContract
	{

		[DataMember]
		public DateTime? Departure { get; set; }
		[DataMember]
		public DateTime? LastDeparture { get; set; }
		[DataMember]
		public DateTime? Arrival { get; set; }

		[DataMember]
		public bool Domestic { get; set; }
		[DataMember]
		public bool Interline { get; set; }
		[DataMember]
		public string SegmentClasses { get; set; }
		[DataMember]
		public string ReissueFor { get; set; }
		[DataMember]
		public string Endorsement { get; set; }
		[DataMember]
		public bool IsManual { get; set; }
		[DataMember]
		public FlightSegmentContract[] Segments { get; set; }
		[DataMember]
		public PenalizeOperationContract[] Penalties { get; set; }

		[DataMember]
		public AirportContract FromAirport { get; set; }
		[DataMember]
		public AirportContract ToAirport { get; set; }
		[DataMember]
		public string Direction { get; set; }


		//===


		public AviaTicketContract() { }

		public AviaTicketContract(AviaTicket r) : base(r)
		{
			Departure = r.Departure;
			LastDeparture = r.LastDeparture;
			Arrival = r.Arrival;
			Domestic = r.Domestic;
			Interline = r.Interline;
			SegmentClasses = r.SegmentClasses;
			ReissueFor = r.ReissueFor;
			Endorsement = r.Endorsement;
			IsManual = r.IsManual;

			Segments = r.Segments.Select(a => new FlightSegmentContract(a)).ToArray();
			Penalties = r.PenalizeOperations.Select(a => new PenalizeOperationContract(a)).ToArray();

			FromAirport = r.Segments.One()?.FromAirport;
			ToAirport = r.GetDirection();
			Direction = ToAirport?.Code;
		}


		//===


		public override void AssignTo(Domain.Domain db, Product rr)
		{
			base.AssignTo(db, rr);
			var r = (AviaTicket)rr;


			r.Departure = Departure;
			//r.LastDeparture = LastDeparture;
			r.Domestic = Domestic;
			r.Interline = Interline;
			r.SegmentClasses = SegmentClasses;
			//r.ReissueFor = ReissueFor;
			r.Endorsement = Endorsement;
			r.IsManual = IsManual;

			Segments.ForEach(a => r.AddSegment(new FlightSegment().Do(seg => a.AssignTo(db, seg))));

			r.PenalizeOperations = Penalties.Select(a => new PenalizeOperation
			{
				Ticket = r,
				Type = a.Type,
				Status = a.Status,
				Description = a.Description,
			}).ToList();

			//r.FromAirport = Segments.One()?.FromAirport;
			//r.ToAirport = GetDirection();
			//r.Direction = ToAirport?.Code;
		}

	}


}
