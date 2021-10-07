using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public static class EFExtensions
	{

		public static Domain.EntityConfiguration<TEntity> Config<TEntity>(
			this Domain.EntityInfo[] infos
		)
			where TEntity : Domain.Entity
		{
			if (infos == null) return null;
			var entityType = typeof(TEntity);

			return new Domain.EntityConfiguration<TEntity>(infos.By(entityType));
		}

	}

}
