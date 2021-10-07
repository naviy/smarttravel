using LxnBase.UI;


namespace Luxena.Travel
{

	public class IdentityListTab : Entity3DListTab
	{

		static IdentityListTab()
		{
			RegisterList("Identity", typeof(IdentityListTab));
		}

		public IdentityListTab(string tabId, ListArgs args) : base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Identity;
		}

		private IdentitySemantic se;

	}


	public class IdentityEditForm : Entity3DEditForm
	{

		static IdentityEditForm()
		{
			RegisterEdit("Identity", typeof(IdentityEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Identity;
		}

		private IdentitySemantic se;

	}

}

