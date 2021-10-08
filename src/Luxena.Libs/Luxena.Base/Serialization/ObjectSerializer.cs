using System.Collections.Generic;

using Luxena.Base.Metamodel;


namespace Luxena.Base.Serialization
{
	public class ObjectSerializer
	{
		public ObjectSerializer()
		{
			IncludeIdentifier = true;
			IncludeVersion = true;
			IncludeType = true;
			IncludeReferenceString = true;
		}

		public bool IncludeIdentifier { get; set; }
		public bool IncludeVersion { get; set; }
		public bool IncludeType { get; set; }
		public bool IncludeReferenceString { get; set; }
		public IList<Property> Properties { get; set; }

		public Dictionary<string, object> Serialize(object obj)
		{
			var result = new Dictionary<string, object>();

			var clazz = obj.GetClass();

			if (IncludeIdentifier)
				result.Add(IdResponseFieldName, clazz.IdentifierProperty.GetValue(obj));

			if (IncludeVersion)
				result.Add(VersionResponseFieldName, clazz.VersionProperty.GetValue(obj));

			if (IncludeType)
				result.Add(ClassResponseFieldName, clazz.Id);

			if (IncludeReferenceString)
				result.Add(ReferenceResponseFieldName, GetReferencePropertyValue(obj));

			var properties = Properties ?? GetAvailableProperties(clazz);

			foreach (var property in properties)
				result.Add(property.Name, GetPropertyValue(property, obj));

			return result;
		}

		private static IList<Property> GetAvailableProperties(Class clazz)
		{
			var properties = new List<Property>();

			foreach (var property in clazz.Properties)
			{
				if ((property.IsString || property.IsTypePersistent || property.IsNumber || property.IsEnum || property.IsDateTime || property.IsBool)
					&& !property.IsCollection && property.Name != clazz.IdentifierProperty.Name && property.Name != clazz.VersionProperty.Name && !property.IsSerializationIgnore)
				{
					properties.Add(property);
				}
			}

			return properties;
		}

		private static object GetPropertyValue(Property property, object obj)
		{
			var value = property.GetValue(obj);

			if (property.IsTypePersistent && value != null)
			{
				var propertyClass = property.Type.GetClass();

				var id = propertyClass.IdentifierProperty.GetValue(value);
				var reference = propertyClass.EntityNameProperty.GetValue(value);

				return new[] { id, reference, propertyClass.Id };
			}

			return value;
		}

		private static object GetReferencePropertyValue(object obj)
		{
			var clazz = obj.GetClass();

			var property = clazz.EntityNameProperty;

			if (property == null)
				return clazz.Caption;

			var value = property.GetValue(obj);

			if (property.IsTypePersistent && value != null)
			{
				var propertyClass = property.Class;
				return propertyClass.EntityNameProperty.GetValue(value);
			}

			return value;
		}

		public const string IdResponseFieldName = "Id";
		public const string VersionResponseFieldName = "Version";
		public const string ClassResponseFieldName = "__class";
		public const string ReferenceResponseFieldName = "__reference";
	}
}