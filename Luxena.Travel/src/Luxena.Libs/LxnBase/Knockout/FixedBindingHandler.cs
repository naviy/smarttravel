using System;
using System.Collections;
using System.Html;
using jQueryApi;
using KnockoutApi;

namespace LxnBase.Knockout
{

	public class FixedBindingHandler : BindingHandler
	{

		static FixedBindingHandler()
		{
			Ko.BindingHandlers["fixed"] = new FixedBindingHandler();
		}

		private FixedBindingHandler()
		{
		}

		public override void Update(Element element, Func<object> valueAccessor, Func<Dictionary> allBindingsAccessor, object viewModel)
		{
			jQueryObject obj = jQuery.FromElement(element);

			object value = Ko.UnwrapObservable<object>(valueAccessor());

			if (!Script.IsValue(value))
				value = "";
			else if (NumberUtility.IsNumber(value))
			{
				Number num = (Number)value;
				value = num.Format("n");

				if (num < 0)
					obj.AddClass("negative");
			}
			else
			{
				object moneyString = Type.GetField(value, "MoneyString");

				if (Script.IsValue(moneyString))
					value = Ko.UnwrapObservable<string>(moneyString);
				else
					value = value.ToString();
			}

			obj.Text((string)value);
		}

	}

}