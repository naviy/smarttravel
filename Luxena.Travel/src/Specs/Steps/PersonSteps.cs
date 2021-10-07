using Luxena.Travel.Specs.Support;

using TechTalk.SpecFlow;

namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	public class PersonSteps : TechTalk.SpecFlow.Steps
	{
		public PersonSteps(UserInterface ui)
		{
			_ui = ui;
		}

		[StepDefinition(@"the user is going to begin to create Person")]
		public void GivenTheUserIsGoingToBeginToCreatePerson()
		{
			When(@"the user presses ""создать""");
			Then(@"the system opens form ""Персона (создание)""");
		}

		[When(@"the user presses ""создать"" to create mileage card")]
		[Then(@"the user presses ""создать"" to create mileage card")]
		public void WhenTheUserPressesСоздатьToCreateMileageCard()
		{
			_ui.Scope.PressButtonAbove("Мильная карточка", "создать");
		}

		[When(@"the user presses ""создать"" to create passport")]
		public void WhenTheUserPressesСоздатьToCreatePassport()
		{
			_ui.Scope.PressButtonAbove("Паспорт", "создать");
		}

		[When(@"the user select from list person ""(.*)""")]
		public void WhenTheUserSelectFromListPerson(string person)
		{
			_ui.FilterList(person); ;
		}

		private readonly UserInterface _ui;
	}
}
