using System.Linq;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	partial class PaymentManager
	{

		public override RangeResponse List(RangeRequest request, RecordConfig config)
		{
			if (!db.IsGranted(UserRole.Supervisor))
				request.NamedFilters = request.NamedFilters.AsConcat("Payments").ToArray();

			return base.List(request, config);
		}

	}

}