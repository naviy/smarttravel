using Luxena.Domain;




namespace Luxena.Travel.Domain
{

	[RU("ÌÑÎ", "ÌÑÎ")]
	public partial class AviaMco : AviaDocument
	{

		public override ProductType Type => ProductType.AviaMco;

		[Patterns.Description, Text]
		public virtual string Description { get; set; }

		[RU("Ñâÿçàí ñ")]
		public virtual AviaDocument InConnectionWith { get; set; }


		public override Entity Resolve(Domain db)
		{
			base.Resolve(db);
			var r = this;

			r.ReissueFor = db.AviaDocument.ByFullNumber(r.ReissueFor);

			r.InConnectionWith = db.AviaDocument.ByNumber(r.InConnectionWith);

			if (r.Origin == ProductOrigin.AmadeusPrint)
				db.AviaDocument.ResolvePrintDocumentCommission(r);

			return r;
		}

		public new class Service : Service<AviaMco>
		{

			protected override bool GetIsProcessed(AviaMco r)
			{
				return base.GetIsProcessed(r) &&
					!(db.Configuration.McoRequiresDescription && r.Description.No());
			}

		}

	}

}