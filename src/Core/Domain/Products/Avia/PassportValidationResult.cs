using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{
	[DataContract]
	public enum PassportValidationResult
	{
		NoPassport,
		NotValid,
		ExpirationDateNotValid,
		Valid
	}
}