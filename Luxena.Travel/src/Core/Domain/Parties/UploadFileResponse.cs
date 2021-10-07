using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public class UploadFileResponse
	{
		public bool success { get; set; }

		public FileDto File { get; set; }

		public string ErrorMessage { get; set; }
	}

}