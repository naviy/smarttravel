using System.Globalization;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Reports
{
	[TestFixture]
	public class MoneyExtentionsTests
	{
		[Test]
		public void MoneyExtentionsTestUkr()
		{
			CommonRes.Culture = CultureInfo.GetCultureInfo("uk");
			Assert.AreEqual("„отирнадц€ть тис€ч ш≥стсот двадц€ть чотири гривн≥ 00 коп.", new Money(new Currency("UAH"), 14624).ToWords());
			Assert.AreEqual("0 гривень 00 коп.", new Money(new Currency("UAH"), 0).ToWords());
			Assert.AreEqual("—то дв≥ гривн≥ 00 коп.", new Money(new Currency("UAH"), 102).ToWords());
			Assert.AreEqual("ƒв≥ст≥ гривень 52 коп.", new Money(new Currency("UAH"), 200.52m).ToWords());
			Assert.AreEqual("ќдна тис€ча двадц€ть дв≥ гривн≥ 47 коп.", new Money(new Currency("UAH"), 1022.465m).ToWords());
			Assert.AreEqual("¬≥с≥мсот дев'€тнадц€ть гривень 00 коп.", new Money(new Currency("UAH"), 819).ToWords());
			Assert.AreEqual("—то три тис€ч≥ дв≥ст≥ п'€тдес€т одна гривн€ 00 коп.", new Money(new Currency("UAH"), 103251).ToWords());
			Assert.AreEqual("„отириста тис€ч гривень 01 коп.", new Money(new Currency("UAH"), 400000.01m).ToWords());
			Assert.AreEqual("ƒва м≥льйона триста с≥м тис€ч одна гривн€ 00 коп.", new Money(new Currency("UAH"), 2307001).ToWords());
		}

		[Test]
		public void MoneyExtentionsTestRus()
		{
			CommonRes.Culture = CultureInfo.GetCultureInfo("ru");
			Assert.AreEqual("„етырнадцать тыс€ч шестьсот двадцать четыре гривны 00 коп.", new Money(new Currency("UAH"), 14624).ToWords());
			Assert.AreEqual("0 гривен 00 коп.", new Money(new Currency("UAH"), 0).ToWords());
			Assert.AreEqual("—то две гривны 00 коп.", new Money(new Currency("UAH"), 102).ToWords());
			Assert.AreEqual("ƒвести гривен 52 коп.", new Money(new Currency("UAH"), 200.52m).ToWords());
			Assert.AreEqual("ќдна тыс€ча двадцать две гривны 47 коп.", new Money(new Currency("UAH"), 1022.465m).ToWords());
			Assert.AreEqual("¬осемьсот дев€тнадцать гривен 00 коп.", new Money(new Currency("UAH"), 819).ToWords());
			Assert.AreEqual("—то три тыс€чи двести п€тьдес€т одна гривна 00 коп.", new Money(new Currency("UAH"), 103251).ToWords());
			Assert.AreEqual("„етыреста тыс€ч гривен 01 коп.", new Money(new Currency("UAH"), 400000.01m).ToWords());
			Assert.AreEqual("ƒва миллиона триста семь тыс€ч одна гривна 00 коп.", new Money(new Currency("UAH"), 2307001).ToWords());
		}
	}
}