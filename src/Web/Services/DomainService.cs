using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	public class DomainService : DomainWebService
	{

		[WebMethod]
		public AccommodationDto GetAccommodation(object id)
		{
			return db.Commit(() => dc.Accommodation.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAccommodation(AccommodationDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Accommodation.Update(dto, @params));
		}

		[WebMethod]
		public AccommodationTypeDto GetAccommodationType(object id)
		{
			return db.Commit(() => dc.AccommodationType.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAccommodationType(AccommodationTypeDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AccommodationType.Update(dto, @params));
		}

		[WebMethod]
		public AirlineCommissionPercentsDto GetAirlineCommissionPercents(object id)
		{
			return db.Commit(() => dc.AirlineCommissionPercents.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAirlineCommissionPercents(AirlineCommissionPercentsDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AirlineCommissionPercents.Update(dto, @params));
		}

		[WebMethod]
		public AirlineMonthCommissionDto GetAirlineMonthCommission(object id)
		{
			return db.Commit(() => dc.AirlineMonthCommission.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAirlineMonthCommission(AirlineMonthCommissionDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AirlineMonthCommission.Update(dto, @params));
		}

		[WebMethod]
		public AirlineServiceClassDto GetAirlineServiceClass(object id)
		{
			return db.Commit(() => dc.AirlineServiceClass.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAirlineServiceClass(AirlineServiceClassDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AirlineServiceClass.Update(dto, @params));
		}

		[WebMethod]
		public AirplaneModelDto GetAirplaneModel(object id)
		{
			return db.Commit(() => dc.AirplaneModel.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAirplaneModel(AirplaneModelDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AirplaneModel.Update(dto, @params));
		}

		[WebMethod]
		public AirportDto GetAirport(object id)
		{
			return db.Commit(() => dc.Airport.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAirport(AirportDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Airport.Update(dto, @params));
		}

		[WebMethod]
		public AmadeusAviaSftpRsaKeyDto GetAmadeusAviaSftpRsaKey(object id)
		{
			return db.Commit(() => dc.AmadeusAviaSftpRsaKey.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAmadeusAviaSftpRsaKey(AmadeusAviaSftpRsaKeyDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AmadeusAviaSftpRsaKey.Update(dto, @params));
		}

		[WebMethod]
		public AviaMcoDto GetAviaMco(object id)
		{
			return db.Commit(() => dc.AviaMco.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAviaMco(AviaMcoDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaMco.Update(dto, @params));
		}

		[WebMethod]
		public AviaMcoProcessDto GetAviaMcoProcess(object id)
		{
			return db.Commit(() => dc.AviaMcoProcess.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAviaMcoProcess(AviaMcoProcessDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaMcoProcess.Update(dto, @params));
		}

		[WebMethod]
		public AviaRefundDto GetAviaRefund(object id)
		{
			return db.Commit(() => dc.AviaRefund.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAviaRefund(AviaRefundDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaRefund.Update(dto, @params));
		}

		[WebMethod]
		public AviaRefundProcessDto GetAviaRefundProcess(object id)
		{
			return db.Commit(() => dc.AviaRefundProcess.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAviaRefundProcess(AviaRefundProcessDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaRefundProcess.Update(dto, @params));
		}

		[WebMethod]
		public AviaTicketDto GetAviaTicket(object id)
		{
			return db.Commit(() => dc.AviaTicket.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAviaTicket(AviaTicketDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaTicket.Update(dto, @params));
		}

		[WebMethod]
		public AviaTicketProcessDto GetAviaTicketProcess(object id)
		{
			return db.Commit(() => dc.AviaTicketProcess.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAviaTicketProcess(AviaTicketProcessDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaTicketProcess.Update(dto, @params));
		}

		[WebMethod]
		public BankAccountDto GetBankAccount(object id)
		{
			return db.Commit(() => dc.BankAccount.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateBankAccount(BankAccountDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.BankAccount.Update(dto, @params));
		}

		[WebMethod]
		public BusTicketDto GetBusTicket(object id)
		{
			return db.Commit(() => dc.BusTicket.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateBusTicket(BusTicketDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.BusTicket.Update(dto, @params));
		}

		[WebMethod]
		public BusTicketRefundDto GetBusTicketRefund(object id)
		{
			return db.Commit(() => dc.BusTicketRefund.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateBusTicketRefund(BusTicketRefundDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.BusTicketRefund.Update(dto, @params));
		}

		[WebMethod]
		public CarRentalDto GetCarRental(object id)
		{
			return db.Commit(() => dc.CarRental.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCarRental(CarRentalDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.CarRental.Update(dto, @params));
		}

		[WebMethod]
		public CashInOrderPaymentDto GetCashInOrderPayment(object id)
		{
			return db.Commit(() => dc.CashInOrderPayment.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCashInOrderPayment(CashInOrderPaymentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.CashInOrderPayment.Update(dto, @params));
		}

		[WebMethod]
		public CashOutOrderPaymentDto GetCashOutOrderPayment(object id)
		{
			return db.Commit(() => dc.CashOutOrderPayment.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCashOutOrderPayment(CashOutOrderPaymentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.CashOutOrderPayment.Update(dto, @params));
		}

		[WebMethod]
		public CateringTypeDto GetCateringType(object id)
		{
			return db.Commit(() => dc.CateringType.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCateringType(CateringTypeDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.CateringType.Update(dto, @params));
		}

		[WebMethod]
		public CheckPaymentDto GetCheckPayment(object id)
		{
			return db.Commit(() => dc.CheckPayment.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCheckPayment(CheckPaymentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.CheckPayment.Update(dto, @params));
		}

		[WebMethod]
		public ClosedPeriodDto GetClosedPeriod(object id)
		{
			return db.Commit(() => dc.ClosedPeriod.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateClosedPeriod(ClosedPeriodDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.ClosedPeriod.Update(dto, @params));
		}

		[WebMethod]
		public ConsignmentDto GetConsignment(object id)
		{
			return db.Commit(() => dc.Consignment.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateConsignment(ConsignmentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Consignment.Update(dto, @params));
		}

		[WebMethod]
		public ContractDto GetContract(object id)
		{
			return db.Commit(() => dc.Contract.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateContract(ContractDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Contract.Update(dto, @params));
		}

		[WebMethod]
		public CountryDto GetCountry(object id)
		{
			return db.Commit(() => dc.Country.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCountry(CountryDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Country.Update(dto, @params));
		}

		[WebMethod]
		public CurrencyDto GetCurrency(object id)
		{
			return db.Commit(() => dc.Currency.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCurrency(CurrencyDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Currency.Update(dto, @params));
		}

		[WebMethod]
		public CurrencyDailyRateDto GetCurrencyDailyRate(object id)
		{
			return db.Commit(() => dc.CurrencyDailyRate.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCurrencyDailyRate(CurrencyDailyRateDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.CurrencyDailyRate.Update(dto, @params));
		}

		[WebMethod]
		public DepartmentDto GetDepartment(object id)
		{
			return db.Commit(() => dc.Department.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateDepartment(DepartmentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Department.Update(dto, @params));
		}

		[WebMethod]
		public DepartmentListDetailDto GetDepartmentListDetail(object id)
		{
			return db.Commit(() => dc.DepartmentListDetail.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateDepartmentListDetail(DepartmentListDetailDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.DepartmentListDetail.Update(dto, @params));
		}

		[WebMethod]
		public DocumentAccessDto GetDocumentAccess(object id)
		{
			return db.Commit(() => dc.DocumentAccess.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateDocumentAccess(DocumentAccessDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.DocumentAccess.Update(dto, @params));
		}

		[WebMethod]
		public DocumentOwnerDto GetDocumentOwner(object id)
		{
			return db.Commit(() => dc.DocumentOwner.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateDocumentOwner(DocumentOwnerDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.DocumentOwner.Update(dto, @params));
		}

		[WebMethod]
		public ElectronicPaymentDto GetElectronicPayment(object id)
		{
			return db.Commit(() => dc.ElectronicPayment.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateElectronicPayment(ElectronicPaymentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.ElectronicPayment.Update(dto, @params));
		}

		[WebMethod]
		public ExcursionDto GetExcursion(object id)
		{
			return db.Commit(() => dc.Excursion.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateExcursion(ExcursionDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Excursion.Update(dto, @params));
		}

		[WebMethod]
		public FileDto GetFile(object id)
		{
			return db.Commit(() => dc.File.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateFile(FileDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.File.Update(dto, @params));
		}

		[WebMethod]
		public FlightSegmentDto GetFlightSegment(object id)
		{
			return db.Commit(() => dc.FlightSegment.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateFlightSegment(FlightSegmentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.FlightSegment.Update(dto, @params));
		}

		[WebMethod]
		public GdsAgentDto GetGdsAgent(object id)
		{
			return db.Commit(() => dc.GdsAgent.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateGdsAgent(GdsAgentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.GdsAgent.Update(dto, @params));
		}

		[WebMethod]
		public GenericProductDto GetGenericProduct(object id)
		{
			return db.Commit(() => dc.GenericProduct.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateGenericProduct(GenericProductDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.GenericProduct.Update(dto, @params));
		}

		[WebMethod]
		public GenericProductTypeDto GetGenericProductType(object id)
		{
			return db.Commit(() => dc.GenericProductType.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateGenericProductType(GenericProductTypeDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.GenericProductType.Update(dto, @params));
		}

		[WebMethod]
		public InsuranceDto GetInsurance(object id)
		{
			return db.Commit(() => dc.Insurance.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateInsurance(InsuranceDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Insurance.Update(dto, @params));
		}

		[WebMethod]
		public InsuranceRefundDto GetInsuranceRefund(object id)
		{
			return db.Commit(() => dc.InsuranceRefund.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateInsuranceRefund(InsuranceRefundDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.InsuranceRefund.Update(dto, @params));
		}

		[WebMethod]
		public InternalTransferDto GetInternalTransfer(object id)
		{
			return db.Commit(() => dc.InternalTransfer.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateInternalTransfer(InternalTransferDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.InternalTransfer.Update(dto, @params));
		}

		[WebMethod]
		public InvoiceDto GetInvoice(object id)
		{
			return db.Commit(() => dc.Invoice.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateInvoice(InvoiceDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Invoice.Update(dto, @params));
		}

		[WebMethod]
		public IsicDto GetIsic(object id)
		{
			return db.Commit(() => dc.Isic.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateIsic(IsicDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Isic.Update(dto, @params));
		}

		[WebMethod]
		public IssuedConsignmentDto GetIssuedConsignment(object id)
		{
			return db.Commit(() => dc.IssuedConsignment.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateIssuedConsignment(IssuedConsignmentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.IssuedConsignment.Update(dto, @params));
		}

		[WebMethod]
		public MilesCardDto GetMilesCard(object id)
		{
			return db.Commit(() => dc.MilesCard.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateMilesCard(MilesCardDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.MilesCard.Update(dto, @params));
		}

		[WebMethod]
		public ModificationDto GetModification(object id)
		{
			return db.Commit(() => dc.Modification.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateModification(ModificationDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Modification.Update(dto, @params));
		}

		[WebMethod]
		public OpeningBalanceDto GetOpeningBalance(object id)
		{
			return db.Commit(() => dc.OpeningBalance.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateOpeningBalance(OpeningBalanceDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.OpeningBalance.Update(dto, @params));
		}

		[WebMethod]
		public OrderDto GetOrder(object id)
		{
			return db.Commit(() => dc.Order.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateOrder(OrderDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Order.Update(dto, @params));
		}

		[WebMethod]
		public OrderItemDto GetOrderItem(object id)
		{
			return db.Commit(() => dc.OrderItem.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateOrderItem(OrderItemDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.OrderItem.Update(dto, @params));
		}

		[WebMethod]
		public OrganizationDto GetOrganization(object id)
		{
			return db.Commit(() => dc.Organization.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateOrganization(OrganizationDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Organization.Update(dto, @params));
		}

		[WebMethod]
		public PartyInvoiceDto GetPartyInvoice(object id)
		{
			return db.Commit(() => dc.PartyInvoice.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePartyInvoice(PartyInvoiceDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.PartyInvoice.Update(dto, @params));
		}

		[WebMethod]
		public PartyOrderDto GetPartyOrder(object id)
		{
			return db.Commit(() => dc.PartyOrder.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePartyOrder(PartyOrderDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.PartyOrder.Update(dto, @params));
		}

		[WebMethod]
		public PartyProductDto GetPartyProduct(object id)
		{
			return db.Commit(() => dc.PartyProduct.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePartyProduct(PartyProductDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.PartyProduct.Update(dto, @params));
		}

		[WebMethod]
		public PassportDto GetPassport(object id)
		{
			return db.Commit(() => dc.Passport.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePassport(PassportDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Passport.Update(dto, @params));
		}

		[WebMethod]
		public PasteboardDto GetPasteboard(object id)
		{
			return db.Commit(() => dc.Pasteboard.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePasteboard(PasteboardDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Pasteboard.Update(dto, @params));
		}

		[WebMethod]
		public PasteboardRefundDto GetPasteboardRefund(object id)
		{
			return db.Commit(() => dc.PasteboardRefund.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePasteboardRefund(PasteboardRefundDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.PasteboardRefund.Update(dto, @params));
		}

		[WebMethod]
		public PaymentSystemDto GetPaymentSystem(object id)
		{
			return db.Commit(() => dc.PaymentSystem.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePaymentSystem(PaymentSystemDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.PaymentSystem.Update(dto, @params));
		}

		[WebMethod]
		public PenalizeOperationDto GetPenalizeOperation(object id)
		{
			return db.Commit(() => dc.PenalizeOperation.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePenalizeOperation(PenalizeOperationDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.PenalizeOperation.Update(dto, @params));
		}

		[WebMethod]
		public PersonDto GetPerson(object id)
		{
			return db.Commit(() => dc.Person.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePerson(PersonDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Person.Update(dto, @params));
		}

		[WebMethod]
		public PersonListDetailDto GetPersonListDetail(object id)
		{
			return db.Commit(() => dc.PersonListDetail.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePersonListDetail(PersonListDetailDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.PersonListDetail.Update(dto, @params));
		}

		[WebMethod]
		public ProductPassengerDto GetProductPassenger(object id)
		{
			return db.Commit(() => dc.ProductPassenger.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateProductPassenger(ProductPassengerDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.ProductPassenger.Update(dto, @params));
		}

		[WebMethod]
		public ProfileDto GetProfile(object id)
		{
			return db.Commit(() => dc.Profile.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateProfile(ProfileDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Profile.Update(dto, @params));
		}

		[WebMethod]
		public SimCardDto GetSimCard(object id)
		{
			return db.Commit(() => dc.SimCard.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateSimCard(SimCardDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.SimCard.Update(dto, @params));
		}

		[WebMethod]
		public SystemConfigurationDto GetSystemConfiguration(object id)
		{
			return db.Commit(() => dc.SystemConfiguration.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateSystemConfiguration(SystemConfigurationDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.SystemConfiguration.Update(dto, @params));
		}

		[WebMethod]
		public TaskDto GetTask(object id)
		{
			return db.Commit(() => dc.Task.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateTask(TaskDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Task.Update(dto, @params));
		}

		[WebMethod]
		public TourDto GetTour(object id)
		{
			return db.Commit(() => dc.Tour.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateTour(TourDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Tour.Update(dto, @params));
		}

		[WebMethod]
		public TransferDto GetTransfer(object id)
		{
			return db.Commit(() => dc.Transfer.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateTransfer(TransferDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Transfer.Update(dto, @params));
		}

		[WebMethod]
		public UserDto GetUser(object id)
		{
			return db.Commit(() => dc.User.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateUser(UserDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.User.Update(dto, @params));
		}

		[WebMethod]
		public WireTransferDto GetWireTransfer(object id)
		{
			return db.Commit(() => dc.WireTransfer.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateWireTransfer(WireTransferDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.WireTransfer.Update(dto, @params));
		}

	}

}