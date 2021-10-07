using Luxena.Travel.Specs.Support;

using NUnit.Framework;

using TechTalk.SpecFlow;

namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	class PaymentSystemSteps : TechTalk.SpecFlow.Steps
	{
		[StepDefinition(@"the user is going to begin to create Payment System")]
		public void GivenTheUserIsGoingToBeginToCreatePaymentSystem()
		{
			When(@"the user presses ""создать""");
			Then(@"the system opens form ""Платежная система (создание)""");
		}
	}
}
