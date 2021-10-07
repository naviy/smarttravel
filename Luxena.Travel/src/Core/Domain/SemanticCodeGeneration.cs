using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public static class SemanticCodeGeneration
	{

		public static List<string> GetCodeForScriptSharp(PropertyInfo prop, IDefaultLocalizationTypesSource lng)
		{
			_getAnnotation = prop.Semantic;

			_getLocalization = () => prop.Localization(lng);

			return GetCodeForScriptSharp_(prop, lng);
		}

		public static List<string> GetCodeForScriptSharp(SemanticSetup.PropertyBag propBag, PropertyInfo prop, IDefaultLocalizationTypesSource lng)
		{
			_getAnnotation = propBag.Semantic;

			_getLocalization = () => propBag.Localization(lng);

			return GetCodeForScriptSharp_(prop, lng, appendType: false);
		}


//		private static readonly Regex reBigBigLetter = new Regex("[A-Z][A-Z]");

		private static List<string> GetCodeForScriptSharp_(PropertyInfo prop, IDefaultLocalizationTypesSource lng, bool appendType = true)
		{
			var tags = new List<string>();

//			if (reBigBigLetter.IsMatch(prop.Name))
//				tags.Add(".Name(\"" + prop.Name + "\")");

			var loc = _getLocalization();
			if (loc != null)
			{
				if (loc.Default.Yes())
					tags.Add(".Title(\"" + loc.Default.Replace("\"", "\\\"") + "\")");
				//				if (loc.Defaults.Yes())
				//					tags.Add(".Titles(\"" + loc.Defaults + "\")");
				if (loc.DefaultShort.Yes())
					tags.Add(".EmptyText(\"" + loc.DefaultShort.Replace("\"", "\\\"") + "\")");
			}

			AppendTypeTags(prop, tags, appendType, lng);

			if (prop.PropertyType.IsValueType && !ReflectionExtentions.IsNullable(prop.PropertyType))
				tags.Add(".Required()");
			else
				GetAnnotation<RequiredAttribute>().Do(a => tags.Add(".Required()"));

			if (prop.PropertyType != typeof(string))
				GetAnnotation<MaxLengthAttribute>().Do(a => tags.Add(".MaxLength(" + a.Length + ")"));

			GetAnnotation<MinLengthAttribute>().Do(a => tags.Add(".MinLength(" + a.Length + ")"));
			GetAnnotation<LineCountAttribute>().Do(a => tags.Add(".LineCount(" + a.Count + ")"));

			GetAnnotation<EntityDateAttribute>().Do(a => tags.Add(".EntityDate()"));
			GetAnnotation<EntityNameAttribute>().Do(a => tags.Add(".EntityName()"));
			GetAnnotation<SecondaryAttribute>().Do(a => tags.Add(".Secondary()"));
			GetAnnotation<UtilityAttribute>().Do(a => tags.Add(".Utility()"));

			GetAnnotation<DefaultValueAttribute>().Do(a =>
			{
				var value = a.Value;
				if (value == null)
					value = "null";
				else if (value is bool)
					value = value.ToString().ToLower();
				else if (value.GetType().IsEnum)
					value = (int)value;

				tags.Add(".DefaultValue(" + value + ")");
			});

			//			if (prop.HasAttribute<NotMappedAttribute>())
			//				tags.Add(".IsPersistent(false)");

			tags.RemoveAll(a => a.No());

			return tags;
		}

		private static void AppendTypeTags(PropertyInfo prop, List<string> tags, bool appendType, IDefaultLocalizationTypesSource lng)
		{
			var ptype = prop.PropertyType;
			var ptypeCode = GetAnnotation<SemanticTypeAttribute>().As(a => a.ToScriptSharp());

			if (ptypeCode != null)
			{
				tags.Add(ptypeCode);
				return;
			}

			var suggest = GetAnnotation<SuggestAttribute>();
			var isSmall = 
				GetAnnotation<SmallAttribute>() != null ||
				ptype.IsSubclassOf(typeof(Entity)) && ptype.Semantic<SmallAttribute>() != null ||
				suggest.As(a => a.ReferenceType.Semantic<SmallAttribute>() != null);

			if (!appendType)
			{
				if (suggest != null)
					tags.Add((isSmall ? ".EnumReference" : ".Reference") + "(\"" + suggest.ReferenceType.Name + "\")");

				return;
			}

			if (ptype.IsBool())
				ptypeCode = ".Bool()";
			else if (ptype.IsDateTime())
				ptypeCode = ".Date()";
			else if (ptype.IsFloat())
				ptypeCode = ".Float2()";
			else if (ptype.IsInt())
				ptypeCode = ".Int32()";
			else if (ptype == typeof(string))
				ptypeCode = ".String(" + GetAnnotation<MaxLengthAttribute>().As(a => (int?)a.Length) + ")";
			else if (ptype.IsSubclassOf(typeof(Entity)))
			{
				ptypeCode =
					(isSmall ? ".EnumReference" : ".Reference") +
					"(\"" + (suggest.As(a => a.ReferenceType.Name) ?? ptype.Name) + "\")";
//					"(delegate (ViewDomain dsm) { return dsm." + (suggest.As(a => a.ReferenceType.Name) ?? ptype.Name) + "; })";
			}
			else if (ptype == typeof(Money))
				ptypeCode = ".Money()";
			else if (ptype == typeof(object))
				ptypeCode = "";
			else if (ptype.IsEnumerable())
				ptypeCode = "";

			if (ptypeCode != null)
			{
				tags.Add(ptypeCode);
			}
			else
			{
				var enumType = ptype.AsEnumType();
				if (enumType != null)
				{
					tags.Add(enumType.HasAttribute<FlagsAttribute>() 
						? ".EnumsItems(new object[][]" 
						: ".EnumItems(new object[][]"
					);

					tags.Add("{");
					foreach (var enumValue in Enum.GetValues(enumType).Cast<object>().OrderBy(a => a.ToString()))
					{
						var name = enumType.GetMember(enumValue.ToString()).One().Localization(lng).Default ?? enumValue.ToString();
						tags.Add("\tnew object[] { " + (int)enumValue + ", \"" + name + "\" }, // " + enumValue);
					}
					tags.Add("})");
				}
				else
					tags.Add(".UNKNOWN");
			}
		}


		private static T GetAnnotation<T>()
			where T : Attribute
		{
			return (T)_getAnnotation(typeof(T));
		}

		private static Func<Type, Attribute> _getAnnotation;
		private static Func<LocalizationAttribute> _getLocalization;
	}

}
