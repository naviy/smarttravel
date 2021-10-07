using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Трансфер", "Трансферы"), Icon("cab")]
	[UA("Трансфер")]
	public partial class Transfer : Product
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<Transfer> se)
		{
			se.For(a => a.ReissueFor)
				.Lookup<Transfer>();

			se.For(a => a.Provider)
				.Lookup<TransferProvider>();
		}

		public override ProductType Type => ProductType.Transfer;

	}


	partial class Domain
	{
		public DbSet<Transfer> Transfers { get; set; }
	}

}
