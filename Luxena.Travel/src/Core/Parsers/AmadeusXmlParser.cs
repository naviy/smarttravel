using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{

	public class AmadeusXmlParser
	{

		public static IEnumerable<Entity2> Parse(string content, Currency defaultCurrency, string[] robotCodes)
		{
			if (content.No())
				throw new GdsImportException("Empty AmadeusXml");

			return new AmadeusXmlParser(defaultCurrency, robotCodes).Parse(content);
		}

		public static IEnumerable<Entity2> Parse(XElement xml, Currency defaultCurrency, string[] robotCodes)
		{
			if (xml == null)
				throw new GdsImportException("Empty AmadeusXml");

			return new AmadeusXmlParser(defaultCurrency, robotCodes).Parse(xml.Value);
		}

		public AmadeusXmlParser(Currency defaultCurrency, string[] robotCodes)
		{
			_defaultCurrency = defaultCurrency;
			_robotCodes = robotCodes;
		}


		private IEnumerable<Entity2> Parse(string content)
		{
			var xml = XDocument.Parse(content).Root;
			if (xml == null) return null;

			var name = xml.Name.LocalName.ToLower();

			if (name == "reservationslist")
				return ParseProducts(xml);

			throw new NotImplementedException();
		}



		private IEnumerable<Entity2> ParseProducts(XElement xml)
		{
			return xml.Els("ReservationsList").SelectMany(ParseProduct);
		}


		private int _passengerIndex;
		private string[] _seats;
		readonly CultureInfo _ruCulture = CultureInfo.GetCultureInfo("ru-ru");
		private string[] _passengerNames;


		private IEnumerable<Entity2> ParseProduct(XElement xml)
		{
			_passengerNames = xml.Els("passenger")
				.Select(a => (a.Value("lastName") + " " + a.Value("firstName")).Clip())
				.ToArray();

			_seats = xml.Value("seats")?.Split(',').Select(a => a.Clip()).ToArray();
			_passengerIndex = -1;

			var service = xml.Value("service");

			foreach (var passengerName in _passengerNames)
			{
				_passengerIndex++;

				if (service == "lowcost")
				{
					var p = new AviaTicket();
					p.AddPassenger(passengerName, null);
					ParseProduct(xml, p);
					ParseAviaTicket(xml, p);
					yield return p;
				}
				else
				{
					var p = new Pasteboard();
					p.AddPassenger(passengerName, null);
					ParseProduct(xml, p);
					ParsePasteboard(xml, p);
					yield return p;
				}
			}
		}


		void ParseProduct(XElement xml, Product r)
		{
			r.Originator = GdsOriginator.Amadeus;
			r.Origin = ProductOrigin.AmadeusXml;

			r.IssueDate = xml.Value("addDate").As().DateTime(_ruCulture).Date;

			r.Provider = new Organization { Name = "Amadeus" };

			var currency = new Currency(xml.Value("currency"));

			r.Fare = new Money(currency, xml.Value("purchasePrice").As().Decimal) / _passengerNames.Length;
			r.EqualFare = r.Fare.Clone();
			r.ServiceFee = new Money(currency, xml.Value("agentCommission").As().Decimal) / _passengerNames.Length;
			r.BookingFee = new Money(currency, xml.Value("fees").As().Decimal) / _passengerNames.Length;

			var officeId = xml.Value("officeId");
			r.TicketerOffice = officeId?.Split('.').By(0) ?? officeId;
			r.TicketerCode = officeId ?? xml.Value("agentName");

			var status = xml.Value("status");
			r.IsVoid = status == "XX";// || status == "RF";

			r.Total = r.GetTotal();
			r.GrandTotal = r.GetGrandTotal();
		}


		void ParseAviaTicket(XElement xml, AviaTicket r)
		{
			r.AirlinePrefixCode = "100";

			r.PnrCode = xml.Value("confirmationNumber");

			var seg1 = new FlightSegment
			{
				DepartureTime = xml.Value("departureDate").As().DateTimen(_ruCulture),
				FromAirportCode = xml.Value("departureStationCode"),
				FromAirportName = xml.Value("departureStation"),

				ArrivalTime = xml.Value("arrivalDate").As().DateTimen(_ruCulture),
				ToAirportCode = xml.Value("arrivalStationCode"),
				ToAirportName = xml.Value("arrivalStation"),

				FlightNumber = xml.Value("trainNumber"),
				CarrierName = xml.Value("transporter"),
			};


			r.AddSegment(seg1);


			if (xml.Value("roundTrip") == "1")
			{
				r.PnrCode += xml.Value("trainNumber").As(a => "-" + a);


				var seg2 = new FlightSegment
				{
					FromAirportCode = xml.Value("arrivalStationCode"),
					FromAirportName = xml.Value("arrivalStation"),

					ToAirportCode = xml.Value("departureStationCode"),
					ToAirportName = xml.Value("departureStation"),

					CarrierName = xml.Value("transporter"),
				};
				
				r.AddSegment(seg2);
			}
		}


		void ParsePasteboard(XElement xml, Pasteboard r)
		{
			r.Number = xml.Value("confirmationNumber");

			// ReSharper disable PossibleInvalidOperationException
			xml.Value("departureDate").As().DateTimen(_ruCulture).Do(a =>
			{
				r.DepartureDate = a.Date;
				r.DepartureTime = a.ToString("HH:mm");
			});
			r.DeparturePlace = xml.Value("departureStation");
			xml.Value("arrivalDate").As().DateTimen(_ruCulture).Do(a =>
			{
				r.ArrivalDate = a.Date;
				r.ArrivalTime = a.ToString("HH:mm");
			});
			r.ArrivalPlace = xml.Value("arrivalStation");
			// ReSharper restore PossibleInvalidOperationException

			r.ServiceClass = PasteboardServiceClass.Unknown;
			r.TrainNumber = xml.Value("trainNumber");
			r.CarNumber = xml.Value("wagonNumber");
			r.SeatNumber = _seats.By(_passengerIndex);
		}


		#region Utils

		private Money NewMoney(string currency, string value)
		{
			var dvalue = value.As().Decimaln;
			return dvalue == null ? null : new Money(currency, dvalue.Value);
		}

		private Money NewMoney(XElement xml, string currencyName = "Curr", string valueName = "Value")
		{
			return new Money(xml.Attr(currencyName), xml.Attr(valueName).As().Decimal);
		}

		private Money NewElMoney(XElement xml, string elName, string currencyName = "Curr", string valueName = "Value")
		{
			return NewMoney(xml.El(elName), currencyName, valueName);
		}

		private Money NewDefaultMoney(string value)
		{
			return new Money(_defaultCurrency, value.As().Decimal);
		}

		#endregion


		private readonly Currency _defaultCurrency;
		private readonly string[] _robotCodes;
	}

}