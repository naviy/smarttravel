using System.Collections;
using System.Collections.Generic;

using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;


namespace Luxena.Base.Data.NHibernate
{
	internal class CompositeInterceptor : IInterceptor
	{
		public CompositeInterceptor(IEnumerable<IInterceptor> interceptors)
		{
			_interceptors = interceptors;
		}

		public bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
		{
			var result = false;

			foreach (var interceptor in _interceptors)
				result = result || interceptor.OnLoad(entity, id, state, propertyNames, types);

			return result;
		}

		public bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
		{
			var result = false;

			foreach (var interceptor in _interceptors)
				result = result || interceptor.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);

			return result;
		}

		public bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
		{
			var result = false;

			foreach (var interceptor in _interceptors)
				result = result || interceptor.OnSave(entity, id, state, propertyNames, types);

			return result;
		}

		public void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
		{
			foreach (var interceptor in _interceptors)
				interceptor.OnDelete(entity, id, state, propertyNames, types);
		}

		public void PreFlush(ICollection entities)
		{
			foreach (var interceptor in _interceptors)
				interceptor.PreFlush(entities);
		}

		public void PostFlush(ICollection entities)
		{
			foreach (var interceptor in _interceptors)
				interceptor.PostFlush(entities);
		}

		public int[] FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
		{
			var tmpList = new List<int>();

			int[] result = null;

			foreach (var interceptor in _interceptors)
			{
				result = interceptor.FindDirty(entity, id, currentState, previousState, propertyNames, types);

				if (result != null)
					tmpList.AddRange(result);
			}

			if (tmpList.Count > 0)
				result = tmpList.ToArray();

			return result;
		}

		public void AfterTransactionBegin(ITransaction tx)
		{
			foreach (var interceptor in _interceptors)
				interceptor.AfterTransactionBegin(tx);
		}

		public void BeforeTransactionCompletion(ITransaction tx)
		{
			foreach (var interceptor in _interceptors)
				interceptor.BeforeTransactionCompletion(tx);
		}

		public void AfterTransactionCompletion(ITransaction tx)
		{
			foreach (var interceptor in _interceptors)
				interceptor.AfterTransactionCompletion(tx);
		}

		public void SetSession(ISession session)
		{
			foreach (var interceptor in _interceptors)
				interceptor.SetSession(session);
		}

		public void OnCollectionRecreate(object collection, object key)
		{
			foreach (var interceptor in _interceptors)
				interceptor.OnCollectionRecreate(collection, key);
		}

		public void OnCollectionRemove(object collection, object key)
		{
			foreach (var interceptor in _interceptors)
				interceptor.OnCollectionRemove(collection, key);
		}

		public void OnCollectionUpdate(object collection, object key)
		{
			foreach (var interceptor in _interceptors)
				interceptor.OnCollectionUpdate(collection, key);
		}

		public bool? IsTransient(object entity)
		{
			bool? result = null;

			foreach (var interceptor in _interceptors)
			{
				var isTransient = interceptor.IsTransient(entity);

				if (isTransient.HasValue)
					result = result ?? isTransient.Value;
			}

			return result;
		}

		public object Instantiate(string entityName, EntityMode entityMode, object id)
		{
			object result = null;

			foreach (var interceptor in _interceptors)
			{
				if (result == null)
					result = interceptor.Instantiate(entityName, entityMode, id);
				else
					interceptor.Instantiate(entityName, entityMode, id);
			}

			return result;
		}

		public string GetEntityName(object entity)
		{
			return null;
		}

		public object GetEntity(string entityName, object id)
		{
			return null;
		}

		public SqlString OnPrepareStatement(SqlString sql)
		{
			return sql;
		}

		private readonly IEnumerable<IInterceptor> _interceptors;
	}
}