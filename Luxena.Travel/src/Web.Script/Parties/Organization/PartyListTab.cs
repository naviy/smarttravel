using System;
using System.Collections;

using Ext;
using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Action = Ext.Action;
using Record = Ext.data.Record;



namespace Luxena.Travel
{

	public abstract class PartyListTab : EntityListTab
	{
		protected PartyListTab(string tabId, ListArgs args) : base(tabId, args) {}


		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			_replaceActionStatus = GetOperationStatus("PartyReplace");
			if (!_replaceActionStatus.Visible) return;

			//_replaceAction = Action(
			//	"Заменить",
			//	Replace,
			//	_replaceActionStatus.IsDisabled,
			//	_replaceActionStatus.DisableInfo
			//);

			//toolbarItems.Add(new ToolbarSeparator());
			//toolbarItems.Add(_replaceAction);
		}


		private void Replace()
		{
			Record record = ((Record[])AutoGrid.SelectionModel.getSelections())[0];

			FormsRegistry.EditObject("PartyReplace", null, new Dictionary("FromParty", record.id), null, null);
		}

		protected override void OnSelectionChange(AbstractSelectionModel model)
		{
			if (Script.IsValue(_replaceAction) && !_replaceActionStatus.IsDisabled)
				_replaceAction.setDisabled(((RowSelectionModel)model).getCount() != 1);
		}

		private Action _replaceAction;
		private OperationStatus _replaceActionStatus;

	}

}