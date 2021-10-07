using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class NumberColumnConfig : ColumnConfig
	{
		public bool IsInteger { get; set; }
	}
}