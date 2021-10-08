using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Threading;


namespace Luxena
{
	public static class TypeExtensions
	{
		public static bool Is<T>(this Type type)
		{
			return type.Is(typeof(T));
		}

		public static bool Is(this Type type, Type target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			return type == target || type.IsSubclassOf(target) ||
				(target.IsInterface && type.GetInterface(target.Name) != null);
		}

		public static bool IsNullable(this Type t)
		{
			if (t == null)
				throw new ArgumentNullException("t");

			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static bool Has<T>(this MemberInfo memberInfo)
		{
			return Attribute.IsDefined(memberInfo, typeof(T));
		}

		public static T GetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			return (T)Attribute.GetCustomAttribute(memberInfo, typeof(T));
		}

		public static string GetCaption(this Type type)
		{
			var key = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName + '-' + type.FullName;

			string result;

			_captionsLock.AcquireReaderLock(Timeout.Infinite);
			try
			{
				if (_captions.TryGetValue(key, out result))
					return result;
			}
			finally
			{
				_captionsLock.ReleaseReaderLock();
			}

			var manager = FindResourceManager(type);

			result = manager == null ? type.Name : manager.GetString(type.Name);

			_captionsLock.AcquireWriterLock(Timeout.Infinite);
			try
			{
				_captions[key] = result;
			}
			finally
			{
				_captionsLock.ReleaseWriterLock();
			}

			return result;
		}

		public static string GetCaption(this Type type, string memberName)
		{
			var key = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName + '-' + type.FullName + '-' + memberName;

			string result;

			_captionsLock.AcquireReaderLock(Timeout.Infinite);
			try
			{
				if (_captions.TryGetValue(key, out result) && result != null)
					return result;
			}
			finally
			{
				_captionsLock.ReleaseReaderLock();
			}

			result = LoadCaption(type, memberName);

			_captionsLock.AcquireWriterLock(Timeout.Infinite);
			try
			{
				_captions[key] = result;
			}
			finally
			{
				_captionsLock.ReleaseWriterLock();
			}

			return result;
		}


		public static string Translate(this object me)
		{
			if (me == null) return null;

			var type = me.GetType();

			return type.IsEnum ? GetCaption(type, me.ToString()) : GetCaption(type);
		}

		private static string LoadCaption(this Type type, string memberName)
		{
			string result = null;

			var curr = type;

			while (curr.BaseType != null)
			{
				var manager = FindResourceManager(curr); 

				if (manager != null)
				{
					var caption = manager.GetString(string.Format("{0}_{1}", curr.Name, memberName));

					if (caption != null)
						return caption;

					if (result == null)
						result = manager.GetString(string.Format("Common_{0}", memberName));
				}

				curr = curr.BaseType;
			}

			if (result != null) 
				return result;
			
			var propertyInfo = type.GetProperty(memberName);

			if (propertyInfo != null && propertyInfo.PropertyType.Name == memberName)
				result = propertyInfo.PropertyType.GetCaption();

			return result;
		}

		private static ResourceManager FindResourceManager(Type type)
		{
			if (type == null)
				throw new ArgumentNullException();

			ResourceManager resourceManager;

			_managersLock.AcquireReaderLock(Timeout.Infinite);
			try
			{
				if (_resourceManagers.TryGetValue(type, out resourceManager))
					return resourceManager;
			}
			finally
			{
				_managersLock.ReleaseReaderLock();
			}

			_managersLock.AcquireWriterLock(Timeout.Infinite);
			try
			{
				if (_resourceManagers.TryGetValue(type, out resourceManager))
					return resourceManager;

				Type resourceSource;

				var typeResourcesAttribute = type.GetAttribute<TypeResourcesAttribute>();

				if (typeResourcesAttribute != null)
				{
					resourceSource = typeResourcesAttribute.Type;
				}
				else
				{
					object[] attributes = type.Assembly.GetCustomAttributes(typeof(AssemblyResourcesAttribute), false);

					AssemblyResourcesAttribute assemblyAttribute = null;

					foreach (AssemblyResourcesAttribute attribute in attributes)
					{
						if (type.Namespace == null || !type.Namespace.StartsWith(attribute.Namespace)) continue;
						
						if (assemblyAttribute == null)
						{
							assemblyAttribute = attribute;
						}
						else if (attribute.Namespace.Length == assemblyAttribute.Namespace.Length)
						{
							throw new Exception(string.Format("There are duplicate resource registrations ('{0}' and '{1}') for namespace '{2}' in assembly '{3}'",
								assemblyAttribute.Type, attribute.Type, attribute.Namespace, type.Assembly));
						}
						else if (attribute.Namespace.Length > assemblyAttribute.Namespace.Length)
						{
							assemblyAttribute = attribute;
						}
					}

					resourceSource = assemblyAttribute == null ? null : assemblyAttribute.Type;
				}

				resourceManager = resourceSource == null ? null : new ResourceManager(resourceSource);

				_resourceManagers.Add(type, resourceManager);
			}
			finally
			{
				_managersLock.ReleaseWriterLock();
			}

			return resourceManager;
		}

		private static readonly ReaderWriterLock _captionsLock = new ReaderWriterLock();
		private static readonly ReaderWriterLock _managersLock = new ReaderWriterLock();
		private static readonly Dictionary<string, string> _captions = new Dictionary<string, string>();
		private static readonly Dictionary<Type, ResourceManager> _resourceManagers = new Dictionary<Type, ResourceManager>();
	}
}