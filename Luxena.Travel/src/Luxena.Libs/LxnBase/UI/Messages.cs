using System;
using System.Collections;
using System.Html;

using Ext;
using Ext.lib;

using Element = System.Html.Element;


namespace LxnBase.UI
{
	public class Messages
	{
		public Messages(Dictionary config)
		{
			if (!config.ContainsKey("title"))
				config["title"] = BaseRes.Messages_Title;

			_panel = new Panel(config);
			_panel.addListener("expand", new AnonymousDelegate(ClearMessageCounter));

			MessageRegister.NewMessage += OnNewMessage;
		}

		public Component Widget
		{
			get { return _panel; }
		}

		public void SetLayoutRegion(Region region)
		{
			_region = region;

			_messageCountElement = Document.CreateElement("div");
			_messageCountElement.ClassName = "message-count";

			Element collapsedHeader = (Element) Type.GetField(Type.InvokeMethod(_region, "getCollapsedEl"), "dom");

			collapsedHeader.AddEventListener("click", delegate { ClearMessageCounter(); }, false);
			collapsedHeader.InsertBefore(_messageCountElement, collapsedHeader.FirstChild);
		}

		private void OnNewMessage(object sender, MessageRegisterEventArgs e)
		{
			if (e.Handled)
				return;

			if (_containerDiv == null)
			{
				_containerDiv = Document.CreateElement("div");
				_panel.body.dom.AppendChild(_containerDiv);
			}

			Element newMessage = GetMessageElement(e);
			Element lastMessage = _containerDiv.Children.Length == 0 ? null : _containerDiv.Children[0];

			if (lastMessage == null)
				_containerDiv.AppendChild(newMessage);
			else
				_containerDiv.InsertBefore(newMessage, lastMessage);

			if (_containerDiv.Children.Length > MaxMessageCount)
				_containerDiv.RemoveChild(_containerDiv.Children[_containerDiv.Children.Length - 1]);

			if (_messageCountElement != null && _panel.collapsed)
			{
				++_unreadMessageCount;

				_messageCountElement.InnerHTML = StringUtility.GetNumberText(_unreadMessageCount,
					BaseRes.UnreadMessageCount_Msg1, BaseRes.UnreadMessageCount_Msg2, BaseRes.UnreadMessageCount_Msg3);
			}
		}

		private static Element GetMessageElement(MessageRegisterEventArgs e)
		{
			Element container = Document.CreateElement("div");
			container.ClassName = "message";

			Element icon = Document.CreateElement("div");
			string className = "messageIcon ";
			if (e.Type == MessageType.Info)
				className += "info";
			else if (e.Type == MessageType.Warn)
				className += "warn";
			else if (e.Type == MessageType.Error)
				className += "error";

			icon.ClassName = className;
			container.AppendChild(icon);

			Element timestamp = Document.CreateElement("div");
			timestamp.ClassName = "messageTimestamp";
			timestamp.InnerHTML = Date.Now.Format("H:i:s");
			container.AppendChild(timestamp);

			Element messageCaption = Document.CreateElement("div");
			messageCaption.ClassName = "messageCaption";

			if (!Script.IsNullOrUndefined(e.MessageCaption))
			{
				messageCaption.InnerHTML = e.MessageCaption;

				if (!Script.IsNullOrUndefined(e.Message))
					messageCaption.InnerHTML += ": " + e.Message;
			}
			else
				messageCaption.InnerHTML = e.Message;

			container.AppendChild(messageCaption);

			if (!string.IsNullOrEmpty(e.Details))
			{
				Element messageDetails = Document.CreateElement("div");
				messageDetails.ClassName = "messageDetails";
				messageDetails.InnerHTML = e.Details;
				container.AppendChild(messageDetails);
			}

			return container;
		}

		private void ClearMessageCounter()
		{
			_unreadMessageCount = 0;

			if (_messageCountElement != null)
				_messageCountElement.InnerHTML = string.Empty;
		}

		private const int MaxMessageCount = 50;

		private readonly Panel _panel;
		private Region _region;
		private Element _containerDiv;

		private int _unreadMessageCount;
		private Element _messageCountElement;
	}
}