using System.Runtime.CompilerServices;

using Ext;

using jQueryApi;


namespace LxnBase.UI
{
	public class Tab : Panel, IKeyHandler
	{
		[AlternateSignature]
		public extern Tab();

		public Tab(object config, string tabIdentifier) : base(config)
		{
			_tabIdentifier = tabIdentifier;
		}

		protected override void initComponent()
		{
			base.initComponent();

			bool isFirst = true;

			on("activate", new AnonymousDelegate(delegate
			{
				OnActivate(isFirst);

				isFirst = false;
			}));

			on("deactivate", new AnonymousDelegate(OnDeactivate));
		}

		public string TabIdentifier
		{
			get { return _tabIdentifier; }
			set { _tabIdentifier = value; }
		}

		public virtual void RestoreFocus()
		{
		}

		public virtual bool HandleKeyEvent(jQueryEvent keyEvent)
		{
			return false;
		}

		public void Close()
		{
			Tabs.Close(this);
		}

		public virtual void BeforeActivate(object @params)
		{
		}

		protected virtual void OnActivate(bool isFirst)
		{
		}

		protected virtual void OnDeactivate()
		{
		}

		private string _tabIdentifier;
	}
}