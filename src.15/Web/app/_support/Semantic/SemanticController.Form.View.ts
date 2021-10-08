module Luxena
{

	export interface IViewFormControllerConfig extends FormControllerConfig
	{
	}


	export class ViewFormController extends FormController<IViewFormControllerConfig>
	{
		viewMode = true;

		getMenuItems(): any[]
		{
			var cfg = this.config;

			var menu = [];
			var id = this.getId();

			cfg.edit && menu.push(cfg.edit.toEditButton(id));

			var menu2 = {
				icon: "ellipsis-v",
				items: <any[]>[]
			};

			menu.push(menu2);

			cfg.edit && menu2.items.push(cfg.edit.toDeleteButton(id, () => app.back()));

			cfg.list && menu2.items.push(cfg.list.toListButton());

			menu2.items.push(cfg.entity.toRefreshButton(() => this.refresh()));

			return menu;
		}


		toDetailListTab<TEntity extends SemanticEntity>(
			se: TEntity,
			getMasterMember: (se: TEntity) => SemanticMember,
			members: SemanticMembers<any>,
			cfg?: IGridControllerConfigExt
		): any
		{
			return {
				entity: se,
				title: cfg && cfg.title,
				template: "grid",

				scope: new GridController($.extend(cfg || { inline: true }, {
					entity: se,
					master: this,
					defaults: [[getMasterMember(se), this.getId()]],
					members: members,
				})).getScope()
			};
		}

	}

}