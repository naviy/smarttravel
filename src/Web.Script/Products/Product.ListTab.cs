using System;
using System.Collections;

using Ext;
using Ext.grid;
using Ext.menu;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.Net;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Luxena.Travel.Cfg;
using Luxena.Travel.Services;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public delegate void ServiceCanUpdateAction(Object[] ids, AjaxCallback onSuccess, WebServiceFailure onError);

	public delegate void ServiceChangeVoidStatusAction(Object[] ids, RangeRequest @params, AjaxCallback onSuccess, WebServiceFailure onError);


	public abstract class ProductListTab : EntityListTab
	{

		protected ProductListTab(string tabId, ListArgs args) : base(tabId, args) { }

		protected virtual bool IsRefund() { return false; }

		protected virtual bool UseManyPassengers() { return false; }

		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			args.ListConfig.IsCreationAllowed.Visible = true;

			args.ForcedProperties = new string[]{"Name", "RequiresProcessing", "IsRefund", "IsVoid", UseManyPassengers() ? "PassengerDtos" : "Passenger" };

			args.BaseRequest.SetDefaultSort("IssueDate");

			config.view(new ProductGridView(null));
		}


		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			CreateVoidAction(VoidProducts);

			CreateStdToolbarActions();

			toolbarItems.InsertRange(0, (object[])toolbarActions);
		}

		protected virtual void CreateVoidAction(AnonymousDelegate voidAction)
		{
			if (ListConfig.IsEditAllowed.Visible)
			{
				_voidButton = Action(Res.Void_Action.ToLowerCase(), voidAction, true, ListConfig.IsEditAllowed.DisableInfo);
			}
		}


		protected void CreateStdToolbarActions()
		{
			ArrayList createOrderMenuItems = new ArrayList();

			createOrderMenuItems.AddRange(new object[]
			{
				MenuItem(Res.AviaDocument_AddToOrderNew.ToLowerCase(), delegate { CreateNewOrder(); }),
				MenuItem(Res.AviaDocument_AddToOrderExist.ToLowerCase(), delegate { TryAddToExistingOrder(); })
			});


			if (AppManager.SystemConfiguration.AviaOrderItemGenerationOption == AviaOrderItemGenerationOption.ManualSetting)
			{
				_separateServiceFee = new CheckItem(new CheckItemConfig()
					.hideOnClick(false)
					.checked_(true)
					.text(Res.Order_SeparateServiceFee)
					.ToDictionary());

				createOrderMenuItems.AddRange(new object[] { "-", _separateServiceFee });
			}


			_createOrderButton = MenuAction(Res.AviaDocument_AddToOrder.ToLowerCase(), (object[])createOrderMenuItems, true);
			_createPaymentButton = Action(Res.AviaDocumentList_CreateCashPayment, TryCreateCashPayment, true);

		}


		protected virtual void CreateCustomColumnConfigs()
		{
			throw new Exception("NotImplemented");
		}

		protected override void CreateColumnConfigs()
		{
			ProductSemantic se = new SemanticDomain(this).Product;

			AddColumns(new object[]
			{
				se.IssueDate,
				ColumnCfg("Type"),
			});

			CreateCustomColumnConfigs();

			AddColumns(new object[]
			{
				se.ReissueFor.ToColumn(true),
				se.Customer,
				se.PaymentType,
				se.Fare,
				se.EqualFare,
				se.Commission,
				se.Total,
				se.ServiceFee,
				se.BookingFee.ToColumn(true),
			});

			if (IsRefund())
				AddColumns(new object[]
				{
					se.CancelFee,
				});

			AddColumns(new object[]
			{
				se.FeesTotal.ToColumn(true),
				se.Vat.ToColumn(true),
				se.GrandTotal,
				se.Order,
				se.Seller,
				se.Booker.ToColumn(true),
				se.BookerCode.ToColumn(true),
				se.BookerOffice.ToColumn(true),
				se.Ticketer.ToColumn(true),
				se.TicketerCode.ToColumn(true),
				se.TicketerOffice.ToColumn(true),
				se.IsPaid,
			});

			if (UseHandling) AddColumns(new object[]
			{
				se.Handling.ToColumn(true),
				se.CommissionDiscount.ToColumn(true),
			});

			AddColumns(new object[]
			{
				se.Discount.ToColumn(true),
				se.BonusDiscount.ToColumn(true),
				se.BonusAccumulation.ToColumn(true),
				se.Owner.ToColumn(false),
				se.LegalEntity.ToColumn(true),

				se.IsProcessed.ToColumn(true),
				se.IsVoid.ToColumn(true),
				se.RequiresProcessing.ToColumn(true),
				se.Note.ToColumn(true),

				se.CreatedOn.ToColumn(true),
				se.CreatedBy.ToColumn(true),
				se.ModifiedOn.ToColumn(true),
				se.ModifiedBy.ToColumn(true),

				new ColumnConfig().header("Id").dataIndex("Id").hidden(true).ToDictionary(),
			});
		}


		protected override void OnSelectionChange(AbstractSelectionModel selectionModel)
		{
			RowSelectionModel model = (RowSelectionModel)selectionModel;

			Record[] selections = (Record[])model.getSelections();

			if (_voidButton != null && !ListConfig.IsEditAllowed.IsDisabled)
			{
				if (selections.Length == 0)
					_voidButton.disable();
				else
				{
					bool isVoid = (bool)Type.GetField(selections[0].data, "IsVoid");
					bool voidDisable = false;

					foreach (Record row in selections)
						if (!voidDisable && (bool)Type.GetField(row.data, "IsVoid") != isVoid)
							voidDisable = true;

					if (!voidDisable)
					{
						_voidButton.enable();
						_voidButton.setText(isVoid ? Res.Restore_Action.ToLowerCase() : Res.Void_Action.ToLowerCase());
					}
					else
						_voidButton.disable();
				}
			}

			if (selections.Length == 0)
			{
				//_printButton.disable();
				_createOrderButton.disable();
				_createPaymentButton.disable();
			}
			else
			{
				IsTickets = true;
				IsVoid = false;
				bool isRefund = false;

				foreach (Record row in selections)
				{
					if ((string)Type.GetField(row.data, ObjectPropertyNames.ObjectClass) != ClassNames.AviaTicket)
						IsTickets = false;
					if ((bool)Type.GetField(row.data, "IsRefund"))
						isRefund = true;
					if ((bool)Type.GetField(row.data, "IsVoid"))
						IsVoid = true;
				}
				if (IsVoid)
					_createOrderButton.disable();
				else
					_createOrderButton.enable();

				if (IsVoid || isRefund)
					_createPaymentButton.disable();
				else
					_createPaymentButton.enable();
			}

			OnRowChange(selections);
		}

		protected virtual void OnRowChange(Record[] selections)
		{

		}


		protected virtual void VoidProducts()
		{
			VoidObjects(ProductService.CanUpdate, ProductService.ChangeVoidStatus);
		}

		protected void VoidObjects(ServiceCanUpdateAction canUpdate, ServiceChangeVoidStatusAction changeVoidStatus)
		{
			RowSelectionModel selectionModel = AutoGrid.SelectionModel;

			if (selectionModel.getSelections().Length == 0)
				return;

			Record[] records = (Record[])selectionModel.getSelections();

			ArrayList ids = new ArrayList();

			foreach (Record r in records)
				ids.Add(r.id);

			canUpdate((object[])ids,
				delegate(object res)
				{
					OperationStatus status = (OperationStatus)res;

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

					bool isVoid = (bool)Type.GetField(records[0].data, "IsVoid");

					MessageBoxWrap.Confirm(
						BaseRes.Confirmation, GetVoidingConfirmationText(isVoid, records.Length),
						delegate(string button, string text)
						{
							if (button != "yes")
								return;

							changeVoidStatus((object[])ids, _baseParams,
								delegate(object result)
								{
									ProductDto[] dtos = (ProductDto[])((object[])result)[0];
									RangeResponse response = (RangeResponse)((object[])result)[1];

									((WebServiceProxy)AutoGrid.store.proxy).SetResponse(response);

									AutoGrid.Reload(false);

									string message;

									if (dtos[0].IsVoid)
									{
										_voidButton.setText(Res.Restore_Action.ToLowerCase());

										message = dtos.Length == 1 ? string.Format(Res.Document_VoidDocument_Msg, dtos[0].Name) :
											string.Format(Res.Document_VoidDocuments_Msg, dtos.Length);
									}
									else
									{
										_voidButton.setText(Res.Void_Action.ToLowerCase());

										message = dtos.Length == 1 ? string.Format(Res.Document_RestoreDocument_Msg, dtos[0].Name) :
											string.Format(Res.Document_RestoreDocuments_Msg, dtos.Length);
									}

									if (Script.IsNullOrUndefined(response.SelectedRow))
										MessageRegister.Info(DomainRes.AviaDocument_Caption_List, message, BaseRes.AutoGrid_NotDisplay_Msg);
									else
									{
										MessageRegister.Info(DomainRes.AviaDocument_Caption_List, message);

										AutoGrid.SelectionModel.selectRow(response.SelectedRow);
									}
								}, null);
						});
				}, null);
		}

		private static string GetVoidingConfirmationText(bool isVoid, int number)
		{
			string msg1 = isVoid ? Res.Document_Restore_Confirmation1 : Res.Document_Void_Confirmation1;
			string msg2 = isVoid ? Res.Document_Restore_Confirmation2 : Res.Document_Void_Confirmation2;
			string msg3 = isVoid ? Res.Document_Restore_Confirmation3 : Res.Document_Void_Confirmation3;

			return StringUtility.GetNumberText(number, msg1, msg2, msg3);
		}

		private void CreateNewOrder()
		{
			Dictionary values = new Dictionary("AviaDocuments", AutoGrid.GetSelectedIds());

			if (_separateServiceFee != null)
				values["SeparateServiceFee"] = Type.GetField(_separateServiceFee, "checked");

			FormsRegistry.EditObject(ClassNames.Order, null, values, delegate(object arg)
			{
				AutoGrid.Reload(false);

				ItemResponse response = (ItemResponse)arg;

				FormsRegistry.ViewObject(ClassNames.Order, ((OrderDto)response.Item).Id);
			}, null);
		}

		private void TryCreateCashPayment()
		{
			object[] documentIds = GetSelectedIds();

			PaymentService.CanCreatePayment(documentIds,
				delegate(object result)
				{
					AviaPaymentResponse response = (AviaPaymentResponse)result;

					if (response.OrderItems == null || response.OrderItems.Length == 0)
						CreateCashPayment(response.DocumentIds, response.Payer, response.Total, response.Vat);
					else
					{
						if (response.DocumentIds != null && response.DocumentIds.Length > 0)
							MessageFactory.DocumentsAlreadyAddedToOrder(Res.AviaDocumentList_CreateCashPayment_Title,
								Res.AviaDocumentList_CannotCreatePayment, response.OrderItems, Res.AviaDocumentList_AddToPayment,
								delegate
								{
									CreateCashPayment(response.DocumentIds, response.Payer, response.Total, response.Vat);
								});
						else
							MessageFactory.DocumentsAlreadyAddedToOrder(Res.AviaDocumentList_CreateCashPayment_Title,
								Res.AviaDocumentList_CannotCreatePayment, response.OrderItems);
					}
				}, null);
		}

		private void CreateCashPayment(object[] documents, Reference payer, MoneyDto amount, MoneyDto vat)
		{
			CashPaymentRegisterForm form = new CashPaymentRegisterForm(new CashPaymentRegisterFormArgs(payer, amount, vat, documents));

			form.Open();
		}


		private void TryAddToExistingOrder()
		{
			Record[] records = SelectedRecords;

			Dictionary documents = new Dictionary();

			foreach (Record r in records)
				documents[(string) r.id] = ((AviaDocumentDto) r.data).Name;


			ProductDto.TryAddToExistingOrder(
				documents,
				_separateServiceFee != null && (bool) Type.GetField(_separateServiceFee, "checked"),
				ListConfig.Caption,
				delegate { AutoGrid.Reload(false); }
			);
		}


		protected object[] GetSelectedIds()
		{
			return AutoGrid.GetSelectedIds();
		}

		protected static bool UseHandling
		{
			get { return AppManager.SystemConfiguration.UseAviaHandling; }
		}


		protected Action _voidButton;
		protected Action _createOrderButton;
		protected Action _createPaymentButton;
		protected CheckItem _separateServiceFee;

		protected bool IsTickets;
		protected bool IsVoid;
	}

}
