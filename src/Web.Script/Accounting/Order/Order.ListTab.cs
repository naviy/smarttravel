using System;
using System.Collections;
using System.Serialization;

using Ext.data;
using Ext.grid;

using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Record = Ext.data.Record;


namespace Luxena.Travel
{

	partial class OrderListTab
	{

		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			args.ForcedProperties = new string[]
			{
				"IsVoid", "TotalDue", "DeliveryBalance", "BillToName", "ConsignmentRefs"
			};

			args.SetDefaultSort("IssueDate", "DESC");

			config.view(new OrderGridView());
		}


		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.IssueDate,
				se.Number,

				se.Customer.ToColumn(false, 150),
				se.BillTo.ToColumn(true, 150, 
					delegate(object value, object metadata, Record record, int rowIndex, int colIndex, Store store)
					{
						return value == null ? record.get("BillToName") : ObjectLink.RenderValue(value);
					}
				),

				se.ConsignmentNumbers.ToColumn(false, 100, ObjectLink.ReferencesRenderer("ConsignmentRefs"),
					delegate(ColumnConfig cfg) { cfg.sortable(false); }
				),

				se.ShipTo.ToColumn(true, 150),
				se.Intermediary.ToColumn(true, 150),

				se.Note.ToColumn(true, 150),

				se.Discount.ToColumn(true, 90),
				se.Total,
				se.Vat,
				se.CheckPaid,
				se.WirePaid,
				se.CreditPaid,
				se.RestPaid,
				se.Paid,
				se.TotalDue,
				se.ServiceFee.ToColumn(true),
				se.DeliveryBalance,

				se.AssignedTo.ToColumn(false, 150),

				se.Owner.ToColumn(true),
				se.IsPublic.ToColumn(true),
				se.IsSubjectOfPaymentsControl.ToColumn(true),

				se.IsVoid.ToColumn(true),

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}


		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			_printButton = Action(Res.AviaDocument_Print_Action.ToLowerCase(), PrintOrders, true);

			toolbarItems.InsertRange(0, (object[])toolbarActions);
		}

		protected override void OnSelectionChange(AbstractSelectionModel selectionModel)
		{
			Record[] selections = (Record[])((RowSelectionModel)selectionModel).getSelections();

			_printButton.setDisabled(selections.Length == 0);
		}

		private void PrintOrders()
		{
			Record[] records = (Record[])AutoGrid.SelectionModel.getSelections();

			ArrayList ids = new ArrayList();

			foreach (Record r in records)
				ids.Add(r.id);

			string value = Json.Stringify(ids);

			ReportLoader.Load(string.Format("print/order/Order_{0}.pdf", Date.Now.Format("Y-m-d_H-i-s")), new Dictionary("orders", value));
		}

		
		private Ext.Action _printButton;

	}

}