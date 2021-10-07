using System.Collections;

using Ext;
using Ext.data;
using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using Luxena.Travel.Controls;

using Action = Ext.Action;
using ActionConfig = Ext.ActionConfig;


namespace Luxena.Travel
{
	public class AirlineListTab : AutoListTabExt
	{
		public AirlineListTab(string tabId, ListArgs args) : base(tabId, args)
		{
		}

		public static void ListObject(ListArgs args, bool newTab)
		{
			Tabs.Open(newTab, args.Type, delegate(string tabId) { return new AirlineListTab(tabId, args); }, args.BaseRequest);
		}

		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			_applyAirlineStatus = GetOperationStatus(ApplyAirlineActionName);

			if (!_applyAirlineStatus.Visible)
				return;

			_applyAirlineAction = new Action(new ActionConfig()
				.text(Res.ApplyToDocuments_Text)
				.handler(new AnonymousDelegate(Apply))
				.disabled(_applyAirlineStatus.IsDisabled)
				.custom("tooltip", _applyAirlineStatus.DisableInfo)
				.ToDictionary());

			toolbarItems.Add(new ToolbarSeparator());
			toolbarItems.Add(_applyAirlineAction);
		}

		protected override void OnSelectionChange(AbstractSelectionModel model)
		{
			if (!_applyAirlineStatus.Visible || _applyAirlineStatus.IsDisabled)
				return;

			if (((RowSelectionModel)model).getCount() == 1)
				_applyAirlineAction.enable();
			else
				_applyAirlineAction.disable();
		}

		private void Apply()
		{
			Record[] records = (Record[]) AutoGrid.SelectionModel.getSelections();

			ArrayList ids = new ArrayList();

			for (int i = 0; i < records.Length; i++)
				ids.Add(records[i].id);

			SetDataToDocumentsForm form = new SetDataToDocumentsForm(Res.ApplyAirline_Title, "Airline");
			form.Saved +=
				delegate(object result)
				{
					if ((int)result == 0)
						return;

					string msg = StringUtility.GetNumberText((int)result, Res.AppliedDataToDocuments_Text1, Res.AppliedDataToDocuments_Text2, Res.AppliedDataToDocuments_Text3);

					MessageRegister.Info(Res.ApplyAirline_Title, msg);
				};

			form.Open((object[]) ids);
		}

		private Action _applyAirlineAction;
		private OperationStatus _applyAirlineStatus;

		public const string ApplyAirlineActionName = "ApplyAirlineToDocuments";
	}
}