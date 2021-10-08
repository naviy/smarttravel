using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("МСО", "МСО")]
	[UA("MCO")]
	public partial class AviaMco : AviaDocument
	{

		public override ProductType Type => ProductType.AviaMco;

		[Patterns.Description, Text]
		public string Description { get; set; }

		[RU("Связан с")]
		protected AviaDocument _InConnectionWith;


		static partial void Config_(Domain.EntityConfiguration<AviaMco> entity)
		{
			entity.Association(a => a.InConnectionWith, a => a.AviaMcos_InConnectionWith);
		}


		public override string GetOrderItemText(string lang) =>
			Description.Yes() ? Description : Localization(lang) + GetOrderItemText2(lang);

	}


	partial class Domain
	{
		public DbSet<AviaMco> AviaMcos { get; set; }
	}

}