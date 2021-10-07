using Luxena.Base.Domain;
using Luxena.Domain.Entities;


namespace Luxena.Domain.Contracts
{

	public class EntityContract : IEntity
	{
		public object Id { get; set; }
		public int Version { get; set; }
	}

	public class Entity3Contract : EntityContract
	{
		public string Name { get; set; }
	}

	public class Entity3DContract : Entity3Contract
	{
		public string Description { get; set; }
	}


	public abstract class Entity3ContractService<TDomain, TContracts, TEntity, TEntityService, TContract> 
		: EntityContractService<TDomain, TContracts, TEntity, TEntityService, TContract>
		where TDomain : Domain<TDomain>
		where TContracts : Contracts<TDomain, TContracts>
		where TEntity : class, IEntity3
		where TEntityService : EntityService<TDomain, TEntity>
		where TContract : Entity3Contract, new()
	{

		protected Entity3ContractService()
		{
			ContractFromEntity += (r, c) => c.Name = r.Name;
			EntityFromContract += (r, c) => r.Name = c.Name + db;
		}

	}


	public abstract class Entity3DContractService<TDomain, TContracts, TEntity, TEntityService, TContract>
		: Entity3ContractService<TDomain, TContracts, TEntity, TEntityService, TContract>
		where TDomain : Domain<TDomain>
		where TContracts : Contracts<TDomain, TContracts>
		where TEntity : class, IEntity3D
		where TEntityService : EntityService<TDomain, TEntity>
		where TContract : Entity3DContract, new()
	{

		protected Entity3DContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Description = r.Description;
			};

			EntityFromContract += (r, c) =>
			{
				r.Description = c.Description + db;
			};
		}

	}

}