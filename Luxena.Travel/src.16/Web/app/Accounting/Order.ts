module Luxena
{

	export interface IOrderSemantic
	{

		totalCharts1?: (
			masterMember: (se: IOrderSemantic) => SemanticMember,
			chart: IOrderTotalByDateSemantic
		) => Field;

		totalGrid1?: (
			masterMember?: (se: IOrderSemantic) => SemanticMember
		) => Field;

		totalTab1?: (
			title: string,
			masterMember: (se: IOrderSemantic) => SemanticMember,
			chart: IOrderTotalByDateSemantic
		) => Field;

	}
	

	$do(sd.Order, se =>
	{

		se.entityStatus(r =>
		{
			if (se.IsVoid.get(r))
				return "disabled";

			const totalDue = se.TotalDue.get(r);
			if (totalDue === undefined) return null;

			return totalDue && totalDue.Amount > 0 ? "error" : "success";
		});


		se.totalCharts1 = (masterMember, chart) => chart
			.chart(masterMember(se))
			.chartController({
				argument: chart.Date,
				value: [chart.SumTotal, chart.Total, ],
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
				.items(se.Number, se.IssueDate, se.Total, se.TotalDue, se.AssignedTo)
				.gridController({ small: true, height: 550, });

		se.totalTab1 = (title, masterMember, chart) =>
			sd.col().icon(se).title(title).unlabelItems().items(
				sd.header(chart),
				se.totalCharts1(masterMember, chart),
				sd.hr2(),
				se.totalGrid1(masterMember)
		);

	});
}


module Luxena.Views
{
	registerEntityControllers(sd.Order, se => ({
		list: [
			se.IssueDate,
			se.Number,
			se.Customer,
			se.Total,
			////se.Vat,
			se.Paid.hidden(),
			se.TotalDue,

			//Ui.moneyProgress(se, se.Paid, se.Total),

			//se.DeliveryBalance,
			se.AssignedTo,
		],

		view: {
			"fields1": [
				se.Number,
				se.IssueDate,
				se.Customer,
				se.BillTo,
				se.ShipTo,
				se.AssignedTo,
				se.Owner,
				se.BankAccount,
				se.IsPublic,
				se.IsSubjectOfPaymentsControl,
				se.Note,
			],
			"fields2": [
				Ui.moneyProgress(se, se.Paid, se.Total),
				se.Total,
				se.ServiceFee,
				se.Discount,
				se.Vat,
				se.Paid,
				se.TotalDue,
				se.VatDue,
			],
		},

		smart: ({
			"fields": [
				se.IssueDate,
				se.Customer,
				se.BillTo,
				se.ShipTo,
				se.AssignedTo,
				se.Owner,
				se.Total,
				se.Paid,
				se.TotalDue,
				se.Note,
			],
			buttons: [
			]
		}),


		smartConfig: {
			//contentHeight: 100,
			//scontentWidth: 400,
		},

		//viewScope: ctrl => ({
		//	tabs: [
		//		se.Items.toGridTab(ctrl, a => [a.Position, a.Product, a.Text, a.GrandTotal, a.Consignment, ]),
		//		se.Payments.toGridTab(ctrl, a => [a.Date, a.Number, a.DocumentNumber, a.PostedOn, a.Payer, a.RegisteredBy, a.Amount, a.Note, ]),
		//		se.IncomingTransfers.toGridTab(ctrl, a => [a.Date, a.Number, a.FromOrder, a.FromParty, a.Amount]),
		//		se.OutgoingTransfers.toGridTab(ctrl, a => [a.Date, a.Number, a.ToOrder, a.ToParty, a.Amount]),
		//	]
		//}),


		edit: {
			"fields": [
				se.IssueDate,
				se.Number,
				se.Customer,
				se.BillTo,
				se.ShipTo,
				sd.row(se.AssignedTo, se.Owner),
				se.BankAccount,
				se.IsPublic,
				se.IsSubjectOfPaymentsControl,
				se.SeparateServiceFee,
				se.Note,
			],
		},

	}));

}