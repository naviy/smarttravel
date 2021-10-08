module Luxena
{

	//#region Enums

	//#region TaxRate 

	export enum TaxRate
	{
		Default = 0,
		A = 1,
		B = 2,
		D = 5,
		None = -1,
	}

	export module TaxRate
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "TaxRate";
		export var _fullName = "Luxena.Travel.Domain.TaxRate";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.TaxRate'" + value + "'");
		

		export var _array = [
			{ Id: "Default", Value: 0, Name: "По умолчанию", TextIconHtml: "", ru: "По умолчанию" },
			{ Id: "A", Value: 1, Name: "А (с НДС)", TextIconHtml: "", ru: "А (с НДС)" },
			{ Id: "B", Value: 2, Name: "Б (без НДС)", TextIconHtml: "", ru: "Б (без НДС)" },
			{ Id: "D", Value: 5, Name: "Д (без НДС)", TextIconHtml: "", ru: "Д (без НДС)" },
			{ Id: "None", Value: -1, Name: "не печатать", TextIconHtml: "", ru: "не печатать" },
		];

		export var _maxLength = 9;

		export var _items = {
			"0": _array[0],
			"Default": _array[0],
			"1": _array[1],
			"A": _array[1],
			"2": _array[2],
			"B": _array[2],
			"5": _array[3],
			"D": _array[3],
			"-1": _array[4],
			"None": _array[4],
		};
			
	}

	//#endregion


	//#region InvoiceNumberMode 

	export enum InvoiceNumberMode
	{
		Default = 0,
		ByOrderNumber = 1,
	}

	export module InvoiceNumberMode
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "InvoiceNumberMode";
		export var _fullName = "Luxena.Travel.Domain.InvoiceNumberMode";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.InvoiceNumberMode'" + value + "'");
		

		export var _array = [
			{ Id: "Default", Value: 0, Name: "По умолчанию", TextIconHtml: "", ru: "По умолчанию" },
			{ Id: "ByOrderNumber", Value: 1, Name: "На основе номера заказа", TextIconHtml: "", ru: "На основе номера заказа" },
		];

		export var _maxLength = 17;

		export var _items = {
			"0": _array[0],
			"Default": _array[0],
			"1": _array[1],
			"ByOrderNumber": _array[1],
		};
			
	}

	//#endregion


	//#region CheckType 

	export enum CheckType
	{
		Unknown = 0,
		Sale = 1,
		Return = 2,
	}

	export module CheckType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "CheckType";
		export var _fullName = "Luxena.Travel.Domain.CheckType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.CheckType'" + value + "'");
		

		export var _array = [
			{ Id: "Unknown", Value: 0, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
			{ Id: "Sale", Value: 1, Name: "Чек продажи", TextIconHtml: "", ru: "Чек продажи" },
			{ Id: "Return", Value: 2, Name: "Чек возврата", TextIconHtml: "", ru: "Чек возврата" },
		];

		export var _maxLength = 9;

		export var _items = {
			"0": _array[0],
			"Unknown": _array[0],
			"1": _array[1],
			"Sale": _array[1],
			"2": _array[2],
			"Return": _array[2],
		};
			
	}

	//#endregion


	//#region CheckPaymentType 

	export enum CheckPaymentType
	{
		Cash = 0,
		Credit = 1,
		Check = 2,
		Card = 3,
	}

	export module CheckPaymentType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "CheckPaymentType";
		export var _fullName = "Luxena.Travel.Domain.CheckPaymentType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.CheckPaymentType'" + value + "'");
		

		export var _array = [
			{ Id: "Cash", Value: 0, Name: "Наличные", TextIconHtml: "", ru: "Наличные" },
			{ Id: "Credit", Value: 1, Name: "Кредит", TextIconHtml: "", ru: "Кредит" },
			{ Id: "Check", Value: 2, Name: "Чек", TextIconHtml: "", ru: "Чек" },
			{ Id: "Card", Value: 3, Name: "Карточка", TextIconHtml: "", ru: "Карточка" },
		];

		export var _maxLength = 6;

		export var _items = {
			"0": _array[0],
			"Cash": _array[0],
			"1": _array[1],
			"Credit": _array[1],
			"2": _array[2],
			"Check": _array[2],
			"3": _array[3],
			"Card": _array[3],
		};
			
	}

	//#endregion


	//#region InvoiceType 

	export enum InvoiceType
	{
		Invoice = 0,
		Receipt = 1,
		CompletionCertificate = 2,
	}

	export module InvoiceType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "InvoiceType";
		export var _fullName = "Luxena.Travel.Domain.InvoiceType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.InvoiceType'" + value + "'");
		

		export var _array = [
			{ Id: "Invoice", Value: 0, Name: "Счет", TextIconHtml: "", ru: "Счет" },
			{ Id: "Receipt", Value: 1, Name: "Квитанция", TextIconHtml: "", ru: "Квитанция" },
			{ Id: "CompletionCertificate", Value: 2, Name: "Акт выполненных работ", TextIconHtml: "", ru: "Акт выполненных работ" },
		];

		export var _maxLength = 16;

		export var _items = {
			"0": _array[0],
			"Invoice": _array[0],
			"1": _array[1],
			"Receipt": _array[1],
			"2": _array[2],
			"CompletionCertificate": _array[2],
		};
			
	}

	//#endregion


	//#region OrderItemLinkType 

	export enum OrderItemLinkType
	{
		ProductData = 0,
		ServiceFee = 1,
		FullDocument = 2,
	}

	export module OrderItemLinkType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "OrderItemLinkType";
		export var _fullName = "Luxena.Travel.Domain.OrderItemLinkType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.OrderItemLinkType'" + value + "'");
		

		export var _array = [
			{ Id: "ProductData", Value: 0, Name: "ProductData", TextIconHtml: "",  },
			{ Id: "ServiceFee", Value: 1, Name: "ServiceFee", TextIconHtml: "",  },
			{ Id: "FullDocument", Value: 2, Name: "FullDocument", TextIconHtml: "",  },
		];

		export var _maxLength = 9;

		export var _items = {
			"0": _array[0],
			"ProductData": _array[0],
			"1": _array[1],
			"ServiceFee": _array[1],
			"2": _array[2],
			"FullDocument": _array[2],
		};
			
	}

	//#endregion


	//#region PaymentForm 

	export enum PaymentForm
	{
		CashInOrder = 0,
		WireTransfer = 1,
		Check = 2,
		Electronic = 3,
		CashOutOrder = 4,
	}

	export module PaymentForm
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "PaymentForm";
		export var _fullName = "Luxena.Travel.Domain.PaymentForm";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.PaymentForm'" + value + "'");
		

		export var _array = [
			{ Id: "CashInOrder", Value: 0, Name: "ПКО", TextIconHtml: "", ru: "ПКО" },
			{ Id: "WireTransfer", Value: 1, Name: "Безналичный платеж", TextIconHtml: "", ru: "Безналичный платеж" },
			{ Id: "Check", Value: 2, Name: "Кассовый чек", TextIconHtml: "", ru: "Кассовый чек" },
			{ Id: "Electronic", Value: 3, Name: "Электронный платеж", TextIconHtml: "", ru: "Электронный платеж" },
			{ Id: "CashOutOrder", Value: 4, Name: "РКО", TextIconHtml: "", ru: "РКО" },
		];

		export var _maxLength = 14;

		export var _items = {
			"0": _array[0],
			"CashInOrder": _array[0],
			"1": _array[1],
			"WireTransfer": _array[1],
			"2": _array[2],
			"Check": _array[2],
			"3": _array[3],
			"Electronic": _array[3],
			"4": _array[4],
			"CashOutOrder": _array[4],
		};
			
	}

	//#endregion


	//#region ServiceFeeMode 

	export enum ServiceFeeMode
	{
		Join = 0,
		Separate = 1,
		AlwaysJoin = 2,
		AlwaysSeparate = 3,
	}

	export module ServiceFeeMode
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "ServiceFeeMode";
		export var _fullName = "Luxena.Travel.Domain.ServiceFeeMode";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ServiceFeeMode'" + value + "'");
		

		export var _array = [
			{ Id: "Join", Value: 0, Name: "Join", TextIconHtml: "",  },
			{ Id: "Separate", Value: 1, Name: "Separate", TextIconHtml: "",  },
			{ Id: "AlwaysJoin", Value: 2, Name: "AlwaysJoin", TextIconHtml: "",  },
			{ Id: "AlwaysSeparate", Value: 3, Name: "AlwaysSeparate", TextIconHtml: "",  },
		];

		export var _maxLength = 10;

		export var _items = {
			"0": _array[0],
			"Join": _array[0],
			"1": _array[1],
			"Separate": _array[1],
			"2": _array[2],
			"AlwaysJoin": _array[2],
			"3": _array[3],
			"AlwaysSeparate": _array[3],
		};
			
	}

	//#endregion


	//#region ProductStateFilter 

	export enum ProductStateFilter
	{
		OnlyProcessed = 0,
		All = 1,
		OnlyReservation = 2,
	}

	export module ProductStateFilter
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "ProductStateFilter";
		export var _fullName = "Luxena.Travel.Domain.ProductStateFilter";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ProductStateFilter'" + value + "'");
		

		export var _array = [
			{ Id: "OnlyProcessed", Value: 0, Name: "Только обработанные", TextIconHtml: "", ru: "Только обработанные" },
			{ Id: "All", Value: 1, Name: "Все", TextIconHtml: "", ru: "Все" },
			{ Id: "OnlyReservation", Value: 2, Name: "Только бронировки", TextIconHtml: "", ru: "Только бронировки" },
		];

		export var _maxLength = 14;

		export var _items = {
			"0": _array[0],
			"OnlyProcessed": _array[0],
			"1": _array[1],
			"All": _array[1],
			"2": _array[2],
			"OnlyReservation": _array[2],
		};
			
	}

	//#endregion


	//#region GdsPassportStatus 

	export enum GdsPassportStatus
	{
		Unknown = 0,
		Exist = 1,
		NotExist = 2,
		Incorrect = 3,
	}

	export module GdsPassportStatus
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "GdsPassportStatus";
		export var _fullName = "Luxena.Travel.Domain.GdsPassportStatus";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.GdsPassportStatus'" + value + "'");
		

		export var _array = [
			{ Id: "Unknown", Value: 0, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
			{ Id: "Exist", Value: 1, Name: "Есть", TextIconHtml: "", ru: "Есть" },
			{ Id: "NotExist", Value: 2, Name: "Нет", TextIconHtml: "", ru: "Нет" },
			{ Id: "Incorrect", Value: 3, Name: "Некорректен", TextIconHtml: "", ru: "Некорректен" },
		];

		export var _maxLength = 8;

		export var _items = {
			"0": _array[0],
			"Unknown": _array[0],
			"1": _array[1],
			"Exist": _array[1],
			"2": _array[2],
			"NotExist": _array[2],
			"3": _array[3],
			"Incorrect": _array[3],
		};
			
	}

	//#endregion


	//#region AirlinePassportRequirement 

	export enum AirlinePassportRequirement
	{
		SystemDefault = 0,
		Required = 1,
		NotRequired = 2,
	}

	export module AirlinePassportRequirement
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "AirlinePassportRequirement";
		export var _fullName = "Luxena.Travel.Domain.AirlinePassportRequirement";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.AirlinePassportRequirement'" + value + "'");
		

		export var _array = [
			{ Id: "SystemDefault", Value: 0, Name: "По умолчанию", TextIconHtml: "", ru: "По умолчанию" },
			{ Id: "Required", Value: 1, Name: "Требуется", TextIconHtml: "", ru: "Требуется" },
			{ Id: "NotRequired", Value: 2, Name: "Не требуется", TextIconHtml: "", ru: "Не требуется" },
		];

		export var _maxLength = 9;

		export var _items = {
			"0": _array[0],
			"SystemDefault": _array[0],
			"1": _array[1],
			"Required": _array[1],
			"2": _array[2],
			"NotRequired": _array[2],
		};
			
	}

	//#endregion


	//#region Gender 

	export enum Gender
	{
		Male = 0,
		Female = 1,
	}

	export module Gender
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "Gender";
		export var _fullName = "Luxena.Travel.Domain.Gender";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.Gender'" + value + "'");
		

		export var _array = [
			{ Id: "Male", Value: 0, Name: "Мужской", Icon: "male", TextIconHtml: getTextIconHtml("male"), ru: "Мужской" },
			{ Id: "Female", Value: 1, Name: "Женский", Icon: "female", TextIconHtml: getTextIconHtml("female"), ru: "Женский" },
		];

		export var _maxLength = 5;

		export var _items = {
			"0": _array[0],
			"Male": _array[0],
			"1": _array[1],
			"Female": _array[1],
		};
			
	}

	//#endregion


	//#region PartyType 

	export enum PartyType
	{
		Department = 0,
		Organization = 1,
		Person = 2,
	}

	export module PartyType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "PartyType";
		export var _fullName = "Luxena.Travel.Domain.PartyType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.PartyType'" + value + "'");
		

		export var _array = [
			{ Id: "Department", Value: 0, Name: "Подразделение", TextIconHtml: "", ru: "Подразделение", rus: "Подразделения" },
			{ Id: "Organization", Value: 1, Name: "Организация", Icon: "group", TextIconHtml: getTextIconHtml("group"), ru: "Организация", rus: "Организации" },
			{ Id: "Person", Value: 2, Name: "Персона", Icon: "user", TextIconHtml: getTextIconHtml("user"), ru: "Персона", rus: "Персоны" },
		];

		export var _maxLength = 10;

		export var _items = {
			"0": _array[0],
			"Department": _array[0],
			"1": _array[1],
			"Organization": _array[1],
			"2": _array[2],
			"Person": _array[2],
		};
			
	}

	//#endregion


	//#region AmadeusRizUsingMode 

	export enum AmadeusRizUsingMode
	{
		None = 0,
		ServiceFeeOnly = 1,
		All = 2,
	}

	export module AmadeusRizUsingMode
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "AmadeusRizUsingMode";
		export var _fullName = "Luxena.Travel.Domain.AmadeusRizUsingMode";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.AmadeusRizUsingMode'" + value + "'");
		

		export var _array = [
			{ Id: "None", Value: 0, Name: "Не использовать", TextIconHtml: "", ru: "Не использовать" },
			{ Id: "ServiceFeeOnly", Value: 1, Name: "Использовать сервисный сбор", TextIconHtml: "", ru: "Использовать сервисный сбор" },
			{ Id: "All", Value: 2, Name: "Использовать полностью", TextIconHtml: "", ru: "Использовать полностью" },
		];

		export var _maxLength = 20;

		export var _items = {
			"0": _array[0],
			"None": _array[0],
			"1": _array[1],
			"ServiceFeeOnly": _array[1],
			"2": _array[2],
			"All": _array[2],
		};
			
	}

	//#endregion


	//#region AviaDocumentVatOptions 

	export enum AviaDocumentVatOptions
	{
		UseHFTax = 0,
		TaxAirlineTotal = 1,
	}

	export module AviaDocumentVatOptions
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "AviaDocumentVatOptions";
		export var _fullName = "Luxena.Travel.Domain.AviaDocumentVatOptions";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.AviaDocumentVatOptions'" + value + "'");
		

		export var _array = [
			{ Id: "UseHFTax", Value: 0, Name: "Использовать HF таксу", TextIconHtml: "", ru: "Использовать HF таксу" },
			{ Id: "TaxAirlineTotal", Value: 1, Name: "Рассчитывать от итога", TextIconHtml: "", ru: "Рассчитывать от итога" },
		];

		export var _maxLength = 16;

		export var _items = {
			"0": _array[0],
			"UseHFTax": _array[0],
			"1": _array[1],
			"TaxAirlineTotal": _array[1],
		};
			
	}

	//#endregion


	//#region ProductOrderItemGenerationOption 

	export enum ProductOrderItemGenerationOption
	{
		AlwaysOneOrderItem = 0,
		SeparateServiceFee = 1,
		ManualSetting = 2,
	}

	export module ProductOrderItemGenerationOption
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "ProductOrderItemGenerationOption";
		export var _fullName = "Luxena.Travel.Domain.ProductOrderItemGenerationOption";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ProductOrderItemGenerationOption'" + value + "'");
		

		export var _array = [
			{ Id: "AlwaysOneOrderItem", Value: 0, Name: "Всегда одна позиция", TextIconHtml: "", ru: "Всегда одна позиция" },
			{ Id: "SeparateServiceFee", Value: 1, Name: "Cервисный сбор отдельной позицией", TextIconHtml: "", ru: "Cервисный сбор отдельной позицией" },
			{ Id: "ManualSetting", Value: 2, Name: "Настраивать вручную", TextIconHtml: "", ru: "Настраивать вручную" },
		];

		export var _maxLength = 25;

		export var _items = {
			"0": _array[0],
			"AlwaysOneOrderItem": _array[0],
			"1": _array[1],
			"SeparateServiceFee": _array[1],
			"2": _array[2],
			"ManualSetting": _array[2],
		};
			
	}

	//#endregion


	//#region GdsFileType 

	export enum GdsFileType
	{
		AirFile = 0,
		MirFile = 1,
		TktFile = 2,
		PrintFile = 3,
		SirenaFile = 4,
		GalileoXmlFile = 5,
		AmadeusXmlFile = 6,
	}

	export module GdsFileType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "GdsFileType";
		export var _fullName = "Luxena.Travel.Domain.GdsFileType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.GdsFileType'" + value + "'");
		

		export var _array = [
			{ Id: "AirFile", Value: 0, Name: "AirFile", TextIconHtml: "",  },
			{ Id: "MirFile", Value: 1, Name: "MirFile", TextIconHtml: "",  },
			{ Id: "TktFile", Value: 2, Name: "TktFile", TextIconHtml: "",  },
			{ Id: "PrintFile", Value: 3, Name: "PrintFile", TextIconHtml: "",  },
			{ Id: "SirenaFile", Value: 4, Name: "SirenaFile", TextIconHtml: "",  },
			{ Id: "GalileoXmlFile", Value: 5, Name: "GalileoXmlFile", TextIconHtml: "",  },
			{ Id: "AmadeusXmlFile", Value: 6, Name: "AmadeusXmlFile", TextIconHtml: "",  },
		];

		export var _maxLength = 10;

		export var _items = {
			"0": _array[0],
			"AirFile": _array[0],
			"1": _array[1],
			"MirFile": _array[1],
			"2": _array[2],
			"TktFile": _array[2],
			"3": _array[3],
			"PrintFile": _array[3],
			"4": _array[4],
			"SirenaFile": _array[4],
			"5": _array[5],
			"GalileoXmlFile": _array[5],
			"6": _array[6],
			"AmadeusXmlFile": _array[6],
		};
			
	}

	//#endregion


	//#region GdsOriginator 

	export enum GdsOriginator
	{
		Unknown = 0,
		Amadeus = 1,
		Galileo = 2,
		Sirena = 3,
		Airline = 4,
		Gabriel = 5,
		WizzAir = 6,
		IATI = 7,
		ETravels = 8,
		TicketConsolidator = 9,
		DeltaTravel = 10,
		TicketsUA = 11,
		FlyDubai = 12,
		AirArabia = 13,
		Pegasus = 14,
	}

	export module GdsOriginator
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "GdsOriginator";
		export var _fullName = "Luxena.Travel.Domain.GdsOriginator";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.GdsOriginator'" + value + "'");
		

		export var _array = [
			{ Id: "Unknown", Value: 0, Name: "Unknown", TextIconHtml: "",  },
			{ Id: "Amadeus", Value: 1, Name: "Amadeus", TextIconHtml: "",  },
			{ Id: "Galileo", Value: 2, Name: "Galileo", TextIconHtml: "",  },
			{ Id: "Sirena", Value: 3, Name: "Sirena", TextIconHtml: "",  },
			{ Id: "Airline", Value: 4, Name: "Airline", TextIconHtml: "",  },
			{ Id: "Gabriel", Value: 5, Name: "Gabriel", TextIconHtml: "",  },
			{ Id: "WizzAir", Value: 6, Name: "Wizz Air", TextIconHtml: "", ru: "Wizz Air" },
			{ Id: "IATI", Value: 7, Name: "IATI", TextIconHtml: "",  },
			{ Id: "ETravels", Value: 8, Name: "E-Travels", TextIconHtml: "", ru: "E-Travels" },
			{ Id: "TicketConsolidator", Value: 9, Name: "Ticket Consolidator", TextIconHtml: "", ru: "Ticket Consolidator" },
			{ Id: "DeltaTravel", Value: 10, Name: "Delta TRAVEL", TextIconHtml: "", ru: "Delta TRAVEL" },
			{ Id: "TicketsUA", Value: 11, Name: "Tickets.UA", TextIconHtml: "", ru: "Tickets.UA" },
			{ Id: "FlyDubai", Value: 12, Name: "Fly Dubai", TextIconHtml: "", ru: "Fly Dubai" },
			{ Id: "AirArabia", Value: 13, Name: "Air Arabia", TextIconHtml: "", ru: "Air Arabia" },
			{ Id: "Pegasus", Value: 14, Name: "Pegasus", TextIconHtml: "",  },
		];

		export var _maxLength = 14;

		export var _items = {
			"0": _array[0],
			"Unknown": _array[0],
			"1": _array[1],
			"Amadeus": _array[1],
			"2": _array[2],
			"Galileo": _array[2],
			"3": _array[3],
			"Sirena": _array[3],
			"4": _array[4],
			"Airline": _array[4],
			"5": _array[5],
			"Gabriel": _array[5],
			"6": _array[6],
			"WizzAir": _array[6],
			"7": _array[7],
			"IATI": _array[7],
			"8": _array[8],
			"ETravels": _array[8],
			"9": _array[9],
			"TicketConsolidator": _array[9],
			"10": _array[10],
			"DeltaTravel": _array[10],
			"11": _array[11],
			"TicketsUA": _array[11],
			"12": _array[12],
			"FlyDubai": _array[12],
			"13": _array[13],
			"AirArabia": _array[13],
			"14": _array[14],
			"Pegasus": _array[14],
		};
			
	}

	//#endregion


	//#region FlightSegmentType 

	export enum FlightSegmentType
	{
		Ticketed = 0,
		Unticketed = 1,
		Voided = 2,
	}

	export module FlightSegmentType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "FlightSegmentType";
		export var _fullName = "Luxena.Travel.Domain.FlightSegmentType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.FlightSegmentType'" + value + "'");
		

		export var _array = [
			{ Id: "Ticketed", Value: 0, Name: "Ticketed", TextIconHtml: "",  },
			{ Id: "Unticketed", Value: 1, Name: "Unticketed", TextIconHtml: "",  },
			{ Id: "Voided", Value: 2, Name: "Voided", TextIconHtml: "",  },
		];

		export var _maxLength = 8;

		export var _items = {
			"0": _array[0],
			"Ticketed": _array[0],
			"1": _array[1],
			"Unticketed": _array[1],
			"2": _array[2],
			"Voided": _array[2],
		};
			
	}

	//#endregion


	//#region ImportResult 

	export enum ImportResult
	{
		None = 0,
		Success = 1,
		Error = 2,
		Warn = 3,
	}

	export module ImportResult
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "ImportResult";
		export var _fullName = "Luxena.Travel.Domain.ImportResult";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ImportResult'" + value + "'");
		

		export var _array = [
			{ Id: "None", Value: 0, Name: "None", TextIconHtml: "",  },
			{ Id: "Success", Value: 1, Name: "Success", TextIconHtml: "",  },
			{ Id: "Error", Value: 2, Name: "Error", TextIconHtml: "",  },
			{ Id: "Warn", Value: 3, Name: "Warn", TextIconHtml: "",  },
		];

		export var _maxLength = 5;

		export var _items = {
			"0": _array[0],
			"None": _array[0],
			"1": _array[1],
			"Success": _array[1],
			"2": _array[2],
			"Error": _array[2],
			"3": _array[3],
			"Warn": _array[3],
		};
			
	}

	//#endregion


	//#region IsicCardType 

	export enum IsicCardType
	{
		Unknown = 0,
		Isic = 1,
		ITIC = 2,
		IYTC = 3,
	}

	export module IsicCardType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "IsicCardType";
		export var _fullName = "Luxena.Travel.Domain.IsicCardType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.IsicCardType'" + value + "'");
		

		export var _array = [
			{ Id: "Unknown", Value: 0, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
			{ Id: "Isic", Value: 1, Name: "Isic", TextIconHtml: "",  },
			{ Id: "ITIC", Value: 2, Name: "ITIC", TextIconHtml: "",  },
			{ Id: "IYTC", Value: 3, Name: "IYTC", TextIconHtml: "",  },
		];

		export var _maxLength = 8;

		export var _items = {
			"0": _array[0],
			"Unknown": _array[0],
			"1": _array[1],
			"Isic": _array[1],
			"2": _array[2],
			"ITIC": _array[2],
			"3": _array[3],
			"IYTC": _array[3],
		};
			
	}

	//#endregion


	//#region MealType 

	export enum MealType
	{
		NoData = 0,
		Breakfast = 1,
		ContinentalBreakfast = 2,
		Lunch = 4,
		Dinner = 8,
		Snack = 16,
		ColdMeal = 32,
		HotMeal = 64,
		Meal = 128,
		Refreshment = 256,
		AlcoholicComplimentaryBeverages = 512,
		Food = 1024,
		AlcoholicBeveragesForPurchase = 2048,
		DutyFree = 4096,
	}

	export module MealType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "MealType";
		export var _fullName = "Luxena.Travel.Domain.MealType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.MealType'" + value + "'");
		export var _isFlags = true;

		export var _array = [
			{ Id: "NoData", Value: 0, Name: "нет", TextIconHtml: "", ru: "нет" },
			{ Id: "Breakfast", Value: 1, Name: "Завтрак", TextIconHtml: "", ru: "Завтрак" },
			{ Id: "ContinentalBreakfast", Value: 2, Name: "Континентальный завтрак", TextIconHtml: "", ru: "Континентальный завтрак" },
			{ Id: "Lunch", Value: 4, Name: "Ланч", TextIconHtml: "", ru: "Ланч" },
			{ Id: "Dinner", Value: 8, Name: "Обед", TextIconHtml: "", ru: "Обед" },
			{ Id: "Snack", Value: 16, Name: "Закуска", TextIconHtml: "", ru: "Закуска" },
			{ Id: "ColdMeal", Value: 32, Name: "Холодная еда", TextIconHtml: "", ru: "Холодная еда" },
			{ Id: "HotMeal", Value: 64, Name: "Горячая еда", TextIconHtml: "", ru: "Горячая еда" },
			{ Id: "Meal", Value: 128, Name: "Еда", TextIconHtml: "", ru: "Еда" },
			{ Id: "Refreshment", Value: 256, Name: "Напитки", TextIconHtml: "", ru: "Напитки" },
			{ Id: "AlcoholicComplimentaryBeverages", Value: 512, Name: "Бесплатные алкогольные напитки", TextIconHtml: "", ru: "Бесплатные алкогольные напитки" },
			{ Id: "Food", Value: 1024, Name: "Еда", TextIconHtml: "", ru: "Еда" },
			{ Id: "AlcoholicBeveragesForPurchase", Value: 2048, Name: "Платные алкогольные напитки", TextIconHtml: "", ru: "Платные алкогольные напитки" },
			{ Id: "DutyFree", Value: 4096, Name: "DutyFree", TextIconHtml: "", ru: "DutyFree" },
		];

		export var _maxLength = 22;

		export var _items = {
			"0": _array[0],
			"NoData": _array[0],
			"1": _array[1],
			"Breakfast": _array[1],
			"2": _array[2],
			"ContinentalBreakfast": _array[2],
			"4": _array[3],
			"Lunch": _array[3],
			"8": _array[4],
			"Dinner": _array[4],
			"16": _array[5],
			"Snack": _array[5],
			"32": _array[6],
			"ColdMeal": _array[6],
			"64": _array[7],
			"HotMeal": _array[7],
			"128": _array[8],
			"Meal": _array[8],
			"256": _array[9],
			"Refreshment": _array[9],
			"512": _array[10],
			"AlcoholicComplimentaryBeverages": _array[10],
			"1024": _array[11],
			"Food": _array[11],
			"2048": _array[12],
			"AlcoholicBeveragesForPurchase": _array[12],
			"4096": _array[13],
			"DutyFree": _array[13],
		};
			
	}

	//#endregion


	//#region PasteboardServiceClass 

	export enum PasteboardServiceClass
	{
		FirstClass = 0,
		SecondClass = 1,
		LuxuryCoupe = 2,
		ReservedSeat = 3,
		Compartment = 4,
		Unknown = 5,
	}

	export module PasteboardServiceClass
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "PasteboardServiceClass";
		export var _fullName = "Luxena.Travel.Domain.PasteboardServiceClass";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.PasteboardServiceClass'" + value + "'");
		

		export var _array = [
			{ Id: "FirstClass", Value: 0, Name: "1-й класс", TextIconHtml: "", ru: "1-й класс" },
			{ Id: "SecondClass", Value: 1, Name: "2-й класс", TextIconHtml: "", ru: "2-й класс" },
			{ Id: "LuxuryCoupe", Value: 2, Name: "люкс", TextIconHtml: "", ru: "люкс" },
			{ Id: "ReservedSeat", Value: 3, Name: "плацкарт", TextIconHtml: "", ru: "плацкарт" },
			{ Id: "Compartment", Value: 4, Name: "купе", TextIconHtml: "", ru: "купе" },
			{ Id: "Unknown", Value: 5, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
		];

		export var _maxLength = 8;

		export var _items = {
			"0": _array[0],
			"FirstClass": _array[0],
			"1": _array[1],
			"SecondClass": _array[1],
			"2": _array[2],
			"LuxuryCoupe": _array[2],
			"3": _array[3],
			"ReservedSeat": _array[3],
			"4": _array[4],
			"Compartment": _array[4],
			"5": _array[5],
			"Unknown": _array[5],
		};
			
	}

	//#endregion


	//#region PaymentType 

	export enum PaymentType
	{
		Unknown = 0,
		Cash = 1,
		Invoice = 2,
		Check = 3,
		CreditCard = 4,
		Exchange = 5,
		WithoutPayment = 6,
	}

	export module PaymentType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "PaymentType";
		export var _fullName = "Luxena.Travel.Domain.PaymentType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.PaymentType'" + value + "'");
		

		export var _array = [
			{ Id: "Unknown", Value: 0, Name: "Unknown", TextIconHtml: "",  },
			{ Id: "Cash", Value: 1, Name: "Cash", TextIconHtml: "",  },
			{ Id: "Invoice", Value: 2, Name: "Invoice", TextIconHtml: "",  },
			{ Id: "Check", Value: 3, Name: "Check", TextIconHtml: "",  },
			{ Id: "CreditCard", Value: 4, Name: "CreditCard", TextIconHtml: "",  },
			{ Id: "Exchange", Value: 5, Name: "Exchange", TextIconHtml: "",  },
			{ Id: "WithoutPayment", Value: 6, Name: "Без оплаты", TextIconHtml: "", ru: "Без оплаты" },
		];

		export var _maxLength = 8;

		export var _items = {
			"0": _array[0],
			"Unknown": _array[0],
			"1": _array[1],
			"Cash": _array[1],
			"2": _array[2],
			"Invoice": _array[2],
			"3": _array[3],
			"Check": _array[3],
			"4": _array[4],
			"CreditCard": _array[4],
			"5": _array[5],
			"Exchange": _array[5],
			"6": _array[6],
			"WithoutPayment": _array[6],
		};
			
	}

	//#endregion


	//#region ProductOrigin 

	export enum ProductOrigin
	{
		AmadeusAir = 0,
		AmadeusPrint = 1,
		GalileoMir = 2,
		GalileoTkt = 3,
		BspLink = 4,
		Manual = 5,
		SirenaXml = 6,
		GalileoXml = 7,
		AmadeusXml = 8,
	}

	export module ProductOrigin
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "ProductOrigin";
		export var _fullName = "Luxena.Travel.Domain.ProductOrigin";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ProductOrigin'" + value + "'");
		

		export var _array = [
			{ Id: "AmadeusAir", Value: 0, Name: "AmadeusAir", TextIconHtml: "",  },
			{ Id: "AmadeusPrint", Value: 1, Name: "AmadeusPrint", TextIconHtml: "",  },
			{ Id: "GalileoMir", Value: 2, Name: "GalileoMir", TextIconHtml: "",  },
			{ Id: "GalileoTkt", Value: 3, Name: "GalileoTkt", TextIconHtml: "",  },
			{ Id: "BspLink", Value: 4, Name: "BspLink", TextIconHtml: "",  },
			{ Id: "Manual", Value: 5, Name: "Manual", TextIconHtml: "",  },
			{ Id: "SirenaXml", Value: 6, Name: "SirenaXml", TextIconHtml: "",  },
			{ Id: "GalileoXml", Value: 7, Name: "GalileoXml", TextIconHtml: "",  },
			{ Id: "AmadeusXml", Value: 8, Name: "AmadeusXml", TextIconHtml: "",  },
		];

		export var _maxLength = 9;

		export var _items = {
			"0": _array[0],
			"AmadeusAir": _array[0],
			"1": _array[1],
			"AmadeusPrint": _array[1],
			"2": _array[2],
			"GalileoMir": _array[2],
			"3": _array[3],
			"GalileoTkt": _array[3],
			"4": _array[4],
			"BspLink": _array[4],
			"5": _array[5],
			"Manual": _array[5],
			"6": _array[6],
			"SirenaXml": _array[6],
			"7": _array[7],
			"GalileoXml": _array[7],
			"8": _array[8],
			"AmadeusXml": _array[8],
		};
			
	}

	//#endregion


	//#region ProductType 

	export enum ProductType
	{
		AviaTicket = 0,
		AviaRefund = 1,
		AviaMco = 2,
		Pasteboard = 3,
		SimCard = 4,
		Isic = 5,
		Excursion = 6,
		Tour = 7,
		Accommodation = 8,
		Transfer = 9,
		Insurance = 10,
		CarRental = 11,
		GenericProduct = 12,
		BusTicket = 13,
		PasteboardRefund = 14,
		InsuranceRefund = 15,
		BusTicketRefund = 16,
	}

	export module ProductType
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "ProductType";
		export var _fullName = "Luxena.Travel.Domain.ProductType";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ProductType'" + value + "'");
		

		export var _array = [
			{ Id: "AviaTicket", Value: 0, Name: "Авиабилет", Icon: "plane", TextIconHtml: getTextIconHtml("plane"), ru: "Авиабилет", rus: "Авиабилеты", ua: "Авіаквиток" },
			{ Id: "AviaRefund", Value: 1, Name: "Возврат авиабилета", Icon: "plane", TextIconHtml: getTextIconHtml("plane"), ru: "Возврат авиабилета", rus: "Возвраты авиабилетов", ua: "Повернення авіаквитка" },
			{ Id: "AviaMco", Value: 2, Name: "МСО", Icon: "plane", TextIconHtml: getTextIconHtml("plane"), ru: "МСО", rus: "МСО", ua: "MCO" },
			{ Id: "Pasteboard", Value: 3, Name: "Ж/д билет", Icon: "subway", TextIconHtml: getTextIconHtml("subway"), ru: "Ж/д билет", rus: "Ж/д билеты", ua: "Залізничний квиток" },
			{ Id: "SimCard", Value: 4, Name: "SIM-карта", Icon: "mobile", TextIconHtml: getTextIconHtml("mobile"), ru: "SIM-карта", rus: "SIM-карты", ua: "SIM-картка" },
			{ Id: "Isic", Value: 5, Name: "Студенческий билет", Icon: "graduation-cap", TextIconHtml: getTextIconHtml("graduation-cap"), ru: "Студенческий билет", rus: "Студенческие билеты", ua: "Студентський квиток" },
			{ Id: "Excursion", Value: 6, Name: "Экскурсия", Icon: "photo", TextIconHtml: getTextIconHtml("photo"), ru: "Экскурсия", rus: "Экскурсии", ua: "Екскурсія" },
			{ Id: "Tour", Value: 7, Name: "Турпакет", Icon: "suitcase", TextIconHtml: getTextIconHtml("suitcase"), ru: "Турпакет", rus: "Турпакеты", ua: "Турпакет" },
			{ Id: "Accommodation", Value: 8, Name: "Проживание", Icon: "bed", TextIconHtml: getTextIconHtml("bed"), ru: "Проживание", rus: "Проживания", ua: "Готель" },
			{ Id: "Transfer", Value: 9, Name: "Трансфер", Icon: "cab", TextIconHtml: getTextIconHtml("cab"), ru: "Трансфер", rus: "Трансферы", ua: "Трансфер" },
			{ Id: "Insurance", Value: 10, Name: "Страховка", Icon: "fire-extinguisher", TextIconHtml: getTextIconHtml("fire-extinguisher"), ru: "Страховка", rus: "Страховки", ua: "Страховка" },
			{ Id: "CarRental", Value: 11, Name: "Аренда автомобиля", Icon: "car", TextIconHtml: getTextIconHtml("car"), ru: "Аренда автомобиля", rus: "Аренды автомобилей", ua: "Оренда автомобіля" },
			{ Id: "GenericProduct", Value: 12, Name: "Дополнительная услуга", Icon: "suitcase", TextIconHtml: getTextIconHtml("suitcase"), ru: "Дополнительная услуга", rus: "Дополнительные услуги", ua: "Додаткова послуга" },
			{ Id: "BusTicket", Value: 13, Name: "Автобусный билет", Icon: "bus", TextIconHtml: getTextIconHtml("bus"), ru: "Автобусный билет", rus: "Автобусные билеты", ua: "Автобусний квиток" },
			{ Id: "PasteboardRefund", Value: 14, Name: "Возврат ж/д билета", Icon: "subway", TextIconHtml: getTextIconHtml("subway"), ru: "Возврат ж/д билета", rus: "Возвраты ж/д билетов", ua: "Повернення залізничного квитка" },
			{ Id: "InsuranceRefund", Value: 15, Name: "Возврат страховки", Icon: "fire-extinguisher", TextIconHtml: getTextIconHtml("fire-extinguisher"), ru: "Возврат страховки", rus: "Возвраты страховок", ua: "Повернення страховки" },
			{ Id: "BusTicketRefund", Value: 16, Name: "Возврат автобусного билета", Icon: "bus", TextIconHtml: getTextIconHtml("bus"), ru: "Возврат автобусного билета", rus: "Возвраты автобусных билетов", ua: "Повернення автобусного квитка" },
		];

		export var _maxLength = 20;

		export var _items = {
			"0": _array[0],
			"AviaTicket": _array[0],
			"1": _array[1],
			"AviaRefund": _array[1],
			"2": _array[2],
			"AviaMco": _array[2],
			"3": _array[3],
			"Pasteboard": _array[3],
			"4": _array[4],
			"SimCard": _array[4],
			"5": _array[5],
			"Isic": _array[5],
			"6": _array[6],
			"Excursion": _array[6],
			"7": _array[7],
			"Tour": _array[7],
			"8": _array[8],
			"Accommodation": _array[8],
			"9": _array[9],
			"Transfer": _array[9],
			"10": _array[10],
			"Insurance": _array[10],
			"11": _array[11],
			"CarRental": _array[11],
			"12": _array[12],
			"GenericProduct": _array[12],
			"13": _array[13],
			"BusTicket": _array[13],
			"14": _array[14],
			"PasteboardRefund": _array[14],
			"15": _array[15],
			"InsuranceRefund": _array[15],
			"16": _array[16],
			"BusTicketRefund": _array[16],
		};
			
	}

	//#endregion


	//#region ServiceClass 

	export enum ServiceClass
	{
		Unknown = 0,
		Economy = 1,
		PremiumEconomy = 2,
		Business = 3,
		First = 4,
	}

	export module ServiceClass
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "ServiceClass";
		export var _fullName = "Luxena.Travel.Domain.ServiceClass";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.ServiceClass'" + value + "'");
		

		export var _array = [
			{ Id: "Unknown", Value: 0, Name: "Неизвестно", TextIconHtml: "", ru: "Неизвестно" },
			{ Id: "Economy", Value: 1, Name: "Эконом", TextIconHtml: "", ru: "Эконом" },
			{ Id: "PremiumEconomy", Value: 2, Name: "Эконом-премиум", TextIconHtml: "", ru: "Эконом-премиум" },
			{ Id: "Business", Value: 3, Name: "Бизнес", TextIconHtml: "", ru: "Бизнес" },
			{ Id: "First", Value: 4, Name: "Первый", TextIconHtml: "", ru: "Первый" },
		];

		export var _maxLength = 10;

		export var _items = {
			"0": _array[0],
			"Unknown": _array[0],
			"1": _array[1],
			"Economy": _array[1],
			"2": _array[2],
			"PremiumEconomy": _array[2],
			"3": _array[3],
			"Business": _array[3],
			"4": _array[4],
			"First": _array[4],
		};
			
	}

	//#endregion


	//#region UserRole 

	export enum UserRole
	{
		None = 0,
		Everyone = 1,
		Administrator = 2,
		Supervisor = 4,
		Agent = 8,
		Cashier = 16,
		Analyst = 32,
		SubAgent = 64,
	}

	export module UserRole
	{
		export var _ns = "Luxena.Travel.Domain";
		export var _name = "UserRole";
		export var _fullName = "Luxena.Travel.Domain.UserRole";
		export var _getEdm = value => !value ? null : new DevExpress.data.EdmLiteral("Luxena.Travel.Domain.UserRole'" + value + "'");
		export var _isFlags = true;

		export var _array = [
			{ Id: "None", Value: 0, Name: "None", TextIconHtml: "",  },
			{ Id: "Everyone", Value: 1, Name: "Все", TextIconHtml: "", ru: "Все" },
			{ Id: "Administrator", Value: 2, Name: "Администратор", TextIconHtml: "", ru: "Администратор" },
			{ Id: "Supervisor", Value: 4, Name: "Супервизор", TextIconHtml: "", ru: "Супервизор" },
			{ Id: "Agent", Value: 8, Name: "Агент", TextIconHtml: "", ru: "Агент" },
			{ Id: "Cashier", Value: 16, Name: "Кассир", TextIconHtml: "", ru: "Кассир" },
			{ Id: "Analyst", Value: 32, Name: "Аналитик", TextIconHtml: "", ru: "Аналитик" },
			{ Id: "SubAgent", Value: 64, Name: "Субагент", TextIconHtml: "", ru: "Субагент" },
		];

		export var _maxLength = 10;

		export var _items = {
			"0": _array[0],
			"None": _array[0],
			"1": _array[1],
			"Everyone": _array[1],
			"2": _array[2],
			"Administrator": _array[2],
			"4": _array[3],
			"Supervisor": _array[3],
			"8": _array[4],
			"Agent": _array[4],
			"16": _array[5],
			"Cashier": _array[5],
			"32": _array[6],
			"Analyst": _array[6],
			"64": _array[7],
			"SubAgent": _array[7],
		};
			
	}

	//#endregion


	//#endregion


	export class Domain extends DevExpress.data.ODataContext
	{
	
		Accommodations: DevExpress.data.ODataStore;
	
		AccommodationLookup: DevExpress.data.ODataStore;
	
		AccommodationProviders: DevExpress.data.ODataStore;
	
		AccommodationProviderLookup: DevExpress.data.ODataStore;
	
		AccommodationTypes: DevExpress.data.ODataStore;
	
		AccommodationTypeLookup: DevExpress.data.ODataStore;
	
		ActiveOwners: DevExpress.data.ODataStore;
	
		ActiveOwnerLookup: DevExpress.data.ODataStore;
	
		Agents: DevExpress.data.ODataStore;
	
		AgentLookup: DevExpress.data.ODataStore;
	
		Airlines: DevExpress.data.ODataStore;
	
		AirlineLookup: DevExpress.data.ODataStore;
	
		AirlineServiceClasses: DevExpress.data.ODataStore;
	
		AirlineServiceClassLookup: DevExpress.data.ODataStore;
	
		Airports: DevExpress.data.ODataStore;
	
		AirportLookup: DevExpress.data.ODataStore;
	
		AviaDocuments: DevExpress.data.ODataStore;
	
		AviaDocumentLookup: DevExpress.data.ODataStore;
	
		AviaMcos: DevExpress.data.ODataStore;
	
		AviaMcoLookup: DevExpress.data.ODataStore;
	
		AviaRefunds: DevExpress.data.ODataStore;
	
		AviaRefundLookup: DevExpress.data.ODataStore;
	
		AviaTickets: DevExpress.data.ODataStore;
	
		AviaTicketLookup: DevExpress.data.ODataStore;
	
		BankAccounts: DevExpress.data.ODataStore;
	
		BankAccountLookup: DevExpress.data.ODataStore;
	
		BusDocuments: DevExpress.data.ODataStore;
	
		BusDocumentLookup: DevExpress.data.ODataStore;
	
		BusTickets: DevExpress.data.ODataStore;
	
		BusTicketLookup: DevExpress.data.ODataStore;
	
		BusTicketProviders: DevExpress.data.ODataStore;
	
		BusTicketProviderLookup: DevExpress.data.ODataStore;
	
		BusTicketRefunds: DevExpress.data.ODataStore;
	
		BusTicketRefundLookup: DevExpress.data.ODataStore;
	
		CarRentals: DevExpress.data.ODataStore;
	
		CarRentalLookup: DevExpress.data.ODataStore;
	
		CarRentalProviders: DevExpress.data.ODataStore;
	
		CarRentalProviderLookup: DevExpress.data.ODataStore;
	
		CashInOrderPayments: DevExpress.data.ODataStore;
	
		CashInOrderPaymentLookup: DevExpress.data.ODataStore;
	
		CashOutOrderPayments: DevExpress.data.ODataStore;
	
		CashOutOrderPaymentLookup: DevExpress.data.ODataStore;
	
		CateringTypes: DevExpress.data.ODataStore;
	
		CateringTypeLookup: DevExpress.data.ODataStore;
	
		CheckPayments: DevExpress.data.ODataStore;
	
		CheckPaymentLookup: DevExpress.data.ODataStore;
	
		Consignments: DevExpress.data.ODataStore;
	
		ConsignmentLookup: DevExpress.data.ODataStore;
	
		Countries: DevExpress.data.ODataStore;
	
		CountryLookup: DevExpress.data.ODataStore;
	
		CurrencyDailyRates: DevExpress.data.ODataStore;
	
		Customers: DevExpress.data.ODataStore;
	
		CustomerLookup: DevExpress.data.ODataStore;
	
		Departments: DevExpress.data.ODataStore;
	
		DepartmentLookup: DevExpress.data.ODataStore;
	
		DocumentAccesses: DevExpress.data.ODataStore;
	
		DocumentOwners: DevExpress.data.ODataStore;
	
		ElectronicPayments: DevExpress.data.ODataStore;
	
		ElectronicPaymentLookup: DevExpress.data.ODataStore;
	
		Employees: DevExpress.data.ODataStore;
	
		EmployeeLookup: DevExpress.data.ODataStore;
	
		EverydayProfitReports: DevExpress.data.ODataStore;
	
		Excursions: DevExpress.data.ODataStore;
	
		ExcursionLookup: DevExpress.data.ODataStore;
	
		Files: DevExpress.data.ODataStore;
	
		FlightSegments: DevExpress.data.ODataStore;
	
		FlightSegmentLookup: DevExpress.data.ODataStore;
	
		FlownReports: DevExpress.data.ODataStore;
	
		GdsAgents: DevExpress.data.ODataStore;
	
		GdsAgentLookup: DevExpress.data.ODataStore;
	
		GdsAgent_ApplyToUnassigned: DevExpress.data.ODataStore;
	
		GdsFiles: DevExpress.data.ODataStore;
	
		GdsFileLookup: DevExpress.data.ODataStore;
	
		GenericProducts: DevExpress.data.ODataStore;
	
		GenericProductLookup: DevExpress.data.ODataStore;
	
		GenericProductProviders: DevExpress.data.ODataStore;
	
		GenericProductProviderLookup: DevExpress.data.ODataStore;
	
		GenericProductTypes: DevExpress.data.ODataStore;
	
		GenericProductTypeLookup: DevExpress.data.ODataStore;
	
		Identities: DevExpress.data.ODataStore;
	
		IdentityLookup: DevExpress.data.ODataStore;
	
		Insurances: DevExpress.data.ODataStore;
	
		InsuranceLookup: DevExpress.data.ODataStore;
	
		InsuranceCompanies: DevExpress.data.ODataStore;
	
		InsuranceCompanyLookup: DevExpress.data.ODataStore;
	
		InsuranceDocuments: DevExpress.data.ODataStore;
	
		InsuranceDocumentLookup: DevExpress.data.ODataStore;
	
		InsuranceRefunds: DevExpress.data.ODataStore;
	
		InsuranceRefundLookup: DevExpress.data.ODataStore;
	
		InternalIdentities: DevExpress.data.ODataStore;
	
		InternalIdentityLookup: DevExpress.data.ODataStore;
	
		InternalTransfers: DevExpress.data.ODataStore;
	
		InternalTransferLookup: DevExpress.data.ODataStore;
	
		Invoices: DevExpress.data.ODataStore;
	
		InvoiceLookup: DevExpress.data.ODataStore;
	
		Isics: DevExpress.data.ODataStore;
	
		IsicLookup: DevExpress.data.ODataStore;
	
		IssuedConsignments: DevExpress.data.ODataStore;
	
		IssuedConsignmentLookup: DevExpress.data.ODataStore;
	
		MilesCards: DevExpress.data.ODataStore;
	
		MilesCardLookup: DevExpress.data.ODataStore;
	
		OpeningBalances: DevExpress.data.ODataStore;
	
		OpeningBalanceLookup: DevExpress.data.ODataStore;
	
		Orders: DevExpress.data.ODataStore;
	
		OrderLookup: DevExpress.data.ODataStore;
	
		OrderBalances: DevExpress.data.ODataStore;
	
		OrderByAssignedTo_TotalByIssueDate: DevExpress.data.ODataStore;
	
		OrderByCustomer_TotalByIssueDate: DevExpress.data.ODataStore;
	
		OrderByOwner_TotalByIssueDate: DevExpress.data.ODataStore;
	
		OrderChecks: DevExpress.data.ODataStore;
	
		OrderCheckLookup: DevExpress.data.ODataStore;
	
		OrderItems: DevExpress.data.ODataStore;
	
		OrderItemLookup: DevExpress.data.ODataStore;
	
		Organizations: DevExpress.data.ODataStore;
	
		OrganizationLookup: DevExpress.data.ODataStore;
	
		Parties: DevExpress.data.ODataStore;
	
		PartyLookup: DevExpress.data.ODataStore;
	
		Passports: DevExpress.data.ODataStore;
	
		PassportLookup: DevExpress.data.ODataStore;
	
		Pasteboards: DevExpress.data.ODataStore;
	
		PasteboardLookup: DevExpress.data.ODataStore;
	
		PasteboardProviders: DevExpress.data.ODataStore;
	
		PasteboardProviderLookup: DevExpress.data.ODataStore;
	
		PasteboardRefunds: DevExpress.data.ODataStore;
	
		PasteboardRefundLookup: DevExpress.data.ODataStore;
	
		Payments: DevExpress.data.ODataStore;
	
		PaymentLookup: DevExpress.data.ODataStore;
	
		PaymentSystems: DevExpress.data.ODataStore;
	
		PaymentSystemLookup: DevExpress.data.ODataStore;
	
		Persons: DevExpress.data.ODataStore;
	
		PersonLookup: DevExpress.data.ODataStore;
	
		Products: DevExpress.data.ODataStore;
	
		ProductLookup: DevExpress.data.ODataStore;
	
		ProductByBooker_TotalByIssueDate: DevExpress.data.ODataStore;
	
		ProductByProvider_TotalByIssueDate: DevExpress.data.ODataStore;
	
		ProductBySeller_TotalByIssueDate: DevExpress.data.ODataStore;
	
		ProductByTicketer_TotalByIssueDate: DevExpress.data.ODataStore;
	
		ProductPassengers: DevExpress.data.ODataStore;
	
		ProductSummaries: DevExpress.data.ODataStore;
	
		ProductTotalByBookers: DevExpress.data.ODataStore;
	
		ProductTotalByDays: DevExpress.data.ODataStore;
	
		ProductTotalByMonths: DevExpress.data.ODataStore;
	
		ProductTotalByOwners: DevExpress.data.ODataStore;
	
		ProductTotalByProviders: DevExpress.data.ODataStore;
	
		ProductTotalByQuarters: DevExpress.data.ODataStore;
	
		ProductTotalBySellers: DevExpress.data.ODataStore;
	
		ProductTotalByTypes: DevExpress.data.ODataStore;
	
		ProductTotalByYears: DevExpress.data.ODataStore;
	
		ProfitDistributionByCustomers: DevExpress.data.ODataStore;
	
		ProfitDistributionByProviders: DevExpress.data.ODataStore;
	
		RailwayDocuments: DevExpress.data.ODataStore;
	
		RailwayDocumentLookup: DevExpress.data.ODataStore;
	
		Receipts: DevExpress.data.ODataStore;
	
		ReceiptLookup: DevExpress.data.ODataStore;
	
		RoamingOperators: DevExpress.data.ODataStore;
	
		RoamingOperatorLookup: DevExpress.data.ODataStore;
	
		Sequences: DevExpress.data.ODataStore;
	
		SequenceLookup: DevExpress.data.ODataStore;
	
		SimCards: DevExpress.data.ODataStore;
	
		SimCardLookup: DevExpress.data.ODataStore;
	
		SystemConfigurations: DevExpress.data.ODataStore;
	
		Tours: DevExpress.data.ODataStore;
	
		TourLookup: DevExpress.data.ODataStore;
	
		TourProviders: DevExpress.data.ODataStore;
	
		TourProviderLookup: DevExpress.data.ODataStore;
	
		Transfers: DevExpress.data.ODataStore;
	
		TransferLookup: DevExpress.data.ODataStore;
	
		TransferProviders: DevExpress.data.ODataStore;
	
		TransferProviderLookup: DevExpress.data.ODataStore;
	
		Users: DevExpress.data.ODataStore;
	
		UserLookup: DevExpress.data.ODataStore;
	
		WireTransfers: DevExpress.data.ODataStore;
	
		WireTransferLookup: DevExpress.data.ODataStore;
	};


	config.services.db.entities = 
	{
	
		Accommodations: { key: "Id", keyType: "String" },
	
		AccommodationLookup: { key: "Id", keyType: "String" },
	
		AccommodationProviders: { key: "Id", keyType: "String" },
	
		AccommodationProviderLookup: { key: "Id", keyType: "String" },
	
		AccommodationTypes: { key: "Id", keyType: "String" },
	
		AccommodationTypeLookup: { key: "Id", keyType: "String" },
	
		ActiveOwners: { key: "Id", keyType: "String" },
	
		ActiveOwnerLookup: { key: "Id", keyType: "String" },
	
		Agents: { key: "Id", keyType: "String" },
	
		AgentLookup: { key: "Id", keyType: "String" },
	
		Airlines: { key: "Id", keyType: "String" },
	
		AirlineLookup: { key: "Id", keyType: "String" },
	
		AirlineServiceClasses: { key: "Id", keyType: "String" },
	
		AirlineServiceClassLookup: { key: "Id", keyType: "String" },
	
		Airports: { key: "Id", keyType: "String" },
	
		AirportLookup: { key: "Id", keyType: "String" },
	
		AviaDocuments: { key: "Id", keyType: "String" },
	
		AviaDocumentLookup: { key: "Id", keyType: "String" },
	
		AviaMcos: { key: "Id", keyType: "String" },
	
		AviaMcoLookup: { key: "Id", keyType: "String" },
	
		AviaRefunds: { key: "Id", keyType: "String" },
	
		AviaRefundLookup: { key: "Id", keyType: "String" },
	
		AviaTickets: { key: "Id", keyType: "String" },
	
		AviaTicketLookup: { key: "Id", keyType: "String" },
	
		BankAccounts: { key: "Id", keyType: "String" },
	
		BankAccountLookup: { key: "Id", keyType: "String" },
	
		BusDocuments: { key: "Id", keyType: "String" },
	
		BusDocumentLookup: { key: "Id", keyType: "String" },
	
		BusTickets: { key: "Id", keyType: "String" },
	
		BusTicketLookup: { key: "Id", keyType: "String" },
	
		BusTicketProviders: { key: "Id", keyType: "String" },
	
		BusTicketProviderLookup: { key: "Id", keyType: "String" },
	
		BusTicketRefunds: { key: "Id", keyType: "String" },
	
		BusTicketRefundLookup: { key: "Id", keyType: "String" },
	
		CarRentals: { key: "Id", keyType: "String" },
	
		CarRentalLookup: { key: "Id", keyType: "String" },
	
		CarRentalProviders: { key: "Id", keyType: "String" },
	
		CarRentalProviderLookup: { key: "Id", keyType: "String" },
	
		CashInOrderPayments: { key: "Id", keyType: "String" },
	
		CashInOrderPaymentLookup: { key: "Id", keyType: "String" },
	
		CashOutOrderPayments: { key: "Id", keyType: "String" },
	
		CashOutOrderPaymentLookup: { key: "Id", keyType: "String" },
	
		CateringTypes: { key: "Id", keyType: "String" },
	
		CateringTypeLookup: { key: "Id", keyType: "String" },
	
		CheckPayments: { key: "Id", keyType: "String" },
	
		CheckPaymentLookup: { key: "Id", keyType: "String" },
	
		Consignments: { key: "Id", keyType: "String" },
	
		ConsignmentLookup: { key: "Id", keyType: "String" },
	
		Countries: { key: "Id", keyType: "String" },
	
		CountryLookup: { key: "Id", keyType: "String" },
	
		CurrencyDailyRates: { key: "Id", keyType: "String" },
	
		Customers: { key: "Id", keyType: "String" },
	
		CustomerLookup: { key: "Id", keyType: "String" },
	
		Departments: { key: "Id", keyType: "String" },
	
		DepartmentLookup: { key: "Id", keyType: "String" },
	
		DocumentAccesses: { key: "Id", keyType: "String" },
	
		DocumentOwners: { key: "Id", keyType: "String" },
	
		ElectronicPayments: { key: "Id", keyType: "String" },
	
		ElectronicPaymentLookup: { key: "Id", keyType: "String" },
	
		Employees: { key: "Id", keyType: "String" },
	
		EmployeeLookup: { key: "Id", keyType: "String" },
	
		EverydayProfitReports: { key: "Id", keyType: "String" },
	
		Excursions: { key: "Id", keyType: "String" },
	
		ExcursionLookup: { key: "Id", keyType: "String" },
	
		Files: { key: "Id", keyType: "String" },
	
		FlightSegments: { key: "Id", keyType: "String" },
	
		FlightSegmentLookup: { key: "Id", keyType: "String" },
	
		FlownReports: { key: "Id", keyType: "String" },
	
		GdsAgents: { key: "Id", keyType: "String" },
	
		GdsAgentLookup: { key: "Id", keyType: "String" },
	
		GdsAgent_ApplyToUnassigned: { key: "Id", keyType: "String" },
	
		GdsFiles: { key: "Id", keyType: "String" },
	
		GdsFileLookup: { key: "Id", keyType: "String" },
	
		GenericProducts: { key: "Id", keyType: "String" },
	
		GenericProductLookup: { key: "Id", keyType: "String" },
	
		GenericProductProviders: { key: "Id", keyType: "String" },
	
		GenericProductProviderLookup: { key: "Id", keyType: "String" },
	
		GenericProductTypes: { key: "Id", keyType: "String" },
	
		GenericProductTypeLookup: { key: "Id", keyType: "String" },
	
		Identities: { key: "Id", keyType: "String" },
	
		IdentityLookup: { key: "Id", keyType: "String" },
	
		Insurances: { key: "Id", keyType: "String" },
	
		InsuranceLookup: { key: "Id", keyType: "String" },
	
		InsuranceCompanies: { key: "Id", keyType: "String" },
	
		InsuranceCompanyLookup: { key: "Id", keyType: "String" },
	
		InsuranceDocuments: { key: "Id", keyType: "String" },
	
		InsuranceDocumentLookup: { key: "Id", keyType: "String" },
	
		InsuranceRefunds: { key: "Id", keyType: "String" },
	
		InsuranceRefundLookup: { key: "Id", keyType: "String" },
	
		InternalIdentities: { key: "Id", keyType: "String" },
	
		InternalIdentityLookup: { key: "Id", keyType: "String" },
	
		InternalTransfers: { key: "Id", keyType: "String" },
	
		InternalTransferLookup: { key: "Id", keyType: "String" },
	
		Invoices: { key: "Id", keyType: "String" },
	
		InvoiceLookup: { key: "Id", keyType: "String" },
	
		Isics: { key: "Id", keyType: "String" },
	
		IsicLookup: { key: "Id", keyType: "String" },
	
		IssuedConsignments: { key: "Id", keyType: "String" },
	
		IssuedConsignmentLookup: { key: "Id", keyType: "String" },
	
		MilesCards: { key: "Id", keyType: "String" },
	
		MilesCardLookup: { key: "Id", keyType: "String" },
	
		OpeningBalances: { key: "Id", keyType: "String" },
	
		OpeningBalanceLookup: { key: "Id", keyType: "String" },
	
		Orders: { key: "Id", keyType: "String" },
	
		OrderLookup: { key: "Id", keyType: "String" },
	
		OrderBalances: { key: "Id", keyType: "String" },
	
		OrderByAssignedTo_TotalByIssueDate: { key: "Id", keyType: "String" },
	
		OrderByCustomer_TotalByIssueDate: { key: "Id", keyType: "String" },
	
		OrderByOwner_TotalByIssueDate: { key: "Id", keyType: "String" },
	
		OrderChecks: { key: "Id", keyType: "String" },
	
		OrderCheckLookup: { key: "Id", keyType: "String" },
	
		OrderItems: { key: "Id", keyType: "String" },
	
		OrderItemLookup: { key: "Id", keyType: "String" },
	
		Organizations: { key: "Id", keyType: "String" },
	
		OrganizationLookup: { key: "Id", keyType: "String" },
	
		Parties: { key: "Id", keyType: "String" },
	
		PartyLookup: { key: "Id", keyType: "String" },
	
		Passports: { key: "Id", keyType: "String" },
	
		PassportLookup: { key: "Id", keyType: "String" },
	
		Pasteboards: { key: "Id", keyType: "String" },
	
		PasteboardLookup: { key: "Id", keyType: "String" },
	
		PasteboardProviders: { key: "Id", keyType: "String" },
	
		PasteboardProviderLookup: { key: "Id", keyType: "String" },
	
		PasteboardRefunds: { key: "Id", keyType: "String" },
	
		PasteboardRefundLookup: { key: "Id", keyType: "String" },
	
		Payments: { key: "Id", keyType: "String" },
	
		PaymentLookup: { key: "Id", keyType: "String" },
	
		PaymentSystems: { key: "Id", keyType: "String" },
	
		PaymentSystemLookup: { key: "Id", keyType: "String" },
	
		Persons: { key: "Id", keyType: "String" },
	
		PersonLookup: { key: "Id", keyType: "String" },
	
		Products: { key: "Id", keyType: "String" },
	
		ProductLookup: { key: "Id", keyType: "String" },
	
		ProductByBooker_TotalByIssueDate: { key: "Id", keyType: "String" },
	
		ProductByProvider_TotalByIssueDate: { key: "Id", keyType: "String" },
	
		ProductBySeller_TotalByIssueDate: { key: "Id", keyType: "String" },
	
		ProductByTicketer_TotalByIssueDate: { key: "Id", keyType: "String" },
	
		ProductPassengers: { key: "Id", keyType: "String" },
	
		ProductSummaries: { key: "Id", keyType: "String" },
	
		ProductTotalByBookers: { key: "Id", keyType: "String" },
	
		ProductTotalByDays: { key: "Id", keyType: "String" },
	
		ProductTotalByMonths: { key: "Id", keyType: "String" },
	
		ProductTotalByOwners: { key: "Id", keyType: "String" },
	
		ProductTotalByProviders: { key: "Id", keyType: "String" },
	
		ProductTotalByQuarters: { key: "Id", keyType: "String" },
	
		ProductTotalBySellers: { key: "Id", keyType: "String" },
	
		ProductTotalByTypes: { key: "Id", keyType: "String" },
	
		ProductTotalByYears: { key: "Id", keyType: "String" },
	
		ProfitDistributionByCustomers: { key: "Id", keyType: "String" },
	
		ProfitDistributionByProviders: { key: "Id", keyType: "String" },
	
		RailwayDocuments: { key: "Id", keyType: "String" },
	
		RailwayDocumentLookup: { key: "Id", keyType: "String" },
	
		Receipts: { key: "Id", keyType: "String" },
	
		ReceiptLookup: { key: "Id", keyType: "String" },
	
		RoamingOperators: { key: "Id", keyType: "String" },
	
		RoamingOperatorLookup: { key: "Id", keyType: "String" },
	
		Sequences: { key: "Id", keyType: "String" },
	
		SequenceLookup: { key: "Id", keyType: "String" },
	
		SimCards: { key: "Id", keyType: "String" },
	
		SimCardLookup: { key: "Id", keyType: "String" },
	
		SystemConfigurations: { key: "Id", keyType: "String" },
	
		Tours: { key: "Id", keyType: "String" },
	
		TourLookup: { key: "Id", keyType: "String" },
	
		TourProviders: { key: "Id", keyType: "String" },
	
		TourProviderLookup: { key: "Id", keyType: "String" },
	
		Transfers: { key: "Id", keyType: "String" },
	
		TransferLookup: { key: "Id", keyType: "String" },
	
		TransferProviders: { key: "Id", keyType: "String" },
	
		TransferProviderLookup: { key: "Id", keyType: "String" },
	
		Users: { key: "Id", keyType: "String" },
	
		UserLookup: { key: "Id", keyType: "String" },
	
		WireTransfers: { key: "Id", keyType: "String" },
	
		WireTransferLookup: { key: "Id", keyType: "String" },
	
	};

}