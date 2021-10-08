using System;
using System.Collections;
using System.Html;

using jQueryApi;

using KnockoutApi;


namespace LxnBase.Knockout
{
	public class EmailBindingHandler : BindingHandler
	{
		static EmailBindingHandler()
		{
			Ko.BindingHandlers["email"] = new EmailBindingHandler();
		}

		private EmailBindingHandler()
		{
		}

		public override void Update(Element element, Func<object> valueAccessor, Func<Dictionary> allBindingsAccessor, object viewModel)
		{
			string text = Ko.UnwrapObservable<string>(valueAccessor());

			string href = string.IsNullOrEmpty(text) ? null : (_protocolCheck.Test(text) ? text : "mailto:" + text);

			jQuery.FromElement(element).Attribute("href", href).Text(text);
		}

		private static readonly RegularExpression _protocolCheck = new RegularExpression("^.+:");
	}
}