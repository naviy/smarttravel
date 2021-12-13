using System;

using Luxena.Domain.Contracts;




namespace Luxena.Travel.Domain
{



	//===g






	public partial class SystemConfigurationDto : EntityContract
	{

		//---g



		public DateTime? ModifiedOn { get; set; }

		public string ModifiedBy { get; set; }

		public Organization.Reference Company { get; set; }

		public string CompanyName { get; set; }

		public string CompanyDetails { get; set; }

		public Country.Reference Country { get; set; }

		public Currency.Reference DefaultCurrency { get; set; }

		public bool UseDefaultCurrencyForInput { get; set; }

		public decimal VatRate { get; set; }

		public int AmadeusRizUsingMode { get; set; }

		public bool IsPassengerPassportRequired { get; set; }

		public int AviaOrderItemGenerationOption { get; set; }

		public bool AllowAgentSetOrderVat { get; set; }

		public bool UseAviaDocumentVatInOrder { get; set; }

		public int AviaDocumentVatOptions { get; set; }

		public string AccountantDisplayString { get; set; }

		public string IncomingCashOrderCorrespondentAccount { get; set; }

		public bool SeparateDocumentAccess { get; set; }

		public bool AllowOtherAgentsToModifyProduct { get; set; }

		public bool IsOrganizationCodeRequired { get; set; }

		public bool UseConsolidatorCommission { get; set; }

		public MoneyDto DefaultConsolidatorCommission { get; set; }

		public bool UseAviaHandling { get; set; }

		public bool UseBonuses { get; set; }

		public int DaysBeforeDeparture { get; set; }

		public Person.Reference BirthdayTaskResponsible { get; set; }

		public bool IsOrderRequiredForProcessedDocument { get; set; }

		public DateTime? MetricsFromDate { get; set; }

		public bool ReservationsInOfficeMetrics { get; set; }

		public bool McoRequiresDescription { get; set; }

		public string NeutralAirlineCode { get; set; }

		public bool Order_UseServiceFeeOnlyInVat { get; set; }

		public int Invoice_NumberMode { get; set; }
		public int Consignment_NumberMode { get; set; }

		public bool InvoicePrinter_ShowVat { get; set; }
		public string InvoicePrinter_FooterDetails { get; set; }

		public DateTime? DrctWebService_LoadedOn { get; set; }
		public DateTime? GalileoWebService_LoadedOn { get; set; }
		public DateTime? GalileoRailWebService_LoadedOn { get; set; }
		public DateTime? GalileoBusWebService_LoadedOn { get; set; }
		public DateTime? TravelPointWebService_LoadedOn { get; set; }
		
		public bool Consignment_SeparateBookingFee { get; set; }

		public int? Pasterboard_DefaultPaymentType { get; set; }

		public bool Ticket_NoPrintReservations { get; set; }



		//---g

	}






	//===g






	public partial class SystemConfigurationContractService : EntityContractService<SystemConfiguration, SystemConfiguration.Service, SystemConfigurationDto>
	{

		//---g



		public SystemConfigurationContractService()
		{

			ContractFromEntity += (r, c) =>
			{

				c.ModifiedOn = r.ModifiedOn;
				c.ModifiedBy = r.ModifiedBy;
				c.Company = r.Company;
				c.CompanyName = r.CompanyName;
				c.CompanyDetails = r.CompanyDetails;
				c.Country = r.Country;
				c.DefaultCurrency = r.DefaultCurrency;
				c.UseDefaultCurrencyForInput = r.UseDefaultCurrencyForInput;
				c.VatRate = r.VatRate;
				c.AmadeusRizUsingMode = (int)r.AmadeusRizUsingMode;
				c.IsPassengerPassportRequired = r.IsPassengerPassportRequired;
				c.AviaOrderItemGenerationOption = (int)r.AviaOrderItemGenerationOption;
				c.AllowAgentSetOrderVat = r.AllowAgentSetOrderVat;
				c.UseAviaDocumentVatInOrder = r.UseAviaDocumentVatInOrder;
				c.AviaDocumentVatOptions = (int)r.AviaDocumentVatOptions;
				c.AccountantDisplayString = r.AccountantDisplayString;
				c.IncomingCashOrderCorrespondentAccount = r.IncomingCashOrderCorrespondentAccount;
				c.SeparateDocumentAccess = r.SeparateDocumentAccess;
				c.AllowOtherAgentsToModifyProduct = r.AllowOtherAgentsToModifyProduct;
				c.IsOrganizationCodeRequired = r.IsOrganizationCodeRequired;
				c.UseConsolidatorCommission = r.UseConsolidatorCommission;
				c.DefaultConsolidatorCommission = r.DefaultConsolidatorCommission;
				c.UseAviaHandling = r.UseAviaHandling;
				c.UseBonuses = r.UseBonuses;
				c.DaysBeforeDeparture = r.DaysBeforeDeparture;
				c.BirthdayTaskResponsible = r.BirthdayTaskResponsible;
				c.IsOrderRequiredForProcessedDocument = r.IsOrderRequiredForProcessedDocument;
				c.MetricsFromDate = r.MetricsFromDate;
				c.ReservationsInOfficeMetrics = r.ReservationsInOfficeMetrics;
				c.McoRequiresDescription = r.McoRequiresDescription;
				c.NeutralAirlineCode = r.NeutralAirlineCode;
				c.Order_UseServiceFeeOnlyInVat = r.Order_UseServiceFeeOnlyInVat;
				c.Invoice_NumberMode = (int)r.Invoice_NumberMode;
				c.Consignment_NumberMode = (int) r.Consignment_NumberMode;
				c.InvoicePrinter_ShowVat = r.InvoicePrinter_ShowVat;
				c.InvoicePrinter_FooterDetails = r.InvoicePrinter_FooterDetails;
				c.DrctWebService_LoadedOn = r.DrctWebService_LoadedOn;
				c.GalileoWebService_LoadedOn = r.GalileoWebService_LoadedOn;
				c.GalileoRailWebService_LoadedOn = r.GalileoRailWebService_LoadedOn;
				c.GalileoBusWebService_LoadedOn = r.GalileoBusWebService_LoadedOn;
				c.TravelPointWebService_LoadedOn = r.TravelPointWebService_LoadedOn;
				c.Consignment_SeparateBookingFee = r.Consignment_SeparateBookingFee;
				c.Pasterboard_DefaultPaymentType = (int?)r.Pasterboard_DefaultPaymentType;
				c.Ticket_NoPrintReservations = r.Ticket_NoPrintReservations;

			};



			EntityFromContract += (r, c) =>
			{

				r.ModifiedOn = DateTime.Now;
				r.ModifiedBy = db.Security.User.Name;

				r.Company = c.Company + db;
				r.CompanyDetails = c.CompanyDetails + db;
				r.Country = c.Country + db;
				r.DefaultCurrency = c.DefaultCurrency + db;
				r.UseDefaultCurrencyForInput = c.UseDefaultCurrencyForInput + db;
				r.VatRate = c.VatRate + db;
				r.AmadeusRizUsingMode = (AmadeusRizUsingMode)c.AmadeusRizUsingMode + db;
				r.IsPassengerPassportRequired = c.IsPassengerPassportRequired + db;
				r.AviaOrderItemGenerationOption = (AviaOrderItemGenerationOption)c.AviaOrderItemGenerationOption + db;
				r.AllowAgentSetOrderVat = c.AllowAgentSetOrderVat + db;
				r.UseAviaDocumentVatInOrder = c.UseAviaDocumentVatInOrder + db;
				r.AviaDocumentVatOptions = (AviaDocumentVatOptions)c.AviaDocumentVatOptions + db;
				r.AccountantDisplayString = c.AccountantDisplayString + db;
				r.IncomingCashOrderCorrespondentAccount = c.IncomingCashOrderCorrespondentAccount + db;
				r.SeparateDocumentAccess = c.SeparateDocumentAccess + db;
				r.AllowOtherAgentsToModifyProduct = c.AllowOtherAgentsToModifyProduct + db;
				r.IsOrganizationCodeRequired = c.IsOrganizationCodeRequired + db;
				r.UseConsolidatorCommission = c.UseConsolidatorCommission + db;
				r.DefaultConsolidatorCommission = c.DefaultConsolidatorCommission + db;
				r.UseAviaHandling = c.UseAviaHandling + db;
				r.UseBonuses = c.UseBonuses + db;
				r.DaysBeforeDeparture = c.DaysBeforeDeparture + db;
				r.BirthdayTaskResponsible = c.BirthdayTaskResponsible + db;
				r.IsOrderRequiredForProcessedDocument = c.IsOrderRequiredForProcessedDocument + db;
				r.MetricsFromDate = c.MetricsFromDate + db;
				r.ReservationsInOfficeMetrics = c.ReservationsInOfficeMetrics + db;
				r.McoRequiresDescription = c.McoRequiresDescription + db;
				r.NeutralAirlineCode = c.NeutralAirlineCode + db;
				r.Order_UseServiceFeeOnlyInVat = c.Order_UseServiceFeeOnlyInVat + db;
				r.Invoice_NumberMode = (InvoiceNumberMode)c.Invoice_NumberMode + db;
				r.Consignment_NumberMode = (InvoiceNumberMode)c.Consignment_NumberMode + db;
				r.InvoicePrinter_ShowVat = c.InvoicePrinter_ShowVat + db;
				r.InvoicePrinter_FooterDetails = c.InvoicePrinter_FooterDetails + db;
				r.DrctWebService_LoadedOn = c.DrctWebService_LoadedOn + db;
				r.GalileoWebService_LoadedOn = c.GalileoWebService_LoadedOn + db;
				r.TravelPointWebService_LoadedOn = c.TravelPointWebService_LoadedOn + db;
				r.GalileoRailWebService_LoadedOn = c.GalileoRailWebService_LoadedOn + db;
				r.GalileoBusWebService_LoadedOn = c.GalileoBusWebService_LoadedOn + db;
				r.Consignment_SeparateBookingFee = c.Consignment_SeparateBookingFee + db;
				r.Pasterboard_DefaultPaymentType = (PaymentType?)c.Pasterboard_DefaultPaymentType + db;
				r.Ticket_NoPrintReservations = c.Ticket_NoPrintReservations + db;

			};

		}



		//---g

	}






	//===g



}