using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Страховка или возврат", "Страховки и возвраты"), Icon("fire-extinguisher")]
	public abstract partial class InsuranceDocument : Product
	{

		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<InsuranceDocument> se)
		{
			se.For(a => a.Type)
				.Enum(ProductType.Insurance, ProductType.InsuranceRefund);

			se.For(a => a.ReissueFor)
				.Lookup<Insurance>();

			se.For(a => a.Producer)
				.Lookup<InsuranceCompany>()
				.RU("Страховая компания")
				.Required();
		}


		[Patterns.Number, EntityName2, Required]
		public string Number { get; set; }

	}


	[RU("Страховка", "Страховки")]
	[UA("Страховка")]
	public partial class Insurance : InsuranceDocument
	{
		public override ProductType Type => ProductType.Insurance;
	}


	[RU("Возврат страховки", "Возвраты страховок")]
	[UA("Повернення страховки")]
	public partial class InsuranceRefund : InsuranceDocument
	{
		public override ProductType Type => ProductType.InsuranceRefund;

		public override bool IsRefund => true;
	}


	partial class Domain
	{
		public DbSet<InsuranceDocument> InsuranceDocuments { get; set; }
		public DbSet<Insurance> Insurances { get; set; }
		public DbSet<InsuranceRefund> InsuranceRefunds { get; set; }
	}

}
