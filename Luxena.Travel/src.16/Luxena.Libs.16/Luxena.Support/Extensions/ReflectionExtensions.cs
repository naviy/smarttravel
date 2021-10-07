using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Luxena
{

	public static class ReflectionExtentions
	{

		public static string AsName(this MemberInfo member) => member?.Name;

		public static string AsName(this PropertyInfo member) => member?.Name;

		public static string AsName(this FieldInfo member) => member?.Name;

		public static string AsName(this Type member) => member?.Name;


		public static T Attribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute
		{
			return member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault() as T;
		}


		public static IEnumerable<T> Attributes<T>(this MemberInfo member, bool inherit = true) where T : Attribute
		{
			return member.GetCustomAttributes(typeof(T), inherit).Cast<T>();
		}


		public static bool HasAttribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute
		{
			return System.Attribute.IsDefined(member, typeof(T), inherit);
		}


		public static PropertyInfo PropertyWithAttribute<TAttribute>(this Type type)
			where TAttribute : Attribute
		{
			return type.GetProperties().By(a => a.HasAttribute<TAttribute>());
		}


		public static bool IsRawGeneric(this Type type, Type genericBaseClass)
		{
			return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseClass;
		}

		public static bool IsSubclassOfRawGeneric(this Type type, Type genericBaseClass)
		{
			while (type != null && type != typeof(object))
			{
				var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

				if (cur == genericBaseClass)
					return true;

				type = type.BaseType;
			}

			return false;
		}

		public static IEnumerable<Type> Subclasses(this Type type, Func<Type, bool> match = null)
		{
			var types = type.Assembly.GetTypes();
			return match != null
				? types.Where(a => a.IsSubclassOf(type) && match(a))
				: types.Where(a => a.IsSubclassOf(type));
		}

		public static MemberInfo[] PropertiesAndFields(this Type type)
		{
			var p = (IEnumerable<MemberInfo>)type.GetProperties();
			var f = (IEnumerable<MemberInfo>)type.GetFields();
			return p.Concat(f).ToArray();
		}

		public static MemberInfo PropertyOrField(this Type type, string name)
		{
			return (MemberInfo)type.GetProperty(name) ?? type.GetField(name);
		}


		/// <summary>
		/// Возвращает тип значения для свойства указанного типа type по имени name
		/// </summary>
		public static Type ResultType(this Type type, string name)
		{
			var prop = type.GetProperty(name);
			if (prop != null)
				return prop.PropertyType;

			var field = type.GetField(name);
			if (field != null)
				return field.FieldType;

			throw new ArgumentException("Can't find property or field '" + name + "' in type " + type.Name, "name");
		}


		/// <summary>
		/// Возвращает тип значения для указанного member
		/// </summary>
		public static Type ResultType(this MemberInfo member)
		{
			var type = member as Type;
			if (type != null)
				return type;

			var prop = member as PropertyInfo;
			if (prop != null)
				return prop.PropertyType;

			var field = member as FieldInfo;
			if (field != null)
				return field.FieldType;

			var method = member as MethodInfo;
			if (method != null)
				return method.ReturnType;

			return null;
		}


		public static bool IsBool(this Type me)
		{
			return
				me == typeof(bool) || me == typeof(bool?);
		}

		public static bool IsDateTime(this Type me)
		{
			return
				me == typeof(DateTime) || me == typeof(DateTime?) ||
				me == typeof(DateTimeOffset) || me == typeof(DateTimeOffset?);
		}

		public static bool IsDateTimeOffset(this Type me)
		{
			return
				me == typeof(DateTimeOffset) || me == typeof(DateTimeOffset?);
		}

		public static bool IsEnumerable(this Type me)
		{
			return me != typeof(string) && me.GetInterfaces().Contains(typeof(IEnumerable));
		}

		public static bool IsFloat(this Type me)
		{
			return
				me == typeof(float) || me == typeof(float?) ||
				me == typeof(double) || me == typeof(double?) ||
				me == typeof(decimal) || me == typeof(decimal?);
		}

		public static bool IsInt(this Type me)
		{
			return
				me == typeof(int) || me == typeof(int?) ||
				me == typeof(long) || me == typeof(long?);
		}

		public static Type AsEnumType(this Type type)
		{
			if (type.IsEnum) return type;

			if (!type.IsNullable()) return null;
			
			return type.GetGenericArguments().One().If(a => a.IsEnum);
		}


		/// <summary>
		/// Тип является nullable (например, int?)
		/// </summary>
		public static bool IsNullable(this Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
		}


		/// <summary>
		/// Возвращает значение свойства name для объекта obj
		/// </summary>
		public static object GetValue(this Type type, object obj, string name, object[] index = null)
		{
			var prop = type.GetProperty(name);
			if (prop != null) return prop.GetValue(obj, index);

			var field = type.GetField(name);
			if (field != null) return field.GetValue(obj);

			throw new ArgumentException("Can't find field or property '" + name + "' in type " + type.Name, "name");
		}


		/// <summary>
		/// Возвращает значение свойства name для объекта obj
		/// </summary>
		public static object GetValue(this MemberInfo member, object obj, object[] index = null)
		{
			var propInfo = member as PropertyInfo;
			if (propInfo != null)
				return propInfo.GetValue(obj, index);

			var fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
				return fieldInfo.GetValue(obj);

			throw new ArgumentException(
				"Can't return member value of '" + member.Name + "'" +
				(member.DeclaringType != null ? " in type " + member.DeclaringType.Name : null)
			);
		}


		/// <summary>
		/// Задаёт значение свойства name для объекта obj
		/// </summary>
		public static void SetValue(this MemberInfo member, object obj, object value, object[] index = null)
		{
			var prop = member as PropertyInfo;
			if (prop != null)
			{
				if (!prop.CanWrite) return;
				prop.SetValue(obj, value, index);
				return;
			}

			var field = member as FieldInfo;
			if (field != null)
			{
				field.SetValue(obj, value);
				return;
			}

			throw new ArgumentException(
				"Can't set member value of '" + member.Name + "'" + member.DeclaringType.As(a => " in type " + a.Name)
			);
		}


		public static string ToCSharp(this Type type)
		{
			var typeName = 
				type.IsEnum ? type.Name :
				type == typeof(bool) ? "bool" :
				type == typeof(bool?) ? "bool?" :
				type == typeof(decimal) ? "decimal" :
				type == typeof(decimal?) ? "decimal?" :
				type == typeof(double) ? "double" :
				type == typeof(double?) ? "double?" :
				type == typeof(int) ? "int" :
				type == typeof(int?) ? "int?" :
				type == typeof(string) ? "string" :
				type.IsNullable() ? type.GetGenericArguments()[0].ToCSharp() + "?" :
				type.Name;

			return typeName;
		}
	}

}