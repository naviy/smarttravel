using System;
using System.IO;
using System.Xml;


namespace Luxena.Travel.Export
{

	public class CustomXmlWriter : XmlTextWriter
	{
		public CustomXmlWriter(Stream stream) : base(stream, null)
		{
			Formatting = Formatting.Indented;
			IndentChar = '\t';
			Indentation = 1;
			//Namespaces = false;
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			base.WriteStartElement("", localName, "");
		}

	}




	//public class CustomXmlReader : XmlTextReader
	//{
	//	public CustomXmlReader(Stream stream) : base(stream)
	//	{
	//		Formatting = Formatting.Indented;
	//		IndentChar = '\t';
	//		Indentation = 1;
	//	}

	//	public override void WriteStartElement(string prefix, string localName, string ns)
	//	{
	//		base.WriteStartElement("", localName, "");
	//	}

	//}


}