module Luxena
{


	export class CurrencyCodeSemanticType extends SemanticType
	{
		static CurrencyCode: CurrencyCodeSemanticType = new CurrencyCodeSemanticType();

		static Codes = ["UAH", "USD", "EUR", "RUB", "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AUH", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV", "BRL", "BSD", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EEK", "EGP", "ERN", "ETB", "FJD", "FKP", "GBP", "GEL", "GHC", "GHS", "GIP", "GMD", "GNF", "GTQ", "GWP", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "INR", "IQD", "IRR", "ISK", "IUA", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MTL", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZM", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NUC", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "ROL", "RON", "RSD", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SIT", "SKK", "SLL", "SOS", "SRD", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UGX", "USN", "USS", "UYI", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XOF", "XPD", "XPF", "XPT", "XTS", "XXX", "YER", "ZAR", "ZMK", "ZWD", "ZWL", ];


		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			this.appendEditor(sf, valueEl, "dxSelectBox", <DevExpress.ui.dxSelectBoxOptions>{
				value: sf.getModelValue(),
				dataSource: CurrencyCodeSemanticType.Codes,
			});
		}
	}


	export class MoneySemanticType extends SemanticType
	{
		static Money: MoneySemanticType = new MoneySemanticType();

		isComplex = true;
		dataType = "number";
		length = 12;
		allowGrouping = false;


		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			return [sm._name + ".Amount", operation || "=", value];
		}

		loadFromData(sf: SemanticField, model: any, data: any): void
		{
			var name = sf._name;
			var newValue = data[name];

			var value = model[name];
			if (value)
			{
				value.Amount(newValue && newValue.Amount);
				value.CurrencyId(newValue && newValue.CurrencyId);
			}
			else
			{
				model[name] = {
					Amount: ko.observable(newValue && newValue.Amount),
					CurrencyId: ko.observable(newValue && newValue.CurrencyId),
				};
			}
		}

		saveToData(sf: SemanticField, model: any, data: any): void
		{
			var value = model[sf._name];

			data[sf._name] = {
				Amount: ko.unwrap(value.Amount),
				CurrencyId: ko.unwrap(value.CurrencyId),
			};
		}


		toGridColumns(sf: SemanticField): DevExpress.ui.dxDataGridColumn[]
		{
			var sm = sf.member;

			var col = this.toStdGridColumn(sf);

			col.dataField += ".Amount";

			col.calculateCellValue = data => moneyToString(data[sm._name]);

			return [col];
		}

		renderDisplay(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			if (!sf._controller.editMode)
				rowEl.attr("data-bind", "visible: r." + sf._name + " && r." + sf._name + ".Amount()");

			$("<div>")
				.addClass("money-display-row")
				.attr("data-bind", "text: Luxena.moneyToString(r." + sf._name + ")")
				.appendTo(valueEl);
		}

		renderEditor(sf: SemanticField, valueEl: JQuery, rowEl: JQuery)
		{
			var sm = sf.member;

			if (sm._isCalculated || sm._isReadOnly)
			{
				if (valueEl.hasClass("dx-field-value"))
				{
					valueEl.removeClass("dx-field-value");
					valueEl.addClass("dx-field-value-static");
				}

				this.renderDisplay(sf, valueEl, rowEl);

				return;
			}

			var valueBox = $("<div>")
				.addClass("money-editor-row")
				.appendTo(valueEl);

			var amountDiv = $("<div>")
				.appendTo(valueBox);

			var currencyDiv = $("<div>")
				.appendTo(valueBox);

			var value = sf.getModelValue();

			var sf2 = $.extend({}, sf);

			sf2.name = sf._name + "_amount";
			this.appendEditor(sf2, amountDiv, "dxNumberBox", <DevExpress.ui.dxNumberBoxOptions>{
				value: value.Amount,
			});
			 
			sf2.name = sf._name + "_currency";
			this.appendEditor(sf, currencyDiv, "dxSelectBox", <DevExpress.ui.dxSelectBoxOptions>{
				value: value.CurrencyId,
				dataSource: CurrencyCodeSemanticType.Codes,
			});
		}
	}

	export function moneyToString(v: { Amount: number; CurrencyId: string; }): string
	{
		if (!v) return "";

		var amount = v.Amount && ko.unwrap(v.Amount);
		return !amount /*&& amount !== 0*/ ? "" : Globalize.format(amount, "n2") + " " + ko.unwrap(v.CurrencyId);
	}

}