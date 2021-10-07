using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{
	[DataContract]
	public class PassportValidationResponse
	{
		public PassportValidationResponse()
		{
		}

		public PassportValidationResponse(PassportDto passportDto, PassportValidationResult passportValidationResult)
		{
			Passport = passportDto;
			PassportValidationResult = passportValidationResult;
		}

		public PassportDto Passport { get; set; }

		public PassportValidationResult PassportValidationResult { get; set; }
	}
}