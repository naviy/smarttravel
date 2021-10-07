namespace Luxena.Travel.Domain
{

	public partial class AviaTicketDto : AviaDocumentDto
	{

		public FlightSegmentDto[] Segments { get; set; }

		public PenalizeOperationDto[] PenalizeOperations { get; set; }

		public bool PassportRequired { get; set; }

		public PassportValidationResult PassportValidationResult { get; set; }

	}


	public partial class AviaTicketContractService : AviaDocumentContractService<AviaTicket, AviaTicket.Service, AviaTicketDto>
	{
		public AviaTicketContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Segments = dc.FlightSegment.New(r.Segments);

				c.PenalizeOperations = dc.PenalizeOperation.New(r.PenalizeOperations);

				c.PassportRequired = db.AviaTicket.IsPassportRequired(r);

				c.PassportValidationResult = dc.Passport.ValidatePassengerPassports(r).PassportValidationResult;
			};

			EntityFromContract += (r, c) =>
			{
				r.SetDomestic();

				if (c.Segments != null)
				{
					dc.FlightSegment.Update(r, 
						r.Segments, c.Segments, r.AddSegment, r.RemoveSegment
					);
				}
			};
		}

		//public AviaTicketDto UpdateFlightSegments(object ticketId, FlightSegmentDto[] prms)
		//{
		//	var ticket = db.AviaTicket.By(ticketId);

		//	foreach (var dto in prms)
		//	{
		//		var segment = new FlightSegment();

		//		if (dto.Id == null)
		//			ticket.AddSegment(segment);
		//		else
		//		{
		//			var dto1 = dto;
		//			segment = ticket.Segments.By(a => Equals(a.Id, dto1.Id));

		//			switch (dto.StateInfo)
		//			{
		//				case "D":
		//					ticket.RemoveSegment(segment);
		//					break;
		//				case "U":
		//					ticket.UpdateSegmentPosition(segment, dto.Position);
		//					break;
		//			}
		//		}
		//	}

		//	db.Save(ticket);

		//	return New(ticket);
		//}

		//public FlightSegmentDto UpdateFlightSegment(object ticketId, Dictionary<string, object> prms)
		//{
		//	var ticket = db.AviaTicket.By(ticketId);

		//	var id = prms["Id"].AsString();

		//	FlightSegment segment;
		//	if (id.No())
		//		ticket.AddSegment(segment = new FlightSegment());
		//	else
		//		segment = ticket.Segments.By(a => a.Id.ToString() == id);

		//	UpdateSegmentFields(segment, prms);

		//	db.Save(ticket);

		//	return dc.FlightSegment.New(segment);
		//}


		//private static bool HasValue(IDictionary<string, object> prms, string key)
		//{
		//	return prms.By(key).AsString().Yes();
		//}

		//private void UpdateSegmentFields(FlightSegment segment, IDictionary<string, object> prms)
		//{
		//	if (HasValue(prms, "FromAirport") && prms["FromAirport"] is Array)
		//	{
		//		var from = db.Airport.Load(((Array)prms["FromAirport"]).GetValue(0).ToString());
		//		segment.FromAirportCode = from.Code;
		//		segment.FromAirportName = from.Name;
		//		segment.FromAirport = from;
		//	}
		//	if (HasValue(prms, "ToAirport") && prms["ToAirport"] is Array)
		//	{
		//		var to = db.Airport.Load(((Array)prms["ToAirport"]).GetValue(0).ToString());
		//		segment.ToAirportCode = to.Code;
		//		segment.ToAirportName = to.Name;
		//		segment.ToAirport = to;
		//	}
		//	if (HasValue(prms, "Carrier") && prms["Carrier"] is Array)
		//	{
		//		var carrier = db.Airline.Load(((Array)prms["Carrier"]).GetValue(0).ToString());
		//		segment.Carrier = carrier;
		//		segment.CarrierIataCode = carrier.AirlineIataCode;
		//		segment.CarrierName = carrier.Name;
		//		segment.CarrierPrefixCode = carrier.AirlinePrefixCode;
		//	}
		//	if (HasValue(prms, "FlightNumber"))
		//		segment.FlightNumber = prms["FlightNumber"].ToString();
		//	if (HasValue(prms, "ServiceClassCode"))
		//		segment.ServiceClassCode = prms["ServiceClassCode"].ToString();
		//	if (HasValue(prms, "ServiceClass"))
		//		segment.ServiceClass = (ServiceClass)prms["ServiceClass"].As().Int;
		//	if (HasValue(prms, "DepartureTime"))
		//		segment.DepartureTime = ((DateTime)prms["DepartureTime"]);
		//	if (HasValue(prms, "DepartureTime_1") && segment.DepartureTime != null)
		//	{
		//		var hms = prms["DepartureTime_1"].ToString().Split(new[] { ':', ',', '.', '\\', '/', ';', '-' });
		//		segment.DepartureTime = segment.DepartureTime.Value.Add(new TimeSpan(int.Parse(hms[0]), int.Parse(hms[1]), int.Parse(hms[2])));
		//	}
		//	if (HasValue(prms, "ArrivalTime"))
		//		segment.ArrivalTime = ((DateTime)prms["ArrivalTime"]);
		//	if (HasValue(prms, "ArrivalTime_1") && segment.ArrivalTime != null)
		//	{
		//		var hms = prms["ArrivalTime_1"].ToString().Split(new[] { ':', ',', '.', '\\', '/', ';', '-' });
		//		segment.ArrivalTime = segment.ArrivalTime.Value.Add(new TimeSpan(int.Parse(hms[0]), int.Parse(hms[1]), int.Parse(hms[2])));
		//	}
		//	if (HasValue(prms, "MealCodes"))
		//		segment.MealCodes = prms["MealCodes"].ToString();
		//	if (HasValue(prms, "MealTypes"))
		//	{
		//		var mealTypes = prms["MealTypes"].ToString();
		//		var meals = mealTypes.Split(',');
		//		var mt = MealType.NoData;
		//		foreach (var s in meals)
		//			mt |= (MealType)Convert.ToInt32(s.Trim());
		//		segment.MealTypes = mt;
		//	}
		//	if (HasValue(prms, "NumberOfStops"))
		//		segment.NumberOfStops = (int)prms["NumberOfStops"];
		//	if (HasValue(prms, "Luggage"))
		//		segment.Luggage = prms["Luggage"].ToString();
		//	if (HasValue(prms, "CheckInTerminal"))
		//		segment.CheckInTerminal = prms["CheckInTerminal"].ToString();
		//	if (HasValue(prms, "CheckInTime"))
		//		segment.CheckInTime = prms["CheckInTime"].ToString();
		//	if (HasValue(prms, "Duration"))
		//		segment.Duration = prms["Duration"].ToString();
		//	if (HasValue(prms, "ArrivalTerminal"))
		//		segment.ArrivalTerminal = prms["ArrivalTerminal"].ToString();
		//	if (HasValue(prms, "Seat"))
		//		segment.Seat = prms["Seat"].ToString();
		//	if (HasValue(prms, "FareBasis"))
		//		segment.FareBasis = prms["FareBasis"].ToString();
		//	if (HasValue(prms, "Stopover"))
		//		segment.Stopover = (bool)prms["Stopover"];
		//	if (HasValue(prms, "Position"))
		//		segment.Position = (int)prms["Position"];
		//}

	}

}