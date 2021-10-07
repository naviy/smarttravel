using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class CustomTypeColumnConfig : ColumnConfig
	{
		public string TypeName { get; set; }
	}
}