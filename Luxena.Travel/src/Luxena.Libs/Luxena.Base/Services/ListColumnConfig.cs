using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class ListColumnConfig : ColumnConfig
	{
		public object[] Items { get; set; }
	}
}