using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Luxena.Domain;


namespace Luxena
{

	public class DefaultLocalizationTypesSource
	{
		public Type[] DefaultLocalizationTypes { get; set; }

		public DefaultLocalizationTypesSource(params Type[] defaultLocalizationTypes)
		{
			DefaultLocalizationTypes = defaultLocalizationTypes.No()
				? new[] { typeof(RUAttribute), typeof(UAAttribute), typeof(ENAttribute), }
				: defaultLocalizationTypes;
		}
	}


	public class LocalizationText
	{
		public LocalizationText(LocalizationAttribute loc)
		{
			_loc = loc;
		}

		public string One { get; set; }
		public string OneOrDefault => One ?? _loc.Default.One;
		public string Many { get; set; }
		public string ManyOrDefault => Many ?? _loc.Default.Many;
		public string Two { get; set; }
		public string Five { get; set; }
		public string Description { get; set; }
		public string Short { get; set; }
		public string Empty { get; set; }

		private readonly LocalizationAttribute _loc;


		public bool Yes() => One.Yes();
		public bool No() => One.No();

		public override string ToString() => One;
		public static implicit operator string(LocalizationText me) => me?.One;

		public void Append(LocalizationText src)
		{
			One = One ?? src.One;
			Many = Many ?? src.Many;
			Two = Two ?? src.Two;
			Five = Five ?? src.Five;
			Description = Description ?? src.Description;
			Short = Short ?? src.Short;
			Empty = Empty ?? src.Empty;
		}

		public string AsCParams(string lang)
		{
			return (
				One.As(a => lang + ": \"" + Quote(a) + "\", ") +
				Many.As(a => lang + "s: \"" + Quote(a) + "\", ") +
				Two.As(a => lang + "2: \"" + Quote(a) + "\", ") +
				Five.As(a => lang + "5: \"" + Quote(a) + "\", ") +
				Description.As(a => lang + "Desc: \"" + Quote(a) + "\", ") +
				Short.As(a => lang + "Short: \"" + Quote(a) + "\", ") +
				Empty.As(a => lang + "Empty: \"" + Quote(a) + "\", ")
			).TrimEnd(", ");
		}

		static string Quote(string s) => s.Replace("\"", "\\\"");

	}


	[AttributeUsage(
		AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Method,
		AllowMultiple = true
	)]
	[DebuggerDisplay(@"Localization - {Default} / en: {English}, ru: {Russian}, ua: {Ukrainian}")]
	public class LocalizationAttribute : Attribute
	{

		public MemberInfo SourceMember { get; private set; }

		public LocalizationText Default { get; }
		public LocalizationText English { get; }
		public LocalizationText Russian { get; }
		public LocalizationText Ukrainian { get; }

		public LocalizationText this[string lang]
		{
			get
			{
				switch (lang.ToLower())
				{
					case "en": return English;
					case "ru": return Russian;
					case "ua": return Ukrainian;
					default: return Default;
				}
			}
		}

		public override string ToString() => Default.One;

		private LocalizationAttribute()
		{
			Default = new LocalizationText(this);
			English = new LocalizationText(this);
			Russian = new LocalizationText(this);
			Ukrainian = new LocalizationText(this);
		}


		[DebuggerStepThrough]
		public LocalizationAttribute(
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
		) : this()
		{
			English.One = en.Clip();
			English.Many = ens.Clip();
			English.Description = enDesc.Clip();
			English.Short = enShort.Clip();
			English.Empty = enEmpty.Clip();

			Russian.One = ru.Clip();
			Russian.Many = rus.Clip();
			Russian.Two = ru2.Clip();
			Russian.Five = ru5.Clip();
			Russian.Description = ruDesc.Clip();
			Russian.Short = ruShort.Clip();
			Russian.Empty = ruEmpty.Clip();

			Ukrainian.One = ua.Clip();
			Ukrainian.Many = uas.Clip();
			Ukrainian.Two = ua2.Clip();
			Ukrainian.Five = ua5.Clip();
			Ukrainian.Description = uaDesc.Clip();
			Ukrainian.Short = uaShort.Clip();
			Ukrainian.Empty = uaEmpty.Clip();
		}

		[DebuggerStepThrough]
		public LocalizationAttribute(Type sourceMember) : this()
		{
			SourceMember = sourceMember;
		}

		[DebuggerStepThrough]
		public LocalizationAttribute(object enumItem) : this()
		{
			SourceMember = enumItem.GetType().GetMember(enumItem.ToString()).One();
		}


		public void Append(MemberInfo sourceMember, IList<Attribute> attrs, DefaultLocalizationTypesSource lng, bool usePropertyPattern = true)
		{
			foreach (var type in lng.DefaultLocalizationTypes)
			{
				foreach (var attr in attrs.Semantics(type, usePropertyPattern: usePropertyPattern).Cast<LocalizationAttribute>())
				{
					attr.AppendTo(this, lng);
				}
			}

			var localizationAttributeType = typeof(LocalizationAttribute);

			foreach (var attr in attrs.Semantics(a => a.GetType() == localizationAttributeType, usePropertyPattern: usePropertyPattern).Cast<LocalizationAttribute>())
			{
				attr.AppendTo(this, lng);
			}


			if (Default.No())
				sourceMember.ResultType().Do(ptype =>
				{
					if (sourceMember == ptype) return;

					var typeName = ptype.Name;
					var memberName = sourceMember.Name;
					if (typeName == memberName || "_" + typeName == memberName || typeName == memberName + "Reference")
						ptype.Localization(lng)?.AppendTo(this, lng);
				});

			if (Default.No())
			{
				var pattern = attrs.OfType<MemberPatternAttribute>().One();
				pattern?.Pattern?.Localization(lng)?.AppendTo(this, lng);
			}
		}

		public void Append(MemberInfo sourceMember, DefaultLocalizationTypesSource lng)
		{
			if (sourceMember == null || SemanticExtensions._skipMemberTypes.Contains(sourceMember)) return;

			var attributes = sourceMember.Attributes<Attribute>().ToArray();
			Append(sourceMember, attributes, lng);
		}

		public virtual void DefaultAppendTo(LocalizationAttribute dest)
		{
			dest.Default.Append(Default);
		}

		// ReSharper disable once FunctionComplexityOverflow
		public virtual void AppendTo(LocalizationAttribute dest, DefaultLocalizationTypesSource lng)
		{
			if (dest == null) return;

			if (SourceMember != null)
			{
				Append(SourceMember, lng);
				dest.SourceMember = SourceMember;
				SourceMember = null;
			}

			DefaultAppendTo(dest);

			dest.English.Append(English);
			dest.Russian.Append(Russian);
			dest.Ukrainian.Append(Ukrainian);
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
			dest.Default.Append(English);
		}
	}


	[DebuggerDisplay(@"RU - {Default} / {Russian}")]
	public class RUAttribute : LocalizationAttribute
	{
		[DebuggerStepThrough]
		public RUAttribute(string ru = null, string rus = null, string ru2 = null, string ru5 = null, string ruDesc = null, string ruShort = null, string ruEmpty = null)
			: base(ru: ru, rus: rus, ru2: ru2, ru5: ru5, ruDesc: ruDesc, ruShort: ruShort, ruEmpty: ruEmpty)
		{
		}

		public override void DefaultAppendTo(LocalizationAttribute dest)
		{
			dest.Default.Append(Russian);
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
			dest.Default.Append(Ukrainian);
		}
	}


	public static class LocalizationExtentions
	{

		public static LocalizationAttribute Localization(
			this MemberInfo member,
			IList<Attribute> attrs,
			DefaultLocalizationTypesSource lng,
			bool usePropertyPattern = true
		)
		{
			var loc = new LocalizationAttribute();
			loc.Append(member, attrs, lng, usePropertyPattern: usePropertyPattern);
			return loc;
		}

		public static LocalizationAttribute Localization(
			this MemberInfo member,
			DefaultLocalizationTypesSource lng)
		{
			var loc = new LocalizationAttribute();
			loc.Append(member, lng);
			return loc;
		}


		public static LocalizationAttribute EnumLocalization(
			this object enumItem,
			DefaultLocalizationTypesSource lng)
		{
			var loc = _enumLocalizations.By(enumItem);

			if (loc == null)
			{
				var member = enumItem.GetType().GetMember(enumItem.ToString()).One();
				loc = new LocalizationAttribute();
				loc.Append(member, lng);
				_enumLocalizations[enumItem] = loc;
			}

			return loc;
		}

		static readonly Dictionary<object, LocalizationAttribute> _enumLocalizations = new Dictionary<object, LocalizationAttribute>();


		static string Quote(string s) => s.Replace("\"", "\\\"");

		public static string AsCParams(this LocalizationAttribute me)
		{
			return me == null ? null : (
				me.English.AsCParams("en").As(a => a + ", ") +
				me.Russian.AsCParams("ru").As(a => a + ", ") +
				me.Ukrainian.AsCParams("ua")
			).Trim(", ");
		}

		//public static string AsCParams(this ICollection<LocalizationAttribute> list)
		//{
		//	return list == null ? null : (
		//		list.By(a => a.English.Yes()).As(a => "en: \"" + a.English + "\", ") +
		//		list.By(a => a.Englishs.Yes()).As(a => "ens: \"" + a.Englishs + "\", ") +
		//		list.By(a => a.EnglishDescription.Yes()).As(a => "enDesc: \"" + a.EnglishDescription.Replace("\"", "\\\"") + "\", ") +
		//		list.By(a => a.EnglishShort.Yes()).As(a => "enShort: \"" + a.EnglishShort + "\", ") +
		//		list.By(a => a.EnglishEmpty.Yes()).As(a => "enEmpty: \"" + a.EnglishEmpty + "\", ") +

		//		list.By(a => a.Russian.Yes()).As(a => "ru: \"" + a.Russian + "\", ") +
		//		list.By(a => a.Russians.Yes()).As(a => "rus: \"" + a.Russians + "\", ") +
		//		list.By(a => a.Russian2.Yes()).As(a => "ru2: \"" + a.Russian2 + "\", ") +
		//		list.By(a => a.Russian5.Yes()).As(a => "ru5: \"" + a.Russian5 + "\", ") +
		//		list.By(a => a.RussianDescription.Yes()).As(a => "ruDesc: \"" + a.RussianDescription.Replace("\"", "\\\"") + "\", ") +
		//		list.By(a => a.RussianShort.Yes()).As(a => "ruShort: \"" + a.RussianShort + "\", ") +
		//		list.By(a => a.RussianEmpty.Yes()).As(a => "ruEmpty: \"" + a.RussianEmpty + "\", ") +

		//		list.By(a => a.Ukrainian.Yes()).As(a => "ua: \"" + a.Ukrainian + "\", ") +
		//		list.By(a => a.Ukrainians.Yes()).As(a => "uas: \"" + a.Ukrainians + "\", ") +
		//		list.By(a => a.Ukrainian2.Yes()).As(a => "ua2: \"" + a.Ukrainian2 + "\", ") +
		//		list.By(a => a.Ukrainian5.Yes()).As(a => "ua5: \"" + a.Ukrainian5 + "\", ") +
		//		list.By(a => a.UkrainianDescription.Yes()).As(a => "uaDesc: \"" + a.UkrainianDescription.Replace("\"", "\\\"") + "\", ") +
		//		list.By(a => a.UkrainianShort.Yes()).As(a => "uaShort: \"" + a.UkrainianShort + "\", ") +
		//		list.By(a => a.UkrainianEmpty.Yes()).As(a => "uaEmpty: \"" + a.UkrainianEmpty + "\", ")
		//	).TrimEnd(", ");
		//}

	}

}