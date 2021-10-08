using System;
using System.Collections;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.UI;

using Luxena.Travel.Services;


namespace Luxena.Travel
{

	public class SetDataToDocumentsForm : BaseEditForm
	{
		public SetDataToDocumentsForm(string title, string type)
		{
			_type = type;

			Window.cls += " apply-to-documents";

			Window.setTitle(title);

			CreateFormItems();

			_applyReportButton = Form.addButton(Res.Apply_Text, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			Fields = new Field[]
			{
				_dateFrom,
				_dateTo
			};

			ComponentSequence = new Component[]
			{
				_dateFrom,
				_dateTo,
				_applyReportButton,
				_cancelButton
			};
		}

		public void Open(object[] ids)
		{
			_ids = ids;

			CheckDocumentCount();

			Window.show();
		}

		private void CreateFormItems()
		{
			BoxComponent text = new BoxComponent(new BoxComponentConfig()
				.autoEl(new Dictionary("tag", "div", "html", Res.ApplyAirline_SelectDates_Text))
				.cls("x-form-item box-label")
				.ToDictionary());

			_dateFrom = new DateField(new DateFieldConfig()
				.fieldLabel(Res.FromDate_Text)
				.value(Date.Today)
				.format("d.m.Y")
				.listeners(new Dictionary("change", new FieldChangeDelegate(OnDateChange)))
				.ToDictionary());

			_dateTo = new DateField(new DateFieldConfig()
				.fieldLabel(Res.ToDate_Text)
				.value(Date.Today)
				.format("d.m.Y")
				.listeners(new Dictionary("change", new FieldChangeDelegate(OnDateChange)))
				.ToDictionary());

			_totalCountBox = new BoxComponent(new BoxComponentConfig()
				.autoEl(new Dictionary("tag", "div", "html", string.Format(Res.CustomerReportForm_DocumentCount_Text, 0)))
				.cls("total-count")
				.ToDictionary());

			Form.add(text);
			Form.add(_dateFrom);
			Form.add(_dateTo);
			Form.add(_totalCountBox);
		}

		private void OnDateChange(Field field, object newvalue, object oldvalue)
		{
			if (field == _dateFrom && _dateFrom.getValue() > _dateTo.getValue())
				_dateFrom.setValue(_dateTo.getValue());

			else if (field == _dateTo && _dateTo.getValue() < _dateFrom.getValue())
				_dateTo.setValue(_dateFrom.getValue());

			CheckDocumentCount();
		}

		private void CheckDocumentCount()
		{
			AviaService.GetDocumentCountForUpdate(_type, _ids, GetDate(_dateFrom), GetDate(_dateTo),
				delegate(object result)
				{
					_totalCountBox.getEl().update(string.Format(Res.CustomerReportForm_DocumentCount_Text, (int) result));

				}, null);
		}

		protected override void OnSave()
		{
			AviaService.ApplyDataToDocuments(_type, _ids, GetDate(_dateFrom), GetDate(_dateTo),
				delegate(object result)
				{
					int count = (int) result;

					if (count == 0)
						MessageRegister.Info(Res.ApplyAirline_Msg_Title, Res.AirlineApplied_NotFound_Msg);
					CompleteSave(result);
				}, null);
		}

		private static object GetDate(DateField field)
		{
			object value = field.getValue();

			if ((string)value == "")
				return null;

			return value;
		}

		private readonly Button _applyReportButton;
		private readonly Button _cancelButton;

		private DateField _dateFrom;
		private DateField _dateTo;
		private BoxComponent _totalCountBox;

		private object[] _ids;
		private readonly string _type;
	}

}