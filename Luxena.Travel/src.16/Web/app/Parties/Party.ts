module Luxena
{

	export interface IPartySemantic
	{
		EditContacts?: Field;
		Contacts?: Field;
		Addresses?: Field;
		PartyTags?: SemanticMember;

		OrderedTab?: Field;
		ProvidedProductTab?: Field;
		BalanceTab?: Field;
		StatisticsTab?: SemanticComponent<any>;
	}


	$doForDerived(sd.Party, se =>
	{
		se.PartyTags = se.member()
			.title("Тэги")
			.name("PartyTags")
			.boolSet([
				se.IsCustomer,
				se.IsSupplier,
			]);

		se.EditContacts = sd.col()
			.name("Contacts")
			.title("Контакты")
			.items(
				se.row(se.Phone1, se.Phone2).title("Телефоны"),
				se.row(se.Email1, se.Email2).title("E-mail"),
				se.WebAddress,
				se.Fax
		);

		se.Contacts = sd.col()
			.name("Contacts")
			.title("Контакты")
			.items(
				se.Phone1,
				se.Phone2,
				se.Email1,
				se.Email2,
				se.WebAddress,
				se.Fax
			);
		se.Addresses = 
			sd.col(se.ActualAddress, se.LegalAddress)
			.title("Адреса");


		se.OrderedTab = sd.Order.totalTab1("Заказывал", se => se.Customer, sd.OrderByCustomer_TotalByIssueDate);

		se.ProvidedProductTab = sd.col().icon(sd.Product).title("Поставил").unlabelItems().items(

			sd.header(sd.ProductByProvider_TotalByIssueDate),
			sd.Product.totalCharts1(se => se.Provider, sd.ProductByProvider_TotalByIssueDate),
			
			sd.hr2(),
			$as(sd.Product, se => se.totalGrid1()
				.gridController({
					filter: ctrl => [
						[[se.Provider, ctrl.masterId()], "or", [se.Producer, ctrl.masterId()]],
						//"and", [se.IsVoid, false]
					],
				})
			)
		);


		se.BalanceTab = sd.col().icon("balance-scale").title(sd.OrderBalance).unlabelItems().items(

			$as(sd.OrderBalance, se => se
				.chart(se.Customer)
				.chartController({
					fixed: true,
					argument: se.Order,
					value: se.Balance,
					colorMode: ChartColorMode.NegativePositive,
				})
			),

			sd.er(),

			$as(sd.OrderBalance, se => se
				.grid(se.Customer)
				.items([
					se.Order.fit().importent(),
					se.IssueDate,
					se.Currency,
					se.Delivered.totalSum(),
					se.Paid.totalSum(),
					se.Balance.sortOrder().importent().totalSum(),
				])
				.gridController({
					edit: null,
					fixed: true,
					useSorting: false,
					useGrouping: false,
					usePager: false,
					entityStatus: r => r.Balance < 0 ? "error" : "success",
				})
			),

			sd.hr2(),
			sd.header(sd.OpeningBalance._titles),
			$as(sd.OpeningBalance, se=> se.grid(se.Party))
		//)
		);

		se.StatisticsTab = sd.tabPanel().title("Статистика").icon("line-chart").items(
			se.BalanceTab.clone(),
			se.ProvidedProductTab.clone(),
			se.OrderedTab.clone()
		);
	});

}


module Luxena.Views
{

	registerEntityControllers(sd.Party, se => ({

		list: [
			se.PartyTags.field().compact().width(90),
			se.Type,
			se.Name,
			se.Contacts,
			se.Addresses,
		],

	}));

}