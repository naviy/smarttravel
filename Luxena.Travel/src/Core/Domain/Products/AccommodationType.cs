using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Тип проживания", "Типы проживания"), Small]
	[SupervisorPrivileges]
	public partial class AccommodationType : Entity3D
	{
		
		public class Service : Entity3Service<AccommodationType>
		{

		}

	}

}
