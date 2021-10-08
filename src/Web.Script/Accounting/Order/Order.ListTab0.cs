//using System;
//using System.Collections;
//using System.Serialization;
//
//using Ext;
//using Ext.data;
//using Ext.grid;
//
//using LxnBase;
//using LxnBase.Data;
//using LxnBase.UI;
//using LxnBase.UI.AutoControls;
//using LxnBase.UI.Controls;
//
//using Luxena.Travel.Controls;
//
//using Record = Ext.data.Record;
//
//
//namespace Luxena.Travel
//{
//
//	public class OrderListTab0 : AutoListTabExt
//	{
//		static OrderListTab0()
//		{
//			FormsRegistry.RegisterList(ClassNames.Order, ListObject);
//		}
//
//		public OrderListTab0(string tabId, ListArgs args) : base(tabId, args)
//		{
//		}
//
//		public static void ListObject(ListArgs args, bool newTab)
//		{
//			Tabs.Open(newTab, args.Type, delegate(string tabId) { return new OrderListTab0(tabId, args); }, args.BaseRequest);
//		}
//
//		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
//		{
//			base.OnInitGrid(args, config);
//
//			args.ForcedProperties = new string[] { "IsVoid", "TotalDue", "DeliveryBalance", "BillToName", "ConsignmentRefs" };
//
//			if (args.BaseRequest == null)
//				args.BaseRequest = new RangeRequest();
//
//			if (args.BaseRequest.Sort == null)
//			{
//				args.BaseRequest.Sort = "IssueDate";
//				args.BaseRequest.Dir = "DESC";
//			}
//
//			args.ColumnsConfig = CreateColumnsConfig();
//
//			config.view(new OrderGridView());
//		}
//
//		private ArrayList CreateColumnsConfig()
//		{
//			ArrayList list = new ArrayList();
//
//			list.Add(CreateReferenceCfg("Number", false, 120));
//			list.Add(ColumnCfg("IssueDate", false, 80));
//
//			list.Add(ColumnCfg("Customer", false, 150));
//			//list.Add(CreateColumnCfg("BillTo", true, 150));
//			list.Add(ColumnCfg("BillTo", true, 150, new GridRenderDelegate(
//				delegate(object value, object metadata, Record record, int rowIndex, int colIndex, Store store)
//				{
//					if (value == null) 
//						return record.get("BillToName");
//
//					object[] arr = value as object[];
//					if (arr != null)
//						return ObjectLink.RenderArray(arr);
//
//					Reference info = (Reference)value;
//					return ObjectLink.RenderInfo(info);
//				}))
//			);
//
//			Dictionary cfg = ColumnCfg(
//				"ConsignmentNumbers", false, 100,
//				ObjectLink.ReferencesRenderer("ConsignmentRefs")
//			);
//			cfg["sortable"] = false;
//
//			list.Add(cfg);
//			
//			list.Add(ColumnCfg("ShipTo", true, 150));
//
//			list.Add(ColumnCfg("Note", true, 150));
//
//			list.Add(ColumnCfg("Discount", true, 90));
//			list.Add(ColumnCfg("Total", false, 90));
//			list.Add(ColumnCfg("Vat", false, 80));
//			list.Add(ColumnCfg("Paid", false, 80));
//			list.Add(ColumnCfg("TotalDue", false, 80));
//			list.Add(ColumnCfg("ServiceFee", true, 80));
//			list.Add(ColumnCfg("DeliveryBalance", false, 80));
//
//			list.Add(ColumnCfg("AssignedTo", false, 150));
//
//			list.Add(ColumnCfg("Owner", true));
//			list.Add(ColumnCfg("IsPublic", true));
//			list.Add(ColumnCfg("IsSubjectOfPaymentsControl", true));
//
//			list.Add(ColumnCfg("IsVoid", true));
//
//			list.Add(ColumnCfg("CreatedOn", true));
//			list.Add(ColumnCfg("CreatedBy", true));
//			list.Add(ColumnCfg("ModifiedOn", true));
//			list.Add(ColumnCfg("ModifiedBy", true));
//
//			return list;
//		}
//
//
//		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
//		{
//			_printButton = new Ext.Action(new ActionConfig()
//				.text(Res.AviaDocument_Print_Action.ToLowerCase())
//				.handler(new AnonymousDelegate(PrintOrders))
//				.disabled(true)
//				.ToDictionary());
//
//			toolbarItems.Add(_printButton);
//		}
//
//		protected override void OnSelectionChange(AbstractSelectionModel selectionModel)
//		{
//			RowSelectionModel model = (RowSelectionModel)selectionModel;
//
//			Record[] selections = (Record[])model.getSelections();
//
//			if (selections.Length == 0)
//			{
//				_printButton.disable();
//			}
//			else
//			{
//				_printButton.enable();
//			}
//		}
//
//		private void PrintOrders()
//		{
//			Record[] records = (Record[])AutoGrid.SelectionModel.getSelections();
//
//			ArrayList ids = new ArrayList();
//
//			for (int i = 0; i < records.Length; i++)
//				ids.Add(records[i].id);
//
//			string value = Json.Stringify(ids);
//
//			ReportLoader.Load(string.Format("print/order/Order_{0}.pdf", Date.Now.Format("Y-m-d_H-i-s")), new Dictionary("orders", value));
//		}
//
//
//
//		private Ext.Action _printButton;
//
//	}
//
//}