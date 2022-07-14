using System;
using System.Collections;

using Ext;
using Ext.grid;
using Ext.menu;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{



	//===g






	public class PaymentListTab : AutoListTabExt
	{

		//---g



		static PaymentListTab()
		{
			FormsRegistry.RegisterList(ClassNames.Payment,
				delegate(ListArgs args, bool newTab) { Tabs.Open(newTab, args.Type, delegate(string tabId) { return new PaymentListTab(tabId, args); }, args.BaseRequest); });
		}



		public PaymentListTab(string tabId, ListArgs args) : base(tabId, args)
		{
		}



		//---g



		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			args.ForcedProperties = new string[] { "IsVoid", "PostedOn" };

			if (args.BaseRequest == null)
				args.BaseRequest = new RangeRequest();

			if (args.BaseRequest.Sort == null)
			{
				args.BaseRequest.Sort = "Date";
				args.BaseRequest.Dir = "DESC";
			}

			config.view(new PaymentGridView());
		}



		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			int pos = 0;

			if (ListConfig.IsCreationAllowed.Visible)
			{
				toolbarItems.RemoveAt(pos++);


				Item createCashInOrder = new Item(new ItemConfig()
					.text(Res.Payment_CreateCashInOrder_Action)
					.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.CashInOrderPayment); }))
					.ToDictionary());

				Item createCashOutOrder = new Item(new ItemConfig()
					.text(Res.Payment_CreateCashOutOrder_Action)
					.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.CashOutOrderPayment); }))
					.ToDictionary());

				Item createNonCashPayment = new Item(new ItemConfig()
					.text(Res.Payment_CreateNonCashPayment_Action)
					.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.WireTransfer); }))
					.ToDictionary());

				Item createCheckPayment = new Item(new ItemConfig()
					.text(Res.Payment_CreateCheckPayment_Action)
					.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.CheckPayment); }))
					.ToDictionary());

				Item createCreditCardPayment = new Item(new ItemConfig()
					.text(Res.Payment_CreateElecronicPayment_Action)
					.handler(new AnonymousDelegate(delegate { CreatePayment(ClassNames.ElectronicPayment); }))
					.ToDictionary());

				Action registerPaymentButton = new Action(new ActionConfig()
					.text(Res.Payment_CreatePayment_Action)
					.custom("menu", new Menu(new MenuConfig()
						.items(new object[]
						{
							createCashInOrder,
							createCashOutOrder,
							createNonCashPayment,
							createCheckPayment,
							createCreditCardPayment
						})
						.ToDictionary()))
					.ToDictionary());

				toolbarItems.Insert(0, registerPaymentButton);
			}

			if (ListConfig.IsEditAllowed.Visible)
				++pos;

			toolbarItems.RemoveAt(pos);

			if (ListConfig.IsEditAllowed.Visible)
			{
				_voidButton = new Action(new ActionConfig()
					.text(Res.Void_Action.ToLowerCase())
					.handler(new AnonymousDelegate(VoidPayments))
					.disabled(true)
					.ToDictionary());

				_postButton = new Action(new ActionConfig()
					.text(Res.Posted_Action.ToLowerCase())
					.handler(new AnonymousDelegate(PostedPayments))
					.disabled(true)
					.ToDictionary());

				toolbarItems.Insert(0, _voidButton);
				toolbarItems.Insert(1, _postButton);
				toolbarItems.Insert(2, new ToolbarSeparator());
			}
		}



		protected override void OnSelectionChange(AbstractSelectionModel selectionModel)
		{
			RowSelectionModel model = (RowSelectionModel) selectionModel;

			Record[] records = (Record[]) model.getSelections();

			if (!ListConfig.IsEditAllowed.IsDisabled)
			{
				if (records.Length == 0)
				{
					_voidButton.disable();
					_postButton.disable();
				}
				else
				{
					bool isVoid = (bool) records[0].get("IsVoid");
					bool isPosted = false;

					bool canChange = true;

					for (int i = 0; i < records.Length; i++)
					{
						if (isVoid ^ (bool) records[i].get("IsVoid"))
							canChange = false;

						if (!Script.IsNullOrUndefined(records[i].get("PostedOn")))
							isPosted = true;
					}

					if (canChange)
					{
						_voidButton.setText(isVoid ? Res.Restore_Action.ToLowerCase() : Res.Void_Action.ToLowerCase());
						_voidButton.enable();
					}
					else
						_voidButton.disable();

					if (isVoid || isPosted)
						_postButton.disable();
					else
						_postButton.enable();
				}
			}
		}



		private void VoidPayments()
		{
			RowSelectionModel selectionModel = AutoGrid.SelectionModel;

			if (selectionModel.getSelections().Length == 0)
				return;

			Record[] records = (Record[]) selectionModel.getSelections();

			ArrayList ids = new ArrayList();

			for (int i = 0; i < records.Length; i++)
				ids.Add(records[i].id);

			PaymentService.CanUpdate((object[]) ids,
				delegate(object res)
				{
					OperationStatus status = (OperationStatus) res;

					if (!status.IsEnabled)
					{
						string msg = Script.IsNullOrUndefined(status.DisableInfo) ? BaseRes.AutoGrid_ActionNotPermitted_Msg : status.DisableInfo;

						MessageBoxWrap.Show(new Dictionary(
							"title", BaseRes.Error,
							"msg", msg,
							"icon", MessageBox.ERROR,
							"buttons", MessageBox.OK
							));
						return;
					}

					bool isVoid = (bool) Type.GetField(records[0].data, "IsVoid");

					MessageFactory.VoidingConfirmation(Res.Document_Void_Confirmation1, Res.Document_Void_Confirmation2, Res.Document_Void_Confirmation3,
						Res.Document_Restore_Confirmation1, Res.Document_Restore_Confirmation2, Res.Document_Restore_Confirmation3,
						isVoid, records.Length,
						delegate { PaymentService.ChangeVoidStatus((object[]) ids, AutoGrid.BaseRequest, OnVoidComplete, null); });
				}, null);
		}



		private void OnVoidComplete(object result)
		{
			ItemListResponse response = (ItemListResponse) result;

			PaymentDto[] dtos = (PaymentDto[]) response.Items;
			RangeResponse rangeResponse = response.RangeResponse;

			((WebServiceProxy) AutoGrid.store.proxy).SetResponse(rangeResponse);

			AutoGrid.Reload(false);

			MessageFactory.VoidCompletedMsg(DomainRes.Payment_Caption_List,
				Res.Document_VoidDocument_Msg, Res.Document_VoidDocuments_Msg,
				Res.Document_RestoreDocument_Msg, Res.Document_RestoreDocuments_Msg,
				dtos[0].IsVoid, dtos.Length, dtos[0].Number,
				Script.IsNullOrUndefined(rangeResponse.SelectedRow) ? BaseRes.AutoGrid_NotDisplay_Msg : null);

			if (!Script.IsNullOrUndefined(rangeResponse.SelectedRow))
				AutoGrid.SelectionModel.selectRow(rangeResponse.SelectedRow);
		}



		private void PostedPayments()
		{
			PaymentService.PostPayments(AutoGrid.GetSelectedIds(), AutoGrid.BaseRequest,
				delegate(object result)
				{
					ItemListResponse response = (ItemListResponse) result;

					RangeResponse rangeResponse = response.RangeResponse;

					((WebServiceProxy) AutoGrid.store.proxy).SetResponse(rangeResponse);

					AutoGrid.Reload(false);

					MessageRegister.Info(ListConfig.Caption, string.Format(Res.PaymentList_PostedDocuments, response.Items.Length));

					if (!Script.IsNullOrUndefined(rangeResponse.SelectedRow))
						AutoGrid.SelectionModel.selectRow(rangeResponse.SelectedRow);
				}, null);
		}



		private void CreatePayment(string type)
		{
			FormsRegistry.EditObject(type, null, null,
				delegate(object arg1)
				{
					ItemResponse response = (ItemResponse) arg1;
					PaymentDto dto = (PaymentDto) response.Item;

					string message = BaseRes.Created + " " + dto.Number;

					if (Script.IsNullOrUndefined(response.RangeResponse.SelectedRow))
					{
						MessageRegister.Info(ListConfig.Caption, message, BaseRes.AutoGrid_NotDisplay_Msg);

						return;
					}

					MessageRegister.Info(ListConfig.Caption, message);

					((WebServiceProxy) AutoGrid.store.proxy).SetResponse(response.RangeResponse);

					AutoGrid.Reload(false);

					AutoGrid.SelectionModel.selectRow(response.RangeResponse.SelectedRow);

					if (type == ClassNames.CashInOrderPayment)
						ReportPrinter.GetCashOrder(dto.Id, dto.DocumentNumber);
				}, null, AutoGrid.BaseRequest);
		}



		//---g



		private Action _voidButton;
		private Action _postButton;



		//---g

	}






	//===g



}