module Luxena.FieldTypes
{

	export class CurrencyCode extends SemanticFieldType
	{
		static CurrencyCode = new CurrencyCode();

		static Codes = ["UAH", "USD", "EUR", "RUB", "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AUH", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV", "BRL", "BSD", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EEK", "EGP", "ERN", "ETB", "FJD", "FKP", "GBP", "GEL", "GHC", "GHS", "GIP", "GMD", "GNF", "GTQ", "GWP", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "INR", "IQD", "IRR", "ISK", "IUA", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MTL", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZM", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NUC", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "ROL", "RON", "RSD", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SIT", "SKK", "SLL", "SOS", "SRD", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UGX", "USN", "USS", "UYI", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XOF", "XPD", "XPF", "XPT", "XTS", "XXX", "YER", "ZAR", "ZMK", "ZWD", "ZWL",];


		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			this.appendEditor(sf, valueEl, "dxSelectBox", <DevExpress.ui.dxSelectBoxOptions>{
				value: sf.getModelValue(),
				dataSource: CurrencyCode.Codes,
			});
		}
	}


	export class Money extends SemanticFieldType
	{
		static Money = new Money();

		dataType = "number";
		length = 12;
		allowGrouping = false;


		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			return [sm._name + ".Amount", operation || "=", value];
		}

		loadFromData(sf: Field, model: any, data: any): void
		{
			const name = sf._name;
			const newValue = data[name];

			const value = model[name];
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

		saveToData(sf: Field, model: any, data: any): void
		{
			const value = model[sf._name];

			data[sf._name] = {
				Amount: ko.unwrap(value.Amount),
				CurrencyId: ko.unwrap(value.CurrencyId),
			};
		}


		toGridColumns(sf: Field): DevExpress.ui.dxDataGridColumn[]
		{
			//var sm = sf.member;
			const col = this.toStdGridColumn(sf);
			col.dataField += ".Amount";
			//col.calculateCellValue = data => moneyToString(data[sm._name]);
			return [col];
		}

		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			let value = data[sf._name];
			if (!value) return;

			value = moneyToString(value);

			if (sf._member._kind === SemanticMemberKind.Important)
				value = `<b>${value}</b>`;

			container.append(value);
		}

		getDisplayValueVisible(sf: Field, model)
		{
			return () =>
			{
				var value = model[sf._name];
				return value && value.Amount();
			};
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			$("<div>")
				.addClass("money-display-row")
				.attr("data-bind", "text: Luxena.moneyToString(r." + sf._name + ")")
				.appendTo(valueEl);
		}

		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const sm = sf._member;

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

			const valueBox = $("<div>")
				.addClass("money-editor-row")
				.appendTo(valueEl);

			const amountDiv = $("<div>")
				.appendTo(valueBox);

			const currencyDiv = $("<div>")
				.appendTo(valueBox);

			const value = sf.getModelValue();

			let sf2 = $.extend({}, sf);
			sf2._name += "_amount";
			this.appendEditor(sf2, amountDiv, "dxNumberBox", <DevExpress.ui.dxNumberBoxOptions>{
				value: value.Amount,
			});

			sf2 = $.extend({}, sf);
			sf2._name += "_currency";
			this.appendEditor(sf2, currencyDiv, "dxSelectBox", <DevExpress.ui.dxSelectBoxOptions>{
				value: value.CurrencyId,
				dataSource: CurrencyCode.Codes,
			});
		}
	}
}


module Luxena
{

	export function moneyToString(v: { Amount: number; CurrencyId: string; }): string
	{
		if (!v) return "";

		const amount = v.Amount && ko.unwrap(v.Amount);
		return !amount /*&& amount !== 0*/ ? "" : Globalize.format(amount, "n2") + " " + (ko.unwrap(v.CurrencyId) || "");
	}

}