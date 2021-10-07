module Luxena
{

	export interface ISemanticFieldRowConfig
	{
		length?: number;
		width?: number|string;
		cellTemplate?: (sc: SemanticFieldRow, cell: JQuery, cellInfo) => void;
		renderDisplay?: (sc: SemanticFieldRow, container: JQuery) => void;
		loadFromData?: (sc: SemanticFieldRow, model: any, data: any) => void;
	}


	export class SemanticFieldRow extends SemanticComponent<SemanticFieldRow>
	{
		_rowMode = true;

		constructor(public members: SemanticMembers<any>, public config?: ISemanticFieldRowConfig)
		{
			super();

			this.config = this.config || {};
		}

		protected components: SemanticComponent<any>[];


		title(title: SemanticTitle<any>)
		{
			if (title === undefined)
				title = "/";

			return super.title(title);
		}


		addItemsToController(ctrl: SemanticController, action?: (sm: SemanticMember, sc: SemanticFieldRow) => void)
		{
			if (this._controller.gridMode && this.config.cellTemplate) return;

			this.components = ctrl.addComponents(this.members, null);

			if (this._title === undefined || this._title === "/")
			{
				this._title = this.components.map(sc =>
				{
					if (sc instanceof SemanticField)
						return sc.member._title || sc._title || "";
					else
						return sc._title || "";
				}).join(" / ");
			}
		}


		loadFromData(model: any, data: any): void
		{
			const cfg = this.config;
			cfg.loadFromData && cfg.loadFromData(this, model, data);
		}


		toGridColumns(): DevExpress.ui.dxDataGridColumn[]
		{
			var cfg = this.config;
			if (!cfg.cellTemplate) return [];

			var width = cfg.width;
			if (!width && cfg.length)
				width = 14 + Math.round(SemanticType.charWidth * cfg.length) + 12;

			var col = <DevExpress.ui.dxDataGridColumn>{
				allowFiltering: false,
				allowGrouping: false,
				allowSorting: false,
				caption: this._title,
				width: width,
				cellTemplate: (cell, cellInfo) => cfg.cellTemplate(this, cell, cellInfo),
			};

			return [col];
		}

		render(container: JQuery)
		{
			const cfg = this.config;
			if (!this._controller.editMode && cfg.renderDisplay)
			{
				cfg.renderDisplay(this, container);
				return;
			}


			var rowEl = $("<div>")
				.addClass("dx-field");

			$("<div>")
				.addClass("dx-field-label")
				.attr("title", this._title)
				.text(this._title + (this._title && ":"))
				.appendTo(rowEl);

			if (this._controller.editMode)
			{
				var valueBox = $("<div>")
					.addClass("field-row");

				$("<div>")
					.addClass("dx-field-value")
					.append(valueBox)
					.appendTo(rowEl);

				this.components.forEach(sc =>
				{
					var valueEl = $("<div>")
						.appendTo(valueBox);

					sc._rowMode = true;

					if (sc instanceof SemanticField)
					{
						const length = sc.getLength();
						valueEl.css("width", (length.length || 1));

						if (length.min)
							valueEl.css("min-width", sc.getWidth(length.min));
						sc.member._type.render(sc, valueEl, valueEl);
					}
					else
						sc.render(valueEl);

				});
			}
			else
			{
				var valueDiv = $("<div>")
					.addClass("dx-field-value-static")
					.appendTo(rowEl);

				this.components.forEach((sc, i) =>
				{
					if (i > 0)
						valueDiv.append("&nbsp; / &nbsp;");

					var valueEl = $("<span>")
						.appendTo(valueDiv);

					sc._rowMode = true;
					if (sc instanceof SemanticField)
					{
						const sm = sc.member;
						valueEl.attr("title", sm._shortTitle || sm._title);
						sm._type.render(sc, valueEl, valueEl);
					}
					else
						sc.render(valueEl);
				});
			}

			container.append(rowEl);
		}
	}

}


module Luxena.Ui
{

	export interface ISemanticFieldRowConfig2<TEntity extends ISemanticEntity> extends ISemanticFieldRowConfig
	{
		title?: SemanticTitle<TEntity>;
		members: SemanticMembers<TEntity>;
		name?: string;
	}


	export function fieldRow2<TEntity extends ISemanticEntity>(
		se: TEntity,
		cfg: ISemanticFieldRowConfig2<TEntity>
	): SemanticFieldRow
	{

		if (cfg.name)
		{
			se.applyToThisAndDerived(dse =>
			{
				var sc = new SemanticFieldRow(cfg.members, cfg);

				sc._entity = dse;
				sc._name = cfg.name;
				sc.title(cfg.title);

				dse[cfg.name] = sc;
			});

			return null;
		}
		else
		{
			const sc = new SemanticFieldRow(cfg.members, cfg);

			sc._entity = se;
			sc.title(cfg.title);

			return sc;
		}
	}


	export function fieldRow<TEntity extends ISemanticEntity>(
		se: TEntity,
		title: SemanticTitle<TEntity>,
		members: SemanticMembers<TEntity>
	): SemanticFieldRow
	{
		return fieldRow2(se, {
			title: title,
			members: members,
		});
	}

}