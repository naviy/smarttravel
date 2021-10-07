using System;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Применить к документам")]
	public partial class GdsAgent_ApplyToUnassigned : Domain.DomainAction
	{

		[UiRequired, Subject]
		protected GdsAgent _GdsAgent;

		[Patterns.DateFrom, Subject]
		public DateTimeOffset? DateFrom { get; set; }

		[Patterns.DateTo, Subject]
		public DateTimeOffset? DateTo { get; set; }

		[RU("Кол-во несвязанных услуг")]
		public int? ProductCount { get; protected set; }


		public override void Calculate()
		{
			if (GdsAgent != null)
				ProductCount = GetProducts().Count();
		}

		public override void Execute()
		{
			var person = GdsAgent.Person;
			var officeCode = GdsAgent.OfficeCode;
			var office = GdsAgent.Office;
			var code = GdsAgent.Code;

			db.Commit(() =>
			{
				foreach (var p in GetProducts().ToList())
				{
					if (p.Booker != person && p.BookerOffice == officeCode && p.BookerCode == code)
						p.Booker = person;

					if (p.Ticketer != person && p.TicketerOffice == officeCode && p.TicketerCode == code)
					{
						p.Ticketer = person;

						if (p.Seller == null)
							p.Seller = person;

						if (p.Owner == null)
							p.Owner = office;
					}
					else if (p.IsTicketerRobot)
					{
						if (p.Seller == null)
							p.Seller = person;

						if (p.Owner == null)
							p.Owner = office;
					}

					p.Save(db);
				}
			});
		}


		private IQueryable<Product> GetProducts()
		{
			var personId = GdsAgent.PersonId;
			var origin = GdsAgent.Origin;
			var officeCode = GdsAgent.OfficeCode;
			var code = GdsAgent.Code;

			var query = (IQueryable<Product>)db.Products;

			if (DateFrom.HasValue)
				query = query.Where(a => a.IssueDate >= DateFrom);

			if (DateTo.HasValue)
				query = query.Where(a => a.IssueDate <= DateTo);

			query = query.Where(a =>
				a.Origin == origin &&
				(
					((a.BookerId == null || a.BookerId != personId) && a.BookerOffice == officeCode && a.BookerCode == code) ||
					((a.TicketerId == null || a.TicketerId != personId) && a.TicketerOffice == officeCode && a.TicketerCode == code)
				)
			);

			return query;
		}

	}

}
