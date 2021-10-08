var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Luxena;
(function (Luxena) {
    var BoolSetFieldType = (function (_super) {
        __extends(BoolSetFieldType, _super);
        function BoolSetFieldType() {
            _super.apply(this, arguments);
            this.allowFiltering = false;
            this.allowGrouping = false;
            this.allowSorting = false;
            this.addColumnFilterWidth = -8;
        }
        //loadFromData(sf: Field, model: any, data: any): void
        //{
        //	const sm = sf.member;
        //	if (!sf._controller.editMode || !sm._enumIsFlags)
        //	{
        //		let value = data[sf._name];
        //		if (sf._controller.editMode && sm._required && !value)
        //			value = sm._enumType._array[0].Id;
        //		this.setModel(model, sf, value);
        //	}
        //	else
        //	{
        //		let values = data[sf._name];
        //		values = !values ? [] : values.split(",").map(a => a.trim());
        //		this.setModel(model, sf, values);
        //	}
        //}
        //saveToData(sf: Field, model: any, data: any): void
        //{
        //	if (!sf.member._enumIsFlags)
        //	{
        //		super.saveToData(sf, model, data);
        //		return;
        //	}
        //	var values = ko.unwrap(model[sf._name]);
        //	data[sf._name] = values.join(", ");
        //}
        //toGridColumns(sf: Field): DevExpress.ui.dxDataGridColumn[]
        //{
        //	var sm = sf.member;
        //	const col = this.toStdGridColumn(sf);
        //	if (sm._enumIsFlags)
        //	{
        //		col.allowFiltering = false;
        //		col.allowGrouping = false;
        //		col.allowSorting = false;
        //		col.calculateCellValue = data => getEnumNames(sm._enumType, data[sm._name]);
        //	}
        //	else
        //	{
        //		col.lookup = {
        //			dataSource: sm._enumList || sm._enumType._array,
        //			valueExpr: "Id",
        //			displayExpr: "Name",
        //			allowClearing: sm._required,
        //		};
        //		col.cellTemplate = (cell, cellInfo) =>
        //			cell.html(getEnumNames(sm._enumType, cellInfo.value));
        //		//col.calculateCellValue = data => getEnumNames(sm._enumType, data[sm._name]);
        //		col.calculateGroupValue = data => sm._enumType._getEdm(data[sm._name]);
        //		col.groupCellTemplate = (cell: JQuery, cellInfo) =>
        //			cell.html(sm._title + ": &nbsp; " + getEnumNames(sm._enumType, cellInfo.value));
        //	}
        //	return [col];
        //}
        //appendDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
        //{
        //	valueEl.attr("data-bind", "html: Luxena.getEnumNames(Luxena." + sf.member._enumType._name + ", r." + sf._name + ")");
        //}
        BoolSetFieldType.prototype.renderEditor = function (sf, valueEl, rowEl) {
            var sm = sf.member;
            this.appendTextBoxEditor(sf, valueEl, "dxTagBox", {
                values: sf.getModelValue(),
                items: sm._enumType._array,
                valueExpr: "Id",
                displayExpr: "Name",
                showClearButton: !sm._required,
            });
        };
        BoolSetFieldType.BoolSet = new BoolSetFieldType();
        return BoolSetFieldType;
    })(SemanticFieldType);
    Luxena.BoolSetFieldType = BoolSetFieldType;
})(Luxena || (Luxena = {}));
