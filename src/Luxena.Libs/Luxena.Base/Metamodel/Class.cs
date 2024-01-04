using System;
using System.Collections.Generic;
using System.Reflection;

using Luxena.Base.Domain;


namespace Luxena.Base.Metamodel
{

	public sealed class Class
	{

		static Class()
		{
			TypeResolver = DefaultTypeResolver.Instance;
		}

		public static ITypeResolver TypeResolver { get; set; }

		public static Class Of<T>()
		{
			return Of(typeof(T));
		}

		public static Class Of(object obj)
		{
			return Of(TypeResolver.Resolve(obj));
		}

		public static Class Of<TEntity>(TEntity obj)
			where TEntity: class, IEntity
		{
			return Of(TypeResolver.Resolve(obj));
		}


		public static Class Of(string id)
		{
			_classes.TryGetValue(id, out var clazz);

			return clazz;
		}



		public static Class Of(Type type)
		{
			var id = GetId(type);

			var clazz = Of(id);

			if (clazz != null) 
				return clazz;


			lock (_lock)
			{
				clazz = Of(id);

				if (clazz != null) 
					return clazz;


				clazz = new Class(type);

				_classes = new Dictionary<string, Class>(_classes)
				{
					{ id, clazz }
				};

				clazz.Initialize();
			}


			return clazz;

		}



		private Class(Type type)
		{
			Type = type;
		}


		public Type Type { get; private set; }

		public string Id => GetId(Type);

		public string Caption => Type.GetCaption();

		public string NewCaption => Type.GetCaption(CaptionNewResourceName);

		public string ListCaption => Type.GetCaption(CaptionListResourceName);

		public Property IdentifierProperty { get; set; }

		public Property VersionProperty { get; set; }

		public Property EntityNameProperty
		{
			get => _entityNameProperty;
			internal set
			{
				if (_entityNameProperty != null)
				{
					var oldType = _entityNameProperty.DeclaringType;
					var newType = value.DeclaringType;

					if (oldType == newType)
						throw new MetamodelException(
							"Class " + Type.Name + ". " +
							"The only one instance of ReferenceAttribute is allowed per class properties " +
							"(properties '" + _entityNameProperty.Name + "' and '" + value.Name + "')"
						);

					if (oldType.IsSubclassOf(newType))
						return;
				}

				_entityNameProperty = value;
			}
		}

		public IList<Property> Properties => _properties;

		public IList<Operation> Operations => _operations;

		public IList<Association> Associations => _associations;

		public bool IsPersistent { get; set; }

		public override string ToString()
		{
			return Type.ToString();
		}

		public bool Is<T>()
		{
			return Type.Is<T>();
		}

		public Property GetProperty(string name)
		{
			var property = _properties.Find(prop => name == prop.Name);

			if (property == null)
				Throw("There is no such property '{0}'", name);

			return property;
		}

		public Property TryGetProperty(string name)
		{
			return _properties.Find(prop => name == prop.Name);
		}

		public T CreateInstance<T>()
		{
			return (T)Activator.CreateInstance(Type);
		}

		private static string GetId(Type type)
		{
			return type.IsGenericType ? type.FullName : type.Name;
		}

		private void Throw(string format, params object[] args)
		{
			throw new MetamodelException($"{Type}: {string.Format(format, args)}");
		}

		private void Initialize()
		{
			InitializeProperties();

			InitializeOperations();

			InitializeAssociations();
		}

		private void InitializeProperties()
		{
			var properties = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var propertyInfo in properties)
				_properties.Add(new Property(this, propertyInfo));
		}

		private static void InitializeOperations()
		{
		}

		private static void InitializeAssociations()
		{
		}

		public Property AddProperty<TEntity>(string name)
		{
			var propertyInfo = typeof(TEntity).GetProperty(name);
			if (propertyInfo == null)
				throw new ArgumentException("Class.AddProperty: the property '" + name + "' not exist in type '" + typeof(TEntity).Name + "'.");
			var prop = new Property(this, propertyInfo);
			_properties.Add(prop);
			return prop;
		}

		private const string CaptionNewResourceName = "Caption_New";
		private const string CaptionListResourceName = "Caption_List";

		private static readonly object _lock = new object();
		private static Dictionary<string, Class> _classes = new Dictionary<string, Class>();

		private readonly List<Property> _properties = new List<Property>();
		private readonly List<Operation> _operations = new List<Operation>();
		private readonly List<Association> _associations = new List<Association>();
		private Property _entityNameProperty;
	}

}