using System;
using System.Collections;
using System.Html;

using jQueryApi;

using KnockoutApi;


namespace LxnBase.Knockout
{
	public class LinkBindingHandler : BindingHandler
	{
		static LinkBindingHandler()
		{
			Ko.BindingHandlers["link"] = new LinkBindingHandler();
		}

		private LinkBindingHandler()
		{
		}

		public override void Update(Element element, Func<object> valueAccessor, Func<Dictionary> allBindingsAccessor, object viewModel)
		{
			string text = Ko.UnwrapObservable<string>(valueAccessor());

			string href = string.IsNullOrEmpty(text) ? null : (_protocolCheck.Test(text) ? text : "http://" + text);

			jQuery.FromElement(element).Attribute("href", href).Text(text);
		}

		private static readonly RegularExpression _protocolCheck = new RegularExpression("^.+://");
	}
}