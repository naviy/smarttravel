namespace Luxena
{

	export abstract class Entity extends Semantic.Entity
	{
		Id = this.stringMember("Id");
	}

	export abstract class Entity2 extends Entity
	{

	}

	export abstract class Entity3 extends Entity2
	{
		Name = this.stringMember("Name", o => o.title("Название"));
	}

	export class Party extends Entity3
	{
		constructor()
		{
			super();

			this._store = db.Parties;
		}

		Type = this.stringMember("Type", o => o.title("Тип"));

		LegalAddress = this.stringMember("LegalAddress", o => o.title("Legal Address"));
		PhysicalAddress = this.stringMember("PhysicalAddress", o => o.title("Physical Address"));

		Phone1 = this.stringMember("Phone1", o => o.title("Телефон 1"));
		Phone2 = this.stringMember("Phone2", o => o.title("Телефон 2"));
		Email1 = this.stringMember("Email1", o => o.title("Email 1"));
		Email2 = this.stringMember("Email2", o => o.title("Email 2"));
	}


	export var sd_ = {
		Party: new Party(),
	};

}


namespace Luxena.Views
{

	export var Test16 = () =>
	{
		var se1 = sd_.Party;

		var v = new Components_.View();

		var ds1 = v.dataSet(se1);

		var grid1 = v.grid().source(ds1).options(o => o
			.columns([
				//se1.Id,
				se1.Name,
				se1.Phone1,
				se1.Email1,
			])
			.gridOptions({
				paging: { pageSize: 10, },
			})
			.title("qqqq")
		);

		var panel1 = v.fieldSet().items([
			"Выбранный контрагент",
			se1.Name,
			//v.ifView()
			//v.ifEdit()
			v.initIf(cnt => cnt.viewMode(), (item, result) => item.visible(result))
				.then([
					se1.Phone1,
					se1.Email1,
				])
				.else([
					se1.Phone1,
					se1.Phone2,
					se1.Email1,
					se1.Email2,
					v.prepareIf(r => se1.Type.get(r) !== "Person").then([
						se1.LegalAddress,
						se1.PhysicalAddress
					]),
				]),
		]).source(ds1);


		//panel1.options(o => o.editMode());

		v.items([grid1, panel1]);

		return {
			//renderer: v.getRenderer(),
			renderer: (container: JQuery) =>
			{
				ds1.prepare();
				ds1.repaint(container);
			}
		};
	};

}