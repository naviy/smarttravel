using System;

using NHibernate;


namespace Luxena.Domain.NHibernate
{

	public interface IUnitOfWork : IDisposable
	{
		ISession Session { get; }

		void Commit();
	}

}