//using System;
//using System.Globalization;
//using System.IO;
//using System.Xml.Serialization;

//using Luxena.Travel.Domain;


//namespace Luxena.Travel.Parsers
//{
//	[XmlRootAttribute(ElementName = "������")]
//	public class PaymentXml
//	{
//		public string ������� { get; set; }
//		public string ���������� { get; set; }
//		public string ���������� { get; set; }
//		public string ����� { get; set; }
//		public string ��� { get; set; }
//		public int ������ { get; set; }
//		public string ������� { get; set; }
//	}

//	public static class PaymentXmlParser
//	{
//		public static Payment ReadFrom(Domain.Domain db, TextReader reader)
//		{
//			var xml = (PaymentXml) new XmlSerializer(typeof (PaymentXml)).Deserialize(reader);

//			var payment = new WireTransfer();

//			payment.SetInvoice(new Invoice { Id = xml.������� });

//			if (xml.����������.Yes())
//				payment.SetDate(db, DateTime.ParseExact(xml.����������, "M/d/yyyy", _culture));

//			if (xml.�����.Yes())
//				payment.SetAmount(new Money(xml.������, decimal.Parse(xml.�����, _culture)));

//			if (xml.���.Yes())
//				payment.SetVat(new Money(xml.������, decimal.Parse(xml.���, _culture)));

//			return payment;
//		}

//		private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;
//	}
//}