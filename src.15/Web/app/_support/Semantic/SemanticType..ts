module Luxena
{

	export class SemanticType
	{
		static charWidth: number = 10;
		static digitWidth: number = 7.6;

		charWidth: number;

		isComplex = false;

		allowFiltering = true;
		allowGrouping = true;
		chartDataType: string;
		dataType = "string";
		format: string;
		length: number;

		addColumnFilterWidth: number;
		nullable = true;


		getSelectFieldNames(sf: SemanticField): string[]
		{
			return [sf._name];
		}

		getExpandFieldNames(sf: SemanticField): string[]
		{
			return [];
		}

		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			return [sm._name, operation || "=", value];
		}


		//#region Controls

		toGridColumns(sf: SemanticField): DevExpress.ui.dxDataGridColumn[]
		{
			return [this.toStdGridColumn(sf)];
		}

		toStdGridColumn(sf: SemanticField): DevExpress.ui.dxDataGridColumn
		{
			const sm = sf.member;
			const se = sf._entity;
			const cfg = (<GridController>sf._controller).config;

			return {
				allowFiltering: sm._allowFiltering && this.allowFiltering && !sm._isCalculated,
				allowGrouping: sm._allowGrouping && this.allowGrouping && !sm._isCalculated,
				allowSorting: !sm._isCalculated,
				caption: sm._title,
				dataField: sm._name,
				dataType: this.dataType,
				fixed: sm._columnFixed || (cfg.wide || se._isWide) && sm._kind === SemanticMemberKind.Primary,
				format: sm._format || this.format,
				sortOrder: sf.sortOrder || sm._sortOrder,
				width: this.getColumnWidth(sf),
				visible: sm._columnVisible,

				calculateFilterExpression: (value, operation) => this.getFilterExpr(sm, value, operation),
			};
		}

		toGridTotalItems(sf: SemanticField)
		{
			return [];
		}



		getLength(sf: SemanticField): { length: number; min: number; max: number }
		{
			const sm = sf.member;
			let length = sm._length || sm._maxLength;
			let minLength = sm._minLength;
			let maxLength = sm._maxLength;

			if (!length)
			{
				var refs = sm.getReference();
				if (refs)
				{
					if (!refs._nameMember)
					{
						console.log(sm);
						throw Error("SemanticMember._nameMember is null");
					}

					length = refs._nameMember._length;
					minLength = refs._nameMember._minLength;
					maxLength = refs._nameMember._maxLength;
				}
			}

			return {
				length: length || sm._length || this.length,
				min: minLength || sm._minLength,
				max: maxLength || sm._maxLength,
			};

		}

		getColumnWidth(sf: SemanticField, length?: number): number
		{
			const sm = sf.member;

			if (!length)
				length = this.getLength(sf).length;

			if (length < 2)
				length = 2;

			const cfg = (<GridController>sf._controller).config;

			return sm._width || (
				14
				+ Math.round((this.charWidth || SemanticType.charWidth) * length)
				+ (cfg.useFilter && cfg.useFilterRow && this.allowFiltering && sm._allowFiltering ? 12 + (this.addColumnFilterWidth || 0) : 0)
			);
		}

		//#endregion


		getFromData(sm: SemanticMember, data: any): any
		{
			return data[sm._name];
		}

		getModel(model: any, name: string): any
		{
			return model[name];
		}

		setModel(model: any, sname: string|SemanticField, value: any): void
		{
			var name: string, sf: SemanticField = null;
			if (typeof sname === "string")
			{
				name = sname;
			}
			else
			{
				sf = sname;
				name = sf._name;
			}

			var existsValue = model[name];
				
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

		loadFromData(sf: SemanticField, model: any, data: any): void
		{
			var value = data[sf._name];
			this.setModel(model, sf, value);
		}

		saveToData(sf: SemanticField, model: any, data: any): void
		{
			var name = sf._name;
			data[name] = ko.unwrap(model[name]);
		}

		removeFromData(sf: SemanticField|SemanticMember, data: any): void
		{
			delete data[sf["name"] || sf["_name"]];
		}


		getFieldLabel(sf: SemanticField)
		{
			return sf.member._title + ":";
		}

		render(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (sf._controller.editMode && !sf.member._isReadOnly)
				this.renderEditor(sf, valueEl, rowEl);
			else
				this.renderDisplay(sf, valueEl, rowEl);
		}

		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (!sf._controller.editMode)
				rowEl.attr("data-bind", "visible: r." + sf._name);

			valueEl.attr("data-bind", "text: r." + sf._name);
		}

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			this.renderDisplay(sf, valueEl, rowEl);
		}

		protected appendEditor<TEditorOptions extends DevExpress.ui.dxTextEditorOptions>(sf: SemanticField, valueEl: JQuery, widgetClassName: string, options: TEditorOptions)
		{
			var ctrl = <EditFormController>sf._controller;
			const sm = sf.member;

			const editorEl = $("<div>");
			editorEl[0]["sf"] = sf;

			options["hint"] = sm._title + (!sm._description ? "" : "\r\n" + sm._description);

			if (sm._isSubject)
				options.onValueChanged = () => ctrl.recalc(sf._name);

			ctrl.widgets[sf._name] = options;

			var bindAttr = `${widgetClassName}: $root.widgets.${sf._name}`;

			var rules = this.getValidationRules(sf);

			if (rules && rules.length)
			{
				ctrl.validators[sf._name] =
				{
					validationGroup: "edit-form",
					validationRules: rules
				};

				bindAttr += ", dxValidator: $root.validators." + sf._name;
			}


			editorEl
				.attr("data-bind", bindAttr)
				.appendTo(valueEl);
		}
		
		protected appendTextBoxEditor<TEditorOptions extends DevExpress.ui.dxTextBoxOptions>(sf: SemanticField, valueEl: JQuery, widgetClassName: string, options: TEditorOptions)
		{
			const sm = sf.member;

			if (sf._controller.filterMode)
				options.mode = "search";

			if (sf._rowMode)
				options.placeholder = sm._shortTitle || sm._title;

			//options.maxLength = sm._maxLength || undefined;

			this.appendEditor(sf, valueEl, widgetClassName, options);
		}


		protected getValidationRules(sf: SemanticField): any[]
		{
			const sm = sf.member; 
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