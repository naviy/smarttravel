using Luxena.Base.Data;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[Extends(typeof(Invoice))]
	public class Receipt
	{
		public class Service : EntityService<Invoice>
		{

			public override RangeResponse Suggest(RangeRequest request)
			{
				return Invoice.Service.Suggest(request, InvoiceType.Receipt, Query);
			}

		}
	}

}