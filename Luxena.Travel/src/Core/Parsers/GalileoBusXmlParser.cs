using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{

	public class GalileoBusXmlParser
	{

		public static IEnumerable<Entity2> Parse(string content, Currency defaultCurrency)
		{
			if (content.No())
				throw new GdsImportException("Empty GalileoBusXml");

			return new GalileoBusXmlParser(defaultCurrency).Parse(content);
		}

		public GalileoBusXmlParser(Currency defaultCurrency)
		{
			_defaultCurrency = defaultCurrency;
		}


		private IEnumerable<Entity2> Parse(string content)
		{
			var xml = XDocument.Parse(content).Root;
			if (xml == null) return null;

			return ParseTicket(xml);
		}


		private IEnumerable<BusTicket> ParseTicket(XContainer xml)
		{
			var ruCulture = CultureInfo.GetCultureInfo("ru-ru");

			var r = new BusTicket
			{
				Originator = GdsOriginator.Galileo,
				Origin = ProductOrigin.GalileoBusXml
			};


			r.IssueDate = xml.Value("created").As().ToDateTimen("dd.MM.yyyy HH:mm:ss")?.Date ?? DateTime.Today;
			r.Number = xml.Value("order_id");

			r.Provider = new Organization { Name = "Galileo" };

			var route = xml.Value("route")?.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

			r.DeparturePlace = route.By(0);
			r.ArrivalPlace = route.By(1);

			r.AddPassenger((xml.Value("passenger_surname") + " " + xml.Value("passenger_name")).Clip(), null);


			var currency = new Currency(xml.Value("payment_currency") ?? _defaultCurrency);

			r.Fare = new Money(currency, xml.Value("amount_to_transfer_to_cms").As().Decimal);

			r.EqualFare = r.Fare.Clone();

			r.FeesTotal = new Money(currency, xml.Value("cost_type_3").As().Decimal);

			//r.BookingFee = new Money(currency, xml.Value("fee_for_payment_method").As().Decimal);

			//r.ServiceFee = new Money(currency, xml.Value("cost_type_3").As().Decimal);

			r.TicketerCode = xml.Value("site_user");
			r.TicketerOffice = xml.Value("company_2");

			r.IsVoid = xml.Value("status") == "отменен";

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