module Luxena
{
	
	export interface IProductSemantic
	{
		IssueDateAndReissueFor?: SemanticMember;
		PassengerRow?: SemanticMember;
		CustomerAndOrder?: SemanticMember;
		StartAndFinishDate?: SemanticMember;
		PnrAndTourCode?: SemanticMember;
		BookerRow?: SemanticMember;
		TicketerRow?: SemanticMember;
		BookerAndTicketer?: SemanticMember;
		SellerAndOwner?: SemanticMember;

		Finance?: SemanticMember;
	}


	Ui.fieldSet2(sd.Product, {
		name: "IssueDateAndReissueFor",
		members: se => [se.IssueDate],
		members2: se => [se.ReissueFor],
	});

	Ui.fieldRow2(sd.Product, {
		name: "PassengerRow",
		title: se => se.Passenger,
		members: se => [se.GdsPassengerName, se.Passenger,],
	});

	Ui.fieldRow2(sd.Product, {
		name: "CustomerAndOrder",
		members: se => [se.Customer, se.Order, ],
	});

	Ui.fieldSet2(sd.Product, {
		name: "StartAndFinishDate",
		members: se => [se.StartDate, se.FinishDate],
	});

	Ui.fieldRow2(sd.Product, {
		name: "PnrAndTourCode",
		members: se => [se.PnrCode, se.TourCode, ],
	});

	Ui.fieldRow2(sd.Product, {
		name: "BookerRow",
		title: se => se.Booker,
		members: se => [se.Booker, se.BookerOffice, se.BookerCode, ],
	});

	Ui.fieldRow2(sd.Product, {
		name: "TicketerRow",
		title: se => se.Ticketer,
		members: se => [se.Ticketer, se.TicketerOffice, se.TicketerCode, ],
	});

	Ui.fieldRow2(sd.Product, {
		name: "BookerAndTicketer",
		members: se => [se.Booker, se.Ticketer, ],
	});

	Ui.fieldRow2(sd.Product, {
		name: "SellerAndOwner",
		members: se => [se.Seller, se.Owner]
	});


	Ui.fieldSet2(sd.Product, {
		name: "Finance",
		members: se => [
			se.Fare,
			se.EqualFare,
			se.FeesTotal,
			se.Total,
			se.Vat,
			se.Commission,
			se.CommissionDiscount,
			se.ServiceFee,
			se.Handling,
			se.Discount,
			//a.BonusDiscount,
			//a.BonusAccumulation,
			se.GrandTotal,
			se.PaymentType,
			se.TaxMode,
		]
	});

}

module Luxena.Views
{

	registerEntityControllers(sd.Product, se => ({

		list: [
			se.IssueDate,
			se.Type,
			se.Name,

			se.PassengerName,
			se.Order,
			//se.Customer,
			se.Seller,
			//se.Producer,
			//se.Provider,
			//se.Country.ToColumn(true),
			//se.PnrCode.ToColumn(true),
			//se.TourCode.ToColumn(true),
			//se.TicketingIataOffice,
			
			se.Total,
			se.ServiceFee,
			se.GrandTotal,
		],

	}));


}