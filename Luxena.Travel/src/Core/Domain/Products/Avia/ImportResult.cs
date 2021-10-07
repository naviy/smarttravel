namespace Luxena.Travel.Domain
{

	public enum ImportResult
	{
		None = 0,
		Success = 1,
		Error = 2,
		Warn = 3
	}

	public class ImportStatus
	{
		public ImportStatus(ImportResult result, string status)
		{
			Result = result;
			StatusMessage = status;
		}

		public ImportResult Result;
		public string StatusMessage;

		public override string ToString() 
			=> $"{Result.ToDisplayString()} - {StatusMessage}";
	}

}