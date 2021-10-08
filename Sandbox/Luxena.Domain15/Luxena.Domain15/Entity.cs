using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Luxena.Travel.Domain
{

	public abstract class Entity : Domain.Entity
	{
		
	}



	public static class EntityExtentions
	{

		[DebuggerStepThrough]
		public static List<TEntity> Clone<TEntity>(this IList<TEntity> entities, Domain db)
			where TEntity : Domain.Entity
		{
			return entities == null ? null : entities.Select(a => a.Clone(db)).Cast<TEntity>().ToList();
		}


		//		[DebuggerStepThrough]
		public static void Delete(this Entity entity, Domain db)
		{
			Save(db, entity, true);
		}

		//		[DebuggerStepThrough]
		public static void Delete(this IEnumerable<Entity> entities, Domain db)
		{
			Save(db, entities, true);
		}


//		[DebuggerStepThrough]
		public static void Save(this Entity entity, Domain db)
		{
			Save(db, entity, false);
		}

//		[DebuggerStepThrough]
		public static void Save(this IEnumerable<Entity> entities, Domain db)
		{
			Save(db, entities, false);
		}


		//		[DebuggerStepThrough]
		private static void Save(Domain db, Entity entity, bool isDeleted)
		{
			if (entity == null) return;

			db.CurrentTransaction.Save(entity, true, isDeleted);
		}

		private static void Save(Domain db, IEnumerable<Entity> entities, bool isDeleted)
		{
			if (entities == null) return;

			var tran = db.CurrentTransaction;

			foreach (var entity in entities)
			{
				if (entity == null) continue;

				tran.Save(entity, true, isDeleted);
			}
		}

	}


}