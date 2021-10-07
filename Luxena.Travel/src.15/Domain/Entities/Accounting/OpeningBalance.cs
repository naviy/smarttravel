using System;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Начальный остаток", "Начальные остатки")]
	[SupervisorPrivileges]
	public partial class OpeningBalance : Entity2
	{

		[Patterns.Number, EntityName]
		public string Number { get; set; }

		[Patterns.Date]
		public DateTimeOffset Date { get; set; }

		[RU("Контрагент")]
		protected Party _Party;

		[RU("Остаток")]
		public decimal Balance { get; set; }


		static partial void Config_(Domain.EntityConfiguration<OpeningBalance> entity)
		{
			entity.Association(a => a.Party);//, a => a.OpeningBalances);
		}


		public override void Calculate()
		{
			base.Calculate();
			if (Number.No())
				Number = db.NewSequence<OpeningBalance>();
		}

	}


	partial class Domain
	{
		public DbSet<OpeningBalance> OpeningBalances { get; set; }
	}

}