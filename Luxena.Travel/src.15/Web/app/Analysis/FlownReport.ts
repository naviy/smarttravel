module Luxena.Views
{

	export var FlownReports = (...args) =>
	{
		var filterForm = NewProductFilterController(args, { oneMonth: true });

		var se = sd.FlownReport;
		
		var grid = new GridController({
			entity: se,
			master: filterForm,
			form: sd.Product,
			smart: null,
			members: [
				se.Date.fixed(),
				se.Op.fixed(),
				se.AC.fixed(),
				se.TicketNumber.fixed(),
				se.Client,
				se.Passenger,
				se.Route,
				se.Curr,
				se.Fare,
				se.Tax,

				se.Flown1,
				se.Flown2,
				se.Flown3,
				se.Flown4,
				se.Flown5,
				se.Flown6,
				se.Flown7,
				se.Flown8,
				se.Flown9,
				se.Flown10,
				se.Flown11,
				se.Flown12,

				se.TourCode,
				se.CheapTicket,
			],

			//fixed: true,
			useFilter: false,
			useGrouping: false,
			wide: true,
		});


		return filterForm.getScopeWithGrid(grid);
	};

}