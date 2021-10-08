namespace Luxena.Components_
{

	export type FieldBox = FieldBox$<any, any>;


	export abstract class FieldBox$<
		TOptions extends FieldBoxOptions,
		TOptionsSetter extends FieldOptionsBoxSetter<any>
	>
		extends Component$<TOptions, TOptionsSetter>
	{
		//_entity: SemanticEntity;
		_dataType: string;

		
		//#region Render

		//prepare()
		//{
		//	super.prepare();

		//	this._renderOnRepaint = !this._options.editMode;
		//}

		render()
		{
			return $logb(`${this.__name}.render()`, () =>
			{
				if (this._options.editMode)
					return this.renderEditor();
				else
					return this.renderDisplay();
			});
		}

		renderDisplay(): JQuery { return undefined; }

		renderEditor(): JQuery { return undefined; }

		toGridColumns(): DevExpress.ui.dxDataGridColumn[]
		{
			return [this.toStdGridColumn()];
		}

		toStdGridColumn()
		{
			const o = this._options;

			const col = <DevExpress.ui.dxDataGridColumn>{
				allowFiltering: o.allowFiltering,
				allowGrouping: o.allowGrouping,
				allowSorting: o.allowSorting,
				caption: o.title,
				dataField: this._options.name,
				dataType: this._dataType,
				//fixed: sm._columnFixed || (cfg.wide || se._isWide) && sm._kind === SemanticMemberKind.Primary,
				//format: sm._format || this.format,
				//groupIndex: sf._groupIndex !== undefined ? sf._groupIndex : sm._groupIndex,
				//sortOrder: sf.sortOrder || sm._sortOrder,
				//width: sf.getWidth(),
				//visible: sf._columnVisible && sm._columnVisible,

				//calculateFilterExpression: (value, operation) => this.getFilterExpr(sm, value, operation),
			};

			//col.cellTemplate = (cell: JQuery, cellInfo) =>
			//{
			//	this.renderDisplayStatic(sf, cell, cellInfo.data);
			//};

			return col;
		}

		toGridTotalItems(): any[]
		{
			return [];
		}

		
		//#endregion


		//#region Data

		loadOptions()
		{
			return {
				select: [this._options.name],
			};
		}

		filterExpr(value: any, operation?: string): [string, string, any]
		{
			return [this._options.name, operation || "=", value];
		}

		dataValue(value?: any)
		{
			const data = this._dataSet._current;
			if (!data) return undefined;

			if (value === undefined)
				return this.getDataValue(data, this._options.name);
			else
			{
				this.setDataValue(data, this._options.name, value);
				return undefined;
			}
		}

		protected getDataValue(data: any, name: string)
		{
			return data[name];
		}

		protected setDataValue(data: any, name: string, value: any)
		{
			data[name] = value;
		}


		//getFieldValue()
		//{
		//	if (this._editor)
		//		return this._editor.option("value");

		//	return undefined;
		//}

		//setFieldValue(value: any)
		//{
		//	if (this._display)
		//		this._display.html(this.valueToHtml(value));
		//	else if (this._editor)
		//		return this._editor.option("value", value);
		//}

		valueToHtml(value)
		{
			return value === undefined || value === null ? "" : value + "";
		}



		//hasValue(): boolean
		//{
		//	if (this._display)
		//		return !!this._display.html();
		//	else if (this._editor)
		//		return !!this._editor.option("value");

		//	return undefined;
		//}


		//loadFromData(data: any): void
		//{
		//	const value = this.getDataValue(data);
		//	this.setFieldValue(value);
		//}

		//saveToData(data: any): void
		//{
		//	const value = this.getFieldValue();
		//	this.setDataValue(data, value);
		//}

		removeFromData(data: any): any
		{
			delete data[this._options.name];
		}

		//#endregion

	}

}