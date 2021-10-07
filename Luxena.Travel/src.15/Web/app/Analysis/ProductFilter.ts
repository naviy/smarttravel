module Luxena.Views
{

	var fse = sd.ProductFilter;
	fse.MinIssueDate.titlePostfix(" с");
	fse.MaxIssueDate.ru("по");

	var date = new Date(), y = date.getFullYear(), m = date.getMonth();

	var filterModel = {
		IssueDate: ko.observable(date),
		IssueMonth: ko.observable(new Date(y, m, 1)),
		MinIssueDate: ko.observable(new Date(y, m, 1)),
		MaxIssueDate: ko.observable(new Date(y, m + 1, 0)),
	};


	export function NewProductFilterController(args, cfg?: {
		oneDayOnly?: boolean;
		oneMonth?: boolean;
	})
	{
		if (!cfg) cfg = {};

		const members: SemanticMember[] = [];

		if (cfg.oneMonth)
			members.push(fse.IssueMonth);
		else
		{
			members.push(fse.MinIssueDate, fse.MaxIssueDate);

			const minDate = filterModel.MinIssueDate;
			const maxDate = filterModel.MaxIssueDate;
			if (cfg.oneDayOnly && (!minDate() || minDate() !== maxDate()))
			{
				const date = new Date();
				minDate(date);
				maxDate(date);
			}
		}

		members.push(
			fse.Type,
			fse.Name,
			fse.State,
			fse.ProductCurrency,
			fse.Provider,
			fse.Customer,
			fse.Booker,
			fse.Ticketer,
			fse.Seller,
			fse.Owner
		);

		return new FilterFormController({
			entity: fse,
			args: args,
			model: filterModel,
			members: members,
		});
	}

}