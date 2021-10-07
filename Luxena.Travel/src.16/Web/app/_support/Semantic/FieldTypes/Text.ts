module Luxena.FieldTypes
{

	export class BaseTextFieldType extends SemanticFieldType
	{
		mode: string;
		maxLength: number;
		mask: string;

		getFilterExpr(sm: SemanticMember, value: any, operation?: string): [string, string, any]
		{
			return [sm._name, operation || "contains", value];
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.addClass("pre");
			super.renderDisplayBind(sf, valueEl, rowEl);
		}

		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			this.appendTextBoxEditor(sf, valueEl, "dxTextBox", <DevExpress.ui.dxTextBoxOptions>{
				value: sf.getModelValue(),
				mask: this.mask,
				mode: this.mode,
				maxLength: this.maxLength,
			});
		}

	}


	export class TextArea extends BaseTextFieldType
	{
		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const sm = sf._member;

			this.appendTextBoxEditor(sf, valueEl, "dxTextArea", <DevExpress.ui.dxTextAreaOptions>{
				value: sf.getModelValue(),
				height: sm._lineCount ? <any>(sm._lineCount * 46 - 10) : undefined,
			});
		}
	}


	export class CodeTextArea extends BaseTextFieldType
	{
		renderEditor(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			const sm = sf._member;

			this.appendTextBoxEditor(sf, valueEl, "dxTextArea", <DevExpress.ui.dxTextAreaOptions>{
				value: sf.getModelValue(),
				height: sm._lineCount ? <any>(sm._lineCount * 64 - 16 - 12) : undefined,
			});
		}
	}


	export class Password extends BaseTextFieldType
	{
		maxLength = 20;
		mode = "password";
	}


	export class Address extends TextArea
	{
		_icon = "map-marker";

		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			const value = $clip(data[sf._name]);
			if (!value) return;

			const evalue = encodeURI(value);
			container.addClass("pre").append(
				sf._member.getIconHtml() +
				`<span>${value}</span>&nbsp; ` +
				`<span class="nowrap">` +
				`<a title="Yandex Maps" target="_blank" href="http://maps.yandex.ru/?text=${evalue}" class="icon-yandex-map"></a> ` +
				`<a title="Google Maps" target="_blank" href="http://maps.google.com/maps?q=${evalue}" class="icon-google-map"></a>` +
				`</span>`
			);
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.addClass("pre").append(
				sf._member.getIconHtml() +
				`<span data-bind="text: r.${sf._name}"></span> &nbsp;` +
				`<span class="nowrap">` +
				`<a title="Yandex Maps" target="_blank" data-bind="attr: { href: 'http://maps.yandex.ru/?text=' + encodeURI(r.${sf._name}()) }" class="icon-yandex-map"></a> ` +
				`<a title="Google Maps" target="_blank" data-bind="attr: { href: 'http://maps.google.com/maps?q=' + encodeURI(r.${sf._name}()) }" class="icon-google-map"></a>` +
				`</span>`
			);
		}
	}


	export class Email extends BaseTextFieldType
	{
		mode = "email";
		_icon = "envelope";
		_singleLine = true;

		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			const value = $clip(data[sf._name]);
			if (!value) return;

			container.addClass("nowrap");
			container.append(
				`<span class="nowrap">${sf._member.getIconHtml()}<a class="dx-link" href="mailto:${value}" target="_blank">${value}</a></span>`
			);
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.addClass("nowrap");
			valueEl.append(
				sf._member.getIconHtml() +
				`<a data-bind="text: r.${sf._name}, attr: { href: 'mailto:' + r.${sf._name}() }"></a>`
			);
		}
	}

	export class Phone extends BaseTextFieldType
	{
		mode = "tel";
		_icon = "phone";
		_singleLine = true;
		//mask = "+38 \\000 000 00 00";
	}

	export class Fax extends BaseTextFieldType
	{
		mode = "tel";
		_icon = "fax";
		_singleLine = true;
	}


	export class Hyperlink extends BaseTextFieldType
	{
		mode = "url";
		_icon = "globe";

		renderDisplayStatic(sf: Field, container: JQuery, data)
		{
			const value = toExternalUrl(data[sf._name]);
			if (!value) return;

			container.addClass("nowrap");
			container.append(
				sf._member.getIconHtml() +
				`<a class="dx-link" href="${value}" target="_blank">${value}</a>`
			);
		}

		renderDisplayBind(sf: Field, valueEl: JQuery, rowEl: JQuery)
		{
			valueEl.addClass("nowrap");
			valueEl.append(
				sf._member.getIconHtml() +
				`<a data-bind="text: r.${sf._name}, attr: { href: Luxena.toExternalUrl(r.${sf._name}()) }" target="_blank"></a>`
			);
		}
	}

	export class Text extends BaseTextFieldType
	{
		static String = new Text();
		static Text = new TextArea();
		static CodeText = new CodeTextArea();
		static Password = new Password();
		static Address = new Address();
		static Email = new Email();
		static Phone = new Phone();
		static Fax = new Fax();
		static Hyperlink = new Hyperlink();


		toGridColumns(sf: Field): DevExpress.ui.dxDataGridColumn[]
		{
			const sm = sf._member;
			var se = sm._entity;

			const col = this.toStdGridColumn(sf);

			if (sm._isEntityName)
			{
				col.width = Math.round(col.width * 1.29);
				col.cellTemplate = (cell: JQuery, cellInfo) =>
				{
					if (cellInfo.column.groupIndex !== undefined) return;

					const data = <ISemanticRowData>cellInfo.data;
					const v = data[sf._name];

					if (!v) return;

					const id = data[se._lookupFields.id];

					cell.append(
						`<a class="dx-link entity-name-cell" href="#${data._viewEntity._name}/${id}">${v}</a>`
					);
				};
			}

			return [col];
		}

	}

}


module Luxena
{

	export function toExternalUrl(url: string)
	{
		url = $clip(url);
		if (!url) return "";

		if (url.indexOf("://") < 0)
			url = "http://" + url;

		url = url.replace(`"`, "");

		return url;
	}

}