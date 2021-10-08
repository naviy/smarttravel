using System.Xml.Serialization;


namespace Luxena.Travel.Export
{
	public class ExportField
	{
		[XmlAttribute]
		public string Caption { get; set; }
		[XmlAttribute]
		public string PropertyName { get; set; }
		[XmlAttribute]
		public string ExcelFormat { get; set; }
		[XmlAttribute]
		public int Width { get; set; }
		[XmlAttribute]
		public string Formula { get; set; }
		[XmlAttribute]
		public string ValueConst { get; set; }

		public bool ShouldSerializeWidth()
		{
			return Width != 0;
		}

		[XmlIgnore]
		public int DataFieldCount { get; set; }
		
		public ExportField[] ChildFields;
	}
}
