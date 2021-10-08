using System;
using System.Collections.Generic;

using Luxena.Domain;
using Luxena.Travel.Domain;

using NUnit.Framework;


namespace Luxena.Travel.Tests
{

	[TestFixture]
	public class InvoiceTests : IDefaultLocalizationTypesSource
	{

		public Type[] DefaultLocalizationTypes { get { return _defaultLocalizationTypes; } }

		static readonly Type[] _defaultLocalizationTypes =
		{
			typeof(RUAttribute), 
			typeof(UAAttribute), 
			typeof(ENAttribute)
		};


		[Test]
		public void TestAnnotation01()
		{
			var attr = typeof(BankAccount).Semantic<SmallAttribute>();
			Assert.NotNull(attr);

			attr = typeof(object).Semantic<SmallAttribute>();
			Assert.Null(attr);

			attr = typeof(int).Semantic<SmallAttribute>();
			Assert.Null(attr);

			attr = typeof(string).Semantic<SmallAttribute>();
			Assert.Null(attr);

			attr = typeof(decimal).Semantic<SmallAttribute>();
			Assert.Null(attr);

			var prop1 = typeof(Party).GetProperty("Id");

			SemanticCodeGeneration.GetCodeForScriptSharp(prop1, this);
		}


//		[Test]
//		public void TestAnnotation02()
//		{
//			var attr = typeof(CurrencyDailyRate).PropertyWithAttribute<>() Annotation<SmallAttribute>();
//			Assert.NotNull(attr);
//		}


		[Test]
		public void TestDeepLocalization01()
		{
			var loc = typeof(Passport).GetProperty("Number").Localization(this);

			Assert.AreEqual("Номер", loc.Russian);
			Assert.AreEqual("Номер", loc.Default);
		}

		[Test]
		public void TestDeepLocalization02()
		{
			var loc = typeof(CurrencyDailyRate).GetProperty("UAH_EUR").Localization(this);
			Assert.AreEqual("UAH/EUR", loc.Default);

			loc = typeof(CurrencyDailyRate).GetProperty("UAH_RUB").Localization(this);
			Assert.AreEqual("UAH/RUB", loc.Default);

			loc = typeof(CurrencyDailyRate).GetProperty("UAH_USD").Localization(this);
			Assert.AreEqual("UAH/USD", loc.Default);

			loc = typeof(CurrencyDailyRate).GetProperty("RUB_EUR").Localization(this);
			Assert.AreEqual("RUB/EUR", loc.Default);

			loc = typeof(CurrencyDailyRate).GetProperty("RUB_USD").Localization(this);
			Assert.AreEqual("RUB/USD", loc.Default);

			loc = typeof(CurrencyDailyRate).GetProperty("EUR_USD").Localization(this);
			Assert.AreEqual("EUR/USD", loc.Default);
		}

		[Test]
		public void TestDeepLocalization03()
		{
			var loc = typeof(Product).GetProperty("PaymentType").Localization(this);

			Assert.AreEqual("Тип оплаты", loc.Russian);
			Assert.AreEqual("Тип оплаты", loc.Default);
		}



		[Test]
		public void TestDefaultLocalization01()
		{
			var loc = typeof(Person).GetProperty("Organization").Localization(this);

			Assert.AreEqual("Организация", loc.Russian);
			Assert.AreEqual("Организация", loc.Default);
		}

		[Test]
		public void TestDefaultLocalization02()
		{
			var loc = typeof(ProductType).GetMember("Transfer").One().Localization(this);

			Assert.AreEqual("Трансфер", loc.Russian);
			Assert.AreEqual("Трансфер", loc.Default);
		}

		[Test]
		public void TestEnumLocalization01()
		{
			var loc = UserRole.Agent.Localization(this);

			Assert.AreEqual("Агент", loc.Russian);
			Assert.AreEqual("Агент", loc.Default);

			loc = typeof(User).GetMember("IsAgent").One().Localization(this);

			Assert.AreEqual("Агент", loc.Russian);
			Assert.AreEqual("Агент", loc.Default);
		}


		[Test]
		public void TestAnnotationSetup01()
		{
			var bag = SemanticSetupAttribute.Invoke(typeof(Organization));

			Assert.NotNull(bag);

			var etags = new List<string>();

			foreach (var propBag in bag.Properties)
			{
				foreach (var prop in propBag.Properties)
				{
					etags.AddRange(SemanticCodeGeneration.GetCodeForScriptSharp(propBag, prop, this));
				}
			}

			Assert.Contains(".Title(\"Код предприятия (ЕДРПОУ)\")", etags);
		}

	}

}