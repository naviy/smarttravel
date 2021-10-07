using Luxena.Travel.Specs.Support;

using TechTalk.SpecFlow;

namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	class CountrySteps : TechTalk.SpecFlow.Steps
	{
		[StepDefinition(@"the user is going to begin to create Country")]
		public void GivenTheUserIsGoingToBeginToCreateCountry()
		{
			When(@"the user presses ""создать""");
			Then(@"the system opens form ""Страна (создание)""");
		}
	}
}
