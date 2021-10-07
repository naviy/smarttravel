using Luxena.Travel.Specs.Support;

using NUnit.Framework;

using TechTalk.SpecFlow;


namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	public class AirlinesCatalogSteps : TechTalk.SpecFlow.Steps
	{
		public AirlinesCatalogSteps(UserInterface ui)
		{
			_ui = ui;
		}

		[Given(@"airline ""AEROSVIT AIRLINES"" with ""VV"" IATA code and ""870"" prefix code exists")]
		public void GivenAerosvitAirlinesExists()
		{
			_ui.FilterList("AEROSVIT AIRLINES");

			Assert.AreNotEqual(0, _ui.ListCount());

			_ui.FilterList("");
		}

		[When(@"the user tries to create ""(.*)"" airline with ""(.*)"" IATA code and ""(.*)"" prefix code")]
		public void TheUserTriesToCreateAirline(string name, string iata, string prefix)
		{
			When(@"the user presses ""создать""");
			Then(@"the system opens form ""Авиакомпания (создание)""");
			var table = new Table("Label", "Value");
			table.AddRow("Название", name);
			table.AddRow("IATA код", iata);
			table.AddRow("Prefix код", prefix);
			table.AddRow("Требование паспортных данных", "По умолчанию");
			When("the user fills out the form as follows", table);
			When(@"the user presses ""Сохранить""");
		}

		private readonly UserInterface _ui;
	}
}
