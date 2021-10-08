using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public class HomeModel
	{

		public const int DashboardTaskListSize = 5;


		public TaskDto[] MyTasks { get; set; }

		public int MyTasksTotal { get; set; }

		public DateTime? MetricsFromDate { get; set; }

		public DocumentStatsModel MyDocuments { get;  set; }

		public bool ShowOfficeBlocks { get; set; }

		public TaskDto[] OfficeTasks { get; set; }

		public int OfficeTasksTotal { get; set; }

		public DocumentStatsModel OfficeDocuments { get; set; }


		public static HomeModel New(Domain db, Contracts dc, DateTime? statsFrom, DateTime? statsTo)
		{
			var user = db.Security.User;

			var model = new HomeModel
			{
				MyTasks = dc.Task.New(db.App.GetActiveTasks(user.Person, DashboardTaskListSize)),
				MetricsFromDate = db.Configuration.MetricsFromDate
			};

			model.MyTasksTotal = model.MyTasks.Length < DashboardTaskListSize
				? model.MyTasks.Length
				: db.App.GetActiveTaskCount(user.Person);

			model.MyDocuments = DocumentStatsModel.New(db, user.Person, null, statsFrom, statsTo);

			foreach (var stats in model.MyDocuments.DayStats)
				stats.ReportUrl = $"reports/agent/{stats.Date:yyyy-MM-dd}/{stats.Date:yyyy-MM-dd}_agent_report_{user.Name}.pdf";

			model.ShowOfficeBlocks = db.App.CanViewOfficeTaskMetrics();

			if (!model.ShowOfficeBlocks)
				return model;

			model.OfficeTasks = dc.Task.New(db.App.GetActiveTasks(null, DashboardTaskListSize));
			model.OfficeTasksTotal = model.OfficeTasks.Length < DashboardTaskListSize
				? model.OfficeTasks.Length
				: db.App.GetActiveTaskCount(null);

			var owners =
				db.IsGranted(UserRole.Administrator) ? null :
				db.Configuration.SeparateDocumentAccess ? db.DocumentAccess.GetDocumentOwners() :
				null;

			model.OfficeDocuments = DocumentStatsModel.New(db, null, owners, statsFrom, statsTo);

			return model;
		}

	}


	[DataContract]
	public class DocumentStatsModel
	{
		public int UnprocessedDocuments { get; set; }

		public int DocumentsWithoutOwners { get; set; }

		public int UnpaidDocuments { get; set; }

		public object[] PassportRequirements { get; set; }

		public object[] UrgentPassportRequirements { get; set; }

		public object[] IncorrectPassports { get; set; }

		public int OrdersToPay { get; set; }

		public int OrdersToExecute { get; set; }

		public int OrdersWithDebt { get; set; }

		public DayStats[] DayStats { get; set; }


		public static DocumentStatsModel New(Domain db, Person person, IList<Party> owners, DateTime? statsFrom, DateTime? statsTo)
		{
			return new DocumentStatsModel
			{
				UnprocessedDocuments = db.App.GetUnprocessedProducts(person, owners),
				DocumentsWithoutOwners = db.App.GetDocumentsWithoutOwners(),
				UnpaidDocuments = db.App.GetUnpaidProducts(person, owners),
				IncorrectPassports = db.App.GetIncorrectPassports(person, owners).ToArray(),
				PassportRequirements = db.App.GetPassportRequirements(person, owners).ToArray(),
				UrgentPassportRequirements = db.App.GetUrgentPassportRequirements(person, owners, db.Configuration.DaysBeforeDeparture).ToArray(),
				OrdersToPay = db.App.GetOrdersToPay(person, owners),
				OrdersToExecute = db.App.GetOrdersToExecute(person, owners),
				OrdersWithDebt = db.App.GetOrdersWithDebt(person, owners),
				DayStats = db.App.GetDayStats(person, owners, statsFrom, statsTo).ToArray(),
			};
		}

	}


	//[ServiceContract]
	//public interface IAppService
	//{
	//	AppParameters GetAppParameters();

	//	HomeModel GetHomeModel(DateTime? statsFrom, DateTime? statsTo);

	//	void CloseTask(object id);

	//	AppStateResponse GetAppStateChanges(AppStateRequest request);

	//	Reference[] GetDocumentOwners();

	//	void UpdateAnalytics();
	//}

}