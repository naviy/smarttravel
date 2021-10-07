//using System;
//using System.IO;

//using Luxena.Travel.Domain;
//using Luxena.Travel.Parsers;

//using NUnit.Framework;


//namespace Luxena.Travel.Tests.Parsers
//{
//	[TestFixture]
//	public class PaymentXmlParserTests
//	{
//		[Test]
//		public void TestParse1()
//		{
//			Payment p;

//			using (var reader = new StringReader(Res.PaymentXml1))
//				p = PaymentXmlParser.ReadFrom(db, reader);

//			Assert.AreEqual("42a32a0affd5462284b39eadfbbfc5ec", p.Invoice.Id);
//			Assert.AreEqual(new DateTime(2011, 3, 10), p.Date);
//			Assert.AreEqual(980, p.Amount.Currency.NumericCode);
//			Assert.AreEqual(1056, p.Amount.Amount);
//		}
//	}
//}