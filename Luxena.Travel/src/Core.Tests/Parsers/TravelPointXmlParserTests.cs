using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NUnit.Framework;


namespace Luxena.Travel.Tests.Parsers
{


	[TestFixture]
	public class TravelPointXmlParserTests
	{

		[Test]
		public void Test01()
		{

			var docs = ParseDocuments(@"
<avia-ticket>
  <issuing-date type=""dateTime"">2021-05-18T13:02:23+03:00</issuing-date>
  <status>issue</status>
  <ticket-number>381-9216616</ticket-number>
  <booking-number>381B0207</booking-number>
  <reissue-old-no nil=""true"" />
  <agent-id-booking nil=""true"" />
  <agent-id-ticketing nil=""true"" />
  <office-id-subagent nil=""true"" />
  <office-id-ticketing nil=""true"" />
  <iata-agency-code nil=""true"" />
  <flight-class nil=""true"" />
  <tour-code nil=""true"" />
  <ticket-type>ticket</ticket-type>
  <currency>UAH</currency>
  <fare-eq type=""float"">55.26</fare-eq>
  <cancellation-fee type=""integer"">0</cancellation-fee>
  <taxes type=""float"">1326.24</taxes>
  <route>EVN-IEV</route>
  <commission nil=""true"" />
  <commission-equiv type=""integer"">1</commission-equiv>
  <passenger>Pidlubnyi Sergii</passenger>
  <carrier>UBE</carrier>
  <origin-city nil=""true"" />
  <destination-city nil=""true"" />
  <air-segments type=""array"">
    <air-segment>
      <no type=""integer"">1</no>
      <carrier>UBE</carrier>
      <number type=""integer"">302</number>
      <air-class nil=""true"" />
      <departure-date>2021-05-28</departure-date>
      <departure-time>01:30</departure-time>
      <arrival-date>2021-05-28</arrival-date>
      <arrival-time>03:25</arrival-time>
      <origin-airport-name>Звартноц</origin-airport-name>
      <origin-airport-code>EVN</origin-airport-code>
      <destination-airport-name>Жуляны</destination-airport-name>
      <destination-airport-code>IEV</destination-airport-code>
      <fare-basis>Smart</fare-basis>
      <baggage>1PC</baggage>
      <meal nil=""true"" />
      <terminal-check-in nil=""true"" />
      <terminal-arrival nil=""true"" />
      <flight-duration-time nil=""true"" />
      <equipment nil=""true"" />
    </air-segment>
  </air-segments>
  <supplier nil=""true"" />
  <buyer>Фламинго тревел</buyer>
  <buyer-edrpou>37406272</buyer-edrpou>
  <buyer-id type=""integer"">160</buyer-id>
  <sf-total type=""float"">0.0</sf-total>
  <miscellaneous-fees type=""float"">0.0</miscellaneous-fees>
  <refund-fee type=""integer"">0</refund-fee>
  <form-of-payment nil=""true"" />
  <total-amount type=""float"">1381.5</total-amount>
  <grand-total-amount type=""float"">0.0</grand-total-amount>
  <our-commission type=""float"">0.0</our-commission>
  <source>drct</source>
  <office-id-booking nil=""true"" />
  <currency-rate type=""float"">34.56376</currency-rate>
  <consolidator>tc</consolidator>
</avia-ticket>");


			Assert.AreEqual(1, docs.Count);

		}


		public List<Entity2> ParseDocuments(string documentContent)
		{
			return TravelPointXmlParser.Parse(documentContent, new Currency("UAH"), null).ToList();
		}

	}


}
