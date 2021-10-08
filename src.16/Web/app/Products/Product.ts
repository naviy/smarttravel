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

		totalCharts1?: (
			masterMember: (se: IProductSemantic) => SemanticMember,
			chart: IProductTotalByDateSemantic
		) => Field;

		totalGrid1?: (
			masterMember?: (se: IProductSemantic) => SemanticMember
		) => Field;

		totalTab1?: (
			title: string,
			masterMember: (se: IProductSemantic) => SemanticMember,
			chart: IProductTotalByDateSemantic
		) => Field;

	}


	$doForDerived(sd.Product, se =>
	{

		se.entityStatus(r =>
		{
			if (se.IsVoid.get(r))
				return "disabled";

			return se.RequiresProcessing.get(r) === true ? "error" : null;
		});

		se.IssueDateAndReissueFor = se.row(sd.col(se.IssueDate), sd.col(se.ReissueFor));

		se.PassengerRow = se.row(se.GdsPassengerName, se.Passenger).title(se.Passenger);

		se.CustomerAndOrder = se.row(se.Customer, se.Order);

		se.StartAndFinishDate = se.row(se.StartDate, se.FinishDate);

		se.PnrAndTourCode = se.row(se.PnrCode, se.TourCode);

		se.BookerRow = se.row(se.Booker, se.BookerOffice, se.BookerCode).title(se.Booker);

		se.TicketerRow = se.row(se.Ticketer, se.TicketerOffice, se.TicketerCode).title(se.Ticketer);

		se.BookerAndTicketer = se.row(se.Booker, se.Ticketer);

		se.SellerAndOwner = se.row(se.Seller, se.Owner);

		se.Finance = se.col(
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
			se.TaxRateOfProduct,
			se.TaxRateOfServiceFee
		);


		se.totalCharts1 = (masterMember, chart) => chart
			.chart(masterMember(se))
			.chartController({
				argument: chart.Date,
				value: [chart.SumGrandTotal, chart.GrandTotal, ],
				zoom: true,

				chartOptions: {
					series: [
						{ pane: "0", },
						{ pane: "1", type: "bar", },
					],
				},
			});

		se.totalGrid1 = (masterMember) =>
			se.grid(masterMember && masterMember(se))
				.items(
					se.IssueDate,
					se.Type,
					se.Name,
					//se.Total,
					//se.ServiceFee,
					se.GrandTotal,
					se.Order,
					se.RequiresProcessing.hidden()
				)
				.gridController({ /*small: true, */height: 550, useFilterRow: false });

		se.totalTab1 = (title, masterMember, chart) =>
			sd.col().title(title).unlabelItems().items(
				sd.header(chart),
				se.totalCharts1(masterMember, chart),
				sd.hr2(),
				se.totalGrid1(masterMember)
			);

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