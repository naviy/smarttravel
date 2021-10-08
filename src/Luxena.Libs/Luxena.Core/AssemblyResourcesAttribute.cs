using System;


namespace Luxena
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class AssemblyResourcesAttribute : Attribute
	{
		public AssemblyResourcesAttribute(Type type)
		{
			_type = type;
			_namespace = string.Empty;
		}

		public AssemblyResourcesAttribute(Type type, string @namespace)
		{
			_type = type;
			_namespace = @namespace ?? string.Empty;
		}

		public Type Type
		{
			get { return _type; }
		}

		public string Namespace
		{
			get { return _namespace; }
		}

		private readonly Type _type;
		private readonly string _namespace;
	}
}