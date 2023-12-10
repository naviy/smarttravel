using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{

	public class GalileoRailXmlParser
	{

		public static IEnumerable<Entity2> Parse(string content, Currency defaultCurrency)
		{
			if (content.No())
				throw new GdsImportException("Empty GalileoRailXml");

			return new GalileoRailXmlParser(defaultCurrency).Parse(content);
		}

		public GalileoRailXmlParser(Currency defaultCurrency)
		{
			_defaultCurrency = defaultCurrency;
		}


		private IEnumerable<Entity2> Parse(string content)
		{
			var xml = XDocument.Parse(content).Root;
			if (xml == null) return null;

			return ParsePasteboard(xml);
		}


		private IEnumerable<Pasteboard> ParsePasteboard(XContainer xml)
		{
			var ruCulture = CultureInfo.GetCultureInfo("ru-ru");

			var r = new Pasteboard
			{
				Originator = GdsOriginator.Galileo,
				Origin = ProductOrigin.GalileoRailXml
			};

			var train = xml.El("train");
			var wagon = xml.El("wagon");
			var pax = xml.El("pax");
			var price = xml.El("price");

			r.IssueDate = xml.Value("sysdate").As().DateTime(ruCulture).Date;
			r.Number = xml.Value("uz_ordernumber");

			r.Provider = new Organization { Name = "Galileo" };

			train.Value("departure").As().DateTimen(ruCulture).Do(a =>
			{
				r.DepartureDate = a.Date;
				r.DepartureTime = a.ToString("HH:mm");
			});
			r.DeparturePlace = train.Value("station_from_ua");
			train.Value("arrival").As().DateTimen(ruCulture).Do(a =>
			{
				r.ArrivalDate = a.Date;
				r.ArrivalTime = a.ToString("HH:mm");
			});
			r.ArrivalPlace = train.Value("station_to_ua");
			

			r.TrainNumber = train.Value("number");
			r.CarNumber = wagon.Value("number");
			r.ServiceClass = wagon.Value("type").As(a=>
				a == "Ë" ? PasteboardServiceClass.LuxuryCoupe :
				a == "Ì" ? PasteboardServiceClass.FirstClass :   
				a == "Ê" ? PasteboardServiceClass.Compartment :   
				a == "Ï" ? PasteboardServiceClass.ReservedSeat :  
				a == "Ñ" ? wagon.Value("class") == "1" ? PasteboardServiceClass.FirstClass : PasteboardServiceClass.SecondClass :
				a == "Î" ? PasteboardServiceClass.Ñommon :
				PasteboardServiceClass.Unknown
			);

			r.SeatNumber = pax.Value("place");


			r.AddPassenger((pax.Value("lastname") + " " + pax.Value("firstname")).Clip(), null);


			var currency = new Currency(price.Value("currency") ?? _defaultCurrency);

			r.Fare = new Money(currency, price.Value("ticketTotalPrice").As().Decimal);
			r.EqualFare = r.Fare.Clone();
			r.BookingFee = new Money(currency, price.Value("apiFee").As().Decimal);

			r.FeesTotal = new Money(currency,
				price.Value("apiFee").As().Decimal +
				price.Value("excessFee").As().Decimal +
				price.Value("paymentFee").As().Decimal
			);

			r.ServiceFee = new Money(currency, price.Value("agencyFee").As().Decimal);

			var email = xml.Value("site_user", "email")?.Split('@');
			r.TicketerCode = email.By(0);
			r.TicketerOffice = email.By(1);
			//email = xml.Value("site_user", "email")?.Split('@');
			//r.TicketerOffice = email.By(1);
			//r.TicketerCode = email.By(0);

			//r.IsVoid = xml.Value("status") == "XX";

			r.Total = r.GetTotal();
			r.GrandTotal = r.GetGrandTotal();

			yield return r;
			
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
	}

}