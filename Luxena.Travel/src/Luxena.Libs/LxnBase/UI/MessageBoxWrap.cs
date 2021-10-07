using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;


namespace LxnBase.UI
{
	public class MessageBoxWrap
	{
		[AlternateSignature]
		public static extern void Show(object config);

		[AlternateSignature]
		public static extern void Show(string title, string msg, string icon, object buttons);

		public static void Show(string title, string msg, string icon, object buttons, MessageBoxResponseDelegate func)
		{
			object config = title;

			if (title is string)
			{
				Dictionary dictionary = new Dictionary(
					"title", title,
					"msg", msg,
					"icon", icon,
					"buttons", buttons);

				if (!Script.IsNullOrUndefined(func))
					dictionary["fn"] = func;

				config = dictionary;
			}

			MessageBoxResponseDelegate fn = (MessageBoxResponseDelegate) Type.GetField(config, "fn");

			Type.SetField(config, "fn", GetCallbackDelegate(fn));

			MessageBox messageBox = MessageBox.show(config);

			EventsManager.RegisterKeyDownHandler(messageBox, false);
		}

		public static void Confirm(string title, string msg, MessageBoxResponseDelegate fn)
		{
			MessageBox messageBox = MessageBox.confirm(title, msg, GetCallbackDelegate(fn));

			EventsManager.RegisterKeyDownHandler(messageBox, false);
		}

		public static void Prompt(string title, string msg, MessageBoxResponseDelegate fn)
		{
			MessageBox messageBox = MessageBox.prompt(title, msg, GetCallbackDelegate(fn));

			EventsManager.RegisterKeyDownHandler(messageBox, false);
		}

		private static Delegate GetCallbackDelegate(MessageBoxResponseDelegate fn)
		{
			if (Script.IsNullOrUndefined(fn))
				return new AnonymousDelegate(
					delegate { EventsManager.UnregisterKeyDownHandler(); });

			return new MessageBoxResponseDelegate(
				delegate(string button, string text)
				{
					EventsManager.UnregisterKeyDownHandler();

					fn.Invoke(button, text);
				});
		}
	}
}