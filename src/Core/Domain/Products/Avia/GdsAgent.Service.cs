using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	partial class GdsAgent
	{
		public class Service : Entity2Service<GdsAgent>
		{

			public GdsAgent By(ProductOrigin origin, string office, string code)
			{
				return By(a =>
					a.Origin == origin &&
					a.OfficeCode == office &&
					a.Code.EndsWith(code)
				);
			}

			public Person PersonBy(ProductOrigin origin, string office, string code)
			{
				return By(origin, office, code).As(a => a.Person);
			}

			public IList<Product> GetNoGdsAgentProducts(GdsAgent agent, DateTime? dateFrom, DateTime? dateTo)
			{
				return GetNoGdsAgentProductsQuery(agent, dateFrom, dateTo).ToList();
			}

			public int GetNoGdsAgentProductCount(GdsAgent agent, DateTime? dateFrom, DateTime? dateTo)
			{
				return GetNoGdsAgentProductsQuery(agent, dateFrom, dateTo).Count();
			}

			public IList<GdsAgent> ListByPersonId(object id)
			{
				return id == null ? EmptyList : ListBy(a => a.Person.Id == id);
			}

			private IQueryable<Product> GetNoGdsAgentProductsQuery(GdsAgent agent, DateTime? dateFrom, DateTime? dateTo)
			{

				var person = agent.Person;
				var origin = agent.Origin;
				var office = agent.OfficeCode;
				var code = agent.Code;


				var query = db.Product.Query.Where(a =>
					a.Origin == origin &&
					(
						((a.Booker == null || a.Booker != person) && a.BookerOffice == office && a.BookerCode == code) ||
						((a.Ticketer == null || a.Ticketer != person) && a.TicketerOffice == office && a.TicketerCode == code)
					)
				);

				if (dateFrom.HasValue)
					query = query.Where(a => a.IssueDate >= dateFrom);

				if (dateTo.HasValue)
					query = query.Where(a => a.IssueDate <= dateTo.Value.AddDays(1).AddMinutes(-1));

				return query;
			}

			public int Apply(GdsAgent gdsAgent, DateTime? dateFrom, DateTime? dateTo)
			{
				var products = GetNoGdsAgentProducts(gdsAgent, dateFrom, dateTo);

				foreach (var product in products)
				{
					if (CheckAgent(product.Booker, product.BookerOffice, product.BookerCode, gdsAgent))
						product.Booker = gdsAgent.Person;

					if (CheckAgent(product.Ticketer, product.TicketerOffice, product.TicketerCode, gdsAgent))
					{
						product.Ticketer = gdsAgent.Person;

						if (product.Seller == null)
							product.Seller = gdsAgent.Person;

						if (product.Owner == null)
							product.Owner = gdsAgent.Office;
					}
					else if (product.IsTicketerRobot)
					{
						if (product.Seller == null)
							product.Seller = gdsAgent.Person;

						if (product.Owner == null)
							product.Owner = gdsAgent.Office;
					}
				}

				return products.Count;
			}

			public OperationStatus CanApply()
			{
				return db.Granted(UserRole.Administrator, UserRole.Supervisor);
			}

			private static bool CheckAgent(Person person, string office, string code, GdsAgent agent)
			{
				return person != agent.Person && office == agent.OfficeCode && code == agent.Code;
			}

		}
	}

}