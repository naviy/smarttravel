using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;


namespace Luxena.Web
{

	public static class WebApiClient
	{

		public static HttpClient NewClient(string mediaType = null)
		{
			var client = new HttpClient { BaseAddress = HttpContext.Current.Request.Url };

			if (mediaType != null)
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

			return client;
		}

		public static HttpClient NewJsonClient()
		{
			return NewClient("application/json");
		}


		#region Content

		private static string ReadString(HttpResponseMessage response)
		{
			LastWebApiResponseError = null;

			if (response.IsSuccessStatusCode)
				return response.Content.ReadAsStringAsync().Result;

			ReadError(response);
			return null;
		}

		private static T Read<T>(HttpResponseMessage response) where T : class
		{
			LastWebApiResponseError = null;

			if (response.IsSuccessStatusCode)
				return response.Content.ReadAsAsync<T>().Result;

			ReadError(response);
			return null;
		}


		private static void ReadError(HttpResponseMessage response)
		{
			try
			{
				LastWebApiResponseError = response.Content.ReadAsAsync<WebApiResponseError>().Result;
			}
			catch
			{
				LastWebApiResponseError = new WebApiResponseError { Message = response.Content.ReadAsStringAsync().Result };
			}
		}

		public static WebApiResponseError LastWebApiResponseError;


		public class WebApiResponseError
		{
			public string Message { get; set; }
			public string MessageDetail { get; set; }
			public string ExceptionMessage { get; set; }
			public string ExceptionType { get; set; }
			public string StackTrace { get; set; }
		}

		#endregion


		#region Operations

		public static string GetJson(string url)
		{
			var client = NewJsonClient();
			var response = client.GetAsync(url).Result;
			return ReadString(response);
		}

		public static T GetJson<T>(string url) where T : class
		{
			var client = NewJsonClient();
			var response = client.GetAsync(url).Result;
			return Read<T>(response);
		}


		public static string PostJson(string url, object value)
		{
			var client = NewJsonClient();
			var response = client.PostAsJsonAsync(url, value).Result;
			return ReadString(response);
		}

		public static T PostJson<T>(string url, object value) where T : class
		{
			var client = NewJsonClient();
			var response = client.PostAsJsonAsync(url, value).Result;
			return Read<T>(response);
		}


		public static string PutJson(string url, object value)
		{
			var client = NewJsonClient();
			var response = client.PutAsJsonAsync(url, value).Result;
			return ReadString(response);
		}

		public static T PutJson<T>(string url, object value) where T : class
		{
			var client = NewJsonClient();
			var response = client.PutAsJsonAsync(url, value).Result;
			return Read<T>(response);
		}


		public static string DeleteJson(string url)
		{
			var client = NewJsonClient();
			var response = client.DeleteAsync(url).Result;
			return ReadString(response);
		}

		public static T DeleteJson<T>(string url) where T : class
		{
			var client = NewJsonClient();
			var response = client.DeleteAsync(url).Result;
			return Read<T>(response);
		}

		#endregion

	}

}