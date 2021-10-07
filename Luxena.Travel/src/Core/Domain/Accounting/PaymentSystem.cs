namespace Luxena.Travel.Domain
{

	[RU("Платёжная система", "Платёжные системы")]
	[SupervisorPrivileges]
	public partial class PaymentSystem : Entity3
	{

		public class Service : Entity3Service<PaymentSystem>
		{

		}

	}

}