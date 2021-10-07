using System;
using System.Collections;
using System.Collections.Generic;
using System.Html;

using jQueryApi;

using KnockoutApi;


namespace LxnBase.Knockout
{
	public class ResBindingHandler : BindingHandler
	{
		static ResBindingHandler()
		{
			Ko.BindingHandlers["res"] = new ResBindingHandler();
		}

		public static List<object> Resources
		{
			get { return _resources; }
		}

		private ResBindingHandler()
		{
		}

		public override void Update(Element element, Func<object> valueAccessor, Func<Dictionary> allBindingsAccessor, object viewModel)
		{
			string name = (string) valueAccessor();

			if (string.IsNullOrEmpty(name))
				return;

			foreach (Dictionary resource in _resources)
			{
				string value = (string) resource[name];

				if (!string.IsNullOrEmpty(value))
				{
					jQuery.FromElement(element).Text(value);
					return;
				}
			}
		}

		private static readonly List<object> _resources = new List<object>();
	}
}