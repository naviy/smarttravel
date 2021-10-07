module Luxena.Views
{
	registerEntityControllers(sd.SystemConfiguration, se => ({

		form: () => sd.tabCard(

			sd.col().title("Турагенство").items(
				se.Company,
				se.CompanyDetails,
				se.AccountantDisplayString,
				se.Country,
				se.DefaultCurrency,
				se.UseDefaultCurrencyForInput,
				se.VatRate
			),

			sd.col().titleForList(sd.Product).items(
				se.UseAviaHandling,
				se.IsPassengerPassportRequired,
				se.AviaDocumentVatOptions,
				se.NeutralAirlineCode
			),

			sd.col()
				.titleForList(sd.Order)
                .addRowClass("field-label-width-300")
				.items(
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
					se.Order_UseServiceFeeOnlyInVat
				),

			sd.col().titleForList(sd.Invoice).items(
				se.Invoice_NumberMode,
				se.InvoicePrinter_FooterDetails
			),

			sd.col().title("Прочее").items(
				se.BirthdayTaskResponsible,
				se.IsOrganizationCodeRequired
			)
		),

	}));



}