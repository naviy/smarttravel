module Luxena
{

	function sameTitleMenuItems(entities: SemanticEntity[]): SemanticEntity[]
	{
		entities.forEach(se => se.titleMenuItems(entities));
		return entities;
	}


	config.menu = [
		//		{
		//			icon: "navicon",
		//			title: "Домашняя страница",
		//			url: "home",
		//		},
		{
			icon: "shopping-cart",
			title: "Продажи",
			description: "Информация о заказах и платежах",

			items: [
				sd.Order,

				sd.Consignment,
				sd.IssuedConsignment,
				sd.Invoice,
				sd.Payment,
				sd.InternalTransfer,
				sd.OrderCheck,

				sd.CurrencyDailyRate,
			],
		},
		{
			icon: "suitcase",
			title: "Услуги",

			items: [
				sd.Product.titleMenuItems([
					sd.Product,
					sd.AviaDocument,
					sd.BusDocument,
					sd.CarRental,
					sd.RailwayDocument,
					sd.Accommodation,
					sd.InsuranceDocument,
					sd.Isic,
					sd.SimCard,
					sd.Transfer,
					sd.Tour,
					sd.Excursion,
					sd.GenericProduct,
				]),

				sd.ProductSummary,

				sd.FlightSegment,
			],
		},
		{
			icon: "line-chart",
			title: "Аналитика",

			items: [
				sd.EverydayProfitReport.titleMenuItems(sameTitleMenuItems([
					sd.EverydayProfitReport,
					sd.ProfitDistributionByProvider,
					sd.ProfitDistributionByCustomer,
					sd.FlownReport,
					sd.ProductSummary,
				])),
				sd.ProfitDistributionByProvider,
				sd.ProfitDistributionByCustomer,
				sd.FlownReport,

				sd.ProductSummary.titleMenuItems(sameTitleMenuItems([
					sd.ProductSummary,
					sd.ProductTotalByYear,
					sd.ProductTotalByQuarter,
					sd.ProductTotalByMonth,
					sd.ProductTotalByDay,
					sd.ProductTotalByType,
					sd.ProductTotalByProvider,
					sd.ProductTotalBySeller,
					sd.ProductTotalByBooker,
					sd.ProductTotalByOwner,
				])), 
			],
		},
		{
			icon: "group",
			title: "Заказчики и поставщики",
			items: [

				sd.Party.titleMenuItems([
					sd.Organization,
					sd.Person,
					sd.Department,
					sd.Party,
					sd.Customer,
				]),

				sd.Organization.titleMenuItems([
					sd.Organization,
					sd.Airline,
					sd.InsuranceCompany,
					sd.RoamingOperator,
					sd.AccommodationProvider,
					sd.BusTicketProvider,
					sd.CarRentalProvider,
					sd.GenericProductProvider,
					sd.PasteboardProvider,
					sd.TourProvider,
					sd.TransferProvider,
					sd.Person,
					sd.Party,
				]),

				sd.Person.titleMenuItems([
					sd.Person,
					sd.Party,
					sd.Organization,
					sd.Agent,
					sd.Passport,
					sd.MilesCard,
				]),

				sd.MilesCard.titleMenuItems([
					sd.Person,
					sd.Passport,
				]),

				sd.Passport.titleMenuItems([
					sd.Person,
					sd.MilesCard,
				]),

				sd.AirlineServiceClass,
			],
		},
		{
			icon: "book",
			title: "Справочники",
			items: [
				sd.Airport,
				sd.Country,
				sd.AccommodationType,
				sd.CateringType,
				sd.GenericProductType,
				sd.PaymentSystem,
			],
		},
		{
			icon: "gears",
			title: "Настройки",
			items: [
				sd.BankAccount,
				sd.DocumentOwner,
				sd.DocumentAccess,
				sd.GdsAgent,
				sd.GdsFile,

				sd.User.titleMenuItems(sameTitleMenuItems([
					sd.User,
					sd.Identity,
					sd.InternalIdentity,
				])),

				sd.Sequence,

				sd.SystemConfiguration.toViewMenuItem("single"),
			],
		},
		{
			icon: "support",
			title: "Отзывы и предложения",
			onExecute: "support",
		},
	];


}
