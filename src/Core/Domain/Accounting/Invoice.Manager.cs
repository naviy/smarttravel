using System.Collections.Generic;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	partial class InvoiceManager
	{

		public override RangeResponse List(RangeRequest request, RecordConfig config)
		{

			var access = db.DocumentAccess.GetAccessRestriction();


			if (access == DocumentAccessRestriction.NoAccess)
				return new RangeResponse();


			if (access == DocumentAccessRestriction.RestrictedAccessByOwner)
			{

				var filters = new List<PropertyFilter>();

				if (request.Filters != null)
					filters.AddRange(request.Filters);


				filters.Add(db.DocumentAccess.CreateDocumentOwnerFilter("Owner"));


				request.Filters = filters.ToArray();

			}


			return base.List(request, config);

		}


	}


}