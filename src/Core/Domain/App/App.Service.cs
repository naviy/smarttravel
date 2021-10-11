using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Luxena.Base.Data;
using Luxena.Domain.Entities;
using Luxena.Travel.Reports;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;




namespace Luxena.Travel.Domain
{



	//===g






	public class AppService : DomainService
	{

		//---g



		public const int DefaultAgentReportDays = 7 - 1;

		public const int MaxAgentReportDays = 10;



		//---g



		public IList<Task> GetActiveTasks(Person person, int count)
		{
			var query = Session.QueryOver<Task>()
				.Where(t => t.Status != TaskStatus.Closed)
				.OrderBy(t => t.DueDate).Asc()
				.OrderBy(t => t.CreatedOn).Asc()
				.OrderBy(t => t.Id).Asc();

			if (person != null)
				query.And(t => t.AssignedTo == person);

			return query.Take(count).List();
		}



		public int GetActiveTaskCount(Person person)
		{
			var query = Session.QueryOver<Task>()
				.Select(Projections.RowCount())
				.Where(t => t.Status != TaskStatus.Closed);

			if (person != null)
				query.And(t => t.AssignedTo == person);

			return query.SingleOrDefault<int>();
		}



		public int GetUnprocessedProducts(Person agent, IList<Party> documentOwners)
		{
			var query = GetProductsQuery(agent, documentOwners);

			if (agent == null && !db.Configuration.ReservationsInOfficeMetrics)
				query.And(d => d.Name != null);

			return query
				.And(t => t.RequiresProcessing)
				.SingleOrDefault<int>();
		}



		public int GetDocumentsWithoutOwners()
		{
			return db.Product.Query.Count(a => a.Owner == null && !a.IsVoid && a.RequiresProcessing);
		}



		public int GetUnpaidProducts(Person agent, IList<Party> documentOwners)
		{
			Order order = null;

			return GetProductsQuery(agent, documentOwners)
				.Left.JoinAlias(d => d.Order, () => order)
				.And(d => !d.IsVoid && d.Name != null && !(d.Order != null && order.IsPaid))
				.SingleOrDefault<int>();
		}



		public IList<object> GetPassportRequirements(Person agent, IList<Party> documentOwners)
		{
			return GetPassportRequirementsQuery(agent, documentOwners).List<object>();
		}



		public IList<object> GetUrgentPassportRequirements(Person agent, IList<Party> documentOwners, int daysPeriod)
		{
			var query = GetPassportRequirementsQuery(agent, documentOwners);

			query.And(t => t.Departure <= DateTime.Today.AddDays(daysPeriod));

			return query.List<object>();
		}



		public IList<object> GetIncorrectPassports(Person agent, IList<Party> documentOwners)
		{
			var query = db.AviaTicket.QueryOver
				.Select(Projections.Id())
				.Where(t => t.GdsPassportStatus == GdsPassportStatus.Incorrect && !t.IsVoid);

			if (agent != null)
				query.And(t => t.Seller == agent);

			if (documentOwners != null)
				query.WhereRestrictionOn(t => t.Owner).IsIn((ICollection)documentOwners);

			return query.List<object>();
		}



		public int GetOrdersToPay(Person agent, IList<Party> documentOwners)
		{
			return GetOrderQuery(agent, documentOwners)
				.Where(o => o.TotalDue.Amount > 0)
				.SingleOrDefault<int>();
		}



		public int GetOrdersToExecute(Person agent, IList<Party> documentOwners)
		{
			return GetOrderQuery(agent, documentOwners)
				.Where(o => o.DeliveryBalance > 0)
				.SingleOrDefault<int>();
		}



		public int GetOrdersWithDebt(Person agent, IList<Party> documentOwners)
		{
			return GetOrderQuery(agent, documentOwners)
				.Where(o => o.DeliveryBalance < 0)
				.SingleOrDefault<int>();
		}



		public IList<DayStats> GetDayStats(Person agent, IList<Party> documentOwners, DateTime? from, DateTime? to)
		{

			if (!to.HasValue && !from.HasValue)
			{
				to = DateTime.Today;
				from = to.Value.AddDays(-DefaultAgentReportDays);
			}
			else if (!to.HasValue)
				to = from.Value.AddDays(DefaultAgentReportDays);
			else if (!from.HasValue)
				from = to.Value.AddDays(-DefaultAgentReportDays);

			var days = (to.Value - from.Value).Days + 1;

			if (days > MaxAgentReportDays)
				throw new ArgumentException("Time period cannot be more than {0} days".Fill(MaxAgentReportDays));

			if (days < 0)
				throw new ArgumentException("Time period cannot be less than 0");

			var sql = new StringBuilder(@"
				select
					p.issuedate ""Date"",
					sum(case when p.requiresprocessing then 1 else 0 end) ""Unprocessed"",
					count(*) ""Total"",
					sum(case when p.isvoid then 1 else 0 end) ""Void""
				from lt_product p
				where
					p.issuedate between :from and :to");

			if (agent == null)
			{
				if (!db.Configuration.ReservationsInOfficeMetrics)
					sql.Append(@" and p.name is not null");
			}
			else
				sql.Append(@" and p.seller = :seller");

			if (documentOwners != null)
				sql.Append(@" and p.owner in (:owners)");

			sql.Append(@" group by p.issuedate");

			var query = Session.CreateSQLQuery(sql.ToString());

			if (agent != null)
				query.SetParameter("seller", agent);

			if (documentOwners != null)
				query.SetParameterList("owners", documentOwners);

			var data = query
				.SetParameter("from", from.Value)
				.SetParameter("to", to.Value)
				.SetReadOnly(true)
				.SetResultTransformer(Transformers.AliasToBean(typeof(DayStats)))
				.List<DayStats>()
				.ToDictionary(s => s.Date);

			var result = new List<DayStats>(days);

			for (var date = to.Value; date >= from.Value; date = date.AddDays(-1))
			{
				DayStats stats;

				if (!data.TryGetValue(date, out stats))
					stats = new DayStats { Date = date };

				stats.DateText = GetDateText(stats.Date);
				stats.Date = stats.Date.AsUtc();

				result.Add(stats);
			}


			return result;

		}

		

		private static string GetDateText(DateTime date)
		{
			var days = (DateTime.Today - date).Days;

			if (days == 0)
				return CommonRes.Today;

			if (days == -1)
				return CommonRes.Tomorrow;

			if (days == 1)
				return CommonRes.Yesterday;

			if (1 < days && days < 8)
				return $"{days} {CommonRes.DaysAgo}";

			return null;
		}



		private IQueryOver<Product, Product> GetProductsQuery(Person agent, IList<Party> documentOwners)
		{
			var query = Session.QueryOver<Product>()
				.Select(Projections.RowCount());

			if (db.Configuration.MetricsFromDate.HasValue)
				query.And(o => o.IssueDate >= db.Configuration.MetricsFromDate);

			if (agent != null)
				query.And(t => t.Seller == agent);

			if (documentOwners != null)
				query.AndRestrictionOn(t => t.Owner).IsIn((ICollection)documentOwners);

			return query;
		}



		private IQueryOver<AviaTicket, AviaTicket> GetPassportRequirementsQuery(Person agent, IList<Party> documentOwners)
		{
			Organization airline = null;

			var query = db.AviaTicket.QueryOver
				.Select(Projections.Id())
				.JoinAlias(t => t.Producer, () => airline)
				.Where(t => !t.Domestic
					&& t.GdsPassportStatus != GdsPassportStatus.Exist
					&& t.GdsPassportStatus != GdsPassportStatus.Incorrect
					&& !t.IsVoid
					&& t.Departure >= DateTime.Now);

			if (agent != null)
				query.And(t => t.Seller == agent);

			if (documentOwners != null)
				query.WhereRestrictionOn(t => t.Owner).IsIn((ICollection)documentOwners);

			if (db.Configuration.IsPassengerPassportRequired)
				query.WhereRestrictionOn(t => airline.AirlinePassportRequirement).IsIn(new[] { AirlinePassportRequirement.SystemDefault, AirlinePassportRequirement.Required });
			else
				query.And(t => airline.AirlinePassportRequirement == AirlinePassportRequirement.Required);

			if (db.Configuration.MetricsFromDate.HasValue)
				query.And(t => t.IssueDate >= db.Configuration.MetricsFromDate);

			return query;
		}



		private IQueryOver<Order, Order> GetOrderQuery(Person agent, IList<Party> documentOwners)
		{
			var query = Session.QueryOver<Order>()
				.Select(Projections.RowCount())
				.Where(o => !o.IsVoid);

			if (agent != null)
				query.And(t => t.AssignedTo == agent);

			if (documentOwners != null)
				query.WhereRestrictionOn(o => o.Owner).IsIn((ICollection)documentOwners);

			if (db.Configuration.MetricsFromDate.HasValue)
				query.And(o => o.IssueDate >= db.Configuration.MetricsFromDate);

			return query;
		}



		//		public User GetCurrentUser()
		//		{
		//			return db.Security.User;
		//		}



		public void ClearUserData()
		{
			db.AppState.ClearUserData(db.Security.User);
		}



		public EntityReference[] GetImportedDocuments()
		{
			var documents = db.AppState.GetImportedDocuments(db.Security.User);

			var list = new EntityReference[documents.Count];

			for (var i = 0; i < documents.Count; i++)
			{
				AviaDocument document;
				var documentState = AviaDocumentState.Imported;

				var voiding = documents[i] as AviaDocumentVoiding;
				if (voiding != null)
				{
					document = voiding.Document;
					documentState = document.IsVoid ? AviaDocumentState.Voided : AviaDocumentState.Restored;
				}
				else
					document = (AviaDocument)documents[i];

				list[i] = ObjectStateInfo.Create(document, documentState);
			}

			return list;
		}



		public EntityReference[] GetAssignedTasks()
		{
			var tasks = db.AppState.GetAssignedTasks(db.Security.User);

			var list = new EntityReference[tasks.Count];

			for (var i = 0; i < tasks.Count; i++)
			{
				var info = (EntityReference)tasks[i];
				info.Name = tasks[i].Subject;

				list[i] = info;
			}

			return list;
		}



		public bool CheckUserRoleChanges()
		{
			return db.AppState.IsUserRolesChanged(db.Security.User);
		}



		public bool CanViewOfficeTaskMetrics()
		{
			bool fullDocumentControl;

			var restriction = db.DocumentAccess.GetAccessRestriction(out fullDocumentControl);

			if (restriction == DocumentAccessRestriction.FullAccess)
				return fullDocumentControl;

			var documentOwners = db.DocumentAccess.GetMappedOwners();

			return documentOwners.Any(owner => db.DocumentAccess.HasAccess(owner, out fullDocumentControl) && fullDocumentControl);
		}



		public bool CanViewOfficeDocumentMetrics()
		{
			bool fullDocumentControl;

			var restriction = db.DocumentAccess.GetAccessRestriction(out fullDocumentControl);
			var documentOwners = db.DocumentAccess.GetMappedOwners();

			if (restriction == DocumentAccessRestriction.FullAccess)
				return fullDocumentControl;

			return documentOwners.Any(owner => db.DocumentAccess.HasAccess(owner, out fullDocumentControl) && fullDocumentControl);
		}



		public AppParameters GetAppParameters()
		{

			var user = db.Security.User;

			var actions = GetActionPermissions();

			var mainPageSettings = new Dictionary<string, object>(2)
			{
				{ "OfficeTasks", CanViewOfficeTaskMetrics() },
				{ "OfficeDocuments", CanViewOfficeDocumentMetrics() }
			};


			var dc = new Contracts();


			var parameters = new AppParameters
			{

				CurrentUser = user,

				UserPerson = user.Person,

				AllowedActions = actions,

				MainPageSettings = mainPageSettings,

				SystemConfiguration = dc.SystemConfiguration.New(db.Configuration),

				Departments = GetDocumentOwners(),

				BankAccounts = (
					from a in db.BankAccount.Query
					orderby a.IsDefault descending, a.Name
					select new BankAccount.Reference("BankAccount", a.Id, a.Name)
				).ToArray(),

				Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),

				//InvoiceFileExtension = InvoicePrinter.FileExtension(db),

			};


			return parameters;

		}



		public void CloseTask(object id)
		{
			var task = db.Task.By(id);

			task.Status = TaskStatus.Closed;
		}



		public AppStateResponse GetAppStateChanges(AppStateRequest request)
		{

			if (request.ClearUserData)
				ClearUserData();


			var response = new AppStateResponse
			{
				Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
			};


			if (request.CheckImportedDocuments)
				response.ImportedDocuments = GetImportedDocuments();


			if (request.CheckNewTasks)
				response.AssignedTasks = GetAssignedTasks();


			if (request.CheckUserRoleChanges)
				response.IsUserRolesChanged = CheckUserRoleChanges();


			return response;

		}



		public Party.Reference[] GetDocumentOwners()
		{
			var result = db.DocumentAccess.GetDocumentOwners().Select(a => (Party.Reference)a).ToArray();

			return result;
		}



		private OperationStatus CanUpdateAnalytics()
		{
			return db.Granted(db.Olap != null && db.IsGranted(UserRole.Administrator));
		}



		public void UpdateAnalytics()
		{
			if (!CanUpdateAnalytics())
				throw new OperationDeniedException();

			db.Olap.Process();
		}



		private Dictionary<string, OperationPermissions> GetActionPermissions()
		{

			var actions = new Dictionary<string, OperationPermissions>();


			foreach (var service in db.GetServices())
			{
				var permissions = service as IEntityPermissions;
				if (permissions == null) continue;

				var entityType = service.GetDeclaringEntityType();
				if (entityType == null) continue;

				actions.Add(entityType.Name, new OperationPermissions
				{
					CanList = permissions.CanList(),
					CanCreate = permissions.CanCreate(),
					CanUpdate = permissions.CanUpdate(),
					CanDelete = permissions.CanDelete(),
				});
			}


			var user = db.Security.User;

			//actions.Add("CustomerReport", (OperationStatus)true);
			//actions.Add("RegistryReport", (OperationStatus)true);
			//actions.Add("UnbalancedReport", (OperationStatus)true);
			actions.Add("CustomerReport", (OperationStatus)(user != null && (user.IsAdministrator || user.AllowCustomerReport)));
			actions.Add("RegistryReport", (OperationStatus)(user != null && (user.IsAdministrator || user.AllowRegistryReport)));
			actions.Add("UnbalancedReport", (OperationStatus)(user != null && (user.IsAdministrator || user.AllowUnbalancedReport)));

			actions.Add("UpdateAnalytics", CanUpdateAnalytics());


			return actions;

		}



		//---g

	}






	//===g



}

