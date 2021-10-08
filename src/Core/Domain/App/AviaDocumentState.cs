using Luxena.Base.Serialization;

namespace Luxena.Travel.Domain
{
	[DataContract]
	public enum AviaDocumentState
	{
		Imported,
		Voided,
		Restored
	}
}