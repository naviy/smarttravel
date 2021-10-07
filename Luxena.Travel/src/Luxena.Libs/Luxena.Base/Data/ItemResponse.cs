using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{
	[DataContract]
	public class ItemResponse
	{
		public object Item { get; set; }

		public RangeResponse RangeResponse { get; set; }

		public object Errors { get; set; }
	}
}