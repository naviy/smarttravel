using System;
using System.Collections;

using LxnBase.UI.AutoControls;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.UI;

using Field = Ext.form.Field;


namespace Luxena.Travel.Reports
{
	public class UnbalancedReportForm : BaseEditForm
	{
		public UnbalancedReportForm()
		{
			Window.cls += " unbalanced-report";

			Window.setTitle(Res.UnbalancedReport);

			CreateFormItems();

			_createReportButton = Form.addButton(Res.Common_CreateReport, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			Fields = new Field[]
			{
				_toField,
				_includeOrders
			};

			ComponentSequence = new Component[]
			{
				_toField,
				_includeOrders,
				_createReportButton,
				_cancelButton
			};
		}

		public void Open()
		{
			Window.show();
		}

		private void CreateFormItems()
		{
			_toField = new DateField(new DateFieldConfig()
				.fieldLabel(Res.ToDate_Text)
				.format("d.m.Y")
				.ToDictionary());

			_includeOrders = new Checkbox(new CheckboxConfig()
				.boxLabel(Res.UnbalancedReportForm_IncludeOrders)
				.ToDictionary());

			Form.add(_toField);
			Form.add(_includeOrders);
		}

		protected override void OnSave()
		{
			ReportLoader.Load(string.Format("reports/unbalanced/Unbalanced_Report_{0}.xls", Date.Now.Format("Y-m-d_H-i")), new Dictionary(
				"to", ((string) ((object) _toField.getValue()) == "") ? null : _toField.getValue().LocaleFormat("yyyy-MM-dd"),
				"includeOrders", _includeOrders.getValue()
			));

			CompleteSave(null);
		}

		private DateField _toField;
		private Checkbox _includeOrders;
		private readonly Button _createReportButton;
		private readonly Button _cancelButton;
	}
}