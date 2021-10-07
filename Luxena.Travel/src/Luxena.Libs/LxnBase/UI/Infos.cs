using System;
using System.Collections;

using System.Html;

using Ext;

using Element = Ext.Element;


namespace LxnBase.UI
{
	public static class Infos
	{
		public static void Init()
		{
			_container = (Element) DomHelper.insertFirst(Document.Body, new Dictionary("id", "msg-div"), true);

			MessageRegister.NewMessage += OnNewMessage;
		}

		private static void OnNewMessage(object sender, MessageRegisterEventArgs e)
		{
			if (e.Type != MessageType.Info)
				return;

			string caption;
			string message = string.Empty;
			string details = string.Empty;
			string separator = string.Empty;

			if (Script.IsNullOrUndefined(e.MessageCaption))
			{
				caption = e.Message;
				message = e.Details;
			}
			else
			{
				caption = e.MessageCaption;

				if (!Script.IsNullOrUndefined(e.Message))
				{
					message = e.Message;
					separator = "<br/>";
				}

				if (!Script.IsNullOrUndefined(e.Details))
				{
					details = "<div class='details'>" + e.Details + "</div";
					message += separator;
				}
			}

			message += details;

			string html = new string[]
			{
				"<div class='msg'><div class='x-box-tl'><div class='x-box-tr'><div class='x-box-tc'></div></div></div><div class='x-box-ml'><div class='x-box-mr'><div class='x-box-mc'><h3>",
				caption, "</h3>", message,
				"</div></div></div><div class='x-box-bl'><div class='x-box-br'><div class='x-box-bc'></div></div></div></div>"
			}.Join("");

			Element msg = (Element) DomHelper.append(_container, new Dictionary("html", html), true);

			msg.slideIn("t", null).pause(Timeout).ghost("t", new Dictionary("remove", true));
		}

		private static Element _container;
		private const int Timeout = 3;
	}
}