module Luxena.Tests
{

	//var se = sd.Product;

	export class FilterFormControllerTests extends tsUnit.TestClass
	{

		prepareFilterExpression01()
		{
			var filter = prepareFilterExpression({}, null);
			this.areIdentical(undefined, filter);
		}
		
		prepareFilterExpression02()
		{
			var filter = prepareFilterExpression({}, []);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression03()
		{
			var filter = prepareFilterExpression({}, [undefined, undefined]);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression04()
		{
			var filter = prepareFilterExpression({}, ["or"]);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression05()
		{
			var filter = prepareFilterExpression({}, ["or", undefined]);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression06()
		{
			var filter = prepareFilterExpression({}, [undefined, "or", undefined]);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression07()
		{
			var filter = prepareFilterExpression({}, ["or", undefined, "or", undefined]);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression08()
		{
			var filter = prepareFilterExpression({}, [undefined, "or", "or", undefined]);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression09()
		{
			var filter = prepareFilterExpression({}, ["or", "or"]);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression10()
		{
			var filter = prepareFilterExpression({}, ["and", undefined, "and", "and", undefined]);
			this.areIdentical(undefined, filter);
		}

		prepareFilterExpression11()
		{
			var filter = prepareFilterExpression({}, [1, "and", "and", 2]);
			this.areCollectionsIdentical([1, "and", 2], filter);
		}

		prepareFilterExpression12()
		{
			var filter = prepareFilterExpression({}, [1, "or"]);
			this.areCollectionsIdentical([1], filter);
		}

		prepareFilterExpression13()
		{
			var filter = prepareFilterExpression({}, [1, "or", undefined]);
			this.areCollectionsIdentical([1], filter);
		}

		prepareFilterExpression14()
		{
			var filter = prepareFilterExpression({}, [1, "or", null]);
			this.areCollectionsIdentical([1, "or", null], filter);
		}

		prepareFilterExpression15()
		{
			var filter = prepareFilterExpression({}, [1, "or", [undefined]]);
			this.areCollectionsIdentical([1], filter);
		}

		prepareFilterExpression16()
		{
			var filter = prepareFilterExpression({}, [1, "or", [undefined, "or", undefined]]);
			this.areCollectionsIdentical([1], filter);
		}

		prepareFilterExpression17()
		{
			var filter = prepareFilterExpression({}, [1, "or", [2, "or", undefined]]);
			this.areCollectionsIdentical([1, "or", [2]], filter);
		}

		prepareFilterExpression18()
		{
			var filter = prepareFilterExpression({}, [undefined, [2, "or"]]);
			this.areCollectionsIdentical([[2]], filter);
		}

		prepareFilterExpression19()
		{
			var filter = prepareFilterExpression({}, [undefined, undefined, undefined, [2, "or", undefined, undefined, ]]);
			this.areCollectionsIdentical([[2]], filter);
		}

		prepareFilterExpression20()
		{
			var filter = prepareFilterExpression({}, [undefined, undefined, undefined, [undefined, "or", 2, "or", undefined, undefined, ]]);
			this.areCollectionsIdentical([[2]], filter);
		}

		prepareFilterExpression21()
		{
			var filter = prepareFilterExpression({}, [undefined, undefined, undefined, [undefined, "or", ["2", "=", "2"], "or", undefined, undefined, ]]);
			this.areCollectionsIdentical([[["2", "=", "2"]]], filter);
		}

		prepareFilterExpression22()
		{
			var filter = prepareFilterExpression({}, [["Booker.Id", "=", "a87fdce4760c4d7ea7e686182c583cb1"], "or", "or"]);
			this.areCollectionsIdentical([["Booker.Id", "=", "a87fdce4760c4d7ea7e686182c583cb1"]], filter);
		}

	}
}
