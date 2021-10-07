module Luxena.Views
{

	export var Tests = () => ({
		viewShown: () =>
		{
			var test = new tsUnit.Test(Luxena.Tests);

			var result = test.run(new tsUnit.TestRunLimiterRunAll());

			test.showResults($("#results").get(0), result);
		},
	});

}