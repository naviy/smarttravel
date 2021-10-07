module Luxena.Views
{
	registerEntityControllers(sd.Accommodation, se => ({
		
		list: [
			se.IssueDate,
			se.Name,
			se.PassengerName,
			se.Provider,
			se.Customer,
			se.Order,
			se.Total,
			se.ServiceFee,
			se.GrandTotal,
		],

		view: {
			"fields1": [
				se.IssueDate,
				se.ReissueFor,

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				se.Provider,
				se.StartDate,
				se.FinishDate,
				se.Country,
				se.PnrCode,
				se.TourCode,

				se.HotelName, se.HotelOffice, se.HotelCode,
				se.PlacementName, se.PlacementOffice, se.PlacementCode,

				se.AccommodationType,
				se.CateringType,

				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		},

		edit: {
			"fields1": [
				se.IssueDateAndReissueFor,

				se.PassengerRow,
				se.CustomerAndOrder,
				se.Intermediary,

				se.Provider,
				se.StartAndFinishDate,
				se.Country,
				se.PnrAndTourCode,

				Ui.fieldRow(se, se.HotelName, [se.HotelName, se.HotelOffice, se.HotelCode, ]),
				Ui.fieldRow(se, se.PlacementName, [se.PlacementName, se.PlacementOffice, se.PlacementCode, ]),

				Ui.fieldSet(se, null, [se.AccommodationType], [se.CateringType]),

				se.SellerAndOwner,
			],
			"fields2": se.Finance,
			"fields3": se.Note,
		}
	}));

}