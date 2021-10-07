module Luxena
{

	export class SemanticFieldSet extends SemanticComponent<SemanticFieldSet>
	{

		constructor(
			public members1: SemanticMembers<any>,
			public members2?: SemanticMembers<any>,
			public members3?: SemanticMembers<any>,
			public members4?: SemanticMembers<any>
		)
		{
			super();
		}

		protected itemsByColumn: Array<SemanticComponent<any>[]>;


		addItemsToController(ctrl: SemanticController, action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void)
		{
			var cols = this.itemsByColumn = [];

			this.members1 && cols.push(ctrl.addComponents(this.members1, null));
			this.members2 && cols.push(ctrl.addComponents(this.members2, null));
			this.members3 && cols.push(ctrl.addComponents(this.members3, null));
			this.members4 && cols.push(ctrl.addComponents(this.members4, null));
		}


		render(container: JQuery)
		{
			var colCount = this.itemsByColumn.length;

			if (colCount <= 0) return;

			if (colCount === 1)
			{
				this.itemsByColumn[0].forEach(sc => sc.render(container));
				return;
			}

			var tbl = $("<div>")
				.css("display", "table")
				.css("width", "100%")
				.css("margin-bottom", "10px");

			this.itemsByColumn.forEach((items, i) =>
			{
				var colEl = $("<div>")
					.css("display", "table-cell")
					.appendTo(tbl);

				items.forEach(sc => sc.render(colEl));
			});

			container.append(tbl);
		}


		toTab()
		{
			return {
				title: this._title,
				template: this._name,
			};
		}
	}

}


module Luxena.Ui
{


	export interface ISemanticFieldSetConfig2<TEntity extends ISemanticEntity> //extends ISemanticFieldSetConfig
	{
		title?: string | SemanticMember | ((se: TEntity) => SemanticMember);
		members: SemanticMembers<any>;
		members2?: SemanticMembers<any>;
		members3?: SemanticMembers<any>;
		members4?: SemanticMembers<any>;
		name?: string;
	}


	export function fieldSet2<TEntity extends ISemanticEntity>(
		se: TEntity,
		cfg: ISemanticFieldSetConfig2<TEntity>
	): SemanticFieldSet
	{
		if (cfg.name)
		{
			se.applyToThisAndDerived(dse =>
			{
				var sc = new SemanticFieldSet(cfg.members, cfg.members2, cfg.members3, cfg.members4);

				sc._entity = dse;
				sc._name = cfg.name;
				sc.title(cfg.title);

				dse[cfg.name] = sc;
			});

			return null;
		}
		else
		{
			var sc = new SemanticFieldSet(cfg.members, cfg.members2, cfg.members3, cfg.members4);

			sc._entity = se;
			sc.title(cfg.title);

			return sc;
		}
	}


	export function fieldSet<TEntity extends ISemanticEntity>(
		se: TEntity,
		title: string | SemanticMember | ((se: TEntity) => SemanticMember),
		members: SemanticMembers<any>,
		members2?: SemanticMembers<any>,
		members3?: SemanticMembers<any>,
		members4?: SemanticMembers<any>
	): SemanticFieldSet
	{
		return fieldSet2(se, {
			title: title,
			members: members,
			members2: members2,
			members3: members3,
			members4: members4,
		});
	}

}