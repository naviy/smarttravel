using System.Collections.Generic;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	partial class OrderManager
	{


		public override Permissions GetCustomPermissions()
		{
			return new Permissions
			{
				{ "ChangeVat", db.Order.CanChangeVat() }
			};
		}



		public override RangeResponse List(RangeRequest request, RecordConfig config)
		{

			var access = db.DocumentAccess.GetAccessRestriction();


			if (access == DocumentAccessRestriction.NoAccess)
				return new RangeResponse();


			if (access == DocumentAccessRestriction.RestrictedAccessByOwner)
			{

				var filters = new List<string>();

				if (request.NamedFilters != null)
					filters.AddRange(request.NamedFilters);

				filters.Add("Orders");

				request.NamedFilters = filters.ToArray();

			}


			return base.List(request, config);

		}



		public override RangeResponse Suggest(RangeRequest request, RecordConfig config)
		{
			request.Query = "%" + request.Query;

			config.Add("Customer", new RecordConfig { IncludeIdentifier = true, IncludeDisplayString = true, IncludeType = true });
			config.Add("BillTo", new RecordConfig { IncludeIdentifier = true, IncludeDisplayString = true, IncludeType = true });
			config.Add("TotalDue");
			config.Add("VatDue");
			config.Add("Owner", new RecordConfig { IncludeIdentifier = true, IncludeDisplayString = true, IncludeType = true });

			request.VisibleProperties = new[] { "Customer", "BillTo", "TotalDue", "VatDue", "Owner" };
			request.Filters = new[]
			{
				new PropertyFilter
				{
					Property = "IsVoid",
					Conditions = new[]
					{
						new PropertyFilterCondition { Operator = FilterOperator.Equals, Value = false }
					}
				}
			};

			var response = List(request, config);

			if (response.TotalCount == 0)
			{
				request.Filters = null;

				response = List(request, config);
			}

			return response;
		}

	}

}