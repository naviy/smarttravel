using System;
using System.Collections;

using Ext;
using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI.AutoControls;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public partial class GdsAgentListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Person,
				se.Origin,
				se.Code,
				se.OfficeCode,
				se.Office,
				se.Provider,
				se.LegalEntity,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			_applyActionStatus = GetOperationStatus("ApplyGdsAgentToDocuments");
			if (!_applyActionStatus.Visible) return;

			_applyAction = Action(
				Res.ApplyToDocuments_Text, 
				Apply, 
				_applyActionStatus.IsDisabled, 
				_applyActionStatus.DisableInfo
			);

			toolbarItems.Add(new ToolbarSeparator());
			toolbarItems.Add(_applyAction);
		}


		private void Apply()
		{
			Record[] records = (Record[])AutoGrid.SelectionModel.getSelections();

			ArrayList ids = new ArrayList();

			foreach (Record r in records)
				ids.Add(r.id);

			SetDataToDocumentsForm form = new SetDataToDocumentsForm(Res.ApplyGdsAgent_Title, "GdsAgent");
			form.Saved += delegate(object result)
			{
				if ((int)result == 0)
					return;

				string msg = StringUtility.GetNumberText((int)result, Res.AppliedDataToDocuments_Text1, Res.AppliedDataToDocuments_Text2, Res.AppliedDataToDocuments_Text3);

				MessageRegister.Info(Res.ApplyGdsAgent_Title, msg);
			};

			form.Open((object[])ids);
		}

		protected override void OnSelectionChange(AbstractSelectionModel model)
		{
			if (Script.IsValue(_applyAction) && !_applyActionStatus.IsDisabled)
				_applyAction.setDisabled(((RowSelectionModel)model).getCount() != 1);
		}

		private Action _applyAction;
		private OperationStatus _applyActionStatus;

	}


	public partial class GdsAgentEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -1;

			Form.add(MainDataPanel(new object[]
			{
				se.Person,
				se.Origin,
				se.Code,
				se.OfficeCode,
				se.Office,
				se.Provider,
				se.LegalEntity,
			}));
		}

	}

}