using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luxena.Travel.Domain
{

	partial class Domain
	{

		private static DefaultLocalizationTypesSource _defaultLocalizationTypesSource;

		public static DefaultLocalizationTypesSource DefaultLocalizationTypesSource
		{
			get { return _defaultLocalizationTypesSource ?? (_defaultLocalizationTypesSource = new DefaultLocalizationTypesSource()); }
		}

		public static implicit operator DefaultLocalizationTypesSource(Domain db)
		{
			return DefaultLocalizationTypesSource;
		}

	}


	public static class LocalizationExtentions
	{

		public static LocalizationAttribute Localization(this MemberInfo member, IList<Attribute> attrs)
		{
			return member.Localization(attrs, Domain.DefaultLocalizationTypesSource);
		}

		public static LocalizationAttribute Localization(this MemberInfo member)
		{
			return member.Localization(Domain.DefaultLocalizationTypesSource);
		}

		public static LocalizationAttribute EnumLocalization(this object enumItem)
		{
			return enumItem.EnumLocalization(Domain.DefaultLocalizationTypesSource);
		}

	}

}
