using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.util;

using jQueryApi;


namespace LxnBase.UI
{
	public delegate Tab TabCreateCallback(string tabId);

	public static class Tabs
	{
		public static void Init(Dictionary config)
		{
			config["deferredRender"] = false;
			config["activeTab"] = 0;
			config["enableTabScroll"] = true;
			config["minTabWidth"] = 100;

			_panel = new TabPanel(config);

			EventsManager.RegisterKeyDownHandler(typeof (Tabs), false);
		}

		public static Component Widget
		{
			get { return _panel; }
		}

		[AlternateSignature]
		public static extern void Open(bool newTab, string tabId, TabCreateCallback callback);

		public static void Open(bool newTab, string tabIdentifier, TabCreateCallback callback, object onActivateParams)
		{
			Tab tab = null;

			if (!newTab)
				tab = FindTab(tabIdentifier);

			if (tab != null)
			{
				tab.BeforeActivate(onActivateParams);

				_panel.setActiveTab(tab.id);
			}
			else
			{
				tab = callback(tabIdentifier);

				if (tab != null)
				{
					_panel.add(tab);

					_panel.setActiveTab(tab.id);
				}
			}
		}

		public static bool HandleKeyEvent(jQueryEvent keyEvent)
		{
			int key = keyEvent.Which;

			bool isAltKey = !keyEvent.CtrlKey && !keyEvent.ShiftKey && keyEvent.AltKey;
			bool isCtrlKey = keyEvent.CtrlKey && !keyEvent.ShiftKey && !keyEvent.AltKey;

			if (key == (int) Type.GetField(typeof (EventObject), "LEFT") && isAltKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				MixedCollection tabs = _panel.items;

				double count = tabs.getCount();
				double pos = tabs.indexOf(_panel.getActiveTab()) - 1;

				if (pos < 0)
					pos = count - 1;

				_panel.setActiveTab(pos);

				return true;
			}

			if (key == (int) Type.GetField(typeof (EventObject), "RIGHT") && isAltKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				MixedCollection tabs = _panel.items;

				double count = tabs.getCount();
				double pos = tabs.indexOf(_panel.getActiveTab()) + 1;

				if (pos == count)
					pos = 0;

				_panel.setActiveTab(pos);

				return true;
			}

			if (key == (int) Type.GetField(typeof (EventObject), "W") && isCtrlKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				Panel tab = (Panel)_panel.getActiveTab();

				if ((bool) Type.GetField(tab, "closable"))
					Close(tab);

				return true;
			}

			if (isAltKey &&
				(key >= (int) Type.GetField(typeof (EventObject), "NUM_ZERO") && key <= (int) Type.GetField(typeof (EventObject), "NUM_NINE")
					|| key >= (int) Type.GetField(typeof (EventObject), "ZERO") && key <= (int) Type.GetField(typeof (EventObject), "NINE")))
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				MixedCollection tabs = _panel.items;
				double count = tabs.getCount();

				int zeroKey = key <= (int) Type.GetField(typeof (EventObject), "NINE") ? (int) Type.GetField(typeof (EventObject), "ZERO") : (int) Type.GetField(typeof (EventObject), "NUM_ZERO");
				double pos = key == zeroKey ? 9 : key - zeroKey - 1;

				if (pos < count)
					_panel.setActiveTab(pos);

				return true;
			}

			Tab activeTab = (Tab) _panel.activeTab;

			activeTab.HandleKeyEvent(keyEvent);

			return false;
		}

		public static Tab FindTab(string tabId)
		{
			MixedCollection tabs = _panel.items;

			for (int i = 0; i < tabs.getCount(); i++)
			{
				Tab tab = (Tab) tabs.itemAt(i);
				if (tab.TabIdentifier == tabId)
					return tab;
			}

			return null;
		}

		public static void Close(Component tab)
		{
			_panel.remove(tab);
		}

		private static TabPanel _panel;
	}
}