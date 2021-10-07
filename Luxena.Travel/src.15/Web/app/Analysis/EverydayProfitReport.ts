module Luxena.Views
{

	export var EverydayProfitReports = (...args) =>
	{
		var filterForm = NewProductFilterController(args, { oneDayOnly: true });

		var se = sd.EverydayProfitReport;

		var grid = new GridController({
			entity: se,
			master: filterForm,
			form: sd.Product,
			smart: null,
			members: [
				se.IssueDate,
				se.ProductType,
				se.Product,
				se.Provider,
				se.Seller.reserve(),
				se.PassengerName.reserve(),
				se.Itinerary,
				se.StartDate.reserve(),
				se.FinishDate.reserve(),
				se.Country.reserve(),
				se.Fare.reserve(),
				se.Currency,
				se.CurrencyRate,
				se.EqualFare.totalSum(),
				se.FeesTotal.totalSum(),
				se.CancelFee.totalSum(),
				se.Total.totalSum(),
				se.Commission.totalSum(),
				se.ServiceFee.totalSum(),
				se.Vat.totalSum(),
				se.GrandTotal.totalSum(),
				se.Payer,
				se.InvoiceDate,
				se.CompletionCertificateDate,
				se.PaymentDate,
				se.Order.reserve(),
				se.Invoice.reserve(),
				se.CompletionCertificate.reserve(),
				se.Payment.reserve(),
			],

			fixed: true,
			wide: true,
		});


		return filterForm.getScopeWithGrid(grid);
	};

}