using System.Collections;

using Ext;
using Ext.data;
using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	public partial class AirlineServiceClassListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Airline,
				se.Code,
				se.ServiceClass,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			_applyActionStatus = GetOperationStatus(ApplyServiceClassActionName);

			if (!_applyActionStatus.Visible) return;

			_applyServiceClassAction = Action(
				Res.ApplyToDocuments_Text, 
				Apply,
				_applyActionStatus.IsDisabled,
				_applyActionStatus.DisableInfo
			);

			toolbarItems.Add(new ToolbarSeparator());
			toolbarItems.Add(_applyServiceClassAction);
		}

		protected override void OnSelectionChange(AbstractSelectionModel model)
		{
			if (!_applyActionStatus.IsEnabled) return;

			_applyServiceClassAction.setDisabled(((RowSelectionModel)model).getCount() != 1);
		}

		private void Apply()
		{
			Record[] records = (Record[])AutoGrid.SelectionModel.getSelections();

			ArrayList ids = new ArrayList();

			foreach (Record r in records)
				ids.Add(r.id);

			SetDataToDocumentsForm form = new SetDataToDocumentsForm(Res.ApplyServiceClass_Title, "AirlineServiceClass");
			form.Saved +=
				delegate(object result)
				{
					if ((int)result == 0)
						return;

					string msg = StringUtility.GetNumberText((int)result, Res.AppliedDataToDocuments_Text1, Res.AppliedDataToDocuments_Text2, Res.AppliedDataToDocuments_Text3);

					MessageRegister.Info(Res.ApplyServiceClass_Title, msg);
				};

			form.Open((object[])ids);
		}

		private Action _applyServiceClassAction;
		private OperationStatus _applyActionStatus;
		public const string ApplyServiceClassActionName = "ApplyServiceClassToDocuments";

	}

}