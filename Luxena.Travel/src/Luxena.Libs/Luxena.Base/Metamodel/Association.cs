using System;
using System.Reflection;


namespace Luxena.Base.Metamodel
{
	public sealed class Association
	{
		public Association(Class owner)
		{
			if (owner == null)
				throw new ArgumentNullException();

			Owner = owner;
			Multiplicity = Multiplicity.Unspecified;
		}

		public Class Owner { get; private set; }

		public string Name { get; set; }

		public string Caption
		{
			get { return Owner.Type.GetCaption(Name); }
		}

		public bool IsNavigable
		{
			get { return !string.IsNullOrEmpty(Name); }
		}

		public string NameInAssociationClass
		{
			get
			{
				if (string.IsNullOrEmpty(_nameInAssociationClass))
					return Owner.Type.Name;

				return _nameInAssociationClass;
			}
			set { _nameInAssociationClass = value; }
		}

		public AggregationKind Aggregation { get; set; }

		public Multiplicity Multiplicity { get; set; }

		public Association Opposite { get; private set; }

		public Class AssociationClass { get; private set; }

		public string FactoryOperation { get; set; }

		public string RemoveOperation { get; set; }

		public void SetOpposite(Association value)
		{
			if (IsNavigable)
			{
				_property = value.Owner.Type.GetProperty(Name);

				if (_property == null)
					throw new ArgumentException(string.Format("Invalid role name '{0}'", Name));
			}

			if (!string.IsNullOrEmpty(FactoryOperation))
			{
				MethodInfo methodInfo = value.Owner.Type.GetMethod(FactoryOperation);

				if (methodInfo == null)
					throw new ArgumentException(string.Format(
						"Cannot find factory operation '{0}' in '{1}'", FactoryOperation, Owner.Type));

				_factoryMethod = methodInfo;
			}

			if (!string.IsNullOrEmpty(RemoveOperation))
			{
				MethodInfo methodInfo = value.Owner.Type.GetMethod(RemoveOperation);

				if (methodInfo == null)
					throw new ArgumentException(string.Format(
						"Cannot find remove operation '{0}' in '{1}'", RemoveOperation, Owner.Type));

				_removeMethod = methodInfo;
			}

			Opposite = value;
		}

		public void SetAssociationClass(Class value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			_propertyInAssociationClass = value.Type.GetProperty(NameInAssociationClass);

			if (_propertyInAssociationClass == null)
				throw new ArgumentException(string.Format("Invalid role name in association class '{0}'",
					_nameInAssociationClass));

			AssociationClass = value;
		}

		public T GetValue<T>(object instance)
		{
			return (T) _property.GetValue(instance, null);
		}

		public T CreateAssociated<T>(object instance)
		{
			if (AssociationClass != null)
				throw new MetamodelException("This method is not allowed for associations with an association class");

			T result;

			if (_factoryMethod != null)
				result = (T) _factoryMethod.Invoke(instance, null);
			else
			{
				result = Owner.CreateInstance<T>();

				if (Opposite.IsNavigable)
					Opposite._property.SetValue(result, instance, null);
			}

			return result;
		}

		public void Associate(object instance, object target)
		{
			if (AssociationClass != null)
				throw new MetamodelException("This method is not allowed for associations with an association class");

			if (Opposite.IsNavigable)
				Opposite._property.SetValue(target, instance, null);
		}

		public void Unassociate(object instance, object target)
		{
			if (AssociationClass != null)
				throw new MetamodelException("This method is not allowed for associations with an association class");

			if (Opposite.IsNavigable)
				Opposite._property.SetValue(target, null, null);

			if (_removeMethod != null)
				_removeMethod.Invoke(instance, new[]
				{
					target
				});
			else if (IsNavigable)
			{
				object value = _property.GetValue(instance, null);

				MethodInfo removeMethod = value.GetClass().Type.GetMethod("Remove");

				if (removeMethod != null)
					removeMethod.Invoke(value, new[]
					{
						target
					});
			}
		}

		public T CreateAssociationClass<T>(object instance, object target)
		{
			if (AssociationClass == null)
				throw new MetamodelException("This method is not allowed for associations without an association class");

			var link = AssociationClass.CreateInstance<T>();
			Opposite._propertyInAssociationClass.SetValue(link, instance, null);
			_propertyInAssociationClass.SetValue(link, target, null);

			return link;
		}

		private string _nameInAssociationClass;
		private PropertyInfo _property;
		private PropertyInfo _propertyInAssociationClass;
		private MethodInfo _factoryMethod;
		private MethodInfo _removeMethod;
	}
}