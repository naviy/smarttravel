using System;
using System.Collections;

using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI.AutoControls;
using Luxena.Travel.Services;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	partial class TaskListTab
	{

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Number,
				se.Subject,
				se.Description,
				se.RelatedTo,
				se.Order,
				se.AssignedTo,
				se.Status,
				se.DueDate,
				se.Overdue,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			_actionStatus = GetOperationStatus(ChangeStatusActionName);
			if (!_actionStatus.Visible) return;
			
			_changeStatusAction = MenuAction(DomainRes.Common_Status.ToLowerCase(), new object[]
			{
				MenuItem(DomainRes.TaskStatus_Open, delegate { ChangeStatus(TaskStatus.Open, DomainRes.TaskStatus_Open); }),
				MenuItem(DomainRes.TaskStatus_InProgress, delegate { ChangeStatus(TaskStatus.InProgress, DomainRes.TaskStatus_InProgress); }),
				MenuItem(DomainRes.TaskStatus_WaitForResponse, delegate { ChangeStatus(TaskStatus.WaitForResponse, DomainRes.TaskStatus_WaitForResponse); }),
				MenuItem(DomainRes.TaskStatus_Closed, delegate { ChangeStatus(TaskStatus.Closed, DomainRes.TaskStatus_Closed); }),
			});
		}

		protected override void OnSelectionChange(AbstractSelectionModel selectionModel)
		{
			if (Script.IsValue(_changeStatusAction))
				_changeStatusAction.setDisabled(((RowSelectionModel)selectionModel).getCount() == 0);
		}


		private void ChangeStatus(TaskStatus status, string statusStr)
		{
			if (AutoGrid.SelectionModel.getCount() == 0)
				return;

			Record[] selections = (Record[]) AutoGrid.SelectionModel.getSelections();

			object[] ids = new object[selections.Length];

			for (int i = 0; i < selections.Length; i++)
				ids[i] = selections[i].id;

			TaskService.ChangeStatus(
				ids, status, 
				(RangeRequest) AutoGrid.getStore().baseParams,
				delegate(object result)
				{
					RangeResponse response = (RangeResponse)((object[])result)[1];

					((WebServiceProxy)AutoGrid.getStore().proxy).SetResponse(response);

					AutoGrid.Reload(false);

					Array data = (Array) ((object[]) result)[0];

					string message;

					if (data.Length == 1)
						message = string.Format(Res.Task_StatusChange_Msg, statusStr, Type.GetField(data[0], ObjectPropertyNames.Reference));
					else
					{
						message = StringUtility.GetNumberText(data.Length, Res.Task_StatusChange_Msg1, Res.Task_StatusChange_Msg2,
							Res.Task_StatusChange_Msg2);

						message = string.Format(Res.Task_StatusChange_Msg, statusStr, message);
					}

					if (Script.IsNullOrUndefined(response.SelectedRow))
						MessageRegister.Info(DomainRes.Task_Caption_List, message, BaseRes.AutoGrid_NotDisplay_Msg);
					else
					{
						MessageRegister.Info(DomainRes.Task_Caption_List, message);

						AutoGrid.SelectionModel.selectRow(response.SelectedRow);
					}

				}, null
			);
		}

		private Action _changeStatusAction;
		private OperationStatus _actionStatus;

		public const string ChangeStatusActionName = "ChangeTaskStatus";

	}

}