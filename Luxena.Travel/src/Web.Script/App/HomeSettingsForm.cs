using System;
using System.Collections;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.UI;


namespace Luxena.Travel
{
	public class HomeSettingsForm : BaseEditForm
	{
		public HomeSettingsForm()
		{
			Window.cls += " home-settings-form";

			Window.setTitle(Res.HomeSettingsForm_Title);

			Form.labelWidth = 150;

			CreateFormItems();

			_applyButton = Form.addButton(Res.Apply, new System.Action(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new System.Action(Cancel));

			Fields = new Field[] { _fromField, _toField };

			ComponentSequence = new Component[]
			{
				_fromField,
				_toField,
				_applyButton,
				_cancelButton
			};
		}

		public void Open()
		{
			Window.show();
		}

		public Date From
		{
			get { return _fromField.getValue(); }
			set { _fromField.setValue(value); }
		}

		public Date To
		{
			get { return _toField.getValue(); }
			set { _toField.setValue(value); }
		}

		private void CreateFormItems()
		{
			_fromField = new DateField(new DateFieldConfig()
				.fieldLabel(Res.HomeSettingsForm_Reports)
				.format("d.m.Y")
				.ToDictionary());

			_toField = new DateField(new DateFieldConfig()
				.hideLabel(true)
				.format("d.m.Y")
				.ToDictionary());

			Component separator = new BoxComponent(new BoxComponentConfig()
				.autoEl(new Dictionary("tag", "div", "html", "&nbsp;-&nbsp;"))
				.cls("x-form-item float-left box-label")
				.ToDictionary());

			Panel panel = new Panel(new PanelConfig()
				.layout("form")
				.itemCls("float-left")
				.items(new Component[] { _fromField, separator, _toField })
				.ToDictionary());

			Form.add(panel);
		}

		protected override bool OnValidate()
		{
			if ((string) _fromField.getRawValue() != "" && (string) _toField.getRawValue() != "")
			{
				int days = (_toField.getValue() - _fromField.getValue())/MsecsInDay + 1;

				if (days > 10)
				{
					_fromField.markInvalid(Res.HomeSettingsForm_IntervalTooBig);
					return false;
				}

				if (days < 0)
				{
					_fromField.markInvalid(Res.HomeSettingsForm_IntervalTooSmall);
					return false;
				}
			}

			return true;
		}

		protected override void OnSave()
		{
			CompleteSave(null);
		}

		private const int MsecsInDay = 1000*60*60*24;

		private DateField _fromField;
		private DateField _toField;
		private readonly Button _applyButton;
		private readonly Button _cancelButton;
	}
}