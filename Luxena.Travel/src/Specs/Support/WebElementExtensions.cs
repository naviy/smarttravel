using OpenQA.Selenium;


namespace Luxena.Travel.Specs.Support
{
	public static class WebElementExtensions
	{
		public static IWebElement FindField(this IWebElement element, string label)
		{
			return element.FindElement(By.XPath(string.Format("descendant::label[text()='{0}:']/following::*[name()='input' or name()='textarea'][1]", label)));
		}

		public static void PressButton(this IWebElement element, string action)
		{
			element.FindElement(By.XPath(string.Format("descendant::button[starts-with(text(), '{0}')]", action))).Click();
		}

		public static void PressButtonAbove(this IWebElement element, string text, string action)
		{
			element.FindElement(By.XPath(string.Format("descendant::*[text()='{0}']/preceding::button[text()='{1}'][1]", text, action))).Click();
		}
	}
}