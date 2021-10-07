using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Статус услуги")]
	public enum ProductStateFilter
	{
		[RU("Только обработанные")]
		OnlyProcessed,

		[RU("Все")]
		All,

		[RU("Только бронировки")]
		OnlyReservation,
	}


	public partial class ProductFilter
	{

		public string Id { get; set; }


		[EntityName]
		public DateTimeOffset? IssueDate { get; set; }

		[MonthAndYear]
		public DateTimeOffset? IssueMonth { get; set; }

		public DateTimeOffset? MinIssueDate { get; set; }

		public DateTimeOffset? MaxIssueDate { get; set; }

		[Luxena.Domain.Flags]
		public ProductType? Type { get { return Types.One(); } set { Types.Register(value); } }

		public List<ProductType> Types = new List<ProductType>();

		[RU("Название услуги")]
		public string Name { get; set; }

		public ProductStateFilter State { get; set; }

		[RU("Валюта услуги"), CurrencyCode]
		public string ProductCurrency { get; set; }

		public OrganizationReference Provider { get; set; }

		public PartyReference Customer { get; set; }

		public PersonReference Booker { get; set; }

		public PersonReference Ticketer { get; set; }

		public PersonReference Seller { get; set; }

		public PartyReference Owner { get; set; }

		public bool AllowVoided { get; set; }


		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup sm)
		{
			sm.Patterns((Product a) => new ProductFilter
			{
				IssueDate = a.IssueDate,
				IssueMonth = a.IssueDate,
				MinIssueDate = a.IssueDate,
				MaxIssueDate = a.IssueDate,
				Type = a.Type,
				Provider = a.Provider,
				Customer = a.Customer,
				Booker = a.Booker,
				Ticketer = a.Ticketer,
				Seller = a.Seller,
				Owner = a.Owner,
			});
		}

		public IQueryable<TProduct> Get<TProduct>(IQueryable<TProduct> query)
			where TProduct : Product
		{
			if (Id.Yes())
				return query.Where(a => a.Id == Id);


			if (Types.Yes())
				query = query.Where(a => Types.Contains(a.Type));

			if (IssueDate.Yes())
				query = query.Where(a => a.IssueDate == IssueDate);
			else if (IssueMonth != null)
			{
				var date = IssueMonth.Value;
				var m = date.Month;
				var y = date.Year;
				var minDate = new DateTime(y, m, 1);
				var maxDate = new DateTime(y, m + 1, 1).AddDays(-1);
				query = query.Where(a => a.IssueDate >= minDate && a.IssueDate <= maxDate);
			}
			else
			{
				if (MinIssueDate.Yes())
					query = query.Where(a => a.IssueDate >= MinIssueDate);

				if (MaxIssueDate.Yes())
					query = query.Where(a => a.IssueDate <= MaxIssueDate);
			}

			if (ProviderId.Yes())
				query = query.Where(a => a.ProviderId == ProviderId || a.ProducerId == ProviderId);

			if (CustomerId.Yes())
				query = query.Where(a => a.CustomerId == CustomerId);

			if (BookerId.Yes())
				query = query.Where(a => a.BookerId == BookerId);

			if (TicketerId.Yes())
				query = query.Where(a => a.TicketerId == TicketerId);

			if (SellerId.Yes())
				query = query.Where(a => a.SellerId == SellerId);

			if (OwnerId.Yes())
				query = query.Where(a => a.OwnerId == OwnerId);

			if (Name.Yes())
				query = query.Where(a => a.Name.Contains(Name));

			if (State == ProductStateFilter.OnlyProcessed)
				query = query.Where(a => !a.IsReservation && !a.RequiresProcessing);
			else if (State == ProductStateFilter.OnlyReservation)
				query = query.Where(a => a.IsReservation);

			if (ProductCurrency.Yes())
				query = query.Where(a => a.EqualFare.CurrencyId == ProductCurrency);

			if (!AllowVoided)
				query = query.Where(a => !a.IsVoid);

			return query;
		}

	}

}
