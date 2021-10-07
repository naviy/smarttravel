module Luxena.Views
{

	export interface EntityControllersOptions
	{
		entityTitle?: SemanticTitle<any>;
		formTitle?: SemanticTitle<any>;
		viewTitle?: SemanticTitle<any>;
		smartTitle?: SemanticTitle<any>;
		editTitle?: SemanticTitle<any>;

		members?: SemanticMembers<any>;
		list?: SemanticMembers<any>;
		form?: SemanticMembers<any>;
		view?: SemanticMembers<any>;
		smart?: SemanticMembers<any>;
		edit?: SemanticMembers<any>;

		formScope?: (ctrl: EditFormController, scope: any) => any;
		viewScope?: (ctrl: ViewFormController, scope: any) => { tabs: { title: string; template: string; scope? }[] };
		editScope?: (ctrl: EditFormController, scope: any) => any;

		actions?: SemanticEntityAction[];

		gridOptions?: DevExpress.ui.dxDataGridOptions;
	}


	export function registerEntityControllers<TEntity extends SemanticEntity>(
		entity: TEntity|TEntity[],
		config: ((se: TEntity) => SemanticObject<any>[]) | ((se: TEntity) => EntityControllersOptions)
	)
	{
		if (!config || !entity) return;

		const entities = <TEntity[]>($.isArray(entity) ? entity : [entity]);

		entities.forEach(se =>
		{
			var cfg = <EntityControllersOptions>config(se);

			if ($.isArray(cfg))
				cfg = { members: <SemanticMembers<any>>cfg };


			var listEntity = cfg.list instanceof SemanticEntity ? <SemanticEntity>cfg.list : se;

			var viewEntity = cfg.view === null ? null :
				(cfg.view || cfg.form) instanceof SemanticEntity ? <SemanticEntity>(cfg.view || cfg.form) : undefined;

			var editEntity = cfg.edit === null ? null :
				(cfg.edit || cfg.form) instanceof SemanticEntity ? <SemanticEntity>(cfg.edit || cfg.form) : undefined;


			var listMembers = cfg.list || (cfg.list === null ? null : cfg.members);

			if (listMembers)
			{
				se._listController = Views[se.listView()] = () =>
				{
					if (listMembers instanceof SemanticEntity)
					{
						var params = listMembers.resolveListAction().uri;

						return {
							viewShowing: () => app.navigate(params, { target: 'current' })
						}
					}

					var ctrl = new GridController({
						entity: se,
						members: listMembers,
						gridOptions: cfg.gridOptions,

						view: viewEntity,
						edit: editEntity,
					});

					return ctrl.getScope();
				};
			}


			var viewMembers = cfg.view || (cfg.view === null ? null : cfg.form || cfg.members);

			if (viewMembers || se._isAbstract)
			{
				se._viewController = Views[se.viewView()] = (...args) =>
				{
					if (viewMembers instanceof SemanticEntity)
					{
						var params = $.extend(args[0] || {},
							viewMembers.resolveViewAction().uri);

						return {
							viewShowing: () => app.navigate(params, { target: 'current' })
						}
					}

					var ctrl = new ViewFormController({
						entity: se,
						args: args,
						entityTitle: cfg.viewTitle || cfg.formTitle || cfg.entityTitle,
						members: viewMembers,

						list: listEntity,
						edit: editEntity,
					});


					var scope = ctrl.getScope();
					var scopeExt = <any>cfg.viewScope || cfg.formScope;

					scopeExt = scopeExt && scopeExt(ctrl, scope);
					//$log(scopeExt);
					if (scopeExt)
						scope = $.extend(scope, scopeExt);

					return scope;
				};
			}


			var smartMembers = cfg.smart || (cfg.smart === null ? null : cfg.view || cfg.form || cfg.members);

			if (smartMembers)
			{
				se.showSmart = (target, smartCfg) =>
				{
					smartCfg.entity = se;
					smartCfg.entityTitle = cfg.smartTitle || cfg.viewTitle || cfg.formTitle || cfg.entityTitle;
					smartCfg.members = smartMembers;

					smartCfg.view = smartCfg.view || viewEntity;
					smartCfg.edit = smartCfg.edit || editEntity;

					smartCfg.actions = cfg.actions;

					var ctrl = new SmartFormController(smartCfg);

					ctrl.show(target);
				};
			}


			var editMembers = cfg.edit || (cfg.edit === null ? null : cfg.form || cfg.members);

			if (editMembers || se._isAbstract)
			{
				se._editController = Views[se.editView()] = (...args) =>
				{
					if (editMembers instanceof SemanticEntity)
					{
						var params = $.extend(args[0] || {},
							(<SemanticEntity>viewMembers).resolveEditAction().uri);

						return {
							viewShowing: () => app.navigate(params, { target: 'current' })
						}
					}

					var ctrl = new EditFormController({
						entity: se,
						args: args,
						entityTitle: cfg.editTitle || cfg.formTitle || cfg.entityTitle,
						members: editMembers,

						list: cfg.list instanceof SemanticEntity ? <SemanticEntity>cfg.list : se,
						view: cfg.view instanceof SemanticEntity ? <SemanticEntity>cfg.view : undefined,
					});


					var scope = ctrl.getScope();
					var scopeExt = <any>cfg.editScope || cfg.formScope;

					scopeExt = scopeExt && scopeExt(ctrl, scope);

					if (scopeExt)
						scope = $.extend(scope, scopeExt);

					return scope;
				};
			}
		});
	}

}