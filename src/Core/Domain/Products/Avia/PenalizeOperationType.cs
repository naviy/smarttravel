using Luxena.Base.Serialization;

namespace Luxena.Travel.Domain
{
	[DataContract]
	public enum PenalizeOperationType
	{
		ChangesBeforeDeparture = 0,

		ChangesAfterDeparture = 1,

		RefundBeforeDeparture = 2,

		RefundAfterDeparture = 3,

		NoShowBeforeDeparture = 4,

		NoShowAfterDeparture = 5
	}
}