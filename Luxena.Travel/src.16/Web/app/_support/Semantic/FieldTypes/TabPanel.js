var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Luxena;
(function (Luxena) {
    var FieldTypes;
    (function (FieldTypes) {
        var TabPanel = (function (_super) {
            __extends(TabPanel, _super);
            function TabPanel() {
                _super.apply(this, arguments);
            }
            TabPanel.prototype.prerender = function (sf) {
                if (sf._unlabel === undefined)
                    sf._unlabel = true;
            };
            TabPanel.prototype.render = function (sf, valueEl, rowEl) {
                var unlabelItems = sf._itemUnlabel || !sf._unlabel && !sf._labelAsHeader;
                var mustPureRender = sf._controller.viewMode && unlabelItems;
                if (mustPureRender) {
                    rowEl.addClass("field-label-none");
                    valueEl.addClass("dx-field-value-static");
                }
                var accEl = $("<div data-bind=\"dxTabPanel: { items: [{ title: '" + (sf._title || "") + "'" + (sf._icon ? ", icon: 'fa fa-" + sf._icon : "") + " }] }\"></div>");
                var itemEl = $("<div data-options=\"dxTemplate: { name: 'item' } \"></div>").appendTo(accEl);
                sf._components.forEach(function (sc2) {
                    if (sf._itemLabelAsHeader)
                        sc2.labelAsHeader();
                    else if (unlabelItems) {
                        sc2.unlabel();
                        if (mustPureRender)
                            sc2._mustPureRender = true;
                    }
                    sc2.render(itemEl);
                });
                valueEl.append(accEl);
            };
            TabPanel.TabPanel = new TabPanel();
            return TabPanel;
        })(CompositeFieldType);
        FieldTypes.TabPanel = TabPanel;
    })(FieldTypes = Luxena.FieldTypes || (Luxena.FieldTypes = {}));
})(Luxena || (Luxena = {}));
