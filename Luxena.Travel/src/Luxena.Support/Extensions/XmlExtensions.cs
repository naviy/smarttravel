using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;


namespace Luxena
{

	public static class XmlExtensions
	{

		[DebuggerStepThrough]
		public static XElement El(this XContainer el, XName name)
		{
			return el?.Element(name);
		}

		[DebuggerStepThrough]
		public static XElement El(this XContainer el, XName name1, XName name)
		{
			return el?.Element(name1)?.Element(name);
		}

		[DebuggerStepThrough]
		public static XElement El(this XContainer el, XName name1, XName name2, XName name)
		{
			return el?.Element(name1)?.Element(name2)?.Element(name);
		}

		[DebuggerStepThrough]
		public static XElement El(this XContainer el, XName name1, XName name2, XName name3, XName name)
		{
			return el?.Element(name1)?.Element(name2)?.Element(name3)?.Element(name);
		}


		[DebuggerStepThrough]
		public static IEnumerable<XElement> Els(this XContainer el, XName name)
		{
			return el?.Elements(name);
		}

		[DebuggerStepThrough]
		public static IEnumerable<XElement> Els(this XContainer el, XName name1, XName name)
		{
			return el?.Element(name1)?.Elements(name);
		}

		[DebuggerStepThrough]
		public static IEnumerable<XElement> Els(this XContainer el, XName name1, XName name2, XName name)
		{
			return el?.Element(name1)?.Element(name2)?.Elements(name);
		}

		[DebuggerStepThrough]
		public static IEnumerable<XElement> Els(this XContainer el, XName name1, XName name2, XName name3, XName name)
		{
			return el?.Element(name1)?.Element(name2)?.Element(name3)?.Elements(name);
		}


		[DebuggerStepThrough]
		public static string Attr(this XElement el, XName name)
		{
			return el?.Attribute(name)?.Value.Clip();
		}

		[DebuggerStepThrough]
		public static string Attr(this XElement el, XName name1, XName name)
		{
			return el?.Element(name1)?.Attribute(name)?.Value.Clip();
		}

		[DebuggerStepThrough]
		public static string Attr(this XElement el, XName name1, XName name2, XName name)
		{
			return el?.Element(name1)?.Element(name2)?.Attribute(name)?.Value.Clip();
		}

		[DebuggerStepThrough]
		public static string Attr(this XElement el, XName name1, XName name2, XName name3, XName name)
		{
			return el?.Element(name1)?.Element(name2)?.Element(name3)?.Attribute(name)?.Value.Clip();
		}


		[DebuggerStepThrough]
		public static T EnumAttr<T>(this XElement el, XName name)
		{
			if (el == null) return default(T);

			var attr = el.Attribute(name);
			if (attr == null) return default(T);

			var t = typeof(T);
			return Enum.IsDefined(t, attr.Value) ? (T)Enum.Parse(t, attr.Value) : default(T);
		}

		[DebuggerStepThrough]
		public static T EnumAttr<T>(this XElement el, XName name1, XName name)
		{
			el = el?.Element(name1);
			return el == null ? default(T) : el.EnumAttr<T>(name);
		}

		[DebuggerStepThrough]
		public static T EnumAttr<T>(this XElement el, XName name1, XName name2, XName name)
		{
			el = el?.Element(name1)?.Element(name2);
			return el == null ? default(T) : el.EnumAttr<T>(name);
		}

		[DebuggerStepThrough]
		public static T EnumAttr<T>(this XElement el, XName name1, XName name2, XName name3, XName name)
		{
			el = el?.Element(name1)?.Element(name2)?.Element(name3);
			return el == null ? default(T) : el.EnumAttr<T>(name);
		}


		[DebuggerStepThrough]
		public static string Value(this XContainer el, XName name)
		{
			return el?.Element(name)?.Value.Clip();
		}

		[DebuggerStepThrough]
		public static string Value(this XContainer el, XName name1, XName name)
		{
			return el?.Element(name1)?.Element(name)?.Value.Clip();
		}

		[DebuggerStepThrough]
		public static string Value(this XContainer el, XName name1, XName name2, XName name)
		{
			return el?.Element(name1)?.Element(name2)?.Element(name)?.Value.Clip();
		}

		[DebuggerStepThrough]
		public static string Value(this XContainer el, XName name1, XName name2, XName name3, XName name)
		{
			return el?.Element(name1)?.Element(name2)?.Element(name3)?.Element(name)?.Value.Clip();
		}

	}

}