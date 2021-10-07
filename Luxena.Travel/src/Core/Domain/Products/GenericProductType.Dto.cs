using Luxena.Base.Serialization;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class GenericProductTypeDto : Entity3Contract { }

	public partial class GenericProductTypeContractService 
		: Entity3ContractService<GenericProductType, GenericProductType.Service, GenericProductTypeDto>
	{
	}

}
