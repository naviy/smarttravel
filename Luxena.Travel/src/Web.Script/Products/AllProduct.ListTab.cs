using System;
using System.Collections;
using System.Serialization;

using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public class AllProductListTab : ProductListTab
	{
		public AllProductListTab(string tabId, ListArgs args) : base(tabId, args) { }

		public static void ListObject(ListArgs args, bool newTab)
		{
			ListObjectsOfType(typeof(AllProductListTab), args, newTab);
		}


		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			args.ForcedProperties = new string[] { "RequiresProcessing", "IsVoid" };
		}

		protected override void CreateCustomColumnConfigs()
		{
			SemanticDomain sd = new SemanticDomain(this);
			ProductSemantic se = sd.Product;

			AddColumns(new object[]
			{
				se.Name,
				se.PassengerName,
				sd.AviaDocument.Producer,
				se.Provider,
				se.Country.ToColumn(true),
				se.PnrCode.ToColumn(true),
				se.TourCode.ToColumn(true),
				sd.AviaDocument.TicketingIataOffice,
			});
		}

		protected override void HandleAltEnterPress()
		{
			if (AutoGrid.SelectionModel.getSelections().Length != 0)
				TryProcess();
		}


		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid grid)
		{
			if (ListConfig.IsEditAllowed.Visible)
			{
				_handleButton = Action(Res.AviaDocument_Handle_Action.ToLowerCase(), TryProcess, true, ListConfig.IsEditAllowed.DisableInfo);
				_printButton = Action(Res.AviaDocument_Print_Action.ToLowerCase(), PrintDocuments, true);

				MenuAction(BaseRes.CreateItem_Lower, new object[]
				{
					MenuCreateItem(DomainRes.AviaTicket, ClassNames.AviaTicket),
					MenuCreateItem(DomainRes.AviaMco, ClassNames.AviaMco),
					MenuCreateItem(DomainRes.AviaRefund, ClassNames.AviaRefund),
					AppActions.AviaConsoleParser,
					"-",
					MenuCreateItem(DomainRes.BusTicket, ClassNames.BusTicket),
					MenuCreateItem(DomainRes.CarRental, ClassNames.CarRental),
					MenuCreateItem(DomainRes.Pasteboard, ClassNames.Pasteboard),
					MenuCreateItem(DomainRes.Accommodation, ClassNames.Accommodation),
					MenuCreateItem(DomainRes.Insurance, ClassNames.Insurance),
					MenuCreateItem(DomainRes.Isic, ClassNames.Isic),
					MenuCreateItem(DomainRes.SimCard, ClassNames.SimCard),
					MenuCreateItem(DomainRes.Transfer, ClassNames.Transfer),
					MenuCreateItem(DomainRes.Tour, ClassNames.Tour),
					MenuCreateItem(DomainRes.Excursion, ClassNames.Excursion),
					"-",
					MenuCreateItem(DomainRes.GenericProduct, ClassNames.GenericProduct)
				});
			}

			CreateStdToolbarActions();

			if (grid != null)
			{
				if (toolbarItems.Contains(grid.CreateAction))
					toolbarItems.Remove(grid.CreateAction);
				if (toolbarItems.Contains(grid.ExportAction))
					toolbarItems.Remove(grid.ExportAction);
			}

			toolbarItems.InsertRange(0, (object[])toolbarActions);
		}

		protected override void OnRowChange(Record[] selections)
		{
			base.OnRowChange(selections);

			if (_handleButton != null && !ListConfig.IsEditAllowed.IsDisabled)
			{
				_handleButton.setDisabled(selections.Length != 1);
			}

			_printButton.setDisabled(selections.Length == 0 || !IsTickets);
		}



		private void TryProcess()
		{
			Record record = AutoGrid.SelectionModel.getSelected();
			string type = (string)Type.GetField(record.data, ObjectPropertyNames.ObjectClass);

			if (type == ClassNames.AviaMco || type == ClassNames.AviaRefund || type == ClassNames.AviaTicket)
				AviaDocumentListTab.BaseTryProcess(AutoGrid, _baseParams, null);
			else
				AutoGrid.Edit();
		}

		private void PrintDocuments()
		{
			string value = Json.Stringify(GetSelectedIds());

			ReportLoader.Load(string.Format("print/ticket/Tickets_{0}.pdf", Date.Now.Format("Y-m-d_H-i-s")), new Dictionary("tickets", value));
		}



		private Action _handleButton;
		private Action _printButton;

	}

}