using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Тип питания", "Типы питания"), Small]
	[SupervisorPrivileges]
	public partial class CateringType : Entity3D
	{

		public class Service : Entity3Service<CateringType>
		{

		}

	}

}
