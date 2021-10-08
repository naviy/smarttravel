using System;
using System.Collections;

using Ext;
using Ext.util;

using LxnBase.Data;
using LxnBase.Services;

using Action = Ext.Action;
using ItemConfig = LxnBase.Services.ItemConfig;


namespace LxnBase.UI
{
	public abstract class BaseClassViewForm : Tab
	{
		protected BaseClassViewForm(string tabId, object id, string type)
			: base(new PanelConfig()
				.closable(true)
				.autoScroll(true)
				.layout("fit")
				.title(BaseRes.Loading)
				.cls("autoView")
				.bodyCssClass("view-body")
				.ToDictionary(), tabId)
		{
			_type = type;
			_id = id;

			ConfigManager.GetViewConfig(type,
				delegate(ItemConfig config)
				{
					_itemConfig = config;

					SetActionStatus(_copyButton, _itemConfig.IsCopyingAllowed);
					SetActionStatus(_editButton, _itemConfig.IsEditAllowed);
					SetActionStatus(_deleteButton, _itemConfig.IsRemovingAllowed);

					OnLoadConfig();

					GetInstance();
				});
		}

		protected override void initComponent()
		{
			_editButton = new Action(new ActionConfig()
				.text(BaseRes.Edit_Lower)
				.handler(new AnonymousDelegate(Edit))
				.hidden(true)
				.ToDictionary());

			_copyButton = new Action(new ActionConfig()
				.text(BaseRes.Copy_Lower)
				.handler(new AnonymousDelegate(Copy))
				.hidden(true)
				.ToDictionary());

			_deleteButton = new Action(new ActionConfig()
				.text(BaseRes.Remove_Lower)
				.handler(new AnonymousDelegate(DeleteInstance))
				.hidden(true)
				.ToDictionary());

			ArrayList list = new ArrayList();
			list.AddRange(new object[] { _editButton, _copyButton, _deleteButton });

			OnInitToolBar(list);

			tbar = new Toolbar(list);

			base.initComponent();
		}

		protected virtual void OnInitToolBar(ArrayList toolbarItems)
		{
		}

		protected object Id
		{
			get { return _id; }
		}

		protected string InstanceType
		{
			get { return _type; }
		}

		protected object Instance
		{
			get { return _instance; }
		}

		protected OperationPermissions Permissions
		{
			get { return _permissions; }
		}

		protected ItemConfig ItemConfig
		{
			get { return _itemConfig; }
		}

		protected virtual void OnLoadConfig()
		{
		}

		protected abstract void GetInstance();

		protected void Load(object result)
		{
			_instance = result;

			UpdatePermissions();

			OnLoad();
		}

		protected void Refresh(object result)
		{
			if (result == null)
				return;

			_instance = result;

			UpdatePermissions();

			OnRefresh();
		}

		protected virtual void UpdateActionsStatus()
		{
			SetActionStatus(_copyButton, _itemConfig.IsCopyingAllowed);
			SetActionStatus(_editButton, _permissions.CanUpdate ?? _itemConfig.IsEditAllowed);
			SetActionStatus(_deleteButton, _permissions.CanDelete ?? _itemConfig.IsRemovingAllowed);

			SetToolbarVisibility();
		}

		protected abstract void OnLoad();

		protected virtual void OnRefresh()
		{
			body.clean();

			OnLoad();
		}

		protected void Edit()
		{
			OnEdit();
		}

		protected virtual void OnEdit()
		{
			FormsRegistry.EditObject(_type, _id, null,
				delegate(object result)
				{
					ItemResponse response = (ItemResponse) result;

					Load(Script.IsNullOrUndefined(response.Item) ? result : response.Item);
				},
				null);
		}

		protected void Copy()
		{
			OnCopy();
		}

		protected virtual void OnCopy()
		{
			Dictionary dictionary = new Dictionary();

			foreach (ColumnConfig t in _itemConfig.Columns)
				dictionary[t.Name] = Type.GetField(_instance, t.Name);

			FormsRegistry.EditObject(_type, null, dictionary,
				delegate(object result)
				{
					string objId = (string) Type.GetField(((ItemResponse) result).Item, ObjectPropertyNames.Id);

					FormsRegistry.ViewObject(_type, objId);
				},
				null);
		}

		protected void DeleteInstance()
		{
			OnDeleteInstance();
		}

		protected virtual void OnDeleteInstance()
		{
			MessageBoxWrap.Confirm(BaseRes.Confirmation, BaseRes.Delete_Confirmation,
				delegate(string button, string text)
				{
					if (button != "yes")
						return;

					GenericService.Delete(_type, new object[] { _id }, null,
						delegate(object result)
						{
							DeleteOperationResponse response = (DeleteOperationResponse) result;

							if (response.Success)
							{
								Close();

								MessageRegister.Info(_itemConfig.ListCaption, BaseRes.Deleted + " " + title);
							}
							else
								OnDeleteFailed();
						}, null);
				});
		}

		protected override void OnActivate(bool isFirst)
		{
			if (!isFirst)
				GetInstance();
		}

		protected static void SetActionStatus(Action action, OperationStatus status)
		{
			if (Script.IsNullOrUndefined(status))
				return;

			action.setHidden(!status.Visible);
			action.setDisabled(status.IsDisabled);

			Type.SetField(action, "tooltip", status.DisableInfo);
		}

		protected void SetToolbarVisibility()
		{
			bool hideToolbar = true;

			MixedCollection list = (MixedCollection) Type.GetField(getTopToolbar(), "items");

			double count = list.getCount();
			bool isSeparator = false;
			bool isFirst = true;

			for (int i = 0; i < count; i++)
			{
				ToolbarItem item = (ToolbarItem) list.itemAt(i);

				if (item is ToolbarSeparator)
				{
					if (i == 0 || i == count - 1 || isSeparator || isFirst)
						item.setVisible(false);

					isSeparator = true;
				}
				else
					isSeparator = false;

				if (!item.hidden)
					isFirst = false;
			}

			for (int i = 0; i < count; i++)
			{
				if (!((Component) list.itemAt(i)).hidden)
					hideToolbar = false;
			}

			if (hideToolbar)
				getTopToolbar().setVisible(false);
		}

		protected OperationStatus GetCustomActionStatus(string actionName)
		{
			Dictionary permissions = ItemConfig.CustomActionPermissions;
			if (permissions != null && permissions.ContainsKey(actionName))
				return (OperationStatus) permissions[actionName];

			OperationStatus status = new OperationStatus();
			status.Visible = false;

			return status;
		}

		private void UpdatePermissions()
		{
			object permissions = Type.GetField(_instance, "Permissions");

			if (!Script.IsNullOrUndefined(permissions))
				_permissions = (OperationPermissions) permissions;

			if (Script.IsNullOrUndefined(_permissions))
				_permissions = new OperationPermissions();

			UpdateActionsStatus();
		}

		private void OnDeleteFailed()
		{
			GenericService.CanReplace(_type, _id,
				delegate(object result)
				{
					if ((bool) result)
					{
						ReplaceForm form = new ReplaceForm(_type, _id);

						form.Saved += delegate { Close(); };

						form.Open();
					}
					else
					{
						MessageBoxWrap.Show(BaseRes.Warning,
							BaseRes.AutoGrid_DeleteConstrainedFailed_Msg + "<br/>" + BaseRes.AutoGrid_ReplaceToAdmin_Msg,
							MessageBox.WARNING, MessageBox.OK);
					}
				}, null);
		}

		private ItemConfig _itemConfig;

		protected readonly string _type;
		private readonly object _id;
		private object _instance;
		private OperationPermissions _permissions;

		private Action _editButton;
		private Action _deleteButton;
		private Action _copyButton;
	}
}
