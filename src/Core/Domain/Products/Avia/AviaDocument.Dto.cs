using System.Collections.Generic;
using System.Linq;


namespace Luxena.Travel.Domain
{

	public abstract partial class AviaDocumentDto : ProductDto
	{

		public string Number { get; set; }

		public string Conjunction { get; set; }

		public string AirlinePrefixCode { get; set; }

		public string PassengerName { get; set; }

		public virtual Person.Reference Passenger { get; set; }

		public virtual GdsPassportStatus GdsPassportStatus { get; set; }

		public virtual string GdsPassport { get; set; }

		public virtual string Itinerary { get; set; }

		public string AirlinePnrCode { get; set; }

		public string TicketingIataOffice { get; set; }

		public string[] Voidings { get; set; }

		public string PaymentForm { get; set; }

		public string PaymentDetails { get; set; }

//		public bool PrintUnticketedFlightSegments { get; set; }
		
		public AviaDocumentFeeDto[] Fees { get; set; }

	}


	public class AviaDocumentContractService<TAviaDocument, TAviaDocumentService, TAviaDocumentDto>
		: ProductContractService<TAviaDocument, TAviaDocumentService, TAviaDocumentDto>
		where TAviaDocument : AviaDocument, new()
		where TAviaDocumentService : AviaDocument.Service<TAviaDocument>
		where TAviaDocumentDto : AviaDocumentDto, new()
	{
		public AviaDocumentContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;

				c.Conjunction = r.ConjunctionNumbers;

				c.AirlinePrefixCode = r.AirlinePrefixCode;

				c.PassengerName = r.PassengerName;
				c.Passenger = r.Passenger;
				c.GdsPassportStatus = r.GdsPassportStatus;
				c.GdsPassport = r.GdsPassport;

				c.Itinerary = r.Itinerary;

				c.AirlinePnrCode = r.AirlinePnrCode;

				c.TicketingIataOffice = r.TicketingIataOffice;

				if (r.Voidings.Yes())
				{
					var voidings = new List<string>();

					foreach (var voiding in r.Voidings)
					{
						var text = string.Empty;
						if (voiding.Agent != null)
							text = voiding.Agent.Name + " ";
						else if (voiding.AgentCode.Yes() && voiding.AgentOffice.Yes())
							text = string.Format("{0}-{1}", voiding.AgentOffice, voiding.AgentCode) + " ";

						voidings.Add(string.Format("{0}({1})", text, voiding.IsVoid ? CommonRes.Voided : CommonRes.Restored));
					}

					c.Voidings = voidings.ToArray();
				}

				c.PaymentForm = r.PaymentForm;
				c.PaymentDetails = r.PaymentDetails;
//				c.PrintUnticketedFlightSegments = r.PrintUnticketedFlightSegments;

				c.Fees = r.Fees.Select(fee => new AviaDocumentFeeDto(fee)).ToArray();
			};

			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;

				r.AirlinePrefixCode = c.AirlinePrefixCode + db;

				r.PassengerName = c.PassengerName + db;
				r.Passenger = c.Passenger + db;

				r.GdsPassportStatus = c.GdsPassportStatus + db;

				r.TicketingIataOffice = c.TicketingIataOffice + db;

//				r.PrintUnticketedFlightSegments = c.PrintUnticketedFlightSegments + db;
			};
		}
	}


	public partial class AviaDocumentContractService : ProductContractService
	{
		public new AviaDocumentDto By(object id)
		{
			var r = db.AviaDocument.By(id);
			var c = (AviaDocumentDto)New(r);
			return c;
		}
	}

}