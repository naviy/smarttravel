using Luxena.Travel.Specs.Support;

using TechTalk.SpecFlow;

namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	public class CompanySteps : TechTalk.SpecFlow.Steps
	{
		[StepDefinition(@"the user is going to begin to create Company")]
		public void GivenTheUserIsGoingToBeginToCreateCompany()
		{
			When(@"the user presses ""создать""");
			Then(@"the system opens form ""Организация (создание)""");
		}
	}
}
