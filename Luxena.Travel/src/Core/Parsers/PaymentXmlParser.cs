//using System;
//using System.Globalization;
//using System.IO;
//using System.Xml.Serialization;

//using Luxena.Travel.Domain;


//namespace Luxena.Travel.Parsers
//{
//	[XmlRootAttribute(ElementName = "ќплата")]
//	public class PaymentXml
//	{
//		public string »д—чета { get; set; }
//		public string Ќомер—чета { get; set; }
//		public string ƒатаќплаты { get; set; }
//		public string —умма { get; set; }
//		public string Ќƒ— { get; set; }
//		public int ¬алюта { get; set; }
//		public string —умма”ч { get; set; }
//	}

//	public static class PaymentXmlParser
//	{
//		public static Payment ReadFrom(Domain.Domain db, TextReader reader)
//		{
//			var xml = (PaymentXml) new XmlSerializer(typeof (PaymentXml)).Deserialize(reader);

//			var payment = new WireTransfer();

//			payment.SetInvoice(new Invoice { Id = xml.»д—чета });

//			if (xml.ƒатаќплаты.Yes())
//				payment.SetDate(db, DateTime.ParseExact(xml.ƒатаќплаты, "M/d/yyyy", _culture));

//			if (xml.—умма.Yes())
//				payment.SetAmount(new Money(xml.¬алюта, decimal.Parse(xml.—умма, _culture)));

//			if (xml.Ќƒ—.Yes())
//				payment.SetVat(new Money(xml.¬алюта, decimal.Parse(xml.Ќƒ—, _culture)));

//			return payment;
//		}

//		private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
//	}
//}