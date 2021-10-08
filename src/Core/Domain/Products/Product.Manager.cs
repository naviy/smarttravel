using System.Collections.Generic;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	public class ProductManager<TProduct, TProductService> : EntityManager<TProduct, TProductService>
		where TProduct : Product
		where TProductService : Product.Service<TProduct>
	{

		public override RangeResponse List(RangeRequest request, RecordConfig config)
		{
			var access = db.DocumentAccess.GetAccessRestriction();

			if (access == DocumentAccessRestriction.NoAccess)
				return new RangeResponse();

			if (access == DocumentAccessRestriction.RestrictedAccess)
			{
				var filters = new List<PropertyFilter>();

				if (request.Filters != null)
					filters.AddRange(request.Filters);

				filters.Add(db.DocumentAccess.CreateDocumentOwnerFilter("Owner"));

				request.Filters = filters.ToArray();
			}

			return base.List(request, config);
		}

		
		public override byte[] Export(string className, DocumentExportArgs args, RecordConfig config)
		{
			PrepareExportArgs(args);

			return db.Commit(() => db.Report.ExportProducts(args.Request));
		}

	}

}