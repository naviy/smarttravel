module Luxena.Views
{

	export var ProfitDistributionByCustomers = (...args) =>
	{
		var filterForm = NewProductFilterController(args);

		var se = sd.ProfitDistributionByCustomer;

		var grid = new GridController({
			entity: se,
			master: filterForm,
			members: [
				se.Rank,
				se.Customer,

				se.SellCount.totalSum(),
				se.RefundCount.totalSum(),
				se.VoidCount.totalSum(),

				se.Currency,

				se.SellGrandTotal.totalSum(),
				se.RefundGrandTotal.totalSum(),
				se.GrandTotal.totalSum(),
				se.Total.totalSum(),
				se.ServiceFee.totalSum(),
				se.Commission.totalSum(),
				se.AgentTotal.totalSum(),
				se.Vat.totalSum(),
			],

			fixed: true,
			useGrouping: false,
			wide: true,
		});


		return filterForm.getScopeWithGrid(grid);
	};

}