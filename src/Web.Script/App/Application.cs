using System;
using System.Collections;

using Ext;
using Ext.lib;
using Ext.state;

using LxnBase;
using LxnBase.Data;
using LxnBase.Knockout;
using LxnBase.Net;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.AutoForms;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;
using LxnTravel.App;
using Luxena.Travel.Services;

using jQueryApi;


namespace Luxena.Travel
{
	public class Application
	{
		static Application()
		{
			WebService.Root = "Services/";

			WebService.Failure += delegate(WebServiceFailureArgs args)
			{
				if (args.Error.StatusCode == 401)
				{
					args.Handled = true;
					jQuery.Window.Unbind("beforeunload");
					AppActions.SignOut();
				}
			};

			ExtClass.onReady(new System.Action(delegate
			{
				AppService.GetAppParameters(delegate(object data) { new Application((AppParameters)data); }, null);
			}));
		}

		private Application(AppParameters parameters)
		{
			if (LocalStorageProvider.IsSupported())
				Manager.setProvider(new LocalStorageProvider());

			AppManager.Init(parameters);

			RegisterForms();

			RegisterExtentions();

			InitAppActions();

			QuickTips.init();

			MessageRegister.NewMessage += OnError;

			Header header = new Header(new Dictionary(
				"id", "header",
				"region", "north",
				"margins", "0 0 0 0"
			));

			Tabs.Init(new Dictionary(
				"id", "content",
				"region", "center"
				));

			HomeTab.Init();

			Messages messages = new Messages(new Dictionary(
				"region", "south",
				"split", true,
				"height", 100,
				"minSize", 100,
				"maxSize", 200,
				"collapsible", true,
				"margins", "0 0 0 0",
				"collapsed", true,
				"autoScroll", true
				));

			Infos.Init();

			ReportLoader.Instance = new ReportProvider();

			SetupServerEvents();

			jQuery.Select("#app-loading").Remove();

			Viewport viewport = new Viewport(new ViewportConfig()
				.layout("border")
				.items(new object[]
				{
					header.Widget,
					Tabs.Widget,
					messages.Widget
				})
				.ToDictionary());

			messages.SetLayoutRegion((Region)Type.GetField(viewport.getLayout(), "south"));

			jQuery.Window.Bind("beforeunload", delegate
			{
				Script.Literal("return {0}", Res.UnloadApplication_Confirm_Msg);
			});
		}

		private static void RegisterForms()
		{
			AutoFormCallbacks.RegisterAsDefaults();

			FormsRegistry.RegisterDefaultList(AutoListTabExt.ListObjects);

			FormsRegistry.RegisterView(ClassNames.Organization, OrganizationViewForm.ViewObject);

			FormsRegistry.RegisterView(ClassNames.Department, DepartmentViewForm.ViewObject);

			FormsRegistry.RegisterView(ClassNames.Airline, AppActions.ListPositioning, false);

			FormsRegistry.RegisterView(ClassNames.Person, PersonViewForm.ViewObject);

			FormsRegistry.RegisterList(ClassNames.Product, AllProductListTab.ListObject);


			FormsRegistry.RegisterList(ClassNames.AviaDocument, AviaDocumentListTab.ListObject);

			FormsRegistry.RegisterView(ClassNames.AviaTicket, AviaDocumentViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.AviaMco, AviaDocumentViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.AviaRefund, AviaDocumentViewForm.ViewObject);

			FormsRegistry.RegisterView(ClassNames.Accommodation, AccommodationViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.Isic, IsicViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.Excursion, ExcursionViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.Pasteboard, PasteboardViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.PasteboardRefund, PasteboardRefundViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.SimCard, SimCardViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.Tour, TourViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.Transfer, TransferViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.Insurance, InsuranceViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.InsuranceRefund, InsuranceRefundViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.CarRental, CarRentalViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.BusTicket, BusTicketViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.BusTicketRefund, BusTicketRefundViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.GenericProduct, GenericProductViewForm.ViewObject);


			FormsRegistry.RegisterView(ClassNames.AirFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.SirenaFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.MirFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.TktFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.PrintFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.GalileoXmlFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.GalileoRailXmlFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.GalileoBusXmlFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.AmadeusXmlFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.SabreFilFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.LuxenaXmlFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.TravelPointXmlFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.CrazyllamaPnrFile, GdsFileViewForm.ViewObject);
			FormsRegistry.RegisterView(ClassNames.DrctXmlFile, GdsFileViewForm.ViewObject);

			FormsRegistry.RegisterView(ClassNames.Task, TaskViewForm.ViewObject);

			//FormsRegistry.RegisterEdit(ClassNames.FlightSegment, SegmentEditForm.EditObject);
			FormsRegistry.RegisterView(ClassNames.AirlineServiceClass, AppActions.ListPositioning, false);
			FormsRegistry.RegisterView(ClassNames.Airport, AppActions.ListPositioning, false);
			//FormsRegistry.RegisterView(ClassNames.Country, AppActions.ListPositioning, false);
			FormsRegistry.RegisterView(ClassNames.Currency, AppActions.ListPositioning, false);
			FormsRegistry.RegisterView(ClassNames.Invoice, AppActions.ListPositioning, false);
			FormsRegistry.RegisterView(ClassNames.PaymentSystem, AppActions.ListPositioning, false);

			FormsRegistry.RegisterView(ClassNames.AirlineCommissionPercents, AppActions.ListPositioning, false);
			FormsRegistry.RegisterView(ClassNames.DocumentOwner, AppActions.ListPositioning, false);
			FormsRegistry.RegisterView(ClassNames.DocumentAccess, AppActions.ListPositioning, false);
			FormsRegistry.RegisterView(ClassNames.GdsAgent, AppActions.ListPositioning, false);

			FormsRegistry.RegisterView(ClassNames.OpeningBalance, AppActions.ListPositioning, false);
			FormsRegistry.RegisterView(ClassNames.InternalTransfer, AppActions.ListPositioning, false);
		}

		private static void RegisterExtentions()
		{
			ControlFactoryExt.Init();

			GridFilterPluginExtention.Init();

			ResBindingHandler.Resources.AddRange(new object[] { typeof(Res), typeof(DomainRes), typeof(BaseRes) });
		}

		private static void OnError(object sender, MessageRegisterEventArgs e)
		{
			if (e.Type == MessageType.Error && !e.Handled)
			{
				MessageBoxWrap.Show(new Dictionary(
					"title", BaseRes.Error,
					"msg", e.Message,
					"buttons", MessageBox.OK,
					"icon", MessageBox.ERROR
				));
			}
		}

		private static void SetupServerEvents()
		{
			ServerEvents.Instance.VersionChanged += OnVersionChanged;
			ServerEvents.Instance.NewAviaDocumentImported += OnNewDocument;
			ServerEvents.Instance.NewTaskAssigned += OnNewTask;
			ServerEvents.Instance.UserRolesChanged += OnUserRolesChanged;

			ServerEvents.Instance.Setup(StateCheckTimeout);
		}

		private static void InitAppActions()
		{
			AppActions.Init();
		}

		private static void OnVersionChanged()
		{
			MessageRegister.Info(Res.Attention, Res.ApplicationVersionChanged);
		}

		private static void OnNewDocument(object arg)
		{
			Reference[] docs = (Reference[])arg;

			if (Script.IsNullOrUndefined(docs) || docs.Length == 0)
				return;

			for (int i = 0; i < docs.Length; i++)
			{
				ObjectStateInfo document = (ObjectStateInfo)docs[i];

				string documentLink = ObjectLink.RenderInfo(document);

				string typeString = null;

				switch (document.Type)
				{
					case "AviaTicket":
						typeString = DomainRes.AviaTicket.ToLowerCase();
						break;

					case "AviaMco":
						typeString = DomainRes.AviaMco.ToLowerCase();
						break;

					case "AviaRefund":
						typeString = DomainRes.AviaRefund.ToLowerCase();
						break;
				}

				string message = null;

				switch ((AviaDocumentState)document.State)
				{
					case AviaDocumentState.Imported:

						message = string.Format(Res.ImportAviaDocument_Msg.ToLowerCase(), typeString, documentLink);

						break;

					case AviaDocumentState.Restored:

						message = string.Format(Res.RestoreAviaDocument_Msg, typeString, documentLink);

						break;

					case AviaDocumentState.Voided:

						message = string.Format(Res.VoidAviaDocument_Msg, typeString, documentLink);

						break;
				}

				if (!Script.IsNullOrUndefined(message))
					MessageRegister.Info(DomainRes.AviaDocument_Caption_List, message);
			}
		}

		private static void OnNewTask(object arg)
		{
			Reference[] tasks = (Reference[])arg;

			if (Script.IsNullOrUndefined(tasks) || tasks.Length == 0)
				return;

			for (int i = 0; i < tasks.Length; i++)
			{
				ObjectStateInfo task = (ObjectStateInfo)tasks[i];

				MessageRegister.Info(DomainRes.Task_Caption_List, string.Format(Res.NewTask_Msg.ToLowerCase(), ObjectLink.RenderInfo(task)));
			}
		}

		private static void OnUserRolesChanged()
		{
			MessageBoxWrap.Show(new Dictionary(
				"title", BaseRes.Warning,
				"msg", Res.UserRolesChanged_Msg,
				"buttons", MessageBox.OK,
				"icon", MessageBox.WARNING
			));
		}

		private const int StateCheckTimeout = 30000;
	}
}