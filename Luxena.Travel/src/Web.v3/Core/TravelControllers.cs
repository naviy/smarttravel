using Luxena.Base.Domain;
using Luxena.Domain.Web;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Controllers
{

	public abstract class DomainApiController
		: DomainApiController<Domain.Domain>
	{
	}

	public abstract class DomainController
		: DomainController<Domain.Domain>
	{
	}


	public abstract class EntityApiController<TEntity, TEntityService>
		: EntityApiController<Domain.Domain, TEntity, TEntityService>
		where TEntity : class, IEntity
		where TEntityService : EntityService<TEntity>, new()
	{
	}


	public abstract class EntityController<TEntity, TEntityService>
		: EntityController<Domain.Domain, TEntity, TEntityService>
		where TEntity : class, IEntity
		where TEntityService : EntityService<TEntity>, new()
	{
	}

}