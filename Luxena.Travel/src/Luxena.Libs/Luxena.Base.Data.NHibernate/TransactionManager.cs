using System;
using System.Collections.Generic;

using NHibernate;


namespace Luxena.Base.Data.NHibernate
{
	public class TransactionManager : ITransactionManager
	{
		public TransactionManager(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;

			_comments = new Dictionary<object, string>();
		}

		public IInterceptor Interceptor
		{
			get { return _interceptor; }
			set
			{
				_interceptor = value;

				var aware = _interceptor as ITransactionManagerAware;

				if (aware != null)
					aware.SetTransactionManager(this);
			}
		}

		public ISession Session
		{
			get
			{
				BeginWork();

				return _session;
			}
		}

		public IDictionary<object, string> Comments
		{
			get { return _comments; }
		}

		public void Dispose()
		{
			if (_session == null)
				return;

			_session.Dispose();
			_session = null;
		}

		public void BeginWork()
		{
			if (_session == null)
			{
				_session = Interceptor == null ? _sessionFactory.OpenSession() : _sessionFactory.OpenSession(Interceptor);

				_session.BeginTransaction();
			}
			else if (!_session.IsConnected)
			{
				_session.Reconnect();

				_session.BeginTransaction();
			}
		}

		private Exception FullException(Exception ex)
		{
			string message = null;

			while (ex != null)
			{
				message += (string.IsNullOrEmpty(message) ? null : "\r\n") + ex.Message;
				ex = ex.InnerException;
			}

			return new Exception(message, ex);
		}


		public void Flush()
		{
			try 
			{ 
				if (_session != null && _session.IsConnected)
					Session.Flush();
			}
			catch (Exception ex)
			{
				throw FullException(ex);
			}
		}

		public void Commit()
		{
			CompleteTransaction(true);
		}

		public void Rollback()
		{
			CompleteTransaction(false);
		}

		public void Close()
		{
			if (_session == null)
				return;

			_session.Dispose();
			_session = null;
		}

		public T By<T>(object id) where T: class
		{
			return id != null ? Session.Get<T>(id) : null;
		}

		public T Get<T>(object id)
		{
			return Session.Get<T>(id);
		}

		public object Get(System.Type type, object id)
		{
			return Session.Get(type, id);
		}

		public object Get(System.Type type, object id, object version)
		{
			var obj = Session.Get(type, id);

			if (obj != null && !Equals(version, Session.SessionFactory.GetClassMetadata(type).GetVersion(obj, EntityMode.Poco)))
				throw new StaleStateException("Object has been modified by another user");

			return obj;
		}

		public T Refer<T>(object id)
		{
			return id == null ? default(T) : Session.Load<T>(id);
		}

		public object Refer(System.Type type, object id)
		{
			return id == null ? null : Session.Load(type, id);
		}

		public void Save(object obj)
		{
			Session.SaveOrUpdate(obj);
		}

		public void Delete(object obj)
		{
			Session.Delete(obj);
		}

		public void Delete(System.Type type, object id)
		{
			Delete(Refer(type, id));
		}

		public void Comment(object obj, string comment)
		{
			_comments[obj] = comment;
		}

		private void CompleteTransaction(bool commit)
		{
			if (_session == null || !_session.IsConnected)
				return;

			try 
			{ 
				if (commit)
				{
					_session.Transaction.Commit();

					_comments.Clear();

					_session.Disconnect();
				}
				else
				{
					_session.Transaction.Rollback();

					_comments.Clear();

					_session.Clear();

					Close();
				}
			}
			catch (Exception ex)
			{
				throw FullException(ex);
			}

		}

		private readonly ISessionFactory _sessionFactory;

		private readonly Dictionary<object, string> _comments;

		private ISession _session;

		private IInterceptor _interceptor;
	}
}