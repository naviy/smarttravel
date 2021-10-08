using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public enum IsicCardType
	{
		[RU("Неизвестно")]
		Unknown,

		Isic,
		ITIC,
		IYTC,
	}


	[RU("Студенческий билет", "Студенческие билеты")]
	public partial class Isic : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Isic> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<Isic>();
		}

		public override ProductType Type => ProductType.Isic;

		public override string Name => Number1 + " " + Number2;

		public override string PassengerName { get { return GetPassengerName(); } set { SetPassengerName(value); } }


		[Patterns.Passenger]
		public virtual Person Passenger { get { return GetPassenger(); } set { SetPassenger(value); } }

		[RU("Тип карты"), Required, DefaultValue(1)]
		public virtual IsicCardType CardType { get; set; }

		[Patterns.Number, MaxLength(12), Required]
		public virtual string Number1 { get; set; }

		[Patterns.Number, MaxLength(1), Required]
		public virtual string Number2 { get; set; }


		public new partial class Service : Service<Isic>
		{

		}

	}

}
