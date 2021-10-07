using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("SIM-карта", "SIM-карты")]
	public partial class SimCard : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<SimCard> sm)
		{
			sm.For(a => a.ReissueFor)
				.Suggest<SimCard>();

			sm.For(a => a.Producer)
				.Suggest<RoamingOperator>()
				.RU("Оператор")
				.Required();
		}


		public override ProductType Type { get { return ProductType.SimCard; } }

		public override string Name { get { return Number; } }

		public override string PassengerName { get { return GetPassengerName(); } set { SetPassengerName(value); } }

		[Patterns.Passenger]
		public virtual Person Passenger { get { return GetPassenger(); } set { SetPassenger(value); } }

		[Patterns.Number, EntityName2, Required, MaxLength(16)]
		public virtual string Number { get; set; }

		[RU("Продажа SIM-карты")]
		public virtual bool IsSale { get; set; }


		public new partial class Service : Service<SimCard>
		{

		}

	}

}
