namespace Luxena.Travel.Domain
{

	public partial class AviaTicketProcessDto : AviaDocumentProcessDto
	{

		public PenalizeOperationDto[] PenalizeOperations { get; set; }

		public int SegmentCount { get; set; }

		public Person.Reference Passenger { get; set; }

		public Person.Reference SuggestPassenger { get; set; }

		public string GdsPassport { get; set; }

		public GdsPassportStatus GdsPassportStatus { get; set; }

		public PassportDto Passport { get; set; }

		public bool PassportRequired { get; set; }

		public PassportValidationResult PassportValidationResult { get; set; }

	}


	public partial class AviaTicketProcessContractService 
		: AviaDocumentProcessContractService<AviaTicket, AviaTicket.Service, AviaTicketProcessDto>
	{
		public AviaTicketProcessContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.PenalizeOperations = dc.PenalizeOperation.New(r.PenalizeOperations);
				c.SegmentCount = r.Segments.Count;

				c.Passenger = r.Passenger;
				c.SuggestPassenger = db.Person.ByPassengerName(r);

				c.GdsPassport = r.GdsPassport;
				c.GdsPassportStatus = r.GdsPassportStatus;

				c.Passport = dc.Passport.New(db.AviaTicket.GetPassport(r));
				c.PassportRequired = db.AviaTicket.IsPassportRequired(r);
				c.PassportValidationResult = dc.Passport.ValidatePassengerPassports(r).PassportValidationResult;
			};

			EntityFromContract += (r, c) =>
			{
				dc.PenalizeOperation.Update(r, c.PenalizeOperations);

				r.Passenger = c.Passenger + db;
				r.GdsPassportStatus = c.GdsPassportStatus + db;
				r.GdsPassport = c.GdsPassport + db;
			};
		}
	}

}