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
				se.Seller.reserved(),
				se.PassengerName.reserved(),
				se.Itinerary,
				se.StartDate.reserved(),
				se.FinishDate.reserved(),
				se.Country.reserved(),
				se.Fare.reserved(),
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
				se.Order.reserved(),
				se.Invoice.reserved(),
				se.CompletionCertificate.reserved(),
				se.Payment.reserved(),
			],

			fixed: true,
			wide: true,
		});


		return filterForm.getScopeWithGrid(grid);
	};

}