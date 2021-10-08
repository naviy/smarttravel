using LxnBase.Data;


namespace LxnBase.UI
{
	public sealed class ListArgs
	{
		public ListArgs(string type, RangeRequest baseRequest)
		{
			if (baseRequest != null)
				baseRequest.ClassName = type;
			Type = type;
			BaseRequest = baseRequest;
		}

		public string Type;

		public RangeRequest BaseRequest;
	}
}