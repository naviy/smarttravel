using Luxena.Base.Domain;


namespace Luxena.Travel.Domain
{

	public partial class PenalizeOperation : Entity2
	{
		public virtual AviaTicket Ticket { get; set; }

		public virtual PenalizeOperationType Type { get; set; }

		public virtual PenalizeOperationStatus Status { get; set; }

		public virtual string Description { get; set; }
	}

}