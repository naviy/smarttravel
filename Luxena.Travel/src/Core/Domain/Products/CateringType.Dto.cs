using Luxena.Base.Serialization;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class CateringTypeDto : Entity3DContract { }

	public partial class CateringTypeContractService 
		: Entity3DContractService<CateringType, CateringType.Service, CateringTypeDto>
	{
	}

}