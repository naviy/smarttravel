using System;

using Ext;
using Ext.form;

using Luxena.Travel.Controls;

using LxnBase;
using LxnBase.UI;

using Luxena.Travel.Services;

using LxnBase.Data;
using LxnBase.UI.Controls;

using ComboBox = LxnBase.UI.Controls.ComboBox;


namespace Luxena.Travel
{
	public class AviaConsoleParserForm : BaseEditForm
	{

		public void Open()
		{
			Window.setTitle(Res.AviaConsoleParserForm_Title);
			Window.addClass("avia-console-parser");
			Window.setSize(800, 780);

			Form.add(new Label(new LabelConfig().html(Res.AviaConsoleParserForm_Label1).ToDictionary()));

			_contentField = new TextArea(new TextAreaConfig()
				.width(760)
				.height(460)
				.hideLabel(true)
				.allowBlank(false)
				.style("font-family: Consolas, Courier New, courier")
				.ToDictionary()
			);

			_seller = ControlFactoryExt.CreateAssignedToControl(DomainRes.Common_Seller, 200, false);
			_seller.SetValue(AppManager.CurrentPerson);

			_owner = ControlFactoryExt.CreateOwnerControl(200);
			if (Script.IsValue(_lastOwner))
				_owner.setValue(_lastOwner);

			Form.add(_contentField);
			Form.add(_seller.Widget);
			Form.add(_owner);

			Button _saveButton = Form.addButton(Res.AviaConsoleParserForm_Parse, new AnonymousDelegate(Save));
			Button _cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			ComponentSequence = new Component[] { _saveButton, _cancelButton };

			Window.show();
		}

		protected override bool OnValidate()
		{
			return !string.IsNullOrEmpty(_contentField.getValue().ToString());
		}

		protected override void OnSave()
		{
			_lastOwner = _owner.GetObjectInfo();

			AviaService.AddDocumentsByConsoleContent(
				(string)_contentField.getValue(),
				(string)_seller.GetObjectId(),
				(string)_owner.GetSelectedId(),
				delegate (object result)
				{
					Reference[] docrRefs = (Reference[])result;
					foreach (Reference doc in docrRefs)
					{
						FormsRegistry.ViewObject(doc.Type, doc.Id);
					}
					Window.close();
				},
				null
			);
		}

		private TextArea _contentField;
		private ComboBox _owner;
		private Reference _lastOwner;
		private ObjectSelector _seller;

	}
}