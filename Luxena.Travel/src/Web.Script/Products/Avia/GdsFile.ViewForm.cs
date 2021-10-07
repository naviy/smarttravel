using System;
using System.Collections;

using Ext;
using Ext.util;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.AutoForms;

using Luxena.Travel.Services;

using Action = Ext.Action;


namespace Luxena.Travel
{


	public class GdsFileViewForm : AutoViewForm
	{
		private GdsFileViewForm(string tabId, object id, string type)
			: base(tabId, id, type)
		{
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string) id, delegate(string tabId) { return new GdsFileViewForm(tabId, id, type); });
		}

		protected override void OnInitToolBar(ArrayList toolbarItems)
		{
			_reimportAction = new Action(new ActionConfig()
				.text(Res.GdsFile_Reimport_Text)
				.handler(new AnonymousDelegate(Reimport))
				.ToDictionary());

			_separator = new ToolbarSeparator();

			toolbarItems.Add(_separator);
			toolbarItems.Add(_reimportAction);
		}

		protected override void OnLoadConfig()
		{
			if (!Script.IsValue(ItemConfig.CustomActionPermissions)) return;

			OperationStatus status = (OperationStatus) ItemConfig.CustomActionPermissions[GdsFileListTab.ReimportActionName];

			SetActionStatus(_reimportAction, status);

			_separator.setVisible(status.Visible);

			base.OnLoadConfig();
		}

		protected override void UpdateActionsStatus()
		{
			if (!Script.IsValue(Permissions.CustomActionPermissions)) return;

			OperationStatus status = (OperationStatus) Permissions.CustomActionPermissions[GdsFileListTab.ReimportActionName];

			SetActionStatus(_reimportAction, status);

			_separator.setVisible(status.Visible);

			base.UpdateActionsStatus();
		}

		protected override void OnLoad()
		{
			ColumnConfig[] columns = ItemConfig.Columns;

			string caption = null;

			StringBuilder template = new StringBuilder();

			template.Append("<table>");

			foreach (ColumnConfig col in columns)
			{
				object val = Type.GetField(Instance, col.Name);

				if (Script.IsNullOrUndefined(val))
					continue;

				if (col.IsReference)
					caption = (string) val;

				RenderDelegate renderer = (RenderDelegate) ControlFactory.CreateRenderer(col);

				string text = (string) (renderer == null ? val : renderer.Invoke(val));

				if (col.Name == "Content")
					text = "<pre>" + Format.htmlEncode(text) + "</pre>";

				template.Append("<tr>");

				template.Append(string.Format("<td valign='top'class='gsdFileViewCaption'>{0}</td>", col.Caption + ":"));
				template.Append(string.Format("<td class='gsdFileViewValue'><pre>{0}</pre></td>", text));

				template.Append("</tr>");
			}

			template.Append("</table>");

			if (caption == null)
				caption = ItemConfig.Caption;

			setTitle(caption);

			new Template(template.ToString()).overwrite(body, Instance);

			doLayout();
		}

		private void Reimport()
		{
			GdsFileService.Reimport(new object[] { Id }, null,
				delegate(object result)
				{
					ListColumnConfig columnConfig = null;
					string message = string.Empty;

					foreach (ColumnConfig col in ItemConfig.Columns)
						if (col.Name == "ImportResult")
						{
							columnConfig = (ListColumnConfig) col;
							break;
						}

					foreach (object col in columnConfig.Items)
					{
						object[] obj = (object[]) col;

						if (obj[Reference.IdPos] == Type.GetField(result, columnConfig.Name))
						{
							message = ((string) obj[Reference.NamePos]).ToLowerCase();
							break;
						}
					}

					message = string.Format("{0} {1}. {2} - {3}.", Res.Imported, Type.GetField(result, ObjectPropertyNames.Reference), columnConfig.Caption, message);

					MessageRegister.Info(ItemConfig.ListCaption, message);
					Load(result);
				}, null);
		}


		private Action _reimportAction;
		private ToolbarSeparator _separator;
	}


}