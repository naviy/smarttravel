using System;
using System.Collections.Generic;
using System.Html;

using KnockoutApi;


namespace LxnBase.Knockout
{
	public class StringTemplate
	{
		static StringTemplate()
		{
			NativeTemplateEngine templateEngine = new NativeTemplateEngine();

			templateEngine.MakeTemplateSource = Get;

			Ko.SetTemplateEngine(templateEngine);
		}

		public StringTemplate(string id, string text)
		{
			_id = id;
			_text = text;
			_data = new Dictionary<object, object>();

			_templates[id] = this;
		}

		public static StringTemplate Get(string id)
		{
			return _templates[id];
		}

		public Element RenderTo(Element el, object model)
		{
			el.SetAttribute("data-bind", "template: '" + _id + "'");

			Ko.ApplyBindings(model, el);

			return el;
		}

		public string Text(string value)
		{
			return Arguments.Length == 0 ? _text : _text = value;
		}

		public object Data(string key, object value)
		{
			return Arguments.Length == 1 ? _data[key] : _data[key] = value;
		}

		private static readonly Dictionary<string, StringTemplate> _templates = new Dictionary<string, StringTemplate>();

		private readonly string _id;
		private string _text;
		private readonly Dictionary<object, object> _data;
	}
}