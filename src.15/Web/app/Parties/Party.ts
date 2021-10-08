module Luxena
{

	export interface IPartySemantic
	{
		Contacts?: SemanticFieldSet;
		Addresses?: SemanticFieldSet;
	}


	Ui.fieldSet2(sd.Party, {
		name: "Contacts",
		title: "Контакты",
		members: se => [
			se.Phone1,
			se.Phone2,
			se.Email1,
			se.Email2,
			se.Fax,
			se.WebAddress,
		]
	});

	Ui.fieldSet2(sd.Party, {
		name: "Addresses",
		title: "Адреса",
		members: se => [
			se.ActualAddress,
			se.LegalAddress,
		]
	});

}

module Luxena.Views
{

	registerEntityControllers(sd.Party, se => ({
		
		list: [
			se.Type,
			se.Name,
			se.IsCustomer,
			se.IsSupplier,
		],

	}));

}