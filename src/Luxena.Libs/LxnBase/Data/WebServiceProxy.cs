using System.Runtime.CompilerServices;

using Ext.data;

using LxnBase.Net;


namespace LxnBase.Data
{
	[Imported]
	public class WebServiceProxy : DataProxy
	{
		public WebServiceProxy(WebService service, string method)
		{
		}

		[IntrinsicProperty]
		public bool UseGet
		{
			get { return false; }
			set { }
		}

		[IntrinsicProperty]
		public object Arguments
		{
			get { return null; }
			set { }
		}

		public void SetResponse(object response)
		{
		}
	}
}