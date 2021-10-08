module Luxena
{

	export class SemanticGridField extends SemanticComponent<SemanticGridField>
	{

		constructor(
			public gridEntity: SemanticEntity,
			public getMasterMember: (se: SemanticEntity) => SemanticMember,
			public members: SemanticMembers<any>,
			public config?: IGridControllerConfigExt)
		{
			super();
		}


		gridController(master?: SemanticController)
		{
			var cfg = this.config || { inline: true };
			master = master || this._controller;

			return new GridController($.extend(cfg, {
				entity: this.gridEntity,
				master: master,
				defaults: [[this.getMasterMember(this.gridEntity), master.getId()]],
				members: this.members,
			}));
		}

		render(container: JQuery)
		{
			var scope = this.gridController().getScope();

			this._controller.model[this._name] = scope.gridOptions;

			$("<div>")
				.attr("data-bind", `dxDataGrid: r.${this._name}`)
				.appendTo(container);
		}

		toTab(master: SemanticController)
		{
			return {
				entity: this.gridEntity,
				title: this._title,
				template: "grid",
				scope: this.gridController(master).getScope(),
			}
		}
	}

}