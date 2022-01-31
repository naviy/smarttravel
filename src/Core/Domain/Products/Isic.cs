using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;




namespace Luxena.Travel.Domain
{



	//===g






	public enum IsicCardType
	{
		[RU("Неизвестно")]
		Unknown,

		Isic,
		ITIC,
		IYTC,
	}






	//===g






	[RU("Студенческий билет", "Студенческие билеты")]
	public partial class Isic : Product
	{

		//---g



		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Isic> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<Isic>();
		}



		//---g



		public override ProductType Type => ProductType.Isic;

		public override string Name => (Number1 + " " + Number2).Clip() ?? PnrCode;


		public override string PassengerName { get => GetPassengerName();
			set => SetPassengerName(value);
		}


		[Patterns.Passenger]
		public virtual Person Passenger { get => GetPassenger();
			set => SetPassenger(value);
		}

		[RU("Тип карты"), Required, DefaultValue(1)]
		public virtual IsicCardType CardType { get; set; }

		[Patterns.Number, MaxLength(12), Required]
		public virtual string Number1 { get; set; }

		[Patterns.Number, MaxLength(1), Required]
		public virtual string Number2 { get; set; }



		//---g


		public new partial class Service : Service<Isic>
		{

		}




		//---g

	}






	//===g



}
