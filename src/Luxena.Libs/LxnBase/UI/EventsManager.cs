using System;
using System.Collections;
using System.Html;
using System.Runtime.CompilerServices;

using jQueryApi;

using Element = System.Html.Element;


namespace LxnBase.UI
{
	public static class EventsManager
	{
		static EventsManager()
		{
			_handlerList = new ArrayList();

			jQuery.Document.Keydown(OnKeyDown);
		}

		public static void RegisterKeyDownHandler(object handler, bool allowBubbling)
		{
			if (_handlerList.Count > 0)
			{
				KeyEventHandler eventHandler = ((KeyEventHandler) _handlerList[_handlerList.Count - 1]);
				eventHandler.FocusedElement = Document.ActiveElement;
			}
			_handlerList.Add(new KeyEventHandler((IKeyHandler) handler, allowBubbling));
		}

		[AlternateSignature]
		public static extern void UnregisterKeyDownHandler();

		public static void UnregisterKeyDownHandler(object handler)
		{
			if (Script.IsNullOrUndefined(handler))
				_handlerList.RemoveAt(_handlerList.Count - 1);
			else
			{
				for (int i = _handlerList.Count - 1; i >= 0; i--)
					if (((KeyEventHandler) _handlerList[i]).Handler == handler)
					{
						_handlerList.RemoveAt(i);
						break;
					}
			}

			if (_handlerList.Count > 0)
			{
				Element element = ((KeyEventHandler) _handlerList[_handlerList.Count - 1]).FocusedElement;

				if (element != null)
					element.Focus();
			}
		}

		private static void OnKeyDown(jQueryEvent e)
		{
			for (int i = _handlerList.Count - 1; i >= 0; i--)
			{
				KeyEventHandler eventHandler = (KeyEventHandler) _handlerList[i];

				IKeyHandler handler = eventHandler.Handler;

				if ((bool) Type.GetField(handler, "handleKeyEvent") && handler.HandleKeyEvent(e) || !eventHandler.AllowBubbling)
					break;
			}
		}

		private static readonly ArrayList _handlerList;
	}
}