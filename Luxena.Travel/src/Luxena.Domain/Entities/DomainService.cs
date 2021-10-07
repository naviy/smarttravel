using System;
using System.Diagnostics;

using Luxena.Base.Metamodel;

using NHibernate;


namespace Luxena.Domain.Entities
{

	public abstract class DomainService
	{
		public virtual Type GetEntityType()
		{
			throw new NotImplementedException();
		}

		public virtual Type GetDeclaringEntityType()
		{
			return GetType().DeclaringType;
		}

		protected static Class FetchClass(Type entityType)
		{
			return Class.Of(entityType);
		}
		protected static Class FetchClass<TEntity>()
		{
			return FetchClass(typeof(TEntity));
		}
	}


	public abstract class DomainService<TDomain> : DomainService
		where TDomain : Domain<TDomain>
	{
		public TDomain Domain
		{
			[DebuggerStepThrough] get { return db; } 
			[DebuggerStepThrough] set { db = value; }
		}
		protected internal TDomain db;

		[DebuggerStepThrough]
		public virtual void Init() { }


		#region Session

		public ISession Session => Domain.Session;

		#endregion

	}
	

}