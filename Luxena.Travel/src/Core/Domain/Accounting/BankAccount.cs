using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Банковский счёт", "Банковские счёта"), Small]
	[SupervisorPrivileges]
	public partial class BankAccount : Entity3D
	{

		[RU("Использовать по умолчанию")]
		public virtual bool IsDefault { get; set; }

		[Patterns.Note]
		public virtual string Note { get; set; }


		public class Service : Entity3Service<BankAccount>
		{

		}

	}

}