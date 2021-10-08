using System;
using System.Collections;

using Ext;
using Ext.form;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI.Controls;


namespace LxnBase.UI
{

	public class ReplaceForm : BaseEditForm
	{

		public static void Exec(string className, object id, Component tab)
		{
			GenericService.CanReplace(className, id,
				delegate (object result)
				{
					if ((bool)result)
					{
						ReplaceForm form = new ReplaceForm(className, id);
						if (Script.IsValue(tab))
							form.Saved += delegate { Tabs.Close(tab); };
						form.Open();
					}
					else
					{
						MessageBoxWrap.Show(BaseRes.Warning,
							BaseRes.AutoGrid_DeleteConstrainedFailed_Msg + "<br/>" + BaseRes.AutoGrid_ReplaceToAdmin_Msg,
							MessageBox.WARNING, MessageBox.OK);
					}
				},
				null
			);
		}

		public ReplaceForm(string type, object id)
		{
			_type = type;
			_id = id;

			Window.setTitle(BaseRes.ReplaceDelete);
			Window.cls += " replace-form";

			Type.SetField(Form, "labelWidth", true);

			CreateFormItems();

			_replaceButton = Form.addButton(BaseRes.Replace, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			Fields = new Field[]
			{
				_replacingObjectSelector.Widget,
			};

			ComponentSequence = new Component[]
			{
				_replacingObjectSelector.Widget,
				_replaceButton,
				_cancelButton
			};
		}

		public void Open()
		{
			ConfigManager.GetViewConfig(_type,
				delegate (ItemConfig config)
				{
					_itemConfig = config;

					GenericService.GetDependencies(_type, _id, OnLoad, null);
				});
		}

		private void OnLoad(object result)
		{
			_objectForReplace = (Reference)result;

			_messageBox.autoEl = new Dictionary("html", RenderMessage());

			Window.show();
		}

		private string RenderMessage()
		{
			string text = string.Format("<b>{0}</b>", _objectForReplace.Name);

			StringBuilder builder = new StringBuilder();

			builder.Append(string.Format(BaseRes.ReplaceForm_ObjectForReplace, text));
			builder.Append("<br/><br/>");
			builder.Append(BaseRes.ReplaceForm_ReplacingObject);

			return builder.ToString();
		}

		private void CreateFormItems()
		{
			_messageBox = new BoxComponent(new BoxComponentConfig()
				.cls("x-form-item delete-message")
				.ToDictionary());

			_replacingObjectSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(_type)
				.selectOnFocus(true)
				//.allowBlank(false)
				.width(250)
				.name("replacingObject")
				.hideLabel(true)
				.labelSeparator(string.Empty)
			);

			Form.add(_messageBox);
			Form.add(_replacingObjectSelector.Widget);
		}

		protected override void OnSave()
		{
			object newId = _replacingObjectSelector.GetObjectId();

			if (Script.IsValue(newId))
				GenericService.Replace(_type, _id, newId, true, CompleteSave, null);
			else
				MessageBox.confirm("Замена", "Вы не выбрали новое значение. Все связи будут заменены на пустое значение. Вы уверены?", new AnonymousDelegate(delegate
				{
					GenericService.Replace(_type, _id, newId, true, CompleteSave, null);
				}));
		}

		protected override void OnSaved(object result)
		{
			MessageRegister.Info(
				_itemConfig.ListCaption,
				string.Format(BaseRes.ReplaceForm_ReplaceComplete_Msg, _objectForReplace.Name, _replacingObjectSelector.GetObjectName())
			);

			base.OnSaved(result);
		}

		private readonly string _type;
		private readonly object _id;

		private BoxComponent _messageBox;
		private ObjectSelector _replacingObjectSelector;

		private readonly Button _replaceButton;
		private readonly Button _cancelButton;

		private Reference _objectForReplace;

		private ItemConfig _itemConfig;
	}

}