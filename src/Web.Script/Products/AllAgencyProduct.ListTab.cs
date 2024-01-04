using System.Collections;
using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;




namespace Luxena.Travel
{



	//===g






	public abstract class AllAgencyProductListTab : EntityListTab
	{

		//---g



		protected AllAgencyProductListTab(string tabId, ListArgs args) : base(tabId, args) { }



		public static void ListObject(ListArgs args, bool newTab)
		{

			args.Type = ClassNames.Product;

			args.BaseRequest = new RangeRequest();
			args.BaseRequest.Params = new Dictionary("forAllAgency", true);

			ListObjectsOfType2(typeof(AllAgencyProductListTab), ClassNames.AllAgencyProduct, args, newTab);

		}


		protected override void OnLoad()
		{
			setTitle(AppActions.AllAgencyProductTitle);

		}



		//---g



		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{

			base.OnInitGrid(args, config);

			args.ListConfig.IsCreationAllowed.Visible = true;

			args.ForcedProperties = new string[] { "Name", "RequiresProcessing", "IsRefund", "IsVoid" };

			args.BaseRequest.SetDefaultSort("IssueDate");

			config.view(new ProductGridView(null));

		}



		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{

			//toolbarItems.InsertRange(0, (object[])toolbarActions);

		}




		protected override void CreateColumnConfigs()
		{

			SemanticDomain sd = new SemanticDomain(this);
			ProductSemantic se = sd.Product;


			AddColumns(new object[]
			{
				se.IssueDate,
				ColumnCfg("Type"),
				se.Name,
				se.PassengerName,
				sd.AviaDocument.Producer,
				se.Provider,
				sd.AviaDocument.TicketingIataOffice,
				se.Customer,
				se.GrandTotal,
				se.Order,
				se.Booker,
				se.Ticketer,
				se.Seller,
				se.IsPaid,
				se.Owner,

				se.CreatedOn.ToColumn(true),
				se.CreatedBy.ToColumn(true),
				se.ModifiedOn.ToColumn(true),
				se.ModifiedBy.ToColumn(true),

				new ColumnConfig().header("Id").dataIndex("Id").hidden(true).ToDictionary(),
			});

		}



		//---g

	}






	//===g



}
