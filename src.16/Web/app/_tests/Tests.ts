module Luxena.Views
{

	export var Tests = () => ({
		viewShown: () =>
		{
			var test = new tsUnit.Test(Luxena.Tests);

			test.run(new tsUnit.TestRunLimiterRunAll());

			test.showResults($("#results")[0]);
		},
	});

}