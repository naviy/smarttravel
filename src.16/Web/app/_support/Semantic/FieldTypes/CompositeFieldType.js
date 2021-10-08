var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Luxena;
(function (Luxena) {
    var CompositeFieldType = (function (_super) {
        __extends(CompositeFieldType, _super);
        function CompositeFieldType() {
            _super.apply(this, arguments);
        }
        CompositeFieldType.prototype.addItemsToController = function (sm, ctrl, action) {
            sm._components = ctrl.addComponents(sm._members, null, function (sm2, sc2) {
                sc2.columnVisible(false);
                action && action(sm2, sc2);
            });
        };
        CompositeFieldType.prototype.getSelectFieldNames = function (sf) {
            return [];
        };
        CompositeFieldType.prototype.loadFromData = function (sf, model, data) {
        };
        CompositeFieldType.prototype.saveToData = function (sf, model, data) {
        };
        return CompositeFieldType;
    })(SemanticFieldType);
    Luxena.CompositeFieldType = CompositeFieldType;
})(Luxena || (Luxena = {}));
