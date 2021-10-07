using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Сводка по услугам")]
	public partial class ProductSummary
	{

		public string Id { get; set; }

		public DateTimeOffset? IssueDate { get; set; }

		public ProductType? Type { get; set; }

		public string Name { get; set; }

		public string Itinerary { get; set; }

		public bool IsRefund { get; set; }

		public Money Total { get; set; }

		public Money ServiceFee { get; set; }

		public Money GrandTotal { get; set; }

		public OrderReference Order { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductSummary> sm)
		{
			sm.Patterns((Product a) => new ProductSummary
			{
				IssueDate = a.IssueDate,
				Type = a.Type,
				Name = a.Name,
				Itinerary = a.Itinerary,
				IsRefund = a.IsRefund,
				Total = a.Total,
				ServiceFee = a.ServiceFee,
				GrandTotal = a.GrandTotal,
				Order = a.Order,
			});
		}

	}


	public partial class ProductSummaryParams : ProductFilter { }


	public class ProductSummaryQuery : Domain.DbQuery<ProductSummaryParams, ProductSummary>
	{
		public override IEnumerable<ProductSummary> Get()
		{
			var products = Params.Get(db.Products);

			Count = products.Count();

			var list = products
				.As(OrderBy)
				.As(Limit)
				.Select(a => new ProductSummary
				{
					Id = a.Id,
					IssueDate = a.IssueDate,
					Type = a.Type,
					Name = a.Name,
					Itinerary = a.Itinerary,
					IsRefund = a.IsRefund,
					Total = a.Total,
					ServiceFee = a.ServiceFee,
					GrandTotal = a.GrandTotal,
					Order = new OrderReference { Id = a.OrderId, Number = a.Order.Number },
				})
				.ToList();

			list.ForEach(a =>
			{
				a.Total = a.IsRefund ? -a.Total : a.Total;
				a.ServiceFee = a.IsRefund ? -a.ServiceFee : a.ServiceFee;
				a.GrandTotal = a.IsRefund ? -a.GrandTotal : a.GrandTotal;
			});

			return list;
		}
	}


	partial class Domain
	{
		public ProductSummaryQuery ProductSummaries { get; set; }
	}

}
