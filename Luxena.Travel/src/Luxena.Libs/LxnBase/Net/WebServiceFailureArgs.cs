namespace LxnBase.Net
{
	public class WebServiceFailureArgs
	{
		public WebServiceFailureArgs(WebServiceError error, string method)
		{
			_error = error;
			_method = method;
		}

		public WebServiceError Error
		{
			get { return _error; }
		}

		public string Method
		{
			get { return _method; }
		}

		public bool Handled
		{
			get { return _handled; }
			set { _handled = value; }
		}

		public override string ToString()
		{
			return _error.Message;
		}

		private readonly WebServiceError _error;
		private readonly string _method;
		private bool _handled;
	}
}