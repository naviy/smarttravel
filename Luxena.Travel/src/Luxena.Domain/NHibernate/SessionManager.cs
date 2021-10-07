using System;
using System.Diagnostics;

using NHibernate;


namespace Luxena.Domain.NHibernate
{

	public class SessionManager
	{

		public SessionManager(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
		}

		public ISession Session
		{
			get
			{
				if (_session == null)
					throw new InvalidOperationException("No work started");

				return _session;
			}
		}

		[DebuggerStepThrough]
		public IUnitOfWork BeginWork()
		{
			if (_session == null)
			{
				_session = _sessionFactory.OpenSession();
				_session.BeginTransaction();
				_refCount = 0;
			}

			++_refCount;

			return new UnitOfWork(this);
		}

		private void Commit()
		{
			CheckRefCount();

			if (_refCount == 1)
			{
				_session.Transaction.Commit();
				_session = null;
			}

			--_refCount;
		}

		private void Rollback()
		{
			CheckRefCount();

			if (--_refCount != 0)
				return;

			_session.Dispose();
			_session = null;
		}

		private void CheckRefCount()
		{
			if (_refCount == 0)
				throw new InvalidOperationException("Invalid dereference operation");
		}


		private class UnitOfWork : IUnitOfWork
		{
			public UnitOfWork(SessionManager manager)
			{
				_manager = manager;
			}

			public ISession Session
			{
				get { return EnsureManager()._session; }
			}

			public void Commit()
			{
				EnsureManager().Commit();

				_manager = null;
			}

			public void Dispose()
			{
				EnsureNotDisposed();

				_disposed = true;

				if (_manager == null)
					return;

				_manager.Rollback();
				_manager = null;
			}

			private void EnsureNotDisposed()
			{
				if (_disposed)
					throw new ObjectDisposedException("Unit of Work");
			}

			private SessionManager EnsureManager()
			{
				EnsureNotDisposed();

				if (_manager == null)
					throw new InvalidOperationException("Unit of Work is committed");

				return _manager;
			}

			private SessionManager _manager;
			private bool _disposed;
		}

		private readonly ISessionFactory _sessionFactory;
		private ISession _session;
		private int _refCount;
	}
}