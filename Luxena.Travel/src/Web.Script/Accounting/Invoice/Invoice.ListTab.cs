using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Luxena.Travel.Services;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	partial class InvoiceListTab
	{

		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			_registerPaymentAction = MenuAction(Res.Payment_CreatePayment_Action, new object[]
			{
				MenuCreateItem2(Res.Payment_CreateCashInOrder_Action, CreatePayment, ClassNames.CashInOrderPayment),
				MenuCreateItem2(Res.Payment_CreateCashOutOrder_Action, CreatePayment, ClassNames.CashOutOrderPayment),
				MenuCreateItem2(Res.Payment_CreateNonCashPayment_Action, CreatePayment, ClassNames.WireTransfer),
				MenuCreateItem2(Res.Payment_CreateCheckPayment_Action, CreatePayment, ClassNames.CheckPayment),
				MenuCreateItem2(Res.Payment_CreateElecronicPayment_Action, CreatePayment, ClassNames.ElectronicPayment),
			});

			toolbarItems.Insert(0, _registerPaymentAction);
			toolbarItems.Insert(1, new ToolbarSeparator());

			_registerPaymentAction.disable();
		}

		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			args.ForcedProperties = new string[] { "IsOrderVoid", "BillToName" };

			config.view(new InvoiceGridView());
		}


		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Number.ToColumn(false, 120),
				se.IssueDate,
				se.Type.ToColumn(false, 80),
				se.Order.ToColumn(false, 120),

				se.Customer.ToColumn(false, 150),
				se.BillTo.ToColumn(false, 150, 
					delegate(object value, object metadata, Record record, int rowIndex, int colIndex, Store store)
					{
						return value == null ? record.get("BillToName") : ObjectLink.RenderValue(value);
					}
				),

				se.ShipTo.ToColumn(true, 150),

				se.IsOrderVoid.ToColumn(true),
				se.Owner.ToColumn(true, 150),
				se.IssuedBy.ToColumn(false, 150),

				se.Total,
				se.Vat,
				se.TimeStamp,
			});
		}


		protected override void OnSelectionChange(AbstractSelectionModel selectionModel)
		{
			RowSelectionModel model = (RowSelectionModel) selectionModel;

			Record[] selections = (Record[]) model.getSelections();

			if (selections.Length == 1)
			{
				_registerPaymentAction.enable();
				_selection = selections[0];
			}
			else
			{
				_registerPaymentAction.disable();
				_selection = null;
			}
		}



		private void CreatePayment(string type)
		{
			Array orderInfo = (Array) _selection.get("Order");

			OrderService.GetOrder(orderInfo[0],
				delegate (object result)
				{
					OrderDto order = (OrderDto) result;

					FormsRegistry.EditObject(type, null,
						new Dictionary(
							"Invoice", Reference.Create(ClassNames.Invoice, (string)_selection.get("Number"), _selection.id),
							"Order", orderInfo,
							"Payer", order.Customer,
							"Amount", order.TotalDue,
							"Vat", order.VatDue,
							"Owner", order.Owner
						),
						delegate(object arg1)
						{
							ItemResponse response = (ItemResponse) arg1;

							PaymentDto dto = (PaymentDto) response.Item;

							string message = BaseRes.Created + " " + dto.Number;

							MessageRegister.Info(DomainRes.Payment_Caption_List, message);

							if (type == ClassNames.CashInOrderPayment)
								ReportPrinter.GetCashOrder(dto.Id, dto.DocumentNumber);
						},
						null
					);
				},
				null
			);
		}


		private Action _registerPaymentAction;

		private Record _selection;
	}
}