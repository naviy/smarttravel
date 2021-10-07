using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("SIM-карта", "SIM-карты"), Icon("mobile")]
	[UA("SIM-картка")]
	public partial class SimCard : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<SimCard> sm)
		{
			sm.For(a => a.ReissueFor)
				.Lookup<SimCard>();

			sm.For(a => a.Producer)
				.Lookup<RoamingOperator>()
				.RU("Оператор")
				.Required();
		}


		public override ProductType Type { get { return ProductType.SimCard; } set { } }

		[Patterns.Number, EntityName2, Required, MaxLength(16)]
		public string Number { get; set; }

		[RU("Продажа SIM-карты")]
		public bool IsSale { get; set; }


		public override void Calculate()
		{
			base.Calculate();

			Name = Number;
		}
	}


	partial class Domain
	{
		public DbSet<SimCard> SimCards { get; set; }
	}
}
