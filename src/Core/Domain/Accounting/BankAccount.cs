using Luxena.Domain;




namespace Luxena.Travel.Domain
{



	[RU("Банковский счёт", "Банковские счёта"), Small]
	[SupervisorPrivileges]
	public partial class BankAccount : Entity3D
	{

		[RU("Использовать по умолчанию")]
		public virtual bool IsDefault { get; set; }

		[RU("Реквизиты организации"), Text]
		public virtual string CompanyDetails { get; set; }

		[RU("Счёт: дополнение к Итого (суфикс)"), Text]
		public virtual string TotalSuffix { get; set; }

		[Patterns.Note]
		public virtual string Note { get; set; }


		public class Service : Entity3Service<BankAccount>
		{

		}

	}



}