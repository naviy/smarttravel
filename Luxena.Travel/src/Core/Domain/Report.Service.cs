using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Security;

using Luxena.Base.Data;
using Luxena.Base.Metamodel;
using Luxena.Travel.Reports;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Transform;

using BaseGenericDao = Luxena.Base.Data.NHibernate.GenericDao;
using LxnOrder = Luxena.Travel.Domain.Order;


namespace Luxena.Travel.Domain
{

	public class ReportService : DomainService
	{

		public IList<Product> GetAgentDocuments(Person seller, DateTime date, IList<Party> documentOwners)
		{
			var date2 = date.AddDays(1).AddSeconds(-1);

			List<Product> list;

			if (documentOwners == null)
				list = (
					from a in db.Product.Query
					where
						a.IssueDate >= date && a.IssueDate <= date2
						&& a.Seller == seller
						&& !a.IsReservation
					select a
				)
				.AsEnumerable()
				.OrderBy(a => a.Type).ThenBy(a => a.PureNumber)
				.ToList();
			else
				list = (
					from a in db.Product.Query
					where
						a.IssueDate >= date && a.IssueDate <= date2
						&& a.Seller == seller
						&& documentOwners.Contains(a.Owner)
						&& !a.IsReservation
					select a
				)
				.AsEnumerable()
				.OrderBy(a => a.Type).ThenBy(a => a.PureNumber)
				.ToList();


			//var criteria = Session.CreateCriteria(typeof(AviaDocument))
			//	.Add(Restrictions.IsNotNull("Number"))
			//	.Add(Restrictions.Ge("IssueDate", date))
			//	.Add(Restrictions.Lt("IssueDate", date.AddDays(1)))
			//	.Add(Restrictions.Eq("Seller", seller))
			//	.AddOrder(new global::NHibernate.Criterion.Order("class", true))
			//	.AddOrder(new global::NHibernate.Criterion.Order("Number", true));

			//if (documentOwners != null)
			//	criteria.Add(Restrictions.In("Owner", new List<Party>(documentOwners)));

			//var list = criteria.List<AviaDocument>();

			ResolveRefundedDocument(list);

			return list;
		}

		public IList<Payment> GetAgentPayments(Person person, DateTime date, IList<Party> owners)
		{
			return Session.QueryOver<Payment>()
				.Where(p => p.PostedOn >= date && p.PostedOn < date.AddDays(1) && p.AssignedTo == person && !p.IsVoid)
				.OrderBy(p => p.Number).Asc
				.List();
		}


		public IList<Product> GetProductsForExport(RangeRequest request)
		{
			var clazz = Class.Of(request.ClassName);

			var criteria = Session.CreateCriteria(clazz.Type);

			var genericDao = db.Resolve<GenericDao>();
			genericDao.SetCriteriaRestrictions(clazz, criteria, request);
			genericDao.SetCriteriaOrder(criteria, clazz, request.Sort, request.Dir, null);

			var list = criteria.List<Product>();

			ResolveRefundedDocument(list);

			return list;
		}

		public int GetCustomerProductCount(CustomerReportParams prms, bool includeDepartments, IList<Party> documentOwners)
		{
			var query = CreateCustomerProductQuery(prms, includeDepartments, documentOwners, true);

			return (int)query.UniqueResult<long>();
		}

		public IList<Product> GetCustomerProducts(CustomerReportParams prms, bool includeDepartments, IList<Party> documentOwners)
		{
			var query = CreateCustomerProductQuery(prms, includeDepartments, documentOwners, false);

			var list = query.SetReadOnly(true).List<Product>();

			ResolveRefundedDocument(list);

			return list;
		}

		public IList<AviaDocument> GetRegistryReportDocuments(
			DateTime? dateFrom, DateTime? dateTo,
			PaymentType? paymentType, PaymentForm? paymentForm,
			bool? onlyWithInvoices, string airline, IList<Party> documentOwners
		)
		{
			var q = db.AviaDocument.Query
				.Where(a => a.Number != null && !a.IsVoid);

			if (dateFrom.HasValue)
				q = q.Where(a => a.IssueDate >= dateFrom.Value);

			if (dateTo.HasValue)
				q = q.Where(a => a.IssueDate <= dateTo.Value.AddDays(1).AddSeconds(-1));

			if (airline != null)
				q = q.Where(a => a.Provider.Id == airline || a.Producer.Id == airline);

			if (paymentType.HasValue)
				q = q.Where(a => a.PaymentType == paymentType.Value);

			if (paymentForm != null)
				q = q.Where(a => a.Order.Payments.Any(p => p.PaymentForm == paymentForm.Value));

			if (documentOwners != null)
				q = q.Where(a => documentOwners.Contains(a.Owner));

			if (onlyWithInvoices == true)
				q = q.Where(a => a.Order.Invoices.Any());

			var list = q.OrderBy(a => a.Type).ThenBy(a => a.Number).ToList();

			ResolveRefundedDocument(list);

			return list;
		}

		public IList<User> GetUsersByPersons(IList<Person> persons)
		{
			return Session
				.CreateCriteria(typeof(User))
				.Add(Restrictions.In("Person", new List<Person>(persons)))
				.List<User>();
		}

		private IQuery CreateCustomerProductQuery(CustomerReportParams prms, bool includeDepartments, IEnumerable<Party> documentOwners, bool countQuery)
		{
			var hql = new StringWrapper("select ");

			hql *= countQuery ? "count(*)" : "d";

			if (prms.Airline != null)
				hql *= @"from AviaDocument d";
			else
				hql *= @"from Product d";

			if (prms.UnpayedDocumentsOnly)
				hql *= @"    join d.Order as o";


			hql *= @" where";

			hql *= " 1=1";

			if (prms.Customer != null)
			{
				if (prms.Customer is Organization && includeDepartments)
					hql *= "and (d.Customer = :customer or d.Customer in (select dep from Department dep where dep.Organization = :customer))";
				else
					hql *= "and d.Customer = :customer";
			}

			if (prms.BillTo != null)
				hql *= "and d.Order.BillTo = :billTo";
			//hql *= "and d.Order in (select o from Order o where o.BillTo = :billTo)";

			if (prms.Passenger.Yes())
				hql *= "and d.PassengerName like :passenger";

			if (prms.Airline != null)
				hql *= "and d.Airline = :airline";

			if (prms.PaymentType.HasValue)
				hql *= "and d.PaymentType = :paymentType";

			if (prms.DateFrom.HasValue)
				hql *= "and d.IssueDate >= :dateFrom";

			if (prms.DateTo.HasValue)
				hql *= "and d.IssueDate <= :dateTo";

			if (documentOwners != null)
				hql *= "and d.Owner in (:owners)";

			if (prms.UnpayedDocumentsOnly)
				hql *= @"and not o.IsPaid";

			hql *= " and not d.IsReservation and not d.IsVoid";

			if (!countQuery)
				hql *= "order by d.IssueDate asc";

			var query = Session
				.CreateQuery(hql.ToString());

			if (prms.Customer != null)
				query.SetEntity("customer", prms.Customer);

			if (prms.BillTo != null)
				query.SetEntity("billTo", prms.BillTo);

			if (prms.Passenger.Yes())
				query.SetString("passenger", string.Format("%{0}%", prms.Passenger));

			if (prms.Airline != null)
				query.SetEntity("airline", prms.Airline);

			if (prms.PaymentType.HasValue)
				query.SetEnum("paymentType", prms.PaymentType);

			if (prms.DateFrom.HasValue)
				query.SetDateTime("dateFrom", prms.DateFrom.Value);

			if (prms.DateTo.HasValue)
				query.SetDateTime("dateTo", prms.DateTo.Value);

			if (documentOwners != null)
				query.SetParameterList("owners", new List<Party>(documentOwners));

			return query;
		}

		private void ResolveRefundedDocument(IEnumerable<Product> list)
		{
			foreach (var refund in list.OfType<AviaRefund>())
			{
				if (refund.RefundedDocument != null && refund.RefundedDocument.IsAviaTicket)
					refund.RefundedProduct = db.AviaTicket.By(refund.RefundedDocument.Id);
			}
		}


		public byte[] GetAgentReport(DateTime date)
		{
			return db.Commit(() =>
			{
				var user = db.Security.User;

				var access = db.DocumentAccess.GetAccessRestriction();

				IList<Product> aviaDocuments = null;
				IList<Payment> payments = null;
				var owners = access == DocumentAccessRestriction.RestrictedAccess ? db.DocumentAccess.GetMappedOwners() : null;

				if (access != DocumentAccessRestriction.NoAccess)
				{
					aviaDocuments = GetAgentDocuments(user.Person, date, owners);
					payments = GetAgentPayments(user.Person, date, owners);
				}

				var stream = new MemoryStream();

				var configuration = db.Configuration;

				new AgentReport
				{
					DefaultCurrency = configuration.DefaultCurrency,
					UseDefaultCurrencyForInput = configuration.UseDefaultCurrencyForInput,
					ShowAviaHandling = configuration.UseAviaHandling,
					CompanyName = configuration.Company.As(a => a.Name),
					AgentName = user.Person.Name,
					Date = date,
					Products = aviaDocuments,
					Payments = payments
				}
					.Build(stream);

				return stream.ToArray();
			});
		}

		public byte[] GetRegistryReport(
			ReportType type, DateTime? dateFrom, DateTime? dateTo,
			PaymentType? paymentType, PaymentForm? paymentForm,
			bool? onlyWithInvoices, string airline
		)
		{
			var user = db.Security.User;
			if (user == null || !user.AllowRegistryReport)
				throw new SecurityAccessDeniedException();


			var access = db.DocumentAccess.GetAccessRestriction();
			IList<AviaDocument> documents = null;
			var owners = access == DocumentAccessRestriction.RestrictedAccess ? db.DocumentAccess.GetMappedOwners() : null;

			if (access != DocumentAccessRestriction.NoAccess)
				documents = GetRegistryReportDocuments(dateFrom, dateTo, paymentType, paymentForm, onlyWithInvoices, airline, owners);

			var stream = new MemoryStream();

			new RegistryReport
			{
				AirlineCode = airline,
				DateFrom = dateFrom,
				DateTo = dateTo,
				Products = documents,
				DefaultCurrency = db.Configuration.DefaultCurrency,
				UseDefaultCurrencyForInput = db.Configuration.UseDefaultCurrencyForInput,
				PaymentType = paymentType
			}.Build(stream, type);

			return stream.ToArray();
		}

		public byte[] ExportProducts(RangeRequest request)
		{
			IList<Product> products;

			var access = db.DocumentAccess.GetAccessRestriction();

			if (access == DocumentAccessRestriction.NoAccess)
				products = new List<Product>();
			else
			{
				if (access == DocumentAccessRestriction.RestrictedAccess)
				{
					var filters = new List<PropertyFilter>();

					if (request.Filters != null)
						filters.AddRange(request.Filters);

					filters.Add(db.DocumentAccess.CreateDocumentOwnerFilter("Owner"));

					request.Filters = filters.ToArray();
				}

				products = GetProductsForExport(request);
			}

			return db.Resolve<Export.AviaDocumentExcelBuilder>().Make(products);
		}

		public int GetCustomerProductCount(CustomerReportParams prms, bool includeDepartments)
		{
			var access = db.DocumentAccess.GetAccessRestriction();

			switch (access)
			{
				case DocumentAccessRestriction.RestrictedAccess:
					return GetCustomerProductCount(prms, includeDepartments, db.DocumentAccess.GetMappedOwners());

				case DocumentAccessRestriction.FullAccess:
					return GetCustomerProductCount(prms, includeDepartments, null);
			}

			return 0;
		}


		public byte[] GetCustomerReport(CustomerReportParams prms, bool includeDepartments)
		{
			IList<Product> products = null;

			var user = db.Security.User;
			if (user == null || !user.AllowCustomerReport)
				throw new SecurityAccessDeniedException();


			var access = db.DocumentAccess.GetAccessRestriction();

			switch (access)
			{
				case DocumentAccessRestriction.RestrictedAccess:
					products = GetCustomerProducts(prms, includeDepartments, db.DocumentAccess.GetMappedOwners());
					break;

				case DocumentAccessRestriction.FullAccess:
					products = GetCustomerProducts(prms, includeDepartments, null);
					break;
			}

			var stream = new MemoryStream();

			var configuration = db.Configuration;

			new CustomerReport(prms, products)
			{
				DefaultCurrency = configuration.DefaultCurrency,
				UseDefaultCurrencyForInput = configuration.UseDefaultCurrencyForInput
			}.Build(stream);

			return stream.ToArray();
		}


		public byte[] PrintOrders(IList<Order> orders)
		{
			var stream = new MemoryStream();

			db.Resolve<IOrderPrinter>().Build(stream, orders);

			return stream.ToArray();
		}

		public byte[] PrintTickets(IList<AviaTicket> tickets)
		{
			var stream = new MemoryStream();

			db.Resolve<ITicketPrinter>().Build(stream, tickets);

			return stream.ToArray();
		}

		public byte[] PrintBlank(object ticketId)
		{
			return db.Commit(() =>
			{
				var ticket = db.AviaTicket.By(ticketId);

				var stream = new MemoryStream();
				new BlankPrinter().Build(stream, ticket);

				return stream.ToArray();
			});
		}

		public int[] GetCustomerDocumentCount(object customerId, object billToId, string passenger, object airlineId, object paymentType, object dateFrom, object dateTo, bool unpayedDocumentsOnly)
		{
			var prms = GetCustomerReportParams(paymentType, dateFrom, dateTo, customerId, billToId, airlineId, passenger, unpayedDocumentsOnly);

			var customerCount = GetCustomerProductCount(prms, false);

			var total = prms.Customer is Organization ? GetCustomerProductCount(prms, true) : customerCount;

			return new[] { customerCount, total };
		}

		public byte[] GetCustomerReport(object customerId, object billToId, object airlineId, string passenger, object paymentType, object dateFrom, object dateTo, bool includeDepartments, bool unpayedDocumentsOnly)
		{
			var report = GetCustomerReport(
				GetCustomerReportParams(paymentType, dateFrom, dateTo, customerId, billToId, airlineId, passenger, unpayedDocumentsOnly),
				includeDepartments);

			return report;
		}

		public byte[] GetRegistryReport(ReportType reportType, object dateFrom, object dateTo, PaymentType? paymentType, PaymentForm? paymentForm, string airline)
		{
			var report = GetRegistryReport(reportType, (DateTime?)dateFrom, (DateTime?)dateTo, paymentType, paymentForm, airline);

			return report;
		}

		public byte[] GetUnbalancedReport(object customer, DateTime? dateTo, bool includeOrders)
		{
			var user = db.Security.User;
			if (user == null || !user.AllowUnbalancedReport)
				throw new SecurityAccessDeniedException();

			var bytes = new UnbalancedReport
			{
				DateTo = dateTo,
				IncludeOrders = includeOrders,
				Data = PartyBalance.GetUnbalanced(db, dateTo, includeOrders)
			}
				.Build();

			return bytes;
		}


		public byte[] PrintOrders(object[] ids)
		{
			var orders = db.Order.ListByIds(ids);

			var bytes = PrintOrders(orders);

			return bytes;
		}

		public byte[] PrintTickets(object[] ids)
		{
			var tickets = db.AviaTicket.ListByIds(ids);

			var bytes = PrintTickets(tickets);

			return bytes;
		}

		public byte[] PrintReservation(object ticketId)
		{
			var bytes = PrintTickets(
				db.AviaDocument.GetReservationList(db.AviaTicket.By(ticketId)).Cast<AviaTicket>().ToList()
			);

			return bytes;
		}


		private CustomerReportParams GetCustomerReportParams(object paymentType, object dateFrom, object dateTo, object customerId, object billToId, object airlineId, string passenger, bool unpayedDocumentsOnly)
		{
			PaymentType? type = null;
			DateTime? from = null;
			DateTime? to = null;

			if (paymentType != null)
				type = (PaymentType)paymentType;

			if (dateFrom != null)
				from = (DateTime)dateFrom;

			if (dateTo != null)
				to = (DateTime)dateTo;

			return new CustomerReportParams
			{
				Customer = db.Party.By(customerId),
				BillTo = db.Party.By(billToId),
				Passenger = passenger,
				Airline = db.Airline.Load(airlineId),
				PaymentType = type,
				DateFrom = from,
				DateTo = to,
				UnpayedDocumentsOnly = unpayedDocumentsOnly
			};
		}

	}

}