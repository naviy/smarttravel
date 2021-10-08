using System.Runtime.CompilerServices;


namespace LxnBase.Net
{
	public sealed class WebServiceError
	{
		[PreserveCase]
		public string Message;

		[PreserveCase]
		public string ExceptionType;

		[PreserveCase]
		public string ExceptionDetail;

		[PreserveCase]
		public string StackTrace;

		[PreserveCase]
		public string StatusText;

		[PreserveCase]
		public int StatusCode;
	}
}