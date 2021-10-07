module Luxena
{

	export abstract class SemanticFieldType
	{
		_gridMode: boolean;
		_chartMode: boolean;

		static charWidth = 10;
		static digitWidth = 7.6;

		charWidth: number;

		allowFiltering = true;
		allowGrouping = true;
		allowSorting = true;
		chartDataType: string;
		dataType = "string";
		format: string;
		length: number;

		addColumnFilterWidth: number;
		nullable = true;

		_isComposite: boolean;
		_icon: string;
		_singleLine: boolean;


		initMember(sm: SemanticMember)
		{
		}


		//#region Data & Model

		addItemsToController(sf: Field, ctrl: SemanticController, action?: (sm: SemanticMember, sc: SemanticComponent<any>) => void)
		{
		}

		getSelectFieldNames(sf: Field): string[]
		{
			return [sf._name];
		}

		getExpandFieldNames(sf: Field): string[]
		{
			return [];
		}

		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			return [sm._name, operation || "=", value];
		}


		getFromData(sm: SemanticMember, data: any): any
		{
			return data[sm._name];
		}

		getModel(model: any, name: string): any
		{
			return model[name];
		}

		setModel(model: any, sname: string | Field, value: any): void
		{
			var name: string, sf: Field = null;
			if (typeof sname === "string")
			{
				name = sname;
			}
			else
			{
				sf = sname;
				name = sf._name;
			}

			const existsValue = model[name];

			if (sf && existsValue && value === undefined && sf._controller.modelIsExternal)
			{
				return;
			}

			if (value === undefined)
				value = null;

			if (!ko.isObservable(existsValue))
				model[name] = ko.observable(value);
			else
				existsValue(value);
		}

		loadFromData(sf: Field, model: any, data: any): void
		{
			const value = data[sf._name];
			this.setModel(model, sf, value);
		}

		saveToData(sf: Field, model: any, data: any): void
		{
			const name = sf._name;
			data[name] = $clip(ko.unwrap(model[name]));
		}

		removeFromData(sf: Field | SemanticMember, data: any): void
		{
			delete data[sf._name];
		}

		//#endregion


		//#region Renders

		toGridColumns(sf: Field)
		{
			return [this.toStdGridColumn(sf)];
		}

		toStdGridColumn(sf: Field)
		{
			const sm = sf._member;
			const se = sf._entity;
			const cfg = (<GridController>sf._controller).config;

			const col = <DevExpress.ui.dxDataGridColumn>{
				allowFiltering: sm._allowFiltering && this.allowFiltering && !sm._isCalculated,
				allowGrouping: sm._allowGrouping && this.allowGrouping && !sm._isCalculated,
				allowSorting: !sm._isCalculated && this.allowSorting,
				caption: sm._title,
				dataField: sm._name,
				dataType: this.dataType,
				fixed: sm._columnFixed || (cfg.wide || se._isWide) && sm._kind === SemanticMemberKind.Primary,
				format: sm._format || this.format,
				groupIndex: sf._groupIndex !== undefined ? sf._groupIndex : sm._groupIndex,
				sortOrder: sf.sortOrder || sm._sortOrder,
				width: sf.getWidth(),
				visible: sf._columnVisible && sm._columnVisible,

				calculateFilterExpression: (value, operation) => this.getFilterExpr(sm, value, operation),
			};

			col.cellTemplate = (cell: JQuery, cellInfo) =>
			{
				this.renderDisplayStatic(sf, cell, cellInfo.data);
			};

			return col;
		}

		toGridTotalItems(sf: Field)
		{
			return [];
		}


		prerender(sf: Field) { }

		render(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			if (sf._controller.editMode && !sf._member._isReadOnly)
				this.renderEditor(sf, valueEl, rowEl);
			else
				this.renderDisplay(sf, valueEl, rowEl);
		}

		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			let value = $clip(data[sf._name]);
			if (!value) return;

			if (sf._member._kind === SemanticMemberKind.Important)
				value = `<b>${value}</b>`;

			if (this._singleLine)
				container.append(`<span class="nowrap">${sf._member.getIconHtml() }${value}</span>`);
			else
				container.append(sf.getIconHtml() + value);
		}


		renderDisplay(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			this.renderDisplayBind(sf, valueEl, rowEl);
			this.renderDisplayVisible(sf, valueEl, rowEl);
		}

		getDisplayValueVisible(sf: Field, model): (() => boolean)
		{
			const name = sf._name;

			if (!name)
				return () => true;

			return () =>
			{
				const value = model[name];
				return !value || $clip(value());
			};
		}

		renderDisplayVisible(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const ctrl = sf._controller;

			ctrl.widgets[sf.uname()] = {
				valueVisible: ko.computed(this.getDisplayValueVisible(sf, ctrl.model)),
			};

			rowEl.attr("data-bind", `visible: widgets.${sf.uname() }.valueVisible`);
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.append(
				sf.getIconHtml() +
				`<span data-bind="text: $clip(r.${sf._name}())">`
			);
		}

		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			this.renderDisplay(sf, valueEl, rowEl);
		}

		pureRender(sf: Field, container: JQuery)
		{
			const valueEl = $(`<div>`);
			sf._type.render(sf, valueEl, valueEl);
			valueEl.appendTo(container);
		}

		protected appendEditor<TEditorOptions extends DevExpress.ui.dxTextEditorOptions>(sf: Field, valueEl: JQuery, widgetClassName: string, options: TEditorOptions)
		{
			var ctrl = <EditFormController>sf._controller;
			const sm = sf._member;


			options["hint"] = sm._title + (!sm._description ? "" : "\r\n" + sm._description);

			if (sm._isSubject)
				options.onValueChanged = () => ctrl.recalc(sf._name);

			const widgetOption = sf._widgetOptions["dxTextBox"];
			if (widgetOption)
				$.extend(options, widgetOption);

			ctrl.widgets[sf.uname()] = options;


			const editorEl = $("<div>");
			//editorEl[0]["sf"] = sf;

			let bindAttr = `${widgetClassName}: widgets.${sf.uname() }`;

			const rules = this.getValidationRules(sf);

			if (rules && rules.length)
			{
				ctrl.validators[sf.uname()] =
				{
					validationGroup: "edit-form",
					validationRules: rules
				};

				bindAttr += ", dxValidator: validators." + sf.uname();
			}


			editorEl
				.attr("data-bind", bindAttr)
				.appendTo(valueEl);
		}

		protected appendTextBoxEditor<TEditorOptions extends DevExpress.ui.dxTextBoxOptions>(sf: Field, valueEl: JQuery, widgetClassName: string, options: TEditorOptions)
		{
			const sm = sf._member;

			if (sf._controller.filterMode)
				options.mode = "search";

			if (sf._rowMode || sf._unlabel || sf._hideLabel)
				options.placeholder = sm._shortTitle || sf._title;

			const widgetOption = sf._widgetOptions["dxTextBox"];
			if (widgetOption)
				$.extend(options, widgetOption);

			//options.maxLength = sm._maxLength || undefined;

			this.appendEditor(sf, valueEl, widgetClassName, options);
		}

		//#endregion


		protected getValidationRules(sf: Field): any[]
		{
			const sm = sf._member; 
			//var se = sf.entity;

			var rules: any[] = [];

			if (this.nullable && sm._required && !sf._controller.filterMode)
			{
				rules.push({
					type: "required"
				});
			}

			if (sm._unique)
			{
				rules.push({
					type: "custom",
					message: "Уже существует",
					validationCallback: Validators.uniqueValidator,
				});
			}

			if ((sm._minLength || sm._maxLength) && !sm._enumType)
			{
				const msg =
					"Длина поля \"" + sm._title + "\" должна быть " +
					(sm._minLength === sm._maxLength
						? "равна " + sm._minLength
						: ko.as(sm._minLength, a => " от " + a, "") + ko.as(sm._maxLength, a => " до " + a, ""))
					+ ".";

				rules.push({
					//type: "stringLength",
					type: "custom",
					min: sm._minLength || undefined,
					max: sm._maxLength || undefined,
					message: msg,
					validationCallback: Validators.stringLength,
				});
			}

			return rules;
		}

	}


	export function getTextIconHtml(icon: string)
	{
		return !icon ? `` : `<i class="fa fa-${icon} fa-lg" /> &nbsp;`;
	}

}