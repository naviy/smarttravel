namespace Luxena.Travel.Domain
{

	[RU("Вид дополнительной услуги", "Виды дополнительных услуг")]
	[SupervisorPrivileges]
	public partial class GenericProductType : Entity3
	{

		public class Service : Entity3Service<GenericProductType>
		{

		}

	}



}
