using System;
using System.Data.Entity;
using System.Linq;
using System.Text;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Настройки системы")]
	[AdminPrivileges]
	public partial class SystemConfiguration : Entity
	{

		public DateTimeOffset? ModifiedOn { get; set; }

		public string ModifiedBy { get; set; }
		
		[RU("Организация")]
		protected Organization _Company;

		[RU("")]
		public string CompanyName => Company?.Name;

		[RU("Реквизиты организации"), Text]
		public string CompanyDetails { get; set; }

		protected Country _Country;

		[RU("Валюта по умолчанию")]
		public string DefaultCurrency { get; set; }

		[RU("Использовать только валюту по умолчанию")]
		public bool UseDefaultCurrencyForInput { get; set; }

		[RU("Ставка НДС,%")]
		public decimal VatRate { get; set; }

		[RU("Использование riz-данных в Amadeus Air")]
		public AmadeusRizUsingMode AmadeusRizUsingMode { get; set; }

		[RU("Требование паспортных данных авиакомпаниями")]
		public bool IsPassengerPassportRequired { get; set; }

		[RU("Формирование номенклатур заказа по авиадокументу")]
		public ProductOrderItemGenerationOption AviaOrderItemGenerationOption { get; set; }

		[RU("Разрешить редактирование НДС в заказе")]
		public bool AllowAgentSetOrderVat { get; set; }

		[RU("Использовать НДС авиадокумента в заказе")]
		public bool UseAviaDocumentVatInOrder { get; set; }

		[RU("Расчет НДС в авиадокументе")]
		public AviaDocumentVatOptions AviaDocumentVatOptions { get; set; }

		[RU("Текст поля \"Главный бухгалтер\""), Text]
		public string AccountantDisplayString { get; set; }

		[RU("Корреспондентский счет для ПКО")]
		public string IncomingCashOrderCorrespondentAccount { get; set; }

		[RU("Разделять доступ к документам")]
		public bool SeparateDocumentAccess { get; set; }

		[RU("Обязательное ЕДРПОУ для организаций")]
		public bool IsOrganizationCodeRequired { get; set; }

		[RU("Использовать доп. доход от АК при обработке авиадокументов")]
		public bool UseAviaHandling { get; set; }

		[RU("Дней до отправления")]
		public int DaysBeforeDeparture { get; set; }

		[RU("Ответственный за поздравление именинников")]
		protected Person _BirthdayTaskResponsible;

		[RU("Заказ обязателен для обработки документов")]
		public bool IsOrderRequiredForProcessedDocument { get; set; }

		[RU("Показывать статистику на главной страницы с")]
		public DateTimeOffset? MetricsFromDate { get; set; }

		[RU("Бронировки в документах по офису")]
		public bool ReservationsInOfficeMetrics { get; set; }

		[RU("Описание обязательно для MCO")]
		public bool McoRequiresDescription { get; set; }

		[RU("Neutral Airline Code")]
		public string NeutralAirlineCode { get; set; }

		[RU("Заказы: НДС только от сервисного сбора")]
		public bool Order_UseServiceFeeOnlyInVat { get; set; }

		[RU("Инвойсы: новый номер")]
		public InvoiceNumberMode Invoice_NumberMode { get; set; }

		[RU("Инвойсы: важное примечание"), Text(6)]
		public string InvoicePrinter_FooterDetails { get; set; }

		public DateTimeOffset? GalileoWebService_LoadedOn { get; set; }


		static partial void Config_(Domain.EntityConfiguration<SystemConfiguration> entity)
		{
			entity.Association(a => a.Company);//, a => a.SystemConfigurations_Company);
			entity.Association(a => a.Country);//, a => a.SystemConfigurations);
			entity.Association(a => a.BirthdayTaskResponsible);//, a => a.SystemConfigurations_BirthdayTaskResponsible);
		}

	}


	partial class Domain
	{
		public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

		public SystemConfiguration AppConfiguration => _appConfiguration ?? (_appConfiguration = SystemConfigurations.First());
		private SystemConfiguration _appConfiguration;


		/// <summary>Валюта по умолчанию</summary>
		public string DefaultCurrency => AppConfiguration.DefaultCurrency;

		public static Money operator +(Money m, Domain db)
		{
			return m == null ? null : m + db.AppConfiguration.DefaultCurrency;
		}


		public string GetSupplierDetails(string lang, Order order = null)
		{
			var cfg = db.AppConfiguration;
			var supplier = cfg.Company;

			if (supplier == null) return null;

			var bankAccount = order.As(a => a.BankAccount) ?? db.BankAccounts.By(a => a.IsDefault);

			return (
				supplier.NameForDocuments + "\n" +
				bankAccount.As(a => a.Description + "\n") +
				cfg.CompanyDetails.As(a => a + "\n") +
				supplier.GetDetails(lang)
			).Replace("\r", "");
		}

		public string GetCustomerDetails(string lang, Party customer)
		{
			if (customer == null) return null;

			return (
				customer.NameForDocuments + "\n" + 
				customer.GetDetails(lang)
			).Replace("\r", "");
		}
	}

}