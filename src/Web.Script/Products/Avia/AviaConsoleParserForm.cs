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



	//===g





	public class AviaConsoleParserForm : BaseEditForm
	{

		//---g



		private TextArea _contentField;
		private ComboBox _owner;
		private Reference _lastOwner;
		private ObjectSelector _seller;



		//---g



		public void Open()
		{

			Window.setTitle(Res.AviaConsoleParserForm_Title);
			Window.addClass("avia-console-parser");
			Window.setSize(800, 780);


			Form.add(new Label(new LabelConfig()
				.html(
@"Для создания авиа-документов на основании данных из терминала систем Amadeus, Galileo и Sabre:<br/>
<br/>
1) в терминале системы поочерёдно введите следующие команды:<br/>
   - для системы <b>Amadeus</b>: rt[номер бронировки] и tqt; в случае, если просчётов несколько - tqt/T1, tqt/T2 ... соответственно; <br/>
<br/>
2) полностью скопируйте с экрана терминала текст данных комманд в буфер обмена;<br/>
<br/>
3) вставте скопированный текст в поле ниже.<br/>
<br/>"
				)
				.ToDictionary()
			));


			_contentField = new TextArea(new TextAreaConfig()
				.width(756)
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



		//---g

	}






	//===g



}