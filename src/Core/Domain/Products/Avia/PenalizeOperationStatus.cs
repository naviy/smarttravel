using Luxena.Base.Serialization;

namespace Luxena.Travel.Domain
{
	[DataContract]
	public enum PenalizeOperationStatus
	{
		NotAllowed = 0,

		NotChargeable = 1,

		Chargeable = 2
	}
}