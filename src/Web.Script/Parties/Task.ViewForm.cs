using System;
using System.Collections;

using Ext;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoForms;

using Luxena.Travel.Services;

using Action = Ext.Action;


namespace Luxena.Travel
{
	public class TaskViewForm : AutoViewForm
	{
		public TaskViewForm(string tabId, object id, string type) : base(tabId, id, type)
		{
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string) id, delegate(string tabId) { return new TaskViewForm(tabId, id, type); });
		}

		protected override void OnInitToolBar(ArrayList toolbarItems)
		{
			_changeStatusAction = new Action(new ActionConfig()
				.text(DomainRes.Common_Status.ToLowerCase())
				.custom("menu", new object[]
				{
					new Action(new ActionConfig()
						.text(DomainRes.TaskStatus_Open)
						.handler(new AnonymousDelegate(delegate { ChangeStatus(TaskStatus.Open, DomainRes.TaskStatus_Open); }))
						.ToDictionary()),
					new Action(new ActionConfig()
						.text(DomainRes.TaskStatus_InProgress)
						.handler(new AnonymousDelegate(delegate { ChangeStatus(TaskStatus.InProgress, DomainRes.TaskStatus_InProgress); }))
						.ToDictionary()),
					new Action(new ActionConfig()
						.text(DomainRes.TaskStatus_WaitForResponse)
						.handler(new AnonymousDelegate(delegate { ChangeStatus(TaskStatus.WaitForResponse, DomainRes.TaskStatus_WaitForResponse); }))
						.ToDictionary()),
					new Action(new ActionConfig()
						.text(DomainRes.TaskStatus_Closed)
						.handler(new AnonymousDelegate(delegate { ChangeStatus(TaskStatus.Closed, DomainRes.TaskStatus_Closed); }))
						.ToDictionary())
				})
				.ToDictionary());

			_applyButtonSeparator = new ToolbarSeparator();

			toolbarItems.Add(_applyButtonSeparator);
			toolbarItems.Add(_changeStatusAction);
		}

		protected override void OnLoadConfig()
		{
			OperationStatus status = (OperationStatus) (ItemConfig.CustomActionPermissions)[TaskListTab.ChangeStatusActionName];

			SetActionStatus(_changeStatusAction, status);

			_applyButtonSeparator.setVisible(status.Visible);

			base.OnLoadConfig();
		}

		protected override void UpdateActionsStatus()
		{
			OperationStatus status = (OperationStatus) Permissions.CustomActionPermissions[TaskListTab.ChangeStatusActionName];

			SetActionStatus(_changeStatusAction, status);

			_applyButtonSeparator.setVisible(status.Visible);

			base.UpdateActionsStatus();
		}

		private void ChangeStatus(TaskStatus status, string statusStr)
		{
			object[] ids = new object[] { Type.GetField(Instance, "Id") };

			TaskService.ChangeStatus(ids, status, null,
				delegate(object result)
				{
					Load(result);

					string message = string.Format(Res.Task_StatusChange_Msg, statusStr, Type.GetField(result, ObjectPropertyNames.Reference));

					MessageRegister.Info(DomainRes.Task_Caption_List, message);
				}, null);
		}

		private Action _changeStatusAction;
		private ToolbarSeparator _applyButtonSeparator;
	}
}