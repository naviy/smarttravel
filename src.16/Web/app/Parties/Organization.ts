module Luxena
{

	export interface IOrganizationSemantic
	{
		Tags?: SemanticMember;
		DepartmentsTab?: Field;
		EmployeesTab?: Field;
	}

	$do(sd.Organization, se =>
	{
		se.Tags = se.member()
			.title("Тэги")
			.name("Tags")
			.boolSet([
				se.IsCustomer,
				se.IsSupplier,
				se.IsAccommodationProvider,
				se.IsAirline,
				se.IsBusTicketProvider,
				se.IsCarRentalProvider,
				se.IsGenericProductProvider,
				se.IsInsuranceCompany,
				se.IsPasteboardProvider,
				se.IsRoamingOperator,
				se.IsTourProvider,
				se.IsTransferProvider,
			]);

		se.DepartmentsTab = se.Departments.field().unlabel()
			//.otitle(r => `${sd.Department._titles} (${se.DepartmentCount.get(r) })`)
			.badge(r => se.DepartmentCount.get(r))
			.items((se: IDepartmentSemantic) => [se.Name, se.Contacts])
			.dependencies(se.DepartmentCount)
			.gridController({ inline: true, useSearch: true, });

		se.EmployeesTab = se.Employees.field().unlabel()
			//.otitle(r => `${se.Employees._title} (${se.EmployeeCount.get(r) })`)
			.badge(r => se.EmployeeCount.get(r))
			.items((se: IPersonSemantic) => [se.Name, se.Title, se.Contacts])
			.dependencies(se.EmployeeCount)
			.gridController({ inline: true, useSearch: true, });
	});
}


module Luxena.Views
{

	registerEntityControllers(sd.Organization, se => ({

		list: () => [
			se.Tags.field().compact().width(90),
			se.Name,
			se.LegalName,
			se.Contacts,
			se.Addresses,
		],

		view: () =>
		{

			//#region mainTab

			const mainTab = sd.col().icon(se).items(
				se.NameForDocuments.field().header3(),
				sd.hr(),
				sd.row(
					sd.col(
						se.Tags.field().unlabel(),
						se.Code, 
						//se.ReportsTo,
						se.DefaultBankAccount,
						se.Note.field().labelAsHeader(),

						sd.col(
							se.IsAirline.field().header5(),
							sd.col(
								se.AirlineIataCode,
								se.AirlinePrefixCode,
								se.AirlinePassportRequirement
							).indentLabelItems()
						)

					).length(7),
					sd.col(
						se.Contacts.clone().labelAsHeader().unlabelItems(),
						se.Addresses.clone().labelAsHeaderItems()
					).length(5)
				)
			);

			//#endregion


			return sd.tabPanel().card().items(

				mainTab,

				se.DepartmentsTab.clone(),

				se.EmployeesTab.clone(),

				//se.StatisticsTab.clone()
				se.OrderedTab.clone(),
				se.BalanceTab.clone(),
				se.ProvidedProductTab.clone(),
				se.HistoryTab.clone()
			);
		},


		smart: () => sd.tabPanel(

			sd.col().icon(se).items(
				se.Name.header2().icon(se),
				se.LegalName.header3(),
				sd.er(),
				sd.row(
					se.Tags.field().unlabel().length(5),
					se.Contacts.unlabelItems().length(7)
				)
			),//.height(200),

			se.DepartmentsTab.clone(),
			se.EmployeesTab.clone(),
			se.HistoryTab.clone()
		),

		smartConfig: {
			contentWidth: 600
			//contentHeight: 300
		},


		edit: () => sd.tabPanel().card().items(

			sd.col().title("Общее").items(
				se.Name,
				se.LegalName,
				se.Tags,
				se.Code,
				//se.ReportsTo,
				se.DefaultBankAccount,
				se.Note.lineCount(6).field().labelAsHeader()
			),

			sd.col().title(se.Contacts).items(
				se.EditContacts.clone(),
				se.Addresses.clone().labelAsHeaderItems()
			),

			sd.col().title("Провайдер услуг").items(
				se.IsAirline.field().header5(),
				sd.col(
					se.AirlineIataCode,
					se.AirlinePrefixCode,
					se.AirlinePassportRequirement
				),
				sd.hr2(),
				sd.row(
					sd.col(
						se.IsAccommodationProvider,
						se.IsBusTicketProvider,
						se.IsCarRentalProvider,
						se.IsPasteboardProvider,
						se.IsTourProvider
					).unlabelItems(),
					sd.col(
						se.IsTransferProvider,
						se.IsGenericProductProvider,
						se.IsProvider,
						se.IsInsuranceCompany,
						se.IsRoamingOperator
					).unlabelItems()
				)
			)
		)


	}));

}