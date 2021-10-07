module Luxena.Validators
{

	export interface ValidatorOptions
	{
		value: string|number;
		rule: {
			isValid: boolean;
			message: string;
		};
		validator: DevExpress.ui.dxValidator;
	}


	export function uniqueValidator(params: ValidatorOptions)
	{
		var sf = params.validator.element()[0]["sf"];

		var se = sf.entity;
		var sm = sf._member;
		var id = sf.controller.getId();

		var filter = id
			? [[sm._name, '=', params.value], [sm._entity._lookupFields.id, '<>', id]]
			: [sm._name, '=', params.value];

		se._store.createQuery({})
			.filter(filter)
			.select(sm._entity._lookupFields.id)
			.enumerate()
			.done(data =>
			{
				params.rule.isValid = data && data.length === 0;
				params.validator.validate();
			});

		return true;
	}

	export function stringLength(params: ValidatorOptions)
	{
		var rule = params.rule;

		var value = <string>params.value;
		if (!value)
		{
			rule.isValid = true;
			return true;
		}
		var min = rule["min"];
		var max = rule["max"];

		if (min !== undefined && value.length < min || max !== undefined && value.length > max)
			return false;

		rule.isValid = true;
		return true;
	}

}