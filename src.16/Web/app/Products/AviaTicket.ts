module Luxena.Views
{

	registerEntityControllers(sd.AviaTicket, se => ({

		list: sd.AviaDocument,

		formTitle: se.FullNumber,

		view: () => sd.tabCard(
			sd.col().icon(se).items(
				sd.row(se.FullNumber, se.Producer).header3(),
				sd.hr(),
				sd.row(
					sd.col(
						se.IssueDate,

						se.ReissueFor,
						se.PassengerRow,
						se.Itinerary,
						se.CustomerAndOrder,
						se.Intermediary,

						se.GdsPassportStatus,

						se.PnrCode,
						se.TourCode,

						se.Booker,
						se.Ticketer,
						se.SellerAndOwner,
						se.OriginalDocument
					).length(8),

					se.Finance.length(4)
				),

				sd.hr2(),
				se.Note
			),

			se.HistoryTab.clone()
		),

		edit: () => ({
			"fields1": [
				se.IssueDateAndReissueFor,
				se.NumberRow,

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				sd.row(
					sd.col(se.PnrCode, se.TourCode),
					sd.col(se.GdsPassportStatus, se.Originator)
				),

				se.BookerRow,
				se.TicketerRow,
				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		}),

		//viewScope: ctrl => ({
		//	tabs: [
		//		se.Segments.toGridTab(ctrl, a => [
		//			a.Position,
		//			a.FromAirport,
		//			a.ToAirport,
		//			a.Carrier,
		//			a.FlightNumber,
		//			a.Seat,
		//			a.ServiceClass,
		//			a.DepartureTime,
		//			a.ArrivalTime,
		//			a.Duration,
		//			a.FareBasis,
		//			a.Luggage,
		//			a.MealTypes,
		//			a.CouponAmount,
		//		])
		//	],
		//}),

	}));

}