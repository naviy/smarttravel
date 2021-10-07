using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class DateTimeColumnConfig : ColumnConfig
	{
		public string FormatString { get; set; }
	}
}