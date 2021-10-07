using LxnBase.Data;


namespace LxnBase.UI
{
	public sealed class SelectArgs
	{
		public SelectArgs(string type, RangeRequest baseRequest, bool singleSelect, GenericOneArgDelegate onSelect, AnonymousDelegate onCancel)
		{
			Type = type;
			BaseRequest = baseRequest;
			SingleSelect = singleSelect;
			OnSelect = onSelect;
			OnCancel = onCancel;
		}

		public string Type;
		public RangeRequest BaseRequest;
		public bool SingleSelect;
		
		public GenericOneArgDelegate OnSelect;
		public AnonymousDelegate OnCancel;
	}
}