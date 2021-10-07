using Luxena.Base.Domain;
using Luxena.Domain.Entities;


namespace Luxena.Domain.Web
{

	public abstract class EntityController<TDomain, TEntity, TEntityService> : DomainController<TDomain>
		where TDomain : Domain<TDomain>
		where TEntity : class, IEntity
		where TEntityService : EntityService<TDomain, TEntity>, new()
	{

		public TEntityService Service { get { return Domain.Service(ref _entityService); } }
		private TEntityService _entityService;

	}

}