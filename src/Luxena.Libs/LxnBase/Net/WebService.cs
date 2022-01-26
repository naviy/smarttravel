using System;
using System.Collections;
using System.Serialization;

using jQueryApi;




namespace LxnBase.Net
{



	//===g






	public delegate void WebServiceFailure(WebServiceFailureArgs args);






	//===g






	public class WebService
	{

		//---g



		public WebService(string path)
		{
			_path = path;
		}



		//---g



		public static string Root;


		public static event WebServiceFailure Failure;


		public int TimeOut
		{
			get { return _timeOut; }
			set { _timeOut = value; }
		}


		public string Wrapper
		{
			get { return _wrapper; }
			set { _wrapper = value; }
		}



		//---g



		public void Invoke(string method, Dictionary args, bool useGet, string wrapper, AjaxCallback onSuccess, WebServiceFailure onError)
		{

			jQueryAjaxOptions options = new jQueryAjaxOptions();

			options.Url = Root + _path + "/" + method;
			options.Data = Json.Stringify(args);
			options.DataType = "text";
			options.Timeout = _timeOut;


			if (useGet)
			{
				options.Type = "GET";
			}
			else
			{
				options.Type = "POST";
				options.ContentType = "application/json; charset=utf-8";
			}


			jQuery.Ajax(options)

				.Success(delegate (object data) 
				{

					if (Script.IsNullOrUndefined(onSuccess))
						return;

					object result = Json.Parse((string) data);

					wrapper = Script.Value(wrapper, _wrapper);

					onSuccess(wrapper == "" ? result : Type.GetField(result, wrapper));

				})

				.Error(delegate(jQueryXmlHttpRequest request, string textStatus, Exception e)
				{

					WebServiceError error = new WebServiceError();


					switch (textStatus)
					{

						case "abort":

							error.Message = String.Format(BaseRes.WebServiceAborted, method);
							break;

						case "timeout":

							error.Message = String.Format(BaseRes.WebServiceTimedOut, method);
							break;

						case "parsererror":

							error.Message = String.Format(BaseRes.WebServiceResponse_Error_Msg, method);
							break;

						case "error":

							if (!string.IsNullOrEmpty(request.ResponseText))
								error = (WebServiceError) Json.Parse(request.ResponseText);
							else
								error.Message = String.Format(BaseRes.WebServiceFailedNoMsg, method);

							break;

					}


					error.StatusCode = request.Status;
					error.StatusText = textStatus;

					WebServiceFailureArgs failureArgs = new WebServiceFailureArgs(error, method);


					if (Failure != null)
						Failure(failureArgs);


					if (onError != null)
						onError(failureArgs);


					if (textStatus != "abort")
					{
						MessageRegister.Error(failureArgs.ToString(), failureArgs.Error.ExceptionType, failureArgs.Handled);
					}

				})

			;
		}



		//---g



		private readonly string _path;
		private int _timeOut = 60000;
		private string _wrapper = "d";



		//---g

	}






	//===g



}