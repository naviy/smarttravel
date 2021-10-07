using Luxena.Base.Domain;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class Contracts : Contracts<Domain, Contracts>
	{
	}


	public class EntityContractService : EntityContractService<Domain, Contracts>
	{
	}

	

	public abstract class EntityContractService<TEntity, TEntityService, TContract> 
		: EntityContractService<Domain, Contracts, TEntity, TEntityService, TContract>
		where TEntity : class, IEntity
		where TEntityService : EntityService<TEntity>
		where TContract : class, IEntity, new()
	{
	}

	public abstract class Entity3ContractService<TEntity, TEntityService, TContract>
		: Entity3ContractService<Domain, Contracts, TEntity, TEntityService, TContract>
		where TEntity : class, IEntity3
		where TEntityService : EntityService<TEntity>
		where TContract : Entity3Contract, new()
	{
	}

	public abstract class Entity3DContractService<TEntity, TEntityService, TContract>
		: Entity3DContractService<Domain, Contracts, TEntity, TEntityService, TContract>
		where TEntity : class, IEntity3D
		where TEntityService : EntityService<TEntity>
		where TContract : Entity3DContract, new()
	{
	}

}
