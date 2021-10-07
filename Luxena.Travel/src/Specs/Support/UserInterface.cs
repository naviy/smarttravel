using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;


namespace Luxena.Travel.Specs.Support
{
	public class UserInterface
	{
		public RemoteWebDriver Browser
		{
			get
			{
				if (_browser == null)
					Assert.Fail("User is not logged in");

				return _browser;
			}
		}

		public IWebElement Scope
		{
			get
			{
				if (_forms.Count != 0)
					return _forms.Peek();

				var tabId = Browser.FindElement(By.XPath("//div[@id='content']/div[contains(@class,'x-tab-panel-header')]//li[contains(@class, 'x-tab-strip-active')]"))
					.GetAttribute("id")
					.Substring("content__".Length);

				return Browser.FindElement(By.Id(tabId));
			}
		}

		public IWebElement MessageBox
		{
			get { return Browser.FindElement(By.XPath("//div[contains(@class, 'x-window-dlg')]")); }
		}

		public static void Cleanup()
		{
			if (_adminBrowser != null)
				_adminBrowser.Quit();

			if (_agentBrowser != null)
				_agentBrowser.Quit();
		}

		public void AsAdmin()
		{
			if (_adminBrowser == null)
				_adminBrowser = CreateSession("admin", "admin");

			_browser = _adminBrowser;
		}

		public void AsAgent()
		{
			if (_agentBrowser == null)
				_agentBrowser = CreateSession("svetlana.vasilkova", "svetlana.vasilkova");

			_browser = _agentBrowser;
		}

		public void PushForm(string name)
		{
			_forms.Push(Browser.FindElement(By.XPath(string.Format("//div[contains(@class, 'window-edit') and descendant::span[text()='{0}']]", name))));
		}

		public void PopForm()
		{
			_forms.Pop();
		}

		public void WaitForAjax()
		{
			var scriptExecutor = (IJavaScriptExecutor) Browser;

			Thread.Sleep(10);

			while (true) // Handle timeout somewhere
			{
				var ajaxIsComplete = (bool) scriptExecutor.ExecuteScript("return jQuery.active == 0");

				if (ajaxIsComplete)
					break;

				Thread.Sleep(100);
			}
		}

		public void FilterList(string filter)
		{
			var element = Scope.FindElement(By.XPath("descendant::div[contains(@class, 'toolbar')]/descendant::div[contains(@class, 'filter')]/following::*[name()='input' or name()='textarea']"));

			element.Clear();
			element.SendKeys(filter + Keys.Enter);

			WaitForAjax();

			element.SendKeys(Keys.ArrowDown);
		}

		public int ListCount()
		{
			var listCountText = Scope.FindElement(By.XPath("descendant::div[contains(@class, 'x-panel-bbar')]/descendant::div[last()]")).Text;

			var match = _listCountRegex.Match(listCountText);

			return match.Success ? int.Parse(match.Groups[1].Value) : 0;
		}

		public void SelectAllRows()
		{
			Scope.FindElement(By.XPath("descendant::div[contains(@class, 'x-grid3-hd-checker')]")).Click();
		}

		public void CloseForm()
		{
			Scope.FindElement(By.XPath("descendant::div[contains(@class, 'x-tool-close')]")).Click();
			PopForm();
		}

		public void CloseMessageBox()
		{
			MessageBox.FindElement(By.XPath("descendant::div[contains(@class, 'x-tool-close')]")).Click();
		}

		public void CloseAllModals()
		{
			WaitForAjax();

			Browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMilliseconds(1));
			try
			{
				foreach (var element in Browser.FindElements(By.XPath("//div[contains(@class, 'x-window-dlg')]/descendant::div[contains(@class, 'x-tool-close')]")))
					if (element.Displayed)
						element.Click();
			}
			finally
			{
				Browser.Manage().Timeouts().ImplicitlyWait(_defaultImplicitWait);
			}
			while (_forms.Count != 0)
				CloseForm();
		}

		private static RemoteWebDriver CreateSession(string user, string password)
		{
			var options = new ChromeOptions();
			options.AddArgument(@"--incognito");
			options.AddArgument(@"--disable-translate");

			var driver = new ChromeDriver(options) { Url = "http://build.travel/login" };

			driver.Manage().Timeouts().ImplicitlyWait(_defaultImplicitWait);

			driver.FindElementByName("UserName").SendKeys(user);
			driver.FindElementByName("Password").SendKeys(password);
			driver.FindElementById("submit").Submit();

			Assert.AreEqual("http://build.travel/", driver.Url);

			return driver;
		}

		private static readonly TimeSpan _defaultImplicitWait = TimeSpan.FromSeconds(3);

		private static readonly Regex _listCountRegex = new Regex("Отображаются \\d+ - \\d+ из (\\d+)", RegexOptions.Compiled);

		private static RemoteWebDriver _adminBrowser;

		private static RemoteWebDriver _agentBrowser;

		private RemoteWebDriver _browser;

		private readonly Stack<IWebElement> _forms = new Stack<IWebElement>();
	}
}