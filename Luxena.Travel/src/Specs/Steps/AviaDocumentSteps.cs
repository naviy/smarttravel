using Luxena.Travel.Specs.Support;

using NUnit.Framework;

using OpenQA.Selenium;

using TechTalk.SpecFlow;


namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	public class AviaDocumentSteps : TechTalk.SpecFlow.Steps
	{
		public AviaDocumentSteps(UserInterface ui)
		{
			_ui = ui;
		}

		[Given(@"the user selected ""3544667874"" ticket")]
		public void TheUserSelectedTicket()
		{
			_ui.FilterList("3544667874");

			Assert.AreNotEqual(0, _ui.ListCount());
		}

		[Then(@"the system calculates ""К оплате"" as ""1813,00""")]
		public void TheSystemCalculatesGrandTotal()
		{
			Assert.AreEqual("1813,00", _ui.Scope.FindField("К оплате").GetAttribute("value"));
		}

		[Then(@"the system updates the selected row as follows")]
		public void TheSystemUpdatesTheSelectedRow(Table table)
		{
			_ui.WaitForAjax();

			var selection = _ui.Scope.FindElement(By.CssSelector("div.x-grid3-row-selected"));

			foreach (var row in table.Rows)
				selection.FindElement(By.XPath(string.Format("descendant::*[text()='{0}']", row["Value"])));
		}

		private readonly UserInterface _ui;
	}
}
