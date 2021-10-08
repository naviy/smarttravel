module Luxena.Views
{

	registerEntityControllers(sd.Tour, se => ({

		list: [
			se.IssueDate,
			se.Name,
			se.PassengerName,
			se.Producer,
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

				se.Producer,
				se.StartDate,
				se.FinishDate, 
				se.Country,
				se.PnrCode,
				se.TourCode,

				se.HotelName,
				se.HotelOffice,
				se.HotelCode,

				se.PlacementName,
				se.PlacementOffice,
				se.PlacementCode,

				se.AccommodationType,
				se.CateringType,

				se.AviaDescription,
				se.TransferDescription,

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

				se.Producer,
				se.StartAndFinishDate,
				se.Country,
				se.PnrAndTourCode,

				sd.row(se.HotelName, se.HotelOffice, se.HotelCode).title(se.HotelName),
				sd.row(se.PlacementName, se.PlacementOffice, se.PlacementCode).title(se.PlacementName),

				sd.row(sd.col(se.AccommodationType), sd.col(se.CateringType)),

				se.AviaDescription,
				se.TransferDescription,

				se.SellerAndOwner,
			],

			"fields2": se.Finance,

			"fields3": se.Note,
		},
	}));

}