module Luxena
{

	export interface IAviaDocumentSemantic
	{
		NumberRow?: SemanticMember;
	}


	$doForDerived(sd.AviaDocument, se =>
		se.NumberRow = se.row(se.AirlinePrefixCode, se.Number, se.Producer).title("Номер / Авиакомпания")
	);

}


module Luxena.Views
{

	registerEntityControllers(sd.AviaDocument, se => ({
		list: [
			se.IssueDate,
			se.Type,
			se.Name,
			se.PassengerName,
			se.Itinerary,
			se.Customer,
			se.Order,
			se.Total,
			se.ServiceFee,
			se.GrandTotal,
		],
	}));


	registerEntityControllers([sd.AviaRefund, sd.AviaMco], se => ({

		list: sd.AviaDocument,

		view: {
			"fields1": [
				se.IssueDate,
				se.ReissueFor,

				sd.row(se.FullNumber, se.Producer),

				se.PassengerRow,
				se.Itinerary,
				se.CustomerAndOrder,
				se.Intermediary,

				se.GdsPassportStatus,

				se.PnrAndTourCode,

				se.BookerAndTicketer,
				se.SellerAndOwner,
				se.OriginalDocument,
			],
			"fields2": se.Finance, 
			"fields3": se.Note,
		},

		edit: {
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
		},
	}));

}