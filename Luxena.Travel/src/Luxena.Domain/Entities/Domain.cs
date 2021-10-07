using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Luxena.Base.Data;
using Luxena.Base.Data.NHibernate;
using Luxena.Base.Domain;
using Luxena.Base.Services;

using NHibernate;
using NHibernate.Proxy;


namespace Luxena.Domain.Entities
{

	public abstract class DomainBase
	{

		protected virtual void Init()
		{
			TransactionManager = NewTransactionManager();
		}


		#region IoC

		public abstract T Resolve<T>() where T : class;

		public T Resolve<T>(ref T obj) where T : class
		{
			return obj ?? (obj = Resolve<T>());
		}

		[DebuggerStepThrough]
		public DomainService Service(Type entityServiceType)
		{
			return (DomainService)_entityServiceProps[entityServiceType].GetValue(this);
		}


		[DebuggerStepThrough]
		public TEntityService Service<TEntityService>() where TEntityService : DomainService
		{
			var entityServiceProp = _entityServiceProps[typeof(TEntityService)];
			var value = (TEntityService)entityServiceProp.GetValue(this);
			return value;
		}

		[DebuggerStepThrough]
		public T Service<T>(ref T service) where T : DomainService
		{
			return service ?? (service = Service<T>());
		}


		[DebuggerStepThrough]
		public DomainService ServiceByEntity(Type entityType)
		{
			var entityServiceProp = _entityProps[entityType];
			var value = (DomainService)entityServiceProp.GetValue(this);
			return value;
		}


		[DebuggerStepThrough]
		protected TEntity ResolveSingleton<TEntity>(ref TEntity entity) where TEntity : class
		{
			return entity ?? (entity = Session.QueryOver<TEntity>().Cacheable().SingleOrDefault());
		}


		[DebuggerStepThrough]
		public static ICollection<Type> ServiceTypes()
		{
			return _entityServiceProps.Keys;
		}

		[DebuggerStepThrough]
		public IEnumerable<DomainService> GetServices()
		{
			return GetType().GetProperties()
				.Where(a => typeof(DomainService).IsAssignableFrom(a.PropertyType))
				.Select(a => (DomainService)a.GetValue(this));
		}

		#endregion


		#region Session

		public TransactionManager TransactionManager { get; protected set; }

		public ISession Session { [DebuggerStepThrough] get { return TransactionManager.Session; } }

		//Fake
		protected virtual TransactionManager NewTransactionManager()
		//protected virtual SessionManager NewSessionManager()
		{
			return Resolve<TransactionManager>();
		}


		public object By(Type type, object id)
		{
			return id == null ? null : Session.Get(type, id);
		}

		public object Load(Type type, object id)
		{
			return id == null ? null : Session.Load(type, id);
		}

		[DebuggerStepThrough]
		public void Flush()
		{
			TransactionManager.Flush();
		}

		#region Commit

		public readonly EntityEvents CommitActions = new EntityEvents();

		[DebuggerStepThrough]
		public void OnCommit<TEntity>(TEntity r, Action<TEntity> keyAction, Action<TEntity> action = null)
			where TEntity : class, IEntity
		{
			CommitActions.Add(this, r, keyAction, action);
		}

		[DebuggerStepThrough]
		public void OnCommit<TEntity>(IEntity mr, TEntity r, Action<TEntity> keyAction, Action<TEntity> action = null)
			where TEntity : class, IEntity
		{
			CommitActions.AddBefore(this, mr, r, keyAction, action);
		}


		[DebuggerStepThrough]
		private void Commit()
		{
			try
			{
				try
				{
					Committing();
				}
				finally
				{
					TransactionManager.Commit();
				}
				//work.Commit();
			}
			finally
			{
				_oldEntities = null;
			}
		}

		[DebuggerStepThrough]
		public void Committing()
		{
			try
			{
				CommitActions.Exec();
			}
			finally
			{
				CommitActions.Clear();
			}
		}

		[DebuggerStepThrough]
		public void Commit(Action action)
		{

			//using (var work = SessionManager.BeginWork())
			//{
			try
			{
				action();
				Commit();
			}
			catch (Exception ex)
			{
				Error(ex);
				Try(() => TransactionManager.Rollback());
				throw;
			}
		}

		[DebuggerStepThrough]
		public T Commit<T>(Func<T> func)
		{
			//using (var work = SessionManager.BeginWork())
			//{
			try
			{
				var result = func();
				Commit();
				return result;
			}
			catch (Exception ex)
			{
				Error(ex);
				Try(() => TransactionManager.Rollback());
				throw;
			}
			//}
		}

		#endregion


		#region State

		public abstract object GetId(object r);


		public bool IsNew(object r)
		{
			return GetId(r) == null;
		}


		[DebuggerStepThrough]
		public TEntity Unproxy<TEntity>(TEntity r) where TEntity : class, IEntity
		{
			// ReSharper disable once SuspiciousTypeConversion.Global
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			return
				r == null ? null :
				r is INHibernateProxy ? (TEntity)Session.GetSessionImplementation().PersistenceContext.Unproxy(r) :
				r;
		}

		private Dictionary<object, IDictionary<string, object>> _oldEntities;

		public void LoadOldValues(object r)
		{
			if (_oldEntities == null)
				_oldEntities = new Dictionary<object, IDictionary<string, object>>();

			if (!_oldEntities.ContainsKey(r))
				_oldEntities.Add(r, GetOldValues(r));
		}

		public TValue OldValue<TValue>(object r, string propertyName)
		{
			if (_oldEntities == null) return default(TValue);

			var oldValues = _oldEntities.By(r);
			if (oldValues == null) return default(TValue);

			return (TValue)oldValues.By(propertyName);
		}

		public TValue OldValue<TEntity, TValue>(TEntity r, Expression<Func<TEntity, TValue>> getProperty)
		{
			if (_oldEntities == null)
				return default(TValue);

			var oldValues = _oldEntities.By(r);
			if (oldValues == null)
				return default(TValue);

			var props = GetPropertyInfos(getProperty);

			var prop = props.First();
			return (TValue)oldValues.By(prop.Name);
		}

		public bool IsDirty<TEntity>(TEntity r, Expression<Func<TEntity, object>> getProperty)
		{
			if (_oldEntities == null) return false;


			var props = GetPropertyInfos(getProperty);

			var oldValues = _oldEntities.By(r);

			if (oldValues == null) return true;


			foreach (var prop in props)
			{
				var oldValue = oldValues.By(prop.Name);
				var newValue = prop.GetValue(r);

				if (!Equals(oldValue, newValue))
					return true;
			}


			return false;
		}


		public IDictionary<string, object> GetOldValues(object r)
		{
			if (IsNew(r)) return null;

			var session = Session;
			var sessionImpl = session.GetSessionImplementation();
			var persistenceContext = sessionImpl.PersistenceContext;
			var oldEntry = persistenceContext.GetEntry(r);
			var className = session.GetEntityName(r);
			var persister = sessionImpl.Factory.GetEntityPersister(className);
			var propNames = persister.PropertyNames;

			if (oldEntry == null)
			{
				var hibernateProxy = r as INHibernateProxy;
				if (hibernateProxy != null)
				{
					var proxy = hibernateProxy;
					var obj = persistenceContext.Unproxy(proxy);
					oldEntry = persistenceContext.GetEntry(obj);
				}
			}

			var oldState = oldEntry.As(a => a.LoadedState);
			if (oldState == null) return null;

			var values = new Dictionary<string, object>();
			for (int i = 0, len = propNames.Length; i < len; i++)
			{
				values[propNames[i]] = oldState[i];
			}

			return values;
		}

		protected IEnumerable<PropertyInfo> GetPropertyInfos<TEntity, TValue>(Expression<Func<TEntity, TValue>> getProperty)
		{
			Func<MemberInfo, PropertyInfo> getProp = member =>
			{
				var prop = member as PropertyInfo;
				if (prop == null || !prop.CanWrite || prop.SetMethod.IsPrivate)
					throw new ArgumentException("Domain.IsDirty: can't check the member " + member.DeclaringType.As(a => a.Name) + "." + member.Name);
				return prop;
			};


			var memberExpr = getProperty.Body as MemberExpression;
			if (memberExpr != null)
			{
				return new[] { getProp(memberExpr.Member) };
			}

			var newExpr = getProperty.Body as NewExpression;
			if (newExpr != null)
			{
				return newExpr.Arguments.OfType<MemberExpression>().Select(a => getProp(a.Member));
			}

			var unaryExpr = getProperty.Body as UnaryExpression;
			if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
			{
				memberExpr = unaryExpr.Operand as MemberExpression;
				if (memberExpr != null)
				{
					return new[] { getProp(memberExpr.Member) };
				}
			}

			throw new NotImplementedException();
		}

		//private int[] DirtyProperties(object r, out IEntityPersister persister, out object[] oldState, out object[] currentState)
		//{
		//	var session = Session;
		//	var sessionImpl = session.GetSessionImplementation();
		//	var persistenceContext = sessionImpl.PersistenceContext;
		//	var oldEntry = persistenceContext.GetEntry(r);
		//	var className = session.GetEntityName(r);
		//	persister = sessionImpl.Factory.GetEntityPersister(className);

		//	if ((oldEntry == null) && (r is INHibernateProxy))
		//	{
		//		var proxy = r as INHibernateProxy;
		//		var obj = sessionImpl.PersistenceContext.Unproxy(proxy);
		//		oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
		//	}

		//	oldState = oldEntry.As(a => a.LoadedState);

		//	currentState = persister.GetPropertyValues(r, sessionImpl.EntityMode);

		//	var dirtyProps = persister.FindDirty(currentState, oldState, r, sessionImpl);
		//	return dirtyProps;
		//}

		#endregion

		#endregion


		#region Operators

		public static bool operator +(bool x, DomainBase y)
		{
			return x;
		}

		public static bool? operator +(bool? x, DomainBase y)
		{
			return x;
		}

		public static DateTime operator +(DateTime x, DomainBase y)
		{
			return x;
		}

		public static DateTime? operator +(DateTime? x, DomainBase y)
		{
			return x;
		}

		public static decimal operator +(decimal x, DomainBase y)
		{
			return x;
		}

		public static decimal? operator +(decimal? x, DomainBase y)
		{
			return x;
		}

		public static double operator +(double x, DomainBase y)
		{
			return x;
		}

		public static double? operator +(double? x, DomainBase y)
		{
			return x;
		}

		public static int operator +(int x, DomainBase y)
		{
			return x;
		}

		public static int? operator +(int? x, DomainBase y)
		{
			return x;
		}

		public static long operator +(long x, DomainBase y)
		{
			return x;
		}

		public static long? operator +(long? x, DomainBase y)
		{
			return x;
		}

		public static string operator +(string x, DomainBase y)
		{
			return x.Clip();
		}

		#endregion


		#region Services, Messages, Events, Handlers

		public void Publish<TMessage>(TMessage e)
		{
			//			var handlers = GetAllInstances<IEventHandler<T>>();
			//			foreach (var handler in handlers)
			//				handler.Handle(e);

			var messageInfo = Messages.By(typeof(TMessage));
			if (messageInfo == null) return;

			var es = new object[] { e };

			foreach (var handlerInfo in messageInfo.Handlers)
			{
				handlerInfo.Invoke(this, es);
			}
		}


		protected static void InitEntityServices(Type domainType)
		{

			var serviceProps = domainType.GetProperties()
				.Where(a => a.PropertyType.IsSubclassOf(typeof(DomainService)))
				.ToArray();

			_entityServiceProps = serviceProps.ToDictionary(a => a.PropertyType);
			_entityProps = serviceProps
				.Where(a => a.PropertyType.DeclaringType != null)
				.ToDictionary(a => a.PropertyType.DeclaringType);

			Messages = (
				from serviceProp in serviceProps
				let serviceType = serviceProp.PropertyType
				let entityType = serviceType.DeclaringType
				let handlers = GetHandlerMethods(serviceType)
				from handler in handlers
				let message = handler.GetParameters().First().ParameterType
				group new HandlerInfo
				{
					ServiceProperty = serviceProp,
					EntityType = entityType,
					Method = handler,
				}
				by message into g
				select new MessageInfo
				{
					Message = g.Key,
					Handlers = g.ToArray(),
				}
			).ToDictionary(a => a.Message);
		}

		private static MethodInfo[] GetHandlerMethods(Type serviceType)
		{
			var methods = serviceType
				.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance)
				.Where(a => a.Name == "On" && a.GetParameters().Length == 1)
				.ToArray();
			return methods;
		}

		/// <summary>
		/// EntityServiceType => Service Property
		/// </summary>
		private static Dictionary<Type, PropertyInfo> _entityServiceProps;

		/// <summary>
		/// EntityType => Service Property
		/// </summary>
		private static Dictionary<Type, PropertyInfo> _entityProps;


		/// <summary>
		/// MessageType => MessageInfo
		/// </summary>
		public static IDictionary<Type, MessageInfo> Messages { get; private set; }


		public class MessageInfo
		{
			public Type Message { get; internal set; }
			public HandlerInfo[] Handlers { get; internal set; }
		}

		public class HandlerInfo
		{
			public PropertyInfo ServiceProperty { get; internal set; }
			public Type EntityType { get; internal set; }
			public MethodInfo Method { get; internal set; }

			public void Invoke(DomainBase domain, object[] e)
			{
				var service = ServiceProperty.GetValue(domain);
				Method.Invoke(service, e);
			}
		}

		#endregion


		#region GenericService

		private GenericService GenericService => Resolve(ref _genericService);

		private GenericService _genericService;

		public RangeResponse GetRange(RangeRequest prms)
		{
			return GenericService.GetRange(prms.ClassName, prms, false);
		}

		public RangeResponse GetRange<T>(RangeRequest prms)
		{
			return GenericService.GetRange<T>(prms, false);
		}

		public RangeResponse Suggest<TEntity>(RangeRequest prms)
		{
			return GenericService.Suggest(typeof(TEntity).Name, prms);
		}


		public OperationStatus CanList(Type entityType)
		{
			return ServiceByEntity(entityType).As<IEntityPermissions>().As(a => a.CanList());
		}

		#endregion


		#region Log

		public abstract void Error(Exception ex);

		[DebuggerStepThrough]
		public void Try(Action action)
		{
			if (action == null) return;

			try
			{
				action();
			}
			catch (Exception ex)
			{
				Error(ex);
			}
		}

		[DebuggerStepThrough]
		public T Try<T>(Func<T> func)
		{
			if (func == null) return default(T);

			try
			{
				return func();
			}
			catch (Exception ex)
			{
				Error(ex);
			}

			return default(T);
		}

		#endregion

	}


	public interface IEntityServiceContainer<TDomain, TEntity>
		where TDomain : Domain<TDomain>
		where TEntity : class, IEntity
	{
		EntityService<TDomain, TEntity> Service { get; }
	}


	public abstract partial class Domain<TDomain> : DomainBase
		where TDomain : Domain<TDomain>
	{

		public readonly TDomain db;

		public object UserModifingEntity;


		protected Domain()
		{
			db = (TDomain)this;
		}

		public static void Configure()
		{
			InitEntityServices(typeof(TDomain));
		}

		#region IoC

		[DebuggerStepThrough]
		private TEntityService ResolveService<TEntityService>() where TEntityService : DomainService<TDomain>, new()
		{
			//var service = Resolve<TEntityService>();
			//service.Domain = (TDomain)this;
			var service = new TEntityService { Domain = (TDomain)this };
			service.Init();
			_resolvedServices.Add(service);
			return service;
		}

		private readonly List<DomainService<TDomain>> _resolvedServices = new List<DomainService<TDomain>>();

		[DebuggerStepThrough]
		protected T ResolveService<T>(ref T service) where T : DomainService<TDomain>, new()
		{
			return service ?? (service = ResolveService<T>());
		}

		public static IEnumerable<Type> GetServiceTypes()
		{
			return typeof(TDomain).GetProperties()
				.Where(a => typeof(DomainService).IsAssignableFrom(a.PropertyType))
				.Select(a => a.PropertyType);
		}

		public static IEnumerable<Type> GetEntityTypes()
		{
			return typeof(TDomain).GetProperties()
				.Where(a => typeof(DomainService).IsAssignableFrom(a.PropertyType) && a.PropertyType.DeclaringType != null)
				.Select(a => a.PropertyType.DeclaringType);
		}


		#endregion


		public EntityService<TDomain, TEntity> EntityService<TEntity>()
			where TEntity : class, IEntity
		{
			// ReSharper disable once SuspiciousTypeConversion.Global
			var container = this as IEntityServiceContainer<TDomain, TEntity>;
			if (container == null)
				throw new InvalidOperationException("Domain not contain interface IEntityServiceContainer<Domain, " + typeof(TEntity).Name + ">.");
			return container.Service;
		}

		public EntityService<TDomain, TEntity> AsEntityService<TEntity>()
			where TEntity : class, IEntity
		{
			var container = this as IEntityServiceContainer<TDomain, TEntity>;

			return container?.Service;
		}


		#region Permissions

		[DebuggerStepThrough]
		public void AssertCreate<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			EntityService<TEntity>().AssertCreate(me);
		}

		[DebuggerStepThrough]
		public void AssertCreate<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			EntityService<TEntity>().AssertCreate(me);
		}

		[DebuggerStepThrough]
		public void AssertDelete<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			EntityService<TEntity>().AssertDelete(me);
		}

		[DebuggerStepThrough]
		public void AssertDelete<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			EntityService<TEntity>().AssertDelete(me);
		}

		[DebuggerStepThrough]
		public void AssertDelete<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			EntityService<TEntity>().AssertDelete(me);
		}


		[DebuggerStepThrough]
		public void AssertUpdate<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			EntityService<TEntity>().AssertUpdate(me);
		}

		[DebuggerStepThrough]
		public void AssertUpdate<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			EntityService<TEntity>().AssertUpdate(me);
		}

		[DebuggerStepThrough]
		public void AssertUpdate<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			EntityService<TEntity>().AssertUpdate(me);
		}


		[DebuggerStepThrough]
		public OperationStatus CanCreate<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanCreate(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanCreate<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanCreate(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanCreate<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanCreate(me);
		}


		[DebuggerStepThrough]
		public OperationStatus CanCopy<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanCopy(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanCopy<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanCopy(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanCopy<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanCopy(me);
		}


		[DebuggerStepThrough]
		public OperationStatus CanDelete<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanDelete(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanDelete<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanDelete(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanDelete<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanDelete(me);
		}


		[DebuggerStepThrough]
		public OperationStatus CanList<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanList(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanList<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanList(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanList<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanList(me);
		}


		[DebuggerStepThrough]
		public OperationStatus CanReplace<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanReplace(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanReplace<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanReplace(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanReplace<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanReplace(me);
		}


		[DebuggerStepThrough]
		public OperationStatus CanUpdate<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanUpdate(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanUpdate<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanUpdate(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanUpdate<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanUpdate(me);
		}


		[DebuggerStepThrough]
		public OperationStatus CanView<TEntity>(TEntity me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanView(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanView<TEntity>(IEnumerable<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanView(me);
		}

		[DebuggerStepThrough]
		public OperationStatus CanView<TEntity>(IList<TEntity> me)
			where TEntity : class, IEntity
		{
			return EntityService<TEntity>().CanView(me);
		}

		#endregion


		#region Modify

		[DebuggerStepThrough]
		public bool Delete<TEntity>(TEntity r)
			where TEntity : class, IEntity
		{
			var service = AsEntityService<TEntity>();

			if (service != null)
				return service.Delete(r);

			Session.Delete(r);
			return true;
		}

		[DebuggerStepThrough]
		public void Delete<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (entities == null) return;

			var service = AsEntityService<TEntity>();

			if (service != null)
				service.Delete(entities);
			else
				entities.ForEach(r => Session.Delete(r));
		}


		[DebuggerStepThrough]
		public void Delete<TEntity>(IList<TEntity> entities)
			where TEntity : class, IEntity
		{
			Delete((IEnumerable<TEntity>)entities);
		}


		[DebuggerStepThrough]
		public TEntity Save<TEntity>(TEntity r)
			where TEntity : class, IEntity
		{
			var service = AsEntityService<TEntity>();

			if (service != null)
				return service.Save(r);

			Session.Save(r);
			return r;
		}

		[DebuggerStepThrough]
		public TEntity Save<TMasterEntity, TEntity>(TMasterEntity mr, TEntity r)
			where TMasterEntity : class, IEntity
			where TEntity : class, IEntity
		{
			var service = AsEntityService<TEntity>();

			if (service != null)
				return service.Save(mr, r);

			Session.Save(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Save<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class, IEntity
		{
			if (entities == null) return;

			var service = AsEntityService<TEntity>();

			if (service != null)
				service.Save(entities);
			else
				entities.ForEach(a => Session.Save(a));
		}

		[DebuggerStepThrough]
		public void Save<TEntity>(IList<TEntity> entities)
			where TEntity : class, IEntity
		{
			Save((IEnumerable<TEntity>)entities);
		}


		private void SaveKey(IEntity r) { }

		[DebuggerStepThrough]
		public void SaveOnCommit<TEntity>(TEntity r)
			where TEntity : class, IEntity
		{
			if (r != null)
				OnCommit(r, SaveKey, rr => Save(r));
		}

		#endregion

	}

}