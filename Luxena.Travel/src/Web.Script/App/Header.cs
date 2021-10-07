using System;
using System.Collections;

using Ext;

using LxnBase;
using LxnBase.Data;

using Luxena.Travel.Reports;

using ActionConfig = Ext.ActionConfig;
using Button = Ext.Button;
using ButtonConfig = Ext.ButtonConfig;

using Luxena.Travel.Configuration;

using LxnBase.UI;
using LxnBase.UI.Controls;

using Action = Ext.Action;


namespace Luxena.Travel
{

	public class Header
	{

		public Header(Dictionary config)
		{
			config["tbar"] = new ArrayList();

			SemanticDomain sd = new SemanticDomain(null);

			AddMainMenuItem(config, "Продажи", new object[]
			{
				AppActions.QuickReceipt,
				"-",
				AppActions.NewOrder,
				AppActions.NewTask,
				"-",
				sd.Contract,
				sd.Order,
				sd.Invoice,
				sd.Payment,
				NewAction(AppActions.ConsignmentList, ClassNames.Consignment),
				sd.Task,
				"-",
				sd.ClosedPeriod,
				sd.OpeningBalance,
				sd.InternalTransfer,
			});

			AddMainMenuItem(config, "Услуги", new object[]
			{
				NewAction(AppActions.ProductList, ClassNames.Product),
				"-",
				sd.AviaDocument,
				sd.FlightSegment,
				sd.BusTicket,
				sd.CarRental,
				sd.Pasteboard,
				sd.Accommodation,
				sd.Insurance,
				sd.Isic,
				sd.SimCard,
				sd.Transfer,
				sd.Tour,
				sd.Excursion,
				sd.GenericProduct,
				"-",
				AppActions.AviaConsoleParser,
			});

			AddMainMenuItem(config, Res.Menu_Reports, new object[]
			{
				NewAction(GetCustomerReportAction(), "CustomerReport"),
				NewAction(GetRegistryReportAction(), "RegistryReport"),
				"-",
				NewAction(GetUnbalancedReportAction(), "UnbalancedReport"),
			});

			AddMainMenuItem(config, Res.Menu_Dictionaries, new object[]
			{
				sd.Person,
				sd.Organization,
				sd.Department,
				"-",
				sd.MilesCard,
				sd.Passport,
				"-",
				sd.AirlineServiceClass,
				sd.AirlineMonthCommission,
				sd.Airport,
				sd.AirplaneModel,
				"-",
				sd.Country,
				sd.Currency,
				sd.CurrencyDailyRate,
				sd.PaymentSystem,
				"-",
				sd.GenericProductType,
				sd.AccommodationType,
				sd.CateringType,
			});

			AddMainMenuItem(config, Res.Menu_Administration, new object[]
			{
				sd.User,
				sd.UserVisit,
				sd.Modification,
				sd.SystemConfiguration.ToViewAction(AppManager.SystemConfiguration.Id),
				NewAction(AppActions.UpdateAnalytics, "UpdateAnalytics"),
				"-",
				sd.BankAccount,
				sd.AirlineCommissionPercents,
				sd.DocumentOwner,
				sd.DocumentAccess,
				"-",
				sd.GdsAgent,
				sd.GdsFile,
			});


			((ArrayList)config["tbar"]).AddRange(new object[]
			{
				//AppActions.AviaConsoleParser,
				"->",

				new BoxComponent(new BoxComponentConfig()
					.autoEl(new Dictionary("tag", "div"))
					.cls("icon-filter")
					.ToDictionary()),
				CreateGlobalSearch(),

				"-",
				ProfileViewAction(),
				"-",
				AppActions.SignOutAction,
			});

			_panel = new Panel(config);
		}

		private ComboBox CreateGlobalSearch()
		{
			GlobalSearch = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				//.customActionsDelegate(GetGetCustomActions(sm))
				.allowCreate(false)
				.setClass("GlobalSearchEntity")
				.selectOnFocus(true)
				.forceSelection(true)
				.autoShow(true)
				.emptyText("Быстрый поиск")
				.listeners(new Dictionary(
					"changeValue", new ObjectSelectorValueChangeEvent(delegate (object sender, object[] data)
					{
						if (!Script.IsValue(data)) return;

						ViewObject(data[2].ToString(), data[0]);
						GlobalSearch.clearValue();
					})
				))
				.width(200)
				.listWidth(200)
			).Widget;

			return GlobalSearch;
		}


		public void ViewObject(string className, object id)
		{
			OperationPermissions permissions = (OperationPermissions)AppManager.AllowedActions[className];
			OperationStatus status = permissions != null ? permissions.CanList : null;

			if (status == null || (!status.Visible && string.IsNullOrEmpty(status.DisableInfo)))
				return;

			FormsRegistry.ViewObject(className, id);
		}

		public Component Widget
		{
			get { return _panel; }
		}


		private void AddMainMenuItem(Dictionary config, string title, object[] items)
		{
			ArrayList list = new ArrayList();

			foreach (object item in items)
			{
				if (item == null) continue;

				if (item.ToString() == "-")
				{
					if (list.Count > 0 && Script.IsValue(list[list.Count - 1]) && list[list.Count - 1].ToString() != "-")
						list.Add("-");

					continue;
				}

				SemanticEntity entity = item as SemanticEntity;
				if (entity != null)
				{
					Action action = entity.ToListAction();

					if (Script.IsValue(action))
						list.Add(action);
					continue;
				}

				list.Add(item);
			}

			if (list.Count > 0 && list[list.Count - 1].ToString() == "-")
				list.RemoveAt(list.Count - 1);

			if (list.Count > 0)
				((ArrayList)config["tbar"]).Add(new Dictionary("text", title, "menu", list));
		}


		private Action NewAction(Action action, string className)
		{
			OperationPermissions permissions = (OperationPermissions)AppManager.AllowedActions[className];
			OperationStatus status = permissions != null ? permissions.CanList : null;

			if (status == null || (!status.Visible && string.IsNullOrEmpty(status.DisableInfo)))
				return null;

			if (Script.IsValue(status.DisableInfo))
				action.disable();

			return action;
		}


		private static Button ProfileViewAction()
		{
			return new Button(new ButtonConfig()
				.text(AppManager.CurrentUser.Name)
				.handler(new AnonymousDelegate(
					delegate { ProfileViewForm.ViewObject(AppManager.CurrentUser.Id, false); }))
				.ToDictionary()
			);
		}

		private static Action GetCustomerReportAction()
		{
			return new Action(new ActionConfig()
				.text(Res.CustomerReport_Text)
				.handler(new AnonymousDelegate(
					delegate
					{
						CustomerReportForm form = new CustomerReportForm();
						form.Open();
					}))
				.ToDictionary()
			);
		}

		private static Action GetRegistryReportAction()
		{
			return new Action(new ActionConfig()
				.text(Res.RegistryReport_Text)
				.handler(new AnonymousDelegate(
					delegate
					{
						RegistryReportForm form = new RegistryReportForm();
						form.Open();
					}))
				.ToDictionary()
			);
		}

		private static Action GetUnbalancedReportAction()
		{
			return new Action(new ActionConfig()
				.text(Res.UnbalancedReport)
				.handler(new AnonymousDelegate(
					delegate
					{
						UnbalancedReportForm form = new UnbalancedReportForm();
						form.Open();
					}))
				.ToDictionary()
			);
		}

		private readonly Panel _panel;
		public static ComboBox GlobalSearch;
	}

}