using System;
using System.Collections.Generic;

using Ext;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.Net;
using LxnBase.UI;

using Luxena.Travel;
using Luxena.Travel.Services;

using Action = System.Action;
using Window = System.Html.Window;


namespace LxnTravel.App
{
	public class HomeViewModel : HomeModel
	{
		public HomeViewModel()
		{
			MyDocuments = new DocumentStatsViewModel(this, true);
			OfficeDocuments = new DocumentStatsViewModel(this, false);
		}
	}

	public class DocumentStatsViewModel : DocumentStatsModel
	{
		public DocumentStatsViewModel(HomeViewModel homeModel, bool mine)
		{
			_homeModel = homeModel;
			_mine = mine;

			OpenUnprocessedDayDocuments = delegate(DayStats stats) { OpenDocuments(stats, true, false, false); };
			OpenAllDayDocuments = delegate(DayStats stats) { OpenDocuments(stats, false, false, false); };
		}

		protected void OpenUnprocessedDocuments()
		{
			OpenDocuments(null, true, false, false);
		}

		
		protected void OpenDocumentsWithoutOwners()
		{
			OpenDocuments(null, false, false, true);
		}

		protected void OpenUnpaidDocuments()
		{
			OpenDocuments(null, false, true, false);
		}

		protected void OpenPassportRequirements()
		{
			OpenDocumentsById(PassportRequirements);
		}

		protected void OpenUrgentPassportRequirements()
		{
			OpenDocumentsById(UrgentPassportRequirements);
		}

		protected void OpenIncorrectPassports()
		{
			OpenDocumentsById(IncorrectPassports);
		}

		protected void OpenOrdersToPay()
		{
			PropertyFilter filter = new PropertyFilter();
			filter.Property = "TotalDue";
			filter.InternalPath = "Amount";
			PropertyFilterCondition condition = new PropertyFilterCondition();
			condition.Operator = FilterOperator.Greater;
			condition.Value = 0;
			filter.Conditions = new PropertyFilterCondition[] { condition };
			OpenOrders(filter);
		}

		protected void OpenOrdersToExecute()
		{
			OpenOrders(PropertyFilterExtention.CreateFilter("DeliveryBalance", FilterOperator.Greater, 0, false));
		}

		protected void OpenOrdersWithDebt()
		{
			OpenOrders(PropertyFilterExtention.CreateFilter("DeliveryBalance", FilterOperator.Less, 0, false));
		}

		protected Action<DayStats> OpenUnprocessedDayDocuments;

		protected Action<DayStats> OpenAllDayDocuments;

		protected bool ShowDocuments()
		{
			return UnprocessedDocuments != 0 || UnpaidDocuments != 0 || ShowPassportRequirements();
		}

		protected bool ShowPassportRequirements()
		{
			return PassportRequirements.Length != 0 || UrgentPassportRequirements.Length != 0 || IncorrectPassports.Length != 0;
		}

		private void OpenDocuments(DayStats stats, bool unprocessed, bool unpaid, bool withoutOwner)
		{
			RangeRequest request = new RangeRequest();

			List<PropertyFilter> filters = new List<PropertyFilter>();

			if (stats != null)
				filters.Add(PropertyFilterExtention.CreateFilter("IssueDate", FilterOperator.Equals, stats.Date, false));
			else if (_homeModel.MetricsFromDate != null)
				filters.Add(PropertyFilterExtention.CreateFilter("IssueDate", FilterOperator.GreaterOrEquals, _homeModel.MetricsFromDate, false));

			if (_mine)
				filters.Add(PropertyFilterExtention.CreateFilter("Seller", FilterOperator.Equals, AppManager.CurrentPerson.Name, false));

			if (unprocessed)
				filters.Add(PropertyFilterExtention.CreateFilter("RequiresProcessing", FilterOperator.Equals, true, false));

			if (unpaid)
			{
				filters.Add(PropertyFilterExtention.CreateFilter("IsVoid", FilterOperator.Equals, false, false));
				filters.Add(PropertyFilterExtention.CreateFilter("IsPaid", FilterOperator.Equals, false, false));
			}

			if (unpaid || !(_mine || AppManager.SystemConfiguration.ReservationsInOfficeMetrics))
				filters.Add(PropertyFilterExtention.CreateFilter("Name", FilterOperator.IsNull, true, true));

			if (withoutOwner)
			{
				filters.Add(PropertyFilterExtention.CreateFilter("Owner", FilterOperator.IsNull, false, false));
				filters.Add(PropertyFilterExtention.CreateFilter("IsVoid", FilterOperator.Equals, false, false));
				filters.Add(PropertyFilterExtention.CreateFilter("RequiresProcessing", FilterOperator.Equals, true, false));
			}

			request.Filters = (PropertyFilter[]) filters;

			FormsRegistry.ListObjects(ClassNames.Product, request, false);
		}

		private void OpenDocumentsById(object[] ids)
		{
			RangeRequest request = new RangeRequest();

			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("Id", FilterOperator.IsIdIn, ids, false),
			};

			FormsRegistry.ListObjects(ClassNames.AviaDocument, request, false);
		}

		private void OpenOrders(PropertyFilter filter)
		{
			RangeRequest request = new RangeRequest();

			List<PropertyFilter> filters = new List<PropertyFilter>(filter);

			filters.Add(PropertyFilterExtention.CreateFilter("IsVoid", FilterOperator.Equals, false, false));

			if (_homeModel.MetricsFromDate != null)
				filters.Add(PropertyFilterExtention.CreateFilter("IssueDate", FilterOperator.GreaterOrEquals, _homeModel.MetricsFromDate, false));

			if (_mine)
				filters.Add(PropertyFilterExtention.CreateFilter("AssignedTo", FilterOperator.Equals, AppManager.CurrentPerson.Name, false));

			request.Filters = (PropertyFilter[])filters;

			FormsRegistry.ListObjects(ClassNames.Order, request, false);
		}

		private readonly HomeViewModel _homeModel;
		private readonly bool _mine;
	}

	public partial class HomeTab : Tab
	{
		private HomeTab(string tabId) : base(
			new PanelConfig()
				.title(Res.Home)
				.cls("homeTab")
				.autoScroll(true)
				.ToDictionary(),
			tabId)
		{
			EditSettings = delegate
			{
				HomeSettingsForm form = new HomeSettingsForm();
				form.From = _statsFrom;
				form.To = _statsTo;
				form.Saved += delegate
				{
					_statsFrom = form.From;
					_statsTo = form.To;
					Reload();
				};
				form.Open();
			};

			CloseTask = delegate (TaskDto task, jQueryEvent e)
			{
				ClearTimeout();

				AppService.CloseTask(task.Id, delegate
				{
					jQuery.FromElement(e.Target).Closest("li.task").FadeTo(500, 0).Promise().Done(new Action(delegate { Reload(); }));
				},
					null);

				return true;
			};
		}

		public static void Init()
		{
			Tabs.Open(false, "homeTab", delegate(string tabId) { return new HomeTab(tabId); });
		}

		protected HomeViewModel Model;

		protected bool ShowMyTasksTotal;

		protected bool ShowOfficeTasksTotal;

		protected Action EditSettings;

		protected Func<TaskDto, jQueryEvent, bool> CloseTask;

		protected override void OnActivate(bool isFirst)
		{
			Reload();
		}

		protected override void OnDeactivate()
		{
			ClearTimeout();
		}

		protected void OpenAllMyTasks()
		{
			RangeRequest request = new RangeRequest();

			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("AssignedTo", FilterOperator.Equals, AppManager.CurrentPerson.Name, false)
			};

			FormsRegistry.ListObjects(ClassNames.Task, request, false);
		}

		protected void OpenMyActiveTasks()
		{
			RangeRequest request = new RangeRequest();

			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("AssignedTo", FilterOperator.Equals, AppManager.CurrentPerson.Name, false),
				PropertyFilterExtention.CreateFilter("Status", FilterOperator.Equals, TaskStatus.Closed, true)
			};

			FormsRegistry.ListObjects(ClassNames.Task, request, false);
		}

		protected void OpenAllTasks()
		{
			FormsRegistry.ListObjects(ClassNames.Task, new RangeRequest(), false);
		}

		protected void OpenActiveTasks()
		{
			RangeRequest request = new RangeRequest();

			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("Status", FilterOperator.Equals, TaskStatus.Closed, true)
			};

			FormsRegistry.ListObjects(ClassNames.Task, request, false);
		}

		protected void OpenDocuments(object[] ids)
		{
			RangeRequest request = new RangeRequest();

			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("Id", FilterOperator.IsIdIn, ids, false),
			};

			FormsRegistry.ListObjects(ClassNames.AviaDocument, request, false);
		}

		protected void OpenOrders(object[] ids)
		{
			RangeRequest request = new RangeRequest();

			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("Id", FilterOperator.IsIdIn, ids, false),
			};

			FormsRegistry.ListObjects(ClassNames.Order, request, false);
		}

		private void ClearTimeout()
		{
			if (_timeout == 0)
				return;

			Window.ClearTimeout(_timeout);
			_timeout = 0;
		}

		private void Load()
		{
			AppService.GetHomeModel(_statsFrom, _statsTo, SetModel, delegate (WebServiceFailureArgs args)
			{
				args.Handled = true;
				_timeout = Window.SetTimeout(Load, 60000);
			});
		}

		private void SetModel(object result)
		{
			Model = new HomeViewModel();

			ObjectUtility.Merge(Model, result);

			ShowMyTasksTotal = Model.MyTasks.Length < Model.MyTasksTotal;

			if (Model.ShowOfficeBlocks)
				ShowOfficeTasksTotal = Model.OfficeTasks != null && Model.OfficeTasks.Length < Model.OfficeTasksTotal;

			jQueryObject obj = jQuery.FromHtml("<div class=\"home\"></div>");

			Main.RenderTo(obj.GetElement(0), this);

			jQuery.FromElement(body.dom).Html(obj);

			_timeout = Window.SetTimeout(Load, 30000);
		}

		private void Reload()
		{
			ClearTimeout();

			Load();
		}

		private int _timeout;
		private Date _statsFrom;
		private Date _statsTo;
	}
}