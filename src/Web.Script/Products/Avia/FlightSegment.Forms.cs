using System.Collections;

using Ext.grid;

using LxnBase;
using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	partial class FlightSegmentListTab
	{

		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			args.SetDefaultSort("DepartureTime", "DESC");
		}

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.Ticket,
				se.Position.ToColumn(true),

				se.FromAirport,
				se.ToAirport,
				se.Carrier,
				se.FlightNumber,
				se.Seat,
				se.ServiceClassCode.ToColumn(true),
				se.ServiceClass,
				se.DepartureTime,
				se.CheckInTime.ToColumn(true),
				se.CheckInTerminal.ToColumn(true),
				se.ArrivalTime,
				se.ArrivalTerminal.ToColumn(true),
				se.NumberOfStops.ToColumn(true),
				se.Luggage,
				se.Duration,
				se.FareBasis,
				se.Stopover,
				se.MealTypes,

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid grid)
		{
			toolbarItems.Remove(grid.CreateAction);
			toolbarItems.Remove(grid.CopyAction);
		}
	}


	partial class FlightSegmentEditForm
	{
		protected override void CreateControls()
		{
			Window.cls += " segment-edit";
			Window.width = -6;
			Form.labelWidth = 140;

			Form.add(ColumnPanel(new object[]
			{
				MainDataPanel(new object[]
				{
					se.FromAirport.ToField(-3),
					se.ToAirport.ToField(-3),
					se.Carrier.ToField(-3),
					se.Operator.ToField(-3),
					se.RowPanel2(se.FlightNumber, null, se.Seat, null),
					se.RowPanel2(se.ServiceClassCode, null, se.ServiceClass, null),
					se.Equipment.ToField(-3),
					se.RowPanel2(se.CheckInTime, null, se.CheckInTerminal, null),
					se.DepartureTime.ToField(-1),
					se.RowPanel2(se.ArrivalTime, null, se.ArrivalTerminal, null),
					EmptyRow(),
					se.Stopover,
					se.Type,
				}),

				MainDataPanel(new object[]
				{
					se.NumberOfStops,
					se.Luggage,
					se.Duration,
					se.FareBasis,
					EmptyRow(),
					se.MealTypes.ToField(-3),
				}),
			}));
		}
	}

}
