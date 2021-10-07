using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Luxena.Domain
{

	[AttributeUsage(AttributeTargets.Method)]
	public class SemanticSetupAttribute : Attribute
	{
		//[SemanticSetup]
		//public static void SemanticSetup(SemanticSetup<Organization> sm)
		//{
		//	sm.For(a => a.Code)
		//		.RU("Код предприятия");
		//}
	}


	public class SemanticSetup
	{

		public static SemanticSetup Invoke(Type entityType)
		{
			if (entityType == null) return null;

			var method = entityType
				.GetMethods(BindingFlags.Public | BindingFlags.Static)
				.By(a => a.HasAttribute<SemanticSetupAttribute>());
			if (method == null) return null;

			var tags = (SemanticSetup)Activator
				.CreateInstance(typeof(SemanticSetup<>)
				.MakeGenericType(entityType));

			method.Invoke(null, new[] { tags });

			return tags;
		}

		public static SemanticSetup Invoke<TEntity>()
		{
			return Invoke(typeof(TEntity));
		}

		public void Patterns<TSource, TDestination>(Expression<Func<TSource, TDestination>> properties)
		{
			foreach (var memberAssignExpr in ((MemberInitExpression)properties.Body).Bindings.OfType<MemberAssignment>())
			{
				var convertExpr = memberAssignExpr.Expression as UnaryExpression;
				var memberExpr = convertExpr != null ? convertExpr.Operand : memberAssignExpr.Expression;

				var srcProp = (PropertyInfo)((MemberExpression)memberExpr).Member;

				var destProp = (PropertyInfo)memberAssignExpr.Member;

				For(destProp).Pattern(srcProp);
			}
		}


		public class PropertyBag
		{
			public PropertyInfo Property;

			public readonly List<Attribute> Attributes = new List<Attribute>();


			public Attribute Semantic(Type attributeType, bool usePropertyPattern = true)
			{
				return Attributes.Semantics(attributeType, usePropertyPattern: usePropertyPattern).One();
			}

			public T Semantic<T>(bool usePropertyPattern = true) where T : Attribute
			{
				return (T)Attributes.Semantics(typeof(T), usePropertyPattern: usePropertyPattern).One();
			}

			public LocalizationAttribute Localization(DefaultLocalizationTypesSource lng)
			{
				return Property.Localization(Attributes, lng);
			}


			#region Semantic setters

			public PropertyBag Add(Attribute attr)
			{
				Attributes.Add(attr);
				return this;
			}


			public PropertyBag EN(string en = null, string ens = null, string enDesc = null, string enShort = null)
			{
				return Add(new ENAttribute(en: en, ens: ens, enDesc: enDesc, enShort: enShort));
			}

			public PropertyBag RU(string ru = null, string rus = null, string ru2 = null, string ru5 = null, string ruDesc = null, string ruShort = null)
			{
				return Add(new RUAttribute(ru: ru, rus: rus, ru2: ru2, ru5: ru5, ruDesc: ruDesc, ruShort: ruShort));
			}

			public PropertyBag UA(string ua = null, string uas = null, string ua2 = null, string ua5 = null, string uaDesc = null, string uaShort = null)
			{
				return Add(new UAAttribute(ua: ua, uas: uas, ua2: ua2, ua5: ua5, uaDesc: uaDesc, uaShort: uaShort));
			}

			public PropertyBag Enum<TEnum>(params TEnum[] enumList)
				where TEnum : struct
			{
				var enumType = typeof(TEnum);

				return Add(new CustomSemanticTypeAttribute
				{
					TypeScriptCode = ".enum(" + enumType.Name + string.Concat(enumList.AsSelect(a => ", \"" + a + "\"")) + ")",
				});
			}


			public PropertyBag EntityDate()
			{
				return Add(new EntityDateAttribute());
			}

			public PropertyBag EntityName()
			{
				return Add(new EntityNameAttribute());
			}

			public PropertyBag EntityPosition()
			{
				return Add(new EntityPositionAttribute());
			}

			public PropertyBag EntityType()
			{
				return Add(new EntityTypeAttribute());
			}


			public PropertyBag Localization(Type sourceMember)
			{
				return Add(new LocalizationAttribute(sourceMember));
			}

			public PropertyBag Localization<TSourceMember>()
			{
				return Add(new LocalizationAttribute(typeof(TSourceMember)));
			}

			public PropertyBag Length(int value, int min = 0, int max = 0)
			{
				return Add(new LengthAttribute(value, min, max));
			}

			public PropertyBag MaxLength(int value)
			{
				return Add(new MaxLengthAttribute(value));
			}

			public PropertyBag MinLength(int value)
			{
				return Add(new MinLengthAttribute(value));
			}

			public PropertyBag LineCount(int value)
			{
				return Add(new LineCountAttribute(value));
			}

			public PropertyBag Required(bool value = true)
			{
				return Add(new RequiredAttribute { AllowEmptyStrings = value });
			}

			public PropertyBag Lookup(Type referenceType)
			{
				return Add(new LookupAttribute(referenceType));
			}
			public PropertyBag Lookup<TReference>()
			{
				return Add(new LookupAttribute(typeof(TReference)));
			}

			public PropertyBag Pattern<TPatternAttribute>() where TPatternAttribute: Attribute, new()
			{
				return Add(new TPatternAttribute());
			}

			public PropertyBag Pattern<TSource, TValue>(Expression<Func<TSource, TValue>> func)
			{
				return Add(new MemberPatternAttribute(func.GetProperty()));
			}

			public PropertyBag Pattern(PropertyInfo prop)
			{
				return Add(new MemberPatternAttribute(prop));
			}

			#endregion


		}

		public readonly List<PropertyBag> Properties = new List<PropertyBag>();

		public PropertyBag PropertyByName(string name)
		{
			return Properties.FirstOrDefault(a => a.Property.Name == name);
		}


		public PropertyBag For(PropertyInfo property)
		{
			var bag = new PropertyBag { Property = property };
			Properties.Add(bag);
			return bag;
		}

		public PropertyBag For<TEntity>(Expression<Func<TEntity, object>> properties)
		{
			return For(properties.GetProperty());
		}

	}


	public class SemanticSetup<TEntity> : SemanticSetup
	{

		public PropertyBag For(Expression<Func<TEntity, object>> properties)
		{
			return base.For(properties);
		}

		//public new CompositePropertyBag<TEntity> FieldRow(string name)
		//{
		//	return base.FieldRow<TEntity>(name);
		//}

		//public new CompositePropertyBag<TEntity> FieldSet(string name)
		//{
		//	return base.FieldSet<TEntity>(name);
		//}

	}


}
