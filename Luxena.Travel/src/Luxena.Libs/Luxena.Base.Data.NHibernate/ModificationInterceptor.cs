using System;
using System.Collections;
using System.Collections.Generic;

using Common.Logging;

using Luxena.Base.Domain;
using Luxena.Base.Metamodel;

using NHibernate;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Type;


namespace Luxena.Base.Data.NHibernate
{
	public class ModificationInterceptor : EmptyInterceptor, ITransactionManagerAware
	{
		public void Initialize(string identityName)
		{
			_identityName = identityName;
		}

		public void SetTransactionManager(TransactionManager transManager)
		{
			_transManager = transManager;
		}

		public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
		{
			if (currentState == null || previousState == null)
				return false;

			var persistenceContext = _sessionImpl.PersistenceContext;

			var oldEntry = persistenceContext.GetEntry(entity);

			var persister = _sessionImpl.Factory.GetEntityPersister(oldEntry.EntityName);

			var dirtyProps = persister.FindDirty(currentState, previousState, entity, _sessionImpl);

			if (dirtyProps == null || dirtyProps.Length == 0)
				return false;

			var modification = new Modification
			{
				Type = ModificationType.Update
			};

			var versionProperty = GetClassMetadata(entity).VersionProperty;

			var createAware = entity as ICreateAware;

			var modifyAware = entity as IModifyAware;

			foreach (var i in dirtyProps)
			{
				if (i == versionProperty)
					continue;

				if (createAware != null && (propertyNames[i] == CreatedBy || propertyNames[i] == CreatedOn))
					continue;

				if (modifyAware != null && (propertyNames[i] == ModifiedBy || propertyNames[i] == ModifiedOn))
					continue;

				string oldValue;

				if (previousState[i] == null)
					oldValue = null;
				else if (previousState[i].GetClass().IsPersistent)
					oldValue = GetIdentifier(previousState[i]).ToString();
				else
					oldValue = previousState[i].ToString();

				modification.Items.Add(propertyNames[i], oldValue);
			}

			AddModification(modification, entity, id);

//			if (modifyAware != null)
//			{
//				modifyAware.ModifiedBy = _identityName;
//				modifyAware.ModifiedOn = DateTime.Now;

//				SetModificationInfo(propertyNames, currentState, 
//					ModifiedBy, _identityName, ModifiedOn, DateTime.Now
//				);
//			}

			return false;
		}

		public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
		{
			var createAware = entity as ICreateAware;

			if (createAware != null)
			{
				if (createAware.CreatedBy == null)
					createAware.CreatedBy = _identityName;
				createAware.CreatedOn = DateTime.Now;

				SetModificationInfo(propertyNames, state, CreatedBy, createAware.CreatedBy, CreatedOn, createAware.CreatedOn);
			}

			return false;
		}

		public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
		{
			var modification = new Modification
			{
				Type = ModificationType.Delete
			};

			AddModification(modification, entity, id);

			var versionProperty = GetClassMetadata(entity).VersionProperty;
			var isCreateAware = entity is ICreateAware;
			var isModifyAware = entity is IModifyAware;

			for (var i = 0; i < propertyNames.Length; i++)
			{
				if (i == versionProperty)
					continue;

				if (isCreateAware && (propertyNames[i] == CreatedBy || propertyNames[i] == CreatedOn))
					continue;

				if (isModifyAware && (propertyNames[i] == ModifiedBy || propertyNames[i] == ModifiedOn))
					continue;

				if (state[i] is IEnumerable)
					continue;

				string oldValue;

				if (state[i] == null)
					oldValue = null;
				else if (state[i].GetClass().IsPersistent)
					oldValue = GetIdentifier(state[i]).ToString();
				else
					oldValue = state[i] == null ? null : state[i].ToString();

				modification.Items.Add(propertyNames[i], oldValue);
			}
		}

		public override void OnCollectionUpdate(object collection, object key)
		{
		}

		public override void PostFlush(ICollection entities)
		{
			if (_modifications.Count == 0)
				return;

			var session = _session.SessionFactory.OpenSession(_session.Connection);
			try
			{
				foreach (var modification in _modifications)
					session.Save(modification);

				session.Flush();
			}
			catch (HibernateException ex)
			{
				throw new CallbackException(ex);
			}
			finally
			{
				_modifications.Clear();

				try
				{
					session.Close();
				}
				catch (HibernateException ex)
				{
					throw new CallbackException(ex);
				}
			}
		}

		public override void SetSession(ISession session)
		{
			_session = session;
			_sessionImpl = (ISessionImplementor)session;
		}

		private void AddModification(Modification modification, object entity, object id)
		{
			System.Type entityType = entity.GetClass().Type;

			if (_log.IsDebugEnabled)
				_log.Debug(string.Format("Add modification (class: {0}, type: {1}, instance: {2})", entityType.FullName, modification.Type, id));

			modification.Author = _identityName;
			modification.TimeStamp = DateTime.Now;
			modification.InstanceType = entityType.Name;
			modification.InstanceId = id.ToString();
			modification.InstanceString = entity.ToString();

			string comment;
			if (_transManager.Comments.TryGetValue(entity, out comment))
				modification.Comment = comment;

			_modifications.Add(modification);
		}

		private static void SetModificationInfo(string[] propertyNames, object[] state, string authorProperty, string author, string timeStampProperty, DateTime timeStamp)
		{
			int changes = 0;

			for (int i = 0; i < propertyNames.Length && changes < 2; ++i)
			{
				if (propertyNames[i] == authorProperty)
				{
					state[i] = author;
					++changes;
				}
				else if (propertyNames[i] == timeStampProperty)
				{
					state[i] = timeStamp;
					++changes;
				}
			}
		}

		private IClassMetadata GetClassMetadata(object obj)
		{
			return _session.SessionFactory.GetClassMetadata(obj.GetClass().Type);
		}

		private object GetIdentifier(object obj)
		{
			return GetClassMetadata(obj).GetIdentifier(obj, EntityMode.Poco);
		}

		private static readonly ILog _log = LogManager.GetLogger(typeof(ModificationInterceptor));

		private ISession _session;
		private ISessionImplementor _sessionImpl;

		private TransactionManager _transManager;

		private readonly List<Modification> _modifications = new List<Modification>();

		private string _identityName;

		private const string CreatedBy = "CreatedBy";
		private const string CreatedOn = "CreatedOn";
		private const string ModifiedBy = "ModifiedBy";
		private const string ModifiedOn = "ModifiedOn";
	}
}