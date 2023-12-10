// ReSharper disable RedundantUsingDirective, RedundantExplicitArrayCreation, RedundantArgumentDefaultValue, NotAccessedField.Local
using System.Runtime.CompilerServices;

using LxnBase.UI;


namespace Luxena.Travel
{

	#region Entity

	public partial class EntitySemantic : SemanticEntity
	{

		public EntitySemantic()
		{
			_name = "Entity";
			_className = "Entity";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] { dsm.Passport, dsm.BankAccount, dsm.MilesCard, dsm.Party, dsm.Organization, dsm.Person, dsm.Department, dsm.Order, dsm.CurrencyDailyRate, dsm.CateringType, dsm.AccommodationType, dsm.GenericProductType, dsm.ProductPassenger, dsm.BusTicket, dsm.Excursion, dsm.Isic, dsm.Pasteboard, dsm.Transfer, dsm.GenericProduct, dsm.CarRental, dsm.Tour, dsm.Accommodation, dsm.SimCard, dsm.Insurance, dsm.AviaDocument, dsm.Product, dsm.AviaTicket, dsm.AviaMco, dsm.AviaRefund, dsm.Country, dsm.AirlineServiceClass, dsm.AirlineMonthCommission, dsm.Airport, dsm.PaymentSystem, dsm.Currency, dsm.Identity, dsm.User, dsm.UserVisit, dsm.SystemConfiguration, dsm.AirlineCommissionPercents, dsm.DocumentOwner, dsm.DocumentAccess, dsm.ClosedPeriod, dsm.OpeningBalance, dsm.InternalTransfer, dsm.GdsAgent, dsm.GdsFile, dsm.Invoice, dsm.Task, dsm.Payment, dsm.Contract, dsm.Modification, dsm.FlightSegment, dsm.AirplaneModel, dsm.AmadeusAviaSftpRsaKey };
			};
		}

		[PreserveCase]
		public SemanticMember Id = Member
			;

		[PreserveCase]
		public SemanticMember Version = Member
			.Int32()
			.Required();

	}

	/*
				se.Id,
				se.Version,
	*/


	#endregion


	#region Entity2

	public partial class Entity2Semantic : EntitySemantic
	{

		public Entity2Semantic()
		{
			_name = "Entity2";
			_className = "Entity2";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] { dsm.Passport, dsm.BankAccount, dsm.MilesCard, dsm.Party, dsm.Organization, dsm.Person, dsm.Department, dsm.Order, dsm.CurrencyDailyRate, dsm.CateringType, dsm.AccommodationType, dsm.GenericProductType, dsm.ProductPassenger, dsm.BusTicket, dsm.Excursion, dsm.Isic, dsm.Pasteboard, dsm.Transfer, dsm.GenericProduct, dsm.CarRental, dsm.Tour, dsm.Accommodation, dsm.SimCard, dsm.Insurance, dsm.AviaDocument, dsm.Product, dsm.AviaTicket, dsm.AviaMco, dsm.AviaRefund, dsm.Country, dsm.AirlineServiceClass, dsm.AirlineMonthCommission, dsm.Airport, dsm.PaymentSystem, dsm.Currency, dsm.Identity, dsm.User, dsm.DocumentAccess, dsm.ClosedPeriod, dsm.OpeningBalance, dsm.InternalTransfer, dsm.GdsAgent, dsm.GdsFile, dsm.Task, dsm.Payment, dsm.Contract, dsm.FlightSegment, dsm.AirplaneModel, dsm.AmadeusAviaSftpRsaKey };
			};
		}

		/// <summary>Дата создания</summary>
		[PreserveCase]
		public SemanticMember CreatedOn = Member
			.Title("Дата создания")
			.DateTime2()
			.Required()
			.Utility();

		/// <summary>Создано пользователем</summary>
		[PreserveCase]
		public SemanticMember CreatedBy = Member
			.Title("Создано пользователем")
			.String()
			.Required()
			.Utility();

		/// <summary>Дата изменения</summary>
		[PreserveCase]
		public SemanticMember ModifiedOn = Member
			.Title("Дата изменения")
			.DateTime2()
			.Utility();

		/// <summary>Изменено пользователем</summary>
		[PreserveCase]
		public SemanticMember ModifiedBy = Member
			.Title("Изменено пользователем")
			.String()
			.Utility();

	}

	/*
				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
	*/


	#endregion


	#region Entity3

	public partial class Entity3Semantic : Entity2Semantic
	{

		public Entity3Semantic()
		{
			_name = "Entity3";
			_className = "Entity3";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] { dsm.BankAccount, dsm.Party, dsm.Organization, dsm.Person, dsm.Department, dsm.CateringType, dsm.AccommodationType, dsm.GenericProductType, dsm.Country, dsm.Airport, dsm.PaymentSystem, dsm.Currency, dsm.Identity, dsm.User, dsm.GdsFile, dsm.AirplaneModel };
			};
		}

		/// <summary>Название</summary>
		[PreserveCase]
		public SemanticMember Name = Member
			.Title("Название")
			.String()
			.Required()
			.EntityName();

	}

	/*
				se.Name,
	*/


	#endregion


	#region Entity3D

	public partial class Entity3DSemantic : Entity3Semantic
	{

		public Entity3DSemantic()
		{
			_name = "Entity3D";
			_className = "Entity3D";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] { dsm.BankAccount, dsm.CateringType, dsm.AccommodationType, dsm.Identity, dsm.User };
			};
		}

		/// <summary>Описание</summary>
		[PreserveCase]
		public SemanticMember Description = Member
			.Title("Описание")
			.Text(3);

	}

	/*
				se.Description,
	*/


	#endregion


	#region Passport

	/// <summary>Паспорт</summary>
	public partial class PassportSemantic : Entity2Semantic
	{

		public PassportSemantic()
		{
			_name = "Passport";
			_className = "Passport";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Паспорт";
			_titles = "Паспорта";
		}

		[PreserveCase]
		public SemanticMember Owner = Member
			.Reference("Person");

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.EntityName();

		/// <summary>Имя</summary>
		[PreserveCase]
		public SemanticMember FirstName = Member
			.Title("Имя")
			.String();

		/// <summary>Отчество</summary>
		[PreserveCase]
		public SemanticMember MiddleName = Member
			.Title("Отчество")
			.String();

		/// <summary>Фамилия</summary>
		[PreserveCase]
		public SemanticMember LastName = Member
			.Title("Фамилия")
			.String();

		/// <summary>Гражданство</summary>
		[PreserveCase]
		public SemanticMember Citizenship = Member
			.Title("Гражданство")
			.Reference("Country");

		/// <summary>Дата рождения</summary>
		[PreserveCase]
		public SemanticMember Birthday = Member
			.Title("Дата рождения")
			.Date();

		/// <summary>Пол</summary>
		[PreserveCase]
		public SemanticMember Gender = Member
			.Title("Пол")
			.EnumItems(new object[][]
			{
				new object[] { 1, "Женский" }, // Female
				new object[] { 0, "Мужской" }, // Male
			});

		/// <summary>Выдавшая страна</summary>
		[PreserveCase]
		public SemanticMember IssuedBy = Member
			.Title("Выдавшая страна")
			.Reference("Country");

		/// <summary>Действителен до</summary>
		[PreserveCase]
		public SemanticMember ExpiredOn = Member
			.Title("Действителен до")
			.Date();

		/// <summary>Примечание</summary>
		[PreserveCase]
		public SemanticMember Note = Member
			.Title("Примечание")
			.Text(3);

		/// <summary>Данные для Amadeus</summary>
		[PreserveCase]
		public SemanticMember AmadeusString = Member
			.Title("Данные для Amadeus")
			.String();

		/// <summary>Данные для Galileo</summary>
		[PreserveCase]
		public SemanticMember GalileoString = Member
			.Title("Данные для Galileo")
			.String();

	}

	/*
				se.Owner,
				se.Number,
				se.FirstName,
				se.MiddleName,
				se.LastName,
				se.Citizenship,
				se.Birthday,
				se.Gender,
				se.IssuedBy,
				se.ExpiredOn,
				se.Note,
				se.AmadeusString,
				se.GalileoString,
	*/

	public partial class PassportListTab : EntityListTab
	{

		static PassportListTab()
		{
			RegisterList("Passport", typeof(PassportListTab));
		}

		public PassportListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Passport;
		}

		private PassportSemantic se;

	}


	public partial class PassportEditForm : EntityEditForm
	{

		static PassportEditForm()
		{
			RegisterEdit("Passport", typeof(PassportEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Passport;
		}

		private PassportSemantic se;

	}

	#endregion


	#region BankAccount

	/// <summary>Банковский счёт</summary>
	public partial class BankAccountSemantic : Entity3DSemantic
	{

		public BankAccountSemantic()
		{
			_name = "BankAccount";
			_className = "BankAccount";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Банковский счёт";
			_titles = "Банковские счёта";
		}

		/// <summary>Использовать по умолчанию</summary>
		[PreserveCase]
		public SemanticMember IsDefault = Member
			.Title("Использовать по умолчанию")
			.Bool()
			.Required();

		/// <summary>Реквизиты организации</summary>
		[PreserveCase]
		public SemanticMember CompanyDetails = Member
			.Title("Реквизиты организации")
			.Text(3);

		/// <summary>Примечание</summary>
		[PreserveCase]
		public SemanticMember Note = Member
			.Title("Примечание")
			.Text(3);

	}

	/*
				se.IsDefault,
				se.CompanyDetails,
				se.Note,
	*/

	public partial class BankAccountListTab : Entity3DListTab
	{

		static BankAccountListTab()
		{
			RegisterList("BankAccount", typeof(BankAccountListTab));
		}

		public BankAccountListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.BankAccount;
		}

		private BankAccountSemantic se;

	}


	public partial class BankAccountEditForm : Entity3DEditForm
	{

		static BankAccountEditForm()
		{
			RegisterEdit("BankAccount", typeof(BankAccountEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.BankAccount;
		}

		private BankAccountSemantic se;

	}

	#endregion


	#region MilesCard

	public partial class MilesCardSemantic : Entity2Semantic
	{

		public MilesCardSemantic()
		{
			_name = "MilesCard";
			_className = "MilesCard";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
		}

		[PreserveCase]
		public SemanticMember Owner = Member
			.Reference("Person");

		[PreserveCase]
		public SemanticMember Number = Member
			.String()
			.EntityName();

		/// <summary>Организация</summary>
		[PreserveCase]
		public SemanticMember Organization = Member
			.Title("Организация")
			.Reference("Organization");

	}

	/*
				se.Owner,
				se.Number,
				se.Organization,
	*/

	public partial class MilesCardListTab : EntityListTab
	{

		static MilesCardListTab()
		{
			RegisterList("MilesCard", typeof(MilesCardListTab));
		}

		public MilesCardListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.MilesCard;
		}

		private MilesCardSemantic se;

	}


	public partial class MilesCardEditForm : EntityEditForm
	{

		static MilesCardEditForm()
		{
			RegisterEdit("MilesCard", typeof(MilesCardEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.MilesCard;
		}

		private MilesCardSemantic se;

	}

	#endregion


	#region Party

	public partial class PartySemantic : Entity3Semantic
	{

		public PartySemantic()
		{
			_name = "Party";
			_className = "Party";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] { dsm.Organization, dsm.Person, dsm.Department };
			};
		}

		[PreserveCase]
		public SemanticMember Type = Member
			.EnumItems(new object[][]
			{
				new object[] { 0, "Department" }, // Department
				new object[] { 1, "Organization" }, // Organization
				new object[] { 2, "Person" }, // Person
			})
			.Required();

		/// <summary>Официальное название</summary>
		[PreserveCase]
		public SemanticMember LegalName = Member
			.Title("Официальное название")
			.String();

		/// <summary>Подпись</summary>
		[PreserveCase]
		public SemanticMember Signature = Member
			.Title("Подпись")
			.String();

		/// <summary>Код</summary>
		[PreserveCase]
		public SemanticMember Code = Member
			.Title("Код")
			.String();

		[PreserveCase]
		public SemanticMember NameForDocuments = Member
			.String();

		/// <summary>№ бонусной карты</summary>
		[PreserveCase]
		public SemanticMember BonusCardNumber = Member
			.Title("№ бонусной карты")
			.String();

		/// <summary>Накоплено бонусов</summary>
		[PreserveCase]
		public SemanticMember BonusAmount = Member
			.Title("Накоплено бонусов")
			.Float2();

		/// <summary>Телефон 1</summary>
		[PreserveCase]
		public SemanticMember Phone1 = Member
			.Title("Телефон 1")
			.String();

		/// <summary>Телефон 2</summary>
		[PreserveCase]
		public SemanticMember Phone2 = Member
			.Title("Телефон 2")
			.String();

		/// <summary>Факс</summary>
		[PreserveCase]
		public SemanticMember Fax = Member
			.Title("Факс")
			.String();

		/// <summary>E-mail 1</summary>
		[PreserveCase]
		public SemanticMember Email1 = Member
			.Title("E-mail 1")
			.String();

		/// <summary>E-mail 2</summary>
		[PreserveCase]
		public SemanticMember Email2 = Member
			.Title("E-mail 2")
			.String();

		/// <summary>Веб адрес</summary>
		[PreserveCase]
		public SemanticMember WebAddress = Member
			.Title("Веб адрес")
			.String();

		/// <summary>Заказчик</summary>
		[PreserveCase]
		public SemanticMember IsCustomer = Member
			.Title("Заказчик")
			.Bool()
			.Required();

		/// <summary>Не может быть заказчиком</summary>
		[PreserveCase]
		public SemanticMember CanNotBeCustomer = Member
			.Title("Не может быть заказчиком")
			.Bool()
			.Required();

		/// <summary>Поставщик</summary>
		[PreserveCase]
		public SemanticMember IsSupplier = Member
			.Title("Поставщик")
			.Bool()
			.Required();

		/// <summary>Подчиняется</summary>
		[PreserveCase]
		public SemanticMember ReportsTo = Member
			.Title("Подчиняется")
			.Reference("Party");

		/// <summary>Банковский счёт агенства по умолчанию</summary>
		[PreserveCase]
		public SemanticMember DefaultBankAccount = Member
			.Title("Банковский счёт агенства по умолчанию")
			.EnumReference("BankAccount");

		/// <summary>Дополнительная информация</summary>
		[PreserveCase]
		public SemanticMember Details = Member
			.Title("Дополнительная информация")
			.Text(3);

		/// <summary>Юридический адрес</summary>
		[PreserveCase]
		public SemanticMember LegalAddress = Member
			.Title("Юридический адрес")
			.Text(4);

		/// <summary>Фактический адрес</summary>
		[PreserveCase]
		public SemanticMember ActualAddress = Member
			.Title("Фактический адрес")
			.Text(4);

		/// <summary>Примечание</summary>
		[PreserveCase]
		public SemanticMember Note = Member
			.Title("Примечание")
			.Text(3)
			.LineCount(6);

		/// <summary>Добавлять окончание к номеру счёта</summary>
		[PreserveCase]
		public SemanticMember InvoiceSuffix = Member
			.Title("Добавлять окончание к номеру счёта")
			.String();

		[PreserveCase]
		public SemanticMember Files = Member
			;

	}

	/*
				se.Type,
				se.LegalName,
				se.Signature,
				se.Code,
				se.NameForDocuments,
				se.BonusCardNumber,
				se.BonusAmount,
				se.Phone1,
				se.Phone2,
				se.Fax,
				se.Email1,
				se.Email2,
				se.WebAddress,
				se.IsCustomer,
				se.CanNotBeCustomer,
				se.IsSupplier,
				se.ReportsTo,
				se.DefaultBankAccount,
				se.Details,
				se.LegalAddress,
				se.ActualAddress,
				se.Note,
				se.InvoiceSuffix,
				se.Files,
	*/


	#endregion


	#region Organization

	/// <summary>Организация</summary>
	public partial class OrganizationSemantic : PartySemantic
	{

		public OrganizationSemantic()
		{
			_name = "Organization";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Организация";
			_titles = "Организации";

			Code
				.Title("Код предприятия (ЕДРПОУ)");
		}

		/// <summary>Данная организация является Авиакомпанией</summary>
		[PreserveCase]
		public SemanticMember IsAirline = Member
			.Title("Данная организация является Авиакомпанией")
			.Bool()
			.Required();

		/// <summary>IATA код</summary>
		[PreserveCase]
		public SemanticMember AirlineIataCode = Member
			.Title("IATA код")
			.String(2);

		/// <summary>Prefix код</summary>
		[PreserveCase]
		public SemanticMember AirlinePrefixCode = Member
			.Title("Prefix код")
			.String(3);

		/// <summary>Требование паспортных данных</summary>
		[PreserveCase]
		public SemanticMember AirlinePassportRequirement = Member
			.Title("Требование паспортных данных")
			.EnumItems(new object[][]
			{
				new object[] { 2, "Не требуется" }, // NotRequired
				new object[] { 1, "Требуется" }, // Required
				new object[] { 0, "По умолчанию" }, // SystemDefault
			})
			.Required();

		/// <summary>Для проживания</summary>
		[PreserveCase]
		public SemanticMember IsAccommodationProvider = Member
			.Title("Для проживания")
			.Bool()
			.Required();

		/// <summary>Для автобусных билетов</summary>
		[PreserveCase]
		public SemanticMember IsBusTicketProvider = Member
			.Title("Для автобусных билетов")
			.Bool()
			.Required();

		/// <summary>Для аренды авто</summary>
		[PreserveCase]
		public SemanticMember IsCarRentalProvider = Member
			.Title("Для аренды авто")
			.Bool()
			.Required();

		/// <summary>Для ж/д билетов</summary>
		[PreserveCase]
		public SemanticMember IsPasteboardProvider = Member
			.Title("Для ж/д билетов")
			.Bool()
			.Required();

		/// <summary>Для туров (готовых)</summary>
		[PreserveCase]
		public SemanticMember IsTourProvider = Member
			.Title("Для туров (готовых)")
			.Bool()
			.Required();

		/// <summary>Для трансферов</summary>
		[PreserveCase]
		public SemanticMember IsTransferProvider = Member
			.Title("Для трансферов")
			.Bool()
			.Required();

		/// <summary>Для дополнительных услуг</summary>
		[PreserveCase]
		public SemanticMember IsGenericProductProvider = Member
			.Title("Для дополнительных услуг")
			.Bool()
			.Required();

		/// <summary>Данная организация является Провайдером услуг</summary>
		[PreserveCase]
		public SemanticMember IsProvider = Member
			.Title("Данная организация является Провайдером услуг")
			.Bool()
			.Required();

		/// <summary>Данная организация является Страховой компанией</summary>
		[PreserveCase]
		public SemanticMember IsInsuranceCompany = Member
			.Title("Данная организация является Страховой компанией")
			.Bool()
			.Required();

		/// <summary>Данная организация является Роуминг-оператором</summary>
		[PreserveCase]
		public SemanticMember IsRoamingOperator = Member
			.Title("Данная организация является Роуминг-оператором")
			.Bool()
			.Required();

	}

	/*
				se.IsAirline,
				se.AirlineIataCode,
				se.AirlinePrefixCode,
				se.AirlinePassportRequirement,
				se.IsAccommodationProvider,
				se.IsBusTicketProvider,
				se.IsCarRentalProvider,
				se.IsPasteboardProvider,
				se.IsTourProvider,
				se.IsTransferProvider,
				se.IsGenericProductProvider,
				se.IsProvider,
				se.IsInsuranceCompany,
				se.IsRoamingOperator,
	*/

	public partial class OrganizationListTab : PartyListTab
	{

		static OrganizationListTab()
		{
			RegisterList("Organization", typeof(OrganizationListTab));
		}

		public OrganizationListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Organization;
		}

		private OrganizationSemantic se;

	}


	public partial class OrganizationEditForm : PartyEditForm
	{

		static OrganizationEditForm()
		{
			RegisterEdit("Organization", typeof(OrganizationEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Organization;
		}

		private OrganizationSemantic se;

	}

	#endregion


	#region Person

	/// <summary>Персона</summary>
	public partial class PersonSemantic : PartySemantic
	{

		public PersonSemantic()
		{
			_name = "Person";
			_className = "Person";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Персона";
			_titles = "Персоны";
		}

		/// <summary>Дата рождения</summary>
		[PreserveCase]
		public SemanticMember Birthday = Member
			.Title("Дата рождения")
			.Date();

		/// <summary>Организация</summary>
		[PreserveCase]
		public SemanticMember Organization = Member
			.Title("Организация")
			.Reference("Organization");

		/// <summary>Должность</summary>
		[PreserveCase]
		public SemanticMember Title = Member
			.Title("Должность")
			.String();

		[PreserveCase]
		public SemanticMember Passports = Member
			;

		[PreserveCase]
		public SemanticMember MilesCardsString = Member
			.String();

		[PreserveCase]
		public SemanticMember MilesCards = Member
			;

	}

	/*
				se.Birthday,
				se.Organization,
				se.Title,
				se.Passports,
				se.MilesCardsString,
				se.MilesCards,
	*/

	public partial class PersonListTab : PartyListTab
	{

		static PersonListTab()
		{
			RegisterList("Person", typeof(PersonListTab));
		}

		public PersonListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Person;
		}

		private PersonSemantic se;

	}


	public partial class PersonEditForm : PartyEditForm
	{

		static PersonEditForm()
		{
			RegisterEdit("Person", typeof(PersonEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Person;
		}

		private PersonSemantic se;

	}

	#endregion


	#region Department

	/// <summary>Подразделение</summary>
	public partial class DepartmentSemantic : PartySemantic
	{

		public DepartmentSemantic()
		{
			_name = "Department";
			_className = "Department";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Подразделение";
			_titles = "Подразделения";
		}

		/// <summary>Организация</summary>
		[PreserveCase]
		public SemanticMember Organization = Member
			.Title("Организация")
			.Reference("Organization");

	}

	/*
				se.Organization,
	*/

	public partial class DepartmentListTab : PartyListTab
	{

		static DepartmentListTab()
		{
			RegisterList("Department", typeof(DepartmentListTab));
		}

		public DepartmentListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Department;
		}

		private DepartmentSemantic se;

	}


	public partial class DepartmentEditForm : PartyEditForm
	{

		static DepartmentEditForm()
		{
			RegisterEdit("Department", typeof(DepartmentEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Department;
		}

		private DepartmentSemantic se;

	}

	#endregion


	#region AccommodationProvider

	public partial class AccommodationProviderSemantic : OrganizationSemantic
	{

		public AccommodationProviderSemantic()
		{
			_name = "AccommodationProvider";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region BusTicketProvider

	public partial class BusTicketProviderSemantic : OrganizationSemantic
	{

		public BusTicketProviderSemantic()
		{
			_name = "BusTicketProvider";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region PasteboardProvider

	public partial class PasteboardProviderSemantic : OrganizationSemantic
	{

		public PasteboardProviderSemantic()
		{
			_name = "PasteboardProvider";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region TransferProvider

	public partial class TransferProviderSemantic : OrganizationSemantic
	{

		public TransferProviderSemantic()
		{
			_name = "TransferProvider";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region GenericProductProvider

	public partial class GenericProductProviderSemantic : OrganizationSemantic
	{

		public GenericProductProviderSemantic()
		{
			_name = "GenericProductProvider";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region CarRentalProvider

	public partial class CarRentalProviderSemantic : OrganizationSemantic
	{

		public CarRentalProviderSemantic()
		{
			_name = "CarRentalProvider";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region TourProvider

	public partial class TourProviderSemantic : OrganizationSemantic
	{

		public TourProviderSemantic()
		{
			_name = "TourProvider";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region RoamingOperator

	public partial class RoamingOperatorSemantic : OrganizationSemantic
	{

		public RoamingOperatorSemantic()
		{
			_name = "RoamingOperator";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region InsuranceCompany

	/// <summary>Страховая компания</summary>
	public partial class InsuranceCompanySemantic : OrganizationSemantic
	{

		public InsuranceCompanySemantic()
		{
			_name = "InsuranceCompany";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Страховая компания";
			_titles = "Страховые компании";
		}

	}

	/*
	*/


	#endregion


	#region Airline

	/// <summary>Авиакомпания</summary>
	public partial class AirlineSemantic : OrganizationSemantic
	{

		public AirlineSemantic()
		{
			_name = "Airline";
			_className = "Organization";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Авиакомпания";
			_titles = "Авиакомпании";
		}

	}

	/*
	*/


	#endregion


	#region Customer

	public partial class CustomerSemantic : PartySemantic
	{

		public CustomerSemantic()
		{
			_name = "Customer";
			_className = "Party";
			_isAbstract = false;
			_getDerivedEntities = null;
		}

	}

	/*
	*/


	#endregion


	#region Order

	/// <summary>Заказ</summary>
	public partial class OrderSemantic : Entity2Semantic
	{

		public OrderSemantic()
		{
			_name = "Order";
			_className = "Order";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Заказ";
			_titles = "Заказы";
		}

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.EntityName();

		/// <summary>Дата выпуска</summary>
		[PreserveCase]
		public SemanticMember IssueDate = Member
			.Title("Дата выпуска")
			.Date()
			.Required()
			.EntityDate();

		/// <summary>Аннулирован</summary>
		[PreserveCase]
		public SemanticMember IsVoid = Member
			.Title("Аннулирован")
			.Bool()
			.Required();

		/// <summary>Заказчик</summary>
		[PreserveCase]
		public SemanticMember Customer = Member
			.Title("Заказчик")
			.Reference("Customer");

		/// <summary>Плательщик</summary>
		[PreserveCase]
		public SemanticMember BillTo = Member
			.Title("Плательщик")
			.Reference("Customer");

		/// <summary>Плательщик</summary>
		[PreserveCase]
		public SemanticMember BillToName = Member
			.Title("Плательщик")
			.String();

		/// <summary>Получатель</summary>
		[PreserveCase]
		public SemanticMember ShipTo = Member
			.Title("Получатель")
			.Reference("Customer");

		/// <summary>Посредник</summary>
		[PreserveCase]
		public SemanticMember Intermediary = Member
			.Title("Посредник")
			.Reference("Party");

		/// <summary>Скидка</summary>
		[PreserveCase]
		public SemanticMember Discount = Member
			.Title("Скидка")
			.Money();

		/// <summary>В т.ч. НДС</summary>
		[PreserveCase]
		public SemanticMember Vat = Member
			.Title("В т.ч. НДС")
			.Money();

		/// <summary>НДС только от сервисного сбора</summary>
		[PreserveCase]
		public SemanticMember UseServiceFeeOnlyInVat = Member
			.Title("НДС только от сервисного сбора")
			.Bool()
			.Required();

		/// <summary>Итого</summary>
		[PreserveCase]
		public SemanticMember Total = Member
			.Title("Итого")
			.Money();

		/// <summary>Оплачено</summary>
		[PreserveCase]
		public SemanticMember Paid = Member
			.Title("Оплачено")
			.Money();

		/// <summary>Оплачено нал</summary>
		[PreserveCase]
		public SemanticMember CheckPaid = Member
			.Title("Оплачено нал")
			.Money();

		/// <summary>Оплачено б/нал</summary>
		[PreserveCase]
		public SemanticMember WirePaid = Member
			.Title("Оплачено б/нал")
			.Money();

		/// <summary>Оплачено КК</summary>
		[PreserveCase]
		public SemanticMember CreditPaid = Member
			.Title("Оплачено КК")
			.Money();

		/// <summary>Оплачено прочее</summary>
		[PreserveCase]
		public SemanticMember RestPaid = Member
			.Title("Оплачено прочее")
			.Money();

		/// <summary>К оплате</summary>
		[PreserveCase]
		public SemanticMember TotalDue = Member
			.Title("К оплате")
			.Money();

		/// <summary>НДС к оплате</summary>
		[PreserveCase]
		public SemanticMember VatDue = Member
			.Title("НДС к оплате")
			.Money();

		/// <summary>Оплачен</summary>
		[PreserveCase]
		public SemanticMember IsPaid = Member
			.Title("Оплачен")
			.Bool()
			.Required();

		/// <summary>Баланс взаиморасчетов</summary>
		[PreserveCase]
		public SemanticMember DeliveryBalance = Member
			.Title("Баланс взаиморасчетов")
			.Float2()
			.Required();

		/// <summary>Дата начисления бонусов</summary>
		[PreserveCase]
		public SemanticMember BonusDate = Member
			.Title("Дата начисления бонусов")
			.Date();

		/// <summary>Списано бонусов</summary>
		[PreserveCase]
		public SemanticMember BonusSpentAmount = Member
			.Title("Списано бонусов")
			.Float2();

		/// <summary>Получатель бонусов</summary>
		[PreserveCase]
		public SemanticMember BonusRecipient = Member
			.Title("Получатель бонусов")
			.Reference("Party");

		/// <summary>Ответственный</summary>
		[PreserveCase]
		public SemanticMember AssignedTo = Member
			.Title("Ответственный")
			.Reference("Person");

		/// <summary>Банковский счёт</summary>
		[PreserveCase]
		public SemanticMember BankAccount = Member
			.Title("Банковский счёт")
			.EnumReference("BankAccount");

		/// <summary>Владелец</summary>
		[PreserveCase]
		public SemanticMember Owner = Member
			.Title("Владелец")
			.Reference("Party");

		/// <summary>Примечание</summary>
		[PreserveCase]
		public SemanticMember Note = Member
			.Title("Примечание")
			.Text(3);

		/// <summary>Общий доступ</summary>
		[PreserveCase]
		public SemanticMember IsPublic = Member
			.Title("Общий доступ")
			.Bool()
			.Required();

		/// <summary>Разрешить добавление билетов, даже если заказ находится в закрытом периоде</summary>
		[PreserveCase]
		public SemanticMember AllowAddProductsInClosedPeriod = Member
			.Title("Разрешить добавление билетов, даже если заказ находится в закрытом периоде")
			.Bool()
			.Required();

		/// <summary>Отображать в контроле оплат</summary>
		[PreserveCase]
		public SemanticMember IsSubjectOfPaymentsControl = Member
			.Title("Отображать в контроле оплат")
			.Bool()
			.Required();

		/// <summary>Выделять сервисный сбор</summary>
		[PreserveCase]
		public SemanticMember SeparateServiceFee = Member
			.Title("Выделять сервисный сбор")
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember Items = Member
			;

		[PreserveCase]
		public SemanticMember Invoices = Member
			;

		[PreserveCase]
		public SemanticMember InvoiceLastIndex = Member
			.Int32();

		[PreserveCase]
		public SemanticMember ConsignmentLastIndex = Member
			.Int32();

		[PreserveCase]
		public SemanticMember Payments = Member
			;

		[PreserveCase]
		public SemanticMember Tasks = Member
			;

		/// <summary>Сервисный сбор</summary>
		[PreserveCase]
		public SemanticMember ServiceFee = Member
			.Title("Сервисный сбор")
			.Money();

		[PreserveCase]
		public SemanticMember OutgoingTransfers = Member
			;

		[PreserveCase]
		public SemanticMember IncomingTransfers = Member
			;

		/// <summary>Накладные</summary>
		[PreserveCase]
		public SemanticMember ConsignmentNumbers = Member
			.Title("Накладные")
			.String();

		/// <summary>Счета</summary>
		[PreserveCase]
		public SemanticMember InvoiceNumbers = Member
			.Title("Счета")
			.String();

		/// <summary>Первый счет</summary>
		[PreserveCase]
		public SemanticMember FirstInvoiceNumber = Member
			.Title("Первый счет")
			.String();

		[PreserveCase]
		public SemanticMember ConsignmentRefs = Member
			;

	}

	/*
				se.Number,
				se.IssueDate,
				se.IsVoid,
				se.Customer,
				se.BillTo,
				se.BillToName,
				se.ShipTo,
				se.Intermediary,
				se.Discount,
				se.Vat,
				se.UseServiceFeeOnlyInVat,
				se.Total,
				se.Paid,
				se.CheckPaid,
				se.WirePaid,
				se.CreditPaid,
				se.RestPaid,
				se.TotalDue,
				se.VatDue,
				se.IsPaid,
				se.DeliveryBalance,
				se.BonusDate,
				se.BonusSpentAmount,
				se.BonusRecipient,
				se.AssignedTo,
				se.BankAccount,
				se.Owner,
				se.Note,
				se.IsPublic,
				se.AllowAddProductsInClosedPeriod,
				se.IsSubjectOfPaymentsControl,
				se.SeparateServiceFee,
				se.Items,
				se.Invoices,
				se.InvoiceLastIndex,
				se.ConsignmentLastIndex,
				se.Payments,
				se.Tasks,
				se.ServiceFee,
				se.OutgoingTransfers,
				se.IncomingTransfers,
				se.ConsignmentNumbers,
				se.InvoiceNumbers,
				se.FirstInvoiceNumber,
				se.ConsignmentRefs,
	*/

	public partial class OrderListTab : EntityListTab
	{

		static OrderListTab()
		{
			RegisterList("Order", typeof(OrderListTab));
		}

		public OrderListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Order;
		}

		private OrderSemantic se;

	}


	#endregion


	#region CurrencyDailyRate

	/// <summary>Курс валюты</summary>
	public partial class CurrencyDailyRateSemantic : Entity2Semantic
	{

		public CurrencyDailyRateSemantic()
		{
			_name = "CurrencyDailyRate";
			_className = "CurrencyDailyRate";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Date";
			_title ="Курс валюты";
			_titles = "Курсы валют";
		}

		/// <summary>Дата</summary>
		[PreserveCase]
		public SemanticMember Date = Member
			.Title("Дата")
			.Date()
			.Required()
			.EntityDate()
			.EntityName();

		/// <summary>UAH/EUR</summary>
		[PreserveCase]
		public SemanticMember UAH_EUR = Member
			.Title("UAH/EUR")
			.Float4();

		/// <summary>UAH/RUB</summary>
		[PreserveCase]
		public SemanticMember UAH_RUB = Member
			.Title("UAH/RUB")
			.Float4();

		/// <summary>UAH/USD</summary>
		[PreserveCase]
		public SemanticMember UAH_USD = Member
			.Title("UAH/USD")
			.Float4();

		/// <summary>RUB/EUR</summary>
		[PreserveCase]
		public SemanticMember RUB_EUR = Member
			.Title("RUB/EUR")
			.Float4();

		/// <summary>RUB/USD</summary>
		[PreserveCase]
		public SemanticMember RUB_USD = Member
			.Title("RUB/USD")
			.Float4();

		/// <summary>EUR/USD</summary>
		[PreserveCase]
		public SemanticMember EUR_USD = Member
			.Title("EUR/USD")
			.Float4();

	}

	/*
				se.Date,
				se.UAH_EUR,
				se.UAH_RUB,
				se.UAH_USD,
				se.RUB_EUR,
				se.RUB_USD,
				se.EUR_USD,
	*/

	public partial class CurrencyDailyRateListTab : EntityListTab
	{

		static CurrencyDailyRateListTab()
		{
			RegisterList("CurrencyDailyRate", typeof(CurrencyDailyRateListTab));
		}

		public CurrencyDailyRateListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.CurrencyDailyRate;
		}

		private CurrencyDailyRateSemantic se;

	}


	public partial class CurrencyDailyRateEditForm : EntityEditForm
	{

		static CurrencyDailyRateEditForm()
		{
			RegisterEdit("CurrencyDailyRate", typeof(CurrencyDailyRateEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.CurrencyDailyRate;
		}

		private CurrencyDailyRateSemantic se;

	}

	#endregion


	#region CateringType

	/// <summary>Тип питания</summary>
	public partial class CateringTypeSemantic : Entity3DSemantic
	{

		public CateringTypeSemantic()
		{
			_name = "CateringType";
			_className = "CateringType";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Тип питания";
			_titles = "Типы питания";
		}

	}

	/*
	*/

	public partial class CateringTypeListTab : Entity3DListTab
	{

		static CateringTypeListTab()
		{
			RegisterList("CateringType", typeof(CateringTypeListTab));
		}

		public CateringTypeListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.CateringType;
		}

		private CateringTypeSemantic se;

	}


	public partial class CateringTypeEditForm : Entity3DEditForm
	{

		static CateringTypeEditForm()
		{
			RegisterEdit("CateringType", typeof(CateringTypeEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.CateringType;
		}

		private CateringTypeSemantic se;

	}

	#endregion


	#region AccommodationType

	/// <summary>Тип проживания</summary>
	public partial class AccommodationTypeSemantic : Entity3DSemantic
	{

		public AccommodationTypeSemantic()
		{
			_name = "AccommodationType";
			_className = "AccommodationType";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Тип проживания";
			_titles = "Типы проживания";
		}

	}

	/*
	*/

	public partial class AccommodationTypeListTab : Entity3DListTab
	{

		static AccommodationTypeListTab()
		{
			RegisterList("AccommodationType", typeof(AccommodationTypeListTab));
		}

		public AccommodationTypeListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AccommodationType;
		}

		private AccommodationTypeSemantic se;

	}


	public partial class AccommodationTypeEditForm : Entity3DEditForm
	{

		static AccommodationTypeEditForm()
		{
			RegisterEdit("AccommodationType", typeof(AccommodationTypeEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AccommodationType;
		}

		private AccommodationTypeSemantic se;

	}

	#endregion


	#region GenericProductType

	/// <summary>Вид дополнительной услуги</summary>
	public partial class GenericProductTypeSemantic : Entity3Semantic
	{

		public GenericProductTypeSemantic()
		{
			_name = "GenericProductType";
			_className = "GenericProductType";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Вид дополнительной услуги";
			_titles = "Виды дополнительных услуг";
		}

	}

	/*
	*/

	public partial class GenericProductTypeListTab : Entity3ListTab
	{

		static GenericProductTypeListTab()
		{
			RegisterList("GenericProductType", typeof(GenericProductTypeListTab));
		}

		public GenericProductTypeListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.GenericProductType;
		}

		private GenericProductTypeSemantic se;

	}


	public partial class GenericProductTypeEditForm : Entity3EditForm
	{

		static GenericProductTypeEditForm()
		{
			RegisterEdit("GenericProductType", typeof(GenericProductTypeEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.GenericProductType;
		}

		private GenericProductTypeSemantic se;

	}

	#endregion


	#region ProductPassenger

	public partial class ProductPassengerSemantic : Entity2Semantic
	{

		public ProductPassengerSemantic()
		{
			_name = "ProductPassenger";
			_className = "ProductPassenger";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = null;
		}

		/// <summary>Услуга</summary>
		[PreserveCase]
		public SemanticMember Product = Member
			.Title("Услуга")
			.Reference("Product");

		/// <summary>Имя пассажира</summary>
		[PreserveCase]
		public SemanticMember PassengerName = Member
			.Title("Имя пассажира")
			.EmptyText("имя")
			.String();

		/// <summary>Пассажир</summary>
		[PreserveCase]
		public SemanticMember Passenger = Member
			.Title("Пассажир")
			.EmptyText("персона")
			.Reference("Person");

	}

	/*
				se.Product,
				se.PassengerName,
				se.Passenger,
	*/

	public partial class ProductPassengerListTab : EntityListTab
	{

		static ProductPassengerListTab()
		{
			RegisterList("ProductPassenger", typeof(ProductPassengerListTab));
		}

		public ProductPassengerListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.ProductPassenger;
		}

		private ProductPassengerSemantic se;

	}


	public partial class ProductPassengerEditForm : EntityEditForm
	{

		static ProductPassengerEditForm()
		{
			RegisterEdit("ProductPassenger", typeof(ProductPassengerEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.ProductPassenger;
		}

		private ProductPassengerSemantic se;

	}

	#endregion


	#region BusTicket

	/// <summary>Автобусный билет</summary>
	public partial class BusTicketSemantic : ProductSemantic
	{

		public BusTicketSemantic()
		{
			_name = "BusTicket";
			_className = "BusTicket";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Автобусный билет";
			_titles = "Автобусные билеты";

			RefundedProduct
				.Reference("BusTicket");

			ReissueFor
				.Reference("BusTicket");

			Provider
				.Reference("BusTicketProvider");
		}

		/// <summary>Пассажир</summary>
		[PreserveCase]
		public SemanticMember Passenger = Member
			.Title("Пассажир")
			.Reference("Person");

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String();

		/// <summary>Начальная станция</summary>
		[PreserveCase]
		public SemanticMember DeparturePlace = Member
			.Title("Начальная станция")
			.EmptyText("место")
			.String();

		/// <summary>Дата отправления</summary>
		[PreserveCase]
		public SemanticMember DepartureDate = Member
			.Title("Дата отправления")
			.EmptyText("дата")
			.Date();

		/// <summary>Время отправления</summary>
		[PreserveCase]
		public SemanticMember DepartureTime = Member
			.Title("Время отправления")
			.EmptyText("время")
			.String();

		/// <summary>Конечная станция</summary>
		[PreserveCase]
		public SemanticMember ArrivalPlace = Member
			.Title("Конечная станция")
			.EmptyText("место")
			.String();

		/// <summary>Дата прибытия</summary>
		[PreserveCase]
		public SemanticMember ArrivalDate = Member
			.Title("Дата прибытия")
			.EmptyText("дата")
			.Date();

		/// <summary>Время прибытия</summary>
		[PreserveCase]
		public SemanticMember ArrivalTime = Member
			.Title("Время прибытия")
			.EmptyText("время")
			.String();

		/// <summary>Номер места</summary>
		[PreserveCase]
		public SemanticMember SeatNumber = Member
			.Title("Номер места")
			.String();

	}

	/*
				se.Passenger,
				se.Number,
				se.DeparturePlace,
				se.DepartureDate,
				se.DepartureTime,
				se.ArrivalPlace,
				se.ArrivalDate,
				se.ArrivalTime,
				se.SeatNumber,
	*/

	public partial class BusTicketListTab : ProductListTab
	{

		static BusTicketListTab()
		{
			RegisterList("BusTicket", typeof(BusTicketListTab));
		}

		public BusTicketListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.BusTicket;
		}

		private BusTicketSemantic se;

	}


	public partial class BusTicketEditForm : ProductEditForm
	{

		static BusTicketEditForm()
		{
			RegisterEdit("BusTicket", typeof(BusTicketEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.BusTicket;
		}

		private BusTicketSemantic se;

	}

	#endregion


	#region Excursion

	/// <summary>Экскурсия</summary>
	public partial class ExcursionSemantic : ProductSemantic
	{

		public ExcursionSemantic()
		{
			_name = "Excursion";
			_className = "Excursion";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Экскурсия";
			_titles = "Экскурсии";

			ReissueFor
				.Reference("Excursion");
		}

		/// <summary>Дата начала</summary>
		[PreserveCase]
		public SemanticMember StartDate = Member
			.Title("Дата начала")
			.Date()
			.Required();

		/// <summary>Дата окончания</summary>
		[PreserveCase]
		public SemanticMember FinishDate = Member
			.Title("Дата окончания")
			.Date();

		/// <summary>Название тура</summary>
		[PreserveCase]
		public SemanticMember TourName = Member
			.Title("Название тура")
			.String()
			.Required();

	}

	/*
				se.StartDate,
				se.FinishDate,
				se.TourName,
	*/

	public partial class ExcursionListTab : ProductListTab
	{

		static ExcursionListTab()
		{
			RegisterList("Excursion", typeof(ExcursionListTab));
		}

		public ExcursionListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Excursion;
		}

		private ExcursionSemantic se;

	}


	public partial class ExcursionEditForm : ProductEditForm
	{

		static ExcursionEditForm()
		{
			RegisterEdit("Excursion", typeof(ExcursionEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Excursion;
		}

		private ExcursionSemantic se;

	}

	#endregion


	#region Isic

	/// <summary>Студенческий билет</summary>
	public partial class IsicSemantic : ProductSemantic
	{

		public IsicSemantic()
		{
			_name = "Isic";
			_className = "Isic";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Студенческий билет";
			_titles = "Студенческие билеты";

			ReissueFor
				.Reference("Isic");
		}

		/// <summary>Пассажир</summary>
		[PreserveCase]
		public SemanticMember Passenger = Member
			.Title("Пассажир")
			.Reference("Person");

		/// <summary>Тип карты</summary>
		[PreserveCase]
		public SemanticMember CardType = Member
			.Title("Тип карты")
			.EnumItems(new object[][]
			{
				new object[] { 1, "Isic" }, // Isic
				new object[] { 2, "ITIC" }, // ITIC
				new object[] { 3, "IYTC" }, // IYTC
				new object[] { 0, "Неизвестно" }, // Unknown
			})
			.Required()
			.DefaultValue(1);

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number1 = Member
			.Title("Номер")
			.String(12)
			.Required();

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number2 = Member
			.Title("Номер")
			.String(1)
			.Required();

	}

	/*
				se.Passenger,
				se.CardType,
				se.Number1,
				se.Number2,
	*/

	public partial class IsicListTab : ProductListTab
	{

		static IsicListTab()
		{
			RegisterList("Isic", typeof(IsicListTab));
		}

		public IsicListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Isic;
		}

		private IsicSemantic se;

	}


	public partial class IsicEditForm : ProductEditForm
	{

		static IsicEditForm()
		{
			RegisterEdit("Isic", typeof(IsicEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Isic;
		}

		private IsicSemantic se;

	}

	#endregion


	#region Pasteboard

	/// <summary>Ж/д билет</summary>
	public partial class PasteboardSemantic : ProductSemantic
	{

		public PasteboardSemantic()
		{
			_name = "Pasteboard";
			_className = "Pasteboard";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Ж/д билет";
			_titles = "Ж/д билеты";

			RefundedProduct
				.Reference("Pasteboard");

			ReissueFor
				.Reference("Pasteboard");

			Provider
				.Reference("PasteboardProvider");
		}

		/// <summary>Пассажир</summary>
		[PreserveCase]
		public SemanticMember Passenger = Member
			.Title("Пассажир")
			.Reference("Person");

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String();

		/// <summary>Начальная станция</summary>
		[PreserveCase]
		public SemanticMember DeparturePlace = Member
			.Title("Начальная станция")
			.EmptyText("место")
			.String();

		/// <summary>Дата отправления</summary>
		[PreserveCase]
		public SemanticMember DepartureDate = Member
			.Title("Дата отправления")
			.EmptyText("дата")
			.Date();

		/// <summary>Время отправления</summary>
		[PreserveCase]
		public SemanticMember DepartureTime = Member
			.Title("Время отправления")
			.EmptyText("время")
			.String();

		/// <summary>Конечная станция</summary>
		[PreserveCase]
		public SemanticMember ArrivalPlace = Member
			.Title("Конечная станция")
			.EmptyText("место")
			.String();

		/// <summary>Дата прибытия</summary>
		[PreserveCase]
		public SemanticMember ArrivalDate = Member
			.Title("Дата прибытия")
			.EmptyText("дата")
			.Date();

		/// <summary>Время прибытия</summary>
		[PreserveCase]
		public SemanticMember ArrivalTime = Member
			.Title("Время прибытия")
			.EmptyText("время")
			.String();

		/// <summary>Маршрут</summary>
		[PreserveCase]
		public SemanticMember Itinerary = Member
			.Title("Маршрут")
			.String();

		/// <summary>Номер поезда</summary>
		[PreserveCase]
		public SemanticMember TrainNumber = Member
			.Title("Номер поезда")
			.String();

		/// <summary>Номер вагона</summary>
		[PreserveCase]
		public SemanticMember CarNumber = Member
			.Title("Номер вагона")
			.String();

		/// <summary>Номер места</summary>
		[PreserveCase]
		public SemanticMember SeatNumber = Member
			.Title("Номер места")
			.String();

		/// <summary>Сервис-класс</summary>
		[PreserveCase]
		public SemanticMember ServiceClass = Member
			.Title("Сервис-класс")
			.EnumItems(new object[][]
			{
				new object[] { 4, "купе" }, // Compartment
				new object[] { 0, "1-й класс" }, // FirstClass
				new object[] { 2, "люкс" }, // LuxuryCoupe
				new object[] { 3, "плацкарт" }, // ReservedSeat
				new object[] { 1, "2-й класс" }, // SecondClass
				new object[] { 5, "Неизвестно" }, // Unknown
				new object[] { 6, "Общий" }, // Сommon
			})
			.Required()
			.DefaultValue(0);

	}

	/*
				se.Passenger,
				se.Number,
				se.DeparturePlace,
				se.DepartureDate,
				se.DepartureTime,
				se.ArrivalPlace,
				se.ArrivalDate,
				se.ArrivalTime,
				se.Itinerary,
				se.TrainNumber,
				se.CarNumber,
				se.SeatNumber,
				se.ServiceClass,
	*/

	public partial class PasteboardListTab : ProductListTab
	{

		static PasteboardListTab()
		{
			RegisterList("Pasteboard", typeof(PasteboardListTab));
		}

		public PasteboardListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Pasteboard;
		}

		private PasteboardSemantic se;

	}


	public partial class PasteboardEditForm : ProductEditForm
	{

		static PasteboardEditForm()
		{
			RegisterEdit("Pasteboard", typeof(PasteboardEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Pasteboard;
		}

		private PasteboardSemantic se;

	}

	#endregion


	#region Transfer

	/// <summary>Трансфер</summary>
	public partial class TransferSemantic : ProductSemantic
	{

		public TransferSemantic()
		{
			_name = "Transfer";
			_className = "Transfer";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Трансфер";
			_titles = "Трансферы";

			ReissueFor
				.Reference("Transfer");

			Provider
				.Reference("TransferProvider");
		}

		/// <summary>Дата начала</summary>
		[PreserveCase]
		public SemanticMember StartDate = Member
			.Title("Дата начала")
			.Date()
			.Required();

	}

	/*
				se.StartDate,
	*/

	public partial class TransferListTab : ProductListTab
	{

		static TransferListTab()
		{
			RegisterList("Transfer", typeof(TransferListTab));
		}

		public TransferListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Transfer;
		}

		private TransferSemantic se;

	}


	public partial class TransferEditForm : ProductEditForm
	{

		static TransferEditForm()
		{
			RegisterEdit("Transfer", typeof(TransferEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Transfer;
		}

		private TransferSemantic se;

	}

	#endregion


	#region GenericProduct

	/// <summary>Дополнительная услуга</summary>
	public partial class GenericProductSemantic : ProductSemantic
	{

		public GenericProductSemantic()
		{
			_name = "GenericProduct";
			_className = "GenericProduct";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Дополнительная услуга";
			_titles = "Дополнительные услуги";

			ReissueFor
				.Reference("GenericProduct");

			Provider
				.Reference("GenericProductProvider");
		}

		/// <summary>Вид услуги</summary>
		[PreserveCase]
		public SemanticMember GenericType = Member
			.Title("Вид услуги")
			.Reference("GenericProductType")
			.Required();

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String();

		/// <summary>Дата начала</summary>
		[PreserveCase]
		public SemanticMember StartDate = Member
			.Title("Дата начала")
			.Date();

		/// <summary>Дата окончания</summary>
		[PreserveCase]
		public SemanticMember FinishDate = Member
			.Title("Дата окончания")
			.Date();

	}

	/*
				se.GenericType,
				se.Number,
				se.StartDate,
				se.FinishDate,
	*/

	public partial class GenericProductListTab : ProductListTab
	{

		static GenericProductListTab()
		{
			RegisterList("GenericProduct", typeof(GenericProductListTab));
		}

		public GenericProductListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.GenericProduct;
		}

		private GenericProductSemantic se;

	}


	public partial class GenericProductEditForm : ProductEditForm
	{

		static GenericProductEditForm()
		{
			RegisterEdit("GenericProduct", typeof(GenericProductEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.GenericProduct;
		}

		private GenericProductSemantic se;

	}

	#endregion


	#region CarRental

	/// <summary>Аренда автомобиля</summary>
	public partial class CarRentalSemantic : ProductSemantic
	{

		public CarRentalSemantic()
		{
			_name = "CarRental";
			_className = "CarRental";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Аренда автомобиля";
			_titles = "Аренды автомобилей";

			ReissueFor
				.Reference("CarRental");

			Provider
				.Reference("CarRentalProvider");
		}

		/// <summary>Дата начала</summary>
		[PreserveCase]
		public SemanticMember StartDate = Member
			.Title("Дата начала")
			.Date()
			.Required();

		/// <summary>Дата окончания</summary>
		[PreserveCase]
		public SemanticMember FinishDate = Member
			.Title("Дата окончания")
			.Date();

		/// <summary>Марка авто</summary>
		[PreserveCase]
		public SemanticMember CarBrand = Member
			.Title("Марка авто")
			.String();

	}

	/*
				se.StartDate,
				se.FinishDate,
				se.CarBrand,
	*/

	public partial class CarRentalListTab : ProductListTab
	{

		static CarRentalListTab()
		{
			RegisterList("CarRental", typeof(CarRentalListTab));
		}

		public CarRentalListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.CarRental;
		}

		private CarRentalSemantic se;

	}


	public partial class CarRentalEditForm : ProductEditForm
	{

		static CarRentalEditForm()
		{
			RegisterEdit("CarRental", typeof(CarRentalEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.CarRental;
		}

		private CarRentalSemantic se;

	}

	#endregion


	#region Tour

	/// <summary>Тур (готовый)</summary>
	public partial class TourSemantic : ProductSemantic
	{

		public TourSemantic()
		{
			_name = "Tour";
			_className = "Tour";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Тур (готовый)";
			_titles = "Туры (готовые)";

			ReissueFor
				.Reference("Tour");

			Provider
				.Reference("TourProvider");
		}

		/// <summary>Дата начала</summary>
		[PreserveCase]
		public SemanticMember StartDate = Member
			.Title("Дата начала")
			.Date()
			.Required();

		/// <summary>Дата окончания</summary>
		[PreserveCase]
		public SemanticMember FinishDate = Member
			.Title("Дата окончания")
			.Date();

		/// <summary>Гостиница</summary>
		[PreserveCase]
		public SemanticMember HotelName = Member
			.Title("Гостиница")
			.String();

		/// <summary>Офис гостиницы</summary>
		[PreserveCase]
		public SemanticMember HotelOffice = Member
			.Title("Офис гостиницы")
			.EmptyText("офис")
			.String();

		/// <summary>Код гостиницы</summary>
		[PreserveCase]
		public SemanticMember HotelCode = Member
			.Title("Код гостиницы")
			.EmptyText("код")
			.String();

		/// <summary>Расположение</summary>
		[PreserveCase]
		public SemanticMember PlacementName = Member
			.Title("Расположение")
			.String();

		[PreserveCase]
		public SemanticMember PlacementOffice = Member
			.EmptyText("офис")
			.String();

		[PreserveCase]
		public SemanticMember PlacementCode = Member
			.EmptyText("код")
			.String();

		/// <summary>Тип проживания</summary>
		[PreserveCase]
		public SemanticMember AccommodationType = Member
			.Title("Тип проживания")
			.EnumReference("AccommodationType");

		/// <summary>Тип питания</summary>
		[PreserveCase]
		public SemanticMember CateringType = Member
			.Title("Тип питания")
			.EnumReference("CateringType");

		/// <summary>Авиа (описание)</summary>
		[PreserveCase]
		public SemanticMember AviaDescription = Member
			.Title("Авиа (описание)")
			.String();

		/// <summary>Трансфер (описание)</summary>
		[PreserveCase]
		public SemanticMember TransferDescription = Member
			.Title("Трансфер (описание)")
			.String();

	}

	/*
				se.StartDate,
				se.FinishDate,
				se.HotelName,
				se.HotelOffice,
				se.HotelCode,
				se.PlacementName,
				se.PlacementOffice,
				se.PlacementCode,
				se.AccommodationType,
				se.CateringType,
				se.AviaDescription,
				se.TransferDescription,
	*/

	public partial class TourListTab : ProductListTab
	{

		static TourListTab()
		{
			RegisterList("Tour", typeof(TourListTab));
		}

		public TourListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Tour;
		}

		private TourSemantic se;

	}


	public partial class TourEditForm : ProductEditForm
	{

		static TourEditForm()
		{
			RegisterEdit("Tour", typeof(TourEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Tour;
		}

		private TourSemantic se;

	}

	#endregion


	#region Accommodation

	/// <summary>Проживание</summary>
	public partial class AccommodationSemantic : ProductSemantic
	{

		public AccommodationSemantic()
		{
			_name = "Accommodation";
			_className = "Accommodation";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Проживание";
			_titles = "Проживания";

			ReissueFor
				.Reference("Accommodation");

			Provider
				.Reference("AccommodationProvider");
		}

		/// <summary>Дата начала</summary>
		[PreserveCase]
		public SemanticMember StartDate = Member
			.Title("Дата начала")
			.Date()
			.Required();

		/// <summary>Дата окончания</summary>
		[PreserveCase]
		public SemanticMember FinishDate = Member
			.Title("Дата окончания")
			.Date();

		/// <summary>Гостиница</summary>
		[PreserveCase]
		public SemanticMember HotelName = Member
			.Title("Гостиница")
			.String();

		/// <summary>Офис гостиницы</summary>
		[PreserveCase]
		public SemanticMember HotelOffice = Member
			.Title("Офис гостиницы")
			.EmptyText("офис")
			.String();

		/// <summary>Код гостиницы</summary>
		[PreserveCase]
		public SemanticMember HotelCode = Member
			.Title("Код гостиницы")
			.EmptyText("код")
			.String();

		/// <summary>Расположение</summary>
		[PreserveCase]
		public SemanticMember PlacementName = Member
			.Title("Расположение")
			.String();

		[PreserveCase]
		public SemanticMember PlacementOffice = Member
			.EmptyText("офис")
			.String();

		[PreserveCase]
		public SemanticMember PlacementCode = Member
			.EmptyText("код")
			.String();

		/// <summary>Тип проживания</summary>
		[PreserveCase]
		public SemanticMember AccommodationType = Member
			.Title("Тип проживания")
			.EnumReference("AccommodationType");

		/// <summary>Тип питания</summary>
		[PreserveCase]
		public SemanticMember CateringType = Member
			.Title("Тип питания")
			.EnumReference("CateringType");

	}

	/*
				se.StartDate,
				se.FinishDate,
				se.HotelName,
				se.HotelOffice,
				se.HotelCode,
				se.PlacementName,
				se.PlacementOffice,
				se.PlacementCode,
				se.AccommodationType,
				se.CateringType,
	*/

	public partial class AccommodationListTab : ProductListTab
	{

		static AccommodationListTab()
		{
			RegisterList("Accommodation", typeof(AccommodationListTab));
		}

		public AccommodationListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Accommodation;
		}

		private AccommodationSemantic se;

	}


	public partial class AccommodationEditForm : ProductEditForm
	{

		static AccommodationEditForm()
		{
			RegisterEdit("Accommodation", typeof(AccommodationEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Accommodation;
		}

		private AccommodationSemantic se;

	}

	#endregion


	#region SimCard

	/// <summary>SIM-карта</summary>
	public partial class SimCardSemantic : ProductSemantic
	{

		public SimCardSemantic()
		{
			_name = "SimCard";
			_className = "SimCard";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="SIM-карта";
			_titles = "SIM-карты";

			ReissueFor
				.Reference("SimCard");

			Producer
				.Title("Оператор")
				.Reference("RoamingOperator")
				.Required();
		}

		/// <summary>Пассажир</summary>
		[PreserveCase]
		public SemanticMember Passenger = Member
			.Title("Пассажир")
			.Reference("Person");

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String(16)
			.Required();

		/// <summary>Продажа SIM-карты</summary>
		[PreserveCase]
		public SemanticMember IsSale = Member
			.Title("Продажа SIM-карты")
			.Bool()
			.Required();

	}

	/*
				se.Passenger,
				se.Number,
				se.IsSale,
	*/

	public partial class SimCardListTab : ProductListTab
	{

		static SimCardListTab()
		{
			RegisterList("SimCard", typeof(SimCardListTab));
		}

		public SimCardListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.SimCard;
		}

		private SimCardSemantic se;

	}


	public partial class SimCardEditForm : ProductEditForm
	{

		static SimCardEditForm()
		{
			RegisterEdit("SimCard", typeof(SimCardEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.SimCard;
		}

		private SimCardSemantic se;

	}

	#endregion


	#region Insurance

	/// <summary>Страховка</summary>
	public partial class InsuranceSemantic : ProductSemantic
	{

		public InsuranceSemantic()
		{
			_name = "Insurance";
			_className = "Insurance";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Страховка";
			_titles = "Страховки";

			ReissueFor
				.Reference("Insurance");

			Producer
				.Title("Страховая компания")
				.Reference("InsuranceCompany")
				.Required();
		}

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.Required();

		/// <summary>Дата начала</summary>
		[PreserveCase]
		public SemanticMember StartDate = Member
			.Title("Дата начала")
			.Date()
			.Required();

		/// <summary>Дата окончания</summary>
		[PreserveCase]
		public SemanticMember FinishDate = Member
			.Title("Дата окончания")
			.Date();

	}

	/*
				se.Number,
				se.StartDate,
				se.FinishDate,
	*/

	public partial class InsuranceListTab : ProductListTab
	{

		static InsuranceListTab()
		{
			RegisterList("Insurance", typeof(InsuranceListTab));
		}

		public InsuranceListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Insurance;
		}

		private InsuranceSemantic se;

	}


	public partial class InsuranceEditForm : ProductEditForm
	{

		static InsuranceEditForm()
		{
			RegisterEdit("Insurance", typeof(InsuranceEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Insurance;
		}

		private InsuranceSemantic se;

	}

	#endregion


	#region AviaDocument

	/// <summary>Авиадокумент</summary>
	public partial class AviaDocumentSemantic : ProductSemantic
	{

		public AviaDocumentSemantic()
		{
			_name = "AviaDocument";
			_className = "AviaDocument";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] { dsm.AviaTicket, dsm.AviaMco, dsm.AviaRefund };
			};
			_title ="Авиадокумент";
			_titles = "Авиадокументы";

			ReissueFor
				.Reference("AviaDocument");

			Producer
				.Reference("Airline");

			Provider
				.Title("Поставщик");
		}

		/// <summary>Пассажир</summary>
		[PreserveCase]
		public SemanticMember Passenger = Member
			.Title("Пассажир")
			.Reference("Person");

		[PreserveCase]
		public SemanticMember AirlineIataCode = Member
			.String(2);

		/// <summary>Код АК</summary>
		[PreserveCase]
		public SemanticMember AirlinePrefixCode = Member
			.Title("Код АК")
			.String(3);

		[PreserveCase]
		public SemanticMember AirlineName = Member
			.String();

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String();

		[PreserveCase]
		public SemanticMember ConjunctionNumbers = Member
			.String();

		/// <summary>Паспорт в GDS</summary>
		[PreserveCase]
		public SemanticMember GdsPassportStatus = Member
			.Title("Паспорт в GDS")
			.EnumItems(new object[][]
			{
				new object[] { 1, "Есть" }, // Exist
				new object[] { 3, "Некорректен" }, // Incorrect
				new object[] { 2, "Нет" }, // NotExist
				new object[] { 0, "Неизвестно" }, // Unknown
			})
			.Required()
			.DefaultValue(0);

		[PreserveCase]
		public SemanticMember GdsPassport = Member
			.String();

		/// <summary>Маршрут</summary>
		[PreserveCase]
		public SemanticMember Itinerary = Member
			.Title("Маршрут")
			.String();

		[PreserveCase]
		public SemanticMember PaymentForm = Member
			.String();

		[PreserveCase]
		public SemanticMember PaymentDetails = Member
			.String();

		[PreserveCase]
		public SemanticMember AirlinePnrCode = Member
			.String();

		[PreserveCase]
		public SemanticMember Remarks = Member
			.String();

		[PreserveCase]
		public SemanticMember Fees = Member
			;

		[PreserveCase]
		public SemanticMember Voidings = Member
			;

		[PreserveCase]
		public SemanticMember FullNumber = Member
			.String();

		[PreserveCase]
		public SemanticMember GdsFileIsExported = Member
			.Bool()
			.Required();

	}

	/*
				se.Passenger,
				se.AirlineIataCode,
				se.AirlinePrefixCode,
				se.AirlineName,
				se.Number,
				se.ConjunctionNumbers,
				se.GdsPassportStatus,
				se.GdsPassport,
				se.Itinerary,
				se.PaymentForm,
				se.PaymentDetails,
				se.AirlinePnrCode,
				se.Remarks,
				se.Fees,
				se.Voidings,
				se.FullNumber,
				se.GdsFileIsExported,
	*/


	#endregion


	#region Product

	/// <summary>Услуга</summary>
	public partial class ProductSemantic : Entity2Semantic
	{

		public ProductSemantic()
		{
			_name = "Product";
			_className = "Product";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] { dsm.BusTicket, dsm.Excursion, dsm.Isic, dsm.Pasteboard, dsm.Transfer, dsm.GenericProduct, dsm.CarRental, dsm.Tour, dsm.Accommodation, dsm.SimCard, dsm.Insurance, dsm.AviaDocument, dsm.AviaTicket, dsm.AviaMco, dsm.AviaRefund };
			};
			_title ="Услуга";
			_titles = "Все услуги";
		}

		[PreserveCase]
		public SemanticMember Type = Member
			.EnumItems(new object[][]
			{
				new object[] { 8, "Проживание" }, // Accommodation
				new object[] { 2, "МСО" }, // AviaMco
				new object[] { 1, "Авиадокумент" }, // AviaRefund
				new object[] { 0, "Авиабилет" }, // AviaTicket
				new object[] { 13, "Автобусный билет" }, // BusTicket
				new object[] { 16, "Автобусный билет" }, // BusTicketRefund
				new object[] { 11, "Аренда автомобиля" }, // CarRental
				new object[] { 6, "Экскурсия" }, // Excursion
				new object[] { 12, "Дополнительная услуга" }, // GenericProduct
				new object[] { 10, "Страховка" }, // Insurance
				new object[] { 15, "Страховка" }, // InsuranceRefund
				new object[] { 5, "Студенческий билет" }, // Isic
				new object[] { 3, "Ж/д билет" }, // Pasteboard
				new object[] { 14, "Возврат ж/д билета" }, // PasteboardRefund
				new object[] { 4, "SIM-карта" }, // SimCard
				new object[] { 7, "Тур (готовый)" }, // Tour
				new object[] { 9, "Трансфер" }, // Transfer
			})
			.Required();

		/// <summary>Название</summary>
		[PreserveCase]
		public SemanticMember Name = Member
			.Title("Название")
			.String()
			.EntityName();

		/// <summary>Пассажир</summary>
		[PreserveCase]
		public SemanticMember PassengerName = Member
			.Title("Пассажир")
			.String();

		/// <summary>Дата выпуска</summary>
		[PreserveCase]
		public SemanticMember IssueDate = Member
			.Title("Дата выпуска")
			.Date()
			.Required();

		[PreserveCase]
		public SemanticMember PureNumber = Member
			.String();

		/// <summary>Бронировка</summary>
		[PreserveCase]
		public SemanticMember PnrCode = Member
			.Title("Бронировка")
			.String();

		/// <summary>Туркод</summary>
		[PreserveCase]
		public SemanticMember TourCode = Member
			.Title("Туркод")
			.String();

		/// <summary>Продюсер</summary>
		[PreserveCase]
		public SemanticMember Producer = Member
			.Title("Продюсер")
			.Reference("Organization");

		/// <summary>Провайдер</summary>
		[PreserveCase]
		public SemanticMember Provider = Member
			.Title("Провайдер")
			.Reference("Organization");

		/// <summary>Перевыпуск для</summary>
		[PreserveCase]
		public SemanticMember ReissueFor = Member
			.Title("Перевыпуск для")
			.Reference("Product");

		/// <summary>Исходный документ</summary>
		[PreserveCase]
		public SemanticMember RefundedProduct = Member
			.Title("Исходный документ")
			.Reference("Product");

		[PreserveCase]
		public SemanticMember Passengers = Member
			;

		[PreserveCase]
		public SemanticMember PassengerDtos = Member
			;

		[PreserveCase]
		public SemanticMember IsRefund = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsReservation = Member
			.Bool()
			.Required();

		/// <summary>Обработан</summary>
		[PreserveCase]
		public SemanticMember IsProcessed = Member
			.Title("Обработан")
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember MustBeUnprocessed = Member
			.Bool()
			.Required();

		/// <summary>Аннулирован</summary>
		[PreserveCase]
		public SemanticMember IsVoid = Member
			.Title("Аннулирован")
			.Bool()
			.Required();

		/// <summary>К обработке</summary>
		[PreserveCase]
		public SemanticMember RequiresProcessing = Member
			.Title("К обработке")
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsDelivered = Member
			.Bool()
			.Required();

		/// <summary>Оплачен</summary>
		[PreserveCase]
		public SemanticMember IsPaid = Member
			.Title("Оплачен")
			.Bool()
			.Required();

		/// <summary>Заказчик</summary>
		[PreserveCase]
		public SemanticMember Customer = Member
			.Title("Заказчик")
			.Reference("Customer");

		/// <summary>Заказ</summary>
		[PreserveCase]
		public SemanticMember Order = Member
			.Title("Заказ")
			.Reference("Order");

		/// <summary>Посредник</summary>
		[PreserveCase]
		public SemanticMember Intermediary = Member
			.Title("Посредник")
			.Reference("Party");

		/// <summary>Страна</summary>
		[PreserveCase]
		public SemanticMember Country = Member
			.Title("Страна")
			.Reference("Country")
			.Required();

		/// <summary>Бронировщик</summary>
		[PreserveCase]
		public SemanticMember Booker = Member
			.Title("Бронировщик")
			.Reference("Person");

		/// <summary>Офис бронировщика</summary>
		[PreserveCase]
		public SemanticMember BookerOffice = Member
			.Title("Офис бронировщика")
			.EmptyText("офис")
			.String(20);

		/// <summary>Код бронировщика</summary>
		[PreserveCase]
		public SemanticMember BookerCode = Member
			.Title("Код бронировщика")
			.EmptyText("код")
			.String(20);

		/// <summary>Тикетер</summary>
		[PreserveCase]
		public SemanticMember Ticketer = Member
			.Title("Тикетер")
			.Reference("Person");

		/// <summary>Офис тикетера</summary>
		[PreserveCase]
		public SemanticMember TicketerOffice = Member
			.Title("Офис тикетера")
			.EmptyText("офис")
			.String(20);

		/// <summary>Код тикетера</summary>
		[PreserveCase]
		public SemanticMember TicketerCode = Member
			.Title("Код тикетера")
			.EmptyText("код")
			.String(20);

		/// <summary>IATA офис</summary>
		[PreserveCase]
		public SemanticMember TicketingIataOffice = Member
			.Title("IATA офис")
			.String(10);

		[PreserveCase]
		public SemanticMember IsTicketerRobot = Member
			.Bool()
			.Required();

		/// <summary>Продавец</summary>
		[PreserveCase]
		public SemanticMember Seller = Member
			.Title("Продавец")
			.Reference("Person");

		/// <summary>Владелец</summary>
		[PreserveCase]
		public SemanticMember Owner = Member
			.Title("Владелец")
			.EnumReference("DocumentOwner");

		/// <summary>Юрлицо</summary>
		[PreserveCase]
		public SemanticMember LegalEntity = Member
			.Title("Юрлицо")
			.Reference("Organization");

		/// <summary>Тариф</summary>
		[PreserveCase]
		public SemanticMember Fare = Member
			.Title("Тариф")
			.Money();

		[PreserveCase]
		public SemanticMember Fare_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember Fare_USD = Member
			.Money();

		/// <summary>Экв. тариф</summary>
		[PreserveCase]
		public SemanticMember EqualFare = Member
			.Title("Экв. тариф")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember EqualFare_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember EqualFare_USD = Member
			.Money();

		/// <summary>Таксы</summary>
		[PreserveCase]
		public SemanticMember FeesTotal = Member
			.Title("Таксы")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember FeesTotal_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember FeesTotal_USD = Member
			.Money();

		/// <summary>Комиссия консолидатора</summary>
		[PreserveCase]
		public SemanticMember ConsolidatorCommission = Member
			.Title("Комиссия консолидатора")
			.Money();

		/// <summary>Итого</summary>
		[PreserveCase]
		public SemanticMember Total = Member
			.Title("Итого")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember Total_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember Total_USD = Member
			.Money();

		/// <summary>Сбор системы бронирования</summary>
		[PreserveCase]
		public SemanticMember BookingFee = Member
			.Title("Сбор системы бронирования")
			.Money();

		/// <summary>Штраф за отмену</summary>
		[PreserveCase]
		public SemanticMember CancelFee = Member
			.Title("Штраф за отмену")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember CancelFee_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember CancelFee_USD = Member
			.Money();

		/// <summary>В т.ч. НДС</summary>
		[PreserveCase]
		public SemanticMember Vat = Member
			.Title("В т.ч. НДС")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember Vat_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember Vat_USD = Member
			.Money();

		/// <summary>Комиссия</summary>
		[PreserveCase]
		public SemanticMember Commission = Member
			.Title("Комиссия")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember Commission_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember Commission_USD = Member
			.Money();

		/// <summary>Скидка от комиссии</summary>
		[PreserveCase]
		public SemanticMember CommissionDiscount = Member
			.Title("Скидка от комиссии")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember CommissionDiscount_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember CommissionDiscount_USD = Member
			.Money();

		/// <summary>Сервисный сбор</summary>
		[PreserveCase]
		public SemanticMember ServiceFee = Member
			.Title("Сервисный сбор")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember ServiceFee_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember ServiceFee_USD = Member
			.Money();

		/// <summary>Доп. доход</summary>
		[PreserveCase]
		public SemanticMember Handling = Member
			.Title("Доп. доход")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember Handling_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember Handling_USD = Member
			.Money();

		/// <summary>Доп. расход</summary>
		[PreserveCase]
		public SemanticMember HandlingN = Member
			.Title("Доп. расход")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember HandlingN_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember HandlingN_USD = Member
			.Money();

		/// <summary>Скидка</summary>
		[PreserveCase]
		public SemanticMember Discount = Member
			.Title("Скидка")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember Discount_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember Discount_USD = Member
			.Money();

		/// <summary>Бонусная скидка</summary>
		[PreserveCase]
		public SemanticMember BonusDiscount = Member
			.Title("Бонусная скидка")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember BonusDiscount_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember BonusDiscount_USD = Member
			.Money();

		/// <summary>Бонусное накопление</summary>
		[PreserveCase]
		public SemanticMember BonusAccumulation = Member
			.Title("Бонусное накопление")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember BonusAccumulation_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember BonusAccumulation_USD = Member
			.Money();

		/// <summary>Cбор за возврат</summary>
		[PreserveCase]
		public SemanticMember RefundServiceFee = Member
			.Title("Cбор за возврат")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember RefundServiceFee_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember RefundServiceFee_USD = Member
			.Money();

		/// <summary>Штраф сервисного сбора</summary>
		[PreserveCase]
		public SemanticMember ServiceFeePenalty = Member
			.Title("Штраф сервисного сбора")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember ServiceFeePenalty_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember ServiceFeePenalty_USD = Member
			.Money();

		[PreserveCase]
		public SemanticMember ServiceTotal = Member
			.Money();

		/// <summary>К оплате</summary>
		[PreserveCase]
		public SemanticMember GrandTotal = Member
			.Title("К оплате")
			.DefaultMoney();

		[PreserveCase]
		public SemanticMember GrandTotal_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember GrandTotal_USD = Member
			.Money();

		[PreserveCase]
		public SemanticMember CancelCommissionPercent = Member
			.Float2();

		/// <summary>Комисия за возврат</summary>
		[PreserveCase]
		public SemanticMember CancelCommission = Member
			.Title("Комисия за возврат")
			.Money();

		[PreserveCase]
		public SemanticMember CancelCommission_EUR = Member
			.Money();

		[PreserveCase]
		public SemanticMember CancelCommission_USD = Member
			.Money();

		/// <summary>% комиссии</summary>
		[PreserveCase]
		public SemanticMember CommissionPercent = Member
			.Title("% комиссии")
			.Float2();

		[PreserveCase]
		public SemanticMember TotalToTransfer = Member
			.Money();

		[PreserveCase]
		public SemanticMember Profit = Member
			.Money();

		[PreserveCase]
		public SemanticMember ExtraCharge = Member
			.Money();

		/// <summary>Тип оплаты</summary>
		[PreserveCase]
		public SemanticMember PaymentType = Member
			.Title("Тип оплаты")
			.EnumItems(new object[][]
			{
				new object[] { 1, "Cash" }, // Cash
				new object[] { 3, "Check" }, // Check
				new object[] { 4, "CreditCard" }, // CreditCard
				new object[] { 5, "Exchange" }, // Exchange
				new object[] { 2, "Invoice" }, // Invoice
				new object[] { 0, "Unknown" }, // Unknown
				new object[] { 6, "Без оплаты" }, // WithoutPayment
			})
			.Required();

		/// <summary>Ставка НДС для услуги</summary>
		[PreserveCase]
		public SemanticMember TaxRateOfProduct = Member
			.Title("Ставка НДС для услуги")
			.EnumItems(new object[][]
			{
				new object[] { 1, "А (с НДС)" }, // A
				new object[] { 2, "Б (без НДС)" }, // B
				new object[] { 5, "Д (без НДС)" }, // D
				new object[] { 0, "По умолчанию" }, // Default
				new object[] { -1, "не печатать" }, // None
			})
			.Required();

		/// <summary>Ставка НДС для сбора</summary>
		[PreserveCase]
		public SemanticMember TaxRateOfServiceFee = Member
			.Title("Ставка НДС для сбора")
			.EnumItems(new object[][]
			{
				new object[] { 1, "А (с НДС)" }, // A
				new object[] { 2, "Б (без НДС)" }, // B
				new object[] { 5, "Д (без НДС)" }, // D
				new object[] { 0, "По умолчанию" }, // Default
				new object[] { -1, "не печатать" }, // None
			})
			.Required();

		/// <summary>Примечание</summary>
		[PreserveCase]
		public SemanticMember Note = Member
			.Title("Примечание")
			.Text(3);

		/// <summary>Оригинатор</summary>
		[PreserveCase]
		public SemanticMember Originator = Member
			.Title("Оригинатор")
			.EnumItems(new object[][]
			{
				new object[] { 13, "Air Arabia" }, // AirArabia
				new object[] { 16, "AirLife" }, // AirLife
				new object[] { 4, "Airline" }, // Airline
				new object[] { 1, "Amadeus" }, // Amadeus
				new object[] { 18, "Amadeus Altea" }, // AmadeusAltea
				new object[] { 22, "Crazy Llama" }, // Crazyllama
				new object[] { 10, "Delta TRAVEL" }, // DeltaTravel
				new object[] { 23, "Drct Aero" }, // Drct
				new object[] { 8, "E-Travels" }, // ETravels
				new object[] { 12, "Fly Dubai" }, // FlyDubai
				new object[] { 5, "Gabriel" }, // Gabriel
				new object[] { 2, "Galileo" }, // Galileo
				new object[] { 7, "IATI" }, // IATI
				new object[] { 14, "Pegasus" }, // Pegasus
				new object[] { 17, "Sabre" }, // Sabre
				new object[] { 3, "Sirena" }, // Sirena
				new object[] { 19, "SPRK" }, // SPRK
				new object[] { 9, "Ticket Consolidator" }, // TicketConsolidator
				new object[] { 11, "Tickets.UA" }, // TicketsUA
				new object[] { 20, "Travel & Marketing" }, // TravelAndMarketing
				new object[] { 21, "Travel Point" }, // TravelPoint
				new object[] { 0, "Unknown" }, // Unknown
				new object[] { 15, "ВіаКиїв" }, // ViaKiev
				new object[] { 6, "Wizz Air" }, // WizzAir
			})
			.Required();

		/// <summary>Источник</summary>
		[PreserveCase]
		public SemanticMember Origin = Member
			.Title("Источник")
			.EnumItems(new object[][]
			{
				new object[] { 0, "AmadeusAir" }, // AmadeusAir
				new object[] { 1, "AmadeusPrint" }, // AmadeusPrint
				new object[] { 8, "AmadeusXml" }, // AmadeusXml
				new object[] { 4, "BspLink" }, // BspLink
				new object[] { 14, "Drct" }, // Drct
				new object[] { 12, "GalileoBusXml" }, // GalileoBusXml
				new object[] { 2, "GalileoMir" }, // GalileoMir
				new object[] { 10, "GalileoRailXml" }, // GalileoRailXml
				new object[] { 3, "GalileoTkt" }, // GalileoTkt
				new object[] { 7, "GalileoXml" }, // GalileoXml
				new object[] { 11, "LuxenaXml" }, // LuxenaXml
				new object[] { 5, "Manual" }, // Manual
				new object[] { 9, "SabreFil" }, // SabreFil
				new object[] { 15, "SabreTerminal" }, // SabreTerminal
				new object[] { 6, "SirenaXml" }, // SirenaXml
				new object[] { 16, "SPRK" }, // SPRK
				new object[] { 13, "TravelPointXml" }, // TravelPointXml
			})
			.Required();

		/// <summary>Оригинальный документ</summary>
		[PreserveCase]
		public SemanticMember OriginalDocument = Member
			.Title("Оригинальный документ")
			.Reference("GdsFile");

		[PreserveCase]
		public SemanticMember ProducerOrProviderAirlineIataCode = Member
			.String();

		[PreserveCase]
		public SemanticMember TextForOrderItem = Member
			.String();

		[PreserveCase]
		public SemanticMember IsAviaDocument = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsAviaTicket = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsAviaRefund = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsAviaMco = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsPasteboard = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsPasteboardRefund = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsSimCard = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsIsic = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsExcursion = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsTour = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsAccommodation = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsTransfer = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsInsurance = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsInsuranceRefund = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsCarRental = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsGenericProduct = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsBusTicket = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember IsBusTicketRefund = Member
			.Bool()
			.Required();

	}

	/*
				se.Type,
				se.Name,
				se.PassengerName,
				se.IssueDate,
				se.PureNumber,
				se.PnrCode,
				se.TourCode,
				se.Producer,
				se.Provider,
				se.ReissueFor,
				se.RefundedProduct,
				se.Passengers,
				se.PassengerDtos,
				se.IsRefund,
				se.IsReservation,
				se.IsProcessed,
				se.MustBeUnprocessed,
				se.IsVoid,
				se.RequiresProcessing,
				se.IsDelivered,
				se.IsPaid,
				se.Customer,
				se.Order,
				se.Intermediary,
				se.Country,
				se.Booker,
				se.BookerOffice,
				se.BookerCode,
				se.Ticketer,
				se.TicketerOffice,
				se.TicketerCode,
				se.TicketingIataOffice,
				se.IsTicketerRobot,
				se.Seller,
				se.Owner,
				se.LegalEntity,
				se.Fare,
				se.Fare_EUR,
				se.Fare_USD,
				se.EqualFare,
				se.EqualFare_EUR,
				se.EqualFare_USD,
				se.FeesTotal,
				se.FeesTotal_EUR,
				se.FeesTotal_USD,
				se.ConsolidatorCommission,
				se.Total,
				se.Total_EUR,
				se.Total_USD,
				se.BookingFee,
				se.CancelFee,
				se.CancelFee_EUR,
				se.CancelFee_USD,
				se.Vat,
				se.Vat_EUR,
				se.Vat_USD,
				se.Commission,
				se.Commission_EUR,
				se.Commission_USD,
				se.CommissionDiscount,
				se.CommissionDiscount_EUR,
				se.CommissionDiscount_USD,
				se.ServiceFee,
				se.ServiceFee_EUR,
				se.ServiceFee_USD,
				se.Handling,
				se.Handling_EUR,
				se.Handling_USD,
				se.HandlingN,
				se.HandlingN_EUR,
				se.HandlingN_USD,
				se.Discount,
				se.Discount_EUR,
				se.Discount_USD,
				se.BonusDiscount,
				se.BonusDiscount_EUR,
				se.BonusDiscount_USD,
				se.BonusAccumulation,
				se.BonusAccumulation_EUR,
				se.BonusAccumulation_USD,
				se.RefundServiceFee,
				se.RefundServiceFee_EUR,
				se.RefundServiceFee_USD,
				se.ServiceFeePenalty,
				se.ServiceFeePenalty_EUR,
				se.ServiceFeePenalty_USD,
				se.ServiceTotal,
				se.GrandTotal,
				se.GrandTotal_EUR,
				se.GrandTotal_USD,
				se.CancelCommissionPercent,
				se.CancelCommission,
				se.CancelCommission_EUR,
				se.CancelCommission_USD,
				se.CommissionPercent,
				se.TotalToTransfer,
				se.Profit,
				se.ExtraCharge,
				se.PaymentType,
				se.TaxRateOfProduct,
				se.TaxRateOfServiceFee,
				se.Note,
				se.Originator,
				se.Origin,
				se.OriginalDocument,
				se.ProducerOrProviderAirlineIataCode,
				se.TextForOrderItem,
				se.IsAviaDocument,
				se.IsAviaTicket,
				se.IsAviaRefund,
				se.IsAviaMco,
				se.IsPasteboard,
				se.IsPasteboardRefund,
				se.IsSimCard,
				se.IsIsic,
				se.IsExcursion,
				se.IsTour,
				se.IsAccommodation,
				se.IsTransfer,
				se.IsInsurance,
				se.IsInsuranceRefund,
				se.IsCarRental,
				se.IsGenericProduct,
				se.IsBusTicket,
				se.IsBusTicketRefund,
	*/


	#endregion


	#region AviaTicket

	/// <summary>Авиабилет</summary>
	public partial class AviaTicketSemantic : AviaDocumentSemantic
	{

		public AviaTicketSemantic()
		{
			_name = "AviaTicket";
			_className = "AviaTicket";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Авиабилет";
			_titles = "Авиабилеты";
		}

		/// <summary>Дата отправления</summary>
		[PreserveCase]
		public SemanticMember Departure = Member
			.Title("Дата отправления")
			.EmptyText("дата")
			.Date();

		/// <summary>Дата прибытия</summary>
		[PreserveCase]
		public SemanticMember Arrival = Member
			.Title("Дата прибытия")
			.EmptyText("дата")
			.Date();

		[PreserveCase]
		public SemanticMember LastDeparture = Member
			.Date();

		[PreserveCase]
		public SemanticMember FareBasises = Member
			.String();

		[PreserveCase]
		public SemanticMember Domestic = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember Interline = Member
			.Bool()
			.Required();

		/// <summary>Классы сегментов</summary>
		[PreserveCase]
		public SemanticMember SegmentClasses = Member
			.Title("Классы сегментов")
			.String();

		[PreserveCase]
		public SemanticMember Endorsement = Member
			.String();

		[PreserveCase]
		public SemanticMember IsManual = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember Segments = Member
			;

		[PreserveCase]
		public SemanticMember FareTotal = Member
			.Money();

		[PreserveCase]
		public SemanticMember PenalizeOperations = Member
			;

	}

	/*
				se.Departure,
				se.Arrival,
				se.LastDeparture,
				se.FareBasises,
				se.Domestic,
				se.Interline,
				se.SegmentClasses,
				se.Endorsement,
				se.IsManual,
				se.Segments,
				se.FareTotal,
				se.PenalizeOperations,
	*/

	public partial class AviaTicketListTab : AviaDocumentListTab
	{

		static AviaTicketListTab()
		{
			RegisterList("AviaTicket", typeof(AviaTicketListTab));
		}

		public AviaTicketListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AviaTicket;
		}

		private AviaTicketSemantic se;

	}


	public partial class AviaTicketEditForm : AviaDocumentEditForm
	{

		static AviaTicketEditForm()
		{
			RegisterEdit("AviaTicket", typeof(AviaTicketEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AviaTicket;
		}

		private AviaTicketSemantic se;

	}

	#endregion


	#region AviaMco

	/// <summary>МСО</summary>
	public partial class AviaMcoSemantic : AviaDocumentSemantic
	{

		public AviaMcoSemantic()
		{
			_name = "AviaMco";
			_className = "AviaMco";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="МСО";
			_titles = "МСО";
		}

		/// <summary>Описание</summary>
		[PreserveCase]
		public SemanticMember Description = Member
			.Title("Описание")
			.Text(3);

		/// <summary>Связан с</summary>
		[PreserveCase]
		public SemanticMember InConnectionWith = Member
			.Title("Связан с")
			.Reference("AviaDocument");

	}

	/*
				se.Description,
				se.InConnectionWith,
	*/

	public partial class AviaMcoListTab : AviaDocumentListTab
	{

		static AviaMcoListTab()
		{
			RegisterList("AviaMco", typeof(AviaMcoListTab));
		}

		public AviaMcoListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AviaMco;
		}

		private AviaMcoSemantic se;

	}


	public partial class AviaMcoEditForm : AviaDocumentEditForm
	{

		static AviaMcoEditForm()
		{
			RegisterEdit("AviaMco", typeof(AviaMcoEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AviaMco;
		}

		private AviaMcoSemantic se;

	}

	#endregion


	#region AviaRefund

	/// <summary>Авиадокумент</summary>
	public partial class AviaRefundSemantic : AviaDocumentSemantic
	{

		public AviaRefundSemantic()
		{
			_name = "AviaRefund";
			_className = "AviaRefund";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Авиадокумент";
			_titles = "Авиадокументы";
		}

		/// <summary>Исходный документ</summary>
		[PreserveCase]
		public SemanticMember RefundedDocument = Member
			.Title("Исходный документ")
			.Reference("AviaDocument");

	}

	/*
				se.RefundedDocument,
	*/

	public partial class AviaRefundListTab : AviaDocumentListTab
	{

		static AviaRefundListTab()
		{
			RegisterList("AviaRefund", typeof(AviaRefundListTab));
		}

		public AviaRefundListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AviaRefund;
		}

		private AviaRefundSemantic se;

	}


	public partial class AviaRefundEditForm : AviaDocumentEditForm
	{

		static AviaRefundEditForm()
		{
			RegisterEdit("AviaRefund", typeof(AviaRefundEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AviaRefund;
		}

		private AviaRefundSemantic se;

	}

	#endregion


	#region Country

	/// <summary>Страна</summary>
	public partial class CountrySemantic : Entity3Semantic
	{

		public CountrySemantic()
		{
			_name = "Country";
			_className = "Country";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Страна";
			_titles = "Страны";
		}

		/// <summary>Код (2-х сим.)</summary>
		[PreserveCase]
		public SemanticMember TwoCharCode = Member
			.Title("Код (2-х сим.)")
			.String();

		/// <summary>Код (3-х сим.)</summary>
		[PreserveCase]
		public SemanticMember ThreeCharCode = Member
			.Title("Код (3-х сим.)")
			.String();

		/// <summary>Примечание</summary>
		[PreserveCase]
		public SemanticMember Note = Member
			.Title("Примечание")
			.Text(10);

	}

	/*
				se.TwoCharCode,
				se.ThreeCharCode,
				se.Note,
	*/

	public partial class CountryListTab : Entity3ListTab
	{

		static CountryListTab()
		{
			RegisterList("Country", typeof(CountryListTab));
		}

		public CountryListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Country;
		}

		private CountrySemantic se;

	}


	public partial class CountryEditForm : Entity3EditForm
	{

		static CountryEditForm()
		{
			RegisterEdit("Country", typeof(CountryEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Country;
		}

		private CountrySemantic se;

	}

	#endregion


	#region AirlineServiceClass

	/// <summary>Сервис-класс авиакомпании</summary>
	public partial class AirlineServiceClassSemantic : Entity2Semantic
	{

		public AirlineServiceClassSemantic()
		{
			_name = "AirlineServiceClass";
			_className = "AirlineServiceClass";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Code";
			_title ="Сервис-класс авиакомпании";
			_titles = "Сервис-классы авиакомпаний";
		}

		/// <summary>Авиакомпания</summary>
		[PreserveCase]
		public SemanticMember Airline = Member
			.Title("Авиакомпания")
			.EmptyText("авиакомпания")
			.Reference("Organization");

		/// <summary>Код</summary>
		[PreserveCase]
		public SemanticMember Code = Member
			.Title("Код")
			.String()
			.EntityName();

		/// <summary>Сервис-класс</summary>
		[PreserveCase]
		public SemanticMember ServiceClass = Member
			.Title("Сервис-класс")
			.EnumItems(new object[][]
			{
				new object[] { 3, "Бизнес" }, // Business
				new object[] { 1, "Эконом" }, // Economy
				new object[] { 4, "Первый" }, // First
				new object[] { 2, "Эконом-премиум" }, // PremiumEconomy
				new object[] { 0, "Неизвестно" }, // Unknown
			})
			.Required();

	}

	/*
				se.Airline,
				se.Code,
				se.ServiceClass,
	*/

	public partial class AirlineServiceClassListTab : EntityListTab
	{

		static AirlineServiceClassListTab()
		{
			RegisterList("AirlineServiceClass", typeof(AirlineServiceClassListTab));
		}

		public AirlineServiceClassListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AirlineServiceClass;
		}

		private AirlineServiceClassSemantic se;

	}


	public partial class AirlineServiceClassEditForm : EntityEditForm
	{

		static AirlineServiceClassEditForm()
		{
			RegisterEdit("AirlineServiceClass", typeof(AirlineServiceClassEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AirlineServiceClass;
		}

		private AirlineServiceClassSemantic se;

	}

	#endregion


	#region AirlineMonthCommission

	/// <summary>Доп.комиссия от авиакомпании</summary>
	public partial class AirlineMonthCommissionSemantic : Entity2Semantic
	{

		public AirlineMonthCommissionSemantic()
		{
			_name = "AirlineMonthCommission";
			_className = "AirlineMonthCommission";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "DateFrom";
			_title ="Доп.комиссия от авиакомпании";
			_titles = "Доп.комиссии от авиакомпаний";
		}

		/// <summary>Авиакомпания</summary>
		[PreserveCase]
		public SemanticMember Airline = Member
			.Title("Авиакомпания")
			.EmptyText("авиакомпания")
			.Reference("Organization")
			.Required();

		/// <summary>Дата начала</summary>
		[PreserveCase]
		public SemanticMember DateFrom = Member
			.Title("Дата начала")
			.Date()
			.Required()
			.EntityName();

		/// <summary>Дата окончания</summary>
		[PreserveCase]
		public SemanticMember DateTo = Member
			.Title("Дата окончания")
			.Date();

		/// <summary>Доп.комиссия, %</summary>
		[PreserveCase]
		public SemanticMember CommissionPc = Member
			.Title("Доп.комиссия, %")
			.Float2();

	}

	/*
				se.Airline,
				se.DateFrom,
				se.DateTo,
				se.CommissionPc,
	*/

	public partial class AirlineMonthCommissionListTab : EntityListTab
	{

		static AirlineMonthCommissionListTab()
		{
			RegisterList("AirlineMonthCommission", typeof(AirlineMonthCommissionListTab));
		}

		public AirlineMonthCommissionListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AirlineMonthCommission;
		}

		private AirlineMonthCommissionSemantic se;

	}


	public partial class AirlineMonthCommissionEditForm : EntityEditForm
	{

		static AirlineMonthCommissionEditForm()
		{
			RegisterEdit("AirlineMonthCommission", typeof(AirlineMonthCommissionEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AirlineMonthCommission;
		}

		private AirlineMonthCommissionSemantic se;

	}

	#endregion


	#region Airport

	/// <summary>Аэропорт</summary>
	public partial class AirportSemantic : Entity3Semantic
	{

		public AirportSemantic()
		{
			_name = "Airport";
			_className = "Airport";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Code";
			_title ="Аэропорт";
			_titles = "Аэропорты";
		}

		/// <summary>Код</summary>
		[PreserveCase]
		public SemanticMember Code = Member
			.Title("Код")
			.String();

		/// <summary>Страна</summary>
		[PreserveCase]
		public SemanticMember Country = Member
			.Title("Страна")
			.Reference("Country");

		/// <summary>Населенный пункт (англ.)</summary>
		[PreserveCase]
		public SemanticMember Settlement = Member
			.Title("Населенный пункт (англ.)")
			.String();

		/// <summary>Населенный пункт</summary>
		[PreserveCase]
		public SemanticMember LocalizedSettlement = Member
			.Title("Населенный пункт")
			.String();

		/// <summary>Широта</summary>
		[PreserveCase]
		public SemanticMember Latitude = Member
			.Title("Широта")
			.Float2();

		/// <summary>Долгота</summary>
		[PreserveCase]
		public SemanticMember Longitude = Member
			.Title("Долгота")
			.Float2();

	}

	/*
				se.Code,
				se.Country,
				se.Settlement,
				se.LocalizedSettlement,
				se.Latitude,
				se.Longitude,
	*/

	public partial class AirportListTab : Entity3ListTab
	{

		static AirportListTab()
		{
			RegisterList("Airport", typeof(AirportListTab));
		}

		public AirportListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Airport;
		}

		private AirportSemantic se;

	}


	public partial class AirportEditForm : Entity3EditForm
	{

		static AirportEditForm()
		{
			RegisterEdit("Airport", typeof(AirportEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Airport;
		}

		private AirportSemantic se;

	}

	#endregion


	#region PaymentSystem

	/// <summary>Платёжная система</summary>
	public partial class PaymentSystemSemantic : Entity3Semantic
	{

		public PaymentSystemSemantic()
		{
			_name = "PaymentSystem";
			_className = "PaymentSystem";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Платёжная система";
			_titles = "Платёжные системы";
		}

	}

	/*
	*/

	public partial class PaymentSystemListTab : Entity3ListTab
	{

		static PaymentSystemListTab()
		{
			RegisterList("PaymentSystem", typeof(PaymentSystemListTab));
		}

		public PaymentSystemListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.PaymentSystem;
		}

		private PaymentSystemSemantic se;

	}


	public partial class PaymentSystemEditForm : Entity3EditForm
	{

		static PaymentSystemEditForm()
		{
			RegisterEdit("PaymentSystem", typeof(PaymentSystemEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.PaymentSystem;
		}

		private PaymentSystemSemantic se;

	}

	#endregion


	#region Currency

	/// <summary>Валюта</summary>
	public partial class CurrencySemantic : Entity3Semantic
	{

		public CurrencySemantic()
		{
			_name = "Currency";
			_className = "Currency";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Code";
			_title ="Валюта";
			_titles = "Валюты";
		}

		/// <summary>Код</summary>
		[PreserveCase]
		public SemanticMember Code = Member
			.Title("Код")
			.String()
			.EntityName();

		/// <summary>Числовой код</summary>
		[PreserveCase]
		public SemanticMember NumericCode = Member
			.Title("Числовой код")
			.Int32()
			.Required();

		/// <summary>Кириллический код</summary>
		[PreserveCase]
		public SemanticMember CyrillicCode = Member
			.Title("Кириллический код")
			.String();

	}

	/*
				se.Code,
				se.NumericCode,
				se.CyrillicCode,
	*/

	public partial class CurrencyListTab : Entity3ListTab
	{

		static CurrencyListTab()
		{
			RegisterList("Currency", typeof(CurrencyListTab));
		}

		public CurrencyListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Currency;
		}

		private CurrencySemantic se;

	}


	public partial class CurrencyEditForm : Entity3EditForm
	{

		static CurrencyEditForm()
		{
			RegisterEdit("Currency", typeof(CurrencyEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Currency;
		}

		private CurrencySemantic se;

	}

	#endregion


	#region Identity

	public partial class IdentitySemantic : Entity3DSemantic
	{

		public IdentitySemantic()
		{
			_name = "Identity";
			_className = "Identity";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] { dsm.User };
			};
		}

		[PreserveCase]
		public SemanticMember Preferences = Member
			;

	}

	/*
				se.Preferences,
	*/


	#endregion


	#region User

	/// <summary>Пользователь</summary>
	public partial class UserSemantic : IdentitySemantic
	{

		public UserSemantic()
		{
			_name = "User";
			_className = "User";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Пользователь";
			_titles = "Пользователи";

			Name
				.Title("Логин");
		}

		/// <summary>Персона</summary>
		[PreserveCase]
		public SemanticMember Person = Member
			.Title("Персона")
			.Reference("Person");

		/// <summary>Пароль</summary>
		[PreserveCase]
		public SemanticMember Password = Member
			.Title("Пароль")
			.Password();

		/// <summary>Подтверждение пароля</summary>
		[PreserveCase]
		public SemanticMember ConfirmPassword = Member
			.Title("Подтверждение пароля")
			.ConfirmPassword("Password");

		/// <summary>Активный</summary>
		[PreserveCase]
		public SemanticMember Active = Member
			.Title("Активный")
			.Bool()
			.Required()
			.DefaultValue(true);

		/// <summary>Администратор</summary>
		[PreserveCase]
		public SemanticMember IsAdministrator = Member
			.Title("Администратор")
			.Bool()
			.Required()
			.Secondary();

		/// <summary>Супервизор</summary>
		[PreserveCase]
		public SemanticMember IsSupervisor = Member
			.Title("Супервизор")
			.Bool()
			.Required()
			.Secondary();

		/// <summary>Агент</summary>
		[PreserveCase]
		public SemanticMember IsAgent = Member
			.Title("Агент")
			.Bool()
			.Required()
			.Secondary();

		/// <summary>Кассир</summary>
		[PreserveCase]
		public SemanticMember IsCashier = Member
			.Title("Кассир")
			.Bool()
			.Required()
			.Secondary();

		/// <summary>Аналитик</summary>
		[PreserveCase]
		public SemanticMember IsAnalyst = Member
			.Title("Аналитик")
			.Bool()
			.Required()
			.Secondary();

		/// <summary>Субагент</summary>
		[PreserveCase]
		public SemanticMember IsSubAgent = Member
			.Title("Субагент")
			.Bool()
			.Required()
			.Secondary();

		/// <summary>Разрешить 'Отчёт по клиенту'</summary>
		[PreserveCase]
		public SemanticMember AllowCustomerReport = Member
			.Title("Разрешить 'Отчёт по клиенту'")
			.Bool()
			.Required()
			.Secondary();

		/// <summary>Разрешить 'Отчёт с реестром'</summary>
		[PreserveCase]
		public SemanticMember AllowRegistryReport = Member
			.Title("Разрешить 'Отчёт с реестром'")
			.Bool()
			.Required()
			.Secondary();

		/// <summary>Разрешить 'Задолженность по взаиморасчетам'</summary>
		[PreserveCase]
		public SemanticMember AllowUnbalancedReport = Member
			.Title("Разрешить 'Задолженность по взаиморасчетам'")
			.Bool()
			.Required()
			.Secondary();

		[PreserveCase]
		public SemanticMember SessionId = Member
			.String();

		/// <summary>Роли</summary>
		[PreserveCase]
		public SemanticMember Roles = Member
			.Title("Роли")
			.String();

	}

	/*
				se.Person,
				se.Password,
				se.ConfirmPassword,
				se.Active,
				se.IsAdministrator,
				se.IsSupervisor,
				se.IsAgent,
				se.IsCashier,
				se.IsAnalyst,
				se.IsSubAgent,
				se.AllowCustomerReport,
				se.AllowRegistryReport,
				se.AllowUnbalancedReport,
				se.SessionId,
				se.Roles,
	*/

	public partial class UserListTab : IdentityListTab
	{

		static UserListTab()
		{
			RegisterList("User", typeof(UserListTab));
		}

		public UserListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.User;
		}

		private UserSemantic se;

	}


	public partial class UserEditForm : IdentityEditForm
	{

		static UserEditForm()
		{
			RegisterEdit("User", typeof(UserEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.User;
		}

		private UserSemantic se;

	}

	#endregion


	#region UserVisit

	/// <summary>Заход пользователя</summary>
	public partial class UserVisitSemantic : EntitySemantic
	{

		public UserVisitSemantic()
		{
			_name = "UserVisit";
			_className = "UserVisit";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = null;
			_title ="Заход пользователя";
			_titles = "Заходы пользователей";
		}

		/// <summary>Пользователь</summary>
		[PreserveCase]
		public SemanticMember User = Member
			.Title("Пользователь")
			.Reference("User")
			.Required();

		/// <summary>Время захода</summary>
		[PreserveCase]
		public SemanticMember StartDate = Member
			.Title("Время захода")
			.DateTime2()
			.Required()
			.EntityDate()
			.Utility();

		/// <summary>IP</summary>
		[PreserveCase]
		public SemanticMember IP = Member
			.Title("IP")
			.String()
			.Required();

		/// <summary>SessionId</summary>
		[PreserveCase]
		public SemanticMember SessionId = Member
			.Title("SessionId")
			.String()
			.Required();

		/// <summary>Подробнее о запросе</summary>
		[PreserveCase]
		public SemanticMember Request = Member
			.Title("Подробнее о запросе")
			.Text(10);

	}

	/*
				se.User,
				se.StartDate,
				se.IP,
				se.SessionId,
				se.Request,
	*/

	public partial class UserVisitListTab : EntityListTab
	{

		static UserVisitListTab()
		{
			RegisterList("UserVisit", typeof(UserVisitListTab));
		}

		public UserVisitListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.UserVisit;
		}

		private UserVisitSemantic se;

	}


	public partial class UserVisitEditForm : EntityEditForm
	{

		static UserVisitEditForm()
		{
			RegisterEdit("UserVisit", typeof(UserVisitEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.UserVisit;
		}

		private UserVisitSemantic se;

	}

	#endregion


	#region SystemConfiguration

	/// <summary>Настройки системы</summary>
	public partial class SystemConfigurationSemantic : EntitySemantic
	{

		public SystemConfigurationSemantic()
		{
			_name = "SystemConfiguration";
			_className = "SystemConfiguration";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = null;
			_title ="Настройки системы";
		}

		[PreserveCase]
		public SemanticMember ModifiedOn = Member
			.Date();

		[PreserveCase]
		public SemanticMember ModifiedBy = Member
			.String();

		/// <summary>Организация</summary>
		[PreserveCase]
		public SemanticMember Company = Member
			.Title("Организация")
			.Reference("Organization");

		[PreserveCase]
		public SemanticMember CompanyName = Member
			.String();

		/// <summary>Реквизиты организации</summary>
		[PreserveCase]
		public SemanticMember CompanyDetails = Member
			.Title("Реквизиты организации")
			.Text(3);

		/// <summary>Страна</summary>
		[PreserveCase]
		public SemanticMember Country = Member
			.Title("Страна")
			.Reference("Country");

		/// <summary>Валюта по умолчанию</summary>
		[PreserveCase]
		public SemanticMember DefaultCurrency = Member
			.Title("Валюта по умолчанию")
			.Reference("Currency");

		/// <summary>Использовать только валюту по умолчанию</summary>
		[PreserveCase]
		public SemanticMember UseDefaultCurrencyForInput = Member
			.Title("Использовать только валюту по умолчанию")
			.Bool()
			.Required();

		/// <summary>Ставка НДС,%</summary>
		[PreserveCase]
		public SemanticMember VatRate = Member
			.Title("Ставка НДС,%")
			.Float2()
			.Required();

		/// <summary>Использование riz-данных в Amadeus Air</summary>
		[PreserveCase]
		public SemanticMember AmadeusRizUsingMode = Member
			.Title("Использование riz-данных в Amadeus Air")
			.EnumItems(new object[][]
			{
				new object[] { 2, "Использовать полностью" }, // All
				new object[] { 0, "Не использовать" }, // None
				new object[] { 1, "Использовать сервисный сбор" }, // ServiceFeeOnly
			})
			.Required();

		/// <summary>Требование паспортных данных авиакомпаниями</summary>
		[PreserveCase]
		public SemanticMember IsPassengerPassportRequired = Member
			.Title("Требование паспортных данных авиакомпаниями")
			.Bool()
			.Required();

		/// <summary>Формирование номенклатур заказа по авиадокументу</summary>
		[PreserveCase]
		public SemanticMember AviaOrderItemGenerationOption = Member
			.Title("Формирование номенклатур заказа по авиадокументу")
			.EnumItems(new object[][]
			{
				new object[] { 0, "Всегда одна позиция" }, // AlwaysOneOrderItem
				new object[] { 2, "Настраивать вручную" }, // ManualSetting
				new object[] { 1, "Cервисный сбор отдельной позицией" }, // SeparateServiceFee
			})
			.Required();

		/// <summary>Разрешить редактирование НДС в заказе</summary>
		[PreserveCase]
		public SemanticMember AllowAgentSetOrderVat = Member
			.Title("Разрешить редактирование НДС в заказе")
			.Bool()
			.Required();

		/// <summary>Использовать НДС авиадокумента в заказе</summary>
		[PreserveCase]
		public SemanticMember UseAviaDocumentVatInOrder = Member
			.Title("Использовать НДС авиадокумента в заказе")
			.Bool()
			.Required();

		/// <summary>Расчет НДС в авиадокументе</summary>
		[PreserveCase]
		public SemanticMember AviaDocumentVatOptions = Member
			.Title("Расчет НДС в авиадокументе")
			.EnumItems(new object[][]
			{
				new object[] { 1, "Рассчитывать от итога" }, // TaxAirlineTotal
				new object[] { 0, "Использовать HF таксу" }, // UseHFTax
			})
			.Required();

		/// <summary>Текст поля "Главный бухгалтер"</summary>
		[PreserveCase]
		public SemanticMember AccountantDisplayString = Member
			.Title("Текст поля \"Главный бухгалтер\"")
			.Text(3);

		/// <summary>Корреспондентский счет для ПКО</summary>
		[PreserveCase]
		public SemanticMember IncomingCashOrderCorrespondentAccount = Member
			.Title("Корреспондентский счет для ПКО")
			.String();

		/// <summary>Разделять доступ к документам по владельцу</summary>
		[PreserveCase]
		public SemanticMember SeparateDocumentAccess = Member
			.Title("Разделять доступ к документам по владельцу")
			.Bool()
			.Required();

		/// <summary>Разделять доступ к документам по агенту</summary>
		[PreserveCase]
		public SemanticMember SeparateDocumentAccessByAgent = Member
			.Title("Разделять доступ к документам по агенту")
			.Bool()
			.Required();

		/// <summary>Возможность редактировать и обрабатывать билеты других агентов</summary>
		[PreserveCase]
		public SemanticMember AllowOtherAgentsToModifyProduct = Member
			.Title("Возможность редактировать и обрабатывать билеты других агентов")
			.Bool()
			.Required();

		/// <summary>Обязательное ЕДРПОУ для организаций</summary>
		[PreserveCase]
		public SemanticMember IsOrganizationCodeRequired = Member
			.Title("Обязательное ЕДРПОУ для организаций")
			.Bool()
			.Required();

		/// <summary>Использовать комиссию консолидатора</summary>
		[PreserveCase]
		public SemanticMember UseConsolidatorCommission = Member
			.Title("Использовать комиссию консолидатора")
			.Bool()
			.Required();

		/// <summary>Комиссия консолидатора по умолчанию</summary>
		[PreserveCase]
		public SemanticMember DefaultConsolidatorCommission = Member
			.Title("Комиссия консолидатора по умолчанию")
			.Money();

		/// <summary>Использовать доп. доход от АК при обработке авиадокументов</summary>
		[PreserveCase]
		public SemanticMember UseAviaHandling = Member
			.Title("Использовать доп. доход от АК при обработке авиадокументов")
			.Bool()
			.Required();

		/// <summary>Использовать бонусы</summary>
		[PreserveCase]
		public SemanticMember UseBonuses = Member
			.Title("Использовать бонусы")
			.Bool()
			.Required();

		/// <summary>Дней до отправления</summary>
		[PreserveCase]
		public SemanticMember DaysBeforeDeparture = Member
			.Title("Дней до отправления")
			.Int32()
			.Required();

		/// <summary>Ответственный за поздравление именинников</summary>
		[PreserveCase]
		public SemanticMember BirthdayTaskResponsible = Member
			.Title("Ответственный за поздравление именинников")
			.Reference("Person");

		/// <summary>Заказ обязателен для обработки документов</summary>
		[PreserveCase]
		public SemanticMember IsOrderRequiredForProcessedDocument = Member
			.Title("Заказ обязателен для обработки документов")
			.Bool()
			.Required();

		/// <summary>Показывать статистику на главной страницы с</summary>
		[PreserveCase]
		public SemanticMember MetricsFromDate = Member
			.Title("Показывать статистику на главной страницы с")
			.Date();

		/// <summary>Бронировки в документах по офису</summary>
		[PreserveCase]
		public SemanticMember ReservationsInOfficeMetrics = Member
			.Title("Бронировки в документах по офису")
			.Bool()
			.Required();

		/// <summary>Описание обязательно для MCO</summary>
		[PreserveCase]
		public SemanticMember McoRequiresDescription = Member
			.Title("Описание обязательно для MCO")
			.Bool()
			.Required();

		/// <summary>Neutral Airline Code</summary>
		[PreserveCase]
		public SemanticMember NeutralAirlineCode = Member
			.Title("Neutral Airline Code")
			.String();

		/// <summary>Заказы: НДС только от сервисного сбора</summary>
		[PreserveCase]
		public SemanticMember Order_UseServiceFeeOnlyInVat = Member
			.Title("Заказы: НДС только от сервисного сбора")
			.Bool()
			.Required();

		/// <summary>Накладные: новый номер</summary>
		[PreserveCase]
		public SemanticMember Consignment_NumberMode = Member
			.Title("Накладные: новый номер")
			.EnumItems(new object[][]
			{
				new object[] { 1, "На основе номера заказа" }, // ByOrderNumber
				new object[] { 0, "По умолчанию" }, // Default
			})
			.Required();

		/// <summary>Инвойсы: новый номер</summary>
		[PreserveCase]
		public SemanticMember Invoice_NumberMode = Member
			.Title("Инвойсы: новый номер")
			.EnumItems(new object[][]
			{
				new object[] { 1, "На основе номера заказа" }, // ByOrderNumber
				new object[] { 0, "По умолчанию" }, // Default
			})
			.Required();

		/// <summary>Инвойсы: возможность выбрать Владельца при формировании счета</summary>
		[PreserveCase]
		public SemanticMember Invoice_CanOwnerSelect = Member
			.Title("Инвойсы: возможность выбрать Владельца при формировании счета")
			.Bool()
			.Required();

		/// <summary>Инвойсы: печатать НДС</summary>
		[PreserveCase]
		public SemanticMember InvoicePrinter_ShowVat = Member
			.Title("Инвойсы: печатать НДС")
			.Bool()
			.Required();

		/// <summary>Инвойсы: выпущен агентом (по умолчанию)</summary>
		[PreserveCase]
		public SemanticMember Invoice_DefaultIssuedBy = Member
			.Title("Инвойсы: выпущен агентом (по умолчанию)")
			.Reference("Person");

		/// <summary>Инвойсы: важное примечание</summary>
		[PreserveCase]
		public SemanticMember InvoicePrinter_FooterDetails = Member
			.Title("Инвойсы: важное примечание")
			.Text(6);

		/// <summary>DrctWebService_LoadedOn</summary>
		[PreserveCase]
		public SemanticMember DrctWebService_LoadedOn = Member
			.Title("DrctWebService_LoadedOn")
			.DateTime2();

		/// <summary>GalileoWebService_LoadedOn</summary>
		[PreserveCase]
		public SemanticMember GalileoWebService_LoadedOn = Member
			.Title("GalileoWebService_LoadedOn")
			.DateTime2();

		/// <summary>GalileoRailWebService_LoadedOn</summary>
		[PreserveCase]
		public SemanticMember GalileoRailWebService_LoadedOn = Member
			.Title("GalileoRailWebService_LoadedOn")
			.DateTime2();

		/// <summary>GalileoBusWebService_LoadedOn</summary>
		[PreserveCase]
		public SemanticMember GalileoBusWebService_LoadedOn = Member
			.Title("GalileoBusWebService_LoadedOn")
			.DateTime2();

		/// <summary>TravelPointWebService_LoadedOn</summary>
		[PreserveCase]
		public SemanticMember TravelPointWebService_LoadedOn = Member
			.Title("TravelPointWebService_LoadedOn")
			.DateTime2();

		/// <summary>Показывать в накладных строку для сбора системы бронирования</summary>
		[PreserveCase]
		public SemanticMember Consignment_SeparateBookingFee = Member
			.Title("Показывать в накладных строку для сбора системы бронирования")
			.Bool()
			.Required();

		/// <summary>Тип оплаты для ж/д билетов по умолчанию</summary>
		[PreserveCase]
		public SemanticMember Pasterboard_DefaultPaymentType = Member
			.Title("Тип оплаты для ж/д билетов по умолчанию")
			.EnumItems(new object[][]
			{
				new object[] { 1, "Cash" }, // Cash
				new object[] { 3, "Check" }, // Check
				new object[] { 4, "CreditCard" }, // CreditCard
				new object[] { 5, "Exchange" }, // Exchange
				new object[] { 2, "Invoice" }, // Invoice
				new object[] { 0, "Unknown" }, // Unknown
				new object[] { 6, "Без оплаты" }, // WithoutPayment
			});

		/// <summary>Не предлагать распечатать все билеты из бронировки</summary>
		[PreserveCase]
		public SemanticMember Ticket_NoPrintReservations = Member
			.Title("Не предлагать распечатать все билеты из бронировки")
			.Bool()
			.Required();

	}

	/*
				se.ModifiedOn,
				se.ModifiedBy,
				se.Company,
				se.CompanyName,
				se.CompanyDetails,
				se.Country,
				se.DefaultCurrency,
				se.UseDefaultCurrencyForInput,
				se.VatRate,
				se.AmadeusRizUsingMode,
				se.IsPassengerPassportRequired,
				se.AviaOrderItemGenerationOption,
				se.AllowAgentSetOrderVat,
				se.UseAviaDocumentVatInOrder,
				se.AviaDocumentVatOptions,
				se.AccountantDisplayString,
				se.IncomingCashOrderCorrespondentAccount,
				se.SeparateDocumentAccess,
				se.SeparateDocumentAccessByAgent,
				se.AllowOtherAgentsToModifyProduct,
				se.IsOrganizationCodeRequired,
				se.UseConsolidatorCommission,
				se.DefaultConsolidatorCommission,
				se.UseAviaHandling,
				se.UseBonuses,
				se.DaysBeforeDeparture,
				se.BirthdayTaskResponsible,
				se.IsOrderRequiredForProcessedDocument,
				se.MetricsFromDate,
				se.ReservationsInOfficeMetrics,
				se.McoRequiresDescription,
				se.NeutralAirlineCode,
				se.Order_UseServiceFeeOnlyInVat,
				se.Consignment_NumberMode,
				se.Invoice_NumberMode,
				se.Invoice_CanOwnerSelect,
				se.InvoicePrinter_ShowVat,
				se.Invoice_DefaultIssuedBy,
				se.InvoicePrinter_FooterDetails,
				se.DrctWebService_LoadedOn,
				se.GalileoWebService_LoadedOn,
				se.GalileoRailWebService_LoadedOn,
				se.GalileoBusWebService_LoadedOn,
				se.TravelPointWebService_LoadedOn,
				se.Consignment_SeparateBookingFee,
				se.Pasterboard_DefaultPaymentType,
				se.Ticket_NoPrintReservations,
	*/


	public partial class SystemConfigurationEditForm : EntityEditForm
	{

		static SystemConfigurationEditForm()
		{
			RegisterEdit("SystemConfiguration", typeof(SystemConfigurationEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.SystemConfiguration;
		}

		private SystemConfigurationSemantic se;

	}

	#endregion


	#region AirlineCommissionPercents

	/// <summary>Комиссия от авиакомпании</summary>
	public partial class AirlineCommissionPercentsSemantic : EntitySemantic
	{

		public AirlineCommissionPercentsSemantic()
		{
			_name = "AirlineCommissionPercents";
			_className = "AirlineCommissionPercents";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Airline";
			_title ="Комиссия от авиакомпании";
			_titles = "Комиссии от авиакомпаний";
		}

		[PreserveCase]
		public SemanticMember Airline = Member
			.Reference("Organization")
			.EntityName();

		/// <summary>Перелет внутри страны, %</summary>
		[PreserveCase]
		public SemanticMember Domestic = Member
			.Title("Перелет внутри страны, %")
			.Float2()
			.Required();

		/// <summary>Международный перелет, %</summary>
		[PreserveCase]
		public SemanticMember International = Member
			.Title("Международный перелет, %")
			.Float2()
			.Required();

		/// <summary>Перелет внутри страны (interline), %</summary>
		[PreserveCase]
		public SemanticMember InterlineDomestic = Member
			.Title("Перелет внутри страны (interline), %")
			.Float2()
			.Required();

		/// <summary>Международный перелет (interline), %</summary>
		[PreserveCase]
		public SemanticMember InterlineInternational = Member
			.Title("Международный перелет (interline), %")
			.Float2()
			.Required();

	}

	/*
				se.Airline,
				se.Domestic,
				se.International,
				se.InterlineDomestic,
				se.InterlineInternational,
	*/

	public partial class AirlineCommissionPercentsListTab : EntityListTab
	{

		static AirlineCommissionPercentsListTab()
		{
			RegisterList("AirlineCommissionPercents", typeof(AirlineCommissionPercentsListTab));
		}

		public AirlineCommissionPercentsListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AirlineCommissionPercents;
		}

		private AirlineCommissionPercentsSemantic se;

	}


	public partial class AirlineCommissionPercentsEditForm : EntityEditForm
	{

		static AirlineCommissionPercentsEditForm()
		{
			RegisterEdit("AirlineCommissionPercents", typeof(AirlineCommissionPercentsEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AirlineCommissionPercents;
		}

		private AirlineCommissionPercentsSemantic se;

	}

	#endregion


	#region DocumentOwner

	/// <summary>Владелец документов</summary>
	public partial class DocumentOwnerSemantic : EntitySemantic
	{

		public DocumentOwnerSemantic()
		{
			_name = "DocumentOwner";
			_className = "DocumentOwner";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Owner";
			_title ="Владелец документов";
			_titles = "Владельцы документов";
		}

		/// <summary>Владелец</summary>
		[PreserveCase]
		public SemanticMember Owner = Member
			.Title("Владелец")
			.Reference("Party")
			.EntityName();

		/// <summary>Действующий</summary>
		[PreserveCase]
		public SemanticMember IsActive = Member
			.Title("Действующий")
			.Bool()
			.Required();

		/// <summary>По умолчанию</summary>
		[PreserveCase]
		public SemanticMember IsDefault = Member
			.Title("По умолчанию")
			.Bool()
			.Required();

	}

	/*
				se.Owner,
				se.IsActive,
				se.IsDefault,
	*/

	public partial class DocumentOwnerListTab : EntityListTab
	{

		static DocumentOwnerListTab()
		{
			RegisterList("DocumentOwner", typeof(DocumentOwnerListTab));
		}

		public DocumentOwnerListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.DocumentOwner;
		}

		private DocumentOwnerSemantic se;

	}


	public partial class DocumentOwnerEditForm : EntityEditForm
	{

		static DocumentOwnerEditForm()
		{
			RegisterEdit("DocumentOwner", typeof(DocumentOwnerEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.DocumentOwner;
		}

		private DocumentOwnerSemantic se;

	}

	#endregion


	#region DocumentAccess

	/// <summary>Доступ к документам</summary>
	public partial class DocumentAccessSemantic : Entity2Semantic
	{

		public DocumentAccessSemantic()
		{
			_name = "DocumentAccess";
			_className = "DocumentAccess";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Person";
			_title ="Доступ к документам";
		}

		/// <summary>Персона</summary>
		[PreserveCase]
		public SemanticMember Person = Member
			.Title("Персона")
			.Reference("Person")
			.EntityName();

		/// <summary>Владелец</summary>
		[PreserveCase]
		public SemanticMember Owner = Member
			.Title("Владелец")
			.EnumReference("DocumentOwner");

		/// <summary>Полный доступ</summary>
		[PreserveCase]
		public SemanticMember FullDocumentControl = Member
			.Title("Полный доступ")
			.Bool()
			.Required();

	}

	/*
				se.Person,
				se.Owner,
				se.FullDocumentControl,
	*/

	public partial class DocumentAccessListTab : EntityListTab
	{

		static DocumentAccessListTab()
		{
			RegisterList("DocumentAccess", typeof(DocumentAccessListTab));
		}

		public DocumentAccessListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.DocumentAccess;
		}

		private DocumentAccessSemantic se;

	}


	public partial class DocumentAccessEditForm : EntityEditForm
	{

		static DocumentAccessEditForm()
		{
			RegisterEdit("DocumentAccess", typeof(DocumentAccessEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.DocumentAccess;
		}

		private DocumentAccessSemantic se;

	}

	#endregion


	#region ClosedPeriod

	/// <summary>Закрытый период</summary>
	public partial class ClosedPeriodSemantic : Entity2Semantic
	{

		public ClosedPeriodSemantic()
		{
			_name = "ClosedPeriod";
			_className = "ClosedPeriod";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = null;
			_title ="Закрытый период";
			_titles = "Закрытые периоды";
		}

		/// <summary>Дата с</summary>
		[PreserveCase]
		public SemanticMember DateFrom = Member
			.Title("Дата с")
			.Date()
			.Required()
			.EntityDate();

		/// <summary>Дата по</summary>
		[PreserveCase]
		public SemanticMember DateTo = Member
			.Title("Дата по")
			.Date()
			.Required();

		/// <summary>Состояние</summary>
		[PreserveCase]
		public SemanticMember PeriodState = Member
			.Title("Состояние")
			.EnumItems(new object[][]
			{
				new object[] { 2, "Закрытый" }, // Closed
				new object[] { 0, "Открытый" }, // Open
				new object[] { 1, "Ограниченный" }, // Restricted
			})
			.Required()
			.DefaultValue(2);

	}

	/*
				se.DateFrom,
				se.DateTo,
				se.PeriodState,
	*/

	public partial class ClosedPeriodListTab : EntityListTab
	{

		static ClosedPeriodListTab()
		{
			RegisterList("ClosedPeriod", typeof(ClosedPeriodListTab));
		}

		public ClosedPeriodListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.ClosedPeriod;
		}

		private ClosedPeriodSemantic se;

	}


	public partial class ClosedPeriodEditForm : EntityEditForm
	{

		static ClosedPeriodEditForm()
		{
			RegisterEdit("ClosedPeriod", typeof(ClosedPeriodEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.ClosedPeriod;
		}

		private ClosedPeriodSemantic se;

	}

	#endregion


	#region OpeningBalance

	/// <summary>Начальный остаток</summary>
	public partial class OpeningBalanceSemantic : Entity2Semantic
	{

		public OpeningBalanceSemantic()
		{
			_name = "OpeningBalance";
			_className = "OpeningBalance";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Начальный остаток";
			_titles = "Начальные остатки";
		}

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.EntityName();

		/// <summary>Дата</summary>
		[PreserveCase]
		public SemanticMember Date = Member
			.Title("Дата")
			.Date()
			.Required();

		/// <summary>Контрагент</summary>
		[PreserveCase]
		public SemanticMember Party = Member
			.Title("Контрагент")
			.Reference("Party");

		/// <summary>Остаток</summary>
		[PreserveCase]
		public SemanticMember Balance = Member
			.Title("Остаток")
			.Float2()
			.Required();

	}

	/*
				se.Number,
				se.Date,
				se.Party,
				se.Balance,
	*/

	public partial class OpeningBalanceListTab : EntityListTab
	{

		static OpeningBalanceListTab()
		{
			RegisterList("OpeningBalance", typeof(OpeningBalanceListTab));
		}

		public OpeningBalanceListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.OpeningBalance;
		}

		private OpeningBalanceSemantic se;

	}


	public partial class OpeningBalanceEditForm : EntityEditForm
	{

		static OpeningBalanceEditForm()
		{
			RegisterEdit("OpeningBalance", typeof(OpeningBalanceEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.OpeningBalance;
		}

		private OpeningBalanceSemantic se;

	}

	#endregion


	#region InternalTransfer

	/// <summary>Внутренний перевод</summary>
	public partial class InternalTransferSemantic : Entity2Semantic
	{

		public InternalTransferSemantic()
		{
			_name = "InternalTransfer";
			_className = "InternalTransfer";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Внутренний перевод";
			_titles = "Внутренние переводы";
		}

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.EntityName();

		/// <summary>Дата</summary>
		[PreserveCase]
		public SemanticMember Date = Member
			.Title("Дата")
			.Date()
			.Required()
			.EntityDate();

		/// <summary>Из заказа</summary>
		[PreserveCase]
		public SemanticMember FromOrder = Member
			.Title("Из заказа")
			.Reference("Order");

		/// <summary>От контрагента</summary>
		[PreserveCase]
		public SemanticMember FromParty = Member
			.Title("От контрагента")
			.Reference("Customer")
			.Required();

		/// <summary>В заказ</summary>
		[PreserveCase]
		public SemanticMember ToOrder = Member
			.Title("В заказ")
			.Reference("Order");

		/// <summary>К контрагенту</summary>
		[PreserveCase]
		public SemanticMember ToParty = Member
			.Title("К контрагенту")
			.Reference("Customer")
			.Required();

		/// <summary>Сумма</summary>
		[PreserveCase]
		public SemanticMember Amount = Member
			.Title("Сумма")
			.Float2()
			.Required();

	}

	/*
				se.Number,
				se.Date,
				se.FromOrder,
				se.FromParty,
				se.ToOrder,
				se.ToParty,
				se.Amount,
	*/

	public partial class InternalTransferListTab : EntityListTab
	{

		static InternalTransferListTab()
		{
			RegisterList("InternalTransfer", typeof(InternalTransferListTab));
		}

		public InternalTransferListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.InternalTransfer;
		}

		private InternalTransferSemantic se;

	}


	public partial class InternalTransferEditForm : EntityEditForm
	{

		static InternalTransferEditForm()
		{
			RegisterEdit("InternalTransfer", typeof(InternalTransferEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.InternalTransfer;
		}

		private InternalTransferSemantic se;

	}

	#endregion


	#region GdsAgent

	/// <summary>Gds-агент</summary>
	public partial class GdsAgentSemantic : Entity2Semantic
	{

		public GdsAgentSemantic()
		{
			_name = "GdsAgent";
			_className = "GdsAgent";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Code";
			_title ="Gds-агент";
			_titles = "Gds-агенты";
		}

		/// <summary>Персона</summary>
		[PreserveCase]
		public SemanticMember Person = Member
			.Title("Персона")
			.Reference("Person");

		/// <summary>Источник документов</summary>
		[PreserveCase]
		public SemanticMember Origin = Member
			.Title("Источник документов")
			.EnumItems(new object[][]
			{
				new object[] { 0, "AmadeusAir" }, // AmadeusAir
				new object[] { 1, "AmadeusPrint" }, // AmadeusPrint
				new object[] { 8, "AmadeusXml" }, // AmadeusXml
				new object[] { 4, "BspLink" }, // BspLink
				new object[] { 14, "Drct" }, // Drct
				new object[] { 12, "GalileoBusXml" }, // GalileoBusXml
				new object[] { 2, "GalileoMir" }, // GalileoMir
				new object[] { 10, "GalileoRailXml" }, // GalileoRailXml
				new object[] { 3, "GalileoTkt" }, // GalileoTkt
				new object[] { 7, "GalileoXml" }, // GalileoXml
				new object[] { 11, "LuxenaXml" }, // LuxenaXml
				new object[] { 5, "Manual" }, // Manual
				new object[] { 9, "SabreFil" }, // SabreFil
				new object[] { 15, "SabreTerminal" }, // SabreTerminal
				new object[] { 6, "SirenaXml" }, // SirenaXml
				new object[] { 16, "SPRK" }, // SPRK
				new object[] { 13, "TravelPointXml" }, // TravelPointXml
			})
			.Required();

		/// <summary>Код агента</summary>
		[PreserveCase]
		public SemanticMember Code = Member
			.Title("Код агента")
			.String()
			.EntityName();

		/// <summary>Код офиса</summary>
		[PreserveCase]
		public SemanticMember OfficeCode = Member
			.Title("Код офиса")
			.String();

		/// <summary>Поставщик</summary>
		[PreserveCase]
		public SemanticMember Provider = Member
			.Title("Поставщик")
			.Reference("Organization");

		/// <summary>Владелец</summary>
		[PreserveCase]
		public SemanticMember Office = Member
			.Title("Владелец")
			.EnumReference("DocumentOwner");

		/// <summary>Юрлицо</summary>
		[PreserveCase]
		public SemanticMember LegalEntity = Member
			.Title("Юрлицо")
			.Reference("Organization");

	}

	/*
				se.Person,
				se.Origin,
				se.Code,
				se.OfficeCode,
				se.Provider,
				se.Office,
				se.LegalEntity,
	*/

	public partial class GdsAgentListTab : EntityListTab
	{

		static GdsAgentListTab()
		{
			RegisterList("GdsAgent", typeof(GdsAgentListTab));
		}

		public GdsAgentListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.GdsAgent;
		}

		private GdsAgentSemantic se;

	}


	public partial class GdsAgentEditForm : EntityEditForm
	{

		static GdsAgentEditForm()
		{
			RegisterEdit("GdsAgent", typeof(GdsAgentEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.GdsAgent;
		}

		private GdsAgentSemantic se;

	}

	#endregion


	#region GdsFile

	/// <summary>Gds-файл</summary>
	public partial class GdsFileSemantic : Entity3Semantic
	{

		public GdsFileSemantic()
		{
			_name = "GdsFile";
			_className = "GdsFile";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] {  };
			};
			_title ="Gds-файл";
			_titles = "Gds-файлы";
		}

		/// <summary>Тип</summary>
		[PreserveCase]
		public SemanticMember FileType = Member
			.Title("Тип")
			.EnumItems(new object[][]
			{
				new object[] { 0, "AirFile" }, // AirFile
				new object[] { 6, "AmadeusXmlFile" }, // AmadeusXmlFile
				new object[] { 12, "CrazyllamaPnrFile" }, // CrazyllamaPnrFile
				new object[] { 13, "DrctXmlFile" }, // DrctXmlFile
				new object[] { 10, "GalileoBusXmlFile" }, // GalileoBusXmlFile
				new object[] { 8, "GalileoRailXmlFile" }, // GalileoRailXmlFile
				new object[] { 5, "GalileoXmlFile" }, // GalileoXmlFile
				new object[] { 9, "LuxenaXmlFile" }, // LuxenaXmlFile
				new object[] { 1, "MirFile" }, // MirFile
				new object[] { 3, "PrintFile" }, // PrintFile
				new object[] { 7, "SabreFilFile" }, // SabreFilFile
				new object[] { 4, "SirenaFile" }, // SirenaFile
				new object[] { 2, "TktFile" }, // TktFile
				new object[] { 11, "TravelPointXmlFile" }, // TravelPointXmlFile
			})
			.Required();

		/// <summary>Дата импорта</summary>
		[PreserveCase]
		public SemanticMember TimeStamp = Member
			.Title("Дата импорта")
			.DateTime2()
			.Required();

		/// <summary>Содержимое</summary>
		[PreserveCase]
		public SemanticMember Content = Member
			.Title("Содержимое")
			.Text(10);

		/// <summary>Результат импорта</summary>
		[PreserveCase]
		public SemanticMember ImportResult = Member
			.Title("Результат импорта")
			.EnumItems(new object[][]
			{
				new object[] { 2, "Error" }, // Error
				new object[] { 0, "None" }, // None
				new object[] { 4, "Reimported" }, // Reimported
				new object[] { 1, "Success" }, // Success
				new object[] { 3, "Warn" }, // Warn
			})
			.Required();

		/// <summary>Журнал</summary>
		[PreserveCase]
		public SemanticMember ImportOutput = Member
			.Title("Журнал")
			.String();

	}

	/*
				se.FileType,
				se.TimeStamp,
				se.Content,
				se.ImportResult,
				se.ImportOutput,
	*/


	#endregion


	#region Invoice

	/// <summary>Счет/квитанция</summary>
	public partial class InvoiceSemantic : EntitySemantic
	{

		public InvoiceSemantic()
		{
			_name = "Invoice";
			_className = "Invoice";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Счет/квитанция";
			_titles = "Счета/квитанции";
		}

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.EntityName();

		/// <summary>Договор</summary>
		[PreserveCase]
		public SemanticMember Agreement = Member
			.Title("Договор")
			.String();

		/// <summary>Дата выпуска</summary>
		[PreserveCase]
		public SemanticMember IssueDate = Member
			.Title("Дата выпуска")
			.Date()
			.Required();

		/// <summary>Дата создания</summary>
		[PreserveCase]
		public SemanticMember TimeStamp = Member
			.Title("Дата создания")
			.DateTime2()
			.Required()
			.Utility();

		/// <summary>Тип</summary>
		[PreserveCase]
		public SemanticMember Type = Member
			.Title("Тип")
			.EnumItems(new object[][]
			{
				new object[] { 2, "Акт выполненных работ" }, // CompletionCertificate
				new object[] { 0, "Счет" }, // Invoice
				new object[] { 1, "Квитанция" }, // Receipt
			})
			.Required();

		[PreserveCase]
		public SemanticMember Content = Member
			;

		/// <summary>Заказ</summary>
		[PreserveCase]
		public SemanticMember Order = Member
			.Title("Заказ")
			.Reference("Order");

		/// <summary>Заказчик</summary>
		[PreserveCase]
		public SemanticMember Customer = Member
			.Title("Заказчик")
			.Reference("Customer");

		/// <summary>Плательщик</summary>
		[PreserveCase]
		public SemanticMember BillTo = Member
			.Title("Плательщик")
			.Reference("Party");

		/// <summary>Плательщик</summary>
		[PreserveCase]
		public SemanticMember BillToName = Member
			.Title("Плательщик")
			.String();

		/// <summary>Получатель</summary>
		[PreserveCase]
		public SemanticMember ShipTo = Member
			.Title("Получатель")
			.Reference("Party");

		/// <summary>Заказ аннулирован</summary>
		[PreserveCase]
		public SemanticMember IsOrderVoid = Member
			.Title("Заказ аннулирован")
			.Bool()
			.Required();

		/// <summary>Владелец</summary>
		[PreserveCase]
		public SemanticMember Owner = Member
			.Title("Владелец")
			.Reference("Party");

		/// <summary>Выпущен</summary>
		[PreserveCase]
		public SemanticMember IssuedBy = Member
			.Title("Выпущен")
			.Reference("Person");

		/// <summary>Итого</summary>
		[PreserveCase]
		public SemanticMember Total = Member
			.Title("Итого")
			.Money();

		/// <summary>В т.ч. НДС</summary>
		[PreserveCase]
		public SemanticMember Vat = Member
			.Title("В т.ч. НДС")
			.Money();

		/// <summary>Формат файла</summary>
		[PreserveCase]
		public SemanticMember FileExtension = Member
			.Title("Формат файла")
			.String();

	}

	/*
				se.Number,
				se.Agreement,
				se.IssueDate,
				se.TimeStamp,
				se.Type,
				se.Content,
				se.Order,
				se.Customer,
				se.BillTo,
				se.BillToName,
				se.ShipTo,
				se.IsOrderVoid,
				se.Owner,
				se.IssuedBy,
				se.Total,
				se.Vat,
				se.FileExtension,
	*/

	public partial class InvoiceListTab : EntityListTab
	{

		static InvoiceListTab()
		{
			RegisterList("Invoice", typeof(InvoiceListTab));
		}

		public InvoiceListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Invoice;
		}

		private InvoiceSemantic se;

	}


	public partial class InvoiceEditForm : EntityEditForm
	{

		static InvoiceEditForm()
		{
			RegisterEdit("Invoice", typeof(InvoiceEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Invoice;
		}

		private InvoiceSemantic se;

	}

	#endregion


	#region Receipt

	public partial class ReceiptSemantic : InvoiceSemantic
	{

		public ReceiptSemantic()
		{
			_name = "Receipt";
			_className = "Invoice";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
		}

	}

	/*
	*/


	#endregion


	#region Task

	/// <summary>Задача</summary>
	public partial class TaskSemantic : Entity2Semantic
	{

		public TaskSemantic()
		{
			_name = "Task";
			_className = "Task";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Задача";
			_titles = "Задачи";
		}

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.EntityName();

		/// <summary>Тема</summary>
		[PreserveCase]
		public SemanticMember Subject = Member
			.Title("Тема")
			.String();

		/// <summary>Описание</summary>
		[PreserveCase]
		public SemanticMember Description = Member
			.Title("Описание")
			.Text(3);

		/// <summary>Относительно</summary>
		[PreserveCase]
		public SemanticMember RelatedTo = Member
			.Title("Относительно")
			.Reference("Party");

		/// <summary>Заказ</summary>
		[PreserveCase]
		public SemanticMember Order = Member
			.Title("Заказ")
			.Reference("Order");

		/// <summary>Ответственный</summary>
		[PreserveCase]
		public SemanticMember AssignedTo = Member
			.Title("Ответственный")
			.Reference("Party");

		/// <summary>Статус</summary>
		[PreserveCase]
		public SemanticMember Status = Member
			.Title("Статус")
			.EnumItems(new object[][]
			{
				new object[] { 3, "Закрыта" }, // Closed
				new object[] { 1, "Выполняется" }, // InProgress
				new object[] { 0, "Открыта" }, // Open
				new object[] { 2, "В ожидании ответа" }, // WaitForResponse
			})
			.Required();

		/// <summary>Выполнить до</summary>
		[PreserveCase]
		public SemanticMember DueDate = Member
			.Title("Выполнить до")
			.Date();

		/// <summary>Просроченный</summary>
		[PreserveCase]
		public SemanticMember Overdue = Member
			.Title("Просроченный")
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember Comments = Member
			;

		[PreserveCase]
		public SemanticMember CanModify = Member
			.Bool()
			.Required();

	}

	/*
				se.Number,
				se.Subject,
				se.Description,
				se.RelatedTo,
				se.Order,
				se.AssignedTo,
				se.Status,
				se.DueDate,
				se.Overdue,
				se.Comments,
				se.CanModify,
	*/

	public partial class TaskListTab : EntityListTab
	{

		static TaskListTab()
		{
			RegisterList("Task", typeof(TaskListTab));
		}

		public TaskListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Task;
		}

		private TaskSemantic se;

	}


	public partial class TaskEditForm : EntityEditForm
	{

		static TaskEditForm()
		{
			RegisterEdit("Task", typeof(TaskEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Task;
		}

		private TaskSemantic se;

	}

	#endregion


	#region Payment

	/// <summary>Платёж</summary>
	public partial class PaymentSemantic : Entity2Semantic
	{

		public PaymentSemantic()
		{
			_name = "Payment";
			_className = "Payment";
			_isAbstract = true;
			_getDerivedEntities = delegate (SemanticDomain dsm)
			{
				return new SemanticEntity[] {  };
			};
			_nameFieldName = "Number";
			_title ="Платёж";
			_titles = "Платежи";
		}

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.EntityName();

		/// <summary>Дата</summary>
		[PreserveCase]
		public SemanticMember Date = Member
			.Title("Дата")
			.Date()
			.Required();

		/// <summary>Форма оплаты</summary>
		[PreserveCase]
		public SemanticMember PaymentForm = Member
			.Title("Форма оплаты")
			.EnumItems(new object[][]
			{
				new object[] { 0, "ПКО" }, // CashInOrder
				new object[] { 4, "РКО" }, // CashOutOrder
				new object[] { 2, "Кассовый чек" }, // Check
				new object[] { 3, "Электронный платеж" }, // Electronic
				new object[] { 1, "Безналичный платеж" }, // WireTransfer
			})
			.Required();

		/// <summary>Номер документа</summary>
		[PreserveCase]
		public SemanticMember DocumentNumber = Member
			.Title("Номер документа")
			.String();

		[PreserveCase]
		public SemanticMember DocumentUniqueCode = Member
			.String();

		/// <summary>Плательщик</summary>
		[PreserveCase]
		public SemanticMember Payer = Member
			.Title("Плательщик")
			.Reference("Party")
			.Required();

		/// <summary>Заказ</summary>
		[PreserveCase]
		public SemanticMember Order = Member
			.Title("Заказ")
			.Reference("Order");

		/// <summary>Квитанция</summary>
		[PreserveCase]
		public SemanticMember Invoice = Member
			.Title("Квитанция")
			.Reference("Receipt");

		/// <summary>Дата счета/квитанции</summary>
		[PreserveCase]
		public SemanticMember InvoiceDate = Member
			.Title("Дата счета/квитанции")
			.Date();

		[PreserveCase]
		public SemanticMember Sign = Member
			.Int32()
			.Required();

		/// <summary>Сумма</summary>
		[PreserveCase]
		public SemanticMember Amount = Member
			.Title("Сумма")
			.Money();

		/// <summary>В т.ч. НДС</summary>
		[PreserveCase]
		public SemanticMember Vat = Member
			.Title("В т.ч. НДС")
			.Money();

		/// <summary>Получен от</summary>
		[PreserveCase]
		public SemanticMember ReceivedFrom = Member
			.Title("Получен от")
			.String();

		/// <summary>Дата проводки</summary>
		[PreserveCase]
		public SemanticMember PostedOn = Member
			.Title("Дата проводки")
			.Date();

		/// <summary>Сохранить проведенным</summary>
		[PreserveCase]
		public SemanticMember SavePosted = Member
			.Title("Сохранить проведенным")
			.Bool()
			.Required();

		/// <summary>Примечание</summary>
		[PreserveCase]
		public SemanticMember Note = Member
			.Title("Примечание")
			.Text(3);

		/// <summary>Аннулирован</summary>
		[PreserveCase]
		public SemanticMember IsVoid = Member
			.Title("Аннулирован")
			.Bool()
			.Required();

		/// <summary>Ответственный</summary>
		[PreserveCase]
		public SemanticMember AssignedTo = Member
			.Title("Ответственный")
			.Reference("Person");

		/// <summary>Зарегистрирован</summary>
		[PreserveCase]
		public SemanticMember RegisteredBy = Member
			.Title("Зарегистрирован")
			.Reference("Person");

		[PreserveCase]
		public SemanticMember IsPosted = Member
			.Bool()
			.Required();

		/// <summary>Владелец</summary>
		[PreserveCase]
		public SemanticMember Owner = Member
			.Title("Владелец")
			.Reference("Party");

		/// <summary>Банковский счёт</summary>
		[PreserveCase]
		public SemanticMember BankAccount = Member
			.Title("Банковский счёт")
			.EnumReference("BankAccount");

		[PreserveCase]
		public SemanticMember PrintedDocument = Member
			;

		/// <summary>Платёжная система</summary>
		[PreserveCase]
		public SemanticMember PaymentSystem = Member
			.Title("Платёжная система")
			.Reference("PaymentSystem");

		[PreserveCase]
		public SemanticMember IsCashOrder = Member
			.Bool()
			.Required()
			.Utility();

		[PreserveCase]
		public SemanticMember IsCashInOrder = Member
			.Bool()
			.Required()
			.Utility();

		[PreserveCase]
		public SemanticMember IsCashOutOrder = Member
			.Bool()
			.Required()
			.Utility();

		[PreserveCase]
		public SemanticMember IsCheck = Member
			.Bool()
			.Required()
			.Utility();

		[PreserveCase]
		public SemanticMember IsElectronic = Member
			.Bool()
			.Required()
			.Utility();

		[PreserveCase]
		public SemanticMember IsWireTransfer = Member
			.Bool()
			.Required()
			.Utility();

	}

	/*
				se.Number,
				se.Date,
				se.PaymentForm,
				se.DocumentNumber,
				se.DocumentUniqueCode,
				se.Payer,
				se.Order,
				se.Invoice,
				se.InvoiceDate,
				se.Sign,
				se.Amount,
				se.Vat,
				se.ReceivedFrom,
				se.PostedOn,
				se.SavePosted,
				se.Note,
				se.IsVoid,
				se.AssignedTo,
				se.RegisteredBy,
				se.IsPosted,
				se.Owner,
				se.BankAccount,
				se.PrintedDocument,
				se.PaymentSystem,
				se.IsCashOrder,
				se.IsCashInOrder,
				se.IsCashOutOrder,
				se.IsCheck,
				se.IsElectronic,
				se.IsWireTransfer,
	*/


	#endregion


	#region Contract

	/// <summary>Контракт</summary>
	public partial class ContractSemantic : Entity2Semantic
	{

		public ContractSemantic()
		{
			_name = "Contract";
			_className = "Contract";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Number";
			_title ="Контракт";
			_titles = "Контракты";
		}

		/// <summary>Организация</summary>
		[PreserveCase]
		public SemanticMember Customer = Member
			.Title("Организация")
			.Reference("Organization")
			.Required();

		/// <summary>Номер</summary>
		[PreserveCase]
		public SemanticMember Number = Member
			.Title("Номер")
			.String()
			.Required()
			.EntityName();

		/// <summary>Дата</summary>
		[PreserveCase]
		public SemanticMember IssueDate = Member
			.Title("Дата")
			.Date()
			.EntityDate();

		/// <summary>Дисконт, %</summary>
		[PreserveCase]
		public SemanticMember DiscountPc = Member
			.Title("Дисконт, %")
			.Float2()
			.Required();

		/// <summary>Примечание</summary>
		[PreserveCase]
		public SemanticMember Note = Member
			.Title("Примечание")
			.Text(3);

	}

	/*
				se.Customer,
				se.Number,
				se.IssueDate,
				se.DiscountPc,
				se.Note,
	*/

	public partial class ContractListTab : EntityListTab
	{

		static ContractListTab()
		{
			RegisterList("Contract", typeof(ContractListTab));
		}

		public ContractListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Contract;
		}

		private ContractSemantic se;

	}


	public partial class ContractEditForm : EntityEditForm
	{

		static ContractEditForm()
		{
			RegisterEdit("Contract", typeof(ContractEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Contract;
		}

		private ContractSemantic se;

	}

	#endregion


	#region Modification

	/// <summary>Изменение</summary>
	public partial class ModificationSemantic : EntitySemantic
	{

		public ModificationSemantic()
		{
			_name = "Modification";
			_className = "Modification";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = null;
			_title ="Изменение";
			_titles = "История изменений";
		}

		/// <summary>Время</summary>
		[PreserveCase]
		public SemanticMember TimeStamp = Member
			.Title("Время")
			.DateTime2()
			.Required();

		/// <summary>Автор</summary>
		[PreserveCase]
		public SemanticMember Author = Member
			.Title("Автор")
			.String();

		/// <summary>Тип</summary>
		[PreserveCase]
		public SemanticMember Type = Member
			.Title("Тип")
			.EnumItems(new object[][]
			{
				new object[] { 0, "Добавлен" }, // Create
				new object[] { 2, "Удалён" }, // Delete
				new object[] { 1, "Изменён" }, // Update
			})
			.Required();

		[PreserveCase]
		public SemanticMember InstanceType = Member
			.String();

		[PreserveCase]
		public SemanticMember InstanceId = Member
			.String();

		/// <summary>Объект</summary>
		[PreserveCase]
		public SemanticMember InstanceString = Member
			.Title("Объект")
			.String();

		[PreserveCase]
		public SemanticMember Comment = Member
			.String();

		[PreserveCase]
		public SemanticMember Items = Member
			;

		/// <summary>Изменённые свойства</summary>
		[PreserveCase]
		public SemanticMember ItemsJson = Member
			.Title("Изменённые свойства")
			.String();

	}

	/*
				se.TimeStamp,
				se.Author,
				se.Type,
				se.InstanceType,
				se.InstanceId,
				se.InstanceString,
				se.Comment,
				se.Items,
				se.ItemsJson,
	*/

	public partial class ModificationListTab : EntityListTab
	{

		static ModificationListTab()
		{
			RegisterList("Modification", typeof(ModificationListTab));
		}

		public ModificationListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Modification;
		}

		private ModificationSemantic se;

	}


	public partial class ModificationEditForm : EntityEditForm
	{

		static ModificationEditForm()
		{
			RegisterEdit("Modification", typeof(ModificationEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.Modification;
		}

		private ModificationSemantic se;

	}

	#endregion


	#region FlightSegment

	/// <summary>Полётный сегмент</summary>
	public partial class FlightSegmentSemantic : Entity2Semantic
	{

		public FlightSegmentSemantic()
		{
			_name = "FlightSegment";
			_className = "FlightSegment";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "Position";
			_title ="Полётный сегмент";
			_titles = "Полётные сегменты";
		}

		/// <summary>Авиабилет</summary>
		[PreserveCase]
		public SemanticMember Ticket = Member
			.Title("Авиабилет")
			.Reference("AviaTicket");

		[PreserveCase]
		public SemanticMember Position = Member
			.Int32()
			.Required()
			.EntityName();

		/// <summary>Тип сегмента</summary>
		[PreserveCase]
		public SemanticMember Type = Member
			.Title("Тип сегмента")
			.EnumItems(new object[][]
			{
				new object[] { 0, "Ticketed" }, // Ticketed
				new object[] { 1, "Unticketed" }, // Unticketed
				new object[] { 2, "Voided" }, // Voided
			})
			.Required();

		/// <summary>Из аэропорта</summary>
		[PreserveCase]
		public SemanticMember FromAirportCode = Member
			.Title("Из аэропорта")
			.String();

		[PreserveCase]
		public SemanticMember FromAirportName = Member
			.String();

		/// <summary>Из аэропорта</summary>
		[PreserveCase]
		public SemanticMember FromAirport = Member
			.Title("Из аэропорта")
			.Reference("Airport");

		[PreserveCase]
		public SemanticMember FromCountry = Member
			.Reference("Country");

		/// <summary>В аэропорт</summary>
		[PreserveCase]
		public SemanticMember ToAirportCode = Member
			.Title("В аэропорт")
			.String();

		[PreserveCase]
		public SemanticMember ToAirportName = Member
			.String();

		/// <summary>В аэропорт</summary>
		[PreserveCase]
		public SemanticMember ToAirport = Member
			.Title("В аэропорт")
			.Reference("Airport");

		[PreserveCase]
		public SemanticMember ToCountry = Member
			.Reference("Country");

		[PreserveCase]
		public SemanticMember CarrierIataCode = Member
			.String();

		/// <summary>Код</summary>
		[PreserveCase]
		public SemanticMember CarrierPrefixCode = Member
			.Title("Код")
			.String();

		/// <summary>Перевозчик</summary>
		[PreserveCase]
		public SemanticMember CarrierName = Member
			.Title("Перевозчик")
			.String();

		/// <summary>Перевозчик</summary>
		[PreserveCase]
		public SemanticMember Carrier = Member
			.Title("Перевозчик")
			.Reference("Airline");

		/// <summary>Оперирующий перевозчик</summary>
		[PreserveCase]
		public SemanticMember Operator = Member
			.Title("Оперирующий перевозчик")
			.Reference("Airline");

		/// <summary>Рейс</summary>
		[PreserveCase]
		public SemanticMember FlightNumber = Member
			.Title("Рейс")
			.String();

		/// <summary>Тип судна</summary>
		[PreserveCase]
		public SemanticMember Equipment = Member
			.Title("Тип судна")
			.Reference("AirplaneModel");

		/// <summary>Код сервис-класса</summary>
		[PreserveCase]
		public SemanticMember ServiceClassCode = Member
			.Title("Код сервис-класса")
			.String();

		/// <summary>Сервис-класс</summary>
		[PreserveCase]
		public SemanticMember ServiceClass = Member
			.Title("Сервис-класс")
			.EnumItems(new object[][]
			{
				new object[] { 3, "Бизнес" }, // Business
				new object[] { 1, "Эконом" }, // Economy
				new object[] { 4, "Первый" }, // First
				new object[] { 2, "Эконом-премиум" }, // PremiumEconomy
				new object[] { 0, "Неизвестно" }, // Unknown
			})
			.DefaultValue(1);

		/// <summary>Отправление</summary>
		[PreserveCase]
		public SemanticMember DepartureTime = Member
			.Title("Отправление")
			.DateTime();

		/// <summary>Прибытие</summary>
		[PreserveCase]
		public SemanticMember ArrivalTime = Member
			.Title("Прибытие")
			.DateTime();

		[PreserveCase]
		public SemanticMember MealCodes = Member
			.String();

		/// <summary>Питание</summary>
		[PreserveCase]
		public SemanticMember MealTypes = Member
			.Title("Питание")
			.EnumsItems(new object[][]
			{
				new object[] { 2048, "Платные алкогольные напитки" }, // AlcoholicBeveragesForPurchase
				new object[] { 512, "Бесплатные алкогольные напитки" }, // AlcoholicComplimentaryBeverages
				new object[] { 1, "Завтрак" }, // Breakfast
				new object[] { 32, "Холодная еда" }, // ColdMeal
				new object[] { 2, "Континентальный завтрак" }, // ContinentalBreakfast
				new object[] { 8, "Обед" }, // Dinner
				new object[] { 4096, "Duty Free" }, // DutyFree
				new object[] { 1024, "Платная еда" }, // FoodForPurchase
				new object[] { 64, "Горячая еда" }, // HotMeal
				new object[] { 4, "Ланч" }, // Lunch
				new object[] { 128, "Еда" }, // Meal
				new object[] { 0, "NoData" }, // NoData
				new object[] { 8192, "no meal service" }, // NoMealService
				new object[] { 256, "Напитки" }, // Refreshment
				new object[] { 16, "Закуска" }, // Snack
			});

		/// <summary>Кол-во остановок</summary>
		[PreserveCase]
		public SemanticMember NumberOfStops = Member
			.Title("Кол-во остановок")
			.Int32();

		/// <summary>Багаж</summary>
		[PreserveCase]
		public SemanticMember Luggage = Member
			.Title("Багаж")
			.String();

		/// <summary>Терминал</summary>
		[PreserveCase]
		public SemanticMember CheckInTerminal = Member
			.Title("Терминал")
			.String();

		/// <summary>Регистрация</summary>
		[PreserveCase]
		public SemanticMember CheckInTime = Member
			.Title("Регистрация")
			.String();

		/// <summary>Время перелета</summary>
		[PreserveCase]
		public SemanticMember Duration = Member
			.Title("Время перелета")
			.String();

		/// <summary>Терминал</summary>
		[PreserveCase]
		public SemanticMember ArrivalTerminal = Member
			.Title("Терминал")
			.String();

		/// <summary>Место</summary>
		[PreserveCase]
		public SemanticMember Seat = Member
			.Title("Место")
			.String();

		/// <summary>База тарифа</summary>
		[PreserveCase]
		public SemanticMember FareBasis = Member
			.Title("База тарифа")
			.String();

		/// <summary>Это конечный пункт</summary>
		[PreserveCase]
		public SemanticMember Stopover = Member
			.Title("Это конечный пункт")
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember Surcharges = Member
			.Float2();

		[PreserveCase]
		public SemanticMember IsInclusive = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember Fare = Member
			.Float2();

		[PreserveCase]
		public SemanticMember StopoverOrTransferCharge = Member
			.Float2();

		[PreserveCase]
		public SemanticMember IsSideTrip = Member
			.Bool()
			.Required();

		[PreserveCase]
		public SemanticMember Distance = Member
			.Float2()
			.Required();

		[PreserveCase]
		public SemanticMember Amount = Member
			.Money();

		[PreserveCase]
		public SemanticMember CouponAmount = Member
			.Money();

	}

	/*
				se.Ticket,
				se.Position,
				se.Type,
				se.FromAirportCode,
				se.FromAirportName,
				se.FromAirport,
				se.FromCountry,
				se.ToAirportCode,
				se.ToAirportName,
				se.ToAirport,
				se.ToCountry,
				se.CarrierIataCode,
				se.CarrierPrefixCode,
				se.CarrierName,
				se.Carrier,
				se.Operator,
				se.FlightNumber,
				se.Equipment,
				se.ServiceClassCode,
				se.ServiceClass,
				se.DepartureTime,
				se.ArrivalTime,
				se.MealCodes,
				se.MealTypes,
				se.NumberOfStops,
				se.Luggage,
				se.CheckInTerminal,
				se.CheckInTime,
				se.Duration,
				se.ArrivalTerminal,
				se.Seat,
				se.FareBasis,
				se.Stopover,
				se.Surcharges,
				se.IsInclusive,
				se.Fare,
				se.StopoverOrTransferCharge,
				se.IsSideTrip,
				se.Distance,
				se.Amount,
				se.CouponAmount,
	*/

	public partial class FlightSegmentListTab : EntityListTab
	{

		static FlightSegmentListTab()
		{
			RegisterList("FlightSegment", typeof(FlightSegmentListTab));
		}

		public FlightSegmentListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.FlightSegment;
		}

		private FlightSegmentSemantic se;

	}


	public partial class FlightSegmentEditForm : EntityEditForm
	{

		static FlightSegmentEditForm()
		{
			RegisterEdit("FlightSegment", typeof(FlightSegmentEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.FlightSegment;
		}

		private FlightSegmentSemantic se;

	}

	#endregion


	#region AirplaneModel

	/// <summary>Модель самолёта</summary>
	public partial class AirplaneModelSemantic : Entity3Semantic
	{

		public AirplaneModelSemantic()
		{
			_name = "AirplaneModel";
			_className = "AirplaneModel";
			_isAbstract = false;
			_getDerivedEntities = null;
			_title ="Модель самолёта";
			_titles = "Модели самолётов (типы судов)";
		}

		/// <summary>IATA код</summary>
		[PreserveCase]
		public SemanticMember IataCode = Member
			.Title("IATA код")
			.String(3)
			.Required();

		/// <summary>ICAO код</summary>
		[PreserveCase]
		public SemanticMember IcaoCode = Member
			.Title("ICAO код")
			.String(4);

	}

	/*
				se.IataCode,
				se.IcaoCode,
	*/

	public partial class AirplaneModelListTab : Entity3ListTab
	{

		static AirplaneModelListTab()
		{
			RegisterList("AirplaneModel", typeof(AirplaneModelListTab));
		}

		public AirplaneModelListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AirplaneModel;
		}

		private AirplaneModelSemantic se;

	}


	public partial class AirplaneModelEditForm : Entity3EditForm
	{

		static AirplaneModelEditForm()
		{
			RegisterEdit("AirplaneModel", typeof(AirplaneModelEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AirplaneModel;
		}

		private AirplaneModelSemantic se;

	}

	#endregion


	#region AmadeusAviaSftpRsaKey

	/// <summary>RSA-ключ для авиабилетов из Amadeus</summary>
	public partial class AmadeusAviaSftpRsaKeySemantic : Entity2Semantic
	{

		public AmadeusAviaSftpRsaKeySemantic()
		{
			_name = "AmadeusAviaSftpRsaKey";
			_className = "AmadeusAviaSftpRsaKey";
			_isAbstract = false;
			_getDerivedEntities = null;
			_nameFieldName = "PPK";
			_title ="RSA-ключ для авиабилетов из Amadeus";
			_titles = "RSA-ключи для авиабилетов из Amadeus";
		}

		/// <summary>Текст PPK</summary>
		[PreserveCase]
		public SemanticMember PPK = Member
			.Title("Текст PPK")
			.Text(20)
			.Required()
			.EntityName();

	}

	/*
				se.PPK,
	*/

	public partial class AmadeusAviaSftpRsaKeyListTab : EntityListTab
	{

		static AmadeusAviaSftpRsaKeyListTab()
		{
			RegisterList("AmadeusAviaSftpRsaKey", typeof(AmadeusAviaSftpRsaKeyListTab));
		}

		public AmadeusAviaSftpRsaKeyListTab(string tabId, ListArgs args): base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AmadeusAviaSftpRsaKey;
		}

		private AmadeusAviaSftpRsaKeySemantic se;

	}


	public partial class AmadeusAviaSftpRsaKeyEditForm : EntityEditForm
	{

		static AmadeusAviaSftpRsaKeyEditForm()
		{
			RegisterEdit("AmadeusAviaSftpRsaKey", typeof(AmadeusAviaSftpRsaKeyEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.AmadeusAviaSftpRsaKey;
		}

		private AmadeusAviaSftpRsaKeySemantic se;

	}

	#endregion


	partial class SemanticDomain
	{
		[PreserveCase] public PassportSemantic Passport = new PassportSemantic();
		[PreserveCase] public BankAccountSemantic BankAccount = new BankAccountSemantic();
		[PreserveCase] public MilesCardSemantic MilesCard = new MilesCardSemantic();
		[PreserveCase] public PartySemantic Party = new PartySemantic();
		[PreserveCase] public OrganizationSemantic Organization = new OrganizationSemantic();
		[PreserveCase] public PersonSemantic Person = new PersonSemantic();
		[PreserveCase] public DepartmentSemantic Department = new DepartmentSemantic();
		[PreserveCase] public AccommodationProviderSemantic AccommodationProvider = new AccommodationProviderSemantic();
		[PreserveCase] public BusTicketProviderSemantic BusTicketProvider = new BusTicketProviderSemantic();
		[PreserveCase] public PasteboardProviderSemantic PasteboardProvider = new PasteboardProviderSemantic();
		[PreserveCase] public TransferProviderSemantic TransferProvider = new TransferProviderSemantic();
		[PreserveCase] public GenericProductProviderSemantic GenericProductProvider = new GenericProductProviderSemantic();
		[PreserveCase] public CarRentalProviderSemantic CarRentalProvider = new CarRentalProviderSemantic();
		[PreserveCase] public TourProviderSemantic TourProvider = new TourProviderSemantic();
		[PreserveCase] public RoamingOperatorSemantic RoamingOperator = new RoamingOperatorSemantic();
		[PreserveCase] public InsuranceCompanySemantic InsuranceCompany = new InsuranceCompanySemantic();
		[PreserveCase] public AirlineSemantic Airline = new AirlineSemantic();
		[PreserveCase] public CustomerSemantic Customer = new CustomerSemantic();
		[PreserveCase] public OrderSemantic Order = new OrderSemantic();
		[PreserveCase] public CurrencyDailyRateSemantic CurrencyDailyRate = new CurrencyDailyRateSemantic();
		[PreserveCase] public CateringTypeSemantic CateringType = new CateringTypeSemantic();
		[PreserveCase] public AccommodationTypeSemantic AccommodationType = new AccommodationTypeSemantic();
		[PreserveCase] public GenericProductTypeSemantic GenericProductType = new GenericProductTypeSemantic();
		[PreserveCase] public ProductPassengerSemantic ProductPassenger = new ProductPassengerSemantic();
		[PreserveCase] public BusTicketSemantic BusTicket = new BusTicketSemantic();
		[PreserveCase] public ExcursionSemantic Excursion = new ExcursionSemantic();
		[PreserveCase] public IsicSemantic Isic = new IsicSemantic();
		[PreserveCase] public PasteboardSemantic Pasteboard = new PasteboardSemantic();
		[PreserveCase] public TransferSemantic Transfer = new TransferSemantic();
		[PreserveCase] public GenericProductSemantic GenericProduct = new GenericProductSemantic();
		[PreserveCase] public CarRentalSemantic CarRental = new CarRentalSemantic();
		[PreserveCase] public TourSemantic Tour = new TourSemantic();
		[PreserveCase] public AccommodationSemantic Accommodation = new AccommodationSemantic();
		[PreserveCase] public SimCardSemantic SimCard = new SimCardSemantic();
		[PreserveCase] public InsuranceSemantic Insurance = new InsuranceSemantic();
		[PreserveCase] public AviaDocumentSemantic AviaDocument = new AviaDocumentSemantic();
		[PreserveCase] public ProductSemantic Product = new ProductSemantic();
		[PreserveCase] public AviaTicketSemantic AviaTicket = new AviaTicketSemantic();
		[PreserveCase] public AviaMcoSemantic AviaMco = new AviaMcoSemantic();
		[PreserveCase] public AviaRefundSemantic AviaRefund = new AviaRefundSemantic();
		[PreserveCase] public CountrySemantic Country = new CountrySemantic();
		[PreserveCase] public AirlineServiceClassSemantic AirlineServiceClass = new AirlineServiceClassSemantic();
		[PreserveCase] public AirlineMonthCommissionSemantic AirlineMonthCommission = new AirlineMonthCommissionSemantic();
		[PreserveCase] public AirportSemantic Airport = new AirportSemantic();
		[PreserveCase] public PaymentSystemSemantic PaymentSystem = new PaymentSystemSemantic();
		[PreserveCase] public CurrencySemantic Currency = new CurrencySemantic();
		[PreserveCase] public IdentitySemantic Identity = new IdentitySemantic();
		[PreserveCase] public UserSemantic User = new UserSemantic();
		[PreserveCase] public UserVisitSemantic UserVisit = new UserVisitSemantic();
		[PreserveCase] public SystemConfigurationSemantic SystemConfiguration = new SystemConfigurationSemantic();
		[PreserveCase] public AirlineCommissionPercentsSemantic AirlineCommissionPercents = new AirlineCommissionPercentsSemantic();
		[PreserveCase] public DocumentOwnerSemantic DocumentOwner = new DocumentOwnerSemantic();
		[PreserveCase] public DocumentAccessSemantic DocumentAccess = new DocumentAccessSemantic();
		[PreserveCase] public ClosedPeriodSemantic ClosedPeriod = new ClosedPeriodSemantic();
		[PreserveCase] public OpeningBalanceSemantic OpeningBalance = new OpeningBalanceSemantic();
		[PreserveCase] public InternalTransferSemantic InternalTransfer = new InternalTransferSemantic();
		[PreserveCase] public GdsAgentSemantic GdsAgent = new GdsAgentSemantic();
		[PreserveCase] public GdsFileSemantic GdsFile = new GdsFileSemantic();
		[PreserveCase] public InvoiceSemantic Invoice = new InvoiceSemantic();
		[PreserveCase] public ReceiptSemantic Receipt = new ReceiptSemantic();
		[PreserveCase] public TaskSemantic Task = new TaskSemantic();
		[PreserveCase] public PaymentSemantic Payment = new PaymentSemantic();
		[PreserveCase] public ContractSemantic Contract = new ContractSemantic();
		[PreserveCase] public ModificationSemantic Modification = new ModificationSemantic();
		[PreserveCase] public FlightSegmentSemantic FlightSegment = new FlightSegmentSemantic();
		[PreserveCase] public AirplaneModelSemantic AirplaneModel = new AirplaneModelSemantic();
		[PreserveCase] public AmadeusAviaSftpRsaKeySemantic AmadeusAviaSftpRsaKey = new AmadeusAviaSftpRsaKeySemantic();
	}

}