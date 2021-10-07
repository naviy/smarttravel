using Luxena.Travel.Specs.Support;

using TechTalk.SpecFlow;

namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	class AirportSteps : TechTalk.SpecFlow.Steps
	{
		[StepDefinition(@"the user is going to begin to create Airport")]
		public void GivenTheUserIsGoingToBeginToCreateAirport()
		{
			When(@"the user presses ""создать""");
			Then(@"the system opens form ""Аэропорт (создание)""");
		}
	}
}
