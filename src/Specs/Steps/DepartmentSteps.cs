using TechTalk.SpecFlow;

namespace Luxena.Travel.Specs.Steps
{
	//#warning Resolve the Department creation workflow.
	//[Binding]
	public class DepartmentSteps : TechTalk.SpecFlow.Steps
	{
		[Given(@"the user is going to begin to create Department")]
		public void GivenTheUserIsGoingToBeginToCreateDepartment()
		{
			When(@"the user presses ""создать""");
			Then(@"the system opens form ""Подразделения (создание)""");
		}
	}
}
