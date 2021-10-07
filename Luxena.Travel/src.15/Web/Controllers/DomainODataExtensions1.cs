using System.Web.OData.Builder;

using Luxena.Domain.Web;


namespace Luxena.Travel.Web
{

	using Domain;
	

	#region Entity Controllers

	public partial class AccommodationsController: EntityODataController<Domain, Accommodation, string> { }

	public partial class AccommodationTypesController: EntityODataController<Domain, AccommodationType, string> { }

	public partial class AirlineServiceClassesController: EntityODataController<Domain, AirlineServiceClass, string> { }

	public partial class AirportsController: EntityODataController<Domain, Airport, string> { }

	public partial class AviaDocumentsController: ReadOnlyEntityODataController<Domain, AviaDocument, string> { }

	public partial class AviaMcosController: EntityODataController<Domain, AviaMco, string> { }

	public partial class AviaRefundsController: EntityODataController<Domain, AviaRefund, string> { }

	public partial class AviaTicketsController: EntityODataController<Domain, AviaTicket, string> { }

	public partial class BankAccountsController: EntityODataController<Domain, BankAccount, string> { }

	public partial class BusDocumentsController: ReadOnlyEntityODataController<Domain, BusDocument, string> { }

	public partial class BusTicketRefundsController: EntityODataController<Domain, BusTicketRefund, string> { }

	public partial class BusTicketsController: EntityODataController<Domain, BusTicket, string> { }

	public partial class CarRentalsController: EntityODataController<Domain, CarRental, string> { }

	public partial class CashInOrderPaymentsController: EntityODataController<Domain, CashInOrderPayment, string> { }

	public partial class CashOutOrderPaymentsController: EntityODataController<Domain, CashOutOrderPayment, string> { }

	public partial class CateringTypesController: EntityODataController<Domain, CateringType, string> { }

	public partial class CheckPaymentsController: EntityODataController<Domain, CheckPayment, string> { }

	public partial class ConsignmentsController: EntityODataController<Domain, Consignment, string> { }

	public partial class CountriesController: EntityODataController<Domain, Country, string> { }

	public partial class CurrencyDailyRatesController: EntityODataController<Domain, CurrencyDailyRate, string> { }

	public partial class DepartmentsController: EntityODataController<Domain, Department, string> { }

	public partial class DocumentAccessesController: EntityODataController<Domain, DocumentAccess, string> { }

	public partial class DocumentOwnersController: EntityODataController<Domain, DocumentOwner, string> { }

	public partial class ElectronicPaymentsController: EntityODataController<Domain, ElectronicPayment, string> { }

	public partial class ExcursionsController: EntityODataController<Domain, Excursion, string> { }

	public partial class FilesController: EntityODataController<Domain, File, string> { }

	public partial class FlightSegmentsController: EntityODataController<Domain, FlightSegment, string> { }

	public partial class GdsAgentsController: EntityODataController<Domain, GdsAgent, string> { }

	public partial class GdsFilesController: ReadOnlyEntityODataController<Domain, GdsFile, string> { }

	public partial class GenericProductsController: EntityODataController<Domain, GenericProduct, string> { }

	public partial class GenericProductTypesController: EntityODataController<Domain, GenericProductType, string> { }

	public partial class IdentitiesController: ReadOnlyEntityODataController<Domain, Identity, string> { }

	public partial class InsuranceDocumentsController: ReadOnlyEntityODataController<Domain, InsuranceDocument, string> { }

	public partial class InsuranceRefundsController: EntityODataController<Domain, InsuranceRefund, string> { }

	public partial class InsurancesController: EntityODataController<Domain, Insurance, string> { }

	public partial class InternalIdentitiesController: EntityODataController<Domain, InternalIdentity, string> { }

	public partial class InternalTransfersController: EntityODataController<Domain, InternalTransfer, string> { }

	public partial class InvoicesController: EntityODataController<Domain, Invoice, string> { }

	public partial class IsicsController: EntityODataController<Domain, Isic, string> { }

	public partial class IssuedConsignmentsController: EntityODataController<Domain, IssuedConsignment, string> { }

	public partial class MilesCardsController: EntityODataController<Domain, MilesCard, string> { }

	public partial class OrderChecksController: EntityODataController<Domain, OrderCheck, string> { }

	public partial class OrderItemsController: EntityODataController<Domain, OrderItem, string> { }

	public partial class OrdersController: EntityODataController<Domain, Order, string> { }

	public partial class OrganizationsController: EntityODataController<Domain, Organization, string> { }

	public partial class PartiesController: ReadOnlyEntityODataController<Domain, Party, string> { }

	public partial class PassportsController: EntityODataController<Domain, Passport, string> { }

	public partial class PasteboardRefundsController: EntityODataController<Domain, PasteboardRefund, string> { }

	public partial class PasteboardsController: EntityODataController<Domain, Pasteboard, string> { }

	public partial class PaymentsController: ReadOnlyEntityODataController<Domain, Payment, string> { }

	public partial class PaymentSystemsController: EntityODataController<Domain, PaymentSystem, string> { }

	public partial class PersonsController: EntityODataController<Domain, Person, string> { }

	public partial class ProductPassengersController: EntityODataController<Domain, ProductPassenger, string> { }

	public partial class ProductsController: ReadOnlyEntityODataController<Domain, Product, string> { }

	public partial class RailwayDocumentsController: ReadOnlyEntityODataController<Domain, RailwayDocument, string> { }

	public partial class SequencesController: EntityODataController<Domain, Sequence, string> { }

	public partial class SimCardsController: EntityODataController<Domain, SimCard, string> { }

	public partial class SystemConfigurationsController: EntityODataController<Domain, SystemConfiguration, string> { }

	public partial class ToursController: EntityODataController<Domain, Tour, string> { }

	public partial class TransfersController: EntityODataController<Domain, Transfer, string> { }

	public partial class UsersController: EntityODataController<Domain, User, string> { }

	public partial class WireTransfersController: EntityODataController<Domain, WireTransfer, string> { }

	#endregion


	#region EntityQuery Controllers

	public partial class AccommodationProvidersController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.AccommodationProviders;
		}
	}

	public partial class ActiveOwnersController: ReadOnlyEntityODataController<Domain, Party, string> 
	{ 
		protected override Domain.EntityQuery<Party> GetEntityQuery()
		{
			return db.ActiveOwners;
		}
	}

	public partial class AgentsController: EntityODataController<Domain, Person, string> 
	{ 
		protected override Domain.EntityQuery<Person> GetEntityQuery()
		{
			return db.Agents;
		}
	}

	public partial class AirlinesController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.Airlines;
		}
	}

	public partial class BusTicketProvidersController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.BusTicketProviders;
		}
	}

	public partial class CarRentalProvidersController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.CarRentalProviders;
		}
	}

	public partial class CustomersController: ReadOnlyEntityODataController<Domain, Party, string> 
	{ 
		protected override Domain.EntityQuery<Party> GetEntityQuery()
		{
			return db.Customers;
		}
	}

	public partial class GenericProductProvidersController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.GenericProductProviders;
		}
	}

	public partial class InsuranceCompaniesController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.InsuranceCompanies;
		}
	}

	public partial class PasteboardProvidersController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.PasteboardProviders;
		}
	}

	public partial class ReceiptsController: EntityODataController<Domain, Invoice, string> 
	{ 
		protected override Domain.EntityQuery<Invoice> GetEntityQuery()
		{
			return db.Receipts;
		}
	}

	public partial class RoamingOperatorsController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.RoamingOperators;
		}
	}

	public partial class TourProvidersController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.TourProviders;
		}
	}

	public partial class TransferProvidersController: EntityODataController<Domain, Organization, string> 
	{ 
		protected override Domain.EntityQuery<Organization> GetEntityQuery()
		{
			return db.TransferProviders;
		}
	}

	#endregion


	#region Lookup Controllers

	public partial class AccommodationProviderLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public AccommodationProviderLookupController()
		{
			Query = db.AccommodationProviders;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class AccommodationLookupController : EntityLookupODataController<Domain, Accommodation, string, AccommodationLookup>
	{
		public AccommodationLookupController()
		{
			Query = db.Accommodations;
			Lookup = AccommodationLookup.DefaultLookup;
			Select = AccommodationLookup.SelectAndOrderByName;
		}
	}

	public partial class AccommodationTypeLookupController : EntityLookupODataController<Domain, AccommodationType, string, AccommodationTypeLookup>
	{
		public AccommodationTypeLookupController()
		{
			Query = db.AccommodationTypes;
			Lookup = Entity3.Lookup;
			Select = AccommodationTypeLookup.SelectAndOrderByName;
		}
	}

	public partial class ActiveOwnerLookupController : EntityLookupODataController<Domain, Party, string, PartyLookup>
	{
		public ActiveOwnerLookupController()
		{
			Query = db.ActiveOwners;
			Lookup = Entity3.Lookup;
			Select = PartyLookup.SelectAndOrderByName;
		}
	}

	public partial class AgentLookupController : EntityLookupODataController<Domain, Person, string, PersonLookup>
	{
		public AgentLookupController()
		{
			Query = db.Agents;
			Lookup = Entity3.Lookup;
			Select = PersonLookup.SelectAndOrderByName;
		}
	}

	public partial class AirlineLookupController : EntityLookupODataController<Domain, Organization, string, AirlineLookup>
	{
		public AirlineLookupController()
		{
			Query = db.Airlines;
			Lookup = Airline.Lookup;
			Select = AirlineLookup.SelectAndOrderByName;
		}
	}

	public partial class AirlineServiceClassLookupController : EntityLookupODataController<Domain, AirlineServiceClass, string, AirlineServiceClassLookup>
	{
		public AirlineServiceClassLookupController()
		{
			Query = db.AirlineServiceClasses;
			Lookup = AirlineServiceClassLookup.DefaultLookup;
			Select = AirlineServiceClassLookup.SelectAndOrderByName;
		}
	}

	public partial class AirportLookupController : EntityLookupODataController<Domain, Airport, string, AirportLookup>
	{
		public AirportLookupController()
		{
			Query = db.Airports;
			Lookup = Entity3.Lookup;
			Select = AirportLookup.SelectAndOrderByName;
		}
	}

	public partial class AviaDocumentLookupController : EntityLookupODataController<Domain, AviaDocument, string, AviaDocumentLookup>
	{
		public AviaDocumentLookupController()
		{
			Query = db.AviaDocuments;
			Lookup = AviaDocumentLookup.DefaultLookup;
			Select = AviaDocumentLookup.SelectAndOrderByName;
		}
	}

	public partial class AviaMcoLookupController : EntityLookupODataController<Domain, AviaMco, string, AviaMcoLookup>
	{
		public AviaMcoLookupController()
		{
			Query = db.AviaMcos;
			Lookup = AviaMcoLookup.DefaultLookup;
			Select = AviaMcoLookup.SelectAndOrderByName;
		}
	}

	public partial class AviaRefundLookupController : EntityLookupODataController<Domain, AviaRefund, string, AviaRefundLookup>
	{
		public AviaRefundLookupController()
		{
			Query = db.AviaRefunds;
			Lookup = AviaRefundLookup.DefaultLookup;
			Select = AviaRefundLookup.SelectAndOrderByName;
		}
	}

	public partial class AviaTicketLookupController : EntityLookupODataController<Domain, AviaTicket, string, AviaTicketLookup>
	{
		public AviaTicketLookupController()
		{
			Query = db.AviaTickets;
			Lookup = AviaTicketLookup.DefaultLookup;
			Select = AviaTicketLookup.SelectAndOrderByName;
		}
	}

	public partial class BankAccountLookupController : EntityLookupODataController<Domain, BankAccount, string, BankAccountLookup>
	{
		public BankAccountLookupController()
		{
			Query = db.BankAccounts;
			Lookup = Entity3.Lookup;
			Select = BankAccountLookup.SelectAndOrderByName;
		}
	}

	public partial class BusDocumentLookupController : EntityLookupODataController<Domain, BusDocument, string, BusDocumentLookup>
	{
		public BusDocumentLookupController()
		{
			Query = db.BusDocuments;
			Lookup = BusDocumentLookup.DefaultLookup;
			Select = BusDocumentLookup.SelectAndOrderByName;
		}
	}

	public partial class BusTicketProviderLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public BusTicketProviderLookupController()
		{
			Query = db.BusTicketProviders;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class BusTicketRefundLookupController : EntityLookupODataController<Domain, BusTicketRefund, string, BusTicketRefundLookup>
	{
		public BusTicketRefundLookupController()
		{
			Query = db.BusTicketRefunds;
			Lookup = BusTicketRefundLookup.DefaultLookup;
			Select = BusTicketRefundLookup.SelectAndOrderByName;
		}
	}

	public partial class BusTicketLookupController : EntityLookupODataController<Domain, BusTicket, string, BusTicketLookup>
	{
		public BusTicketLookupController()
		{
			Query = db.BusTickets;
			Lookup = BusTicketLookup.DefaultLookup;
			Select = BusTicketLookup.SelectAndOrderByName;
		}
	}

	public partial class CarRentalProviderLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public CarRentalProviderLookupController()
		{
			Query = db.CarRentalProviders;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class CarRentalLookupController : EntityLookupODataController<Domain, CarRental, string, CarRentalLookup>
	{
		public CarRentalLookupController()
		{
			Query = db.CarRentals;
			Lookup = CarRentalLookup.DefaultLookup;
			Select = CarRentalLookup.SelectAndOrderByName;
		}
	}

	public partial class CashInOrderPaymentLookupController : EntityLookupODataController<Domain, CashInOrderPayment, string, CashInOrderPaymentLookup>
	{
		public CashInOrderPaymentLookupController()
		{
			Query = db.CashInOrderPayments;
			Lookup = CashInOrderPaymentLookup.DefaultLookup;
			Select = CashInOrderPaymentLookup.SelectAndOrderByName;
		}
	}

	public partial class CashOutOrderPaymentLookupController : EntityLookupODataController<Domain, CashOutOrderPayment, string, CashOutOrderPaymentLookup>
	{
		public CashOutOrderPaymentLookupController()
		{
			Query = db.CashOutOrderPayments;
			Lookup = CashOutOrderPaymentLookup.DefaultLookup;
			Select = CashOutOrderPaymentLookup.SelectAndOrderByName;
		}
	}

	public partial class CateringTypeLookupController : EntityLookupODataController<Domain, CateringType, string, CateringTypeLookup>
	{
		public CateringTypeLookupController()
		{
			Query = db.CateringTypes;
			Lookup = Entity3.Lookup;
			Select = CateringTypeLookup.SelectAndOrderByName;
		}
	}

	public partial class CheckPaymentLookupController : EntityLookupODataController<Domain, CheckPayment, string, CheckPaymentLookup>
	{
		public CheckPaymentLookupController()
		{
			Query = db.CheckPayments;
			Lookup = CheckPaymentLookup.DefaultLookup;
			Select = CheckPaymentLookup.SelectAndOrderByName;
		}
	}

	public partial class ConsignmentLookupController : EntityLookupODataController<Domain, Consignment, string, ConsignmentLookup>
	{
		public ConsignmentLookupController()
		{
			Query = db.Consignments;
			Lookup = ConsignmentLookup.DefaultLookup;
			Select = ConsignmentLookup.SelectAndOrderByName;
		}
	}

	public partial class CountryLookupController : EntityLookupODataController<Domain, Country, string, CountryLookup>
	{
		public CountryLookupController()
		{
			Query = db.Countries;
			Lookup = Entity3.Lookup;
			Select = CountryLookup.SelectAndOrderByName;
		}
	}

	public partial class CustomerLookupController : EntityLookupODataController<Domain, Party, string, PartyLookup>
	{
		public CustomerLookupController()
		{
			Query = db.Customers;
			Lookup = Entity3.Lookup;
			Select = PartyLookup.SelectAndOrderByName;
		}
	}

	public partial class DepartmentLookupController : EntityLookupODataController<Domain, Department, string, DepartmentLookup>
	{
		public DepartmentLookupController()
		{
			Query = db.Departments;
			Lookup = Entity3.Lookup;
			Select = DepartmentLookup.SelectAndOrderByName;
		}
	}

	public partial class ElectronicPaymentLookupController : EntityLookupODataController<Domain, ElectronicPayment, string, ElectronicPaymentLookup>
	{
		public ElectronicPaymentLookupController()
		{
			Query = db.ElectronicPayments;
			Lookup = ElectronicPaymentLookup.DefaultLookup;
			Select = ElectronicPaymentLookup.SelectAndOrderByName;
		}
	}

	public partial class ExcursionLookupController : EntityLookupODataController<Domain, Excursion, string, ExcursionLookup>
	{
		public ExcursionLookupController()
		{
			Query = db.Excursions;
			Lookup = ExcursionLookup.DefaultLookup;
			Select = ExcursionLookup.SelectAndOrderByName;
		}
	}

	public partial class FlightSegmentLookupController : EntityLookupODataController<Domain, FlightSegment, string, FlightSegmentLookup>
	{
		public FlightSegmentLookupController()
		{
			Query = db.FlightSegments;
			Lookup = FlightSegmentLookup.DefaultLookup;
			Select = FlightSegmentLookup.SelectAndOrderByName;
		}
	}

	public partial class GdsAgentLookupController : EntityLookupODataController<Domain, GdsAgent, string, GdsAgentLookup>
	{
		public GdsAgentLookupController()
		{
			Query = db.GdsAgents;
			Lookup = GdsAgentLookup.DefaultLookup;
			Select = GdsAgentLookup.SelectAndOrderByName;
		}
	}

	public partial class GdsFileLookupController : EntityLookupODataController<Domain, GdsFile, string, GdsFileLookup>
	{
		public GdsFileLookupController()
		{
			Query = db.GdsFiles;
			Lookup = Entity3.Lookup;
			Select = GdsFileLookup.SelectAndOrderByName;
		}
	}

	public partial class GenericProductProviderLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public GenericProductProviderLookupController()
		{
			Query = db.GenericProductProviders;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class GenericProductLookupController : EntityLookupODataController<Domain, GenericProduct, string, GenericProductLookup>
	{
		public GenericProductLookupController()
		{
			Query = db.GenericProducts;
			Lookup = GenericProductLookup.DefaultLookup;
			Select = GenericProductLookup.SelectAndOrderByName;
		}
	}

	public partial class GenericProductTypeLookupController : EntityLookupODataController<Domain, GenericProductType, string, GenericProductTypeLookup>
	{
		public GenericProductTypeLookupController()
		{
			Query = db.GenericProductTypes;
			Lookup = Entity3.Lookup;
			Select = GenericProductTypeLookup.SelectAndOrderByName;
		}
	}

	public partial class IdentityLookupController : EntityLookupODataController<Domain, Identity, string, IdentityLookup>
	{
		public IdentityLookupController()
		{
			Query = db.Identities;
			Lookup = Entity3.Lookup;
			Select = IdentityLookup.SelectAndOrderByName;
		}
	}

	public partial class InsuranceCompanyLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public InsuranceCompanyLookupController()
		{
			Query = db.InsuranceCompanies;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class InsuranceDocumentLookupController : EntityLookupODataController<Domain, InsuranceDocument, string, InsuranceDocumentLookup>
	{
		public InsuranceDocumentLookupController()
		{
			Query = db.InsuranceDocuments;
			Lookup = InsuranceDocumentLookup.DefaultLookup;
			Select = InsuranceDocumentLookup.SelectAndOrderByName;
		}
	}

	public partial class InsuranceRefundLookupController : EntityLookupODataController<Domain, InsuranceRefund, string, InsuranceRefundLookup>
	{
		public InsuranceRefundLookupController()
		{
			Query = db.InsuranceRefunds;
			Lookup = InsuranceRefundLookup.DefaultLookup;
			Select = InsuranceRefundLookup.SelectAndOrderByName;
		}
	}

	public partial class InsuranceLookupController : EntityLookupODataController<Domain, Insurance, string, InsuranceLookup>
	{
		public InsuranceLookupController()
		{
			Query = db.Insurances;
			Lookup = InsuranceLookup.DefaultLookup;
			Select = InsuranceLookup.SelectAndOrderByName;
		}
	}

	public partial class InternalIdentityLookupController : EntityLookupODataController<Domain, InternalIdentity, string, InternalIdentityLookup>
	{
		public InternalIdentityLookupController()
		{
			Query = db.InternalIdentities;
			Lookup = Entity3.Lookup;
			Select = InternalIdentityLookup.SelectAndOrderByName;
		}
	}

	public partial class InternalTransferLookupController : EntityLookupODataController<Domain, InternalTransfer, string, InternalTransferLookup>
	{
		public InternalTransferLookupController()
		{
			Query = db.InternalTransfers;
			Lookup = InternalTransferLookup.DefaultLookup;
			Select = InternalTransferLookup.SelectAndOrderByName;
		}
	}

	public partial class InvoiceLookupController : EntityLookupODataController<Domain, Invoice, string, InvoiceLookup>
	{
		public InvoiceLookupController()
		{
			Query = db.Invoices;
			Lookup = InvoiceLookup.DefaultLookup;
			Select = InvoiceLookup.SelectAndOrderByName;
		}
	}

	public partial class IsicLookupController : EntityLookupODataController<Domain, Isic, string, IsicLookup>
	{
		public IsicLookupController()
		{
			Query = db.Isics;
			Lookup = IsicLookup.DefaultLookup;
			Select = IsicLookup.SelectAndOrderByName;
		}
	}

	public partial class IssuedConsignmentLookupController : EntityLookupODataController<Domain, IssuedConsignment, string, IssuedConsignmentLookup>
	{
		public IssuedConsignmentLookupController()
		{
			Query = db.IssuedConsignments;
			Lookup = IssuedConsignmentLookup.DefaultLookup;
			Select = IssuedConsignmentLookup.SelectAndOrderByName;
		}
	}

	public partial class MilesCardLookupController : EntityLookupODataController<Domain, MilesCard, string, MilesCardLookup>
	{
		public MilesCardLookupController()
		{
			Query = db.MilesCards;
			Lookup = MilesCardLookup.DefaultLookup;
			Select = MilesCardLookup.SelectAndOrderByName;
		}
	}

	public partial class OrderCheckLookupController : EntityLookupODataController<Domain, OrderCheck, string, OrderCheckLookup>
	{
		public OrderCheckLookupController()
		{
			Query = db.OrderChecks;
			Lookup = OrderCheckLookup.DefaultLookup;
			Select = OrderCheckLookup.SelectAndOrderByName;
		}
	}

	public partial class OrderItemLookupController : EntityLookupODataController<Domain, OrderItem, string, OrderItemLookup>
	{
		public OrderItemLookupController()
		{
			Query = db.OrderItems;
			Lookup = OrderItemLookup.DefaultLookup;
			Select = OrderItemLookup.SelectAndOrderByName;
		}
	}

	public partial class OrderLookupController : EntityLookupODataController<Domain, Order, string, OrderLookup>
	{
		public OrderLookupController()
		{
			Query = db.Orders;
			Lookup = OrderLookup.DefaultLookup;
			Select = OrderLookup.SelectAndOrderByName;
		}
	}

	public partial class OrganizationLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public OrganizationLookupController()
		{
			Query = db.Organizations;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class PartyLookupController : EntityLookupODataController<Domain, Party, string, PartyLookup>
	{
		public PartyLookupController()
		{
			Query = db.Parties;
			Lookup = Entity3.Lookup;
			Select = PartyLookup.SelectAndOrderByName;
		}
	}

	public partial class PassportLookupController : EntityLookupODataController<Domain, Passport, string, PassportLookup>
	{
		public PassportLookupController()
		{
			Query = db.Passports;
			Lookup = PassportLookup.DefaultLookup;
			Select = PassportLookup.SelectAndOrderByName;
		}
	}

	public partial class PasteboardProviderLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public PasteboardProviderLookupController()
		{
			Query = db.PasteboardProviders;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class PasteboardRefundLookupController : EntityLookupODataController<Domain, PasteboardRefund, string, PasteboardRefundLookup>
	{
		public PasteboardRefundLookupController()
		{
			Query = db.PasteboardRefunds;
			Lookup = PasteboardRefundLookup.DefaultLookup;
			Select = PasteboardRefundLookup.SelectAndOrderByName;
		}
	}

	public partial class PasteboardLookupController : EntityLookupODataController<Domain, Pasteboard, string, PasteboardLookup>
	{
		public PasteboardLookupController()
		{
			Query = db.Pasteboards;
			Lookup = PasteboardLookup.DefaultLookup;
			Select = PasteboardLookup.SelectAndOrderByName;
		}
	}

	public partial class PaymentLookupController : EntityLookupODataController<Domain, Payment, string, PaymentLookup>
	{
		public PaymentLookupController()
		{
			Query = db.Payments;
			Lookup = PaymentLookup.DefaultLookup;
			Select = PaymentLookup.SelectAndOrderByName;
		}
	}

	public partial class PaymentSystemLookupController : EntityLookupODataController<Domain, PaymentSystem, string, PaymentSystemLookup>
	{
		public PaymentSystemLookupController()
		{
			Query = db.PaymentSystems;
			Lookup = Entity3.Lookup;
			Select = PaymentSystemLookup.SelectAndOrderByName;
		}
	}

	public partial class PersonLookupController : EntityLookupODataController<Domain, Person, string, PersonLookup>
	{
		public PersonLookupController()
		{
			Query = db.Persons;
			Lookup = Entity3.Lookup;
			Select = PersonLookup.SelectAndOrderByName;
		}
	}

	public partial class ProductLookupController : EntityLookupODataController<Domain, Product, string, ProductLookup>
	{
		public ProductLookupController()
		{
			Query = db.Products;
			Lookup = ProductLookup.DefaultLookup;
			Select = ProductLookup.SelectAndOrderByName;
		}
	}

	public partial class RailwayDocumentLookupController : EntityLookupODataController<Domain, RailwayDocument, string, RailwayDocumentLookup>
	{
		public RailwayDocumentLookupController()
		{
			Query = db.RailwayDocuments;
			Lookup = RailwayDocumentLookup.DefaultLookup;
			Select = RailwayDocumentLookup.SelectAndOrderByName;
		}
	}

	public partial class ReceiptLookupController : EntityLookupODataController<Domain, Invoice, string, InvoiceLookup>
	{
		public ReceiptLookupController()
		{
			Query = db.Receipts;
			Lookup = InvoiceLookup.DefaultLookup;
			Select = InvoiceLookup.SelectAndOrderByName;
		}
	}

	public partial class RoamingOperatorLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public RoamingOperatorLookupController()
		{
			Query = db.RoamingOperators;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class SequenceLookupController : EntityLookupODataController<Domain, Sequence, string, SequenceLookup>
	{
		public SequenceLookupController()
		{
			Query = db.Sequences;
			Lookup = SequenceLookup.DefaultLookup;
			Select = SequenceLookup.SelectAndOrderByName;
		}
	}

	public partial class SimCardLookupController : EntityLookupODataController<Domain, SimCard, string, SimCardLookup>
	{
		public SimCardLookupController()
		{
			Query = db.SimCards;
			Lookup = SimCardLookup.DefaultLookup;
			Select = SimCardLookup.SelectAndOrderByName;
		}
	}

	public partial class TourProviderLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public TourProviderLookupController()
		{
			Query = db.TourProviders;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class TourLookupController : EntityLookupODataController<Domain, Tour, string, TourLookup>
	{
		public TourLookupController()
		{
			Query = db.Tours;
			Lookup = TourLookup.DefaultLookup;
			Select = TourLookup.SelectAndOrderByName;
		}
	}

	public partial class TransferProviderLookupController : EntityLookupODataController<Domain, Organization, string, OrganizationLookup>
	{
		public TransferProviderLookupController()
		{
			Query = db.TransferProviders;
			Lookup = Entity3.Lookup;
			Select = OrganizationLookup.SelectAndOrderByName;
		}
	}

	public partial class TransferLookupController : EntityLookupODataController<Domain, Transfer, string, TransferLookup>
	{
		public TransferLookupController()
		{
			Query = db.Transfers;
			Lookup = TransferLookup.DefaultLookup;
			Select = TransferLookup.SelectAndOrderByName;
		}
	}

	public partial class UserLookupController : EntityLookupODataController<Domain, User, string, UserLookup>
	{
		public UserLookupController()
		{
			Query = db.Users;
			Lookup = Entity3.Lookup;
			Select = UserLookup.SelectAndOrderByName;
		}
	}

	public partial class WireTransferLookupController : EntityLookupODataController<Domain, WireTransfer, string, WireTransferLookup>
	{
		public WireTransferLookupController()
		{
			Query = db.WireTransfers;
			Lookup = WireTransferLookup.DefaultLookup;
			Select = WireTransferLookup.SelectAndOrderByName;
		}
	}

	#endregion


	#region QueryResult Controllers

	public partial class EverydayProfitReportsController: DbQueryODataController<Domain, EverydayProfitReportParams, EverydayProfitReport, EverydayProfitReportQuery> { }

	public partial class FlownReportsController: DbQueryODataController<Domain, FlownReportParams, FlownReport, FlownReportQuery> { }

	public partial class ProductSummariesController: DbQueryODataController<Domain, ProductSummaryParams, ProductSummary, ProductSummaryQuery> { }

	public partial class ProductTotalByBookersController: DbQueryODataController<Domain, ProductTotalByBookerParams, ProductTotalByBooker, ProductTotalByBookerQuery> { }

	public partial class ProductTotalByDaysController: DbQueryODataController<Domain, ProductTotalByDayParams, ProductTotalByDay, ProductTotalByDayQuery> { }

	public partial class ProductTotalByMonthsController: DbQueryODataController<Domain, ProductTotalByMonthParams, ProductTotalByMonth, ProductTotalByMonthQuery> { }

	public partial class ProductTotalByOwnersController: DbQueryODataController<Domain, ProductTotalByOwnerParams, ProductTotalByOwner, ProductTotalByOwnerQuery> { }

	public partial class ProductTotalByProvidersController: DbQueryODataController<Domain, ProductTotalByProviderParams, ProductTotalByProvider, ProductTotalByProviderQuery> { }

	public partial class ProductTotalByQuartersController: DbQueryODataController<Domain, ProductTotalByQuarterParams, ProductTotalByQuarter, ProductTotalByQuarterQuery> { }

	public partial class ProductTotalBySellersController: DbQueryODataController<Domain, ProductTotalBySellerParams, ProductTotalBySeller, ProductTotalBySellerQuery> { }

	public partial class ProductTotalByTypesController: DbQueryODataController<Domain, ProductTotalByTypeParams, ProductTotalByType, ProductTotalByTypeQuery> { }

	public partial class ProductTotalByYearsController: DbQueryODataController<Domain, ProductTotalByYearParams, ProductTotalByYear, ProductTotalByYearQuery> { }

	public partial class ProfitDistributionByCustomersController: DbQueryODataController<Domain, ProfitDistributionByCustomerParams, ProfitDistributionByCustomer, ProfitDistributionByCustomerQuery> { }

	public partial class ProfitDistributionByProvidersController: DbQueryODataController<Domain, ProfitDistributionByProviderParams, ProfitDistributionByProvider, ProfitDistributionByProviderQuery> { }

	#endregion


	#region DomainAction Controllers

	public partial class GdsAgent_ApplyToUnassignedController: DomainActionODataController<Domain, GdsAgent_ApplyToUnassigned> { }

	#endregion


	partial class AppConfig
	{

		public class DomainEdmModel
		{

			#region Properties

			public EntitySetConfiguration<AccommodationProvider> AccommodationProviders;
			public EntityTypeConfiguration<AccommodationProvider> AccommodationProvider;

			public EntitySetConfiguration<Accommodation> Accommodations;
			public EntityTypeConfiguration<Accommodation> Accommodation;

			public EntitySetConfiguration<AccommodationType> AccommodationTypes;
			public EntityTypeConfiguration<AccommodationType> AccommodationType;

			public EntitySetConfiguration<ActiveOwner> ActiveOwners;
			public EntityTypeConfiguration<ActiveOwner> ActiveOwner;

			public EntitySetConfiguration<Agent> Agents;
			public EntityTypeConfiguration<Agent> Agent;

			public EntitySetConfiguration<Airline> Airlines;
			public EntityTypeConfiguration<Airline> Airline;

			public EntitySetConfiguration<AirlineServiceClass> AirlineServiceClasses;
			public EntityTypeConfiguration<AirlineServiceClass> AirlineServiceClass;

			public EntitySetConfiguration<Airport> Airports;
			public EntityTypeConfiguration<Airport> Airport;

			public EntitySetConfiguration<AviaDocument> AviaDocuments;
			public EntityTypeConfiguration<AviaDocument> AviaDocument;

			public EntitySetConfiguration<AviaMco> AviaMcos;
			public EntityTypeConfiguration<AviaMco> AviaMco;

			public EntitySetConfiguration<AviaRefund> AviaRefunds;
			public EntityTypeConfiguration<AviaRefund> AviaRefund;

			public EntitySetConfiguration<AviaTicket> AviaTickets;
			public EntityTypeConfiguration<AviaTicket> AviaTicket;

			public EntitySetConfiguration<BankAccount> BankAccounts;
			public EntityTypeConfiguration<BankAccount> BankAccount;

			public EntitySetConfiguration<BusDocument> BusDocuments;
			public EntityTypeConfiguration<BusDocument> BusDocument;

			public EntitySetConfiguration<BusTicketProvider> BusTicketProviders;
			public EntityTypeConfiguration<BusTicketProvider> BusTicketProvider;

			public EntitySetConfiguration<BusTicketRefund> BusTicketRefunds;
			public EntityTypeConfiguration<BusTicketRefund> BusTicketRefund;

			public EntitySetConfiguration<BusTicket> BusTickets;
			public EntityTypeConfiguration<BusTicket> BusTicket;

			public EntitySetConfiguration<CarRentalProvider> CarRentalProviders;
			public EntityTypeConfiguration<CarRentalProvider> CarRentalProvider;

			public EntitySetConfiguration<CarRental> CarRentals;
			public EntityTypeConfiguration<CarRental> CarRental;

			public EntitySetConfiguration<CashInOrderPayment> CashInOrderPayments;
			public EntityTypeConfiguration<CashInOrderPayment> CashInOrderPayment;

			public EntitySetConfiguration<CashOutOrderPayment> CashOutOrderPayments;
			public EntityTypeConfiguration<CashOutOrderPayment> CashOutOrderPayment;

			public EntitySetConfiguration<CateringType> CateringTypes;
			public EntityTypeConfiguration<CateringType> CateringType;

			public EntitySetConfiguration<CheckPayment> CheckPayments;
			public EntityTypeConfiguration<CheckPayment> CheckPayment;

			public EntitySetConfiguration<Consignment> Consignments;
			public EntityTypeConfiguration<Consignment> Consignment;

			public EntitySetConfiguration<Country> Countries;
			public EntityTypeConfiguration<Country> Country;

			public EntitySetConfiguration<CurrencyDailyRate> CurrencyDailyRates;
			public EntityTypeConfiguration<CurrencyDailyRate> CurrencyDailyRate;

			public EntitySetConfiguration<Customer> Customers;
			public EntityTypeConfiguration<Customer> Customer;

			public EntitySetConfiguration<Department> Departments;
			public EntityTypeConfiguration<Department> Department;

			public EntitySetConfiguration<DocumentAccess> DocumentAccesses;
			public EntityTypeConfiguration<DocumentAccess> DocumentAccess;

			public EntitySetConfiguration<DocumentOwner> DocumentOwners;
			public EntityTypeConfiguration<DocumentOwner> DocumentOwner;

			public EntitySetConfiguration<ElectronicPayment> ElectronicPayments;
			public EntityTypeConfiguration<ElectronicPayment> ElectronicPayment;

			public EntitySetConfiguration<EverydayProfitReport> EverydayProfitReports;
			public EntityTypeConfiguration<EverydayProfitReport> EverydayProfitReport;

			public EntitySetConfiguration<Excursion> Excursions;
			public EntityTypeConfiguration<Excursion> Excursion;

			public EntitySetConfiguration<File> Files;
			public EntityTypeConfiguration<File> File;

			public EntitySetConfiguration<FlightSegment> FlightSegments;
			public EntityTypeConfiguration<FlightSegment> FlightSegment;

			public EntitySetConfiguration<FlownReport> FlownReports;
			public EntityTypeConfiguration<FlownReport> FlownReport;

			public EntitySetConfiguration<GdsAgent> GdsAgents;
			public EntityTypeConfiguration<GdsAgent> GdsAgent;

			public EntitySetConfiguration<GdsFile> GdsFiles;
			public EntityTypeConfiguration<GdsFile> GdsFile;

			public EntitySetConfiguration<GenericProductProvider> GenericProductProviders;
			public EntityTypeConfiguration<GenericProductProvider> GenericProductProvider;

			public EntitySetConfiguration<GenericProduct> GenericProducts;
			public EntityTypeConfiguration<GenericProduct> GenericProduct;

			public EntitySetConfiguration<GenericProductType> GenericProductTypes;
			public EntityTypeConfiguration<GenericProductType> GenericProductType;

			public EntitySetConfiguration<Identity> Identities;
			public EntityTypeConfiguration<Identity> Identity;

			public EntitySetConfiguration<InsuranceCompany> InsuranceCompanies;
			public EntityTypeConfiguration<InsuranceCompany> InsuranceCompany;

			public EntitySetConfiguration<InsuranceDocument> InsuranceDocuments;
			public EntityTypeConfiguration<InsuranceDocument> InsuranceDocument;

			public EntitySetConfiguration<InsuranceRefund> InsuranceRefunds;
			public EntityTypeConfiguration<InsuranceRefund> InsuranceRefund;

			public EntitySetConfiguration<Insurance> Insurances;
			public EntityTypeConfiguration<Insurance> Insurance;

			public EntitySetConfiguration<InternalIdentity> InternalIdentities;
			public EntityTypeConfiguration<InternalIdentity> InternalIdentity;

			public EntitySetConfiguration<InternalTransfer> InternalTransfers;
			public EntityTypeConfiguration<InternalTransfer> InternalTransfer;

			public EntitySetConfiguration<Invoice> Invoices;
			public EntityTypeConfiguration<Invoice> Invoice;

			public EntitySetConfiguration<Isic> Isics;
			public EntityTypeConfiguration<Isic> Isic;

			public EntitySetConfiguration<IssuedConsignment> IssuedConsignments;
			public EntityTypeConfiguration<IssuedConsignment> IssuedConsignment;

			public EntitySetConfiguration<MilesCard> MilesCards;
			public EntityTypeConfiguration<MilesCard> MilesCard;

			public EntitySetConfiguration<OrderCheck> OrderChecks;
			public EntityTypeConfiguration<OrderCheck> OrderCheck;

			public EntitySetConfiguration<OrderItem> OrderItems;
			public EntityTypeConfiguration<OrderItem> OrderItem;

			public EntitySetConfiguration<Order> Orders;
			public EntityTypeConfiguration<Order> Order;

			public EntitySetConfiguration<Organization> Organizations;
			public EntityTypeConfiguration<Organization> Organization;

			public EntitySetConfiguration<Party> Parties;
			public EntityTypeConfiguration<Party> Party;

			public EntitySetConfiguration<Passport> Passports;
			public EntityTypeConfiguration<Passport> Passport;

			public EntitySetConfiguration<PasteboardProvider> PasteboardProviders;
			public EntityTypeConfiguration<PasteboardProvider> PasteboardProvider;

			public EntitySetConfiguration<PasteboardRefund> PasteboardRefunds;
			public EntityTypeConfiguration<PasteboardRefund> PasteboardRefund;

			public EntitySetConfiguration<Pasteboard> Pasteboards;
			public EntityTypeConfiguration<Pasteboard> Pasteboard;

			public EntitySetConfiguration<Payment> Payments;
			public EntityTypeConfiguration<Payment> Payment;

			public EntitySetConfiguration<PaymentSystem> PaymentSystems;
			public EntityTypeConfiguration<PaymentSystem> PaymentSystem;

			public EntitySetConfiguration<Person> Persons;
			public EntityTypeConfiguration<Person> Person;

			public EntitySetConfiguration<ProductPassenger> ProductPassengers;
			public EntityTypeConfiguration<ProductPassenger> ProductPassenger;

			public EntitySetConfiguration<Product> Products;
			public EntityTypeConfiguration<Product> Product;

			public EntitySetConfiguration<ProductSummary> ProductSummaries;
			public EntityTypeConfiguration<ProductSummary> ProductSummary;

			public EntitySetConfiguration<ProductTotalByBooker> ProductTotalByBookers;
			public EntityTypeConfiguration<ProductTotalByBooker> ProductTotalByBooker;

			public EntitySetConfiguration<ProductTotalByDay> ProductTotalByDays;
			public EntityTypeConfiguration<ProductTotalByDay> ProductTotalByDay;

			public EntitySetConfiguration<ProductTotalByMonth> ProductTotalByMonths;
			public EntityTypeConfiguration<ProductTotalByMonth> ProductTotalByMonth;

			public EntitySetConfiguration<ProductTotalByOwner> ProductTotalByOwners;
			public EntityTypeConfiguration<ProductTotalByOwner> ProductTotalByOwner;

			public EntitySetConfiguration<ProductTotalByProvider> ProductTotalByProviders;
			public EntityTypeConfiguration<ProductTotalByProvider> ProductTotalByProvider;

			public EntitySetConfiguration<ProductTotalByQuarter> ProductTotalByQuarters;
			public EntityTypeConfiguration<ProductTotalByQuarter> ProductTotalByQuarter;

			public EntitySetConfiguration<ProductTotalBySeller> ProductTotalBySellers;
			public EntityTypeConfiguration<ProductTotalBySeller> ProductTotalBySeller;

			public EntitySetConfiguration<ProductTotalByType> ProductTotalByTypes;
			public EntityTypeConfiguration<ProductTotalByType> ProductTotalByType;

			public EntitySetConfiguration<ProductTotalByYear> ProductTotalByYears;
			public EntityTypeConfiguration<ProductTotalByYear> ProductTotalByYear;

			public EntitySetConfiguration<ProfitDistributionByCustomer> ProfitDistributionByCustomers;
			public EntityTypeConfiguration<ProfitDistributionByCustomer> ProfitDistributionByCustomer;

			public EntitySetConfiguration<ProfitDistributionByProvider> ProfitDistributionByProviders;
			public EntityTypeConfiguration<ProfitDistributionByProvider> ProfitDistributionByProvider;

			public EntitySetConfiguration<RailwayDocument> RailwayDocuments;
			public EntityTypeConfiguration<RailwayDocument> RailwayDocument;

			public EntitySetConfiguration<Receipt> Receipts;
			public EntityTypeConfiguration<Receipt> Receipt;

			public EntitySetConfiguration<RoamingOperator> RoamingOperators;
			public EntityTypeConfiguration<RoamingOperator> RoamingOperator;

			public EntitySetConfiguration<Sequence> Sequences;
			public EntityTypeConfiguration<Sequence> Sequence;

			public EntitySetConfiguration<SimCard> SimCards;
			public EntityTypeConfiguration<SimCard> SimCard;

			public EntitySetConfiguration<SystemConfiguration> SystemConfigurations;
			public EntityTypeConfiguration<SystemConfiguration> SystemConfiguration;

			public EntitySetConfiguration<TourProvider> TourProviders;
			public EntityTypeConfiguration<TourProvider> TourProvider;

			public EntitySetConfiguration<Tour> Tours;
			public EntityTypeConfiguration<Tour> Tour;

			public EntitySetConfiguration<TransferProvider> TransferProviders;
			public EntityTypeConfiguration<TransferProvider> TransferProvider;

			public EntitySetConfiguration<Transfer> Transfers;
			public EntityTypeConfiguration<Transfer> Transfer;

			public EntitySetConfiguration<User> Users;
			public EntityTypeConfiguration<User> User;

			public EntitySetConfiguration<WireTransfer> WireTransfers;
			public EntityTypeConfiguration<WireTransfer> WireTransfer;

			public EntitySetConfiguration<ProductFilter> ProductFilter;
			public EntitySetConfiguration<EverydayProfitReportParams> EverydayProfitReportParams;
			public EntitySetConfiguration<FlownReportParams> FlownReportParams;
			public EntitySetConfiguration<ProductSummaryParams> ProductSummaryParams;
			public EntitySetConfiguration<ProductTotalByBookerParams> ProductTotalByBookerParams;
			public EntitySetConfiguration<ProductTotalByDayParams> ProductTotalByDayParams;
			public EntitySetConfiguration<ProductTotalByMonthParams> ProductTotalByMonthParams;
			public EntitySetConfiguration<ProductTotalByOwnerParams> ProductTotalByOwnerParams;
			public EntitySetConfiguration<ProductTotalByProviderParams> ProductTotalByProviderParams;
			public EntitySetConfiguration<ProductTotalByQuarterParams> ProductTotalByQuarterParams;
			public EntitySetConfiguration<ProductTotalBySellerParams> ProductTotalBySellerParams;
			public EntitySetConfiguration<ProductTotalByTypeParams> ProductTotalByTypeParams;
			public EntitySetConfiguration<ProductTotalByYearParams> ProductTotalByYearParams;
			public EntitySetConfiguration<ProfitDistributionByCustomerParams> ProfitDistributionByCustomerParams;
			public EntitySetConfiguration<ProfitDistributionByProviderParams> ProfitDistributionByProviderParams;
			public EntitySetConfiguration<GdsAgent_ApplyToUnassigned> GdsAgent_ApplyToUnassigned;

			#endregion


			public DomainEdmModel(ODataConventionModelBuilder mb)
			{

				#region Register References

				mb.ComplexType<AccommodationReference>();
				mb.ComplexType<AccommodationTypeReference>();
				mb.ComplexType<AirlineServiceClassReference>();
				mb.ComplexType<AirportReference>();
				mb.ComplexType<AviaDocumentReference>();
				mb.ComplexType<AviaMcoReference>();
				mb.ComplexType<AviaRefundReference>();
				mb.ComplexType<AviaTicketReference>();
				mb.ComplexType<BankAccountReference>();
				mb.ComplexType<BusDocumentReference>();
				mb.ComplexType<BusTicketRefundReference>();
				mb.ComplexType<BusTicketReference>();
				mb.ComplexType<CarRentalReference>();
				mb.ComplexType<CashInOrderPaymentReference>();
				mb.ComplexType<CashOutOrderPaymentReference>();
				mb.ComplexType<CateringTypeReference>();
				mb.ComplexType<CheckPaymentReference>();
				mb.ComplexType<ConsignmentReference>();
				mb.ComplexType<CountryReference>();
				mb.ComplexType<DepartmentReference>();
				mb.ComplexType<ElectronicPaymentReference>();
				mb.ComplexType<ExcursionReference>();
				mb.ComplexType<FlightSegmentReference>();
				mb.ComplexType<GdsAgentReference>();
				mb.ComplexType<GdsFileReference>();
				mb.ComplexType<GenericProductReference>();
				mb.ComplexType<GenericProductTypeReference>();
				mb.ComplexType<IdentityReference>();
				mb.ComplexType<InsuranceDocumentReference>();
				mb.ComplexType<InsuranceRefundReference>();
				mb.ComplexType<InsuranceReference>();
				mb.ComplexType<InternalIdentityReference>();
				mb.ComplexType<InternalTransferReference>();
				mb.ComplexType<InvoiceReference>();
				mb.ComplexType<IsicReference>();
				mb.ComplexType<IssuedConsignmentReference>();
				mb.ComplexType<MilesCardReference>();
				mb.ComplexType<OrderCheckReference>();
				mb.ComplexType<OrderItemReference>();
				mb.ComplexType<OrderReference>();
				mb.ComplexType<OrganizationReference>();
				mb.ComplexType<PartyReference>();
				mb.ComplexType<PassportReference>();
				mb.ComplexType<PasteboardRefundReference>();
				mb.ComplexType<PasteboardReference>();
				mb.ComplexType<PaymentReference>();
				mb.ComplexType<PaymentSystemReference>();
				mb.ComplexType<PersonReference>();
				mb.ComplexType<ProductReference>();
				mb.ComplexType<RailwayDocumentReference>();
				mb.ComplexType<SequenceReference>();
				mb.ComplexType<SimCardReference>();
				mb.ComplexType<TourReference>();
				mb.ComplexType<TransferReference>();
				mb.ComplexType<UserReference>();
				mb.ComplexType<WireTransferReference>();

				#endregion


				#region Register EntitySets

				AccommodationProviders = mb.EntitySet<AccommodationProvider>("AccommodationProviders");
				AccommodationProvider = AccommodationProviders.EntityType;
				mb.EntitySet<OrganizationLookup>("AccommodationProviderLookup");

				Accommodations = mb.EntitySet<Accommodation>("Accommodations");
				Accommodation = Accommodations.EntityType;
				mb.EntitySet<AccommodationLookup>("AccommodationLookup");

				AccommodationTypes = mb.EntitySet<AccommodationType>("AccommodationTypes");
				AccommodationType = AccommodationTypes.EntityType;
				mb.EntitySet<AccommodationTypeLookup>("AccommodationTypeLookup");

				ActiveOwners = mb.EntitySet<ActiveOwner>("ActiveOwners");
				ActiveOwner = ActiveOwners.EntityType;
				mb.EntitySet<PartyLookup>("ActiveOwnerLookup");

				Agents = mb.EntitySet<Agent>("Agents");
				Agent = Agents.EntityType;
				mb.EntitySet<PersonLookup>("AgentLookup");

				Airlines = mb.EntitySet<Airline>("Airlines");
				Airline = Airlines.EntityType;
				mb.EntitySet<AirlineLookup>("AirlineLookup");

				AirlineServiceClasses = mb.EntitySet<AirlineServiceClass>("AirlineServiceClasses");
				AirlineServiceClass = AirlineServiceClasses.EntityType;
				mb.EntitySet<AirlineServiceClassLookup>("AirlineServiceClassLookup");

				Airports = mb.EntitySet<Airport>("Airports");
				Airport = Airports.EntityType;
				mb.EntitySet<AirportLookup>("AirportLookup");

				AviaDocuments = mb.EntitySet<AviaDocument>("AviaDocuments");
				AviaDocument = AviaDocuments.EntityType;
				mb.EntitySet<AviaDocumentLookup>("AviaDocumentLookup");

				AviaMcos = mb.EntitySet<AviaMco>("AviaMcos");
				AviaMco = AviaMcos.EntityType;
				mb.EntitySet<AviaMcoLookup>("AviaMcoLookup");

				AviaRefunds = mb.EntitySet<AviaRefund>("AviaRefunds");
				AviaRefund = AviaRefunds.EntityType;
				mb.EntitySet<AviaRefundLookup>("AviaRefundLookup");

				AviaTickets = mb.EntitySet<AviaTicket>("AviaTickets");
				AviaTicket = AviaTickets.EntityType;
				mb.EntitySet<AviaTicketLookup>("AviaTicketLookup");

				BankAccounts = mb.EntitySet<BankAccount>("BankAccounts");
				BankAccount = BankAccounts.EntityType;
				mb.EntitySet<BankAccountLookup>("BankAccountLookup");

				BusDocuments = mb.EntitySet<BusDocument>("BusDocuments");
				BusDocument = BusDocuments.EntityType;
				mb.EntitySet<BusDocumentLookup>("BusDocumentLookup");

				BusTicketProviders = mb.EntitySet<BusTicketProvider>("BusTicketProviders");
				BusTicketProvider = BusTicketProviders.EntityType;
				mb.EntitySet<OrganizationLookup>("BusTicketProviderLookup");

				BusTicketRefunds = mb.EntitySet<BusTicketRefund>("BusTicketRefunds");
				BusTicketRefund = BusTicketRefunds.EntityType;
				mb.EntitySet<BusTicketRefundLookup>("BusTicketRefundLookup");

				BusTickets = mb.EntitySet<BusTicket>("BusTickets");
				BusTicket = BusTickets.EntityType;
				mb.EntitySet<BusTicketLookup>("BusTicketLookup");

				CarRentalProviders = mb.EntitySet<CarRentalProvider>("CarRentalProviders");
				CarRentalProvider = CarRentalProviders.EntityType;
				mb.EntitySet<OrganizationLookup>("CarRentalProviderLookup");

				CarRentals = mb.EntitySet<CarRental>("CarRentals");
				CarRental = CarRentals.EntityType;
				mb.EntitySet<CarRentalLookup>("CarRentalLookup");

				CashInOrderPayments = mb.EntitySet<CashInOrderPayment>("CashInOrderPayments");
				CashInOrderPayment = CashInOrderPayments.EntityType;
				mb.EntitySet<CashInOrderPaymentLookup>("CashInOrderPaymentLookup");

				CashOutOrderPayments = mb.EntitySet<CashOutOrderPayment>("CashOutOrderPayments");
				CashOutOrderPayment = CashOutOrderPayments.EntityType;
				mb.EntitySet<CashOutOrderPaymentLookup>("CashOutOrderPaymentLookup");

				CateringTypes = mb.EntitySet<CateringType>("CateringTypes");
				CateringType = CateringTypes.EntityType;
				mb.EntitySet<CateringTypeLookup>("CateringTypeLookup");

				CheckPayments = mb.EntitySet<CheckPayment>("CheckPayments");
				CheckPayment = CheckPayments.EntityType;
				mb.EntitySet<CheckPaymentLookup>("CheckPaymentLookup");

				Consignments = mb.EntitySet<Consignment>("Consignments");
				Consignment = Consignments.EntityType;
				mb.EntitySet<ConsignmentLookup>("ConsignmentLookup");

				Countries = mb.EntitySet<Country>("Countries");
				Country = Countries.EntityType;
				mb.EntitySet<CountryLookup>("CountryLookup");

				CurrencyDailyRates = mb.EntitySet<CurrencyDailyRate>("CurrencyDailyRates");
				CurrencyDailyRate = CurrencyDailyRates.EntityType;

				Customers = mb.EntitySet<Customer>("Customers");
				Customer = Customers.EntityType;
				mb.EntitySet<PartyLookup>("CustomerLookup");

				Departments = mb.EntitySet<Department>("Departments");
				Department = Departments.EntityType;
				mb.EntitySet<DepartmentLookup>("DepartmentLookup");

				DocumentAccesses = mb.EntitySet<DocumentAccess>("DocumentAccesses");
				DocumentAccess = DocumentAccesses.EntityType;

				DocumentOwners = mb.EntitySet<DocumentOwner>("DocumentOwners");
				DocumentOwner = DocumentOwners.EntityType;

				ElectronicPayments = mb.EntitySet<ElectronicPayment>("ElectronicPayments");
				ElectronicPayment = ElectronicPayments.EntityType;
				mb.EntitySet<ElectronicPaymentLookup>("ElectronicPaymentLookup");

				EverydayProfitReports = mb.EntitySet<EverydayProfitReport>("EverydayProfitReports");
				EverydayProfitReport = EverydayProfitReports.EntityType;

				Excursions = mb.EntitySet<Excursion>("Excursions");
				Excursion = Excursions.EntityType;
				mb.EntitySet<ExcursionLookup>("ExcursionLookup");

				Files = mb.EntitySet<File>("Files");
				File = Files.EntityType;

				FlightSegments = mb.EntitySet<FlightSegment>("FlightSegments");
				FlightSegment = FlightSegments.EntityType;
				mb.EntitySet<FlightSegmentLookup>("FlightSegmentLookup");

				FlownReports = mb.EntitySet<FlownReport>("FlownReports");
				FlownReport = FlownReports.EntityType;

				GdsAgents = mb.EntitySet<GdsAgent>("GdsAgents");
				GdsAgent = GdsAgents.EntityType;
				mb.EntitySet<GdsAgentLookup>("GdsAgentLookup");

				GdsFiles = mb.EntitySet<GdsFile>("GdsFiles");
				GdsFile = GdsFiles.EntityType;
				mb.EntitySet<GdsFileLookup>("GdsFileLookup");

				GenericProductProviders = mb.EntitySet<GenericProductProvider>("GenericProductProviders");
				GenericProductProvider = GenericProductProviders.EntityType;
				mb.EntitySet<OrganizationLookup>("GenericProductProviderLookup");

				GenericProducts = mb.EntitySet<GenericProduct>("GenericProducts");
				GenericProduct = GenericProducts.EntityType;
				mb.EntitySet<GenericProductLookup>("GenericProductLookup");

				GenericProductTypes = mb.EntitySet<GenericProductType>("GenericProductTypes");
				GenericProductType = GenericProductTypes.EntityType;
				mb.EntitySet<GenericProductTypeLookup>("GenericProductTypeLookup");

				Identities = mb.EntitySet<Identity>("Identities");
				Identity = Identities.EntityType;
				mb.EntitySet<IdentityLookup>("IdentityLookup");

				InsuranceCompanies = mb.EntitySet<InsuranceCompany>("InsuranceCompanies");
				InsuranceCompany = InsuranceCompanies.EntityType;
				mb.EntitySet<OrganizationLookup>("InsuranceCompanyLookup");

				InsuranceDocuments = mb.EntitySet<InsuranceDocument>("InsuranceDocuments");
				InsuranceDocument = InsuranceDocuments.EntityType;
				mb.EntitySet<InsuranceDocumentLookup>("InsuranceDocumentLookup");

				InsuranceRefunds = mb.EntitySet<InsuranceRefund>("InsuranceRefunds");
				InsuranceRefund = InsuranceRefunds.EntityType;
				mb.EntitySet<InsuranceRefundLookup>("InsuranceRefundLookup");

				Insurances = mb.EntitySet<Insurance>("Insurances");
				Insurance = Insurances.EntityType;
				mb.EntitySet<InsuranceLookup>("InsuranceLookup");

				InternalIdentities = mb.EntitySet<InternalIdentity>("InternalIdentities");
				InternalIdentity = InternalIdentities.EntityType;
				mb.EntitySet<InternalIdentityLookup>("InternalIdentityLookup");

				InternalTransfers = mb.EntitySet<InternalTransfer>("InternalTransfers");
				InternalTransfer = InternalTransfers.EntityType;
				mb.EntitySet<InternalTransferLookup>("InternalTransferLookup");

				Invoices = mb.EntitySet<Invoice>("Invoices");
				Invoice = Invoices.EntityType;
				mb.EntitySet<InvoiceLookup>("InvoiceLookup");

				Isics = mb.EntitySet<Isic>("Isics");
				Isic = Isics.EntityType;
				mb.EntitySet<IsicLookup>("IsicLookup");

				IssuedConsignments = mb.EntitySet<IssuedConsignment>("IssuedConsignments");
				IssuedConsignment = IssuedConsignments.EntityType;
				mb.EntitySet<IssuedConsignmentLookup>("IssuedConsignmentLookup");

				MilesCards = mb.EntitySet<MilesCard>("MilesCards");
				MilesCard = MilesCards.EntityType;
				mb.EntitySet<MilesCardLookup>("MilesCardLookup");

				OrderChecks = mb.EntitySet<OrderCheck>("OrderChecks");
				OrderCheck = OrderChecks.EntityType;
				mb.EntitySet<OrderCheckLookup>("OrderCheckLookup");

				OrderItems = mb.EntitySet<OrderItem>("OrderItems");
				OrderItem = OrderItems.EntityType;
				mb.EntitySet<OrderItemLookup>("OrderItemLookup");

				Orders = mb.EntitySet<Order>("Orders");
				Order = Orders.EntityType;
				mb.EntitySet<OrderLookup>("OrderLookup");

				Organizations = mb.EntitySet<Organization>("Organizations");
				Organization = Organizations.EntityType;
				mb.EntitySet<OrganizationLookup>("OrganizationLookup");

				Parties = mb.EntitySet<Party>("Parties");
				Party = Parties.EntityType;
				mb.EntitySet<PartyLookup>("PartyLookup");

				Passports = mb.EntitySet<Passport>("Passports");
				Passport = Passports.EntityType;
				mb.EntitySet<PassportLookup>("PassportLookup");

				PasteboardProviders = mb.EntitySet<PasteboardProvider>("PasteboardProviders");
				PasteboardProvider = PasteboardProviders.EntityType;
				mb.EntitySet<OrganizationLookup>("PasteboardProviderLookup");

				PasteboardRefunds = mb.EntitySet<PasteboardRefund>("PasteboardRefunds");
				PasteboardRefund = PasteboardRefunds.EntityType;
				mb.EntitySet<PasteboardRefundLookup>("PasteboardRefundLookup");

				Pasteboards = mb.EntitySet<Pasteboard>("Pasteboards");
				Pasteboard = Pasteboards.EntityType;
				mb.EntitySet<PasteboardLookup>("PasteboardLookup");

				Payments = mb.EntitySet<Payment>("Payments");
				Payment = Payments.EntityType;
				mb.EntitySet<PaymentLookup>("PaymentLookup");

				PaymentSystems = mb.EntitySet<PaymentSystem>("PaymentSystems");
				PaymentSystem = PaymentSystems.EntityType;
				mb.EntitySet<PaymentSystemLookup>("PaymentSystemLookup");

				Persons = mb.EntitySet<Person>("Persons");
				Person = Persons.EntityType;
				mb.EntitySet<PersonLookup>("PersonLookup");

				ProductPassengers = mb.EntitySet<ProductPassenger>("ProductPassengers");
				ProductPassenger = ProductPassengers.EntityType;

				Products = mb.EntitySet<Product>("Products");
				Product = Products.EntityType;
				mb.EntitySet<ProductLookup>("ProductLookup");

				ProductSummaries = mb.EntitySet<ProductSummary>("ProductSummaries");
				ProductSummary = ProductSummaries.EntityType;

				ProductTotalByBookers = mb.EntitySet<ProductTotalByBooker>("ProductTotalByBookers");
				ProductTotalByBooker = ProductTotalByBookers.EntityType;

				ProductTotalByDays = mb.EntitySet<ProductTotalByDay>("ProductTotalByDays");
				ProductTotalByDay = ProductTotalByDays.EntityType;

				ProductTotalByMonths = mb.EntitySet<ProductTotalByMonth>("ProductTotalByMonths");
				ProductTotalByMonth = ProductTotalByMonths.EntityType;

				ProductTotalByOwners = mb.EntitySet<ProductTotalByOwner>("ProductTotalByOwners");
				ProductTotalByOwner = ProductTotalByOwners.EntityType;

				ProductTotalByProviders = mb.EntitySet<ProductTotalByProvider>("ProductTotalByProviders");
				ProductTotalByProvider = ProductTotalByProviders.EntityType;

				ProductTotalByQuarters = mb.EntitySet<ProductTotalByQuarter>("ProductTotalByQuarters");
				ProductTotalByQuarter = ProductTotalByQuarters.EntityType;

				ProductTotalBySellers = mb.EntitySet<ProductTotalBySeller>("ProductTotalBySellers");
				ProductTotalBySeller = ProductTotalBySellers.EntityType;

				ProductTotalByTypes = mb.EntitySet<ProductTotalByType>("ProductTotalByTypes");
				ProductTotalByType = ProductTotalByTypes.EntityType;

				ProductTotalByYears = mb.EntitySet<ProductTotalByYear>("ProductTotalByYears");
				ProductTotalByYear = ProductTotalByYears.EntityType;

				ProfitDistributionByCustomers = mb.EntitySet<ProfitDistributionByCustomer>("ProfitDistributionByCustomers");
				ProfitDistributionByCustomer = ProfitDistributionByCustomers.EntityType;

				ProfitDistributionByProviders = mb.EntitySet<ProfitDistributionByProvider>("ProfitDistributionByProviders");
				ProfitDistributionByProvider = ProfitDistributionByProviders.EntityType;

				RailwayDocuments = mb.EntitySet<RailwayDocument>("RailwayDocuments");
				RailwayDocument = RailwayDocuments.EntityType;
				mb.EntitySet<RailwayDocumentLookup>("RailwayDocumentLookup");

				Receipts = mb.EntitySet<Receipt>("Receipts");
				Receipt = Receipts.EntityType;
				mb.EntitySet<InvoiceLookup>("ReceiptLookup");

				RoamingOperators = mb.EntitySet<RoamingOperator>("RoamingOperators");
				RoamingOperator = RoamingOperators.EntityType;
				mb.EntitySet<OrganizationLookup>("RoamingOperatorLookup");

				Sequences = mb.EntitySet<Sequence>("Sequences");
				Sequence = Sequences.EntityType;
				mb.EntitySet<SequenceLookup>("SequenceLookup");

				SimCards = mb.EntitySet<SimCard>("SimCards");
				SimCard = SimCards.EntityType;
				mb.EntitySet<SimCardLookup>("SimCardLookup");

				SystemConfigurations = mb.EntitySet<SystemConfiguration>("SystemConfigurations");
				SystemConfiguration = SystemConfigurations.EntityType;

				TourProviders = mb.EntitySet<TourProvider>("TourProviders");
				TourProvider = TourProviders.EntityType;
				mb.EntitySet<OrganizationLookup>("TourProviderLookup");

				Tours = mb.EntitySet<Tour>("Tours");
				Tour = Tours.EntityType;
				mb.EntitySet<TourLookup>("TourLookup");

				TransferProviders = mb.EntitySet<TransferProvider>("TransferProviders");
				TransferProvider = TransferProviders.EntityType;
				mb.EntitySet<OrganizationLookup>("TransferProviderLookup");

				Transfers = mb.EntitySet<Transfer>("Transfers");
				Transfer = Transfers.EntityType;
				mb.EntitySet<TransferLookup>("TransferLookup");

				Users = mb.EntitySet<User>("Users");
				User = Users.EntityType;
				mb.EntitySet<UserLookup>("UserLookup");

				WireTransfers = mb.EntitySet<WireTransfer>("WireTransfers");
				WireTransfer = WireTransfers.EntityType;
				mb.EntitySet<WireTransferLookup>("WireTransferLookup");

				ProductFilter = mb.EntitySet<ProductFilter>("ProductFilter");
				EverydayProfitReportParams = mb.EntitySet<EverydayProfitReportParams>("EverydayProfitReportParams");
				FlownReportParams = mb.EntitySet<FlownReportParams>("FlownReportParams");
				ProductSummaryParams = mb.EntitySet<ProductSummaryParams>("ProductSummaryParams");
				ProductTotalByBookerParams = mb.EntitySet<ProductTotalByBookerParams>("ProductTotalByBookerParams");
				ProductTotalByDayParams = mb.EntitySet<ProductTotalByDayParams>("ProductTotalByDayParams");
				ProductTotalByMonthParams = mb.EntitySet<ProductTotalByMonthParams>("ProductTotalByMonthParams");
				ProductTotalByOwnerParams = mb.EntitySet<ProductTotalByOwnerParams>("ProductTotalByOwnerParams");
				ProductTotalByProviderParams = mb.EntitySet<ProductTotalByProviderParams>("ProductTotalByProviderParams");
				ProductTotalByQuarterParams = mb.EntitySet<ProductTotalByQuarterParams>("ProductTotalByQuarterParams");
				ProductTotalBySellerParams = mb.EntitySet<ProductTotalBySellerParams>("ProductTotalBySellerParams");
				ProductTotalByTypeParams = mb.EntitySet<ProductTotalByTypeParams>("ProductTotalByTypeParams");
				ProductTotalByYearParams = mb.EntitySet<ProductTotalByYearParams>("ProductTotalByYearParams");
				ProfitDistributionByCustomerParams = mb.EntitySet<ProfitDistributionByCustomerParams>("ProfitDistributionByCustomerParams");
				ProfitDistributionByProviderParams = mb.EntitySet<ProfitDistributionByProviderParams>("ProfitDistributionByProviderParams");
				GdsAgent_ApplyToUnassigned = mb.EntitySet<GdsAgent_ApplyToUnassigned>("GdsAgent_ApplyToUnassigned");

				#endregion


				#region Register calculated properties

				AviaDocument.Property(a => a.FullNumber);
				AviaRefunds.HasOptionalBinding(a => a.RefundedDocument, AviaDocuments);
				Consignments.HasOptionalBinding(a => a.Order, Orders);
				Consignment.ComplexProperty(a => a.Total);
				FlightSegment.Property(a => a.Name);
				GdsAgent_ApplyToUnassigned.EntityType.Property(a => a.ProductCount);
				GdsAgent.Property(a => a.Name);
				GdsAgent.Property(a => a.Codes);
				OrderItem.ComplexProperty(a => a.Total);
				OrderItem.ComplexProperty(a => a.GrandTotal);
				OrderItem.ComplexProperty(a => a.GivenVat);
				OrderItem.ComplexProperty(a => a.TaxedTotal);
				OrderItem.ComplexProperty(a => a.ServiceFee);
				OrderItem.Property(a => a.IsDelivered);
				OrderItem.Property(a => a.CheckNameUA);
				Order.ComplexProperty(a => a.Discount);
				Order.ComplexProperty(a => a.Total);
				Order.ComplexProperty(a => a.Vat);
				Order.ComplexProperty(a => a.Paid);
				Order.ComplexProperty(a => a.TotalDue);
				Order.Property(a => a.IsPaid);
				Order.ComplexProperty(a => a.VatDue);
				Order.Property(a => a.DeliveryBalance);
				Order.ComplexProperty(a => a.ServiceFee);
				Organization.Property(a => a.DepartmentCount);
				Organization.Property(a => a.EmployeeCount);
				Party.Property(a => a.NameForDocuments);
				Party.Property(a => a.FileCount);
				Passport.Property(a => a.Name);
				Passport.Property(a => a.AmadeusString);
				Passport.Property(a => a.GalileoString);
				Payment.Property(a => a.DocumentUniqueCode);
				Payment.Property(a => a.InvoiceDate);
				Payment.Property(a => a.IsPosted);
				Product.Property(a => a.IsDelivered);
				Product.Property(a => a.IsPaid);
				Product.ComplexProperty(a => a.Total);
				Product.ComplexProperty(a => a.ServiceTotal);
				Product.ComplexProperty(a => a.TotalToTransfer);
				Product.ComplexProperty(a => a.Profit);
				Product.ComplexProperty(a => a.ExtraCharge);
				Products.HasOptionalBinding(a => a.Passenger, Persons);
				SystemConfiguration.Property(a => a.CompanyName);

				#endregion


				#region Register functions

				AddDefaultFunctions(AccommodationProvider);
				AddDefaultFunctions(Accommodation);
				AddDefaultFunctions(AccommodationType);
				AddDefaultFunctions(ActiveOwner);
				AddDefaultFunctions(Agent);
				AddDefaultFunctions(Airline);
				AddDefaultFunctions(AirlineServiceClass);
				AddDefaultFunctions(Airport);
				AddDefaultFunctions(AviaDocument);
				AddDefaultFunctions(AviaMco);
				AddDefaultFunctions(AviaRefund);
				AddDefaultFunctions(AviaTicket);
				AddDefaultFunctions(BankAccount);
				AddDefaultFunctions(BusDocument);
				AddDefaultFunctions(BusTicketProvider);
				AddDefaultFunctions(BusTicketRefund);
				AddDefaultFunctions(BusTicket);
				AddDefaultFunctions(CarRentalProvider);
				AddDefaultFunctions(CarRental);
				AddDefaultFunctions(CashInOrderPayment);
				AddDefaultFunctions(CashOutOrderPayment);
				AddDefaultFunctions(CateringType);
				AddDefaultFunctions(CheckPayment);
				AddDefaultFunctions(Consignment);
				AddDefaultFunctions(Country);
				AddDefaultFunctions(CurrencyDailyRate);
				AddDefaultFunctions(Customer);
				AddDefaultFunctions(Department);
				AddDefaultFunctions(DocumentAccess);
				AddDefaultFunctions(DocumentOwner);
				AddDefaultFunctions(ElectronicPayment);
				AddDefaultFunctions(Excursion);
				AddDefaultFunctions(File);
				AddDefaultFunctions(FlightSegment);
				AddDefaultFunctions(GdsAgent);
				AddDefaultFunctions(GdsFile);
				AddDefaultFunctions(GenericProductProvider);
				AddDefaultFunctions(GenericProduct);
				AddDefaultFunctions(GenericProductType);
				AddDefaultFunctions(Identity);
				AddDefaultFunctions(InsuranceCompany);
				AddDefaultFunctions(InsuranceDocument);
				AddDefaultFunctions(InsuranceRefund);
				AddDefaultFunctions(Insurance);
				AddDefaultFunctions(InternalIdentity);
				AddDefaultFunctions(InternalTransfer);
				AddDefaultFunctions(Invoice);
				AddDefaultFunctions(Isic);
				AddDefaultFunctions(IssuedConsignment);
				AddDefaultFunctions(MilesCard);
				AddDefaultFunctions(OrderCheck);
				AddDefaultFunctions(OrderItem);
				AddDefaultFunctions(Order);
				AddDefaultFunctions(Organization);
				AddDefaultFunctions(Party);
				AddDefaultFunctions(Passport);
				AddDefaultFunctions(PasteboardProvider);
				AddDefaultFunctions(PasteboardRefund);
				AddDefaultFunctions(Pasteboard);
				AddDefaultFunctions(Payment);
				AddDefaultFunctions(PaymentSystem);
				AddDefaultFunctions(Person);
				AddDefaultFunctions(ProductPassenger);
				AddDefaultFunctions(Product);
				AddDefaultFunctions(RailwayDocument);
				AddDefaultFunctions(Receipt);
				AddDefaultFunctions(RoamingOperator);
				AddDefaultFunctions(Sequence);
				AddDefaultFunctions(SimCard);
				AddDefaultFunctions(SystemConfiguration);
				AddDefaultFunctions(TourProvider);
				AddDefaultFunctions(Tour);
				AddDefaultFunctions(TransferProvider);
				AddDefaultFunctions(Transfer);
				AddDefaultFunctions(User);
				AddDefaultFunctions(WireTransfer);
				AddDefaultFunctions(GdsAgent_ApplyToUnassigned.EntityType);

				#endregion


				#region Entity Actions

				AddEntityAction(Payment, "Void", a =>
				{
					a.Parameter<bool?>("b1").OptionalParameter = true;
					a.ReturnsFromEntitySet<Payment>("Payments");
				});

				AddEntityAction(Payment, "Unvoid", a =>
				{
					a.ReturnsFromEntitySet<Payment>("Payments");
				});

				AddEntityAction(Payment, "GetNote", a =>
				{
					a.Returns<string>();
				});

				#endregion

			}

		}

	}

}