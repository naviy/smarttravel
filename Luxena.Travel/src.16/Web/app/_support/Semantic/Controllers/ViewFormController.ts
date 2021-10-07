module Luxena
{

	export interface ViewFormControllerConfig extends FormControllerConfig
	{
	}


	export class ViewFormController extends FormController<ViewFormControllerConfig>
	{
		viewMode = true;


		getToolbarItems()
		{
			const cfg = this.config;
			const se = this._entity;
			const items = super.getToolbarItems();

			if (cfg.edit)
				items.push(cfg.edit.editAction.button().right());

			items.push(se.refreshAction.button().small().right());

			var items2: any[] = [];

			cfg.edit && items2.push(cfg.edit.deleteAction.button().right().onExecuteDone(() => app.back()));

			cfg.list && items2.push(cfg.list.backToListAction);
			
			items.push(sd.button(...items2).right());

			return items;
		}


		//toDetailListTab<TEntity extends SemanticEntity>(
		//	se: TEntity,
		//	getMasterMember: (se: TEntity) => SemanticMember,
		//	members: SemanticMembers<any>,
		//	cfg?: GridControllerConfigExt
		//): any
		//{
		//	return {
		//		entity: se,
		//		title: cfg && cfg.title,
		//		template: "grid",

		//		scope: new GridController($.extend(cfg || { inline: true }, {
		//			entity: se,
		//			master: this,
		//			defaults: [[getMasterMember(se), this.getId()]],
		//			members: members,
		//		})).getScope()
		//	};
		//}

	}

}