module Luxena
{

	/** Владелец документов */
	export interface IDocumentOwnerSemantic extends IEntitySemantic
	{
		//se.IsActive,
		//se.LastChangedPropertyName,
		//se.Owner,

		/** Действующий */
		IsActive: SemanticMember;

		/** Владелец */
		Owner: SemanticMember;
	
	}

	/** Владелец документов */
	export class DocumentOwnerSemantic extends EntitySemantic implements IDocumentOwnerSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "DocumentOwner";
			this._names = "DocumentOwners";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Владелец документов", rus: "Владельцы документов" });

			this._getDerivedEntities = null;

			this._className = "DocumentOwner";
			this._getRootEntity = () => sd.DocumentOwner;
			this._store = db.DocumentOwners;
			this._saveStore = db.DocumentOwners;
			this._lookupFields = { id: "Id", name: "" };
			this.small();
		}

		_DocumentOwner = new SemanticMember()
			.localizeTitle({ ru: "Владелец документов", rus: "Владельцы документов" })
			.lookup(() => sd.DocumentOwner);
				

		/** Действующий */
		IsActive = this.member()
			.localizeTitle({ ru: "Действующий" })
			.bool()
			.required();

		/** Владелец */
		Owner = this.member()
			.localizeTitle({ ru: "Владелец" })
			.lookup(() => sd.Party)
			.entityName();
	

	}


	
	export interface IFileSemantic extends IEntitySemantic
	{
		//se.FileName,
		//se.TimeStamp,
		//se.Content,
		//se.LastChangedPropertyName,
		//se.Party,
		//se.UploadedBy,

		FileName: SemanticMember;

		TimeStamp: SemanticMember;

		Content: SemanticMember;

		/** Контрагент */
		Party: SemanticMember;

		UploadedBy: SemanticMember;
	
	}

	
	export class FileSemantic extends EntitySemantic implements IFileSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "File";
			this._names = "Files";
			
			this._isEntity = true;

			this._getDerivedEntities = null;

			this._className = "File";
			this._getRootEntity = () => sd.File;
			this._store = db.Files;
			this._saveStore = db.Files;
			this._lookupFields = { id: "Id", name: "" };
		}

		_File = new SemanticMember()
			.lookup(() => sd.File);
				

		FileName = this.member()
			.string();

		TimeStamp = this.member()
			.date()
			.required();

		Content = this.member()
			;

		/** Контрагент */
		Party = this.member()
			.localizeTitle({ ru: "Контрагент", rus: "Контрагенты" })
			.lookup(() => sd.Party)
			.required();

		UploadedBy = this.member()
			.lookup(() => sd.Person);
	

	}


	/** Инвойс */
	export interface IInvoiceSemantic extends IEntitySemantic
	{
		//se.Type,
		//se.IssueDate,
		//se.Number,
		//se.Agreement,
		//se.TimeStamp,
		//se.Content,
		//se.Total,
		//se.Vat,
		//se.LastChangedPropertyName,
		//se.Order,
		//se.IssuedBy,

		/** Тип */
		Type: SemanticMember;

		/** Дата выпуска */
		IssueDate: SemanticMember;

		/** Номер */
		Number: SemanticMember;

		/** Договор */
		Agreement: SemanticMember;

		/** Дата создания */
		TimeStamp: SemanticMember;

		Content: SemanticMember;

		/** Итого */
		Total: SemanticMember;

		/** В т.ч. НДС */
		Vat: SemanticMember;

		/** Заказ */
		Order: SemanticMember;

		/** Выпустил */
		IssuedBy: SemanticMember;
		
		Payments: SemanticMember;
	
	}

	/** Инвойс */
	export class InvoiceSemantic extends EntitySemantic implements IInvoiceSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Invoice";
			this._names = "Invoices";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Инвойс", rus: "Счета/квитанции" });

			this._getDerivedEntities = () => [
				sd.Receipt
			];

			this._className = "Invoice";
			this._getRootEntity = () => sd.Invoice;
			this._store = db.Invoices;
			this._saveStore = db.Invoices;
			this._lookupStore = db.InvoiceLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_Invoice = new SemanticMember()
			.localizeTitle({ ru: "Инвойс", rus: "Счета/квитанции" })
			.lookup(() => sd.Invoice);
				

		/** Тип */
		Type = this.member()
			.localizeTitle({ ru: "Тип" })
			.enum(InvoiceType)
			.required()
			.entityType();

		/** Дата выпуска */
		IssueDate = this.member()
			.localizeTitle({ ru: "Дата выпуска" })
			.date()
			.required()
			.entityDate();

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.entityName();

		/** Договор */
		Agreement = this.member()
			.localizeTitle({ ru: "Договор" })
			.string();

		/** Дата создания */
		TimeStamp = this.member()
			.localizeTitle({ ru: "Дата создания" })
			.dateTime2()
			.required()
			.utility();

		Content = this.member()
			;

		/** Итого */
		Total = this.member()
			.localizeTitle({ ru: "Итого" })
			.money();

		/** В т.ч. НДС */
		Vat = this.member()
			.localizeTitle({ ru: "В т.ч. НДС" })
			.money();

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);

		/** Выпустил */
		IssuedBy = this.member()
			.localizeTitle({ ru: "Выпустил", rus: "Агенты" })
			.lookup(() => sd.Person);

	
		Payments = this.collection(() => sd.Payment, se => se.Invoice);
	

	}


	/** Выпущенная накладная */
	export interface IIssuedConsignmentSemantic extends IEntitySemantic
	{
		//se.Number,
		//se.TimeStamp,
		//se.Content,
		//se.LastChangedPropertyName,
		//se.Consignment,
		//se.IssuedBy,

		/** Название */
		Number: SemanticMember;

		TimeStamp: SemanticMember;

		Content: SemanticMember;

		/** Накладная */
		Consignment: SemanticMember;

		/** Выпустил */
		IssuedBy: SemanticMember;
	
	}

	/** Выпущенная накладная */
	export class IssuedConsignmentSemantic extends EntitySemantic implements IIssuedConsignmentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "IssuedConsignment";
			this._names = "IssuedConsignments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Выпущенная накладная", rus: "Выпущенные накладные" });

			this._getDerivedEntities = null;

			this._className = "IssuedConsignment";
			this._getRootEntity = () => sd.IssuedConsignment;
			this._store = db.IssuedConsignments;
			this._saveStore = db.IssuedConsignments;
			this._lookupStore = db.IssuedConsignmentLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_IssuedConsignment = new SemanticMember()
			.localizeTitle({ ru: "Выпущенная накладная", rus: "Выпущенные накладные" })
			.lookup(() => sd.IssuedConsignment);
				

		/** Название */
		Number = this.member()
			.localizeTitle({ ru: "Название" })
			.string()
			.entityName();

		TimeStamp = this.member()
			.dateTime2()
			.required()
			.entityDate();

		Content = this.member()
			;

		/** Накладная */
		Consignment = this.member()
			.localizeTitle({ ru: "Накладная", rus: "Накладные" })
			.lookup(() => sd.Consignment);

		/** Выпустил */
		IssuedBy = this.member()
			.localizeTitle({ ru: "Выпустил", rus: "Агенты" })
			.lookup(() => sd.Person);
	

	}


	
	export interface ISequenceSemantic extends IEntitySemantic
	{
		//se.Name,
		//se.Discriminator,
		//se.Current,
		//se.Format,
		//se.Timestamp,
		//se.LastChangedPropertyName,

		Name: SemanticMember;

		Discriminator: SemanticMember;

		Current: SemanticMember;

		Format: SemanticMember;

		Timestamp: SemanticMember;
	
	}

	
	export class SequenceSemantic extends EntitySemantic implements ISequenceSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Sequence";
			this._names = "Sequences";
			
			this._isEntity = true;

			this._getDerivedEntities = null;

			this._className = "Sequence";
			this._getRootEntity = () => sd.Sequence;
			this._store = db.Sequences;
			this._saveStore = db.Sequences;
			this._lookupStore = db.SequenceLookup;
			this._lookupFields = { id: "Id", name: "Name" };
		}

		_Sequence = new SemanticMember()
			.lookup(() => sd.Sequence);
				

		Name = this.member()
			.string()
			.entityName();

		Discriminator = this.member()
			.string();

		Current = this.member()
			.int()
			.required();

		Format = this.member()
			.string();

		Timestamp = this.member()
			.date()
			.required();
	

	}


	/** Настройки системы */
	export interface ISystemConfigurationSemantic extends IEntitySemantic
	{
		//se.CompanyName,
		//se.CompanyDetails,
		//se.DefaultCurrency,
		//se.UseDefaultCurrencyForInput,
		//se.VatRate,
		//se.AmadeusRizUsingMode,
		//se.IsPassengerPassportRequired,
		//se.AviaOrderItemGenerationOption,
		//se.AllowAgentSetOrderVat,
		//se.UseAviaDocumentVatInOrder,
		//se.AviaDocumentVatOptions,
		//se.AccountantDisplayString,
		//se.IncomingCashOrderCorrespondentAccount,
		//se.SeparateDocumentAccess,
		//se.IsOrganizationCodeRequired,
		//se.UseAviaHandling,
		//se.DaysBeforeDeparture,
		//se.IsOrderRequiredForProcessedDocument,
		//se.MetricsFromDate,
		//se.ReservationsInOfficeMetrics,
		//se.McoRequiresDescription,
		//se.NeutralAirlineCode,
		//se.Order_UseServiceFeeOnlyInVat,
		//se.Invoice_NumberMode,
		//se.InvoicePrinter_FooterDetails,
		//se.GalileoWebService_LoadedOn,
		//se.LastChangedPropertyName,
		//se.Company,
		//se.Country,
		//se.BirthdayTaskResponsible,

		ModifiedOn: SemanticMember;

		ModifiedBy: SemanticMember;

		CompanyName: SemanticMember;

		/** Реквизиты организации */
		CompanyDetails: SemanticMember;

		/** Валюта по умолчанию */
		DefaultCurrency: SemanticMember;

		/** Использовать только валюту по умолчанию */
		UseDefaultCurrencyForInput: SemanticMember;

		/** Ставка НДС,% */
		VatRate: SemanticMember;

		/** Использование riz-данных в Amadeus Air */
		AmadeusRizUsingMode: SemanticMember;

		/** Требование паспортных данных авиакомпаниями */
		IsPassengerPassportRequired: SemanticMember;

		/** Формирование номенклатур заказа по авиадокументу */
		AviaOrderItemGenerationOption: SemanticMember;

		/** Разрешить редактирование НДС в заказе */
		AllowAgentSetOrderVat: SemanticMember;

		/** Использовать НДС авиадокумента в заказе */
		UseAviaDocumentVatInOrder: SemanticMember;

		/** Расчет НДС в авиадокументе */
		AviaDocumentVatOptions: SemanticMember;

		/** Текст поля "Главный бухгалтер" */
		AccountantDisplayString: SemanticMember;

		/** Корреспондентский счет для ПКО */
		IncomingCashOrderCorrespondentAccount: SemanticMember;

		/** Разделять доступ к документам */
		SeparateDocumentAccess: SemanticMember;

		/** Обязательное ЕДРПОУ для организаций */
		IsOrganizationCodeRequired: SemanticMember;

		/** Использовать доп. доход от АК при обработке авиадокументов */
		UseAviaHandling: SemanticMember;

		/** Дней до отправления */
		DaysBeforeDeparture: SemanticMember;

		/** Заказ обязателен для обработки документов */
		IsOrderRequiredForProcessedDocument: SemanticMember;

		/** Показывать статистику на главной страницы с */
		MetricsFromDate: SemanticMember;

		/** Бронировки в документах по офису */
		ReservationsInOfficeMetrics: SemanticMember;

		/** Описание обязательно для MCO */
		McoRequiresDescription: SemanticMember;

		/** Neutral Airline Code */
		NeutralAirlineCode: SemanticMember;

		/** Заказы: НДС только от сервисного сбора */
		Order_UseServiceFeeOnlyInVat: SemanticMember;

		/** Инвойсы: новый номер */
		Invoice_NumberMode: SemanticMember;

		/** Инвойсы: важное примечание */
		InvoicePrinter_FooterDetails: SemanticMember;

		GalileoWebService_LoadedOn: SemanticMember;

		/** Организация */
		Company: SemanticMember;

		/** Страна */
		Country: SemanticMember;

		/** Ответственный за поздравление именинников */
		BirthdayTaskResponsible: SemanticMember;
	
	}

	/** Настройки системы */
	export class SystemConfigurationSemantic extends EntitySemantic implements ISystemConfigurationSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "SystemConfiguration";
			this._names = "SystemConfigurations";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Настройки системы" });

			this._getDerivedEntities = null;

			this._className = "SystemConfiguration";
			this._getRootEntity = () => sd.SystemConfiguration;
			this._store = db.SystemConfigurations;
			this._saveStore = db.SystemConfigurations;
			this._lookupFields = { id: "Id", name: "" };
		}

		_SystemConfiguration = new SemanticMember()
			.localizeTitle({ ru: "Настройки системы" })
			.lookup(() => sd.SystemConfiguration);
				

		ModifiedOn = this.member()
			.date();

		ModifiedBy = this.member()
			.string();

		CompanyName = this.member()
			.string()
			.calculated()
			.nonsaved();

		/** Реквизиты организации */
		CompanyDetails = this.member()
			.localizeTitle({ ru: "Реквизиты организации" })
			.text(3);

		/** Валюта по умолчанию */
		DefaultCurrency = this.member()
			.localizeTitle({ ru: "Валюта по умолчанию" })
			.string();

		/** Использовать только валюту по умолчанию */
		UseDefaultCurrencyForInput = this.member()
			.localizeTitle({ ru: "Использовать только валюту по умолчанию" })
			.bool()
			.required();

		/** Ставка НДС,% */
		VatRate = this.member()
			.localizeTitle({ ru: "Ставка НДС,%" })
			.float()
			.required();

		/** Использование riz-данных в Amadeus Air */
		AmadeusRizUsingMode = this.member()
			.localizeTitle({ ru: "Использование riz-данных в Amadeus Air" })
			.enum(AmadeusRizUsingMode)
			.required();

		/** Требование паспортных данных авиакомпаниями */
		IsPassengerPassportRequired = this.member()
			.localizeTitle({ ru: "Требование паспортных данных авиакомпаниями" })
			.bool()
			.required();

		/** Формирование номенклатур заказа по авиадокументу */
		AviaOrderItemGenerationOption = this.member()
			.localizeTitle({ ru: "Формирование номенклатур заказа по авиадокументу" })
			.enum(ProductOrderItemGenerationOption)
			.required();

		/** Разрешить редактирование НДС в заказе */
		AllowAgentSetOrderVat = this.member()
			.localizeTitle({ ru: "Разрешить редактирование НДС в заказе" })
			.bool()
			.required();

		/** Использовать НДС авиадокумента в заказе */
		UseAviaDocumentVatInOrder = this.member()
			.localizeTitle({ ru: "Использовать НДС авиадокумента в заказе" })
			.bool()
			.required();

		/** Расчет НДС в авиадокументе */
		AviaDocumentVatOptions = this.member()
			.localizeTitle({ ru: "Расчет НДС в авиадокументе" })
			.enum(AviaDocumentVatOptions)
			.required();

		/** Текст поля "Главный бухгалтер" */
		AccountantDisplayString = this.member()
			.localizeTitle({ ru: "Текст поля \"Главный бухгалтер\"" })
			.text(3);

		/** Корреспондентский счет для ПКО */
		IncomingCashOrderCorrespondentAccount = this.member()
			.localizeTitle({ ru: "Корреспондентский счет для ПКО" })
			.string();

		/** Разделять доступ к документам */
		SeparateDocumentAccess = this.member()
			.localizeTitle({ ru: "Разделять доступ к документам" })
			.bool()
			.required();

		/** Обязательное ЕДРПОУ для организаций */
		IsOrganizationCodeRequired = this.member()
			.localizeTitle({ ru: "Обязательное ЕДРПОУ для организаций" })
			.bool()
			.required();

		/** Использовать доп. доход от АК при обработке авиадокументов */
		UseAviaHandling = this.member()
			.localizeTitle({ ru: "Использовать доп. доход от АК при обработке авиадокументов" })
			.bool()
			.required();

		/** Дней до отправления */
		DaysBeforeDeparture = this.member()
			.localizeTitle({ ru: "Дней до отправления" })
			.int()
			.required();

		/** Заказ обязателен для обработки документов */
		IsOrderRequiredForProcessedDocument = this.member()
			.localizeTitle({ ru: "Заказ обязателен для обработки документов" })
			.bool()
			.required();

		/** Показывать статистику на главной страницы с */
		MetricsFromDate = this.member()
			.localizeTitle({ ru: "Показывать статистику на главной страницы с" })
			.date();

		/** Бронировки в документах по офису */
		ReservationsInOfficeMetrics = this.member()
			.localizeTitle({ ru: "Бронировки в документах по офису" })
			.bool()
			.required();

		/** Описание обязательно для MCO */
		McoRequiresDescription = this.member()
			.localizeTitle({ ru: "Описание обязательно для MCO" })
			.bool()
			.required();

		/** Neutral Airline Code */
		NeutralAirlineCode = this.member()
			.localizeTitle({ ru: "Neutral Airline Code" })
			.string();

		/** Заказы: НДС только от сервисного сбора */
		Order_UseServiceFeeOnlyInVat = this.member()
			.localizeTitle({ ru: "Заказы: НДС только от сервисного сбора" })
			.bool()
			.required();

		/** Инвойсы: новый номер */
		Invoice_NumberMode = this.member()
			.localizeTitle({ ru: "Инвойсы: новый номер" })
			.enum(InvoiceNumberMode)
			.required();

		/** Инвойсы: важное примечание */
		InvoicePrinter_FooterDetails = this.member()
			.localizeTitle({ ru: "Инвойсы: важное примечание" })
			.text(6);

		GalileoWebService_LoadedOn = this.member()
			.date();

		/** Организация */
		Company = this.member()
			.localizeTitle({ ru: "Организация" })
			.lookup(() => sd.Organization);

		/** Страна */
		Country = this.member()
			.localizeTitle({ ru: "Страна", rus: "Страны" })
			.lookup(() => sd.Country);

		/** Ответственный за поздравление именинников */
		BirthdayTaskResponsible = this.member()
			.localizeTitle({ ru: "Ответственный за поздравление именинников" })
			.lookup(() => sd.Person);
	

	}


	/** Сервис-класс авиакомпании */
	export interface IAirlineServiceClassSemantic extends IEntity2Semantic
	{
		//se.Code,
		//se.ServiceClass,
		//se.LastChangedPropertyName,
		//se.Airline,

		/** Код */
		Code: SemanticMember;

		/** Сервис-класс */
		ServiceClass: SemanticMember;

		/** Авиакомпания */
		Airline: SemanticMember;
	
	}

	/** Сервис-класс авиакомпании */
	export class AirlineServiceClassSemantic extends Entity2Semantic implements IAirlineServiceClassSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "AirlineServiceClass";
			this._names = "AirlineServiceClasses";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Сервис-класс авиакомпании", rus: "Сервис-классы авиакомпаний" });

			this._getDerivedEntities = null;

			this._className = "AirlineServiceClass";
			this._getRootEntity = () => sd.AirlineServiceClass;
			this._store = db.AirlineServiceClasses;
			this._saveStore = db.AirlineServiceClasses;
			this._lookupStore = db.AirlineServiceClassLookup;
			this._lookupFields = { id: "Id", name: "Code" };
		}

		_AirlineServiceClass = new SemanticMember()
			.localizeTitle({ ru: "Сервис-класс авиакомпании", rus: "Сервис-классы авиакомпаний" })
			.lookup(() => sd.AirlineServiceClass);
				

		/** Код */
		Code = this.member()
			.localizeTitle({ ru: "Код" })
			.string()
			.required()
			.entityName();

		/** Сервис-класс */
		ServiceClass = this.member()
			.localizeTitle({ ru: "Сервис-класс" })
			.enum(ServiceClass)
			.required();

		/** Авиакомпания */
		Airline = this.member()
			.localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" })
			.lookup(() => sd.Organization)
			.required();
	

	}


	/** Накладная */
	export interface IConsignmentSemantic extends IEntity2Semantic
	{
		//se.Number,
		//se.IssueDate,
		//se.Order,
		//se.GrandTotal,
		//se.Vat,
		//se.Total,
		//se.Discount,
		//se.TotalSupplied,
		//se.LastChangedPropertyName,
		//se.Supplier,
		//se.Acquirer,

		/** Номер */
		Number: SemanticMember;

		/** Дата выпуска */
		IssueDate: SemanticMember;

		/** Заказ */
		Order: SemanticMember;

		/** Сумма с НДС */
		GrandTotal: SemanticMember;

		/** В т.ч. НДС */
		Vat: SemanticMember;

		/** Сумма без НДС */
		Total: SemanticMember;

		/** Скидка */
		Discount: SemanticMember;

		/** Всего отпущено */
		TotalSupplied: SemanticMember;

		/** Отпущено */
		Supplier: SemanticMember;

		/** Получено */
		Acquirer: SemanticMember;
		
		OrderItems: SemanticMember;
		
		IssuedConsignments: SemanticMember;
	
	}

	/** Накладная */
	export class ConsignmentSemantic extends Entity2Semantic implements IConsignmentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Consignment";
			this._names = "Consignments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Накладная", rus: "Накладные" });

			this._getDerivedEntities = null;

			this._className = "Consignment";
			this._getRootEntity = () => sd.Consignment;
			this._store = db.Consignments;
			this._saveStore = db.Consignments;
			this._lookupStore = db.ConsignmentLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_Consignment = new SemanticMember()
			.localizeTitle({ ru: "Накладная", rus: "Накладные" })
			.lookup(() => sd.Consignment);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.length(12, 0, 0)
			.entityName();

		/** Дата выпуска */
		IssueDate = this.member()
			.localizeTitle({ ru: "Дата выпуска" })
			.date()
			.required()
			.entityDate();

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order)
			.calculated()
			.nonsaved();

		/** Сумма с НДС */
		GrandTotal = this.member()
			.localizeTitle({ ru: "Сумма с НДС" })
			.money();

		/** В т.ч. НДС */
		Vat = this.member()
			.localizeTitle({ ru: "В т.ч. НДС" })
			.money();

		/** Сумма без НДС */
		Total = this.member()
			.localizeTitle({ ru: "Сумма без НДС" })
			.money()
			.calculated()
			.nonsaved();

		/** Скидка */
		Discount = this.member()
			.localizeTitle({ ru: "Скидка" })
			.money();

		/** Всего отпущено */
		TotalSupplied = this.member()
			.localizeTitle({ ru: "Всего отпущено" })
			.string();

		/** Отпущено */
		Supplier = this.member()
			.localizeTitle({ ru: "Отпущено" })
			.lookup(() => sd.Party);

		/** Получено */
		Acquirer = this.member()
			.localizeTitle({ ru: "Получено" })
			.lookup(() => sd.Party);

	
		OrderItems = this.collection(() => sd.OrderItem, se => se.Consignment);

	
		IssuedConsignments = this.collection(() => sd.IssuedConsignment, se => se.Consignment);
	

	}


	/** Курс валюты */
	export interface ICurrencyDailyRateSemantic extends IEntity2Semantic
	{
		//se.Date,
		//se.UAH_EUR,
		//se.UAH_RUB,
		//se.UAH_USD,
		//se.RUB_EUR,
		//se.RUB_USD,
		//se.EUR_USD,
		//se.LastChangedPropertyName,

		/** Дата */
		Date: SemanticMember;

		/** UAH/EUR */
		UAH_EUR: SemanticMember;

		/** UAH/RUB */
		UAH_RUB: SemanticMember;

		/** UAH/USD */
		UAH_USD: SemanticMember;

		/** RUB/EUR */
		RUB_EUR: SemanticMember;

		/** RUB/USD */
		RUB_USD: SemanticMember;

		/** EUR/USD */
		EUR_USD: SemanticMember;
	
	}

	/** Курс валюты */
	export class CurrencyDailyRateSemantic extends Entity2Semantic implements ICurrencyDailyRateSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "CurrencyDailyRate";
			this._names = "CurrencyDailyRates";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Курс валюты", rus: "Курсы валют" });

			this._getDerivedEntities = null;

			this._className = "CurrencyDailyRate";
			this._getRootEntity = () => sd.CurrencyDailyRate;
			this._store = db.CurrencyDailyRates;
			this._saveStore = db.CurrencyDailyRates;
			this._lookupFields = { id: "Id", name: "" };
		}

		_CurrencyDailyRate = new SemanticMember()
			.localizeTitle({ ru: "Курс валюты", rus: "Курсы валют" })
			.lookup(() => sd.CurrencyDailyRate);
				

		/** Дата */
		Date = this.member()
			.localizeTitle({ en: "Date", ru: "Дата" })
			.date()
			.required()
			.entityDate()
			.entityName()
			.unique();

		/** UAH/EUR */
		UAH_EUR = this.member()
			.localizeTitle({ ru: "UAH/EUR" })
			.float();

		/** UAH/RUB */
		UAH_RUB = this.member()
			.localizeTitle({ ru: "UAH/RUB" })
			.float();

		/** UAH/USD */
		UAH_USD = this.member()
			.localizeTitle({ ru: "UAH/USD" })
			.float();

		/** RUB/EUR */
		RUB_EUR = this.member()
			.localizeTitle({ ru: "RUB/EUR" })
			.float();

		/** RUB/USD */
		RUB_USD = this.member()
			.localizeTitle({ ru: "RUB/USD" })
			.float();

		/** EUR/USD */
		EUR_USD = this.member()
			.localizeTitle({ ru: "EUR/USD" })
			.float();
	

	}


	/** Доступ к документам */
	export interface IDocumentAccessSemantic extends IEntity2Semantic
	{
		//se.FullDocumentControl,
		//se.LastChangedPropertyName,
		//se.Person,
		//se.Owner,

		/** Полный доступ */
		FullDocumentControl: SemanticMember;

		/** Персона */
		Person: SemanticMember;

		/** Владелец */
		Owner: SemanticMember;
	
	}

	/** Доступ к документам */
	export class DocumentAccessSemantic extends Entity2Semantic implements IDocumentAccessSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "DocumentAccess";
			this._names = "DocumentAccesses";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Доступ к документам" });

			this._getDerivedEntities = null;

			this._className = "DocumentAccess";
			this._getRootEntity = () => sd.DocumentAccess;
			this._store = db.DocumentAccesses;
			this._saveStore = db.DocumentAccesses;
			this._lookupFields = { id: "Id", name: "" };
		}

		_DocumentAccess = new SemanticMember()
			.localizeTitle({ ru: "Доступ к документам" })
			.lookup(() => sd.DocumentAccess);
				

		/** Полный доступ */
		FullDocumentControl = this.member()
			.localizeTitle({ ru: "Полный доступ" })
			.bool()
			.required();

		/** Персона */
		Person = this.member()
			.localizeTitle({ ru: "Персона", rus: "Персоны" })
			.lookup(() => sd.Person)
			.entityName();

		/** Владелец */
		Owner = this.member()
			.localizeTitle({ ru: "Владелец" })
			.lookup(() => sd.Party);
	

	}


	/** Полетный сегмент */
	export interface IFlightSegmentSemantic extends IEntity2Semantic
	{
		//se.Name,
		//se.Position,
		//se.Type,
		//se.FromAirportCode,
		//se.FromAirportName,
		//se.ToAirportCode,
		//se.ToAirportName,
		//se.CarrierIataCode,
		//se.CarrierPrefixCode,
		//se.CarrierName,
		//se.FlightNumber,
		//se.ServiceClassCode,
		//se.ServiceClass,
		//se.DepartureTime,
		//se.ArrivalTime,
		//se.MealCodes,
		//se.MealTypes,
		//se.NumberOfStops,
		//se.Luggage,
		//se.CheckInTerminal,
		//se.CheckInTime,
		//se.Duration,
		//se.ArrivalTerminal,
		//se.Seat,
		//se.FareBasis,
		//se.Stopover,
		//se.Surcharges,
		//se.IsInclusive,
		//se.Fare,
		//se.StopoverOrTransferCharge,
		//se.IsSideTrip,
		//se.Distance,
		//se.Amount,
		//se.CouponAmount,
		//se.LastChangedPropertyName,
		//se.Ticket,
		//se.FromAirport,
		//se.ToAirport,
		//se.Carrier,

		Name: SemanticMember;

		/** № позиции */
		Position: SemanticMember;

		Type: SemanticMember;

		/** Из аэропорта (код) */
		FromAirportCode: SemanticMember;

		/** Из аэропорта (название) */
		FromAirportName: SemanticMember;

		/** В аэропорт (код) */
		ToAirportCode: SemanticMember;

		/** В аэропорт (название) */
		ToAirportName: SemanticMember;

		CarrierIataCode: SemanticMember;

		CarrierPrefixCode: SemanticMember;

		CarrierName: SemanticMember;

		/** Рейс */
		FlightNumber: SemanticMember;

		/** Код сервис-класса */
		ServiceClassCode: SemanticMember;

		ServiceClass: SemanticMember;

		/** Дата/время отправления */
		DepartureTime: SemanticMember;

		/** Дата/время прибытия */
		ArrivalTime: SemanticMember;

		/** Коды питания */
		MealCodes: SemanticMember;

		/** Питание */
		MealTypes: SemanticMember;

		/** Остановки */
		NumberOfStops: SemanticMember;

		/** Багаж */
		Luggage: SemanticMember;

		/** Терминал отправления */
		CheckInTerminal: SemanticMember;

		/** Регистрация */
		CheckInTime: SemanticMember;

		/** Перелет */
		Duration: SemanticMember;

		/** Терминал прибытия */
		ArrivalTerminal: SemanticMember;

		/** Место */
		Seat: SemanticMember;

		/** База тарифа */
		FareBasis: SemanticMember;

		/** Это конечный пункт */
		Stopover: SemanticMember;

		Surcharges: SemanticMember;

		IsInclusive: SemanticMember;

		Fare: SemanticMember;

		StopoverOrTransferCharge: SemanticMember;

		IsSideTrip: SemanticMember;

		/** Расстояние, км */
		Distance: SemanticMember;

		Amount: SemanticMember;

		CouponAmount: SemanticMember;

		/** Авиабилет */
		Ticket: SemanticMember;

		/** Из аэропорта */
		FromAirport: SemanticMember;

		/** В аэропорт */
		ToAirport: SemanticMember;

		/** Перевозчик */
		Carrier: SemanticMember;
	
	}

	/** Полетный сегмент */
	export class FlightSegmentSemantic extends Entity2Semantic implements IFlightSegmentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "FlightSegment";
			this._names = "FlightSegments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Полетный сегмент", rus: "Полетные сегменты" });

			this._getDerivedEntities = null;

			this._className = "FlightSegment";
			this._getRootEntity = () => sd.FlightSegment;
			this._store = db.FlightSegments;
			this._saveStore = db.FlightSegments;
			this._lookupStore = db.FlightSegmentLookup;
			this._lookupFields = { id: "Id", name: "Name" };
		}

		_FlightSegment = new SemanticMember()
			.localizeTitle({ ru: "Полетный сегмент", rus: "Полетные сегменты" })
			.lookup(() => sd.FlightSegment);
				

		Name = this.member()
			.string()
			.calculated()
			.nonsaved()
			.entityName();

		/** № позиции */
		Position = this.member()
			.localizeTitle({ ru: "№ позиции" })
			.int()
			.required()
			.entityPosition();

		Type = this.member()
			.enum(FlightSegmentType)
			.required();

		/** Из аэропорта (код) */
		FromAirportCode = this.member()
			.localizeTitle({ ru: "Из аэропорта (код)", ruShort: "код" })
			.emptyText("код")
			.string();

		/** Из аэропорта (название) */
		FromAirportName = this.member()
			.localizeTitle({ ru: "Из аэропорта (название)", ruShort: "название" })
			.emptyText("название")
			.string();

		/** В аэропорт (код) */
		ToAirportCode = this.member()
			.localizeTitle({ ru: "В аэропорт (код)", ruShort: "код" })
			.emptyText("код")
			.string();

		/** В аэропорт (название) */
		ToAirportName = this.member()
			.localizeTitle({ ru: "В аэропорт (название)", ruShort: "название" })
			.emptyText("название")
			.string();

		CarrierIataCode = this.member()
			.string();

		CarrierPrefixCode = this.member()
			.string();

		CarrierName = this.member()
			.string();

		/** Рейс */
		FlightNumber = this.member()
			.localizeTitle({ ru: "Рейс" })
			.string()
			.length(4, 0, 0);

		/** Код сервис-класса */
		ServiceClassCode = this.member()
			.localizeTitle({ ru: "Код сервис-класса" })
			.string();

		ServiceClass = this.member()
			.localizeTitle({ ru: "Сервис-класс" })
			.enum(ServiceClass);

		/** Дата/время отправления */
		DepartureTime = this.member()
			.localizeTitle({ ru: "Дата/время отправления" })
			.dateTime();

		/** Дата/время прибытия */
		ArrivalTime = this.member()
			.localizeTitle({ ru: "Дата/время прибытия" })
			.dateTime();

		/** Коды питания */
		MealCodes = this.member()
			.localizeTitle({ ru: "Коды питания" })
			.string();

		/** Питание */
		MealTypes = this.member()
			.localizeTitle({ ru: "Питание" })
			.enum(MealType);

		/** Остановки */
		NumberOfStops = this.member()
			.localizeTitle({ ru: "Остановки" })
			.int();

		/** Багаж */
		Luggage = this.member()
			.localizeTitle({ ru: "Багаж" })
			.string()
			.length(3, 0, 0);

		/** Терминал отправления */
		CheckInTerminal = this.member()
			.localizeTitle({ ru: "Терминал отправления", ruShort: "терминал" })
			.emptyText("терминал")
			.string();

		/** Регистрация */
		CheckInTime = this.member()
			.localizeTitle({ ru: "Регистрация" })
			.string();

		/** Перелет */
		Duration = this.member()
			.localizeTitle({ ru: "Перелет" })
			.string()
			.length(4, 0, 0);

		/** Терминал прибытия */
		ArrivalTerminal = this.member()
			.localizeTitle({ ru: "Терминал прибытия", ruShort: "терминал" })
			.emptyText("терминал")
			.string();

		/** Место */
		Seat = this.member()
			.localizeTitle({ ru: "Место" })
			.string()
			.length(3, 0, 0);

		/** База тарифа */
		FareBasis = this.member()
			.localizeTitle({ ru: "База тарифа" })
			.string()
			.length(10, 0, 0);

		/** Это конечный пункт */
		Stopover = this.member()
			.localizeTitle({ ru: "Это конечный пункт" })
			.bool()
			.required();

		Surcharges = this.member()
			.float();

		IsInclusive = this.member()
			.bool()
			.required();

		Fare = this.member()
			.float();

		StopoverOrTransferCharge = this.member()
			.float();

		IsSideTrip = this.member()
			.bool()
			.required();

		/** Расстояние, км */
		Distance = this.member()
			.localizeTitle({ ru: "Расстояние, км" })
			.float()
			.required();

		Amount = this.member()
			.money();

		CouponAmount = this.member()
			.money();

		/** Авиабилет */
		Ticket = this.member()
			.localizeTitle({ ru: "Авиабилет", rus: "Авиабилеты", ua: "Авіаквиток" })
			.lookup(() => sd.AviaTicket);

		/** Из аэропорта */
		FromAirport = this.member()
			.localizeTitle({ ru: "Из аэропорта" })
			.lookup(() => sd.Airport);

		/** В аэропорт */
		ToAirport = this.member()
			.localizeTitle({ ru: "В аэропорт" })
			.lookup(() => sd.Airport);

		/** Перевозчик */
		Carrier = this.member()
			.localizeTitle({ ru: "Перевозчик" })
			.lookup(() => sd.Organization);
	

	}


	/** Gds-агент */
	export interface IGdsAgentSemantic extends IEntity2Semantic
	{
		//se.Name,
		//se.Codes,
		//se.Origin,
		//se.Code,
		//se.OfficeCode,
		//se.LastChangedPropertyName,
		//se.Person,
		//se.Office,

		/** Название */
		Name: SemanticMember;

		Codes: SemanticMember;

		/** Источник документов */
		Origin: SemanticMember;

		/** Код агента */
		Code: SemanticMember;

		/** Код офиса */
		OfficeCode: SemanticMember;

		/** Персона */
		Person: SemanticMember;

		/** Владелец */
		Office: SemanticMember;
	
	}

	/** Gds-агент */
	export class GdsAgentSemantic extends Entity2Semantic implements IGdsAgentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "GdsAgent";
			this._names = "GdsAgents";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Gds-агент", rus: "Gds-агенты" });

			this._getDerivedEntities = null;

			this._className = "GdsAgent";
			this._getRootEntity = () => sd.GdsAgent;
			this._store = db.GdsAgents;
			this._saveStore = db.GdsAgents;
			this._lookupStore = db.GdsAgentLookup;
			this._lookupFields = { id: "Id", name: "Name" };
		}

		_GdsAgent = new SemanticMember()
			.localizeTitle({ ru: "Gds-агент", rus: "Gds-агенты" })
			.lookup(() => sd.GdsAgent);
				

		/** Название */
		Name = this.member()
			.localizeTitle({ ru: "Название" })
			.string()
			.calculated()
			.nonsaved()
			.entityName();

		Codes = this.member()
			.string()
			.calculated()
			.nonsaved();

		/** Источник документов */
		Origin = this.member()
			.localizeTitle({ ru: "Источник документов" })
			.enum(ProductOrigin)
			.required();

		/** Код агента */
		Code = this.member()
			.localizeTitle({ ru: "Код агента" })
			.string();

		/** Код офиса */
		OfficeCode = this.member()
			.localizeTitle({ ru: "Код офиса" })
			.string();

		/** Персона */
		Person = this.member()
			.localizeTitle({ ru: "Персона", rus: "Персоны" })
			.lookup(() => sd.Person);

		/** Владелец */
		Office = this.member()
			.localizeTitle({ ru: "Владелец" })
			.lookup(() => sd.Party);
	

	}


	/** Внутренний перевод */
	export interface IInternalTransferSemantic extends IEntity2Semantic
	{
		//se.Number,
		//se.Date,
		//se.Amount,
		//se.LastChangedPropertyName,
		//se.FromOrder,
		//se.FromParty,
		//se.ToOrder,
		//se.ToParty,

		/** Номер */
		Number: SemanticMember;

		/** Дата */
		Date: SemanticMember;

		/** Сумма */
		Amount: SemanticMember;

		/** Из заказа */
		FromOrder: SemanticMember;

		/** От контрагента */
		FromParty: SemanticMember;

		/** В заказ */
		ToOrder: SemanticMember;

		/** К контрагенту */
		ToParty: SemanticMember;
	
	}

	/** Внутренний перевод */
	export class InternalTransferSemantic extends Entity2Semantic implements IInternalTransferSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "InternalTransfer";
			this._names = "InternalTransfers";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Внутренний перевод", rus: "Внутренние переводы" });

			this._getDerivedEntities = null;

			this._className = "InternalTransfer";
			this._getRootEntity = () => sd.InternalTransfer;
			this._store = db.InternalTransfers;
			this._saveStore = db.InternalTransfers;
			this._lookupStore = db.InternalTransferLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_InternalTransfer = new SemanticMember()
			.localizeTitle({ ru: "Внутренний перевод", rus: "Внутренние переводы" })
			.lookup(() => sd.InternalTransfer);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.entityName();

		/** Дата */
		Date = this.member()
			.localizeTitle({ en: "Date", ru: "Дата" })
			.date()
			.required()
			.entityDate();

		/** Сумма */
		Amount = this.member()
			.localizeTitle({ ru: "Сумма" })
			.float()
			.required();

		/** Из заказа */
		FromOrder = this.member()
			.localizeTitle({ ru: "Из заказа" })
			.lookup(() => sd.Order);

		/** От контрагента */
		FromParty = this.member()
			.localizeTitle({ ru: "От контрагента", rus: "Заказчики" })
			.lookup(() => sd.Party)
			.required();

		/** В заказ */
		ToOrder = this.member()
			.localizeTitle({ ru: "В заказ" })
			.lookup(() => sd.Order);

		/** К контрагенту */
		ToParty = this.member()
			.localizeTitle({ ru: "К контрагенту", rus: "Заказчики" })
			.lookup(() => sd.Party)
			.required();
	

	}


	/** Мильная карта */
	export interface IMilesCardSemantic extends IEntity2Semantic
	{
		//se.Number,
		//se.LastChangedPropertyName,
		//se.Owner,
		//se.Organization,

		/** Номер */
		Number: SemanticMember;

		/** Владелец */
		Owner: SemanticMember;

		/** Организация */
		Organization: SemanticMember;
	
	}

	/** Мильная карта */
	export class MilesCardSemantic extends Entity2Semantic implements IMilesCardSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "MilesCard";
			this._names = "MilesCards";
			this.icon("desktop");
			this._isEntity = true;
			this._localizeTitle({ ru: "Мильная карта", rus: "Мильные карты" });

			this._getDerivedEntities = null;

			this._className = "MilesCard";
			this._getRootEntity = () => sd.MilesCard;
			this._store = db.MilesCards;
			this._saveStore = db.MilesCards;
			this._lookupStore = db.MilesCardLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_MilesCard = new SemanticMember()
			.localizeTitle({ ru: "Мильная карта", rus: "Мильные карты" })
			.lookup(() => sd.MilesCard);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.entityName();

		/** Владелец */
		Owner = this.member()
			.localizeTitle({ ru: "Владелец" })
			.lookup(() => sd.Person)
			.required();

		/** Организация */
		Organization = this.member()
			.localizeTitle({ ru: "Организация", rus: "Организации" })
			.lookup(() => sd.Organization);
	

	}


	/** Начальный остаток */
	export interface IOpeningBalanceSemantic extends IEntity2Semantic
	{
		//se.Number,
		//se.Date,
		//se.Balance,
		//se.LastChangedPropertyName,
		//se.Party,

		/** Номер */
		Number: SemanticMember;

		/** Дата */
		Date: SemanticMember;

		/** Остаток */
		Balance: SemanticMember;

		/** Контрагент */
		Party: SemanticMember;
	
	}

	/** Начальный остаток */
	export class OpeningBalanceSemantic extends Entity2Semantic implements IOpeningBalanceSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "OpeningBalance";
			this._names = "OpeningBalances";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Начальный остаток", rus: "Начальные остатки" });

			this._getDerivedEntities = null;

			this._className = "OpeningBalance";
			this._getRootEntity = () => sd.OpeningBalance;
			this._store = db.OpeningBalances;
			this._saveStore = db.OpeningBalances;
			this._lookupStore = db.OpeningBalanceLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_OpeningBalance = new SemanticMember()
			.localizeTitle({ ru: "Начальный остаток", rus: "Начальные остатки" })
			.lookup(() => sd.OpeningBalance);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.entityName();

		/** Дата */
		Date = this.member()
			.localizeTitle({ en: "Date", ru: "Дата" })
			.date()
			.required();

		/** Остаток */
		Balance = this.member()
			.localizeTitle({ ru: "Остаток" })
			.float()
			.required();

		/** Контрагент */
		Party = this.member()
			.localizeTitle({ ru: "Контрагент" })
			.lookup(() => sd.Party);
	

	}


	/** Заказ */
	export interface IOrderSemantic extends IEntity2Semantic
	{
		//se.Number,
		//se.IssueDate,
		//se.IsVoid,
		//se.BillToName,
		//se.IsPublic,
		//se.IsSubjectOfPaymentsControl,
		//se.SeparateServiceFee,
		//se.UseServiceFeeOnlyInVat,
		//se.Discount,
		//se.Total,
		//se.Vat,
		//se.Paid,
		//se.TotalDue,
		//se.IsPaid,
		//se.VatDue,
		//se.DeliveryBalance,
		//se.ServiceFee,
		//se.Note,
		//se.InvoiceLastIndex,
		//se.LastChangedPropertyName,
		//se.Customer,
		//se.BillTo,
		//se.ShipTo,
		//se.AssignedTo,
		//se.Owner,
		//se.BankAccount,

		/** Номер */
		Number: SemanticMember;

		/** Дата выпуска */
		IssueDate: SemanticMember;

		/** Аннулирован */
		IsVoid: SemanticMember;

		/** Плательщик */
		BillToName: SemanticMember;

		/** Общий доступ */
		IsPublic: SemanticMember;

		/** Отображать в контроле оплат */
		IsSubjectOfPaymentsControl: SemanticMember;

		/** Выделять сервисный сбор */
		SeparateServiceFee: SemanticMember;

		/** НДС только от сервисного сбора */
		UseServiceFeeOnlyInVat: SemanticMember;

		/** Скидка */
		Discount: SemanticMember;

		/** Итого */
		Total: SemanticMember;

		/** В т.ч. НДС */
		Vat: SemanticMember;

		/** Оплачено */
		Paid: SemanticMember;

		/** К оплате */
		TotalDue: SemanticMember;

		/** Оплачен */
		IsPaid: SemanticMember;

		/** НДС к оплате */
		VatDue: SemanticMember;

		/** Баланс взаиморасчетов */
		DeliveryBalance: SemanticMember;

		/** Сервисный сбор */
		ServiceFee: SemanticMember;

		/** Примечание */
		Note: SemanticMember;

		InvoiceLastIndex: SemanticMember;

		/** Заказчик */
		Customer: SemanticMember;

		/** Плательщик */
		BillTo: SemanticMember;

		/** Получатель */
		ShipTo: SemanticMember;

		/** Ответственный */
		AssignedTo: SemanticMember;

		/** Владелец */
		Owner: SemanticMember;

		/** Банковский счёт */
		BankAccount: SemanticMember;
		
		Items: SemanticMember;
		
		Products: SemanticMember;
		
		Invoices: SemanticMember;
		
		Payments: SemanticMember;
		/** Входящие внутренние переводы */
		
		IncomingTransfers: SemanticMember;
		/** Исходящие внутренние переводы */
		
		OutgoingTransfers: SemanticMember;
	
	}

	/** Заказ */
	export class OrderSemantic extends Entity2Semantic implements IOrderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Order";
			this._names = "Orders";
			this.icon("briefcase");
			this._isEntity = true;
			this._localizeTitle({ ru: "Заказ", rus: "Заказы" });

			this._getDerivedEntities = null;

			this._className = "Order";
			this._getRootEntity = () => sd.Order;
			this._store = db.Orders;
			this._saveStore = db.Orders;
			this._lookupStore = db.OrderLookup;
			this._lookupFields = { id: "Id", name: "Number" };
			this.big();
		}

		_Order = new SemanticMember()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.length(10, 0, 0)
			.entityName()
			.unique();

		/** Дата выпуска */
		IssueDate = this.member()
			.localizeTitle({ ru: "Дата выпуска" })
			.date()
			.required()
			.entityDate();

		/** Аннулирован */
		IsVoid = this.member()
			.localizeTitle({ ru: "Аннулирован" })
			.bool()
			.required();

		/** Плательщик */
		BillToName = this.member()
			.localizeTitle({ ru: "Плательщик" })
			.string();

		/** Общий доступ */
		IsPublic = this.member()
			.localizeTitle({ ru: "Общий доступ" })
			.bool()
			.required();

		/** Отображать в контроле оплат */
		IsSubjectOfPaymentsControl = this.member()
			.localizeTitle({ ru: "Отображать в контроле оплат" })
			.bool()
			.required();

		/** Выделять сервисный сбор */
		SeparateServiceFee = this.member()
			.localizeTitle({ ru: "Выделять сервисный сбор" })
			.bool();

		/** НДС только от сервисного сбора */
		UseServiceFeeOnlyInVat = this.member()
			.localizeTitle({ ru: "НДС только от сервисного сбора" })
			.bool()
			.required();

		/** Скидка */
		Discount = this.member()
			.localizeTitle({ ru: "Скидка" })
			.money()
			.readOnly()
			.nonsaved();

		/** Итого */
		Total = this.member()
			.localizeTitle({ ru: "Итого" })
			.money()
			.readOnly()
			.nonsaved();

		/** В т.ч. НДС */
		Vat = this.member()
			.localizeTitle({ ru: "В т.ч. НДС" })
			.money()
			.readOnly()
			.nonsaved();

		/** Оплачено */
		Paid = this.member()
			.localizeTitle({ ru: "Оплачено" })
			.money()
			.readOnly()
			.nonsaved();

		/** К оплате */
		TotalDue = this.member()
			.localizeTitle({ ru: "К оплате" })
			.money()
			.readOnly()
			.nonsaved();

		/** Оплачен */
		IsPaid = this.member()
			.localizeTitle({ ru: "Оплачен" })
			.bool()
			.readOnly()
			.nonsaved()
			.required();

		/** НДС к оплате */
		VatDue = this.member()
			.localizeTitle({ ru: "НДС к оплате" })
			.money()
			.readOnly()
			.nonsaved();

		/** Баланс взаиморасчетов */
		DeliveryBalance = this.member()
			.localizeTitle({ ru: "Баланс взаиморасчетов" })
			.float()
			.readOnly()
			.nonsaved()
			.required();

		/** Сервисный сбор */
		ServiceFee = this.member()
			.localizeTitle({ ru: "Сервисный сбор" })
			.money()
			.calculated()
			.nonsaved();

		/** Примечание */
		Note = this.member()
			.localizeTitle({ ru: "Примечание" })
			.string();

		InvoiceLastIndex = this.member()
			.int();

		/** Заказчик */
		Customer = this.member()
			.localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
			.lookup(() => sd.Party);

		/** Плательщик */
		BillTo = this.member()
			.localizeTitle({ ru: "Плательщик", rus: "Заказчики" })
			.lookup(() => sd.Party);

		/** Получатель */
		ShipTo = this.member()
			.localizeTitle({ ru: "Получатель", rus: "Заказчики" })
			.lookup(() => sd.Party);

		/** Ответственный */
		AssignedTo = this.member()
			.localizeTitle({ ru: "Ответственный", rus: "Агенты" })
			.lookup(() => sd.Person);

		/** Владелец */
		Owner = this.member()
			.localizeTitle({ ru: "Владелец" })
			.lookup(() => sd.Party);

		/** Банковский счёт */
		BankAccount = this.member()
			.localizeTitle({ ru: "Банковский счёт", rus: "Банковские счёта" })
			.lookup(() => sd.BankAccount);

	
		Items = this.collection(() => sd.OrderItem, se => se.Order);

	
		Products = this.collection(() => sd.Product, se => se.Order);

	
		Invoices = this.collection(() => sd.Invoice, se => se.Order);

	
		Payments = this.collection(() => sd.Payment, se => se.Order);

		/** Входящие внутренние переводы */
		IncomingTransfers = this.collection(() => sd.InternalTransfer, se => se.ToOrder, m => m
			.localizeTitle({ ru: "Входящие внутренние переводы" }));

		/** Исходящие внутренние переводы */
		OutgoingTransfers = this.collection(() => sd.InternalTransfer, se => se.FromOrder, m => m
			.localizeTitle({ ru: "Исходящие внутренние переводы" }));
	

	}


	/** Чек */
	export interface IOrderCheckSemantic extends IEntity2Semantic
	{
		//se.Date,
		//se.CheckType,
		//se.CheckNumber,
		//se.Currency,
		//se.CheckAmount,
		//se.CheckVat,
		//se.PayAmount,
		//se.PaymentType,
		//se.Description,
		//se.LastChangedPropertyName,
		//se.Order,
		//se.Person,

		/** Дата */
		Date: SemanticMember;

		/** Тип чека */
		CheckType: SemanticMember;

		/** Номер чека */
		CheckNumber: SemanticMember;

		/** Валюта */
		Currency: SemanticMember;

		/** Сумма чека */
		CheckAmount: SemanticMember;

		/** В т.ч. НДС */
		CheckVat: SemanticMember;

		/** Сумма оплаты */
		PayAmount: SemanticMember;

		/** Тип оплаты */
		PaymentType: SemanticMember;

		/** Описание */
		Description: SemanticMember;

		/** Заказ */
		Order: SemanticMember;

		/** Печатал чек */
		Person: SemanticMember;
	
	}

	/** Чек */
	export class OrderCheckSemantic extends Entity2Semantic implements IOrderCheckSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "OrderCheck";
			this._names = "OrderChecks";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Чек", rus: "Чеки" });

			this._getDerivedEntities = null;

			this._className = "OrderCheck";
			this._getRootEntity = () => sd.OrderCheck;
			this._store = db.OrderChecks;
			this._saveStore = db.OrderChecks;
			this._lookupStore = db.OrderCheckLookup;
			this._lookupFields = { id: "Id", name: "CheckNumber" };
		}

		_OrderCheck = new SemanticMember()
			.localizeTitle({ ru: "Чек", rus: "Чеки" })
			.lookup(() => sd.OrderCheck);
				

		/** Дата */
		Date = this.member()
			.localizeTitle({ en: "Date", ru: "Дата" })
			.dateTime2()
			.required()
			.entityDate();

		/** Тип чека */
		CheckType = this.member()
			.localizeTitle({ ru: "Тип чека" })
			.enum(CheckType)
			.required();

		/** Номер чека */
		CheckNumber = this.member()
			.localizeTitle({ ru: "Номер чека" })
			.string()
			.length(10, 0, 0)
			.entityName();

		/** Валюта */
		Currency = this.member()
			.localizeTitle({ ru: "Валюта" })
			.string();

		/** Сумма чека */
		CheckAmount = this.member()
			.localizeTitle({ ru: "Сумма чека" })
			.float(2);

		/** В т.ч. НДС */
		CheckVat = this.member()
			.localizeTitle({ ru: "В т.ч. НДС" })
			.float(2);

		/** Сумма оплаты */
		PayAmount = this.member()
			.localizeTitle({ ru: "Сумма оплаты", ruDesc: "Деньги, которые клиент передал кассиру (из которых последний возвращает сдачу)" })
			.float(2);

		/** Тип оплаты */
		PaymentType = this.member()
			.localizeTitle({ ru: "Тип оплаты" })
			.enum(CheckPaymentType);

		/** Описание */
		Description = this.member()
			.localizeTitle({ ru: "Описание" })
			.string();

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);

		/** Печатал чек */
		Person = this.member()
			.localizeTitle({ ru: "Печатал чек" })
			.lookup(() => sd.Person);
	

	}


	/** Позиция заказа */
	export interface IOrderItemSemantic extends IEntity2Semantic
	{
		//se.Position,
		//se.Text,
		//se.LinkType,
		//se.Price,
		//se.Quantity,
		//se.Total,
		//se.Discount,
		//se.GrandTotal,
		//se.GivenVat,
		//se.TaxedTotal,
		//se.HasVat,
		//se.ServiceFee,
		//se.IsDelivered,
		//se.CheckNameUA,
		//se.LastChangedPropertyName,
		//se.Order,
		//se.Product,
		//se.Consignment,

		/** Номер */
		Position: SemanticMember;

		/** Название */
		Text: SemanticMember;

		/** Тип */
		LinkType: SemanticMember;

		/** Цена */
		Price: SemanticMember;

		/** Количество */
		Quantity: SemanticMember;

		/** Итого */
		Total: SemanticMember;

		/** Скидка */
		Discount: SemanticMember;

		/** К оплате */
		GrandTotal: SemanticMember;

		GivenVat: SemanticMember;

		TaxedTotal: SemanticMember;

		HasVat: SemanticMember;

		ServiceFee: SemanticMember;

		IsDelivered: SemanticMember;

		CheckNameUA: SemanticMember;

		/** Заказ */
		Order: SemanticMember;

		/** Услуга */
		Product: SemanticMember;

		/** Накладная */
		Consignment: SemanticMember;
	
	}

	/** Позиция заказа */
	export class OrderItemSemantic extends Entity2Semantic implements IOrderItemSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "OrderItem";
			this._names = "OrderItems";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Позиция заказа", rus: "Позиции заказа" });

			this._getDerivedEntities = null;

			this._className = "OrderItem";
			this._getRootEntity = () => sd.OrderItem;
			this._store = db.OrderItems;
			this._saveStore = db.OrderItems;
			this._lookupStore = db.OrderItemLookup;
			this._lookupFields = { id: "Id", name: "Text" };
		}

		_OrderItem = new SemanticMember()
			.localizeTitle({ ru: "Позиция заказа", rus: "Позиции заказа" })
			.lookup(() => sd.OrderItem);
				

		/** Номер */
		Position = this.member()
			.localizeTitle({ ru: "Номер" })
			.int()
			.required()
			.entityPosition();

		/** Название */
		Text = this.member()
			.localizeTitle({ ru: "Название" })
			.text(3)
			.entityName();

		/** Тип */
		LinkType = this.member()
			.localizeTitle({ ru: "Тип" })
			.enum(OrderItemLinkType);

		/** Цена */
		Price = this.member()
			.localizeTitle({ ru: "Цена" })
			.money();

		/** Количество */
		Quantity = this.member()
			.localizeTitle({ ru: "Количество" })
			.int()
			.required();

		/** Итого */
		Total = this.member()
			.localizeTitle({ ru: "Итого" })
			.money()
			.calculated()
			.nonsaved();

		/** Скидка */
		Discount = this.member()
			.localizeTitle({ ru: "Скидка" })
			.money();

		/** К оплате */
		GrandTotal = this.member()
			.localizeTitle({ ru: "К оплате" })
			.money()
			.readOnly()
			.nonsaved();

		GivenVat = this.member()
			.money()
			.readOnly()
			.nonsaved();

		TaxedTotal = this.member()
			.money()
			.readOnly()
			.nonsaved();

		HasVat = this.member()
			.bool()
			.required();

		ServiceFee = this.member()
			.money()
			.calculated()
			.nonsaved();

		IsDelivered = this.member()
			.bool()
			.calculated()
			.nonsaved()
			.required();

		CheckNameUA = this.member()
			.string()
			.calculated()
			.nonsaved();

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);

		/** Услуга */
		Product = this.member()
			.localizeTitle({ ru: "Услуга", rus: "Все услуги" })
			.lookup(() => sd.Product);

		/** Накладная */
		Consignment = this.member()
			.localizeTitle({ ru: "Накладная", rus: "Накладные" })
			.lookup(() => sd.Consignment);
	

	}


	/** Паспорт */
	export interface IPassportSemantic extends IEntity2Semantic
	{
		//se.Number,
		//se.FirstName,
		//se.MiddleName,
		//se.LastName,
		//se.Name,
		//se.Birthday,
		//se.Gender,
		//se.ExpiredOn,
		//se.Note,
		//se.AmadeusString,
		//se.GalileoString,
		//se.LastChangedPropertyName,
		//se.Owner,
		//se.Citizenship,
		//se.IssuedBy,

		/** Номер */
		Number: SemanticMember;

		/** Имя */
		FirstName: SemanticMember;

		/** Отчество */
		MiddleName: SemanticMember;

		/** Фамилия */
		LastName: SemanticMember;

		/** Ф.И.О. */
		Name: SemanticMember;

		/** Дата рождения */
		Birthday: SemanticMember;

		/** Пол */
		Gender: SemanticMember;

		/** Действителен до */
		ExpiredOn: SemanticMember;

		/** Примечание */
		Note: SemanticMember;

		/** Данные для Amadeus */
		AmadeusString: SemanticMember;

		/** Данные для Galileo */
		GalileoString: SemanticMember;

		/** Владелец */
		Owner: SemanticMember;

		/** Гражданство */
		Citizenship: SemanticMember;

		/** Выдавшая страна */
		IssuedBy: SemanticMember;
	
	}

	/** Паспорт */
	export class PassportSemantic extends Entity2Semantic implements IPassportSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Passport";
			this._names = "Passports";
			this.icon("certificate");
			this._isEntity = true;
			this._localizeTitle({ ru: "Паспорт", rus: "Паспорта" });

			this._getDerivedEntities = null;

			this._className = "Passport";
			this._getRootEntity = () => sd.Passport;
			this._store = db.Passports;
			this._saveStore = db.Passports;
			this._lookupStore = db.PassportLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_Passport = new SemanticMember()
			.localizeTitle({ ru: "Паспорт", rus: "Паспорта" })
			.lookup(() => sd.Passport);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.entityName();

		/** Имя */
		FirstName = this.member()
			.localizeTitle({ ru: "Имя" })
			.string();

		/** Отчество */
		MiddleName = this.member()
			.localizeTitle({ ru: "Отчество" })
			.string();

		/** Фамилия */
		LastName = this.member()
			.localizeTitle({ ru: "Фамилия" })
			.string();

		/** Ф.И.О. */
		Name = this.member()
			.localizeTitle({ ru: "Ф.И.О." })
			.string()
			.calculated()
			.nonsaved();

		/** Дата рождения */
		Birthday = this.member()
			.localizeTitle({ ru: "Дата рождения" })
			.date();

		/** Пол */
		Gender = this.member()
			.localizeTitle({ ru: "Пол" })
			.enum(Gender);

		/** Действителен до */
		ExpiredOn = this.member()
			.localizeTitle({ ru: "Действителен до" })
			.date();

		/** Примечание */
		Note = this.member()
			.localizeTitle({ ru: "Примечание" })
			.text(3);

		/** Данные для Amadeus */
		AmadeusString = this.member()
			.localizeTitle({ ru: "Данные для Amadeus" })
			.string()
			.calculated()
			.nonsaved();

		/** Данные для Galileo */
		GalileoString = this.member()
			.localizeTitle({ ru: "Данные для Galileo" })
			.string()
			.calculated()
			.nonsaved();

		/** Владелец */
		Owner = this.member()
			.localizeTitle({ ru: "Владелец" })
			.lookup(() => sd.Person)
			.required();

		/** Гражданство */
		Citizenship = this.member()
			.localizeTitle({ ru: "Гражданство" })
			.lookup(() => sd.Country);

		/** Выдавшая страна */
		IssuedBy = this.member()
			.localizeTitle({ ru: "Выдавшая страна" })
			.lookup(() => sd.Country);
	

	}


	/** Платёж */
	export interface IPaymentSemantic extends IEntity2Semantic
	{
		//se.PaymentForm,
		//se.Number,
		//se.Date,
		//se.DocumentNumber,
		//se.DocumentUniqueCode,
		//se.InvoiceDate,
		//se.Amount,
		//se.Vat,
		//se.ReceivedFrom,
		//se.PostedOn,
		//se.SavePosted,
		//se.Note,
		//se.IsVoid,
		//se.IsPosted,
		//se.PrintedDocument,
		//se.LastChangedPropertyName,
		//se.Payer,
		//se.Order,
		//se.Invoice,
		//se.AssignedTo,
		//se.RegisteredBy,
		//se.Owner,
		//se.PaymentSystem,

		/** Форма оплаты */
		PaymentForm: SemanticMember;

		/** Номер */
		Number: SemanticMember;

		/** Дата */
		Date: SemanticMember;

		/** Номер документа */
		DocumentNumber: SemanticMember;

		DocumentUniqueCode: SemanticMember;

		/** Дата счета/квитанции */
		InvoiceDate: SemanticMember;

		/** Сумма */
		Amount: SemanticMember;

		/** В т.ч. НДС */
		Vat: SemanticMember;

		/** Получен от */
		ReceivedFrom: SemanticMember;

		/** Дата проводки */
		PostedOn: SemanticMember;

		/** Сохранить проведенным */
		SavePosted: SemanticMember;

		/** Примечание */
		Note: SemanticMember;

		/** Аннулирован */
		IsVoid: SemanticMember;

		IsPosted: SemanticMember;

		PrintedDocument: SemanticMember;

		/** Плательщик */
		Payer: SemanticMember;

		/** Заказ */
		Order: SemanticMember;

		/** Квитанция */
		Invoice: SemanticMember;

		/** Ответственный */
		AssignedTo: SemanticMember;

		/** Зарегистрирован */
		RegisteredBy: SemanticMember;

		/** Владелец */
		Owner: SemanticMember;

		/** Платёжная система */
		PaymentSystem: SemanticMember;

		/** Анулировать */
		Void: SemanticEntityAction;

		/** Восстановить */
		Unvoid: SemanticEntityAction;

		GetNote: SemanticEntityAction;
	
	}

	/** Платёж */
	export class PaymentSemantic extends Entity2Semantic implements IPaymentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "Payment";
			this._names = "Payments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Платёж", rus: "Платежи" });

			this._getDerivedEntities = () => [
				sd.WireTransfer, sd.CheckPayment, sd.CashInOrderPayment, sd.CashOutOrderPayment, sd.ElectronicPayment
			];

			this._className = "Payment";
			this._getRootEntity = () => sd.Payment;
			this._store = db.Payments;
			this._saveStore = db.Payments;
			this._lookupStore = db.PaymentLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_Payment = new SemanticMember()
			.localizeTitle({ ru: "Платёж", rus: "Платежи" })
			.lookup(() => sd.Payment);
				

		/** Форма оплаты */
		PaymentForm = this.member()
			.localizeTitle({ ru: "Форма оплаты" })
			.enum(PaymentForm)
			.required()
			.entityType();

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.entityName();

		/** Дата */
		Date = this.member()
			.localizeTitle({ en: "Date", ru: "Дата" })
			.date()
			.required()
			.entityDate();

		/** Номер документа */
		DocumentNumber = this.member()
			.localizeTitle({ ru: "Номер документа" })
			.string()
			.length(10, 0, 0);

		DocumentUniqueCode = this.member()
			.string()
			.calculated()
			.nonsaved();

		/** Дата счета/квитанции */
		InvoiceDate = this.member()
			.localizeTitle({ ru: "Дата счета/квитанции" })
			.date()
			.calculated()
			.nonsaved();

		/** Сумма */
		Amount = this.member()
			.localizeTitle({ ru: "Сумма" })
			.money();

		/** В т.ч. НДС */
		Vat = this.member()
			.localizeTitle({ ru: "В т.ч. НДС" })
			.money();

		/** Получен от */
		ReceivedFrom = this.member()
			.localizeTitle({ ru: "Получен от" })
			.string();

		/** Дата проводки */
		PostedOn = this.member()
			.localizeTitle({ ru: "Дата проводки" })
			.date()
			.subject();

		/** Сохранить проведенным */
		SavePosted = this.member()
			.localizeTitle({ ru: "Сохранить проведенным" })
			.bool()
			.calculated()
			.subject();

		/** Примечание */
		Note = this.member()
			.localizeTitle({ ru: "Примечание" })
			.string();

		/** Аннулирован */
		IsVoid = this.member()
			.localizeTitle({ ru: "Аннулирован" })
			.bool()
			.required();

		IsPosted = this.member()
			.bool()
			.calculated()
			.nonsaved()
			.required();

		PrintedDocument = this.member()
			;

		/** Плательщик */
		Payer = this.member()
			.localizeTitle({ ru: "Плательщик" })
			.lookup(() => sd.Party)
			.required();

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order)
			.subject();

		/** Квитанция */
		Invoice = this.member()
			.localizeTitle({ ru: "Квитанция", rus: "Квитанции" })
			.lookup(() => sd.Invoice);

		/** Ответственный */
		AssignedTo = this.member()
			.localizeTitle({ ru: "Ответственный", rus: "Агенты" })
			.lookup(() => sd.Person);

		/** Зарегистрирован */
		RegisteredBy = this.member()
			.localizeTitle({ ru: "Зарегистрирован", rus: "Агенты" })
			.lookup(() => sd.Person);

		/** Владелец */
		Owner = this.member()
			.localizeTitle({ ru: "Владелец" })
			.lookup(() => sd.Party);

		/** Платёжная система */
		PaymentSystem = this.member()
			.localizeTitle({ ru: "Платёжная система", rus: "Платёжные системы" })
			.lookup(() => sd.PaymentSystem);

		/** Анулировать */
		Void = this.action()
			.localizeTitle({ ru: "Анулировать" });

		/** Восстановить */
		Unvoid = this.action()
			.localizeTitle({ ru: "Восстановить" });

		GetNote = this.action()
			;
	

	}


	/** Услуга */
	export interface IProductSemantic extends IEntity2Semantic
	{
		//se.Type,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		Type: SemanticMember;

		/** Название */
		Name: SemanticMember;

		/** Дата выпуска */
		IssueDate: SemanticMember;

		/** Это возврат */
		IsRefund: SemanticMember;

		IsReservation: SemanticMember;

		/** Обработан */
		IsProcessed: SemanticMember;

		/** Аннулирован */
		IsVoid: SemanticMember;

		/** К обработке */
		RequiresProcessing: SemanticMember;

		IsDelivered: SemanticMember;

		/** Оплачен */
		IsPaid: SemanticMember;

		/** Маршрут */
		Itinerary: SemanticMember;

		/** Дата начала */
		StartDate: SemanticMember;

		/** Дата окончания */
		FinishDate: SemanticMember;

		/** Бронировка */
		PnrCode: SemanticMember;

		/** Туркод */
		TourCode: SemanticMember;

		/** Бронировщик: код офиса GDS-агента */
		BookerOffice: SemanticMember;

		/** Бронировщик: код GDS-агента */
		BookerCode: SemanticMember;

		/** Тикетер: код офиса GDS-агента */
		TicketerOffice: SemanticMember;

		/** Тикетер: код GDS-агента */
		TicketerCode: SemanticMember;

		/** IATA офис */
		TicketingIataOffice: SemanticMember;

		IsTicketerRobot: SemanticMember;

		/** Тариф */
		Fare: SemanticMember;

		/** Экв. тариф */
		EqualFare: SemanticMember;

		/** Таксы */
		FeesTotal: SemanticMember;

		/** Штраф за отмену */
		CancelFee: SemanticMember;

		/** К перечислению провайдеру */
		Total: SemanticMember;

		/** В т.ч. НДС */
		Vat: SemanticMember;

		/** Сервисный сбор */
		ServiceFee: SemanticMember;

		/** Штраф сервисного сбора */
		ServiceFeePenalty: SemanticMember;

		/** Доп. доход */
		Handling: SemanticMember;

		/** Комиссия */
		Commission: SemanticMember;

		/** Скидка от комиссии */
		CommissionDiscount: SemanticMember;

		/** Скидка */
		Discount: SemanticMember;

		/** Бонусная скидка */
		BonusDiscount: SemanticMember;

		/** Бонусное накопление */
		BonusAccumulation: SemanticMember;

		/** Cбор за возврат */
		RefundServiceFee: SemanticMember;

		ServiceTotal: SemanticMember;

		/** К оплате */
		GrandTotal: SemanticMember;

		CancelCommissionPercent: SemanticMember;

		/** Комисия за возврат */
		CancelCommission: SemanticMember;

		/** % комиссии */
		CommissionPercent: SemanticMember;

		TotalToTransfer: SemanticMember;

		Profit: SemanticMember;

		ExtraCharge: SemanticMember;

		/** Тип оплаты */
		PaymentType: SemanticMember;

		/** Ставка НДС для услуги */
		TaxRateOfProduct: SemanticMember;

		/** Ставка НДС для сбора */
		TaxRateOfServiceFee: SemanticMember;

		/** Примечание */
		Note: SemanticMember;

		/** Оригинатор */
		Originator: SemanticMember;

		/** Источник */
		Origin: SemanticMember;

		/** Пассажир */
		PassengerName: SemanticMember;

		/** Пассажир из GDS */
		GdsPassengerName: SemanticMember;

		/** Пассажир */
		Passenger: SemanticMember;

		/** Продюсер */
		Producer: SemanticMember;

		/** Провайдер */
		Provider: SemanticMember;

		/** Перевыпуск для */
		ReissueFor: SemanticMember;

		/** Исходный документ */
		RefundedProduct: SemanticMember;

		/** Заказчик */
		Customer: SemanticMember;

		/** Заказ */
		Order: SemanticMember;

		/** Посредник */
		Intermediary: SemanticMember;

		/** Страна */
		Country: SemanticMember;

		/** Бронировщик */
		Booker: SemanticMember;

		/** Тикетер */
		Ticketer: SemanticMember;

		/** Продавец */
		Seller: SemanticMember;

		/** Владелец */
		Owner: SemanticMember;

		/** Оригинальный документ */
		OriginalDocument: SemanticMember;
		
		Passengers: SemanticMember;
		
		Products_ReissueFor: SemanticMember;
		
		Products_RefundedProduct: SemanticMember;
	
	}

	/** Услуга */
	export class ProductSemantic extends Entity2Semantic implements IProductSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "Product";
			this._names = "Products";
			this.icon("suitcase");
			this._isEntity = true;
			this._localizeTitle({ ru: "Услуга", rus: "Все услуги" });

			this._getDerivedEntities = () => [
				sd.SimCard, sd.AviaTicket, sd.AviaDocument, sd.BusTicket, sd.BusDocument, sd.CarRental, sd.AviaRefund, sd.BusTicketRefund, sd.PasteboardRefund, sd.InsuranceRefund, sd.GenericProduct, sd.Pasteboard, sd.RailwayDocument, sd.AviaMco, sd.Accommodation, sd.Insurance, sd.InsuranceDocument, sd.Isic, sd.Transfer, sd.Tour, sd.Excursion
			];

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.Products;
			this._saveStore = db.Products;
			this._lookupStore = db.ProductLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_Product = new SemanticMember()
			.localizeTitle({ ru: "Услуга", rus: "Все услуги" })
			.lookup(() => sd.Product);
				

		Type = this.member()
			.localizeTitle({ ru: "Вид услуги" })
			.enum(ProductType)
			.required()
			.length(12, 0, 0)
			.entityType();

		/** Название */
		Name = this.member()
			.localizeTitle({ ru: "Название" })
			.string()
			.length(16, 0, 0)
			.entityName();

		/** Дата выпуска */
		IssueDate = this.member()
			.localizeTitle({ ru: "Дата выпуска" })
			.date()
			.required()
			.entityDate();

		/** Это возврат */
		IsRefund = this.member()
			.localizeTitle({ ru: "Это возврат" })
			.bool()
			.required();

		IsReservation = this.member()
			.bool()
			.required();

		/** Обработан */
		IsProcessed = this.member()
			.localizeTitle({ ru: "Обработан" })
			.bool()
			.required();

		/** Аннулирован */
		IsVoid = this.member()
			.localizeTitle({ ru: "Аннулирован" })
			.bool()
			.required();

		/** К обработке */
		RequiresProcessing = this.member()
			.localizeTitle({ ru: "К обработке" })
			.bool()
			.required();

		IsDelivered = this.member()
			.bool()
			.calculated()
			.nonsaved()
			.required();

		/** Оплачен */
		IsPaid = this.member()
			.localizeTitle({ ru: "Оплачен" })
			.bool()
			.calculated()
			.nonsaved()
			.required();

		/** Маршрут */
		Itinerary = this.member()
			.localizeTitle({ ru: "Маршрут" })
			.string()
			.length(16, 0, 0);

		/** Дата начала */
		StartDate = this.member()
			.localizeTitle({ ru: "Дата начала" })
			.date();

		/** Дата окончания */
		FinishDate = this.member()
			.localizeTitle({ ru: "Дата окончания" })
			.date();

		/** Бронировка */
		PnrCode = this.member()
			.localizeTitle({ ru: "Бронировка" })
			.string();

		/** Туркод */
		TourCode = this.member()
			.localizeTitle({ ru: "Туркод" })
			.string();

		/** Бронировщик: код офиса GDS-агента */
		BookerOffice = this.member()
			.localizeTitle({ ru: "Бронировщик: код офиса GDS-агента", ruShort: "код офиса" })
			.emptyText("код офиса")
			.string()
			.length(8, 0, 0);

		/** Бронировщик: код GDS-агента */
		BookerCode = this.member()
			.localizeTitle({ ru: "Бронировщик: код GDS-агента", ruShort: "код агента" })
			.emptyText("код агента")
			.string()
			.length(8, 0, 0);

		/** Тикетер: код офиса GDS-агента */
		TicketerOffice = this.member()
			.localizeTitle({ ru: "Тикетер: код офиса GDS-агента", ruShort: "код офиса" })
			.emptyText("код офиса")
			.string()
			.length(8, 0, 0);

		/** Тикетер: код GDS-агента */
		TicketerCode = this.member()
			.localizeTitle({ ru: "Тикетер: код GDS-агента", ruShort: "код агента" })
			.emptyText("код агента")
			.string()
			.length(8, 0, 0);

		/** IATA офис */
		TicketingIataOffice = this.member()
			.localizeTitle({ ru: "IATA офис" })
			.string();

		IsTicketerRobot = this.member()
			.bool()
			.required();

		/** Тариф */
		Fare = this.member()
			.localizeTitle({ ru: "Тариф" })
			.money();

		/** Экв. тариф */
		EqualFare = this.member()
			.localizeTitle({ ru: "Экв. тариф" })
			.defaultMoney()
			.subject();

		/** Таксы */
		FeesTotal = this.member()
			.localizeTitle({ ru: "Таксы" })
			.defaultMoney()
			.subject();

		/** Штраф за отмену */
		CancelFee = this.member()
			.localizeTitle({ ru: "Штраф за отмену" })
			.defaultMoney()
			.subject();

		/** К перечислению провайдеру */
		Total = this.member()
			.localizeTitle({ ru: "К перечислению провайдеру" })
			.defaultMoney()
			.readOnly()
			.nonsaved();

		/** В т.ч. НДС */
		Vat = this.member()
			.localizeTitle({ ru: "В т.ч. НДС" })
			.defaultMoney();

		/** Сервисный сбор */
		ServiceFee = this.member()
			.localizeTitle({ ru: "Сервисный сбор" })
			.defaultMoney()
			.subject();

		/** Штраф сервисного сбора */
		ServiceFeePenalty = this.member()
			.localizeTitle({ ru: "Штраф сервисного сбора" })
			.defaultMoney()
			.subject();

		/** Доп. доход */
		Handling = this.member()
			.localizeTitle({ ru: "Доп. доход" })
			.defaultMoney()
			.subject();

		/** Комиссия */
		Commission = this.member()
			.localizeTitle({ ru: "Комиссия" })
			.defaultMoney();

		/** Скидка от комиссии */
		CommissionDiscount = this.member()
			.localizeTitle({ ru: "Скидка от комиссии" })
			.defaultMoney()
			.subject();

		/** Скидка */
		Discount = this.member()
			.localizeTitle({ ru: "Скидка" })
			.defaultMoney()
			.subject();

		/** Бонусная скидка */
		BonusDiscount = this.member()
			.localizeTitle({ ru: "Бонусная скидка" })
			.defaultMoney()
			.subject();

		/** Бонусное накопление */
		BonusAccumulation = this.member()
			.localizeTitle({ ru: "Бонусное накопление" })
			.defaultMoney();

		/** Cбор за возврат */
		RefundServiceFee = this.member()
			.localizeTitle({ ru: "Cбор за возврат" })
			.defaultMoney()
			.subject();

		ServiceTotal = this.member()
			.money()
			.calculated()
			.nonsaved();

		/** К оплате */
		GrandTotal = this.member()
			.localizeTitle({ ru: "К оплате" })
			.defaultMoney();

		CancelCommissionPercent = this.member()
			.float();

		/** Комисия за возврат */
		CancelCommission = this.member()
			.localizeTitle({ ru: "Комисия за возврат" })
			.money();

		/** % комиссии */
		CommissionPercent = this.member()
			.localizeTitle({ ru: "% комиссии" })
			.float();

		TotalToTransfer = this.member()
			.money()
			.calculated()
			.nonsaved();

		Profit = this.member()
			.money()
			.calculated()
			.nonsaved();

		ExtraCharge = this.member()
			.money()
			.calculated()
			.nonsaved();

		/** Тип оплаты */
		PaymentType = this.member()
			.localizeTitle({ ru: "Тип оплаты" })
			.enum(PaymentType)
			.required();

		/** Ставка НДС для услуги */
		TaxRateOfProduct = this.member()
			.localizeTitle({ ru: "Ставка НДС для услуги" })
			.enum(TaxRate)
			.required();

		/** Ставка НДС для сбора */
		TaxRateOfServiceFee = this.member()
			.localizeTitle({ ru: "Ставка НДС для сбора" })
			.enum(TaxRate)
			.required();

		/** Примечание */
		Note = this.member()
			.localizeTitle({ ru: "Примечание" })
			.string();

		/** Оригинатор */
		Originator = this.member()
			.localizeTitle({ ru: "Оригинатор" })
			.enum(GdsOriginator)
			.required();

		/** Источник */
		Origin = this.member()
			.localizeTitle({ ru: "Источник" })
			.enum(ProductOrigin)
			.required();

		/** Пассажир */
		PassengerName = this.member()
			.localizeTitle({ ru: "Пассажир" })
			.string();

		/** Пассажир из GDS */
		GdsPassengerName = this.member()
			.localizeTitle({ ru: "Пассажир из GDS" })
			.string()
			.calculated()
			.length(20, 0, 0);

		/** Пассажир */
		Passenger = this.member()
			.localizeTitle({ ru: "Пассажир" })
			.lookup(() => sd.Person)
			.calculated()
			.nonsaved();

		/** Продюсер */
		Producer = this.member()
			.localizeTitle({ ru: "Продюсер" })
			.lookup(() => sd.Organization);

		/** Провайдер */
		Provider = this.member()
			.localizeTitle({ ru: "Провайдер" })
			.lookup(() => sd.Organization);

		/** Перевыпуск для */
		ReissueFor = this.member()
			.localizeTitle({ ru: "Перевыпуск для" })
			.lookup(() => sd.Product);

		/** Исходный документ */
		RefundedProduct = this.member()
			.localizeTitle({ ru: "Исходный документ" })
			.lookup(() => sd.Product);

		/** Заказчик */
		Customer = this.member()
			.localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
			.lookup(() => sd.Party);

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order)
			.subject();

		/** Посредник */
		Intermediary = this.member()
			.localizeTitle({ ru: "Посредник" })
			.lookup(() => sd.Party);

		/** Страна */
		Country = this.member()
			.localizeTitle({ ru: "Страна", rus: "Страны" })
			.lookup(() => sd.Country);

		/** Бронировщик */
		Booker = this.member()
			.localizeTitle({ ru: "Бронировщик", rus: "Агенты" })
			.lookup(() => sd.Person);

		/** Тикетер */
		Ticketer = this.member()
			.localizeTitle({ ru: "Тикетер", rus: "Агенты" })
			.lookup(() => sd.Person);

		/** Продавец */
		Seller = this.member()
			.localizeTitle({ ru: "Продавец", rus: "Агенты" })
			.lookup(() => sd.Person);

		/** Владелец */
		Owner = this.member()
			.localizeTitle({ ru: "Владелец" })
			.lookup(() => sd.Party);

		/** Оригинальный документ */
		OriginalDocument = this.member()
			.localizeTitle({ ru: "Оригинальный документ" })
			.lookup(() => sd.GdsFile);

	
		Passengers = this.collection(() => sd.ProductPassenger, se => se.Product);

	
		Products_ReissueFor = this.collection(() => sd.Product, se => se.ReissueFor);

	
		Products_RefundedProduct = this.collection(() => sd.Product, se => se.RefundedProduct);
	

	}


	/** Пассажир */
	export interface IProductPassengerSemantic extends IEntity2Semantic
	{
		//se.PassengerName,
		//se.LastChangedPropertyName,
		//se.Product,
		//se.Passenger,

		/** Имя пассажира */
		PassengerName: SemanticMember;

		/** Услуга */
		Product: SemanticMember;

		/** Пассажир */
		Passenger: SemanticMember;
	
	}

	/** Пассажир */
	export class ProductPassengerSemantic extends Entity2Semantic implements IProductPassengerSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductPassenger";
			this._names = "ProductPassengers";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Пассажир", rus: "Пассажиры" });

			this._getDerivedEntities = null;

			this._className = "ProductPassenger";
			this._getRootEntity = () => sd.ProductPassenger;
			this._store = db.ProductPassengers;
			this._saveStore = db.ProductPassengers;
			this._lookupFields = { id: "Id", name: "" };
		}

		_ProductPassenger = new SemanticMember()
			.localizeTitle({ ru: "Пассажир", rus: "Пассажиры" })
			.lookup(() => sd.ProductPassenger);
				

		/** Имя пассажира */
		PassengerName = this.member()
			.localizeTitle({ ru: "Имя пассажира", ruShort: "имя" })
			.emptyText("имя")
			.string();

		/** Услуга */
		Product = this.member()
			.localizeTitle({ ru: "Услуга", rus: "Все услуги" })
			.lookup(() => sd.Product);

		/** Пассажир */
		Passenger = this.member()
			.localizeTitle({ ru: "Пассажир", ruShort: "персона" })
			.emptyText("персона")
			.lookup(() => sd.Person);
	

	}


	/** Проживание */
	export interface IAccommodationSemantic extends IProductSemantic
	{
		//se.Type,
		//se.HotelName,
		//se.HotelOffice,
		//se.HotelCode,
		//se.PlacementName,
		//se.PlacementOffice,
		//se.PlacementCode,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.AccommodationType,
		//se.CateringType,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Гостиница */
		HotelName: SemanticMember;

		/** Офис гостиницы */
		HotelOffice: SemanticMember;

		/** Код гостиницы */
		HotelCode: SemanticMember;

		/** Расположение */
		PlacementName: SemanticMember;

		PlacementOffice: SemanticMember;

		PlacementCode: SemanticMember;

		/** Тип проживания */
		AccommodationType: SemanticMember;

		/** Тип питания */
		CateringType: SemanticMember;
	
	}

	/** Проживание */
	export class AccommodationSemantic extends ProductSemantic implements IAccommodationSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Accommodation";
			this._names = "Accommodations";
			this.icon("bed");
			this._isEntity = true;
			this._localizeTitle({ ru: "Проживание", rus: "Проживания", ua: "Готель" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.Accommodations;
			this._saveStore = db.Accommodations;
			this._lookupStore = db.AccommodationLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.ReissueFor
				.localizeTitle({ ru: "Перевыпуск для" })
				.lookup(() => sd.Accommodation);

			this.Provider
				.lookup(() => sd.AccommodationProvider);
		}

		_Accommodation = new SemanticMember()
			.localizeTitle({ ru: "Проживание", rus: "Проживания", ua: "Готель" })
			.lookup(() => sd.Accommodation);
				

		/** Гостиница */
		HotelName = this.member()
			.localizeTitle({ ru: "Гостиница" })
			.string();

		/** Офис гостиницы */
		HotelOffice = this.member()
			.localizeTitle({ ru: "Офис гостиницы", ruShort: "офис" })
			.emptyText("офис")
			.string();

		/** Код гостиницы */
		HotelCode = this.member()
			.localizeTitle({ ru: "Код гостиницы", ruShort: "код" })
			.emptyText("код")
			.string();

		/** Расположение */
		PlacementName = this.member()
			.localizeTitle({ ru: "Расположение" })
			.string();

		PlacementOffice = this.member()
			.emptyText("офис")
			.string();

		PlacementCode = this.member()
			.emptyText("код")
			.string();

		/** Тип проживания */
		AccommodationType = this.member()
			.localizeTitle({ ru: "Тип проживания", rus: "Типы проживания" })
			.lookup(() => sd.AccommodationType);

		/** Тип питания */
		CateringType = this.member()
			.localizeTitle({ ru: "Тип питания", rus: "Типы питания" })
			.lookup(() => sd.CateringType);
	

	}


	/** Аэропорт */
	export interface IAirportSemantic extends IEntity3Semantic
	{
		//se.Code,
		//se.Settlement,
		//se.LocalizedSettlement,
		//se.Latitude,
		//se.Longitude,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.Country,

		/** Код */
		Code: SemanticMember;

		/** Населенный пункт (англ.) */
		Settlement: SemanticMember;

		/** Населенный пункт */
		LocalizedSettlement: SemanticMember;

		/** Широта */
		Latitude: SemanticMember;

		/** Долгота */
		Longitude: SemanticMember;

		/** Страна */
		Country: SemanticMember;
	
	}

	/** Аэропорт */
	export class AirportSemantic extends Entity3Semantic implements IAirportSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Airport";
			this._names = "Airports";
			this.icon("road");
			this._isEntity = true;
			this._localizeTitle({ ru: "Аэропорт", rus: "Аэропорты" });

			this._getDerivedEntities = null;

			this._className = "Airport";
			this._getRootEntity = () => sd.Airport;
			this._store = db.Airports;
			this._saveStore = db.Airports;
			this._lookupStore = db.AirportLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Name
				.required()
				.length(12, 0, 0);
		}

		_Airport = new SemanticMember()
			.localizeTitle({ ru: "Аэропорт", rus: "Аэропорты" })
			.lookup(() => sd.Airport);
				

		/** Код */
		Code = this.member()
			.localizeTitle({ ru: "Код" })
			.string(3, 3, 3)
			.required()
			.unique();

		/** Населенный пункт (англ.) */
		Settlement = this.member()
			.localizeTitle({ ru: "Населенный пункт (англ.)" })
			.string();

		/** Населенный пункт */
		LocalizedSettlement = this.member()
			.localizeTitle({ ru: "Населенный пункт" })
			.string();

		/** Широта */
		Latitude = this.member()
			.localizeTitle({ ru: "Широта" })
			.float();

		/** Долгота */
		Longitude = this.member()
			.localizeTitle({ ru: "Долгота" })
			.float();

		/** Страна */
		Country = this.member()
			.localizeTitle({ ru: "Страна", rus: "Страны" })
			.lookup(() => sd.Country)
			.required();
	

	}


	/** Авиадокумент */
	export interface IAviaDocumentSemantic extends IProductSemantic
	{
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlineName,
		//se.Number,
		//se.FullNumber,
		//se.IsReservation,
		//se.ConjunctionNumbers,
		//se.GdsPassportStatus,
		//se.GdsPassport,
		//se.PaymentForm,
		//se.PaymentDetails,
		//se.AirlinePnrCode,
		//se.Remarks,
		//se.Type,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		AirlineIataCode: SemanticMember;

		/** Код АК */
		AirlinePrefixCode: SemanticMember;

		AirlineName: SemanticMember;

		/** Номер */
		Number: SemanticMember;

		/** Номер */
		FullNumber: SemanticMember;

		ConjunctionNumbers: SemanticMember;

		/** Паспорт в GDS */
		GdsPassportStatus: SemanticMember;

		GdsPassport: SemanticMember;

		PaymentForm: SemanticMember;

		PaymentDetails: SemanticMember;

		AirlinePnrCode: SemanticMember;

		Remarks: SemanticMember;
		
		AviaMcos_InConnectionWith: SemanticMember;
	
	}

	/** Авиадокумент */
	export class AviaDocumentSemantic extends ProductSemantic implements IAviaDocumentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "AviaDocument";
			this._names = "AviaDocuments";
			this.icon("plane");
			this._isEntity = true;
			this._localizeTitle({ ru: "Авиадокумент", rus: "Авиадокументы" });

			this._getDerivedEntities = () => [
				sd.AviaTicket, sd.AviaRefund, sd.AviaMco
			];

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.AviaDocuments;
			this._saveStore = db.AviaDocuments;
			this._lookupStore = db.AviaDocumentLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.Type
				.localizeTitle({ ru: "Вид услуги" })
				.enum(ProductType, "AviaTicket", "AviaRefund", "AviaMco")
				.required()
				.length(12, 0, 0);

			this.Name
				.localizeTitle({ ru: "Номер" })
				.length(10, 0, 0);

			this.ReissueFor
				.lookup(() => sd.AviaDocument);

			this.Producer
				.localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" })
				.lookup(() => sd.Airline);
		}

		_AviaDocument = new SemanticMember()
			.localizeTitle({ ru: "Авиадокумент", rus: "Авиадокументы" })
			.lookup(() => sd.AviaDocument);
				

		AirlineIataCode = this.member()
			.string();

		/** Код АК */
		AirlinePrefixCode = this.member()
			.localizeTitle({ ru: "Код АК" })
			.string()
			.length(3, 3, 3);

		AirlineName = this.member()
			.string();

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.length(10, 10, 10);

		/** Номер */
		FullNumber = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.calculated()
			.nonsaved();

		ConjunctionNumbers = this.member()
			.string();

		/** Паспорт в GDS */
		GdsPassportStatus = this.member()
			.localizeTitle({ ru: "Паспорт в GDS" })
			.enum(GdsPassportStatus)
			.required();

		GdsPassport = this.member()
			.string();

		PaymentForm = this.member()
			.string();

		PaymentDetails = this.member()
			.string();

		AirlinePnrCode = this.member()
			.string();

		Remarks = this.member()
			.string();

	
		AviaMcos_InConnectionWith = this.collection(() => sd.AviaMco, se => se.InConnectionWith);
	

	}


	/** Автобусный билет или возврат */
	export interface IBusDocumentSemantic extends IProductSemantic
	{
		//se.Number,
		//se.DeparturePlace,
		//se.DepartureDate,
		//se.DepartureTime,
		//se.ArrivalPlace,
		//se.ArrivalDate,
		//se.ArrivalTime,
		//se.SeatNumber,
		//se.Type,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Номер */
		Number: SemanticMember;

		/** Начальная станция */
		DeparturePlace: SemanticMember;

		/** Дата отправления */
		DepartureDate: SemanticMember;

		/** Время отправления */
		DepartureTime: SemanticMember;

		/** Конечная станция */
		ArrivalPlace: SemanticMember;

		/** Дата прибытия */
		ArrivalDate: SemanticMember;

		/** Время прибытия */
		ArrivalTime: SemanticMember;

		/** Номер места */
		SeatNumber: SemanticMember;
	
	}

	/** Автобусный билет или возврат */
	export class BusDocumentSemantic extends ProductSemantic implements IBusDocumentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "BusDocument";
			this._names = "BusDocuments";
			this.icon("bus");
			this._isEntity = true;
			this._localizeTitle({ ru: "Автобусный билет или возврат", rus: "Автобусные билеты и возвраты" });

			this._getDerivedEntities = () => [
				sd.BusTicket, sd.BusTicketRefund
			];

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.BusDocuments;
			this._saveStore = db.BusDocuments;
			this._lookupStore = db.BusDocumentLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.Type
				.localizeTitle({ ru: "Вид услуги" })
				.enum(ProductType, "BusTicket", "BusTicketRefund")
				.required()
				.length(12, 0, 0);

			this.RefundedProduct
				.lookup(() => sd.BusTicket);

			this.ReissueFor
				.lookup(() => sd.BusTicket);

			this.Provider
				.lookup(() => sd.BusTicketProvider);
		}

		_BusDocument = new SemanticMember()
			.localizeTitle({ ru: "Автобусный билет или возврат", rus: "Автобусные билеты и возвраты" })
			.lookup(() => sd.BusDocument);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string();

		/** Начальная станция */
		DeparturePlace = this.member()
			.localizeTitle({ ru: "Начальная станция", ruShort: "место" })
			.emptyText("место")
			.string();

		/** Дата отправления */
		DepartureDate = this.member()
			.localizeTitle({ ru: "Дата отправления", ruShort: "дата" })
			.emptyText("дата")
			.date();

		/** Время отправления */
		DepartureTime = this.member()
			.localizeTitle({ ru: "Время отправления", ruShort: "время" })
			.emptyText("время")
			.string();

		/** Конечная станция */
		ArrivalPlace = this.member()
			.localizeTitle({ ru: "Конечная станция", ruShort: "место" })
			.emptyText("место")
			.string();

		/** Дата прибытия */
		ArrivalDate = this.member()
			.localizeTitle({ ru: "Дата прибытия", ruShort: "дата" })
			.emptyText("дата")
			.date();

		/** Время прибытия */
		ArrivalTime = this.member()
			.localizeTitle({ ru: "Время прибытия", ruShort: "время" })
			.emptyText("время")
			.string();

		/** Номер места */
		SeatNumber = this.member()
			.localizeTitle({ ru: "Номер места" })
			.string();
	

	}


	/** Аренда автомобиля */
	export interface ICarRentalSemantic extends IProductSemantic
	{
		//se.Type,
		//se.CarBrand,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Марка авто */
		CarBrand: SemanticMember;
	
	}

	/** Аренда автомобиля */
	export class CarRentalSemantic extends ProductSemantic implements ICarRentalSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "CarRental";
			this._names = "CarRentals";
			this.icon("car");
			this._isEntity = true;
			this._localizeTitle({ ru: "Аренда автомобиля", rus: "Аренды автомобилей", ua: "Оренда автомобіля" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.CarRentals;
			this._saveStore = db.CarRentals;
			this._lookupStore = db.CarRentalLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.ReissueFor
				.lookup(() => sd.CarRental);

			this.Provider
				.lookup(() => sd.CarRentalProvider);
		}

		_CarRental = new SemanticMember()
			.localizeTitle({ ru: "Аренда автомобиля", rus: "Аренды автомобилей", ua: "Оренда автомобіля" })
			.lookup(() => sd.CarRental);
				

		/** Марка авто */
		CarBrand = this.member()
			.localizeTitle({ ru: "Марка авто" })
			.string();
	

	}


	/** ПКО */
	export interface ICashInOrderPaymentSemantic extends IPaymentSemantic
	{
		//se.PaymentForm,
		//se.DocumentUniqueCode,
		//se.Number,
		//se.Date,
		//se.DocumentNumber,
		//se.InvoiceDate,
		//se.Amount,
		//se.Vat,
		//se.ReceivedFrom,
		//se.PostedOn,
		//se.SavePosted,
		//se.Note,
		//se.IsVoid,
		//se.IsPosted,
		//se.PrintedDocument,
		//se.LastChangedPropertyName,
		//se.Payer,
		//se.Order,
		//se.Invoice,
		//se.AssignedTo,
		//se.RegisteredBy,
		//se.Owner,
		//se.PaymentSystem,
	
	}

	/** ПКО */
	export class CashInOrderPaymentSemantic extends PaymentSemantic implements ICashInOrderPaymentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "CashInOrderPayment";
			this._names = "CashInOrderPayments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "ПКО" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Payment;

			this._className = "Payment";
			this._getRootEntity = () => sd.Payment;
			this._store = db.CashInOrderPayments;
			this._saveStore = db.CashInOrderPayments;
			this._lookupStore = db.CashInOrderPaymentLookup;
			this._lookupFields = { id: "Id", name: "Number" };

			this.DocumentNumber
				.localizeTitle({ ru: "№ ПКО" });
		}

		_CashInOrderPayment = new SemanticMember()
			.localizeTitle({ ru: "ПКО" })
			.lookup(() => sd.CashInOrderPayment);
				
	

	}


	/** РКО */
	export interface ICashOutOrderPaymentSemantic extends IPaymentSemantic
	{
		//se.PaymentForm,
		//se.DocumentUniqueCode,
		//se.Number,
		//se.Date,
		//se.DocumentNumber,
		//se.InvoiceDate,
		//se.Amount,
		//se.Vat,
		//se.ReceivedFrom,
		//se.PostedOn,
		//se.SavePosted,
		//se.Note,
		//se.IsVoid,
		//se.IsPosted,
		//se.PrintedDocument,
		//se.LastChangedPropertyName,
		//se.Payer,
		//se.Order,
		//se.Invoice,
		//se.AssignedTo,
		//se.RegisteredBy,
		//se.Owner,
		//se.PaymentSystem,
	
	}

	/** РКО */
	export class CashOutOrderPaymentSemantic extends PaymentSemantic implements ICashOutOrderPaymentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "CashOutOrderPayment";
			this._names = "CashOutOrderPayments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "РКО" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Payment;

			this._className = "Payment";
			this._getRootEntity = () => sd.Payment;
			this._store = db.CashOutOrderPayments;
			this._saveStore = db.CashOutOrderPayments;
			this._lookupStore = db.CashOutOrderPaymentLookup;
			this._lookupFields = { id: "Id", name: "Number" };

			this.DocumentNumber
				.localizeTitle({ ru: "№ РКО" });
		}

		_CashOutOrderPayment = new SemanticMember()
			.localizeTitle({ ru: "РКО" })
			.lookup(() => sd.CashOutOrderPayment);
				
	

	}


	/** Кассовый чек */
	export interface ICheckPaymentSemantic extends IPaymentSemantic
	{
		//se.PaymentForm,
		//se.Number,
		//se.Date,
		//se.DocumentNumber,
		//se.DocumentUniqueCode,
		//se.InvoiceDate,
		//se.Amount,
		//se.Vat,
		//se.ReceivedFrom,
		//se.PostedOn,
		//se.SavePosted,
		//se.Note,
		//se.IsVoid,
		//se.IsPosted,
		//se.PrintedDocument,
		//se.LastChangedPropertyName,
		//se.Payer,
		//se.Order,
		//se.Invoice,
		//se.AssignedTo,
		//se.RegisteredBy,
		//se.Owner,
		//se.PaymentSystem,
	
	}

	/** Кассовый чек */
	export class CheckPaymentSemantic extends PaymentSemantic implements ICheckPaymentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "CheckPayment";
			this._names = "CheckPayments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Кассовый чек" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Payment;

			this._className = "Payment";
			this._getRootEntity = () => sd.Payment;
			this._store = db.CheckPayments;
			this._saveStore = db.CheckPayments;
			this._lookupStore = db.CheckPaymentLookup;
			this._lookupFields = { id: "Id", name: "Number" };

			this.DocumentNumber
				.localizeTitle({ ru: "№ чека", ruShort: "автоматически" })
				.emptyText("автоматически");
		}

		_CheckPayment = new SemanticMember()
			.localizeTitle({ ru: "Кассовый чек" })
			.lookup(() => sd.CheckPayment);
				
	

	}


	/** Страна */
	export interface ICountrySemantic extends IEntity3Semantic
	{
		//se.TwoCharCode,
		//se.ThreeCharCode,
		//se.Name,
		//se.LastChangedPropertyName,

		/** Код (2-х сим.) */
		TwoCharCode: SemanticMember;

		/** Код (3-х сим.) */
		ThreeCharCode: SemanticMember;
		
		Airports: SemanticMember;
	
	}

	/** Страна */
	export class CountrySemantic extends Entity3Semantic implements ICountrySemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Country";
			this._names = "Countries";
			this.icon("globe");
			this._isEntity = true;
			this._localizeTitle({ ru: "Страна", rus: "Страны" });

			this._getDerivedEntities = null;

			this._className = "Country";
			this._getRootEntity = () => sd.Country;
			this._store = db.Countries;
			this._saveStore = db.Countries;
			this._lookupStore = db.CountryLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Name
				.length(16, 0, 0);
		}

		_Country = new SemanticMember()
			.localizeTitle({ ru: "Страна", rus: "Страны" })
			.lookup(() => sd.Country);
				

		/** Код (2-х сим.) */
		TwoCharCode = this.member()
			.localizeTitle({ ru: "Код (2-х сим.)" })
			.string(2, undefined, undefined);

		/** Код (3-х сим.) */
		ThreeCharCode = this.member()
			.localizeTitle({ ru: "Код (3-х сим.)" })
			.string(3, undefined, undefined);

	
		Airports = this.collection(() => sd.Airport, se => se.Country);
	

	}


	/** Электронный платеж */
	export interface IElectronicPaymentSemantic extends IPaymentSemantic
	{
		//se.PaymentForm,
		//se.AuthorizationCode,
		//se.Number,
		//se.Date,
		//se.DocumentNumber,
		//se.DocumentUniqueCode,
		//se.InvoiceDate,
		//se.Amount,
		//se.Vat,
		//se.ReceivedFrom,
		//se.PostedOn,
		//se.SavePosted,
		//se.Note,
		//se.IsVoid,
		//se.IsPosted,
		//se.PrintedDocument,
		//se.LastChangedPropertyName,
		//se.Payer,
		//se.Order,
		//se.Invoice,
		//se.AssignedTo,
		//se.RegisteredBy,
		//se.Owner,
		//se.PaymentSystem,

		/** Код авторизации */
		AuthorizationCode: SemanticMember;
	
	}

	/** Электронный платеж */
	export class ElectronicPaymentSemantic extends PaymentSemantic implements IElectronicPaymentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ElectronicPayment";
			this._names = "ElectronicPayments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Электронный платеж" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Payment;

			this._className = "Payment";
			this._getRootEntity = () => sd.Payment;
			this._store = db.ElectronicPayments;
			this._saveStore = db.ElectronicPayments;
			this._lookupStore = db.ElectronicPaymentLookup;
			this._lookupFields = { id: "Id", name: "Number" };

			this.DocumentNumber
				.localizeTitle({ ru: "№ транзакции" });
		}

		_ElectronicPayment = new SemanticMember()
			.localizeTitle({ ru: "Электронный платеж" })
			.lookup(() => sd.ElectronicPayment);
				

		/** Код авторизации */
		AuthorizationCode = this.member()
			.localizeTitle({ ru: "Код авторизации" })
			.string();
	

	}


	/** Экскурсия */
	export interface IExcursionSemantic extends IProductSemantic
	{
		//se.Type,
		//se.TourName,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Название тура */
		TourName: SemanticMember;
	
	}

	/** Экскурсия */
	export class ExcursionSemantic extends ProductSemantic implements IExcursionSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Excursion";
			this._names = "Excursions";
			this.icon("photo");
			this._isEntity = true;
			this._localizeTitle({ ru: "Экскурсия", rus: "Экскурсии", ua: "Екскурсія" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.Excursions;
			this._saveStore = db.Excursions;
			this._lookupStore = db.ExcursionLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.ReissueFor
				.lookup(() => sd.Excursion);
		}

		_Excursion = new SemanticMember()
			.localizeTitle({ ru: "Экскурсия", rus: "Экскурсии", ua: "Екскурсія" })
			.lookup(() => sd.Excursion);
				

		/** Название тура */
		TourName = this.member()
			.localizeTitle({ ru: "Название тура" })
			.string()
			.required();
	

	}


	/** Gds-файл */
	export interface IGdsFileSemantic extends IEntity3Semantic
	{
		//se.FileType,
		//se.TimeStamp,
		//se.Content,
		//se.ImportResult,
		//se.ImportOutput,
		//se.Name,
		//se.LastChangedPropertyName,

		/** Тип */
		FileType: SemanticMember;

		/** Дата импорта */
		TimeStamp: SemanticMember;

		/** Содержимое */
		Content: SemanticMember;

		/** Результат импорта */
		ImportResult: SemanticMember;

		/** Журнал */
		ImportOutput: SemanticMember;
		
		Products: SemanticMember;
	
	}

	/** Gds-файл */
	export class GdsFileSemantic extends Entity3Semantic implements IGdsFileSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "GdsFile";
			this._names = "GdsFiles";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Gds-файл", rus: "Gds-файлы" });

			this._getDerivedEntities = null;

			this._className = "GdsFile";
			this._getRootEntity = () => sd.GdsFile;
			this._store = db.GdsFiles;
			this._saveStore = db.GdsFiles;
			this._lookupStore = db.GdsFileLookup;
			this._lookupFields = { id: "Id", name: "Name" };
		}

		_GdsFile = new SemanticMember()
			.localizeTitle({ ru: "Gds-файл", rus: "Gds-файлы" })
			.lookup(() => sd.GdsFile);
				

		/** Тип */
		FileType = this.member()
			.localizeTitle({ ru: "Тип" })
			.enum(GdsFileType)
			.required();

		/** Дата импорта */
		TimeStamp = this.member()
			.localizeTitle({ ru: "Дата импорта" })
			.dateTime2()
			.required()
			.entityDate();

		/** Содержимое */
		Content = this.member()
			.localizeTitle({ ru: "Содержимое" })
			.codeText(8);

		/** Результат импорта */
		ImportResult = this.member()
			.localizeTitle({ ru: "Результат импорта" })
			.enum(ImportResult)
			.required();

		/** Журнал */
		ImportOutput = this.member()
			.localizeTitle({ ru: "Журнал" })
			.string();

	
		Products = this.collection(() => sd.Product, se => se.OriginalDocument);
	

	}


	/** Дополнительная услуга */
	export interface IGenericProductSemantic extends IProductSemantic
	{
		//se.Type,
		//se.Number,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.GenericType,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Номер */
		Number: SemanticMember;

		/** Вид услуги */
		GenericType: SemanticMember;
	
	}

	/** Дополнительная услуга */
	export class GenericProductSemantic extends ProductSemantic implements IGenericProductSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "GenericProduct";
			this._names = "GenericProducts";
			this.icon("suitcase");
			this._isEntity = true;
			this._localizeTitle({ ru: "Дополнительная услуга", rus: "Дополнительные услуги", ua: "Додаткова послуга" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.GenericProducts;
			this._saveStore = db.GenericProducts;
			this._lookupStore = db.GenericProductLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.ReissueFor
				.lookup(() => sd.GenericProduct);

			this.Provider
				.lookup(() => sd.GenericProductProvider);
		}

		_GenericProduct = new SemanticMember()
			.localizeTitle({ ru: "Дополнительная услуга", rus: "Дополнительные услуги", ua: "Додаткова послуга" })
			.lookup(() => sd.GenericProduct);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string();

		/** Вид услуги */
		GenericType = this.member()
			.localizeTitle({ ru: "Вид услуги" })
			.lookup(() => sd.GenericProductType)
			.required();
	

	}


	/** Вид дополнительной услуги */
	export interface IGenericProductTypeSemantic extends IEntity3Semantic
	{
		//se.Name,
		//se.LastChangedPropertyName,
	
	}

	/** Вид дополнительной услуги */
	export class GenericProductTypeSemantic extends Entity3Semantic implements IGenericProductTypeSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "GenericProductType";
			this._names = "GenericProductTypes";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Вид дополнительной услуги", rus: "Виды дополнительных услуг" });

			this._getDerivedEntities = null;

			this._className = "GenericProductType";
			this._getRootEntity = () => sd.GenericProductType;
			this._store = db.GenericProductTypes;
			this._saveStore = db.GenericProductTypes;
			this._lookupStore = db.GenericProductTypeLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.small();
		}

		_GenericProductType = new SemanticMember()
			.localizeTitle({ ru: "Вид дополнительной услуги", rus: "Виды дополнительных услуг" })
			.lookup(() => sd.GenericProductType);
				
	

	}


	/** Страховка или возврат */
	export interface IInsuranceDocumentSemantic extends IProductSemantic
	{
		//se.Number,
		//se.Type,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Номер */
		Number: SemanticMember;
	
	}

	/** Страховка или возврат */
	export class InsuranceDocumentSemantic extends ProductSemantic implements IInsuranceDocumentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "InsuranceDocument";
			this._names = "InsuranceDocuments";
			this.icon("fire-extinguisher");
			this._isEntity = true;
			this._localizeTitle({ ru: "Страховка или возврат", rus: "Страховки и возвраты" });

			this._getDerivedEntities = () => [
				sd.InsuranceRefund, sd.Insurance
			];

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.InsuranceDocuments;
			this._saveStore = db.InsuranceDocuments;
			this._lookupStore = db.InsuranceDocumentLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.Type
				.localizeTitle({ ru: "Вид услуги" })
				.enum(ProductType, "Insurance", "InsuranceRefund")
				.required()
				.length(12, 0, 0);

			this.ReissueFor
				.lookup(() => sd.Insurance);

			this.Producer
				.localizeTitle({ ru: "Страховая компания" })
				.lookup(() => sd.InsuranceCompany)
				.required();
		}

		_InsuranceDocument = new SemanticMember()
			.localizeTitle({ ru: "Страховка или возврат", rus: "Страховки и возвраты" })
			.lookup(() => sd.InsuranceDocument);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.required();
	

	}


	/** Студенческий билет */
	export interface IIsicSemantic extends IProductSemantic
	{
		//se.Type,
		//se.CardType,
		//se.Number1,
		//se.Number2,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Тип карты */
		CardType: SemanticMember;

		/** Номер */
		Number1: SemanticMember;

		/** Номер */
		Number2: SemanticMember;
	
	}

	/** Студенческий билет */
	export class IsicSemantic extends ProductSemantic implements IIsicSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Isic";
			this._names = "Isics";
			this.icon("graduation-cap");
			this._isEntity = true;
			this._localizeTitle({ ru: "Студенческий билет", rus: "Студенческие билеты", ua: "Студентський квиток" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.Isics;
			this._saveStore = db.Isics;
			this._lookupStore = db.IsicLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.ReissueFor
				.lookup(() => sd.Isic);
		}

		_Isic = new SemanticMember()
			.localizeTitle({ ru: "Студенческий билет", rus: "Студенческие билеты", ua: "Студентський квиток" })
			.lookup(() => sd.Isic);
				

		/** Тип карты */
		CardType = this.member()
			.localizeTitle({ ru: "Тип карты" })
			.enum(IsicCardType)
			.required()
			.defaultValue(1);

		/** Номер */
		Number1 = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.required();

		/** Номер */
		Number2 = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.required();
	

	}


	/** Контрагент */
	export interface IPartySemantic extends IEntity3Semantic
	{
		//se.Type,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,

		Type: SemanticMember;

		/** Официальное название */
		LegalName: SemanticMember;

		/** Код */
		Code: SemanticMember;

		NameForDocuments: SemanticMember;

		/** Телефон 1 */
		Phone1: SemanticMember;

		/** Телефон 2 */
		Phone2: SemanticMember;

		/** Факс */
		Fax: SemanticMember;

		/** E-mail 1 */
		Email1: SemanticMember;

		/** E-mail 2 */
		Email2: SemanticMember;

		/** Веб адрес */
		WebAddress: SemanticMember;

		/** Заказчик */
		IsCustomer: SemanticMember;

		/** Поставщик */
		IsSupplier: SemanticMember;

		/** Дополнительная информация */
		Details: SemanticMember;

		/** Юридический адрес */
		LegalAddress: SemanticMember;

		/** Фактический адрес */
		ActualAddress: SemanticMember;

		/** Примечание */
		Note: SemanticMember;

		FileCount: SemanticMember;

		/** Подчиняется */
		ReportsTo: SemanticMember;

		/** На банковский счёт по умолчанию */
		DefaultBankAccount: SemanticMember;
		
		Files: SemanticMember;
		
		DocumentOwners: SemanticMember;
	
	}

	/** Контрагент */
	export class PartySemantic extends Entity3Semantic implements IPartySemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "Party";
			this._names = "Parties";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Контрагент", rus: "Контрагенты" });

			this._getDerivedEntities = () => [
				sd.Airline, sd.Agent, sd.ActiveOwner, sd.Customer, sd.RoamingOperator, sd.Organization, sd.Person, sd.Department, sd.BusTicketProvider, sd.CarRentalProvider, sd.GenericProductProvider, sd.PasteboardProvider, sd.AccommodationProvider, sd.TransferProvider, sd.TourProvider, sd.Employee, sd.InsuranceCompany
			];

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.Parties;
			this._saveStore = db.Parties;
			this._lookupStore = db.PartyLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Name
				.length(20, 0, 0);
		}

		_Party = new SemanticMember()
			.localizeTitle({ ru: "Контрагент", rus: "Контрагенты" })
			.lookup(() => sd.Party);
				

		Type = this.member()
			.enum(PartyType)
			.required()
			.entityType();

		/** Официальное название */
		LegalName = this.member()
			.localizeTitle({ ru: "Официальное название" })
			.string();

		/** Код */
		Code = this.member()
			.localizeTitle({ ru: "Код" })
			.string()
			.icon("asterisk");

		NameForDocuments = this.member()
			.string()
			.calculated()
			.nonsaved();

		/** Телефон 1 */
		Phone1 = this.member()
			.localizeTitle({ ru: "Телефон 1" })
			.phone();

		/** Телефон 2 */
		Phone2 = this.member()
			.localizeTitle({ ru: "Телефон 2" })
			.phone();

		/** Факс */
		Fax = this.member()
			.localizeTitle({ ru: "Факс" })
			.fax();

		/** E-mail 1 */
		Email1 = this.member()
			.localizeTitle({ ru: "E-mail 1" })
			.email();

		/** E-mail 2 */
		Email2 = this.member()
			.localizeTitle({ ru: "E-mail 2" })
			.email();

		/** Веб адрес */
		WebAddress = this.member()
			.localizeTitle({ ru: "Веб адрес" })
			.hyperlink();

		/** Заказчик */
		IsCustomer = this.member()
			.localizeTitle({ ru: "Заказчик" })
			.bool()
			.required()
			.icon("street-view");

		/** Поставщик */
		IsSupplier = this.member()
			.localizeTitle({ ru: "Поставщик" })
			.bool()
			.required()
			.icon("industry");

		/** Дополнительная информация */
		Details = this.member()
			.localizeTitle({ ru: "Дополнительная информация" })
			.text(3);

		/** Юридический адрес */
		LegalAddress = this.member()
			.localizeTitle({ ru: "Юридический адрес" })
			.address(3);

		/** Фактический адрес */
		ActualAddress = this.member()
			.localizeTitle({ ru: "Фактический адрес" })
			.address(3);

		/** Примечание */
		Note = this.member()
			.localizeTitle({ ru: "Примечание" })
			.text(3);

		FileCount = this.member()
			.int()
			.calculated()
			.nonsaved();

		/** Подчиняется */
		ReportsTo = this.member()
			.localizeTitle({ ru: "Подчиняется" })
			.lookup(() => sd.Party);

		/** На банковский счёт по умолчанию */
		DefaultBankAccount = this.member()
			.localizeTitle({ ru: "На банковский счёт по умолчанию", ruDesc: "По умолчаничанию оплачивать через выбранный банковский счёт агенства" })
			.lookup(() => sd.BankAccount);

	
		Files = this.collection(() => sd.File, se => se.Party);

	
		DocumentOwners = this.collection(() => sd.DocumentOwner, se => se.Owner);
	

	}


	/** Платёжная система */
	export interface IPaymentSystemSemantic extends IEntity3Semantic
	{
		//se.Name,
		//se.LastChangedPropertyName,
	
	}

	/** Платёжная система */
	export class PaymentSystemSemantic extends Entity3Semantic implements IPaymentSystemSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "PaymentSystem";
			this._names = "PaymentSystems";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Платёжная система", rus: "Платёжные системы" });

			this._getDerivedEntities = null;

			this._className = "PaymentSystem";
			this._getRootEntity = () => sd.PaymentSystem;
			this._store = db.PaymentSystems;
			this._saveStore = db.PaymentSystems;
			this._lookupStore = db.PaymentSystemLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.small();
		}

		_PaymentSystem = new SemanticMember()
			.localizeTitle({ ru: "Платёжная система", rus: "Платёжные системы" })
			.lookup(() => sd.PaymentSystem);
				
	

	}


	/** Ж/д билет или возврат */
	export interface IRailwayDocumentSemantic extends IProductSemantic
	{
		//se.Number,
		//se.DeparturePlace,
		//se.DepartureDate,
		//se.DepartureTime,
		//se.ArrivalPlace,
		//se.ArrivalDate,
		//se.ArrivalTime,
		//se.TrainNumber,
		//se.CarNumber,
		//se.SeatNumber,
		//se.ServiceClass,
		//se.Type,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Номер */
		Number: SemanticMember;

		/** Начальная станция */
		DeparturePlace: SemanticMember;

		/** Дата отправления */
		DepartureDate: SemanticMember;

		/** Время отправления */
		DepartureTime: SemanticMember;

		/** Конечная станция */
		ArrivalPlace: SemanticMember;

		/** Дата прибытия */
		ArrivalDate: SemanticMember;

		/** Время прибытия */
		ArrivalTime: SemanticMember;

		/** Номер поезда */
		TrainNumber: SemanticMember;

		/** Номер вагона */
		CarNumber: SemanticMember;

		/** Номер места */
		SeatNumber: SemanticMember;

		/** Сервис-класс */
		ServiceClass: SemanticMember;
	
	}

	/** Ж/д билет или возврат */
	export class RailwayDocumentSemantic extends ProductSemantic implements IRailwayDocumentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "RailwayDocument";
			this._names = "RailwayDocuments";
			this.icon("subway");
			this._isEntity = true;
			this._localizeTitle({ ru: "Ж/д билет или возврат", rus: "Ж/д билеты и возвраты" });

			this._getDerivedEntities = () => [
				sd.PasteboardRefund, sd.Pasteboard
			];

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.RailwayDocuments;
			this._saveStore = db.RailwayDocuments;
			this._lookupStore = db.RailwayDocumentLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.Type
				.localizeTitle({ ru: "Вид услуги" })
				.enum(ProductType, "Pasteboard", "PasteboardRefund")
				.required()
				.length(12, 0, 0);

			this.Number
				.length(24, 0, 0);

			this.Provider
				.lookup(() => sd.PasteboardProvider);

			this.RefundedProduct
				.lookup(() => sd.Pasteboard);

			this.ReissueFor
				.lookup(() => sd.Pasteboard);
		}

		_RailwayDocument = new SemanticMember()
			.localizeTitle({ ru: "Ж/д билет или возврат", rus: "Ж/д билеты и возвраты" })
			.lookup(() => sd.RailwayDocument);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string();

		/** Начальная станция */
		DeparturePlace = this.member()
			.localizeTitle({ ru: "Начальная станция", ruShort: "место" })
			.emptyText("место")
			.string()
			.length(20, 0, 0);

		/** Дата отправления */
		DepartureDate = this.member()
			.localizeTitle({ ru: "Дата отправления", ruShort: "дата" })
			.emptyText("дата")
			.date();

		/** Время отправления */
		DepartureTime = this.member()
			.localizeTitle({ ru: "Время отправления", ruShort: "время" })
			.emptyText("время")
			.string();

		/** Конечная станция */
		ArrivalPlace = this.member()
			.localizeTitle({ ru: "Конечная станция", ruShort: "место" })
			.emptyText("место")
			.string()
			.length(20, 0, 0);

		/** Дата прибытия */
		ArrivalDate = this.member()
			.localizeTitle({ ru: "Дата прибытия", ruShort: "дата" })
			.emptyText("дата")
			.date();

		/** Время прибытия */
		ArrivalTime = this.member()
			.localizeTitle({ ru: "Время прибытия", ruShort: "время" })
			.emptyText("время")
			.string();

		/** Номер поезда */
		TrainNumber = this.member()
			.localizeTitle({ ru: "Номер поезда" })
			.string();

		/** Номер вагона */
		CarNumber = this.member()
			.localizeTitle({ ru: "Номер вагона" })
			.string();

		/** Номер места */
		SeatNumber = this.member()
			.localizeTitle({ ru: "Номер места" })
			.string();

		/** Сервис-класс */
		ServiceClass = this.member()
			.localizeTitle({ ru: "Сервис-класс" })
			.enum(PasteboardServiceClass)
			.required()
			.defaultValue(0);
	

	}


	/** SIM-карта */
	export interface ISimCardSemantic extends IProductSemantic
	{
		//se.Type,
		//se.Number,
		//se.IsSale,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Номер */
		Number: SemanticMember;

		/** Продажа SIM-карты */
		IsSale: SemanticMember;
	
	}

	/** SIM-карта */
	export class SimCardSemantic extends ProductSemantic implements ISimCardSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "SimCard";
			this._names = "SimCards";
			this.icon("mobile");
			this._isEntity = true;
			this._localizeTitle({ ru: "SIM-карта", rus: "SIM-карты", ua: "SIM-картка" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.SimCards;
			this._saveStore = db.SimCards;
			this._lookupStore = db.SimCardLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.ReissueFor
				.lookup(() => sd.SimCard);

			this.Producer
				.localizeTitle({ ru: "Оператор" })
				.lookup(() => sd.RoamingOperator)
				.required();
		}

		_SimCard = new SemanticMember()
			.localizeTitle({ ru: "SIM-карта", rus: "SIM-карты", ua: "SIM-картка" })
			.lookup(() => sd.SimCard);
				

		/** Номер */
		Number = this.member()
			.localizeTitle({ ru: "Номер" })
			.string()
			.required();

		/** Продажа SIM-карты */
		IsSale = this.member()
			.localizeTitle({ ru: "Продажа SIM-карты" })
			.bool()
			.required();
	

	}


	/** Турпакет */
	export interface ITourSemantic extends IProductSemantic
	{
		//se.Type,
		//se.HotelName,
		//se.HotelOffice,
		//se.HotelCode,
		//se.PlacementName,
		//se.PlacementOffice,
		//se.PlacementCode,
		//se.AviaDescription,
		//se.TransferDescription,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.AccommodationType,
		//se.CateringType,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Гостиница */
		HotelName: SemanticMember;

		/** Офис гостиницы */
		HotelOffice: SemanticMember;

		/** Код гостиницы */
		HotelCode: SemanticMember;

		/** Расположение */
		PlacementName: SemanticMember;

		PlacementOffice: SemanticMember;

		PlacementCode: SemanticMember;

		/** Авиа (описание) */
		AviaDescription: SemanticMember;

		/** Трансфер (описание) */
		TransferDescription: SemanticMember;

		/** Тип проживания */
		AccommodationType: SemanticMember;

		/** Тип питания */
		CateringType: SemanticMember;
	
	}

	/** Турпакет */
	export class TourSemantic extends ProductSemantic implements ITourSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Tour";
			this._names = "Tours";
			this.icon("suitcase");
			this._isEntity = true;
			this._localizeTitle({ ru: "Турпакет", rus: "Турпакеты", ua: "Турпакет" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.Tours;
			this._saveStore = db.Tours;
			this._lookupStore = db.TourLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.ReissueFor
				.lookup(() => sd.Tour);

			this.Provider
				.lookup(() => sd.TourProvider);
		}

		_Tour = new SemanticMember()
			.localizeTitle({ ru: "Турпакет", rus: "Турпакеты", ua: "Турпакет" })
			.lookup(() => sd.Tour);
				

		/** Гостиница */
		HotelName = this.member()
			.localizeTitle({ ru: "Гостиница" })
			.string();

		/** Офис гостиницы */
		HotelOffice = this.member()
			.localizeTitle({ ru: "Офис гостиницы", ruShort: "офис" })
			.emptyText("офис")
			.string();

		/** Код гостиницы */
		HotelCode = this.member()
			.localizeTitle({ ru: "Код гостиницы", ruShort: "код" })
			.emptyText("код")
			.string();

		/** Расположение */
		PlacementName = this.member()
			.localizeTitle({ ru: "Расположение" })
			.string();

		PlacementOffice = this.member()
			.emptyText("офис")
			.string();

		PlacementCode = this.member()
			.emptyText("код")
			.string();

		/** Авиа (описание) */
		AviaDescription = this.member()
			.localizeTitle({ ru: "Авиа (описание)" })
			.string();

		/** Трансфер (описание) */
		TransferDescription = this.member()
			.localizeTitle({ ru: "Трансфер (описание)" })
			.string();

		/** Тип проживания */
		AccommodationType = this.member()
			.localizeTitle({ ru: "Тип проживания", rus: "Типы проживания" })
			.lookup(() => sd.AccommodationType);

		/** Тип питания */
		CateringType = this.member()
			.localizeTitle({ ru: "Тип питания", rus: "Типы питания" })
			.lookup(() => sd.CateringType);
	

	}


	/** Трансфер */
	export interface ITransferSemantic extends IProductSemantic
	{
		//se.Type,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,
	
	}

	/** Трансфер */
	export class TransferSemantic extends ProductSemantic implements ITransferSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Transfer";
			this._names = "Transfers";
			this.icon("cab");
			this._isEntity = true;
			this._localizeTitle({ ru: "Трансфер", rus: "Трансферы", ua: "Трансфер" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Product;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.Transfers;
			this._saveStore = db.Transfers;
			this._lookupStore = db.TransferLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();

			this.ReissueFor
				.lookup(() => sd.Transfer);

			this.Provider
				.lookup(() => sd.TransferProvider);
		}

		_Transfer = new SemanticMember()
			.localizeTitle({ ru: "Трансфер", rus: "Трансферы", ua: "Трансфер" })
			.lookup(() => sd.Transfer);
				
	

	}


	/** Безналичный платеж */
	export interface IWireTransferSemantic extends IPaymentSemantic
	{
		//se.PaymentForm,
		//se.Number,
		//se.Date,
		//se.DocumentNumber,
		//se.DocumentUniqueCode,
		//se.InvoiceDate,
		//se.Amount,
		//se.Vat,
		//se.ReceivedFrom,
		//se.PostedOn,
		//se.SavePosted,
		//se.Note,
		//se.IsVoid,
		//se.IsPosted,
		//se.PrintedDocument,
		//se.LastChangedPropertyName,
		//se.Payer,
		//se.Order,
		//se.Invoice,
		//se.AssignedTo,
		//se.RegisteredBy,
		//se.Owner,
		//se.PaymentSystem,
	
	}

	/** Безналичный платеж */
	export class WireTransferSemantic extends PaymentSemantic implements IWireTransferSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "WireTransfer";
			this._names = "WireTransfers";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Безналичный платеж" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Payment;

			this._className = "Payment";
			this._getRootEntity = () => sd.Payment;
			this._store = db.WireTransfers;
			this._saveStore = db.WireTransfers;
			this._lookupStore = db.WireTransferLookup;
			this._lookupFields = { id: "Id", name: "Number" };

			this.DocumentNumber
				.localizeTitle({ ru: "№ платежного поручения" });

			this.Invoice
				.localizeTitle({ ru: "Счёт" })
				.lookup(() => sd.Invoice);
		}

		_WireTransfer = new SemanticMember()
			.localizeTitle({ ru: "Безналичный платеж" })
			.lookup(() => sd.WireTransfer);
				
	

	}


	/** Тип проживания */
	export interface IAccommodationTypeSemantic extends IEntity3DSemantic
	{
		//se.Description,
		//se.Name,
		//se.LastChangedPropertyName,
	
	}

	/** Тип проживания */
	export class AccommodationTypeSemantic extends Entity3DSemantic implements IAccommodationTypeSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "AccommodationType";
			this._names = "AccommodationTypes";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Тип проживания", rus: "Типы проживания" });

			this._getDerivedEntities = null;

			this._className = "AccommodationType";
			this._getRootEntity = () => sd.AccommodationType;
			this._store = db.AccommodationTypes;
			this._saveStore = db.AccommodationTypes;
			this._lookupStore = db.AccommodationTypeLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.small();
		}

		_AccommodationType = new SemanticMember()
			.localizeTitle({ ru: "Тип проживания", rus: "Типы проживания" })
			.lookup(() => sd.AccommodationType);
				
	

	}


	/** МСО */
	export interface IAviaMcoSemantic extends IAviaDocumentSemantic
	{
		//se.Type,
		//se.Description,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlineName,
		//se.Number,
		//se.FullNumber,
		//se.IsReservation,
		//se.ConjunctionNumbers,
		//se.GdsPassportStatus,
		//se.GdsPassport,
		//se.PaymentForm,
		//se.PaymentDetails,
		//se.AirlinePnrCode,
		//se.Remarks,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.InConnectionWith,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Описание */
		Description: SemanticMember;

		/** Связан с */
		InConnectionWith: SemanticMember;
	
	}

	/** МСО */
	export class AviaMcoSemantic extends AviaDocumentSemantic implements IAviaMcoSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "AviaMco";
			this._names = "AviaMcos";
			this.icon("plane");
			this._isEntity = true;
			this._localizeTitle({ ru: "МСО", rus: "МСО", ua: "MCO" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.AviaDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.AviaMcos;
			this._saveStore = db.AviaMcos;
			this._lookupStore = db.AviaMcoLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_AviaMco = new SemanticMember()
			.localizeTitle({ ru: "МСО", rus: "МСО", ua: "MCO" })
			.lookup(() => sd.AviaMco);
				

		/** Описание */
		Description = this.member()
			.localizeTitle({ ru: "Описание" })
			.text(3);

		/** Связан с */
		InConnectionWith = this.member()
			.localizeTitle({ ru: "Связан с" })
			.lookup(() => sd.AviaDocument);
	

	}


	/** Возврат авиабилета */
	export interface IAviaRefundSemantic extends IAviaDocumentSemantic
	{
		//se.Type,
		//se.IsRefund,
		//se.RefundedDocument,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlineName,
		//se.Number,
		//se.FullNumber,
		//se.IsReservation,
		//se.ConjunctionNumbers,
		//se.GdsPassportStatus,
		//se.GdsPassport,
		//se.PaymentForm,
		//se.PaymentDetails,
		//se.AirlinePnrCode,
		//se.Remarks,
		//se.Name,
		//se.IssueDate,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Исходный документ */
		RefundedDocument: SemanticMember;
	
	}

	/** Возврат авиабилета */
	export class AviaRefundSemantic extends AviaDocumentSemantic implements IAviaRefundSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "AviaRefund";
			this._names = "AviaRefunds";
			this.icon("plane");
			this._isEntity = true;
			this._localizeTitle({ ru: "Возврат авиабилета", rus: "Возвраты авиабилетов", ua: "Повернення авіаквитка" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.AviaDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.AviaRefunds;
			this._saveStore = db.AviaRefunds;
			this._lookupStore = db.AviaRefundLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_AviaRefund = new SemanticMember()
			.localizeTitle({ ru: "Возврат авиабилета", rus: "Возвраты авиабилетов", ua: "Повернення авіаквитка" })
			.lookup(() => sd.AviaRefund);
				

		/** Исходный документ */
		RefundedDocument = this.member()
			.localizeTitle({ ru: "Исходный документ" })
			.lookup(() => sd.AviaDocument)
			.calculated()
			.nonsaved();
	

	}


	/** Авиабилет */
	export interface IAviaTicketSemantic extends IAviaDocumentSemantic
	{
		//se.Type,
		//se.Departure,
		//se.Domestic,
		//se.Interline,
		//se.SegmentClasses,
		//se.Endorsement,
		//se.FareTotal,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlineName,
		//se.Number,
		//se.FullNumber,
		//se.IsReservation,
		//se.ConjunctionNumbers,
		//se.GdsPassportStatus,
		//se.GdsPassport,
		//se.PaymentForm,
		//se.PaymentDetails,
		//se.AirlinePnrCode,
		//se.Remarks,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,

		/** Дата отправления */
		Departure: SemanticMember;

		Domestic: SemanticMember;

		Interline: SemanticMember;

		/** Классы сегментов */
		SegmentClasses: SemanticMember;

		Endorsement: SemanticMember;

		FareTotal: SemanticMember;
		
		Segments: SemanticMember;
	
	}

	/** Авиабилет */
	export class AviaTicketSemantic extends AviaDocumentSemantic implements IAviaTicketSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "AviaTicket";
			this._names = "AviaTickets";
			this.icon("plane");
			this._isEntity = true;
			this._localizeTitle({ ru: "Авиабилет", rus: "Авиабилеты", ua: "Авіаквиток" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.AviaDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.AviaTickets;
			this._saveStore = db.AviaTickets;
			this._lookupStore = db.AviaTicketLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_AviaTicket = new SemanticMember()
			.localizeTitle({ ru: "Авиабилет", rus: "Авиабилеты", ua: "Авіаквиток" })
			.lookup(() => sd.AviaTicket);
				

		/** Дата отправления */
		Departure = this.member()
			.localizeTitle({ ru: "Дата отправления", ruShort: "дата" })
			.emptyText("дата")
			.date();

		Domestic = this.member()
			.bool()
			.required();

		Interline = this.member()
			.bool()
			.required();

		/** Классы сегментов */
		SegmentClasses = this.member()
			.localizeTitle({ ru: "Классы сегментов" })
			.string();

		Endorsement = this.member()
			.string();

		FareTotal = this.member()
			.money();

	
		Segments = this.collection(() => sd.FlightSegment, se => se.Ticket);
	

	}


	/** Банковский счёт */
	export interface IBankAccountSemantic extends IEntity3DSemantic
	{
		//se.IsDefault,
		//se.Note,
		//se.Description,
		//se.Name,
		//se.LastChangedPropertyName,

		/** Использовать по умолчанию */
		IsDefault: SemanticMember;

		/** Примечание */
		Note: SemanticMember;
	
	}

	/** Банковский счёт */
	export class BankAccountSemantic extends Entity3DSemantic implements IBankAccountSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "BankAccount";
			this._names = "BankAccounts";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Банковский счёт", rus: "Банковские счёта" });

			this._getDerivedEntities = null;

			this._className = "BankAccount";
			this._getRootEntity = () => sd.BankAccount;
			this._store = db.BankAccounts;
			this._saveStore = db.BankAccounts;
			this._lookupStore = db.BankAccountLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.small();
		}

		_BankAccount = new SemanticMember()
			.localizeTitle({ ru: "Банковский счёт", rus: "Банковские счёта" })
			.lookup(() => sd.BankAccount);
				

		/** Использовать по умолчанию */
		IsDefault = this.member()
			.localizeTitle({ ru: "Использовать по умолчанию" })
			.bool()
			.required();

		/** Примечание */
		Note = this.member()
			.localizeTitle({ ru: "Примечание" })
			.string();
	

	}


	/** Автобусный билет */
	export interface IBusTicketSemantic extends IBusDocumentSemantic
	{
		//se.Type,
		//se.Number,
		//se.DeparturePlace,
		//se.DepartureDate,
		//se.DepartureTime,
		//se.ArrivalPlace,
		//se.ArrivalDate,
		//se.ArrivalTime,
		//se.SeatNumber,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,
	
	}

	/** Автобусный билет */
	export class BusTicketSemantic extends BusDocumentSemantic implements IBusTicketSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "BusTicket";
			this._names = "BusTickets";
			this.icon("bus");
			this._isEntity = true;
			this._localizeTitle({ ru: "Автобусный билет", rus: "Автобусные билеты", ua: "Автобусний квиток" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.BusDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.BusTickets;
			this._saveStore = db.BusTickets;
			this._lookupStore = db.BusTicketLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_BusTicket = new SemanticMember()
			.localizeTitle({ ru: "Автобусный билет", rus: "Автобусные билеты", ua: "Автобусний квиток" })
			.lookup(() => sd.BusTicket);
				
	

	}


	/** Возврат автобусного билета */
	export interface IBusTicketRefundSemantic extends IBusDocumentSemantic
	{
		//se.Type,
		//se.IsRefund,
		//se.Number,
		//se.DeparturePlace,
		//se.DepartureDate,
		//se.DepartureTime,
		//se.ArrivalPlace,
		//se.ArrivalDate,
		//se.ArrivalTime,
		//se.SeatNumber,
		//se.Name,
		//se.IssueDate,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,
	
	}

	/** Возврат автобусного билета */
	export class BusTicketRefundSemantic extends BusDocumentSemantic implements IBusTicketRefundSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "BusTicketRefund";
			this._names = "BusTicketRefunds";
			this.icon("bus");
			this._isEntity = true;
			this._localizeTitle({ ru: "Возврат автобусного билета", rus: "Возвраты автобусных билетов", ua: "Повернення автобусного квитка" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.BusDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.BusTicketRefunds;
			this._saveStore = db.BusTicketRefunds;
			this._lookupStore = db.BusTicketRefundLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_BusTicketRefund = new SemanticMember()
			.localizeTitle({ ru: "Возврат автобусного билета", rus: "Возвраты автобусных билетов", ua: "Повернення автобусного квитка" })
			.lookup(() => sd.BusTicketRefund);
				
	

	}


	/** Тип питания */
	export interface ICateringTypeSemantic extends IEntity3DSemantic
	{
		//se.Description,
		//se.Name,
		//se.LastChangedPropertyName,
	
	}

	/** Тип питания */
	export class CateringTypeSemantic extends Entity3DSemantic implements ICateringTypeSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "CateringType";
			this._names = "CateringTypes";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Тип питания", rus: "Типы питания" });

			this._getDerivedEntities = null;

			this._className = "CateringType";
			this._getRootEntity = () => sd.CateringType;
			this._store = db.CateringTypes;
			this._saveStore = db.CateringTypes;
			this._lookupStore = db.CateringTypeLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.small();
		}

		_CateringType = new SemanticMember()
			.localizeTitle({ ru: "Тип питания", rus: "Типы питания" })
			.lookup(() => sd.CateringType);
				
	

	}


	/** Подразделение */
	export interface IDepartmentSemantic extends IPartySemantic
	{
		//se.Type,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.Organization,
		//se.ReportsTo,
		//se.DefaultBankAccount,

		/** Организация */
		Organization: SemanticMember;
	
	}

	/** Подразделение */
	export class DepartmentSemantic extends PartySemantic implements IDepartmentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Department";
			this._names = "Departments";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Подразделение", rus: "Подразделения" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Party;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.Departments;
			this._saveStore = db.Departments;
			this._lookupStore = db.DepartmentLookup;
			this._lookupFields = { id: "Id", name: "Name" };
		}

		_Department = new SemanticMember()
			.localizeTitle({ ru: "Подразделение", rus: "Подразделения" })
			.lookup(() => sd.Department);
				

		/** Организация */
		Organization = this.member()
			.localizeTitle({ ru: "Организация", rus: "Организации" })
			.lookup(() => sd.Organization);
	

	}


	
	export interface IIdentitySemantic extends IEntity3DSemantic
	{
		//se.Description,
		//se.Name,
		//se.LastChangedPropertyName,
	
	}

	
	export class IdentitySemantic extends Entity3DSemantic implements IIdentitySemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "Identity";
			this._names = "Identities";
			
			this._isEntity = true;

			this._getDerivedEntities = () => [
				sd.InternalIdentity, sd.User
			];

			this._className = "Identity";
			this._getRootEntity = () => sd.Identity;
			this._store = db.Identities;
			this._saveStore = db.Identities;
			this._lookupStore = db.IdentityLookup;
			this._lookupFields = { id: "Id", name: "Name" };
		}

		_Identity = new SemanticMember()
			.lookup(() => sd.Identity);
				
	

	}


	/** Страховка */
	export interface IInsuranceSemantic extends IInsuranceDocumentSemantic
	{
		//se.Type,
		//se.Number,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,
	
	}

	/** Страховка */
	export class InsuranceSemantic extends InsuranceDocumentSemantic implements IInsuranceSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Insurance";
			this._names = "Insurances";
			this.icon("fire-extinguisher");
			this._isEntity = true;
			this._localizeTitle({ ru: "Страховка", rus: "Страховки", ua: "Страховка" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.InsuranceDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.Insurances;
			this._saveStore = db.Insurances;
			this._lookupStore = db.InsuranceLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_Insurance = new SemanticMember()
			.localizeTitle({ ru: "Страховка", rus: "Страховки", ua: "Страховка" })
			.lookup(() => sd.Insurance);
				
	

	}


	/** Возврат страховки */
	export interface IInsuranceRefundSemantic extends IInsuranceDocumentSemantic
	{
		//se.Type,
		//se.IsRefund,
		//se.Number,
		//se.Name,
		//se.IssueDate,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,
	
	}

	/** Возврат страховки */
	export class InsuranceRefundSemantic extends InsuranceDocumentSemantic implements IInsuranceRefundSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "InsuranceRefund";
			this._names = "InsuranceRefunds";
			this.icon("fire-extinguisher");
			this._isEntity = true;
			this._localizeTitle({ ru: "Возврат страховки", rus: "Возвраты страховок", ua: "Повернення страховки" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.InsuranceDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.InsuranceRefunds;
			this._saveStore = db.InsuranceRefunds;
			this._lookupStore = db.InsuranceRefundLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_InsuranceRefund = new SemanticMember()
			.localizeTitle({ ru: "Возврат страховки", rus: "Возвраты страховок", ua: "Повернення страховки" })
			.lookup(() => sd.InsuranceRefund);
				
	

	}


	/** Организация */
	export interface IOrganizationSemantic extends IPartySemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,

		/** Авиакомпания */
		IsAirline: SemanticMember;

		/** IATA код */
		AirlineIataCode: SemanticMember;

		/** Prefix код */
		AirlinePrefixCode: SemanticMember;

		/** Требование паспортных данных */
		AirlinePassportRequirement: SemanticMember;

		/** Провайдер проживания */
		IsAccommodationProvider: SemanticMember;

		/** Провайдер автобусных билетов */
		IsBusTicketProvider: SemanticMember;

		/** Провайдер аренды авто */
		IsCarRentalProvider: SemanticMember;

		/** Провайдер ж/д билетов */
		IsPasteboardProvider: SemanticMember;

		/** Провайдер турпакетов */
		IsTourProvider: SemanticMember;

		/** Провайдер трансферов */
		IsTransferProvider: SemanticMember;

		/** Провайдер дополнительных услуг */
		IsGenericProductProvider: SemanticMember;

		/** Провайдером услуг */
		IsProvider: SemanticMember;

		/** Страховая компания */
		IsInsuranceCompany: SemanticMember;

		/** Роуминг-оператор */
		IsRoamingOperator: SemanticMember;

		DepartmentCount: SemanticMember;

		EmployeeCount: SemanticMember;
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Организация */
	export class OrganizationSemantic extends PartySemantic implements IOrganizationSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Organization";
			this._names = "Organizations";
			this.icon("group");
			this._isEntity = true;
			this._localizeTitle({ ru: "Организация", rus: "Организации" });

			this._getDerivedEntities = () => [
				sd.Airline, sd.RoamingOperator, sd.BusTicketProvider, sd.CarRentalProvider, sd.GenericProductProvider, sd.PasteboardProvider, sd.AccommodationProvider, sd.TransferProvider, sd.TourProvider, sd.InsuranceCompany
			];

			this._getBaseEntity = () => sd.Party;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.Organizations;
			this._saveStore = db.Organizations;
			this._lookupStore = db.OrganizationLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_Organization = new SemanticMember()
			.localizeTitle({ ru: "Организация", rus: "Организации" })
			.lookup(() => sd.Organization);
				

		/** Авиакомпания */
		IsAirline = this.member()
			.localizeTitle({ ru: "Авиакомпания" })
			.bool()
			.required()
			.icon("plane");

		/** IATA код */
		AirlineIataCode = this.member()
			.localizeTitle({ ru: "IATA код" })
			.string();

		/** Prefix код */
		AirlinePrefixCode = this.member()
			.localizeTitle({ ru: "Prefix код" })
			.string();

		/** Требование паспортных данных */
		AirlinePassportRequirement = this.member()
			.localizeTitle({ ru: "Требование паспортных данных" })
			.enum(AirlinePassportRequirement)
			.required();

		/** Провайдер проживания */
		IsAccommodationProvider = this.member()
			.localizeTitle({ ru: "Провайдер проживания" })
			.bool()
			.required()
			.icon("bed");

		/** Провайдер автобусных билетов */
		IsBusTicketProvider = this.member()
			.localizeTitle({ ru: "Провайдер автобусных билетов" })
			.bool()
			.required()
			.icon("bus");

		/** Провайдер аренды авто */
		IsCarRentalProvider = this.member()
			.localizeTitle({ ru: "Провайдер аренды авто" })
			.bool()
			.required()
			.icon("car");

		/** Провайдер ж/д билетов */
		IsPasteboardProvider = this.member()
			.localizeTitle({ ru: "Провайдер ж/д билетов" })
			.bool()
			.required()
			.icon("subway");

		/** Провайдер турпакетов */
		IsTourProvider = this.member()
			.localizeTitle({ ru: "Провайдер турпакетов" })
			.bool()
			.required()
			.icon("suitcase");

		/** Провайдер трансферов */
		IsTransferProvider = this.member()
			.localizeTitle({ ru: "Провайдер трансферов" })
			.bool()
			.required()
			.icon("cab");

		/** Провайдер дополнительных услуг */
		IsGenericProductProvider = this.member()
			.localizeTitle({ ru: "Провайдер дополнительных услуг" })
			.bool()
			.required()
			.icon("suitcase");

		/** Провайдером услуг */
		IsProvider = this.member()
			.localizeTitle({ ru: "Провайдером услуг" })
			.bool()
			.required();

		/** Страховая компания */
		IsInsuranceCompany = this.member()
			.localizeTitle({ ru: "Страховая компания" })
			.bool()
			.required()
			.icon("");

		/** Роуминг-оператор */
		IsRoamingOperator = this.member()
			.localizeTitle({ ru: "Роуминг-оператор" })
			.bool()
			.required()
			.icon("mobile");

		DepartmentCount = this.member()
			.int()
			.calculated()
			.nonsaved()
			.required();

		EmployeeCount = this.member()
			.int()
			.calculated()
			.nonsaved()
			.required();

	
		MilesCards = this.collection(() => sd.MilesCard, se => se.Organization);

	
		Departments = this.collection(() => sd.Department, se => se.Organization);

		/** Сотрудники */
		Employees = this.collection(() => sd.Person, se => se.Organization, m => m
			.localizeTitle({ ru: "Сотрудники" }));

	
		AirlineServiceClasses = this.collection(() => sd.AirlineServiceClass, se => se.Airline);
	

	}


	/** Ж/д билет */
	export interface IPasteboardSemantic extends IRailwayDocumentSemantic
	{
		//se.Type,
		//se.Number,
		//se.DeparturePlace,
		//se.DepartureDate,
		//se.DepartureTime,
		//se.ArrivalPlace,
		//se.ArrivalDate,
		//se.ArrivalTime,
		//se.TrainNumber,
		//se.CarNumber,
		//se.SeatNumber,
		//se.ServiceClass,
		//se.Name,
		//se.IssueDate,
		//se.IsRefund,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,
	
	}

	/** Ж/д билет */
	export class PasteboardSemantic extends RailwayDocumentSemantic implements IPasteboardSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Pasteboard";
			this._names = "Pasteboards";
			this.icon("subway");
			this._isEntity = true;
			this._localizeTitle({ ru: "Ж/д билет", rus: "Ж/д билеты", ua: "Залізничний квиток" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.RailwayDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.Pasteboards;
			this._saveStore = db.Pasteboards;
			this._lookupStore = db.PasteboardLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_Pasteboard = new SemanticMember()
			.localizeTitle({ ru: "Ж/д билет", rus: "Ж/д билеты", ua: "Залізничний квиток" })
			.lookup(() => sd.Pasteboard);
				
	

	}


	/** Возврат ж/д билета */
	export interface IPasteboardRefundSemantic extends IRailwayDocumentSemantic
	{
		//se.Type,
		//se.IsRefund,
		//se.Number,
		//se.DeparturePlace,
		//se.DepartureDate,
		//se.DepartureTime,
		//se.ArrivalPlace,
		//se.ArrivalDate,
		//se.ArrivalTime,
		//se.TrainNumber,
		//se.CarNumber,
		//se.SeatNumber,
		//se.ServiceClass,
		//se.Name,
		//se.IssueDate,
		//se.IsReservation,
		//se.IsProcessed,
		//se.IsVoid,
		//se.RequiresProcessing,
		//se.IsDelivered,
		//se.IsPaid,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.PnrCode,
		//se.TourCode,
		//se.BookerOffice,
		//se.BookerCode,
		//se.TicketerOffice,
		//se.TicketerCode,
		//se.TicketingIataOffice,
		//se.IsTicketerRobot,
		//se.Fare,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Vat,
		//se.ServiceFee,
		//se.ServiceFeePenalty,
		//se.Handling,
		//se.Commission,
		//se.CommissionDiscount,
		//se.Discount,
		//se.BonusDiscount,
		//se.BonusAccumulation,
		//se.RefundServiceFee,
		//se.ServiceTotal,
		//se.GrandTotal,
		//se.CancelCommissionPercent,
		//se.CancelCommission,
		//se.CommissionPercent,
		//se.TotalToTransfer,
		//se.Profit,
		//se.ExtraCharge,
		//se.PaymentType,
		//se.TaxRateOfProduct,
		//se.TaxRateOfServiceFee,
		//se.Note,
		//se.Originator,
		//se.Origin,
		//se.PassengerName,
		//se.GdsPassengerName,
		//se.Passenger,
		//se.LastChangedPropertyName,
		//se.Producer,
		//se.Provider,
		//se.ReissueFor,
		//se.RefundedProduct,
		//se.Customer,
		//se.Order,
		//se.Intermediary,
		//se.Country,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.OriginalDocument,
	
	}

	/** Возврат ж/д билета */
	export class PasteboardRefundSemantic extends RailwayDocumentSemantic implements IPasteboardRefundSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "PasteboardRefund";
			this._names = "PasteboardRefunds";
			this.icon("subway");
			this._isEntity = true;
			this._localizeTitle({ ru: "Возврат ж/д билета", rus: "Возвраты ж/д билетов", ua: "Повернення залізничного квитка" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.RailwayDocument;

			this._className = "Product";
			this._getRootEntity = () => sd.Product;
			this._store = db.PasteboardRefunds;
			this._saveStore = db.PasteboardRefunds;
			this._lookupStore = db.PasteboardRefundLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.big();
		}

		_PasteboardRefund = new SemanticMember()
			.localizeTitle({ ru: "Возврат ж/д билета", rus: "Возвраты ж/д билетов", ua: "Повернення залізничного квитка" })
			.lookup(() => sd.PasteboardRefund);
				
	

	}


	/** Персона */
	export interface IPersonSemantic extends IPartySemantic
	{
		//se.Type,
		//se.MilesCardsString,
		//se.Birthday,
		//se.Title,
		//se.BonusCardNumber,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.Organization,
		//se.ReportsTo,
		//se.DefaultBankAccount,

		/** Номера мильных карт */
		MilesCardsString: SemanticMember;

		/** Дата рождения */
		Birthday: SemanticMember;

		/** Должность */
		Title: SemanticMember;

		/** № бонусной карты */
		BonusCardNumber: SemanticMember;

		/** Организация */
		Organization: SemanticMember;
		
		MilesCards: SemanticMember;
		
		Passports: SemanticMember;
		
		DocumentAccesses: SemanticMember;
		
		GdsAgents: SemanticMember;
	
	}

	/** Персона */
	export class PersonSemantic extends PartySemantic implements IPersonSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Person";
			this._names = "Persons";
			this.icon("user");
			this._isEntity = true;
			this._localizeTitle({ ru: "Персона", rus: "Персоны" });

			this._getDerivedEntities = () => [
				sd.Agent, sd.Employee
			];

			this._getBaseEntity = () => sd.Party;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.Persons;
			this._saveStore = db.Persons;
			this._lookupStore = db.PersonLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Name
				.localizeTitle({ ru: "Ф.И.О." })
				.length(20, 0, 0);
		}

		_Person = new SemanticMember()
			.localizeTitle({ ru: "Персона", rus: "Персоны" })
			.lookup(() => sd.Person);
				

		/** Номера мильных карт */
		MilesCardsString = this.member()
			.localizeTitle({ ru: "Номера мильных карт" })
			.string();

		/** Дата рождения */
		Birthday = this.member()
			.localizeTitle({ ru: "Дата рождения" })
			.date();

		/** Должность */
		Title = this.member()
			.localizeTitle({ ru: "Должность" })
			.string();

		/** № бонусной карты */
		BonusCardNumber = this.member()
			.localizeTitle({ ru: "№ бонусной карты" })
			.string();

		/** Организация */
		Organization = this.member()
			.localizeTitle({ ru: "Организация", rus: "Организации" })
			.lookup(() => sd.Organization);

	
		MilesCards = this.collection(() => sd.MilesCard, se => se.Owner);

	
		Passports = this.collection(() => sd.Passport, se => se.Owner);

	
		DocumentAccesses = this.collection(() => sd.DocumentAccess, se => se.Person);

	
		GdsAgents = this.collection(() => sd.GdsAgent, se => se.Person);
	

	}


	
	export interface IInternalIdentitySemantic extends IIdentitySemantic
	{
		//se.Description,
		//se.Name,
		//se.LastChangedPropertyName,
	
	}

	
	export class InternalIdentitySemantic extends IdentitySemantic implements IInternalIdentitySemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "InternalIdentity";
			this._names = "InternalIdentities";
			
			this._isEntity = true;

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Identity;

			this._className = "Identity";
			this._getRootEntity = () => sd.Identity;
			this._store = db.InternalIdentities;
			this._saveStore = db.InternalIdentities;
			this._lookupStore = db.InternalIdentityLookup;
			this._lookupFields = { id: "Id", name: "Name" };
		}

		_InternalIdentity = new SemanticMember()
			.lookup(() => sd.InternalIdentity);
				
	

	}


	/** Пользователь */
	export interface IUserSemantic extends IIdentitySemantic
	{
		//se.Password,
		//se.NewPassword,
		//se.ConfirmPassword,
		//se.Active,
		//se.IsAdministrator,
		//se.IsSupervisor,
		//se.IsAgent,
		//se.IsCashier,
		//se.IsAnalyst,
		//se.IsSubAgent,
		//se.Roles,
		//se.Description,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.Person,

		/** Пароль */
		Password: SemanticMember;

		/** Новый пароль */
		NewPassword: SemanticMember;

		/** Подтверждение пароля */
		ConfirmPassword: SemanticMember;

		/** Активный */
		Active: SemanticMember;

		/** Администратор */
		IsAdministrator: SemanticMember;

		/** Супервизор */
		IsSupervisor: SemanticMember;

		/** Агент */
		IsAgent: SemanticMember;

		/** Кассир */
		IsCashier: SemanticMember;

		/** Аналитик */
		IsAnalyst: SemanticMember;

		/** Субагент */
		IsSubAgent: SemanticMember;

		/** Роли */
		Roles: SemanticMember;

		/** Персона */
		Person: SemanticMember;
	
	}

	/** Пользователь */
	export class UserSemantic extends IdentitySemantic implements IUserSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "User";
			this._names = "Users";
			
			this._isEntity = true;
			this._localizeTitle({ ru: "Пользователь", rus: "Пользователи" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Identity;

			this._className = "Identity";
			this._getRootEntity = () => sd.Identity;
			this._store = db.Users;
			this._saveStore = db.Users;
			this._lookupStore = db.UserLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Name
				.localizeTitle({ ru: "Логин" })
				.length(16, 0, 0);
		}

		_User = new SemanticMember()
			.localizeTitle({ ru: "Пользователь", rus: "Пользователи" })
			.lookup(() => sd.User);
				

		/** Пароль */
		Password = this.member()
			.localizeTitle({ ru: "Пароль" })
			.string();

		/** Новый пароль */
		NewPassword = this.member()
			.localizeTitle({ ru: "Новый пароль" })
			.string()
			.calculated();

		/** Подтверждение пароля */
		ConfirmPassword = this.member()
			.localizeTitle({ ru: "Подтверждение пароля" })
			.string()
			.calculated();

		/** Активный */
		Active = this.member()
			.localizeTitle({ ru: "Активный" })
			.bool()
			.required()
			.defaultValue(true);

		/** Администратор */
		IsAdministrator = this.member()
			.localizeTitle({ ru: "Администратор" })
			.bool()
			.required()
			.secondary();

		/** Супервизор */
		IsSupervisor = this.member()
			.localizeTitle({ ru: "Супервизор" })
			.bool()
			.required()
			.secondary();

		/** Агент */
		IsAgent = this.member()
			.localizeTitle({ ru: "Агент" })
			.bool()
			.required()
			.secondary();

		/** Кассир */
		IsCashier = this.member()
			.localizeTitle({ ru: "Кассир" })
			.bool()
			.required()
			.secondary();

		/** Аналитик */
		IsAnalyst = this.member()
			.localizeTitle({ ru: "Аналитик" })
			.bool()
			.required()
			.secondary();

		/** Субагент */
		IsSubAgent = this.member()
			.localizeTitle({ ru: "Субагент" })
			.bool()
			.required()
			.secondary();

		/** Роли */
		Roles = this.member()
			.localizeTitle({ ru: "Роли" })
			.enum(UserRole)
			.calculated()
			.required()
			.length(30, 0, 0);

		/** Персона */
		Person = this.member()
			.localizeTitle({ ru: "Персона", rus: "Персоны" })
			.lookup(() => sd.Person);
	

	}


	/** Провайдер проживания */
	export interface IAccommodationProviderSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Провайдер проживания */
	export class AccommodationProviderSemantic extends OrganizationSemantic implements IAccommodationProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "AccommodationProvider";
			this._names = "AccommodationProviders";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Провайдер проживания", rus: "Провайдеры проживания" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.AccommodationProviders;
			this._saveStore = db.Organizations;
			this._lookupStore = db.AccommodationProviderLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_AccommodationProvider = new SemanticMember()
			.localizeTitle({ ru: "Провайдер проживания", rus: "Провайдеры проживания" })
			.lookup(() => sd.AccommodationProvider);
				
	

	}


	/** Владелец документов (активный) */
	export interface IActiveOwnerSemantic extends IPartySemantic
	{
		//se.Type,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		Files: SemanticMember;
		
		DocumentOwners: SemanticMember;
	
	}

	/** Владелец документов (активный) */
	export class ActiveOwnerSemantic extends PartySemantic implements IActiveOwnerSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "ActiveOwner";
			this._names = "ActiveOwners";
			
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Владелец документов (активный)", rus: "Владельцы документов (активные)" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Party;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.ActiveOwners;
			this._saveStore = db.Parties;
			this._lookupStore = db.ActiveOwnerLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.small();

			this.Name
				.length(20, 0, 0);
		}

		_ActiveOwner = new SemanticMember()
			.localizeTitle({ ru: "Владелец документов (активный)", rus: "Владельцы документов (активные)" })
			.lookup(() => sd.ActiveOwner);
				
	

	}


	/** Агент */
	export interface IAgentSemantic extends IPersonSemantic
	{
		//se.Type,
		//se.MilesCardsString,
		//se.Birthday,
		//se.Title,
		//se.BonusCardNumber,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.Organization,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Passports: SemanticMember;
		
		DocumentAccesses: SemanticMember;
		
		GdsAgents: SemanticMember;
	
	}

	/** Агент */
	export class AgentSemantic extends PersonSemantic implements IAgentSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Agent";
			this._names = "Agents";
			this.icon("user-secret");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Агент", rus: "Агенты" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Person;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.Agents;
			this._saveStore = db.Persons;
			this._lookupStore = db.AgentLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.small();

			this.Name
				.localizeTitle({ ru: "Ф.И.О." })
				.length(20, 0, 0);
		}

		_Agent = new SemanticMember()
			.localizeTitle({ ru: "Агент", rus: "Агенты" })
			.lookup(() => sd.Agent);
				
	

	}


	/** Авиакомпания */
	export interface IAirlineSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Авиакомпания */
	export class AirlineSemantic extends OrganizationSemantic implements IAirlineSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Airline";
			this._names = "Airlines";
			this.icon("plane");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.Airlines;
			this._saveStore = db.Organizations;
			this._lookupStore = db.AirlineLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_Airline = new SemanticMember()
			.localizeTitle({ ru: "Авиакомпания", rus: "Авиакомпании" })
			.lookup(() => sd.Airline);
				
	

	}


	/** Провайдер автобусных билетов */
	export interface IBusTicketProviderSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Провайдер автобусных билетов */
	export class BusTicketProviderSemantic extends OrganizationSemantic implements IBusTicketProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "BusTicketProvider";
			this._names = "BusTicketProviders";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Провайдер автобусных билетов", rus: "Провайдеры автобусных билетов" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.BusTicketProviders;
			this._saveStore = db.Organizations;
			this._lookupStore = db.BusTicketProviderLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_BusTicketProvider = new SemanticMember()
			.localizeTitle({ ru: "Провайдер автобусных билетов", rus: "Провайдеры автобусных билетов" })
			.lookup(() => sd.BusTicketProvider);
				
	

	}


	/** Провайдер аренды авто */
	export interface ICarRentalProviderSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Провайдер аренды авто */
	export class CarRentalProviderSemantic extends OrganizationSemantic implements ICarRentalProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "CarRentalProvider";
			this._names = "CarRentalProviders";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Провайдер аренды авто", rus: "Провайдеры аренды авто" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.CarRentalProviders;
			this._saveStore = db.Organizations;
			this._lookupStore = db.CarRentalProviderLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_CarRentalProvider = new SemanticMember()
			.localizeTitle({ ru: "Провайдер аренды авто", rus: "Провайдеры аренды авто" })
			.lookup(() => sd.CarRentalProvider);
				
	

	}


	/** Заказчик */
	export interface ICustomerSemantic extends IPartySemantic
	{
		//se.Type,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		Files: SemanticMember;
		
		DocumentOwners: SemanticMember;
	
	}

	/** Заказчик */
	export class CustomerSemantic extends PartySemantic implements ICustomerSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = true;
			this._name = "Customer";
			this._names = "Customers";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Заказчик", rus: "Заказчики" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Party;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.Customers;
			this._saveStore = db.Parties;
			this._lookupStore = db.CustomerLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Name
				.length(20, 0, 0);
		}

		_Customer = new SemanticMember()
			.localizeTitle({ ru: "Заказчик", rus: "Заказчики" })
			.lookup(() => sd.Customer);
				
	

	}


	/** Сотрудник */
	export interface IEmployeeSemantic extends IPersonSemantic
	{
		//se.Type,
		//se.MilesCardsString,
		//se.Birthday,
		//se.Title,
		//se.BonusCardNumber,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.Organization,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Passports: SemanticMember;
		
		DocumentAccesses: SemanticMember;
		
		GdsAgents: SemanticMember;
	
	}

	/** Сотрудник */
	export class EmployeeSemantic extends PersonSemantic implements IEmployeeSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Employee";
			this._names = "Employees";
			this.icon("user");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Сотрудник", rus: "Сотрудники" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Person;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.Employees;
			this._saveStore = db.Persons;
			this._lookupStore = db.EmployeeLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Name
				.localizeTitle({ ru: "Ф.И.О." })
				.length(20, 0, 0);
		}

		_Employee = new SemanticMember()
			.localizeTitle({ ru: "Сотрудник", rus: "Сотрудники" })
			.lookup(() => sd.Employee);
				
	

	}


	/** Провайдер дополнительных услуг */
	export interface IGenericProductProviderSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Провайдер дополнительных услуг */
	export class GenericProductProviderSemantic extends OrganizationSemantic implements IGenericProductProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "GenericProductProvider";
			this._names = "GenericProductProviders";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Провайдер дополнительных услуг", rus: "Провайдеры дополнительных услуг" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.GenericProductProviders;
			this._saveStore = db.Organizations;
			this._lookupStore = db.GenericProductProviderLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_GenericProductProvider = new SemanticMember()
			.localizeTitle({ ru: "Провайдер дополнительных услуг", rus: "Провайдеры дополнительных услуг" })
			.lookup(() => sd.GenericProductProvider);
				
	

	}


	/** Страховая компания */
	export interface IInsuranceCompanySemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Страховая компания */
	export class InsuranceCompanySemantic extends OrganizationSemantic implements IInsuranceCompanySemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "InsuranceCompany";
			this._names = "InsuranceCompanies";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Страховая компания", rus: "Страховые компании" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.InsuranceCompanies;
			this._saveStore = db.Organizations;
			this._lookupStore = db.InsuranceCompanyLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_InsuranceCompany = new SemanticMember()
			.localizeTitle({ ru: "Страховая компания", rus: "Страховые компании" })
			.lookup(() => sd.InsuranceCompany);
				
	

	}


	/** Провайдер ж/д билетов */
	export interface IPasteboardProviderSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Провайдер ж/д билетов */
	export class PasteboardProviderSemantic extends OrganizationSemantic implements IPasteboardProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "PasteboardProvider";
			this._names = "PasteboardProviders";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Провайдер ж/д билетов", rus: "Провайдеры ж/д билетов" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.PasteboardProviders;
			this._saveStore = db.Organizations;
			this._lookupStore = db.PasteboardProviderLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_PasteboardProvider = new SemanticMember()
			.localizeTitle({ ru: "Провайдер ж/д билетов", rus: "Провайдеры ж/д билетов" })
			.lookup(() => sd.PasteboardProvider);
				
	

	}


	/** Квитанция */
	export interface IReceiptSemantic extends IInvoiceSemantic
	{
		//se.Type,
		//se.IssueDate,
		//se.Number,
		//se.Agreement,
		//se.TimeStamp,
		//se.Content,
		//se.Total,
		//se.Vat,
		//se.LastChangedPropertyName,
		//se.Order,
		//se.IssuedBy,
		
		Payments: SemanticMember;
	
	}

	/** Квитанция */
	export class ReceiptSemantic extends InvoiceSemantic implements IReceiptSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "Receipt";
			this._names = "Receipts";
			
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Квитанция", rus: "Квитанции" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Invoice;

			this._className = "Invoice";
			this._getRootEntity = () => sd.Invoice;
			this._store = db.Receipts;
			this._saveStore = db.Invoices;
			this._lookupStore = db.ReceiptLookup;
			this._lookupFields = { id: "Id", name: "Number" };
		}

		_Receipt = new SemanticMember()
			.localizeTitle({ ru: "Квитанция", rus: "Квитанции" })
			.lookup(() => sd.Receipt);
				
	

	}


	/** Мобильный оператор */
	export interface IRoamingOperatorSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Мобильный оператор */
	export class RoamingOperatorSemantic extends OrganizationSemantic implements IRoamingOperatorSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "RoamingOperator";
			this._names = "RoamingOperators";
			this.icon("mobile");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Мобильный оператор", rus: "Мобильные операторы" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.RoamingOperators;
			this._saveStore = db.Organizations;
			this._lookupStore = db.RoamingOperatorLookup;
			this._lookupFields = { id: "Id", name: "Name" };
			this.small();

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_RoamingOperator = new SemanticMember()
			.localizeTitle({ ru: "Мобильный оператор", rus: "Мобильные операторы" })
			.lookup(() => sd.RoamingOperator);
				
	

	}


	/** Провайдер туров (готовых) */
	export interface ITourProviderSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Провайдер туров (готовых) */
	export class TourProviderSemantic extends OrganizationSemantic implements ITourProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "TourProvider";
			this._names = "TourProviders";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Провайдер туров (готовых)", rus: "Провайдеры туров (готовых)" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.TourProviders;
			this._saveStore = db.Organizations;
			this._lookupStore = db.TourProviderLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_TourProvider = new SemanticMember()
			.localizeTitle({ ru: "Провайдер туров (готовых)", rus: "Провайдеры туров (готовых)" })
			.lookup(() => sd.TourProvider);
				
	

	}


	/** Провайдер трансферов */
	export interface ITransferProviderSemantic extends IOrganizationSemantic
	{
		//se.Type,
		//se.IsAirline,
		//se.AirlineIataCode,
		//se.AirlinePrefixCode,
		//se.AirlinePassportRequirement,
		//se.IsAccommodationProvider,
		//se.IsBusTicketProvider,
		//se.IsCarRentalProvider,
		//se.IsPasteboardProvider,
		//se.IsTourProvider,
		//se.IsTransferProvider,
		//se.IsGenericProductProvider,
		//se.IsProvider,
		//se.IsInsuranceCompany,
		//se.IsRoamingOperator,
		//se.DepartmentCount,
		//se.EmployeeCount,
		//se.LegalName,
		//se.Code,
		//se.NameForDocuments,
		//se.Phone1,
		//se.Phone2,
		//se.Fax,
		//se.Email1,
		//se.Email2,
		//se.WebAddress,
		//se.IsCustomer,
		//se.IsSupplier,
		//se.Details,
		//se.LegalAddress,
		//se.ActualAddress,
		//se.Note,
		//se.FileCount,
		//se.Name,
		//se.LastChangedPropertyName,
		//se.ReportsTo,
		//se.DefaultBankAccount,
		
		MilesCards: SemanticMember;
		
		Departments: SemanticMember;
		/** Сотрудники */
		
		Employees: SemanticMember;
		
		AirlineServiceClasses: SemanticMember;
	
	}

	/** Провайдер трансферов */
	export class TransferProviderSemantic extends OrganizationSemantic implements ITransferProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "TransferProvider";
			this._names = "TransferProviders";
			this.icon("");
			this._isEntityQuery = true;
			this._localizeTitle({ ru: "Провайдер трансферов", rus: "Провайдеры трансферов" });

			this._getDerivedEntities = null;

			this._getBaseEntity = () => sd.Organization;

			this._className = "Party";
			this._getRootEntity = () => sd.Party;
			this._store = db.TransferProviders;
			this._saveStore = db.Organizations;
			this._lookupStore = db.TransferProviderLookup;
			this._lookupFields = { id: "Id", name: "Name" };

			this.Code
				.localizeTitle({ ru: "Код предприятия (ЕДРПОУ)" });
		}

		_TransferProvider = new SemanticMember()
			.localizeTitle({ ru: "Провайдер трансферов", rus: "Провайдеры трансферов" })
			.lookup(() => sd.TransferProvider);
				
	

	}


	
	export interface IProductTotalSemantic extends ISemanticEntity
	{
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		/** Примечание */
		Note: SemanticMember;
	
	}

	
	export class ProductTotalSemantic extends SemanticEntity implements IProductTotalSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotal";
			this._names = "ProductTotal";
			

			this._getDerivedEntities = () => [
				sd.ProductTotalByBooker, sd.ProductTotalByType, sd.ProductTotalByOwner, sd.ProductTotalByYear, sd.ProductTotalByProvider, sd.ProductTotalBySeller, sd.ProductTotalByQuarter, sd.ProductTotalByMonth, sd.ProductTotalByDay
			];

			this.Total
				.localizeTitle({ ru: "К перечислению провайдеру" });

			this.ServiceFee
				.localizeTitle({ ru: "Сервисный сбор" })
				.subject();

			this.GrandTotal
				.localizeTitle({ ru: "К оплате" });
		}

		Total = this.member()
			.float(2);

		ServiceFee = this.member()
			.float(2);

		GrandTotal = this.member()
			.float(2);

		/** Примечание */
		Note = this.member()
			.localizeTitle({ ru: "Примечание" })
			.string();
	

	}


	
	export interface IProfitDistributionTotalSemantic extends ISemanticEntity
	{
		//se.Rank,
		//se.SellCount,
		//se.RefundCount,
		//se.VoidCount,
		//se.Currency,
		//se.SellGrandTotal,
		//se.RefundGrandTotal,
		//se.GrandTotal,
		//se.Total,
		//se.ServiceFee,
		//se.Commission,
		//se.AgentTotal,
		//se.Vat,

		/** № */
		Rank: SemanticMember;

		/** Кол-во продаж */
		SellCount: SemanticMember;

		/** Кол-во возвратов */
		RefundCount: SemanticMember;

		/** Кол-во ануляций */
		VoidCount: SemanticMember;

		/** Валюта */
		Currency: SemanticMember;

		/** Продано */
		SellGrandTotal: SemanticMember;

		/** Возврат */
		RefundGrandTotal: SemanticMember;

		GrandTotal: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		Commission: SemanticMember;

		/** Итого по агенту */
		AgentTotal: SemanticMember;

		Vat: SemanticMember;
	
	}

	
	export class ProfitDistributionTotalSemantic extends SemanticEntity implements IProfitDistributionTotalSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProfitDistributionTotal";
			this._names = "ProfitDistributionTotal";
			

			this._getDerivedEntities = () => [
				sd.ProfitDistributionByCustomer, sd.ProfitDistributionByProvider
			];

			this.GrandTotal
				.localizeTitle({ ru: "К оплате" });

			this.Total
				.localizeTitle({ ru: "К перечислению провайдеру" });

			this.ServiceFee
				.localizeTitle({ ru: "Сервисный сбор" })
				.subject();

			this.Commission
				.localizeTitle({ ru: "Комиссия" });
		}

		/** № */
		Rank = this.member()
			.localizeTitle({ ru: "№" })
			.int()
			.required()
			.entityPosition();

		/** Кол-во продаж */
		SellCount = this.member()
			.localizeTitle({ ru: "Кол-во продаж" })
			.int()
			.required();

		/** Кол-во возвратов */
		RefundCount = this.member()
			.localizeTitle({ ru: "Кол-во возвратов" })
			.int()
			.required();

		/** Кол-во ануляций */
		VoidCount = this.member()
			.localizeTitle({ ru: "Кол-во ануляций" })
			.int()
			.required();

		/** Валюта */
		Currency = this.member()
			.localizeTitle({ ru: "Валюта" })
			.string();

		/** Продано */
		SellGrandTotal = this.member()
			.localizeTitle({ ru: "Продано" })
			.float(2);

		/** Возврат */
		RefundGrandTotal = this.member()
			.localizeTitle({ ru: "Возврат" })
			.float(2);

		GrandTotal = this.member()
			.float(2);

		Total = this.member()
			.float(2);

		ServiceFee = this.member()
			.float(2);

		Commission = this.member()
			.float(2);

		/** Итого по агенту */
		AgentTotal = this.member()
			.localizeTitle({ ru: "Итого по агенту" })
			.float(2);

		Vat = this.member()
			.float(2);
	

	}


	/** Ежедневный отчет по выручке */
	export interface IEverydayProfitReportSemantic extends ISemanticEntity
	{
		//se.Provider,
		//se.ProductType,
		//se.Product,
		//se.IssueDate,
		//se.Seller,
		//se.PassengerName,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.Country,
		//se.Fare,
		//se.Currency,
		//se.CurrencyRate,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Commission,
		//se.ServiceFee,
		//se.Vat,
		//se.GrandTotal,
		//se.Order,
		//se.Payer,
		//se.Invoice,
		//se.InvoiceDate,
		//se.CompletionCertificate,
		//se.CompletionCertificateDate,
		//se.Payment,
		//se.PaymentDate,

		Provider: SemanticMember;

		/** Вид услуги */
		ProductType: SemanticMember;

		/** Услуга */
		Product: SemanticMember;

		IssueDate: SemanticMember;

		Seller: SemanticMember;

		/** Пассажиры / Покупатели */
		PassengerName: SemanticMember;

		Itinerary: SemanticMember;

		StartDate: SemanticMember;

		FinishDate: SemanticMember;

		/** Страна */
		Country: SemanticMember;

		Fare: SemanticMember;

		/** Валюта */
		Currency: SemanticMember;

		/** Курс валюты */
		CurrencyRate: SemanticMember;

		EqualFare: SemanticMember;

		FeesTotal: SemanticMember;

		CancelFee: SemanticMember;

		Total: SemanticMember;

		Commission: SemanticMember;

		ServiceFee: SemanticMember;

		Vat: SemanticMember;

		GrandTotal: SemanticMember;

		/** Заказ */
		Order: SemanticMember;

		Payer: SemanticMember;

		/** Счёт */
		Invoice: SemanticMember;

		/** Дата счёта */
		InvoiceDate: SemanticMember;

		/** Акт */
		CompletionCertificate: SemanticMember;

		/** Дата акта */
		CompletionCertificateDate: SemanticMember;

		/** Оплата */
		Payment: SemanticMember;

		/** Дата оплаты */
		PaymentDate: SemanticMember;
	
	}

	/** Ежедневный отчет по выручке */
	export class EverydayProfitReportSemantic extends SemanticEntity implements IEverydayProfitReportSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "EverydayProfitReport";
			this._names = "EverydayProfitReports";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Ежедневный отчет по выручке" });

			this._getDerivedEntities = null;

			this._className = "EverydayProfitReport";
			this._getRootEntity = () => sd.EverydayProfitReport;
			this._store = db.EverydayProfitReports;
			this._saveStore = db.EverydayProfitReports;
			this._lookupFields = { id: "", name: "" };

			this.Provider
				.lookup(() => sd.Organization);

			this.ProductType
				.localizeTitle({ ru: "Вид услуги" })
				.required()
				.length(12, 0, 0)
				.entityType();

			this.IssueDate
				.required()
				.entityDate();

			this.Seller
				.lookup(() => sd.Person);

			this.Itinerary
				.localizeTitle({ ru: "Маршрут" })
				.length(16, 0, 0);

			this.Fare
				.localizeTitle({ ru: "Тариф" });

			this.EqualFare
				.localizeTitle({ ru: "Экв. тариф" })
				.subject();

			this.FeesTotal
				.localizeTitle({ ru: "Таксы" })
				.subject();

			this.CancelFee
				.localizeTitle({ ru: "Штраф за отмену" })
				.subject();

			this.Total
				.localizeTitle({ ru: "К перечислению провайдеру" });

			this.Commission
				.localizeTitle({ ru: "Комиссия" });

			this.ServiceFee
				.localizeTitle({ ru: "Сервисный сбор" })
				.subject();

			this.GrandTotal
				.localizeTitle({ ru: "К оплате" });

			this.Order
				.localizeTitle({ ru: "Заказ", rus: "Заказы" })
				.lookup(() => sd.Order);

			this.Payer
				.lookup(() => sd.Party);
		}

		_EverydayProfitReport = new SemanticMember()
			.localizeTitle({ ru: "Ежедневный отчет по выручке" })
			.lookup(() => sd.EverydayProfitReport);
				

		Provider = this.member()
			.lookup(() => sd.Organization);

		/** Вид услуги */
		ProductType = this.member()
			.localizeTitle({ ru: "Вид услуги" })
			.enum(ProductType)
			.required()
			.length(12, 0, 0);

		/** Услуга */
		Product = this.member()
			.localizeTitle({ ru: "Услуга", rus: "Все услуги" })
			.lookup(() => sd.Product)
			.entityName();

		IssueDate = this.member()
			.date()
			.required();

		Seller = this.member()
			.lookup(() => sd.Person);

		/** Пассажиры / Покупатели */
		PassengerName = this.member()
			.localizeTitle({ ru: "Пассажиры / Покупатели" })
			.string()
			.length(16, 0, 0);

		Itinerary = this.member()
			.string();

		StartDate = this.member()
			.date();

		FinishDate = this.member()
			.date();

		/** Страна */
		Country = this.member()
			.localizeTitle({ ru: "Страна", rus: "Страны" })
			.lookup(() => sd.Country);

		Fare = this.member()
			.money();

		/** Валюта */
		Currency = this.member()
			.localizeTitle({ ru: "Валюта" })
			.string();

		/** Курс валюты */
		CurrencyRate = this.member()
			.localizeTitle({ ru: "Курс валюты" })
			.float();

		EqualFare = this.member()
			.float(2);

		FeesTotal = this.member()
			.float(2);

		CancelFee = this.member()
			.float(2);

		Total = this.member()
			.float(2);

		Commission = this.member()
			.float(2);

		ServiceFee = this.member()
			.float(2);

		Vat = this.member()
			.float(2);

		GrandTotal = this.member()
			.float(2);

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);

		Payer = this.member()
			.lookup(() => sd.Party);

		/** Счёт */
		Invoice = this.member()
			.localizeTitle({ ru: "Счёт" })
			.lookup(() => sd.Invoice);

		/** Дата счёта */
		InvoiceDate = this.member()
			.localizeTitle({ ru: "Дата счёта" })
			.date();

		/** Акт */
		CompletionCertificate = this.member()
			.localizeTitle({ ru: "Акт" })
			.lookup(() => sd.Invoice);

		/** Дата акта */
		CompletionCertificateDate = this.member()
			.localizeTitle({ ru: "Дата акта" })
			.date();

		/** Оплата */
		Payment = this.member()
			.localizeTitle({ ru: "Оплата" })
			.lookup(() => sd.Payment);

		/** Дата оплаты */
		PaymentDate = this.member()
			.localizeTitle({ ru: "Дата оплаты" })
			.date();
	

	}


	/** Flown-отчет */
	export interface IFlownReportSemantic extends ISemanticEntity
	{
		//se.Date,
		//se.Op,
		//se.AC,
		//se.TicketNumber,
		//se.Client,
		//se.Passenger,
		//se.Route,
		//se.Curr,
		//se.Fare,
		//se.Tax,
		//se.Flown1,
		//se.Flown2,
		//se.Flown3,
		//se.Flown4,
		//se.Flown5,
		//se.Flown6,
		//se.Flown7,
		//se.Flown8,
		//se.Flown9,
		//se.Flown10,
		//se.Flown11,
		//se.Flown12,
		//se.TourCode,
		//se.CheapTicket,

		Date: SemanticMember;

		Op: SemanticMember;

		AC: SemanticMember;

		TicketNumber: SemanticMember;

		Client: SemanticMember;

		Passenger: SemanticMember;

		Route: SemanticMember;

		Curr: SemanticMember;

		Fare: SemanticMember;

		Tax: SemanticMember;

		Flown1: SemanticMember;

		Flown2: SemanticMember;

		Flown3: SemanticMember;

		Flown4: SemanticMember;

		Flown5: SemanticMember;

		Flown6: SemanticMember;

		Flown7: SemanticMember;

		Flown8: SemanticMember;

		Flown9: SemanticMember;

		Flown10: SemanticMember;

		Flown11: SemanticMember;

		Flown12: SemanticMember;

		TourCode: SemanticMember;

		/** Добор с билета */
		CheapTicket: SemanticMember;
	
	}

	/** Flown-отчет */
	export class FlownReportSemantic extends SemanticEntity implements IFlownReportSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "FlownReport";
			this._names = "FlownReports";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Flown-отчет" });

			this._getDerivedEntities = null;

			this._className = "FlownReport";
			this._getRootEntity = () => sd.FlownReport;
			this._store = db.FlownReports;
			this._saveStore = db.FlownReports;
			this._lookupFields = { id: "", name: "" };

			this.TourCode
				.localizeTitle({ ru: "Туркод" });
		}

		_FlownReport = new SemanticMember()
			.localizeTitle({ ru: "Flown-отчет" })
			.lookup(() => sd.FlownReport);
				

		Date = this.member()
			.date()
			.required()
			.entityDate();

		Op = this.member()
			.string()
			.length(2, 0, 0);

		AC = this.member()
			.string()
			.length(2, 0, 0);

		TicketNumber = this.member()
			.lookup(() => sd.AviaDocument);

		Client = this.member()
			.lookup(() => sd.Party)
			.length(20, 0, 0);

		Passenger = this.member()
			.string()
			.length(20, 0, 0);

		Route = this.member()
			.string()
			.length(16, 0, 0);

		Curr = this.member()
			.string()
			.length(3, 0, 0);

		Fare = this.member()
			.float(2);

		Tax = this.member()
			.float(2);

		Flown1 = this.member()
			.float(2);

		Flown2 = this.member()
			.float(2);

		Flown3 = this.member()
			.float(2);

		Flown4 = this.member()
			.float(2);

		Flown5 = this.member()
			.float(2);

		Flown6 = this.member()
			.float(2);

		Flown7 = this.member()
			.float(2);

		Flown8 = this.member()
			.float(2);

		Flown9 = this.member()
			.float(2);

		Flown10 = this.member()
			.float(2);

		Flown11 = this.member()
			.float(2);

		Flown12 = this.member()
			.float(2);

		TourCode = this.member()
			.string()
			.length(10, 0, 0);

		/** Добор с билета */
		CheapTicket = this.member()
			.localizeTitle({ ru: "Добор с билета" })
			.lookup(() => sd.AviaDocument)
			.length(14, 0, 0);
	

	}


	/** Баланс */
	export interface IOrderBalanceSemantic extends ISemanticEntity
	{
		//se.Order,
		//se.IssueDate,
		//se.Customer,
		//se.Currency,
		//se.Delivered,
		//se.Paid,
		//se.Balance,

		/** Заказ */
		Order: SemanticMember;

		/** Дата заказа */
		IssueDate: SemanticMember;

		Customer: SemanticMember;

		/** Валюта */
		Currency: SemanticMember;

		/** Оказано услуг на сумму */
		Delivered: SemanticMember;

		/** Оплачено */
		Paid: SemanticMember;

		/** Баланс */
		Balance: SemanticMember;
	
	}

	/** Баланс */
	export class OrderBalanceSemantic extends SemanticEntity implements IOrderBalanceSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "OrderBalance";
			this._names = "OrderBalances";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Баланс" });

			this._getDerivedEntities = null;

			this._className = "OrderBalance";
			this._getRootEntity = () => sd.OrderBalance;
			this._store = db.OrderBalances;
			this._saveStore = db.OrderBalances;
			this._lookupFields = { id: "", name: "" };
		}

		_OrderBalance = new SemanticMember()
			.localizeTitle({ ru: "Баланс" })
			.lookup(() => sd.OrderBalance);
				

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);

		/** Дата заказа */
		IssueDate = this.member()
			.localizeTitle({ ru: "Дата заказа" })
			.date();

		Customer = this.member()
			.lookup(() => sd.Party);

		/** Валюта */
		Currency = this.member()
			.localizeTitle({ ru: "Валюта" })
			.string()
			.length(3, 0, 0);

		/** Оказано услуг на сумму */
		Delivered = this.member()
			.localizeTitle({ ru: "Оказано услуг на сумму" })
			.float(2)
			.required();

		/** Оплачено */
		Paid = this.member()
			.localizeTitle({ ru: "Оплачено" })
			.float(2)
			.required();

		/** Баланс */
		Balance = this.member()
			.localizeTitle({ ru: "Баланс" })
			.float(2)
			.required();
	

	}


	/** Сводка по услугам */
	export interface IProductSummarySemantic extends ISemanticEntity
	{
		//se.IssueDate,
		//se.Type,
		//se.Name,
		//se.Itinerary,
		//se.IsRefund,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Order,

		IssueDate: SemanticMember;

		Type: SemanticMember;

		Name: SemanticMember;

		Itinerary: SemanticMember;

		IsRefund: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		/** Заказ */
		Order: SemanticMember;
	
	}

	/** Сводка по услугам */
	export class ProductSummarySemantic extends SemanticEntity implements IProductSummarySemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductSummary";
			this._names = "ProductSummaries";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Сводка по услугам" });

			this._getDerivedEntities = null;

			this._className = "ProductSummary";
			this._getRootEntity = () => sd.ProductSummary;
			this._store = db.ProductSummaries;
			this._saveStore = db.ProductSummaries;
			this._lookupFields = { id: "", name: "" };

			this.IssueDate
				.entityDate();

			this.Type
				.localizeTitle({ ru: "Вид услуги" })
				.length(12, 0, 0)
				.entityType();

			this.Name
				.length(16, 0, 0)
				.entityName();

			this.Itinerary
				.localizeTitle({ ru: "Маршрут" })
				.length(16, 0, 0);

			this.IsRefund
				.localizeTitle({ ru: "Это возврат" })
				.required();

			this.Total
				.localizeTitle({ ru: "К перечислению провайдеру" });

			this.ServiceFee
				.localizeTitle({ ru: "Сервисный сбор" })
				.subject();

			this.GrandTotal
				.localizeTitle({ ru: "К оплате" });

			this.Order
				.localizeTitle({ ru: "Заказ", rus: "Заказы" })
				.lookup(() => sd.Order);
		}

		_ProductSummary = new SemanticMember()
			.localizeTitle({ ru: "Сводка по услугам" })
			.lookup(() => sd.ProductSummary);
				

		IssueDate = this.member()
			.date();

		Type = this.member()
			.localizeTitle({ ru: "Вид услуги" })
			.enum(ProductType)
			.length(12, 0, 0);

		Name = this.member()
			.string();

		Itinerary = this.member()
			.string();

		IsRefund = this.member()
			.bool()
			.required();

		Total = this.member()
			.money();

		ServiceFee = this.member()
			.money();

		GrandTotal = this.member()
			.money();

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);
	

	}


	/** Услуги итого по бронировщику */
	export interface IProductTotalByBookerSemantic extends IProductTotalSemantic
	{
		//se.Rank,
		//se.BookerName,
		//se.Booker,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** № */
		Rank: SemanticMember;

		BookerName: SemanticMember;

		Booker: SemanticMember;
	
	}

	/** Услуги итого по бронировщику */
	export class ProductTotalByBookerSemantic extends ProductTotalSemantic implements IProductTotalByBookerSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByBooker";
			this._names = "ProductTotalByBookers";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого по бронировщику", ruShort: "По бронировщику" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalByBooker";
			this._getRootEntity = () => sd.ProductTotalByBooker;
			this._store = db.ProductTotalByBookers;
			this._saveStore = db.ProductTotalByBookers;
			this._lookupFields = { id: "", name: "" };

			this.Booker
				.lookup(() => sd.Person);
		}

		_ProductTotalByBooker = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого по бронировщику", ruShort: "По бронировщику" })
			.lookup(() => sd.ProductTotalByBooker);
				

		/** № */
		Rank = this.member()
			.localizeTitle({ ru: "№" })
			.int()
			.required();

		BookerName = this.member()
			.string();

		Booker = this.member()
			.lookup(() => sd.Person);
	

	}


	/** Услуги итого посуточно */
	export interface IProductTotalByDaySemantic extends IProductTotalSemantic
	{
		//se.IssueDate,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** Дата */
		IssueDate: SemanticMember;
	
	}

	/** Услуги итого посуточно */
	export class ProductTotalByDaySemantic extends ProductTotalSemantic implements IProductTotalByDaySemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByDay";
			this._names = "ProductTotalByDays";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого посуточно", ruShort: "Посуточно" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalByDay";
			this._getRootEntity = () => sd.ProductTotalByDay;
			this._store = db.ProductTotalByDays;
			this._saveStore = db.ProductTotalByDays;
			this._lookupFields = { id: "IssueDate", name: "" };
		}

		_ProductTotalByDay = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого посуточно", ruShort: "Посуточно" })
			.lookup(() => sd.ProductTotalByDay);
				

		/** Дата */
		IssueDate = this.member()
			.localizeTitle({ en: "Date", ru: "Дата" })
			.date()
			.required();
	

	}


	/** Услуги итого помесячно */
	export interface IProductTotalByMonthSemantic extends IProductTotalSemantic
	{
		//se.IssueDate,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** Месяц */
		IssueDate: SemanticMember;
	
	}

	/** Услуги итого помесячно */
	export class ProductTotalByMonthSemantic extends ProductTotalSemantic implements IProductTotalByMonthSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByMonth";
			this._names = "ProductTotalByMonths";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого помесячно", ruShort: "Помесячно" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalByMonth";
			this._getRootEntity = () => sd.ProductTotalByMonth;
			this._store = db.ProductTotalByMonths;
			this._saveStore = db.ProductTotalByMonths;
			this._lookupFields = { id: "", name: "" };
		}

		_ProductTotalByMonth = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого помесячно", ruShort: "Помесячно" })
			.lookup(() => sd.ProductTotalByMonth);
				

		/** Месяц */
		IssueDate = this.member()
			.localizeTitle({ ru: "Месяц" })
			.monthAndYear()
			.required();
	

	}


	/** Услуги итого по владельцу */
	export interface IProductTotalByOwnerSemantic extends IProductTotalSemantic
	{
		//se.Rank,
		//se.OwnerName,
		//se.Owner,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** № */
		Rank: SemanticMember;

		OwnerName: SemanticMember;

		Owner: SemanticMember;
	
	}

	/** Услуги итого по владельцу */
	export class ProductTotalByOwnerSemantic extends ProductTotalSemantic implements IProductTotalByOwnerSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByOwner";
			this._names = "ProductTotalByOwners";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого по владельцу", ruShort: "По владельцу" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalByOwner";
			this._getRootEntity = () => sd.ProductTotalByOwner;
			this._store = db.ProductTotalByOwners;
			this._saveStore = db.ProductTotalByOwners;
			this._lookupFields = { id: "", name: "" };

			this.Owner
				.lookup(() => sd.Party);
		}

		_ProductTotalByOwner = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого по владельцу", ruShort: "По владельцу" })
			.lookup(() => sd.ProductTotalByOwner);
				

		/** № */
		Rank = this.member()
			.localizeTitle({ ru: "№" })
			.int()
			.required();

		OwnerName = this.member()
			.string();

		Owner = this.member()
			.lookup(() => sd.Party);
	

	}


	/** Услуги итого по провайдеру */
	export interface IProductTotalByProviderSemantic extends IProductTotalSemantic
	{
		//se.Rank,
		//se.ProviderName,
		//se.Provider,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** № */
		Rank: SemanticMember;

		ProviderName: SemanticMember;

		Provider: SemanticMember;
	
	}

	/** Услуги итого по провайдеру */
	export class ProductTotalByProviderSemantic extends ProductTotalSemantic implements IProductTotalByProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByProvider";
			this._names = "ProductTotalByProviders";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого по провайдеру", ruShort: "По провайдеру" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalByProvider";
			this._getRootEntity = () => sd.ProductTotalByProvider;
			this._store = db.ProductTotalByProviders;
			this._saveStore = db.ProductTotalByProviders;
			this._lookupFields = { id: "", name: "" };

			this.Provider
				.lookup(() => sd.Organization);
		}

		_ProductTotalByProvider = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого по провайдеру", ruShort: "По провайдеру" })
			.lookup(() => sd.ProductTotalByProvider);
				

		/** № */
		Rank = this.member()
			.localizeTitle({ ru: "№" })
			.int()
			.required();

		ProviderName = this.member()
			.string();

		Provider = this.member()
			.lookup(() => sd.Organization);
	

	}


	/** Услуги итого поквартально */
	export interface IProductTotalByQuarterSemantic extends IProductTotalSemantic
	{
		//se.IssueDate,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** Квартал */
		IssueDate: SemanticMember;
	
	}

	/** Услуги итого поквартально */
	export class ProductTotalByQuarterSemantic extends ProductTotalSemantic implements IProductTotalByQuarterSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByQuarter";
			this._names = "ProductTotalByQuarters";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого поквартально", ruShort: "Поквартально" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalByQuarter";
			this._getRootEntity = () => sd.ProductTotalByQuarter;
			this._store = db.ProductTotalByQuarters;
			this._saveStore = db.ProductTotalByQuarters;
			this._lookupFields = { id: "", name: "" };
		}

		_ProductTotalByQuarter = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого поквартально", ruShort: "Поквартально" })
			.lookup(() => sd.ProductTotalByQuarter);
				

		/** Квартал */
		IssueDate = this.member()
			.localizeTitle({ ru: "Квартал" })
			.quarterAndYear()
			.required();
	

	}


	/** Услуги итого по продавцу */
	export interface IProductTotalBySellerSemantic extends IProductTotalSemantic
	{
		//se.Rank,
		//se.SellerName,
		//se.Seller,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** № */
		Rank: SemanticMember;

		SellerName: SemanticMember;

		Seller: SemanticMember;
	
	}

	/** Услуги итого по продавцу */
	export class ProductTotalBySellerSemantic extends ProductTotalSemantic implements IProductTotalBySellerSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalBySeller";
			this._names = "ProductTotalBySellers";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого по продавцу", ruShort: "По продавцу" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalBySeller";
			this._getRootEntity = () => sd.ProductTotalBySeller;
			this._store = db.ProductTotalBySellers;
			this._saveStore = db.ProductTotalBySellers;
			this._lookupFields = { id: "", name: "" };

			this.Seller
				.lookup(() => sd.Person);
		}

		_ProductTotalBySeller = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого по продавцу", ruShort: "По продавцу" })
			.lookup(() => sd.ProductTotalBySeller);
				

		/** № */
		Rank = this.member()
			.localizeTitle({ ru: "№" })
			.int()
			.required();

		SellerName = this.member()
			.string();

		Seller = this.member()
			.lookup(() => sd.Person);
	

	}


	/** Услуги итого по видам услуг */
	export interface IProductTotalByTypeSemantic extends IProductTotalSemantic
	{
		//se.Rank,
		//se.Type,
		//se.TypeName,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** № */
		Rank: SemanticMember;

		Type: SemanticMember;

		TypeName: SemanticMember;
	
	}

	/** Услуги итого по видам услуг */
	export class ProductTotalByTypeSemantic extends ProductTotalSemantic implements IProductTotalByTypeSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByType";
			this._names = "ProductTotalByTypes";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого по видам услуг", ruShort: "По видам услуг" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalByType";
			this._getRootEntity = () => sd.ProductTotalByType;
			this._store = db.ProductTotalByTypes;
			this._saveStore = db.ProductTotalByTypes;
			this._lookupFields = { id: "", name: "" };

			this.Type
				.localizeTitle({ ru: "Вид услуги" })
				.required()
				.length(20, 0, 0)
				.entityType();
		}

		_ProductTotalByType = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого по видам услуг", ruShort: "По видам услуг" })
			.lookup(() => sd.ProductTotalByType);
				

		/** № */
		Rank = this.member()
			.localizeTitle({ ru: "№" })
			.int()
			.required();

		Type = this.member()
			.localizeTitle({ ru: "Вид услуги" })
			.enum(ProductType)
			.required()
			.length(12, 0, 0);

		TypeName = this.member()
			.string();
	

	}


	/** Услуги итого по годам */
	export interface IProductTotalByYearSemantic extends IProductTotalSemantic
	{
		//se.Year,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,

		/** Год */
		Year: SemanticMember;
	
	}

	/** Услуги итого по годам */
	export class ProductTotalByYearSemantic extends ProductTotalSemantic implements IProductTotalByYearSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByYear";
			this._names = "ProductTotalByYears";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Услуги итого по годам", ruShort: "По годам" });

			this._getDerivedEntities = null;

			this._className = "ProductTotalByYear";
			this._getRootEntity = () => sd.ProductTotalByYear;
			this._store = db.ProductTotalByYears;
			this._saveStore = db.ProductTotalByYears;
			this._lookupFields = { id: "Year", name: "" };
		}

		_ProductTotalByYear = new SemanticMember()
			.localizeTitle({ ru: "Услуги итого по годам", ruShort: "По годам" })
			.lookup(() => sd.ProductTotalByYear);
				

		/** Год */
		Year = this.member()
			.localizeTitle({ ru: "Год" })
			.int()
			.required();
	

	}


	/** Распределение выручки по заказчикам */
	export interface IProfitDistributionByCustomerSemantic extends IProfitDistributionTotalSemantic
	{
		//se.Customer,
		//se.Rank,
		//se.SellCount,
		//se.RefundCount,
		//se.VoidCount,
		//se.Currency,
		//se.SellGrandTotal,
		//se.RefundGrandTotal,
		//se.GrandTotal,
		//se.Total,
		//se.ServiceFee,
		//se.Commission,
		//se.AgentTotal,
		//se.Vat,

		Customer: SemanticMember;
	
	}

	/** Распределение выручки по заказчикам */
	export class ProfitDistributionByCustomerSemantic extends ProfitDistributionTotalSemantic implements IProfitDistributionByCustomerSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProfitDistributionByCustomer";
			this._names = "ProfitDistributionByCustomers";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Распределение выручки по заказчикам" });

			this._getDerivedEntities = null;

			this._className = "ProfitDistributionByCustomer";
			this._getRootEntity = () => sd.ProfitDistributionByCustomer;
			this._store = db.ProfitDistributionByCustomers;
			this._saveStore = db.ProfitDistributionByCustomers;
			this._lookupFields = { id: "", name: "" };

			this.Customer
				.lookup(() => sd.Party);
		}

		_ProfitDistributionByCustomer = new SemanticMember()
			.localizeTitle({ ru: "Распределение выручки по заказчикам" })
			.lookup(() => sd.ProfitDistributionByCustomer);
				

		Customer = this.member()
			.lookup(() => sd.Party)
			.entityName();
	

	}


	/** Распределение выручки по провайдерам */
	export interface IProfitDistributionByProviderSemantic extends IProfitDistributionTotalSemantic
	{
		//se.Provider,
		//se.Rank,
		//se.SellCount,
		//se.RefundCount,
		//se.VoidCount,
		//se.Currency,
		//se.SellGrandTotal,
		//se.RefundGrandTotal,
		//se.GrandTotal,
		//se.Total,
		//se.ServiceFee,
		//se.Commission,
		//se.AgentTotal,
		//se.Vat,

		Provider: SemanticMember;
	
	}

	/** Распределение выручки по провайдерам */
	export class ProfitDistributionByProviderSemantic extends ProfitDistributionTotalSemantic implements IProfitDistributionByProviderSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProfitDistributionByProvider";
			this._names = "ProfitDistributionByProviders";
			
			this._isQueryResult = true;
			this._localizeTitle({ ru: "Распределение выручки по провайдерам" });

			this._getDerivedEntities = null;

			this._className = "ProfitDistributionByProvider";
			this._getRootEntity = () => sd.ProfitDistributionByProvider;
			this._store = db.ProfitDistributionByProviders;
			this._saveStore = db.ProfitDistributionByProviders;
			this._lookupFields = { id: "", name: "" };

			this.Provider
				.lookup(() => sd.Organization);
		}

		_ProfitDistributionByProvider = new SemanticMember()
			.localizeTitle({ ru: "Распределение выручки по провайдерам" })
			.lookup(() => sd.ProfitDistributionByProvider);
				

		Provider = this.member()
			.lookup(() => sd.Organization);
	

	}


	
	export interface IProductFilterSemantic extends ISemanticEntity
	{
		//se.Provider,
		//se.IssueDate,
		//se.Seller,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Owner,
		//se.AllowVoided,

		Provider: SemanticMember;

		IssueDate: SemanticMember;

		Seller: SemanticMember;

		IssueMonth: SemanticMember;

		MinIssueDate: SemanticMember;

		MaxIssueDate: SemanticMember;

		Type: SemanticMember;

		/** Название услуги */
		Name: SemanticMember;

		State: SemanticMember;

		/** Валюта услуги */
		ProductCurrency: SemanticMember;

		Customer: SemanticMember;

		Booker: SemanticMember;

		Ticketer: SemanticMember;

		Owner: SemanticMember;

		AllowVoided: SemanticMember;
	
	}

	
	export class ProductFilterSemantic extends SemanticEntity implements IProductFilterSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductFilter";
			this._names = "ProductFilter";
			
			this._isQueryParams = true;

			this._getDerivedEntities = () => [
				sd.EverydayProfitReportParams, sd.FlownReportParams, sd.ProductSummaryParams, sd.ProductTotalByBookerParams, sd.ProductTotalByDayParams, sd.ProductTotalByMonthParams, sd.ProductTotalByOwnerParams, sd.ProductTotalByProviderParams, sd.ProductTotalByQuarterParams, sd.ProductTotalBySellerParams, sd.ProductTotalByTypeParams, sd.ProductTotalByYearParams, sd.ProfitDistributionByCustomerParams, sd.ProfitDistributionByProviderParams
			];

			this.IssueDate
				.entityDate();

			this.IssueMonth
				.entityDate();

			this.MinIssueDate
				.entityDate();

			this.MaxIssueDate
				.entityDate();

			this.Type
				.localizeTitle({ ru: "Вид услуги" })
				.length(12, 0, 0)
				.entityType();

			this.Provider
				.lookup(() => sd.Organization);

			this.Customer
				.lookup(() => sd.Party);

			this.Booker
				.lookup(() => sd.Person);

			this.Ticketer
				.lookup(() => sd.Person);

			this.Seller
				.lookup(() => sd.Person);

			this.Owner
				.lookup(() => sd.Party);
		}

		Provider = this.member()
			.lookup(() => sd.Organization);

		IssueDate = this.member()
			.date()
			.entityName();

		Seller = this.member()
			.lookup(() => sd.Person);

		IssueMonth = this.member()
			.monthAndYear();

		MinIssueDate = this.member()
			.date();

		MaxIssueDate = this.member()
			.date();

		Type = this.member()
			.localizeTitle({ ru: "Вид услуги" })
			.enum(ProductType).enumIsFlags()
			.length(12, 0, 0);

		/** Название услуги */
		Name = this.member()
			.localizeTitle({ ru: "Название услуги" })
			.string();

		State = this.member()
			.localizeTitle({ ru: "Статус услуги" })
			.enum(ProductStateFilter)
			.required();

		/** Валюта услуги */
		ProductCurrency = this.member()
			.localizeTitle({ ru: "Валюта услуги" })
			.currencyCode();

		Customer = this.member()
			.lookup(() => sd.Party);

		Booker = this.member()
			.lookup(() => sd.Person);

		Ticketer = this.member()
			.lookup(() => sd.Person);

		Owner = this.member()
			.lookup(() => sd.Party);

		AllowVoided = this.member()
			.bool()
			.required();
	

	}


	
	export interface IEverydayProfitReportParamsSemantic extends IProductFilterSemantic
	{
		//se.ProductType,
		//se.Product,
		//se.PassengerName,
		//se.Itinerary,
		//se.StartDate,
		//se.FinishDate,
		//se.Country,
		//se.Fare,
		//se.Currency,
		//se.CurrencyRate,
		//se.EqualFare,
		//se.FeesTotal,
		//se.CancelFee,
		//se.Total,
		//se.Commission,
		//se.ServiceFee,
		//se.Vat,
		//se.GrandTotal,
		//se.Order,
		//se.Payer,
		//se.Invoice,
		//se.InvoiceDate,
		//se.CompletionCertificate,
		//se.CompletionCertificateDate,
		//se.Payment,
		//se.PaymentDate,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		/** Вид услуги */
		ProductType: SemanticMember;

		/** Услуга */
		Product: SemanticMember;

		PassengerName: SemanticMember;

		Itinerary: SemanticMember;

		StartDate: SemanticMember;

		FinishDate: SemanticMember;

		/** Страна */
		Country: SemanticMember;

		Fare: SemanticMember;

		Currency: SemanticMember;

		CurrencyRate: SemanticMember;

		EqualFare: SemanticMember;

		FeesTotal: SemanticMember;

		CancelFee: SemanticMember;

		Total: SemanticMember;

		Commission: SemanticMember;

		ServiceFee: SemanticMember;

		Vat: SemanticMember;

		GrandTotal: SemanticMember;

		/** Заказ */
		Order: SemanticMember;

		Payer: SemanticMember;

		/** Инвойс */
		Invoice: SemanticMember;

		InvoiceDate: SemanticMember;

		CompletionCertificate: SemanticMember;

		CompletionCertificateDate: SemanticMember;

		/** Платёж */
		Payment: SemanticMember;

		PaymentDate: SemanticMember;
	
	}

	
	export class EverydayProfitReportParamsSemantic extends ProductFilterSemantic implements IEverydayProfitReportParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "EverydayProfitReportParams";
			this._names = "EverydayProfitReportParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		/** Вид услуги */
		ProductType = this.member()
			.localizeTitle({ ru: "Вид услуги" })
			.enum(ProductType)
			.required()
			.length(12, 0, 0);

		/** Услуга */
		Product = this.member()
			.localizeTitle({ ru: "Услуга", rus: "Все услуги" })
			.lookup(() => sd.Product);

		PassengerName = this.member()
			.string();

		Itinerary = this.member()
			.string();

		StartDate = this.member()
			.date();

		FinishDate = this.member()
			.date();

		/** Страна */
		Country = this.member()
			.localizeTitle({ ru: "Страна", rus: "Страны" })
			.lookup(() => sd.Country);

		Fare = this.member()
			.money();

		Currency = this.member()
			.string();

		CurrencyRate = this.member()
			.float();

		EqualFare = this.member()
			.float();

		FeesTotal = this.member()
			.float();

		CancelFee = this.member()
			.float();

		Total = this.member()
			.float();

		Commission = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		Vat = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);

		Payer = this.member()
			.lookup(() => sd.Party);

		/** Инвойс */
		Invoice = this.member()
			.localizeTitle({ ru: "Инвойс", rus: "Счета/квитанции" })
			.lookup(() => sd.Invoice);

		InvoiceDate = this.member()
			.date();

		CompletionCertificate = this.member()
			.lookup(() => sd.Invoice);

		CompletionCertificateDate = this.member()
			.date();

		/** Платёж */
		Payment = this.member()
			.localizeTitle({ ru: "Платёж", rus: "Платежи" })
			.lookup(() => sd.Payment);

		PaymentDate = this.member()
			.date();
	

	}


	
	export interface IFlownReportParamsSemantic extends IProductFilterSemantic
	{
		//se.Date,
		//se.Op,
		//se.AC,
		//se.TicketNumber,
		//se.Client,
		//se.Passenger,
		//se.Route,
		//se.Curr,
		//se.Fare,
		//se.Tax,
		//se.Flown1,
		//se.Flown2,
		//se.Flown3,
		//se.Flown4,
		//se.Flown5,
		//se.Flown6,
		//se.Flown7,
		//se.Flown8,
		//se.Flown9,
		//se.Flown10,
		//se.Flown11,
		//se.Flown12,
		//se.TourCode,
		//se.CheapTicket,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Date: SemanticMember;

		Op: SemanticMember;

		AC: SemanticMember;

		TicketNumber: SemanticMember;

		Client: SemanticMember;

		Passenger: SemanticMember;

		Route: SemanticMember;

		Curr: SemanticMember;

		Fare: SemanticMember;

		Tax: SemanticMember;

		Flown1: SemanticMember;

		Flown2: SemanticMember;

		Flown3: SemanticMember;

		Flown4: SemanticMember;

		Flown5: SemanticMember;

		Flown6: SemanticMember;

		Flown7: SemanticMember;

		Flown8: SemanticMember;

		Flown9: SemanticMember;

		Flown10: SemanticMember;

		Flown11: SemanticMember;

		Flown12: SemanticMember;

		TourCode: SemanticMember;

		CheapTicket: SemanticMember;
	
	}

	
	export class FlownReportParamsSemantic extends ProductFilterSemantic implements IFlownReportParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "FlownReportParams";
			this._names = "FlownReportParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Date = this.member()
			.date()
			.required();

		Op = this.member()
			.string();

		AC = this.member()
			.string();

		TicketNumber = this.member()
			.lookup(() => sd.AviaDocument);

		Client = this.member()
			.lookup(() => sd.Party);

		Passenger = this.member()
			.string();

		Route = this.member()
			.string();

		Curr = this.member()
			.string();

		Fare = this.member()
			.float();

		Tax = this.member()
			.float();

		Flown1 = this.member()
			.float();

		Flown2 = this.member()
			.float();

		Flown3 = this.member()
			.float();

		Flown4 = this.member()
			.float();

		Flown5 = this.member()
			.float();

		Flown6 = this.member()
			.float();

		Flown7 = this.member()
			.float();

		Flown8 = this.member()
			.float();

		Flown9 = this.member()
			.float();

		Flown10 = this.member()
			.float();

		Flown11 = this.member()
			.float();

		Flown12 = this.member()
			.float();

		TourCode = this.member()
			.string();

		CheapTicket = this.member()
			.lookup(() => sd.AviaDocument);
	

	}


	
	export interface IOrderBalanceParamsSemantic extends ISemanticEntity
	{
		//se.Order,
		//se.IssueDate,
		//se.Customer,
		//se.Currency,
		//se.Delivered,
		//se.Paid,
		//se.Balance,

		/** Заказ */
		Order: SemanticMember;

		IssueDate: SemanticMember;

		Customer: SemanticMember;

		Currency: SemanticMember;

		Delivered: SemanticMember;

		Paid: SemanticMember;

		Balance: SemanticMember;
	
	}

	
	export class OrderBalanceParamsSemantic extends SemanticEntity implements IOrderBalanceParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "OrderBalanceParams";
			this._names = "OrderBalanceParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);

		IssueDate = this.member()
			.date();

		Customer = this.member()
			.lookup(() => sd.Party);

		Currency = this.member()
			.string();

		Delivered = this.member()
			.float()
			.required();

		Paid = this.member()
			.float()
			.required();

		Balance = this.member()
			.float()
			.required();
	

	}


	
	export interface IProductSummaryParamsSemantic extends IProductFilterSemantic
	{
		//se.Itinerary,
		//se.IsRefund,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Order,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Itinerary: SemanticMember;

		IsRefund: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		/** Заказ */
		Order: SemanticMember;
	
	}

	
	export class ProductSummaryParamsSemantic extends ProductFilterSemantic implements IProductSummaryParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductSummaryParams";
			this._names = "ProductSummaryParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Itinerary = this.member()
			.string();

		IsRefund = this.member()
			.bool()
			.required();

		Total = this.member()
			.money();

		ServiceFee = this.member()
			.money();

		GrandTotal = this.member()
			.money();

		/** Заказ */
		Order = this.member()
			.localizeTitle({ ru: "Заказ", rus: "Заказы" })
			.lookup(() => sd.Order);
	

	}


	
	export interface IProductTotalByBookerParamsSemantic extends IProductFilterSemantic
	{
		//se.Rank,
		//se.BookerName,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Rank: SemanticMember;

		BookerName: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalByBookerParamsSemantic extends ProductFilterSemantic implements IProductTotalByBookerParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByBookerParams";
			this._names = "ProductTotalByBookerParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Rank = this.member()
			.int()
			.required();

		BookerName = this.member()
			.string();

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProductTotalByDayParamsSemantic extends IProductFilterSemantic
	{
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalByDayParamsSemantic extends ProductFilterSemantic implements IProductTotalByDayParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByDayParams";
			this._names = "ProductTotalByDayParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProductTotalByMonthParamsSemantic extends IProductFilterSemantic
	{
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalByMonthParamsSemantic extends ProductFilterSemantic implements IProductTotalByMonthParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByMonthParams";
			this._names = "ProductTotalByMonthParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProductTotalByOwnerParamsSemantic extends IProductFilterSemantic
	{
		//se.Rank,
		//se.OwnerName,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Rank: SemanticMember;

		OwnerName: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalByOwnerParamsSemantic extends ProductFilterSemantic implements IProductTotalByOwnerParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByOwnerParams";
			this._names = "ProductTotalByOwnerParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Rank = this.member()
			.int()
			.required();

		OwnerName = this.member()
			.string();

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProductTotalByProviderParamsSemantic extends IProductFilterSemantic
	{
		//se.Rank,
		//se.ProviderName,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Rank: SemanticMember;

		ProviderName: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalByProviderParamsSemantic extends ProductFilterSemantic implements IProductTotalByProviderParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByProviderParams";
			this._names = "ProductTotalByProviderParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Rank = this.member()
			.int()
			.required();

		ProviderName = this.member()
			.string();

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProductTotalByQuarterParamsSemantic extends IProductFilterSemantic
	{
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalByQuarterParamsSemantic extends ProductFilterSemantic implements IProductTotalByQuarterParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByQuarterParams";
			this._names = "ProductTotalByQuarterParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProductTotalBySellerParamsSemantic extends IProductFilterSemantic
	{
		//se.Rank,
		//se.SellerName,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Rank: SemanticMember;

		SellerName: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalBySellerParamsSemantic extends ProductFilterSemantic implements IProductTotalBySellerParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalBySellerParams";
			this._names = "ProductTotalBySellerParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Rank = this.member()
			.int()
			.required();

		SellerName = this.member()
			.string();

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProductTotalByTypeParamsSemantic extends IProductFilterSemantic
	{
		//se.Rank,
		//se.TypeName,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Rank: SemanticMember;

		TypeName: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalByTypeParamsSemantic extends ProductFilterSemantic implements IProductTotalByTypeParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByTypeParams";
			this._names = "ProductTotalByTypeParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Rank = this.member()
			.int()
			.required();

		TypeName = this.member()
			.string();

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProductTotalByYearParamsSemantic extends IProductFilterSemantic
	{
		//se.Year,
		//se.Total,
		//se.ServiceFee,
		//se.GrandTotal,
		//se.Note,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Year: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		GrandTotal: SemanticMember;

		Note: SemanticMember;
	
	}

	
	export class ProductTotalByYearParamsSemantic extends ProductFilterSemantic implements IProductTotalByYearParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByYearParams";
			this._names = "ProductTotalByYearParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Year = this.member()
			.int()
			.required();

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Note = this.member()
			.string();
	

	}


	
	export interface IProfitDistributionByCustomerParamsSemantic extends IProductFilterSemantic
	{
		//se.Rank,
		//se.SellCount,
		//se.RefundCount,
		//se.VoidCount,
		//se.Currency,
		//se.SellGrandTotal,
		//se.RefundGrandTotal,
		//se.GrandTotal,
		//se.Total,
		//se.ServiceFee,
		//se.Commission,
		//se.AgentTotal,
		//se.Vat,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Rank: SemanticMember;

		SellCount: SemanticMember;

		RefundCount: SemanticMember;

		VoidCount: SemanticMember;

		Currency: SemanticMember;

		SellGrandTotal: SemanticMember;

		RefundGrandTotal: SemanticMember;

		GrandTotal: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		Commission: SemanticMember;

		AgentTotal: SemanticMember;

		Vat: SemanticMember;
	
	}

	
	export class ProfitDistributionByCustomerParamsSemantic extends ProductFilterSemantic implements IProfitDistributionByCustomerParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProfitDistributionByCustomerParams";
			this._names = "ProfitDistributionByCustomerParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Rank = this.member()
			.int()
			.required();

		SellCount = this.member()
			.int()
			.required();

		RefundCount = this.member()
			.int()
			.required();

		VoidCount = this.member()
			.int()
			.required();

		Currency = this.member()
			.string();

		SellGrandTotal = this.member()
			.float();

		RefundGrandTotal = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		Commission = this.member()
			.float();

		AgentTotal = this.member()
			.float();

		Vat = this.member()
			.float();
	

	}


	
	export interface IProfitDistributionByProviderParamsSemantic extends IProductFilterSemantic
	{
		//se.Rank,
		//se.SellCount,
		//se.RefundCount,
		//se.VoidCount,
		//se.Currency,
		//se.SellGrandTotal,
		//se.RefundGrandTotal,
		//se.GrandTotal,
		//se.Total,
		//se.ServiceFee,
		//se.Commission,
		//se.AgentTotal,
		//se.Vat,
		//se.IssueDate,
		//se.IssueMonth,
		//se.MinIssueDate,
		//se.MaxIssueDate,
		//se.Type,
		//se.Name,
		//se.State,
		//se.ProductCurrency,
		//se.Provider,
		//se.Customer,
		//se.Booker,
		//se.Ticketer,
		//se.Seller,
		//se.Owner,
		//se.AllowVoided,

		Rank: SemanticMember;

		SellCount: SemanticMember;

		RefundCount: SemanticMember;

		VoidCount: SemanticMember;

		Currency: SemanticMember;

		SellGrandTotal: SemanticMember;

		RefundGrandTotal: SemanticMember;

		GrandTotal: SemanticMember;

		Total: SemanticMember;

		ServiceFee: SemanticMember;

		Commission: SemanticMember;

		AgentTotal: SemanticMember;

		Vat: SemanticMember;
	
	}

	
	export class ProfitDistributionByProviderParamsSemantic extends ProductFilterSemantic implements IProfitDistributionByProviderParamsSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProfitDistributionByProviderParams";
			this._names = "ProfitDistributionByProviderParams";
			
			this._isQueryParams = true;

			this._getDerivedEntities = null;
		}

		Rank = this.member()
			.int()
			.required();

		SellCount = this.member()
			.int()
			.required();

		RefundCount = this.member()
			.int()
			.required();

		VoidCount = this.member()
			.int()
			.required();

		Currency = this.member()
			.string();

		SellGrandTotal = this.member()
			.float();

		RefundGrandTotal = this.member()
			.float();

		GrandTotal = this.member()
			.float();

		Total = this.member()
			.float();

		ServiceFee = this.member()
			.float();

		Commission = this.member()
			.float();

		AgentTotal = this.member()
			.float();

		Vat = this.member()
			.float();
	

	}


	/** Применить к документам */
	export interface IGdsAgent_ApplyToUnassignedSemantic extends IDomainActionSemantic
	{
		//se.DateFrom,
		//se.DateTo,
		//se.ProductCount,
		//se.GdsAgent,

		/** С даты */
		DateFrom: SemanticMember;

		/** По дату */
		DateTo: SemanticMember;

		/** Кол-во несвязанных услуг */
		ProductCount: SemanticMember;

		/** Gds-агент */
		GdsAgent: SemanticMember;
	
	}

	/** Применить к документам */
	export class GdsAgent_ApplyToUnassignedSemantic extends DomainActionSemantic implements IGdsAgent_ApplyToUnassignedSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "GdsAgent_ApplyToUnassigned";
			this._names = "GdsAgent_ApplyToUnassigned";
			
			this._isDomainAction = true;
			this._localizeTitle({ ru: "Применить к документам" });

			this._getDerivedEntities = null;

			this._className = "GdsAgent_ApplyToUnassigned";
			this._getRootEntity = () => sd.GdsAgent_ApplyToUnassigned;
			this._store = db.GdsAgent_ApplyToUnassigned;
			this._saveStore = db.GdsAgent_ApplyToUnassigned;
			this._lookupFields = { id: "Id", name: "" };
		}

		_GdsAgent_ApplyToUnassigned = new SemanticMember()
			.localizeTitle({ ru: "Применить к документам" })
			.lookup(() => sd.GdsAgent_ApplyToUnassigned);
				

		/** С даты */
		DateFrom = this.member()
			.localizeTitle({ ru: "С даты" })
			.date()
			.subject();

		/** По дату */
		DateTo = this.member()
			.localizeTitle({ ru: "По дату" })
			.date()
			.subject();

		/** Кол-во несвязанных услуг */
		ProductCount = this.member()
			.localizeTitle({ ru: "Кол-во несвязанных услуг" })
			.int()
			.readOnly()
			.nonsaved();

		/** Gds-агент */
		GdsAgent = this.member()
			.localizeTitle({ ru: "Gds-агент", rus: "Gds-агенты" })
			.lookup(() => sd.GdsAgent)
			.required()
			.subject();
	

	}


	
	export interface IOrderTotalByDateSemantic extends ISemanticEntity
	{
		//se.Date,
		//se.Total,
		//se.SumTotal,

		/** Дата заказа */
		Date: SemanticMember;

		/** Итого к оплате */
		Total: SemanticMember;

		/** Всего за период к оплате */
		SumTotal: SemanticMember;
	
	}

	
	export class OrderTotalByDateSemantic extends SemanticEntity implements IOrderTotalByDateSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "OrderTotalByDate";
			this._names = "OrderTotalByDate";
			

			this._getDerivedEntities = null;
		}

		/** Дата заказа */
		Date = this.member()
			.localizeTitle({ ru: "Дата заказа" })
			.date();

		/** Итого к оплате */
		Total = this.member()
			.localizeTitle({ ru: "Итого к оплате" })
			.float();

		/** Всего за период к оплате */
		SumTotal = this.member()
			.localizeTitle({ ru: "Всего за период к оплате" })
			.float();
	

	}


	
	export interface IProductTotalByDateSemantic extends ISemanticEntity
	{
		//se.Date,
		//se.GrandTotal,
		//se.SumGrandTotal,

		/** Дата услуги */
		Date: SemanticMember;

		/** Итого к оплате */
		GrandTotal: SemanticMember;

		/** Всего за период к оплате */
		SumGrandTotal: SemanticMember;
	
	}

	
	export class ProductTotalByDateSemantic extends SemanticEntity implements IProductTotalByDateSemantic
	{

		constructor()
		{
			super();
		
			this._isAbstract = false;
			this._name = "ProductTotalByDate";
			this._names = "ProductTotalByDate";
			

			this._getDerivedEntities = null;
		}

		/** Дата услуги */
		Date = this.member()
			.localizeTitle({ ru: "Дата услуги" })
			.date();

		/** Итого к оплате */
		GrandTotal = this.member()
			.localizeTitle({ ru: "Итого к оплате" })
			.float();

		/** Всего за период к оплате */
		SumGrandTotal = this.member()
			.localizeTitle({ ru: "Всего за период к оплате" })
			.float();
	

	}



	/** Динамика заказов ответственного */
	export class OrderByAssignedTo_TotalByIssueDateSemantic extends OrderTotalByDateSemantic
	{
		constructor()
		{
			super();
		
			this._names = this._name = "OrderByAssignedTo_TotalByIssueDate";
			this._store = db.OrderByAssignedTo_TotalByIssueDate;
			this._isDomainFunction = true;
			
			this._localizeTitle({ ru: "Динамика заказов ответственного" });
		}

		AssignedToId = this.member().string();
	}

	/** Динамика заказов заказчика */
	export class OrderByCustomer_TotalByIssueDateSemantic extends OrderTotalByDateSemantic
	{
		constructor()
		{
			super();
		
			this._names = this._name = "OrderByCustomer_TotalByIssueDate";
			this._store = db.OrderByCustomer_TotalByIssueDate;
			this._isDomainFunction = true;
			
			this._localizeTitle({ ru: "Динамика заказов заказчика" });
		}

		CustomerId = this.member().string();
	}

	/** Динамика заказов владельца */
	export class OrderByOwner_TotalByIssueDateSemantic extends OrderTotalByDateSemantic
	{
		constructor()
		{
			super();
		
			this._names = this._name = "OrderByOwner_TotalByIssueDate";
			this._store = db.OrderByOwner_TotalByIssueDate;
			this._isDomainFunction = true;
			
			this._localizeTitle({ ru: "Динамика заказов владельца" });
		}

		OwnerId = this.member().string();
	}

	/** Динамика забронированных услуг */
	export class ProductByBooker_TotalByIssueDateSemantic extends ProductTotalByDateSemantic
	{
		constructor()
		{
			super();
		
			this._names = this._name = "ProductByBooker_TotalByIssueDate";
			this._store = db.ProductByBooker_TotalByIssueDate;
			this._isDomainFunction = true;
			
			this._localizeTitle({ ru: "Динамика забронированных услуг" });
		}

		BookerId = this.member().string();
	}

	/** Динамика поставленных услуг */
	export class ProductByProvider_TotalByIssueDateSemantic extends ProductTotalByDateSemantic
	{
		constructor()
		{
			super();
		
			this._names = this._name = "ProductByProvider_TotalByIssueDate";
			this._store = db.ProductByProvider_TotalByIssueDate;
			this._isDomainFunction = true;
			
			this._localizeTitle({ ru: "Динамика поставленных услуг" });
		}

		ProviderId = this.member().string();
	}

	/** Динамика проданных услуг */
	export class ProductBySeller_TotalByIssueDateSemantic extends ProductTotalByDateSemantic
	{
		constructor()
		{
			super();
		
			this._names = this._name = "ProductBySeller_TotalByIssueDate";
			this._store = db.ProductBySeller_TotalByIssueDate;
			this._isDomainFunction = true;
			
			this._localizeTitle({ ru: "Динамика проданных услуг" });
		}

		SellerId = this.member().string();
	}

	/** Динамика тикетированных услуг */
	export class ProductByTicketer_TotalByIssueDateSemantic extends ProductTotalByDateSemantic
	{
		constructor()
		{
			super();
		
			this._names = this._name = "ProductByTicketer_TotalByIssueDate";
			this._store = db.ProductByTicketer_TotalByIssueDate;
			this._isDomainFunction = true;
			
			this._localizeTitle({ ru: "Динамика тикетированных услуг" });
		}

		TicketerId = this.member().string();
	}

	export class DomainSemantic extends SemanticDomain
	{
		/** Проживание */
		Accommodation = <IAccommodationSemantic>this.entity(new AccommodationSemantic());
		/** Провайдер проживания */
		AccommodationProvider = <IAccommodationProviderSemantic>this.entity(new AccommodationProviderSemantic());
		/** Тип проживания */
		AccommodationType = <IAccommodationTypeSemantic>this.entity(new AccommodationTypeSemantic());
		/** Владелец документов (активный) */
		ActiveOwner = <IActiveOwnerSemantic>this.entity(new ActiveOwnerSemantic());
		/** Агент */
		Agent = <IAgentSemantic>this.entity(new AgentSemantic());
		/** Авиакомпания */
		Airline = <IAirlineSemantic>this.entity(new AirlineSemantic());
		/** Сервис-класс авиакомпании */
		AirlineServiceClass = <IAirlineServiceClassSemantic>this.entity(new AirlineServiceClassSemantic());
		/** Аэропорт */
		Airport = <IAirportSemantic>this.entity(new AirportSemantic());
		/** Авиадокумент */
		AviaDocument = <IAviaDocumentSemantic>this.entity(new AviaDocumentSemantic());
		/** МСО */
		AviaMco = <IAviaMcoSemantic>this.entity(new AviaMcoSemantic());
		/** Возврат авиабилета */
		AviaRefund = <IAviaRefundSemantic>this.entity(new AviaRefundSemantic());
		/** Авиабилет */
		AviaTicket = <IAviaTicketSemantic>this.entity(new AviaTicketSemantic());
		/** Банковский счёт */
		BankAccount = <IBankAccountSemantic>this.entity(new BankAccountSemantic());
		/** Автобусный билет или возврат */
		BusDocument = <IBusDocumentSemantic>this.entity(new BusDocumentSemantic());
		/** Автобусный билет */
		BusTicket = <IBusTicketSemantic>this.entity(new BusTicketSemantic());
		/** Провайдер автобусных билетов */
		BusTicketProvider = <IBusTicketProviderSemantic>this.entity(new BusTicketProviderSemantic());
		/** Возврат автобусного билета */
		BusTicketRefund = <IBusTicketRefundSemantic>this.entity(new BusTicketRefundSemantic());
		/** Аренда автомобиля */
		CarRental = <ICarRentalSemantic>this.entity(new CarRentalSemantic());
		/** Провайдер аренды авто */
		CarRentalProvider = <ICarRentalProviderSemantic>this.entity(new CarRentalProviderSemantic());
		/** ПКО */
		CashInOrderPayment = <ICashInOrderPaymentSemantic>this.entity(new CashInOrderPaymentSemantic());
		/** РКО */
		CashOutOrderPayment = <ICashOutOrderPaymentSemantic>this.entity(new CashOutOrderPaymentSemantic());
		/** Тип питания */
		CateringType = <ICateringTypeSemantic>this.entity(new CateringTypeSemantic());
		/** Кассовый чек */
		CheckPayment = <ICheckPaymentSemantic>this.entity(new CheckPaymentSemantic());
		/** Накладная */
		Consignment = <IConsignmentSemantic>this.entity(new ConsignmentSemantic());
		/** Страна */
		Country = <ICountrySemantic>this.entity(new CountrySemantic());
		/** Курс валюты */
		CurrencyDailyRate = <ICurrencyDailyRateSemantic>this.entity(new CurrencyDailyRateSemantic());
		/** Заказчик */
		Customer = <ICustomerSemantic>this.entity(new CustomerSemantic());
		/** Подразделение */
		Department = <IDepartmentSemantic>this.entity(new DepartmentSemantic());
		/** Доступ к документам */
		DocumentAccess = <IDocumentAccessSemantic>this.entity(new DocumentAccessSemantic());
		/** Владелец документов */
		DocumentOwner = <IDocumentOwnerSemantic>this.entity(new DocumentOwnerSemantic());
		/** Электронный платеж */
		ElectronicPayment = <IElectronicPaymentSemantic>this.entity(new ElectronicPaymentSemantic());
		/** Сотрудник */
		Employee = <IEmployeeSemantic>this.entity(new EmployeeSemantic());
		/** Ежедневный отчет по выручке */
		EverydayProfitReport = <IEverydayProfitReportSemantic>this.entity(new EverydayProfitReportSemantic());

		EverydayProfitReportParams = <IEverydayProfitReportParamsSemantic>this.entity(new EverydayProfitReportParamsSemantic());
		/** Экскурсия */
		Excursion = <IExcursionSemantic>this.entity(new ExcursionSemantic());

		File = <IFileSemantic>this.entity(new FileSemantic());
		/** Полетный сегмент */
		FlightSegment = <IFlightSegmentSemantic>this.entity(new FlightSegmentSemantic());
		/** Flown-отчет */
		FlownReport = <IFlownReportSemantic>this.entity(new FlownReportSemantic());

		FlownReportParams = <IFlownReportParamsSemantic>this.entity(new FlownReportParamsSemantic());
		/** Gds-агент */
		GdsAgent = <IGdsAgentSemantic>this.entity(new GdsAgentSemantic());
		/** Применить к документам */
		GdsAgent_ApplyToUnassigned = <IGdsAgent_ApplyToUnassignedSemantic>this.entity(new GdsAgent_ApplyToUnassignedSemantic());
		/** Gds-файл */
		GdsFile = <IGdsFileSemantic>this.entity(new GdsFileSemantic());
		/** Дополнительная услуга */
		GenericProduct = <IGenericProductSemantic>this.entity(new GenericProductSemantic());
		/** Провайдер дополнительных услуг */
		GenericProductProvider = <IGenericProductProviderSemantic>this.entity(new GenericProductProviderSemantic());
		/** Вид дополнительной услуги */
		GenericProductType = <IGenericProductTypeSemantic>this.entity(new GenericProductTypeSemantic());

		Identity = <IIdentitySemantic>this.entity(new IdentitySemantic());
		/** Страховка */
		Insurance = <IInsuranceSemantic>this.entity(new InsuranceSemantic());
		/** Страховая компания */
		InsuranceCompany = <IInsuranceCompanySemantic>this.entity(new InsuranceCompanySemantic());
		/** Страховка или возврат */
		InsuranceDocument = <IInsuranceDocumentSemantic>this.entity(new InsuranceDocumentSemantic());
		/** Возврат страховки */
		InsuranceRefund = <IInsuranceRefundSemantic>this.entity(new InsuranceRefundSemantic());

		InternalIdentity = <IInternalIdentitySemantic>this.entity(new InternalIdentitySemantic());
		/** Внутренний перевод */
		InternalTransfer = <IInternalTransferSemantic>this.entity(new InternalTransferSemantic());
		/** Инвойс */
		Invoice = <IInvoiceSemantic>this.entity(new InvoiceSemantic());
		/** Студенческий билет */
		Isic = <IIsicSemantic>this.entity(new IsicSemantic());
		/** Выпущенная накладная */
		IssuedConsignment = <IIssuedConsignmentSemantic>this.entity(new IssuedConsignmentSemantic());
		/** Мильная карта */
		MilesCard = <IMilesCardSemantic>this.entity(new MilesCardSemantic());
		/** Начальный остаток */
		OpeningBalance = <IOpeningBalanceSemantic>this.entity(new OpeningBalanceSemantic());
		/** Заказ */
		Order = <IOrderSemantic>this.entity(new OrderSemantic());
		/** Баланс */
		OrderBalance = <IOrderBalanceSemantic>this.entity(new OrderBalanceSemantic());

		OrderBalanceParams = <IOrderBalanceParamsSemantic>this.entity(new OrderBalanceParamsSemantic());
		/** Динамика заказов ответственного */
		OrderByAssignedTo_TotalByIssueDate = this.entity(new OrderByAssignedTo_TotalByIssueDateSemantic());
		/** Динамика заказов заказчика */
		OrderByCustomer_TotalByIssueDate = this.entity(new OrderByCustomer_TotalByIssueDateSemantic());
		/** Динамика заказов владельца */
		OrderByOwner_TotalByIssueDate = this.entity(new OrderByOwner_TotalByIssueDateSemantic());
		/** Чек */
		OrderCheck = <IOrderCheckSemantic>this.entity(new OrderCheckSemantic());
		/** Позиция заказа */
		OrderItem = <IOrderItemSemantic>this.entity(new OrderItemSemantic());

		OrderTotalByDate = <IOrderTotalByDateSemantic>this.entity(new OrderTotalByDateSemantic());
		/** Организация */
		Organization = <IOrganizationSemantic>this.entity(new OrganizationSemantic());
		/** Контрагент */
		Party = <IPartySemantic>this.entity(new PartySemantic());
		/** Паспорт */
		Passport = <IPassportSemantic>this.entity(new PassportSemantic());
		/** Ж/д билет */
		Pasteboard = <IPasteboardSemantic>this.entity(new PasteboardSemantic());
		/** Провайдер ж/д билетов */
		PasteboardProvider = <IPasteboardProviderSemantic>this.entity(new PasteboardProviderSemantic());
		/** Возврат ж/д билета */
		PasteboardRefund = <IPasteboardRefundSemantic>this.entity(new PasteboardRefundSemantic());
		/** Платёж */
		Payment = <IPaymentSemantic>this.entity(new PaymentSemantic());
		/** Платёжная система */
		PaymentSystem = <IPaymentSystemSemantic>this.entity(new PaymentSystemSemantic());
		/** Персона */
		Person = <IPersonSemantic>this.entity(new PersonSemantic());
		/** Услуга */
		Product = <IProductSemantic>this.entity(new ProductSemantic());
		/** Динамика забронированных услуг */
		ProductByBooker_TotalByIssueDate = this.entity(new ProductByBooker_TotalByIssueDateSemantic());
		/** Динамика поставленных услуг */
		ProductByProvider_TotalByIssueDate = this.entity(new ProductByProvider_TotalByIssueDateSemantic());
		/** Динамика проданных услуг */
		ProductBySeller_TotalByIssueDate = this.entity(new ProductBySeller_TotalByIssueDateSemantic());
		/** Динамика тикетированных услуг */
		ProductByTicketer_TotalByIssueDate = this.entity(new ProductByTicketer_TotalByIssueDateSemantic());

		ProductFilter = <IProductFilterSemantic>this.entity(new ProductFilterSemantic());
		/** Пассажир */
		ProductPassenger = <IProductPassengerSemantic>this.entity(new ProductPassengerSemantic());
		/** Сводка по услугам */
		ProductSummary = <IProductSummarySemantic>this.entity(new ProductSummarySemantic());

		ProductSummaryParams = <IProductSummaryParamsSemantic>this.entity(new ProductSummaryParamsSemantic());

		ProductTotal = <IProductTotalSemantic>this.entity(new ProductTotalSemantic());
		/** Услуги итого по бронировщику */
		ProductTotalByBooker = <IProductTotalByBookerSemantic>this.entity(new ProductTotalByBookerSemantic());

		ProductTotalByBookerParams = <IProductTotalByBookerParamsSemantic>this.entity(new ProductTotalByBookerParamsSemantic());

		ProductTotalByDate = <IProductTotalByDateSemantic>this.entity(new ProductTotalByDateSemantic());
		/** Услуги итого посуточно */
		ProductTotalByDay = <IProductTotalByDaySemantic>this.entity(new ProductTotalByDaySemantic());

		ProductTotalByDayParams = <IProductTotalByDayParamsSemantic>this.entity(new ProductTotalByDayParamsSemantic());
		/** Услуги итого помесячно */
		ProductTotalByMonth = <IProductTotalByMonthSemantic>this.entity(new ProductTotalByMonthSemantic());

		ProductTotalByMonthParams = <IProductTotalByMonthParamsSemantic>this.entity(new ProductTotalByMonthParamsSemantic());
		/** Услуги итого по владельцу */
		ProductTotalByOwner = <IProductTotalByOwnerSemantic>this.entity(new ProductTotalByOwnerSemantic());

		ProductTotalByOwnerParams = <IProductTotalByOwnerParamsSemantic>this.entity(new ProductTotalByOwnerParamsSemantic());
		/** Услуги итого по провайдеру */
		ProductTotalByProvider = <IProductTotalByProviderSemantic>this.entity(new ProductTotalByProviderSemantic());

		ProductTotalByProviderParams = <IProductTotalByProviderParamsSemantic>this.entity(new ProductTotalByProviderParamsSemantic());
		/** Услуги итого поквартально */
		ProductTotalByQuarter = <IProductTotalByQuarterSemantic>this.entity(new ProductTotalByQuarterSemantic());

		ProductTotalByQuarterParams = <IProductTotalByQuarterParamsSemantic>this.entity(new ProductTotalByQuarterParamsSemantic());
		/** Услуги итого по продавцу */
		ProductTotalBySeller = <IProductTotalBySellerSemantic>this.entity(new ProductTotalBySellerSemantic());

		ProductTotalBySellerParams = <IProductTotalBySellerParamsSemantic>this.entity(new ProductTotalBySellerParamsSemantic());
		/** Услуги итого по видам услуг */
		ProductTotalByType = <IProductTotalByTypeSemantic>this.entity(new ProductTotalByTypeSemantic());

		ProductTotalByTypeParams = <IProductTotalByTypeParamsSemantic>this.entity(new ProductTotalByTypeParamsSemantic());
		/** Услуги итого по годам */
		ProductTotalByYear = <IProductTotalByYearSemantic>this.entity(new ProductTotalByYearSemantic());

		ProductTotalByYearParams = <IProductTotalByYearParamsSemantic>this.entity(new ProductTotalByYearParamsSemantic());
		/** Распределение выручки по заказчикам */
		ProfitDistributionByCustomer = <IProfitDistributionByCustomerSemantic>this.entity(new ProfitDistributionByCustomerSemantic());

		ProfitDistributionByCustomerParams = <IProfitDistributionByCustomerParamsSemantic>this.entity(new ProfitDistributionByCustomerParamsSemantic());
		/** Распределение выручки по провайдерам */
		ProfitDistributionByProvider = <IProfitDistributionByProviderSemantic>this.entity(new ProfitDistributionByProviderSemantic());

		ProfitDistributionByProviderParams = <IProfitDistributionByProviderParamsSemantic>this.entity(new ProfitDistributionByProviderParamsSemantic());

		ProfitDistributionTotal = <IProfitDistributionTotalSemantic>this.entity(new ProfitDistributionTotalSemantic());
		/** Ж/д билет или возврат */
		RailwayDocument = <IRailwayDocumentSemantic>this.entity(new RailwayDocumentSemantic());
		/** Квитанция */
		Receipt = <IReceiptSemantic>this.entity(new ReceiptSemantic());
		/** Мобильный оператор */
		RoamingOperator = <IRoamingOperatorSemantic>this.entity(new RoamingOperatorSemantic());

		Sequence = <ISequenceSemantic>this.entity(new SequenceSemantic());
		/** SIM-карта */
		SimCard = <ISimCardSemantic>this.entity(new SimCardSemantic());
		/** Настройки системы */
		SystemConfiguration = <ISystemConfigurationSemantic>this.entity(new SystemConfigurationSemantic());
		/** Турпакет */
		Tour = <ITourSemantic>this.entity(new TourSemantic());
		/** Провайдер туров (готовых) */
		TourProvider = <ITourProviderSemantic>this.entity(new TourProviderSemantic());
		/** Трансфер */
		Transfer = <ITransferSemantic>this.entity(new TransferSemantic());
		/** Провайдер трансферов */
		TransferProvider = <ITransferProviderSemantic>this.entity(new TransferProviderSemantic());
		/** Пользователь */
		User = <IUserSemantic>this.entity(new UserSemantic());
		/** Безналичный платеж */
		WireTransfer = <IWireTransferSemantic>this.entity(new WireTransferSemantic());
	};

	export interface IDomainSemantic extends DomainSemantic { }

	export var sd: IDomainSemantic = new DomainSemantic();

}