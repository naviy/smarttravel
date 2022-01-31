using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Managers;
using Luxena.Base.Metamodel;


namespace Luxena.Travel.Domain
{

	public class ClassManagerProvider : IClassManagerProvider
	{
		public string TypeNamePattern { get; set; }

		public IServiceResolver ServiceResolver { get; set; }

		public static IList<Type> Managers => _managers;

		public GenericManager GetClassManager(Class cls)
		{
			Type managerType = null;
			var entityType = cls.Type;

			while (entityType != null)
			{
				var typeName = TypeNamePattern.Fill(entityType.Name);

				managerType = Managers.FirstOrDefault(type => type.Name == typeName);

				if (managerType != null)
					break;

				entityType = entityType.BaseType;
			}

			if (managerType != null)
				return (GenericManager)ServiceResolver.Resolve(managerType);

			return null;
		}

		private static readonly List<Type> _managers = new List<Type>();
		
	}

}