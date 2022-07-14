using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;




namespace Luxena.Travel.Domain.NHibernate.Mapping.Configuration
{



	//===g






	public sealed class SystemConfigurationMap : ClassMapping<SystemConfiguration>
	{

		//---g



		public SystemConfigurationMap()
		{

			Cache(x => x.Usage(CacheUsage.ReadWrite));

			Id(x => x.Id, Uuid.Mapping);

			Version(x => x.Version, m => { });

			Property(x => x.ModifiedBy, m => m.Length(32));

			Property(x => x.ModifiedOn);

			ManyToOne(x => x.Company);

			Property(x => x.CompanyDetails, m => m.Type(NHibernateUtil.StringClob));

			ManyToOne(x => x.BirthdayTaskResponsible);

			Property(x => x.AmadeusRizUsingMode, m => m.NotNullable(true));

			Property(x => x.VatRate, m => m.NotNullable(true));

			ManyToOne(x => x.DefaultCurrency, m =>
			{
				m.NotNullable(true);
				m.Lazy(LazyRelation.NoLazy);
			});

			Property(x => x.UseDefaultCurrencyForInput, m => m.NotNullable(true));

			ManyToOne(x => x.Country, m => m.Lazy(LazyRelation.NoLazy));

			Property(x => x.IsPassengerPassportRequired, m => m.NotNullable(true));

			Property(x => x.AviaOrderItemGenerationOption, m => m.NotNullable(true));

			Property(x => x.SeparateDocumentAccess, m => m.NotNullable(true));

			Property(x => x.AllowOtherAgentsToModifyProduct, m => m.NotNullable(true));

			Property(x => x.AllowAgentSetOrderVat, m => m.NotNullable(true));

			Property(x => x.UseAviaDocumentVatInOrder, m => m.NotNullable(true));

			Property(x => x.AviaDocumentVatOptions, m => m.NotNullable(true));

			Property(x => x.IncomingCashOrderCorrespondentAccount, m => m.NotNullable(true));

			Property(x => x.AccountantDisplayString);


			Property(x => x.UseConsolidatorCommission, m => m.NotNullable(true));

			Component(x => x.DefaultConsolidatorCommission);

			Property(x => x.UseAviaHandling, m => m.NotNullable(true));
			
			Property(x => x.UseBonuses, m => m.NotNullable(true));


			Property(x => x.IsOrganizationCodeRequired, m => m.NotNullable(true));

			Property(x => x.DaysBeforeDeparture, m => m.NotNullable(true));

			Property(x => x.IsOrderRequiredForProcessedDocument, m => m.NotNullable(true));

			Property(x => x.MetricsFromDate, m => m.Type<UtcKindDateType>());

			Property(x => x.ReservationsInOfficeMetrics, m => m.NotNullable(true));

			Property(x => x.McoRequiresDescription, m => m.NotNullable(true));

			Property(x => x.NeutralAirlineCode);

			Property(x => x.Order_UseServiceFeeOnlyInVat, m => m.NotNullable(true));

			Property(x => x.Consignment_NumberMode, m => m.NotNullable(true));

			Property(x => x.Invoice_NumberMode, m => m.NotNullable(true));
			Property(x => x.InvoicePrinter_ShowVat, m => m.NotNullable(true));
			Property(x => x.InvoicePrinter_FooterDetails);
			Property(x => x.Invoice_CanOwnerSelect, m => m.NotNullable(true));
			
			Property(x => x.DrctWebService_LoadedOn);
			Property(x => x.GalileoWebService_LoadedOn);
			Property(x => x.GalileoRailWebService_LoadedOn);
			Property(x => x.GalileoBusWebService_LoadedOn);
			Property(x => x.TravelPointWebService_LoadedOn);

			Property(x => x.Consignment_SeparateBookingFee);
			Property(x => x.Pasterboard_DefaultPaymentType);

			Property(x => x.Ticket_NoPrintReservations);

		}



		//---g

	}






	//===g



}