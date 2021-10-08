using System;

using Ext;

using Action = Ext.Action;


namespace LxnBase.UI
{
	public static class ActionFactory
	{
		public static Action CreateListAction(string type, string text)
		{
			return new Action(new ActionConfig()
				.text(text)
				.handler(new AnonymousDelegate(delegate { FormsRegistry.ListObjects(type); }))
				.ToDictionary());
		}

		public static Action CreateViewAction(string type, object id, string text)
		{
			return new Action(new ActionConfig()
				.text(text)
				.handler(new AnonymousDelegate(delegate { FormsRegistry.ViewObject(type, id); }))
				.ToDictionary());
		}


		public static Action CreateAction3(string viewPage, string text)
		{
			return new Action(new ActionConfig()
				.text(text)
				.handler(new AnonymousDelegate(delegate
				{
					Script.Eval(@"window.open('http://travel3/#/" + viewPage + @"', 'LuxenaTravel3');");
				}))
				.ToDictionary());
		}
		
	}
}