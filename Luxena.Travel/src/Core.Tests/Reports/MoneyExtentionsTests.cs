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
			Assert.AreEqual("������������ ����� ������� �������� ������ ����� 00 ���.", new Money(new Currency("UAH"), 14624).ToWords());
			Assert.AreEqual("0 ������� 00 ���.", new Money(new Currency("UAH"), 0).ToWords());
			Assert.AreEqual("��� �� ����� 00 ���.", new Money(new Currency("UAH"), 102).ToWords());
			Assert.AreEqual("���� ������� 52 ���.", new Money(new Currency("UAH"), 200.52m).ToWords());
			Assert.AreEqual("���� ������ �������� �� ����� 47 ���.", new Money(new Currency("UAH"), 1022.465m).ToWords());
			Assert.AreEqual("³����� ���'��������� ������� 00 ���.", new Money(new Currency("UAH"), 819).ToWords());
			Assert.AreEqual("��� ��� ������ ���� �'������� ���� ������ 00 ���.", new Money(new Currency("UAH"), 103251).ToWords());
			Assert.AreEqual("��������� ����� ������� 01 ���.", new Money(new Currency("UAH"), 400000.01m).ToWords());
			Assert.AreEqual("��� ������� ������ �� ����� ���� ������ 00 ���.", new Money(new Currency("UAH"), 2307001).ToWords());
		}

		[Test]
		public void MoneyExtentionsTestRus()
		{
			CommonRes.Culture = CultureInfo.GetCultureInfo("ru");
			Assert.AreEqual("������������ ����� �������� �������� ������ ������ 00 ���.", new Money(new Currency("UAH"), 14624).ToWords());
			Assert.AreEqual("0 ������ 00 ���.", new Money(new Currency("UAH"), 0).ToWords());
			Assert.AreEqual("��� ��� ������ 00 ���.", new Money(new Currency("UAH"), 102).ToWords());
			Assert.AreEqual("������ ������ 52 ���.", new Money(new Currency("UAH"), 200.52m).ToWords());
			Assert.AreEqual("���� ������ �������� ��� ������ 47 ���.", new Money(new Currency("UAH"), 1022.465m).ToWords());
			Assert.AreEqual("��������� ������������ ������ 00 ���.", new Money(new Currency("UAH"), 819).ToWords());
			Assert.AreEqual("��� ��� ������ ������ ��������� ���� ������ 00 ���.", new Money(new Currency("UAH"), 103251).ToWords());
			Assert.AreEqual("��������� ����� ������ 01 ���.", new Money(new Currency("UAH"), 400000.01m).ToWords());
			Assert.AreEqual("��� �������� ������ ���� ����� ���� ������ 00 ���.", new Money(new Currency("UAH"), 2307001).ToWords());
		}
	}
}