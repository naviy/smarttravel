namespace Luxena.Travel.Domain
{



	//===g






	public enum ImportResult
	{

		None = 0,
		Success = 1,
		Error = 2,
		Warn = 3,
		Reimported = 4,

	}






	public class ImportStatus
	{

		public ImportStatus(ImportResult result, string status)
		{
			Result = result;
			StatusMessage = status;
		}


		public readonly ImportResult Result;
		public readonly string StatusMessage;


		public override string ToString()
		{
			return $"{Result.ToDisplayString()} - {StatusMessage}";
		}

	}






	//===g

}