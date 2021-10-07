using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{
	[DataContract]
	public class PropertyFilter
	{
		public string InternalPath { get; set; }

		public string Property { get; set; }

		public PropertyFilterCondition[] Conditions { get; set; }
	}
}