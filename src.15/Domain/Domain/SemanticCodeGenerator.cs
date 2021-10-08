using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public static class SemanticCodeGenerator
	{

		public static List<string> GetCodeForTypeScript(MemberInfo member, DefaultLocalizationTypesSource lng)
		{
			_getSemantic = member.Semantic;

			_getLocalization = () => member.Localization(lng);

			var method = member as MethodInfo;
			if (method!=null)
				return GetCodeForTypeScript_(method, lng);

			return GetCodeForTypeScript_(member, lng);
		}

		public static List<string> GetCodeForTypeScript(SemanticSetup.PropertyBag propBag, DefaultLocalizationTypesSource lng)
		{
			_getSemantic = propBag.Semantic;

			_getLocalization = () => propBag.Localization(lng);

			return GetCodeForTypeScript_(propBag.Property, lng, appendType: false);
		}


		//public static List<string> GetCodeForTypeScript(SemanticSetup.CompositePropertyBag propBag, IDefaultLocalizationTypesSource lng)
		//{
		//	_getSemantic = propBag.Semantic;

		//	_getLocalization = () => propBag.Localization(lng);

		//	return GetCodeForTypeScript_(propBag.Property, lng, appendType: false);
		//}



		//		private static readonly Regex reBigBigLetter = new Regex("[A-Z][A-Z]");

		private static List<string> GetCodeForTypeScript_(MemberInfo member, DefaultLocalizationTypesSource lng, bool appendType = true)
		{
			var ptype = member.ResultType();
			var enumType = ptype.AsEnumType();
			var prop = member as PropertyInfo;

			var tags = new List<string>();

			var loc = _getLocalization();
			if ((loc == null || loc.Default.No()) && enumType != null)
				loc = enumType.Localization(lng);

			if (loc != null)
			{
				if (loc.Default.Yes())
					tags.Add(".localizeTitle({ " + loc.AsCParams() + " })");

				if (loc.Default.Short.Yes())
					tags.Add(".emptyText(\"" + loc.Default.Short.Replace("\"", "\\\"") + "\")");
			}

			AppendTypeTags(member, tags, appendType);

			if (!member.IsDbMapped())
				tags.Add(".calculated()");
			else if (prop != null)
			{
				if (!prop.CanWrite)
				{
					tags.Add(".calculated()");
					tags.Add(".nonsaved()");
				}
				else if (!prop.SetMethod.IsPublic)
				{
					tags.Add(".readOnly()");
					tags.Add(".nonsaved()");
				}
			}


			if (ptype.IsValueType && !ptype.IsNullable() ||
				GetSemantic<UiRequiredAttribute>() != null ||
				GetSemantic<RequiredAttribute>() != null)
				tags.Add(".required()");


			var length = GetSemantic<LengthAttribute>() ?? enumType.As(a => a.Semantic<LengthAttribute>());
			length.Do(a => tags.Add(".length(" + a.Length + ", " + a.MinLength + ", " + a.MaxLength + ")"));

			if (ptype != typeof(string))
				GetSemantic<MaxLengthAttribute>().Do(a => tags.Add(".maxLength(" + a.Length + ")"));

			GetSemantic<MinLengthAttribute>().Do(a => tags.Add(".minLength(" + a.Length + ")"));
			GetSemantic<LineCountAttribute>().Do(a => tags.Add(".lineCount(" + a.Count + ")"));

			GetSemantic<EntityDateAttribute>().Do(a => tags.Add(".entityDate()"));
			GetSemantic<EntityNameAttribute>().Do(a => tags.Add(".entityName()"));
			GetSemantic<EntityTypeAttribute>().Do(a => tags.Add(".entityType()"));
			GetSemantic<EntityPositionAttribute>().Do(a => tags.Add(".entityPosition()"));
			GetSemantic<SecondaryAttribute>().Do(a => tags.Add(".secondary()"));
			GetSemantic<UtilityAttribute>().Do(a => tags.Add(".utility()"));

			GetSemantic<SubjectAttribute>().Do(a => tags.Add(".subject()"));

			GetSemantic<UniqueAttribute>().Do(a => tags.Add(".unique()"));


			GetSemantic<DefaultValueAttribute>().Do(a =>
			{
				var value = a.Value;
				if (value == null)
					value = "null";
				else if (value is bool)
					value = value.ToString().ToLower();
				else if (value.GetType().IsEnum)
					value = (int)value;

				tags.Add(".defaultValue(" + value + ")");
			});

			//			if (prop.HasAttribute<NotMappedAttribute>())
			//				tags.Add(".IsPersistent(false)");

			tags.RemoveAll(a => a.No());

			return tags;
		}

		private static List<string> GetCodeForTypeScript_(MethodInfo method, DefaultLocalizationTypesSource lng, bool appendType = true)
		{
			var tags = new List<string>();

			var loc = _getLocalization();

			if (loc != null)
			{
				if (loc.Default.Yes())
					tags.Add(".localizeTitle({ " + loc.AsCParams() + " })");
			}

			GetSemantic<IconAttribute>().Do(a => tags.Add($".icon(\"{a.IconName}\")"));

			tags.RemoveAll(a => a.No());

			return tags;
		}



		private static void AppendTypeTags(MemberInfo prop, List<string> tags, bool appendType)
		{
			var ptype = prop.ResultType();
			var ptypeCode = GetSemantic<SemanticTypeAttribute>(usePropertyPattern: false).As(a => a.ToTypeScript());

			if (ptypeCode != null)
			{
				tags.Add(ptypeCode);
				return;
			}

			var lookup = GetSemantic<LookupAttribute>();

			if (!appendType)
			{
				if (lookup != null)
					tags.Add(".lookup(() => sd." + lookup.ReferenceType.Name + ")");
				else if (ptype.IsReference())
				{
					var pattern = GetSemantic<MemberPatternAttribute>();
					if (pattern != null)
						tags.Add(".lookup(() => sd." + pattern.Pattern.ResultType().Name + ")");
				}

				return;
			}

			if (ptype.IsBool())
				ptypeCode = ".bool()";
			else if (ptype.IsDateTime())
				ptypeCode = ".date()";
			else if (ptype.IsFloat())
				ptypeCode = ".float()";
			else if (ptype.IsInt())
				ptypeCode = ".int()";
			else if (ptype == typeof(string))
				ptypeCode = ".string(" + GetSemantic<MaxLengthAttribute>().As(a => (int?)a.Length) + ")";
			else if (ptype.IsEntity())
			{
				ptypeCode = ".lookup(() => sd." + (lookup.As(a => a.ReferenceType.Name) ?? ptype.Name) + ")";
			}
			else if (ptype.IsReference())
			{
				ptypeCode = ".lookup(() => sd." + ptype.Name.TrimEnd("Reference") + ")";
			}
			else if (ptype == typeof(Money))
				ptypeCode = ".money()";
			else if (ptype == typeof(object))
				ptypeCode = "";
			else if (ptype.IsEnumerable())
				ptypeCode = "";

			if (ptypeCode != null)
			{
				if (ptypeCode != "")
					tags.Add(ptypeCode);
			}
			else
			{
				var enumType = ptype.AsEnumType();
				if (enumType != null)
				{
					tags.Add(".enum(" + enumType.Name + ")"
						+ prop.Semantic<Luxena.Domain.FlagsAttribute>().As(a => ".enumIsFlags()")
					);
				}
				else
					tags.Add(".UNKNOWN");
			}
		}


		private static T GetSemantic<T>(bool usePropertyPattern = true)
			where T : Attribute
		{
			return (T)_getSemantic(typeof(T), usePropertyPattern);
		}

		private static Func<Type, bool, Attribute> _getSemantic;
		private static Func<LocalizationAttribute> _getLocalization;
	}

}
