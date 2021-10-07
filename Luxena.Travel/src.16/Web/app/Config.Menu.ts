module Luxena
{

	function sameTitleMenuItems(entities: SemanticEntity[]): SemanticEntity[]
	{
		entities.forEach(se => se.titleMenuItems(entities));
		return entities;
	}


	config.menu = [
		{
			icon: "home",
			text: "Домашняя страница",
			onClick: "home",
		},
		//{
		//	icon: "shopping-cart",
		//	text: "Продажи",
		//	hint: "Информация о заказах и платежах",

		//	items: toMenuItems([
		//		sd.Order,

		//		sd.Consignment,
		//		sd.IssuedConsignment,
		//		sd.Invoice,
		//		sd.Payment,
		//		sd.InternalTransfer,
		//		sd.OrderCheck,

		//		sd.CurrencyDailyRate,
		//	]),
		//},
		//{
		//	icon: "suitcase",
		//	text: "Услуги",

		//	items: toMenuItems([
		//		sd.Product,//.titleMenuItems([
		//			//sd.Product,
		//		sd.AviaDocument,
		//		sd.BusDocument,
		//		sd.CarRental,
		//		sd.RailwayDocument,
		//		sd.Accommodation,
		//		sd.InsuranceDocument,
		//		sd.Isic,
		//		sd.SimCard,
		//		sd.Transfer,
		//		sd.Tour,
		//		sd.Excursion,
		//		sd.GenericProduct,
		//		//]),

		//		//sd.ProductSummary,

		//		sd.FlightSegment,
		//	]),
		//},
		//{
		//	icon: "line-chart",
		//	text: "Аналитика",

		//	items: toMenuItems([
		//		sd.EverydayProfitReport.titleMenuItems(sameTitleMenuItems([
		//			sd.EverydayProfitReport,
		//			sd.ProfitDistributionByProvider,
		//			sd.ProfitDistributionByCustomer,
		//			sd.FlownReport,
		//			sd.ProductSummary,
		//		])),
		//		sd.ProfitDistributionByProvider,
		//		sd.ProfitDistributionByCustomer,
		//		sd.FlownReport,

		//		sd.ProductSummary.titleMenuItems(sameTitleMenuItems([
		//			sd.ProductSummary,
		//			sd.ProductTotalByYear,
		//			sd.ProductTotalByQuarter,
		//			sd.ProductTotalByMonth,
		//			sd.ProductTotalByDay,
		//			sd.ProductTotalByType,
		//			sd.ProductTotalByProvider,
		//			sd.ProductTotalBySeller,
		//			sd.ProductTotalByBooker,
		//			sd.ProductTotalByOwner,
		//		])), 
		//	]),
		//},
		//{
		//	icon: "group",
		//	text: "Заказчики и поставщики",
		//	items: toMenuItems([

		//		sd.Party, //.titleMenuItems([
		//		//	sd.Organization,
		//		//	sd.Person,
		//		//	sd.Department,
		//		//	sd.Party,
		//		//	sd.Customer,
		//		//]),

		//		sd.Person,//.titleMenuItems([
		//		//	sd.Person,
		//		//	sd.Party,
		//		//	sd.Organization,
		//		//	sd.Agent,
		//		//	sd.Passport,
		//		//	sd.MilesCard,
		//		//]),

		//		sd.Organization,//.titleMenuItems([
		//		//	sd.Organization,
		//		//	sd.Airline,
		//		//	sd.InsuranceCompany,
		//		//	sd.RoamingOperator,
		//		//	sd.AccommodationProvider,
		//		//	sd.BusTicketProvider,
		//		//	sd.CarRentalProvider,
		//		//	sd.GenericProductProvider,
		//		//	sd.PasteboardProvider,
		//		//	sd.TourProvider,
		//		//	sd.TransferProvider,
		//		//	sd.Person,
		//		//	sd.Party,
		//		//]),

		//		sd.Department,

		//		sd.MilesCard,//.titleMenuItems([
		//		//	sd.Person,
		//		//	sd.Passport,
		//		//]),

		//		sd.Passport,//.titleMenuItems([
		//		//	sd.Person,
		//		//	sd.MilesCard,
		//		//]),

		//		//sd.AirlineServiceClass,
		//		sd.Employee,
		//	]),
		//},
		//{
		//	icon: "tasks",
		//	text: "Персонал",
		//	items: toMenuItems([
		//		sd.Agent,
		//		sd.DocumentOwner,
		//		sd.DocumentAccess,
		//		sd.GdsAgent,
		//		sd.GdsFile,
		//	])
		//},
		//{
		//	icon: "book",
		//	text: "Справочники",
		//	items: toMenuItems([
		//		sd.Airport,
		//		sd.Country,
		//		sd.AccommodationType,
		//		sd.CateringType,
		//		sd.GenericProductType,
		//		sd.PaymentSystem,
		//	]),
		//},
		//{
		//	icon: "gears",
		//	text: "Настройки",
		//	items: toMenuItems([
		//		sd.BankAccount,

		//		sd.User,//.titleMenuItems(sameTitleMenuItems([
		//		//	sd.User,
		//		//	sd.Identity,
		//		//	sd.InternalIdentity,
		//		//])),

		//		sd.Sequence,

		//		sd.SystemConfiguration.viewSingletonAction,
		//	]),
		//},
		//{
		//	icon: "support",
		//	text: "Отзывы и предложения",
		//	onClick: "support",
		//},
	];


}
