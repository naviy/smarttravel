using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class ClassColumnConfig : ColumnConfig
	{
		public string Clazz { get; set; }

		public TypeEnum FilterType { get; set; }

		public int Length { get; set; }

		public bool RenderAsString { get; set; }
	}
}