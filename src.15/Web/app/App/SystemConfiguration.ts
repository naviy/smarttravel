module Luxena.Views
{
	registerEntityControllers(sd.SystemConfiguration, se => ({
		form: {
			Company: [
				se.Company,
				se.CompanyDetails,
				se.AccountantDisplayString,
				se.Country,
				se.DefaultCurrency,
				se.UseDefaultCurrencyForInput,
				se.VatRate,
			],

			Products: [
				se.UseAviaHandling,
				se.IsPassengerPassportRequired,
				se.AviaDocumentVatOptions,
				se.NeutralAirlineCode,
			],

			Orders: [
				se.AviaOrderItemGenerationOption,
				se.AmadeusRizUsingMode,
				se.IncomingCashOrderCorrespondentAccount,
				se.DaysBeforeDeparture,
				se.MetricsFromDate,
				se.UseAviaDocumentVatInOrder,
				se.AllowAgentSetOrderVat,
				se.SeparateDocumentAccess,
				se.IsOrderRequiredForProcessedDocument,
				se.ReservationsInOfficeMetrics,
				se.McoRequiresDescription,
				se.Order_UseServiceFeeOnlyInVat,
			],

			Invoices: [
				se.Invoice_NumberMode,
				se.InvoicePrinter_FooterDetails,
			],

			Rest: [
				se.BirthdayTaskResponsible,
				se.IsOrganizationCodeRequired,
			],
		},

		formScope: ctrl => ({
			tabs: [
				{ template: "Company", title: "Турагенство", },
				sd.Product.toTabs(),
				sd.Order.toTabs(),
				sd.Invoice.toTabs(),
				{ template: "Rest", title: "Прочее", },
			]
		}),

	}));



}