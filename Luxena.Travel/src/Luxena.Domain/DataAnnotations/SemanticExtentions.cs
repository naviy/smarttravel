using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Luxena.Domain
{

	public static class SemanticExtentions
	{

		public static Type[] DefaultLocalizationTypes = new[]
		{
			typeof(RUAttribute), 
			typeof(UAAttribute), 
			typeof(ENAttribute)
		};

		public static readonly Type[] _skipMemberTypes =
		{
			typeof(object), 
			typeof(int),
			typeof(string), 
			typeof(decimal), 
			typeof(float), 
			typeof(AttributeUsageAttribute), 
			typeof(SerializableAttribute),
			typeof(System.Runtime.InteropServices.ComVisibleAttribute),
		};


		public static IEnumerable<Attribute> Semantics(this IList<Attribute> attrs, Func<Attribute, bool> match)
		{
			foreach (var attr in attrs.Where(match))
			{
				yield return attr;
			}

			foreach (var attr in attrs.Where(attr => !match(attr)))
			{
				foreach (var attr2 in attr.GetType().Semantics(match))
				{
					yield return attr2;
				}
			}
		}


		public static IEnumerable<Attribute> Semantics(this IList<Attribute> attrs, Type attributeType)
		{
			return attrs.Semantics(attributeType.IsInstanceOfType);
		}


		public static IEnumerable<Attribute> Semantics(this MemberInfo member, Func<Attribute, bool> match)
		{
			if (_skipMemberTypes.Contains(member))
				return new Attribute[0];

			return member.Attributes<Attribute>().ToArray().Semantics(match);
		}


		public static IEnumerable<Attribute> Semantics(this MemberInfo member, Type attributeType)
		{
			if (_skipMemberTypes.Contains(member))
				return new Attribute[0];

			return member.Attributes<Attribute>().ToArray().Semantics(attributeType);
		}

		public static IEnumerable<T> Semantics<T>(this MemberInfo member)
			where T : Attribute
		{
			return member.Semantics(typeof(T)).Cast<T>();
		}

		public static Attribute Semantic(this MemberInfo member, Type attributeType)
		{
			return member.Semantics(attributeType).One();
		}

		public static T Semantic<T>(this MemberInfo member)
			where T : Attribute
		{
			return member.Semantics<T>().One();
		}
		
	}

}
