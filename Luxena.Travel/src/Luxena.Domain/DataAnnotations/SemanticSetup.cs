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
	}


	public class SemanticSetup
	{

		public class PropertyBag
		{
			public PropertyInfo[] Properties;

			public readonly List<Attribute> Attributes = new List<Attribute>();


			public Attribute Semantic(Type attributeType)
			{
				return Attributes.Semantics(attributeType).One();
			}

			public T Semantic<T>() where T: Attribute
			{
				return (T)Attributes.Semantics(typeof(T)).One();
			}

			public LocalizationAttribute Localization(IDefaultLocalizationTypesSource lng)
			{
				return Attributes.Localization(lng);
			}


			public PropertyBag Add(Attribute attr)
			{
				Attributes.Add(attr);
				return this;
			}


//			public PropertyTagsInfo Format(string jsFormat)
//			{
//				return Add(new FormatAttribute(jsFormat));
//			}

			public PropertyBag StringLength(int value)
			{
				return Add(new StringLengthAttribute(value));
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

			public PropertyBag Suggest(Type referenceType)
			{
				return Add(new SuggestAttribute(referenceType));
			}
			public PropertyBag Suggest<TReference>()
			{
				return Add(new SuggestAttribute(typeof(TReference)));
			}
		}

		public readonly List<PropertyBag> Properties = new List<PropertyBag>();

		public PropertyBag Add(PropertyInfo[] properties)
		{
			var tags = new PropertyBag { Properties = properties };
			Properties.Add(tags);
			return tags;
		}

	}


	public class SemanticSetup<TEntity> : SemanticSetup
	{
		public PropertyBag For<T>(Expression<Func<TEntity, T>> properties)
		{
			return Add(properties.GetProperties().ToArray());
		}
	}

}
