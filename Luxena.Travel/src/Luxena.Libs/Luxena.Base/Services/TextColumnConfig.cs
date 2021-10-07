using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class TextColumnConfig : ColumnConfig
	{
		public int Length { get; set; }

		public int Lines { get; set; }
	}
}