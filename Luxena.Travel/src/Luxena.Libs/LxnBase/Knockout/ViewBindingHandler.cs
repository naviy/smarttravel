using System;
using System.Collections;
using System.Html;

using jQueryApi;

using KnockoutApi;

using LxnBase.Data;
using LxnBase.UI;


namespace LxnBase.Knockout
{
	public class ViewBindingHandler : BindingHandler
	{
		static ViewBindingHandler()
		{
			Ko.BindingHandlers["view"] = new ViewBindingHandler();
		}

		private ViewBindingHandler()
		{
		}

		public override void Init(Element element, Func<object> valueAccessor, Func<Dictionary> allBindingsAccessor, object viewModel)
		{
			jQuery.FromElement(element).Attribute("href", "javascript:void(0)");
		}

		public override void Update(Element element, Func<object> valueAccessor, Func<Dictionary> allBindingsAccessor, object viewModel)
		{
			object value = Ko.Mapping.ToJs(valueAccessor());

			if (value is Array)
			{
				Array arr = (Array) value;

				jQuery.FromElement(element).Text((string) arr[Reference.NamePos]).Click(
					delegate { FormsRegistry.ViewObject((string) arr[Reference.TypePos], arr[Reference.IdPos]); });
			}
			else
			{
				Reference obj = (Reference) value;

				jQuery.FromElement(element).Text(obj.Name).Click(delegate { FormsRegistry.ViewObject(obj.Type, obj.Id); });
			}
		}
	}
}