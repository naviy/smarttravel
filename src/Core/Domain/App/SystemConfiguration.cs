using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Luxena.Base.Domain;
using Luxena.Domain;
using Luxena.Travel.Reports;



namespace Luxena.Travel.Domain
{


	[RU("Настройки системы"), Singleton]
	[AdminPrivileges]
	public partial class SystemConfiguration : Entity, IModifyAware
	{

		public virtual DateTime? ModifiedOn { get; set; }

		public virtual string ModifiedBy { get; set; }


		[RU("Организация")]
		public virtual Organization Company { get; set; }

		[RU("")]
		public virtual string CompanyName { get { return Company.As(a => a.Name); } }

		[RU("Реквизиты организации"), Text]
		public virtual string CompanyDetails { get; set; }

		[RU("")]
		public virtual Country Country { get; set; }

		[RU("Валюта по умолчанию")]
		public virtual Currency DefaultCurrency { get; set; }

		[RU("Использовать только валюту по умолчанию")]
		public virtual bool UseDefaultCurrencyForInput { get; set; }

		[RU("Ставка НДС,%")]
		public virtual decimal VatRate { get; set; }

		[RU("Использование riz-данных в Amadeus Air")]
		public virtual AmadeusRizUsingMode AmadeusRizUsingMode { get; set; }

		[RU("Требование паспортных данных авиакомпаниями")]
		public virtual bool IsPassengerPassportRequired { get; set; }

		[RU("Формирование номенклатур заказа по авиадокументу")]
		public virtual AviaOrderItemGenerationOption AviaOrderItemGenerationOption { get; set; }

		[RU("Разрешить редактирование НДС в заказе")]
		public virtual bool AllowAgentSetOrderVat { get; set; }

		[RU("Использовать НДС авиадокумента в заказе")]
		public virtual bool UseAviaDocumentVatInOrder { get; set; }

		[RU("Расчет НДС в авиадокументе")]
		public virtual AviaDocumentVatOptions AviaDocumentVatOptions { get; set; }

		[RU("Текст поля \"Главный бухгалтер\""), Text]
		public virtual string AccountantDisplayString { get; set; }

		[RU("Корреспондентский счет для ПКО")]
		public virtual string IncomingCashOrderCorrespondentAccount { get; set; }

		[RU("Разделять доступ к документам")]
		public virtual bool SeparateDocumentAccess { get; set; }

		[RU("Возможность редактировать и обрабатывать билеты других агентов")]
		public virtual bool AllowOtherAgentsToModifyProduct { get; set; }

		[RU("Обязательное ЕДРПОУ для организаций")]
		public virtual bool IsOrganizationCodeRequired { get; set; }

		[RU("Использовать доп. доход от АК при обработке авиадокументов")]
		public virtual bool UseAviaHandling { get; set; }

		[RU("Дней до отправления")]
		public virtual int DaysBeforeDeparture { get; set; }

		[RU("Ответственный за поздравление именинников")]
		public virtual Person BirthdayTaskResponsible { get; set; }

		[RU("Заказ обязателен для обработки документов")]
		public virtual bool IsOrderRequiredForProcessedDocument { get; set; }

		[RU("Показывать статистику на главной страницы с")]
		public virtual DateTime? MetricsFromDate { get; set; }

		[RU("Бронировки в документах по офису")]
		public virtual bool ReservationsInOfficeMetrics { get; set; }

		[RU("Описание обязательно для MCO")]
		public virtual bool McoRequiresDescription { get; set; }

		[RU("Neutral Airline Code")]
		public virtual string NeutralAirlineCode { get; set; }

		[RU("Заказы: НДС только от сервисного сбора")]
		public virtual bool Order_UseServiceFeeOnlyInVat { get; set; }

		[RU("Инвойсы: новый номер")]
		public virtual InvoiceNumberMode Invoice_NumberMode { get; set; }

		[RU("Накладные: новый номер")]
		public virtual InvoiceNumberMode Consignment_NumberMode { get; set; }

		[RU("Инвойсы: важное примечание"), Text(6)]
		public virtual string InvoicePrinter_FooterDetails { get; set; }

		[RU("DrctWebService_LoadedOn"), DateTime2]
		public virtual DateTime? DrctWebService_LoadedOn { get; set; }

		[RU("GalileoWebService_LoadedOn"), DateTime2]
		public virtual DateTime? GalileoWebService_LoadedOn { get; set; }

		[RU("GalileoRailWebService_LoadedOn"), DateTime2]
		public virtual DateTime? GalileoRailWebService_LoadedOn { get; set; }

		[RU("GalileoBusWebService_LoadedOn"), DateTime2]
		public virtual DateTime? GalileoBusWebService_LoadedOn { get; set; }

		[RU("TravelPointWebService_LoadedOn"), DateTime2]
		public virtual DateTime? TravelPointWebService_LoadedOn { get; set; }

		[RU("Показывать в накладных строку для сбора системы бронирования")]
		public virtual bool Consignment_SeparateBookingFee { get; set; }

		[RU("Тип оплаты для ж/д билетов по умолчанию")]
		public virtual PaymentType? Pasterboard_DefaultPaymentType { get; set; }


		[RU("Не предлагать распечатать все билеты из бронировки")]
		public virtual bool Ticket_NoPrintReservations { get; set; }


		//===


		public virtual string GetSupplierDetails(Domain db, Order order = null)
		{
			var supplier = Company;

			if (supplier == null)
				return null;

			var sb = new StringBuilder()
				.AppendLine(supplier.NameForDocuments);

			var bankAccount = order?.BankAccount.Do(a => sb.AppendLine(a.Description));
			if (bankAccount == null)
				db.BankAccount.Query.Where(a => a.IsDefault).ForEach(a => sb.AppendLine(a.Description));

			if (CompanyDetails.Yes())
				sb.AppendLine(CompanyDetails);

			if (supplier.LegalAddress.Yes())
				sb.AppendFormat(ReportRes.InvoicePrinter_Address, supplier.LegalAddress)
					.AppendLine();

			var separator = string.Empty;

			if (supplier.Phone1.Yes())
			{
				sb.AppendFormat(ReportRes.InvoicePrinter_Phone, supplier.Phone1);
				separator = ", ";
			}

			if (supplier.Fax.Yes())
				sb.Append(separator)
					.AppendFormat(ReportRes.InvoicePrinter_Fax, supplier.Fax);

			return sb.ToString().Replace("\r", string.Empty);
		}

		public virtual string GetCustomerDetails(Domain db, Party customer)
		{
			if (customer == null) return null;

			var sb = new StringBuilder()
				.AppendLine(customer.NameForDocuments);

			if (customer.Details.Yes())
				sb.AppendLine(customer.Details);

			if (customer.LegalAddress.Yes())
				sb.AppendFormat(ReportRes.InvoicePrinter_Address, customer.LegalAddress)
					.AppendLine();

			var separator = string.Empty;

			if (customer.Phone1.Yes())
			{
				sb.AppendFormat(ReportRes.InvoicePrinter_Phone, customer.Phone1);
				separator = ", ";
			}

			if (customer.Fax.Yes())
				sb.Append(separator)
					.AppendFormat(ReportRes.InvoicePrinter_Fax, customer.Fax);

			return sb.ToString().Replace("\r", string.Empty);
		}



		public class Service : EntityService<SystemConfiguration>
		{

		}

	}



	partial class Domain
	{
		public SystemConfiguration Configuration {[DebuggerStepThrough] get { return ResolveSingleton(ref _configuration); } }
		private SystemConfiguration _configuration;
	}


}