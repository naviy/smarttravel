//using System;

//using Luxena.Travel.Domain;
//using Luxena.Travel.Parsers;

//using NUnit.Framework;


//namespace Luxena.Travel.Tests.Parsers
//{
//	[TestFixture]
//	public class AirBufferParserTests
//	{
//		[Test]
//		public void TestParseTicket1()
//		{
//			var docs = AirBufferParser.Parse(Res.AirBufferTicket1_1, Res.AirBufferTicket1_2, new Currency("UAH"));

//			Assert.AreEqual(1, docs.Count);
//			Assert.IsInstanceOf<AviaTicket>(docs[0]);

//			var ticket = (AviaTicket) docs[0];

//			Assert.AreEqual("195", ticket.AirlinePrefixCode);
//			Assert.AreEqual(9586722426, ticket.Number);
//			Assert.AreEqual(new DateTime(2012, 6, 20), ticket.IssueDate);
//		}
//	}
//}