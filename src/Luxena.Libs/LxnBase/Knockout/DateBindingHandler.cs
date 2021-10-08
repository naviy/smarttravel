using System;
using System.Collections;
using System.Html;

using jQueryApi;

using KnockoutApi;


namespace LxnBase.Knockout
{

	public class DateBindingHandler : BindingHandler
	{
		static DateBindingHandler()
		{
			Ko.BindingHandlers["date"] = new DateBindingHandler();
		}

		private DateBindingHandler()
		{
		}

		public override void Update(Element element, Func<object> valueAccessor, Func<Dictionary> allBindingsAccessor, object viewModel)
		{
			Date value = Ko.UnwrapObservable<Date>(valueAccessor());

			// ReSharper disable IsExpressionAlwaysTrue
			if (value is Date)
				jQuery.FromElement(element).Text(value.Format("d.m.Y"));
		}
	}

}