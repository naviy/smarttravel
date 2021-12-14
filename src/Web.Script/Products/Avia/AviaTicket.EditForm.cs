using Ext;

using Luxena.Travel.Services;


namespace Luxena.Travel
{

	public partial class AviaTicketEditForm
	{

		protected override void Initialize()
		{
			Window.cls += " tabbed";
			Form.cls = "tabbed";
		}


		protected override void CreateControls()
		{

			Panel pnlMain = ColumnPanel(new object[]
			{
				MainDataPanel(new object[]
				{
					se.IssueDate,
					se.FullNumber,
					se.ReissueFor,
					se.PassengerRow,
					se.GdsPassportStatus,
					
					se.Provider.ToField(-3),
					se.CustomerAndOrder,
					se.Intermediary,
					se.Country,
					se.Originator,
					se.PnrAndTourCodes,
					se.BookerRow,
					se.TicketerRow,
					se.TicketingIataOffice,
					se.SellerAndOwnerRow,
					se.LegalEntity,
//					se.PrintUnticketedFlightSegments,
				}),

				se.Finance,
			});
			pnlMain.title = DomainRes.ProductType_AviaTicket;

			//_segmentGrid = new SegmentGridControl(295);
			//_segmentGrid.Widget.addClass("Segments");


			Form.add(TabPanel(600, new Component[]
			{
				pnlMain,
				TabFitPane(DomainRes.AviaTicket_FlightSegment, se.GridMember("Segments", new FlightSegmentGridControl())),
			}));


			Form.add(se.Note.ToEditor());

		}

		//protected override void OnLoad(object result)
		//{
		//	base.OnLoad(result);

		//	if (!_args.IsCopy)
		//		_segmentGrid.LoadData((AviaTicketDto)result);
		//}

		//protected override void Save()
		//{
		//	if (!IsModified() && _segmentGrid.IsModified())
		//		CompleteSave(null);
		//	else
		//		base.Save();
		//}


		protected override void OnSave()
		{
			AviaService.UpdateAviaTicket(
				(AviaTicketDto)GetData(), _args.RangeRequest, 
				CompleteSave, FailSave
			);
		}

	}

}