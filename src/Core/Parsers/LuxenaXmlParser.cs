using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Luxena.Domain.Entities;
using Luxena.Travel.Domain;
using Luxena.Travel.Export;


namespace Luxena.Travel.Parsers
{


	public class LuxenaXmlParser
	{

		public static IEnumerable<Entity2> Parse(Domain.Domain db, string content, Currency defaultCurrency)
		{
			if (content.No())
				throw new GdsImportException("Empty LuxenaXml");

			return new LuxenaXmlParser(defaultCurrency).Parse(db, content);
		}

		public LuxenaXmlParser(Currency defaultCurrency)
		{
			_defaultCurrency = defaultCurrency;
		}


		//===


		private IEnumerable<Entity2> Parse(Domain.Domain db, string content)
		{
			if (content.No()) return null;

			if (content.Contains("<AviaTicket"))
				return Parse<AviaTicket, AviaTicketContract>(db, content);
			if (content.Contains("<AviaRefund"))
				return Parse<AviaRefund, AviaRefundContract>(db, content);
			if (content.Contains("<AviaMco"))
				return Parse<AviaMco, AviaMcoContract>(db, content);

			return null;
		}

		private IEnumerable<Entity2> Parse<TEntity, TContract>(Domain.Domain db, string content)
			where TEntity : Product, new()
			where TContract : ProductContract
		{
			//content = content.Replace("AviaTicket", "AviaTicketContract");

			var strReader = new StringReader(content);
			var serializer = new XmlSerializer(typeof(TContract));
			var xmlReader = new XmlTextReader(strReader);
			var c = (TContract)serializer.Deserialize(xmlReader);

			var r = new TEntity();
			c.AssignTo(db, r);
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