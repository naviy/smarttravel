using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Luxena.Base.Serialization;



namespace Luxena.Base.Metamodel
{


	[DebuggerDisplay("{Name} - {Caption}")]
	public sealed class Property
	{

		public Property(Class owner, PropertyInfo propertyInfo)
		{
			if (owner == null)
				throw new ArgumentNullException(nameof(owner));

			if (propertyInfo == null)
				throw new ArgumentNullException(nameof(propertyInfo));

			Owner = owner;
			Name = propertyInfo.Name;

			_propertyInfo = propertyInfo;

			if (propertyInfo.DeclaringType != propertyInfo.ReflectedType && propertyInfo.DeclaringType != null)
				_propertyInfo = propertyInfo.DeclaringType.GetProperty(Name);


			SetType(_propertyInfo.PropertyType);

			DisplayFormat = DisplayFormatAttribute.Get(_propertyInfo);

			EditFormat = EditFormatAttribute.Get(_propertyInfo);

			IsReadOnly = !_propertyInfo.CanWrite || _propertyInfo.Has<ReadOnlyAttribute>();

			IsSerializationIgnore = _propertyInfo.Has<IgnoreSerializationAttribute>();

			HiddenOptions = HiddenAttribute.Get(_propertyInfo);

			if (_propertyInfo.Has<RequiredAttribute>())
				IsRequired = true;

			IsEntityName = _propertyInfo.Has<EntityNameAttribute>();
			IsEntityDate = _propertyInfo.Has<EntityDateAttribute>();

			if (IsEntityName)
				Owner.EntityNameProperty = this;
			else if (IsEntityDate && Owner.EntityNameProperty == null)
				Owner.EntityNameProperty = this;

			HasDefaultValue = _propertyInfo.Has<DefaultValueAttribute>();
			if (HasDefaultValue)
				DefaultValue = _propertyInfo.GetAttribute<DefaultValueAttribute>().Value;

			DataPath = DataPathAttribute.Get(_propertyInfo);
		}


		public Class Owner { get; }

		public string Name { get; }

		public string Caption => Owner.Type.GetCaption(Name);

		public Type Type { get; private set; }

		public object DefaultValue { get; }

		public bool HasDefaultValue { get; }

		public bool IsNullable { get; private set; }

		public bool IsString { get; private set; }

		public bool IsBool { get; private set; }

		public bool IsNumber { get; private set; }

		public bool IsDateTime { get; private set; }

		public bool IsEnum { get; private set; }

		public bool IsCollection { get; private set; }

		public Class Class { get; private set; }

		public bool IsTypePersistent => Class.IsPersistent;

		public int Length { get; set; }

		public string DisplayFormat { get; }

		public string EditFormat { get; }

		public bool IsRequired { get; set; }

		public bool IsReadOnly { get; }

		public HiddenOptions HiddenOptions { get; }

		public bool IsEntityName { get; set; }

		public bool IsEntityDate { get; set; }

		public string DataPath { get; set; }

		public bool IsPersistent => DataPath != null;

		public bool IsComposite { get; set; }

		public bool IsSerializationIgnore { get; set; }


		public void SetType(Type type)
		{
			Type = type;

			IsNullable = Type.IsNullable();
			if (IsNullable)
				Type = Type.GetGenericArguments()[0];

			IsString = false;
			IsBool = false;
			IsNumber = false;
			IsDateTime = false;
			IsEnum = false;
			IsCollection = false;

			IsComposite = false;

			Class = Type.GetClass();

			if (Type == typeof(string) || Type == typeof(char))
				IsString = true;
			else if (Type == typeof(bool))
				IsBool = true;
			else if (Type == typeof(int) || Type == typeof(long) || Type == typeof(double) || Type == typeof(decimal))
				IsNumber = true;
			else if (Type == typeof(DateTime))
				IsDateTime = true;
			else if (Type.IsEnum)
				IsEnum = true;
			else
			{
				if (Type.IsGenericType && Type.IsInterface)
				{
					var genericTypeDefinition = Type.GetGenericTypeDefinition();

					IsCollection = genericTypeDefinition == typeof(ICollection<>)
						|| genericTypeDefinition == typeof(IList<>)
						|| genericTypeDefinition == typeof(IDictionary<,>);
				}
				else
				{
					IsCollection = Type.GetInterfaces().Any(interfaceType => interfaceType == typeof(ICollection));
				}
			}
		}

		public object GetValue(object obj)
		{
			return GetPropertyValue(obj);
		}

		public T GetValue<T>(object obj)
		{
			return (T)GetPropertyValue(obj);
		}

		public void SetValue(object obj, object value)
		{
			SetPropertyValue(obj, value);
		}

		public string GetString(object obj)
		{
			return string.Format("{0:" + DisplayFormat + "}", GetPropertyValue(obj));
		}

		private object GetPropertyValue(object obj)
		{
			if (obj == null || _propertyInfo.DeclaringType == null || !_propertyInfo.DeclaringType.IsInstanceOfType(obj))
				return null;


			try
			{
				return _propertyInfo.GetValue(obj, null);
			}
			catch (Exception ex)
			{
				throw new Exception(
					"GetPropertyValue('" + obj + "', '" + Owner.Type.Name + "." + _propertyInfo.Name + "'): " +
					ex.InnerException.Message,
					ex.InnerException
				);
			}
		}

		private void SetPropertyValue(object obj, object value)
		{
			if (value != null)
			{
				var type = _propertyInfo.PropertyType;

				var converter = TypeDescriptor.GetConverter(type);

				if (converter.CanConvertFrom(value.GetType()))
					value = converter.ConvertFrom(value);
				else if (type.IsValueType && type != value.GetType() && !type.IsEnum)
					value = ChangeType(value, type);
			}

			_propertyInfo.SetValue(obj, value, null);
		}

		private static object ChangeType(object value, Type conversionType)
		{

			if (conversionType == null)
			{
				throw new ArgumentNullException(nameof(conversionType));
			}

			if (conversionType.IsGenericType &&
			  conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				if (value == null)
				{
					return null;
				}
				var nullableConverter = new NullableConverter(conversionType);

				conversionType = nullableConverter.UnderlyingType;
			}

			if (conversionType.IsEnum)
				return Enum.ToObject(conversionType, value);

			return Convert.ChangeType(value, conversionType);
		}

		public PropertyInfo PropertyInfo => _propertyInfo;

		public Type DeclaringType => _propertyInfo.DeclaringType;


		private readonly PropertyInfo _propertyInfo;

	}



}