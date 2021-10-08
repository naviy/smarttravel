using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{

	[DataContract]
	public class RangeResponse
	{
		public RangeResponse()
		{
		}

		public RangeResponse(object[] list)
		{
			List = list;
			TotalCount = list.Length;
		}


		public int Start { get; set; }

		public string Sort { get; set; }

		public string Dir { get; set; }

		public int TotalCount { get; set; }

		public object[] List { get; set; }

		public int? SelectedRow { get; set; }

	}


	[DataContract]
	public class ItemListResponse
	{
		public object[] Items { get; set; }

		public RangeResponse RangeResponse { get; set; }
	}

}