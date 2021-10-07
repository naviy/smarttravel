using System;


namespace LxnBase.UI
{
	//TODO: not in use now, restore or remove class
	public class LxnMenu : Ext.menu.Menu
	{
		public LxnMenu(object config) : base(config)
		{
			//addListener("beforeshow", new MenuBeforeshowDelegate(delegate(Component menu) { EventsManager.RegisterKeyDownHandler(menu, false); }));

			//addListener("beforehide", new AnonymousDelegate(EventsManager.UnregisterKeyDownHandler));
		}

		public void TryActivateItem(int itemIndex)
		{
			Type.InvokeMethod(this, "tryActivate", itemIndex, itemIndex + 1);
		}
	}
}