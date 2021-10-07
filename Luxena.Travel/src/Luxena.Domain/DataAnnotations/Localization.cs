using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Luxena.Domain;


namespace Luxena
{

	public interface IDefaultLocalizationTypesSource
	{
		Type[] DefaultLocalizationTypes { get; }
	}

	public class DefaultLocalizationTypesSource : IDefaultLocalizationTypesSource
	{
		public Type[] DefaultLocalizationTypes { get; set; }

		public DefaultLocalizationTypesSource(params Type[] defaultLocalizationTypes)
		{
			DefaultLocalizationTypes = defaultLocalizationTypes;
		}
	}



	[AttributeUsage(
		AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field,
		AllowMultiple = true
	)]
	[DebuggerDisplay(@"Localization - {Default} / en: {English}, ru: {Russian}, ua: {Ukrainian}")]
	public class LocalizationAttribute : Attribute
	{

		public string Id { get; set; }

		public string Default { get; set; }
		public string Defaults { get; set; }
		public string Default2 { get; set; }
		public string Default5 { get; set; }
		public string DefaultDescription { get; set; }
		public string DefaultShort { get; set; }
		public string DefaultEmpty { get; set; }

		public string English { get; set; }
		public string Englishs { get; set; }
		public string EnglishDescription { get; set; }
		public string EnglishShort { get; set; }
		public string EnglishEmpty { get; set; }

		public string Russian { get; set; }
		public string Russians { get; set; }
		public string Russian2 { get; set; }
		public string Russian5 { get; set; }
		public string RussianDescription { get; set; }
		public string RussianShort { get; set; }
		public string RussianEmpty { get; set; }

		public string Ukrainian { get; set; }
		public string Ukrainians { get; set; }
		public string Ukrainian2 { get; set; }
		public string Ukrainian5 { get; set; }
		public string UkrainianDescription { get; set; }
		public string UkrainianShort { get; set; }
		public string UkrainianEmpty { get; set; }


		// ReSharper disable once FunctionComplexityOverflow
		public LocalizationAttribute(
			string id = null,

			string en = null,
			string ens = null,
			string enDesc = null,
			string enShort = null,
			string enEmpty = null,

			string ru = null,
			string rus = null,
			string ru2 = null,
			string ru5 = null,
			string ruDesc = null,
			string ruShort = null,
			string ruEmpty = null,

			string ua = null,
			string uas = null,
			string ua2 = null,
			string ua5 = null,
			string uaDesc = null,
			string uaShort = null,
			string uaEmpty = null
		)
		{
			Id = id;

			English = en.Clip();
			Englishs = ens.Clip();
			EnglishDescription = enDesc.Clip();
			EnglishShort = enShort.Clip();
			EnglishEmpty = enEmpty.Clip();

			Russian = ru.Clip();
			Russians = rus.Clip();
			Russian2 = ru2.Clip();
			Russian5 = ru5.Clip();
			RussianDescription = ruDesc.Clip();
			RussianShort = ruShort.Clip();
			RussianEmpty = ruEmpty.Clip();

			Ukrainian = ua.Clip();
			Ukrainians = uas.Clip();
			Ukrainian2 = ua2.Clip();
			Ukrainian5 = ua5.Clip();
			UkrainianDescription = uaDesc.Clip();
			UkrainianShort = uaShort.Clip();
			UkrainianEmpty = uaEmpty.Clip();
		}


		public LocalizationAttribute(Type sourceMember)
		{
			_sourceMember = sourceMember;
		}

		public LocalizationAttribute(object enumItem)
		{
			_sourceMember = enumItem.GetType().GetMember(enumItem.ToString()).One();
		}

		private MemberInfo _sourceMember;

		private void Load(IDefaultLocalizationTypesSource lng)
		{
			if (_sourceMember == null) return;

			Append(_sourceMember, lng);

			_sourceMember = null;
		}


		public void Append(IList<Attribute> attrs, IDefaultLocalizationTypesSource lng)
		{
			foreach (var type in lng.DefaultLocalizationTypes)
			{
				foreach (var attr in attrs.Semantics(type).Cast<LocalizationAttribute>())
				{
					attr.AppendTo(this, lng);
				}
			}

			foreach (var attr in attrs.Semantics(a => a.GetType() == typeof(LocalizationAttribute)).Cast<LocalizationAttribute>())
			{
				attr.AppendTo(this, lng);
			}

		}

		public void Append(MemberInfo sourceMember, IDefaultLocalizationTypesSource lng)
		{
			if (sourceMember == null || SemanticExtentions._skipMemberTypes.Contains(sourceMember)) return;

			var attributes = sourceMember.Attributes<Attribute>().ToArray();
			Append(attributes, lng);

			if (Default.No())
				sourceMember.Do((PropertyInfo prop) =>
				{
					if (prop.PropertyType.Name == prop.Name)
						prop.PropertyType.Localization(lng).Do(loc => loc.AppendTo(this, lng));
				});
		}

		public virtual void DefaultAppendTo(LocalizationAttribute dest)
		{
			dest.Default = dest.Default ?? Default;
			dest.Defaults = dest.Defaults ?? Defaults;
			dest.Default2 = dest.Default2 ?? Default2;
			dest.Default5 = dest.Default5 ?? Default5;
			dest.DefaultDescription = dest.DefaultDescription ?? DefaultDescription;
			dest.DefaultShort = dest.DefaultShort ?? DefaultShort;
			dest.DefaultEmpty = dest.DefaultEmpty ?? DefaultEmpty;
		}

		// ReSharper disable once FunctionComplexityOverflow
		public virtual void AppendTo(LocalizationAttribute dest, IDefaultLocalizationTypesSource lng)
		{
			if (dest == null) return;

			Load(lng);

			DefaultAppendTo(dest);

			dest.English = dest.English ?? English;
			dest.Englishs = dest.Englishs ?? Englishs;
			dest.EnglishDescription = dest.EnglishDescription ?? EnglishDescription;
			dest.EnglishShort = dest.EnglishShort ?? EnglishShort;
			dest.EnglishEmpty = dest.EnglishEmpty ?? EnglishEmpty;

			dest.Russian = dest.Russian ?? Russian;
			dest.Russians = dest.Russians ?? Russians;
			dest.Russian2 = dest.Russian2 ?? Russian2;
			dest.Russian5 = dest.Russian5 ?? Russian5;
			dest.RussianDescription = dest.RussianDescription ?? RussianDescription;
			dest.RussianShort = dest.RussianShort ?? RussianShort;
			dest.RussianEmpty = dest.RussianEmpty ?? RussianEmpty;

			dest.Ukrainian = dest.Ukrainian ?? Ukrainian;
			dest.Ukrainians = dest.Ukrainians ?? Ukrainians;
			dest.Ukrainian2 = dest.Ukrainian2 ?? Ukrainian2;
			dest.Ukrainian5 = dest.Ukrainian5 ?? Ukrainian5;
			dest.UkrainianDescription = dest.UkrainianDescription ?? UkrainianDescription;
			dest.UkrainianShort = dest.UkrainianShort ?? UkrainianShort;
			dest.UkrainianEmpty = dest.UkrainianEmpty ?? UkrainianEmpty;
		}

	}


	[DebuggerDisplay(@"EN - {Default} / {English}")]
	public class ENAttribute : LocalizationAttribute
	{
		public ENAttribute(string en = null, string ens = null, string enDesc = null, string enShort = null, string enEmpty = null)
			: base(en: en, ens: ens, enDesc: enDesc, enShort: enShort, enEmpty: enEmpty)
		{
		}

		public override void DefaultAppendTo(LocalizationAttribute dest)
		{
			dest.Default = dest.Default ?? English;
			dest.Defaults = dest.Defaults ?? Englishs;
			dest.DefaultDescription = dest.DefaultDescription ?? EnglishDescription;
			dest.DefaultShort = dest.DefaultShort ?? EnglishShort;
			dest.DefaultEmpty = dest.DefaultEmpty ?? EnglishEmpty;
		}
	}


	[DebuggerDisplay(@"RU - {Default} / {Russian}")]
	public class RUAttribute : LocalizationAttribute
	{
		public RUAttribute(string ru = null, string rus = null, string ru2 = null, string ru5 = null, string ruDesc = null, string ruShort = null, string ruEmpty = null)
			: base(ru: ru, rus: rus, ru2: ru2, ru5: ru5, ruDesc: ruDesc, ruShort: ruShort, ruEmpty: ruEmpty)
		{
		}

		public override void DefaultAppendTo(LocalizationAttribute dest)
		{
			dest.Default = dest.Default ?? Russian;
			dest.Defaults = dest.Defaults ?? Russians;
			dest.Default2 = dest.Default2 ?? Russian2;
			dest.Default5 = dest.Default5 ?? Russian5;
			dest.DefaultDescription = dest.DefaultDescription ?? RussianDescription;
			dest.DefaultShort = dest.DefaultShort ?? RussianShort;
			dest.DefaultEmpty = dest.DefaultEmpty ?? RussianEmpty;
		}
	}


	[DebuggerDisplay(@"UA - {Default} / {Ukrainian}")]
	public class UAAttribute : LocalizationAttribute
	{
		public UAAttribute(string ua = null, string uas = null, string ua2 = null, string ua5 = null, string uaDesc = null, string uaShort = null, string uaEmpty = null)
			: base(ua: ua, uas: uas, ua2: ua2, ua5: ua5, uaDesc: uaDesc, uaShort: uaShort, uaEmpty: uaEmpty)
		{
		}

		public override void DefaultAppendTo(LocalizationAttribute dest)
		{
			dest.Default = dest.Default ?? Ukrainian;
			dest.Defaults = dest.Defaults ?? Ukrainians;
			dest.Default2 = dest.Default2 ?? Ukrainian2;
			dest.Default5 = dest.Default5 ?? Ukrainian5;
			dest.DefaultDescription = dest.DefaultDescription ?? UkrainianDescription;
			dest.DefaultShort = dest.DefaultShort ?? UkrainianShort;
			dest.DefaultEmpty = dest.DefaultEmpty ?? UkrainianEmpty;
		}
	}


	public static class LocalizationExtentions
	{

		public static LocalizationAttribute Localization(this IList<Attribute> attrs, IDefaultLocalizationTypesSource lng)
		{
			return new LocalizationAttribute().Do(a => a.Append(attrs, lng));
		}

		public static LocalizationAttribute Localization(this MemberInfo member, IDefaultLocalizationTypesSource lng)
		{
			return new LocalizationAttribute().Do(a => a.Append(member, lng));
		}

		public static LocalizationAttribute Localization(this object enumItem, IDefaultLocalizationTypesSource lng)
		{
			return new LocalizationAttribute().Do(a => a.Append(enumItem.GetType().GetMember(enumItem.ToString()).One(), lng));
		}


		public static string AsCParams(this LocalizationAttribute me)
		{
			return me == null ? null : (
				me.English.As(a => "en: \"" + a + "\", ") +
				me.Englishs.As(a => "ens: \"" + a + "\", ") +
				me.EnglishDescription.As(a => "enDesc: \"" + a.Replace("\"", "\\\"") + "\", ") +
				me.EnglishShort.As(a => "enShort: \"" + a + "\", ") +
				me.EnglishEmpty.As(a => "enEmpty: \"" + a + "\", ") +

				me.Russian.As(a => "ru: \"" + a + "\", ") +
				me.Russians.As(a => "rus: \"" + a + "\", ") +
				me.Russian2.As(a => "ru2: \"" + a + "\", ") +
				me.Russian5.As(a => "ru5: \"" + a + "\", ") +
				me.RussianDescription.As(a => "ruDesc: \"" + a.Replace("\"", "\\\"") + "\", ") +
				me.RussianShort.As(a => "ruShort: \"" + a + "\", ") +
				me.RussianEmpty.As(a => "ruEmpty: \"" + a + "\", ") +

				me.Ukrainian.As(a => "ua: \"" + a + "\", ") +
				me.Ukrainians.As(a => "uas: \"" + a + "\", ") +
				me.Ukrainian2.As(a => "ua2: \"" + a + "\", ") +
				me.Ukrainian5.As(a => "ua5: \"" + a + "\", ") +
				me.UkrainianDescription.As(a => "uaDesc: \"" + a.Replace("\"", "\\\"") + "\", ") +
				me.UkrainianShort.As(a => "uaShort: \"" + a + "\", ")
			).TrimEnd(", ");
		}

		public static string AsCParams(this ICollection<LocalizationAttribute> list)
		{
			return list == null ? null : (
				list.By(a => a.English.Yes()).As(a => "en: \"" + a.English + "\", ") +
				list.By(a => a.Englishs.Yes()).As(a => "ens: \"" + a.Englishs + "\", ") +
				list.By(a => a.EnglishDescription.Yes()).As(a => "enDesc: \"" + a.EnglishDescription.Replace("\"", "\\\"") + "\", ") +
				list.By(a => a.EnglishShort.Yes()).As(a => "enShort: \"" + a.EnglishShort + "\", ") +
				list.By(a => a.EnglishEmpty.Yes()).As(a => "enEmpty: \"" + a.EnglishEmpty + "\", ") +

				list.By(a => a.Russian.Yes()).As(a => "ru: \"" + a.Russian + "\", ") +
				list.By(a => a.Russians.Yes()).As(a => "rus: \"" + a.Russians + "\", ") +
				list.By(a => a.Russian2.Yes()).As(a => "ru2: \"" + a.Russian2 + "\", ") +
				list.By(a => a.Russian5.Yes()).As(a => "ru5: \"" + a.Russian5 + "\", ") +
				list.By(a => a.RussianDescription.Yes()).As(a => "ruDesc: \"" + a.RussianDescription.Replace("\"", "\\\"") + "\", ") +
				list.By(a => a.RussianShort.Yes()).As(a => "ruShort: \"" + a.RussianShort + "\", ") +
				list.By(a => a.RussianEmpty.Yes()).As(a => "ruEmpty: \"" + a.RussianEmpty + "\", ") +

				list.By(a => a.Ukrainian.Yes()).As(a => "ua: \"" + a.Ukrainian + "\", ") +
				list.By(a => a.Ukrainians.Yes()).As(a => "uas: \"" + a.Ukrainians + "\", ") +
				list.By(a => a.Ukrainian2.Yes()).As(a => "ua2: \"" + a.Ukrainian2 + "\", ") +
				list.By(a => a.Ukrainian5.Yes()).As(a => "ua5: \"" + a.Ukrainian5 + "\", ") +
				list.By(a => a.UkrainianDescription.Yes()).As(a => "uaDesc: \"" + a.UkrainianDescription.Replace("\"", "\\\"") + "\", ") +
				list.By(a => a.UkrainianShort.Yes()).As(a => "uaShort: \"" + a.UkrainianShort + "\", ") +
				list.By(a => a.UkrainianEmpty.Yes()).As(a => "uaEmpty: \"" + a.UkrainianEmpty + "\", ")
			).TrimEnd(", ");
		}

	}

}