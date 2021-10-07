using Luxena.Base.Serialization;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class AccommodationTypeDto : Entity3DContract { }

	public partial class AccommodationTypeContractService : Entity3DContractService<AccommodationType, AccommodationType.Service, AccommodationTypeDto>
	{
	}

}
