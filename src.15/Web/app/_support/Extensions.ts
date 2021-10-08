declare var DX;


interface KnockoutStatic
{
	as<T, T2>(value: T, evaluator: (value) => T2, defaults?: T2);

	format(value: any, format: string, precision?: number): string;

	unwrap2(obj: any): any;
}


ko.as = (value, evaluator, defaults?) =>
{
	if (!value)
		return defaults !== undefined ? defaults : null;

	value = ko.unwrap(value);

	if (!value)
		return defaults !== undefined ? defaults : null;

	if (!evaluator)
		return value;

	return evaluator(value);
};


ko.format = (value, format, precision) => DevExpress["formatHelper"].format(value, format, precision);


ko.unwrap2 = obj =>
{
	if (!obj) return obj;

	obj = ko.unwrap(obj);

	var r = {};
	var propCount = 0;

	for (var name in obj)
	{
		if (!obj.hasOwnProperty(name)) continue;

		r[name] = ko.unwrap(obj[name]);

		propCount++;
	}

	return propCount > 0 ? r : obj;
};


interface KnockoutBindingHandlers
{
	renderer: KnockoutBindingHandler;
	buttonsCol: KnockoutBindingHandler;
	pre: KnockoutBindingHandler;
}


ko.bindingHandlers.renderer =
{
	update(element: JQuery, valueAccessor, allBindings)
	{
		var value = ko.unwrap(valueAccessor());
		if (!value) return;

		element = $(element);

		element.html("");

		if ($.isFunction(value))
			value(element);
		else
		{
			for (var containerId in value)
			{
				if (!value.hasOwnProperty(containerId)) continue;

				var container = value[containerId];

				if ($.isFunction(value))
					container(element);
				else if (container.renderer)
					container.renderer(element);
			}
		}
	}
};


ko.bindingHandlers.buttonsCol =
{
	update(element, valueAccessor, allBindings)
	{
		var buttons: DevExpress.ui.dxButtonOptions[] = ko.unwrap(valueAccessor());
		if (!buttons) return;

		var jelement = $(element);

		jelement.html("");

		buttons.forEach(btn =>
		{
			$(`<div class="smart-action-button">`)
				.dxButton(btn)
				.appendTo(jelement);
		});
	}
};

ko.bindingHandlers.pre =
{
	update(element, valueAccessor, allBindings)
	{
		const value = ko.unwrap(valueAccessor());
		if (!value) return;
		$(element).append($("<pre>").text(value));
	}
};