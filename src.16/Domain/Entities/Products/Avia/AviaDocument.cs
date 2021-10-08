using System.Collections.Generic;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Авиадокумент", "Авиадокументы"), Icon("plane")]
	public abstract partial class AviaDocument : Product
	{

		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<AviaDocument> se)
		{
			se.For(a => a.Type)
				.Enum(ProductType.AviaTicket, ProductType.AviaRefund, ProductType.AviaMco);

			se.For(a => a.Name)
				.Pattern<Patterns.Number>();

			se.For(a => a.ReissueFor)
				.Lookup<AviaDocument>();

			se.For(a => a.Producer)
				.Lookup<Airline>()
				.Localization<Airline>();
		}

		protected AviaDocument()
		{
			Originator = GdsOriginator.Unknown;
			Origin = ProductOrigin.Manual;
		}


		public string AirlineIataCode { get; set; }

		[RU("Код АК"), Length(3, 3, 3)]
		public virtual string AirlinePrefixCode { get; set; }

		public string AirlineName { get; set; }

		[Patterns.Number, Length(10, 10, 10)]
		public virtual string Number { get; set; }

		[Patterns.Number]
		public string FullNumber => 
			IsReservation ? null : $"{AirlinePrefixCode}-{Number.As().Long:0000000000}";

		public override bool IsReservation => Number == null;

		public string ConjunctionNumbers { get; set; }

		public GdsPassportStatus GdsPassportStatus { get; set; }

		public string GdsPassport { get; set; }


		public string PaymentForm { get; set; }

		public string PaymentDetails { get; set; }


		public string AirlinePnrCode { get; set; }

		public string Remarks { get; set; }

		
		public virtual ICollection<AviaMco> AviaMcos_InConnectionWith { get; set; }
	}


	partial class Domain
	{
		public DbSet<AviaDocument> AviaDocuments { get; set; }
	}

}