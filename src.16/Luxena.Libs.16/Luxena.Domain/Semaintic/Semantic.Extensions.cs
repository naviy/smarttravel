using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Luxena.Domain
{

	public static class SemanticExtensions
	{

//		public static Type[] DefaultLocalizationTypes = new[]
//		{
//			typeof(RUAttribute), 
//			typeof(UAAttribute), 
//			typeof(ENAttribute)
//		};

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


		public static IEnumerable<Attribute> Semantics(
			this IList<Attribute> attrs, 
			Func<Attribute, bool> match, 
			bool usePropertyPattern = true
		)
		{
			foreach (var attr in attrs.Where(match))
			{
				yield return attr;
			}

			foreach (var attr in attrs.Where(attr => !match(attr) && !(attr is MemberPatternAttribute)))
			{
				foreach (var attr2 in attr.GetType().Semantics(match, usePropertyPattern: false))
				{
					yield return attr2;
				}
			}

			foreach (var attr in attrs.Where(attr => !match(attr)))
			{
				foreach (var attr2 in attr.GetType().Semantics(match))
				{
					yield return attr2;
				}
			}

			if (usePropertyPattern)
			{
				foreach (var pattern in attrs.OfType<MemberPatternAttribute>())
				{
					foreach (var attr2 in pattern.Pattern.Semantics(match))
					{
						yield return attr2;
					}
				}
			}
		}


		public static IEnumerable<Attribute> Semantics(this IList<Attribute> attrs, Type attributeType, bool usePropertyPattern = true)
		{
			return attrs.Semantics(attributeType.IsInstanceOfType, usePropertyPattern: usePropertyPattern);
		}


		public static IEnumerable<Attribute> Semantics(this MemberInfo member, Func<Attribute, bool> match, bool usePropertyPattern = true)
		{
			if (member == null) return null;

			if (_skipMemberTypes.Contains(member))
				return new Attribute[0];

			var t = cashMemberSemantics.By(member);
			if (t == null)
			{
				var attributes = member.Attributes<Attribute>().ToArray();

				t = Tuple.Create(
					attributes.Semantics(a => true, false).ToArray(),
					attributes.Semantics(a => true).ToArray()
				);

				cashMemberSemantics[member] = t;
			}

			var attrs = usePropertyPattern ? t.Item2 : t.Item1;

			return attrs.Where(match);
		}

		static readonly Dictionary<MemberInfo, Tuple<Attribute[], Attribute[]>> cashMemberSemantics = 
			new Dictionary<MemberInfo, Tuple<Attribute[], Attribute[]>>();


		public static IEnumerable<Attribute> Semantics(this MemberInfo member, Type attributeType, bool usePropertyPattern = true)
		{
			return member?.Semantics(attributeType.IsInstanceOfType, usePropertyPattern);
		}

		public static IEnumerable<T> Semantics<T>(this MemberInfo member, bool usePropertyPattern = true)
			where T : Attribute
		{
			return member?.Semantics(typeof(T).IsInstanceOfType, usePropertyPattern).Cast<T>();
		}

		public static Attribute Semantic(this MemberInfo member, Type attributeType, bool usePropertyPattern = true)
		{
			return member?.Semantics(attributeType.IsInstanceOfType, usePropertyPattern).One();
		}

		public static T Semantic<T>(this MemberInfo member, bool usePropertyPattern = true)
			where T : Attribute
		{
			return (T)member?.Semantics(typeof(T).IsInstanceOfType, usePropertyPattern).One();
		}
		
	}

}
