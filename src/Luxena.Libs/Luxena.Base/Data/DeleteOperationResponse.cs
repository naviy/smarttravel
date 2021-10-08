using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{
	[DataContract]
	public class DeleteOperationResponse
	{
		public bool Success { get; set; }

		public RangeResponse RangeResponse { get; set; }

		public object[] UndeletableObjects { get; set; }
	}
}