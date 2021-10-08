using Luxena.Travel.Specs.Support;

using TechTalk.SpecFlow;

namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	class CurrencySteps : TechTalk.SpecFlow.Steps
	{
		[StepDefinition(@"the user is going to begin to create Currency")]
		public void GivenTheUserIsGoingToBeginToCreateCurrency()
		{
			When(@"the user presses ""создать""");
			Then(@"the system opens form ""Валюта (создание)""");
		}
	}
}
