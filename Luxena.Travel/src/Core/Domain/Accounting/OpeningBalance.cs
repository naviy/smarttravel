using System;


namespace Luxena.Travel.Domain
{

	[RU("Начальный остаток", "Начальные остатки")]
	[SupervisorPrivileges]
	public partial class OpeningBalance : Entity2
	{
		[Patterns.Number, EntityName]
		public virtual string Number { get; set; }

		[Patterns.Date]
		public virtual DateTime Date { get; set; }

		[RU("Контрагент")]
		public virtual Party Party { get; set; }

		[RU("Остаток")]
		public virtual decimal Balance { get; set; }


		public class Service : Entity2Service<OpeningBalance>
		{
			public Service()
			{
				Calculating += r =>
				{
					if (r.Number.No())
						r.Number = NewSequence();
				};
			}
		}

	}

}