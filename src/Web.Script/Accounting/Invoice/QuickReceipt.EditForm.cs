using System.Collections;

using Ext;
using Ext.form;

using LxnBase;
using LxnBase.UI;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using ComboBox = LxnBase.UI.Controls.ComboBox;


namespace Luxena.Travel
{
	public class QuickReceiptEditForm : BaseEditForm
	{
		public void Open()
		{
			Button saveButton = Form.addButton(Res.Create, new AnonymousDelegate(Save));
			Button cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			_customerSelector = ControlFactoryExt.CreateCustomerControl(DomainRes.Common_Customer, 200, false);

			_notaxedAmount = new MoneyControl(MoneyControlConfig.DefaultConfig(Res.QuickReceipt_AviaTickets).SetAllowBlank(false));
			_taxedAmount = new MoneyControl(MoneyControlConfig.DefaultConfig(Res.QuickReceipt_ServiceFee));

			_note = new TextArea(new TextAreaConfig()
				.fieldLabel(DomainRes.Common_Note)
				.height(80)
				.width(300)
				.ToDictionary());

			_owner = ControlFactoryExt.CreateOwnerControl(200);

			ArrayList list = new ArrayList();

			list.Add(_customerSelector.Widget);
			list.Add(_notaxedAmount);
			list.Add(_taxedAmount);
			list.Add(_owner);
			list.Add(_note);

			Form.add(list);

			list.Remove(_note);

			list.Add(saveButton);
			list.Add(cancelButton);

			ComponentSequence = (Component[])list;

			Form.labelWidth = 120;

			Window.setTitle(Res.QuickReceipt_Title);
			Window.setWidth(500);
			Window.addClass("quick-receipt");

			Window.show();
		}

		protected override void OnSave()
		{
			string note = (string) _note.getValue();

			QuickReceiptRequest request = new QuickReceiptRequest();

			request.Customer = _customerSelector.GetObjectInfo();
			request.NotaxedAmount = _notaxedAmount.getValue();
			request.TaxedAmount = _taxedAmount.getValue();
			request.Owner = _owner.GetObjectInfo();
			request.Note = string.IsNullOrEmpty(note) ? null : note;

			OrderService.CreateQuickReceipt(request,
				delegate(object result)
				{
					QuickReceiptResponse response = (QuickReceiptResponse) result;

					MessageFactory.ObjectUpdatedMsg(DomainRes.Order, ObjectLink.RenderInfo(response.Order), true);

					CompleteSave(result);
				},
				null);
		}

		private ObjectSelector _customerSelector;
		private MoneyControl _notaxedAmount;
		private MoneyControl _taxedAmount;
		private ComboBox _owner;
		private TextArea _note;
	}
}