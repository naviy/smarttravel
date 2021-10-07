using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

using Luxena.Travel.Specs.Support;

using NUnit.Framework;

using OpenQA.Selenium;

using TechTalk.SpecFlow;


namespace Luxena.Travel.Specs.Steps
{
	[Binding]
	public class CommonSteps : TechTalk.SpecFlow.Steps
	{
		public CommonSteps(UserInterface ui)
		{
			_ui = ui;
		}

		[BeforeTestRun]
		public static void BeforeTestRun()
		{
			Environment.SetEnvironmentVariable("PGPASSWORD", "test");

			var process = Process.Start(@"d:\data\luxena-ua\Projects\Luxena.Travel\Implementation\dotNet\tools\psql\psql.exe",
				@"-h localhost -U test -q -f d:\data\luxena-ua\Projects\Luxena.Travel\Implementation\dotNet\src\Specs\Data\test_data.sql travel");

			if (process == null)
				Assert.Fail("Failed to import test data");

			process.WaitForExit();
		}

		[AfterTestRun]
		public static void AfterTestRun()
		{
			UserInterface.Cleanup();
		}

		[AfterScenario]
		public void AfterScenario()
		{
			_ui.CloseAllModals();
		}

		[Given(@"the user has logged in as (.*)")]
		public void TheUserHasLoggedInAs(string role)
		{
			role = role.ToLower();

			if (role == "agent")
				_ui.AsAgent();
			else if (role == "admin")
				_ui.AsAdmin();
		}

		[StepDefinition(@"the user has selected menu item ""(.*)""")]
		public void TheUserHasSelectedMenuItem(string menuPath)
		{
			var items = menuPath.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in items)
				_ui.Browser.FindElement(By.XPath(string.Format("//div[@id='header' or contains(@class,'x-menu')]//*[text()='{0}']", item.Trim()))).Click();
		}

		[When(@"the user selects ""(.*)"" in the list")]
		public void TheUserSelectsInTheList(string filter)
		{
			_ui.FilterList(filter);
			_ui.SelectAllRows();
		}

		[StepDefinition(@"the user presses ""(.*)""")]
		public void TheUserPresses(string action)
		{
			_ui.Scope.PressButton(action);
		}

		[When(@"the user presses at confirmation dialog ""(.*)""")]
		public void TheUserPressesOnMessageBox(string action)
		{
			_ui.MessageBox.PressButton(action);
		}

		[StepDefinition(@"the user fills out the form as follows")]
		public void TheUserFillsOutTheFormAsFollows(Table table)
		{
			foreach (var row in table.Rows)
			{
				var field = _ui.Scope.FindField(row["Label"]);
				if (field.TagName == "textarea" || field.GetAttribute("readonly") != "true")
				{
					field.Clear();
					field.SendKeys(row["Value"]);
					if (_selectorClassTest.IsMatch(field.GetAttribute("class")))
						_ui.WaitForAjax();
					field.SendKeys(Keys.Enter);
				}
				else
				{
					field.SendKeys(Keys.ArrowDown);
					foreach (var element in _ui.Browser.FindElements(By.CssSelector(".x-layer.x-combo-list")))
						if (element.Displayed)
						{
							element.FindElement(By.XPath(string.Format("descendant::div[text()='{0}']", row["Value"]))).Click();
							break;
						}
				}
			}
		}

		[Then(@"the system shows confirmation dialog")]
		public void ThenTheSystemShowsConfirmationDialog()
		{
			Assert.NotNull(_ui.MessageBox);
		}

		[Then(@"the system opens form ""(.*)""")]
		public void TheSystemOpensForm(string name)
		{
			_ui.PushForm(name);
		}

		[Then(@"the system closes the form")]
		public void TheSystemClosesTheForm()
		{
			_ui.PopForm();
		}

		[Then(@"the system shows notification ""(.*): (.*)""")]
		public void ThenTheSystemShowsNotification(string title, string message)
		{
			const int timeout = 5000;
			const int interval = 100;

			for (var i = 0; i < timeout/interval; ++i)
			{
				var notificationText = _ui.Browser.FindElement(By.XPath("//div[@id='msg-div']/div[last()]//div[@class='x-box-mc']")).Text;

				if (new Regex(string.Format("^{0}\\s+{1}", title, message)).IsMatch(notificationText))
					return;

				Thread.Sleep(interval);
			}

			Assert.Fail("Notification \"{0}: {0}\" wasn't shown", title, message);
		}

		[Then(@"the system shows error ""(.*)""")]
		public void ThenTheSystemShowsError(string messsage)
		{
			var dialogText = _ui.MessageBox.FindElement(By.XPath("descendant::span[@class='ext-mb-text']")).Text;

			Assert.AreEqual(messsage, dialogText);
		}

		[Then(@"the system shows content ""(.*)""")]
		public void ThenSystemShowsContent(string content)
		{
			// ???
			_ui.Scope.FindElement(By.XPath(string.Format("descendant::*[text()='{0}']", content)));
		}

		[Then(@"the system shows validation message ""(.*)""")]
		public void ThenTheSystemShowsValidationMessage(string message)
		{
			_ui.Scope.FindElement(By.XPath(string.Format("descendant::div[@class='x-panel errors']//li//a[text()='{0}']", message)));
		}

		private static readonly Regex _selectorClassTest = new Regex(@"(^|\W)selector($|\W)", RegexOptions.Compiled);

		private readonly UserInterface _ui;
	}
}