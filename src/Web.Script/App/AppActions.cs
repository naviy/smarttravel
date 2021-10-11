using System.Html;

using Ext;

using LxnBase.Net;

using Luxena.Travel.Services;

using jQueryApi;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoForms;




namespace Luxena.Travel
{

	public static class AppActions
	{

		public static Action ProductList = ActionFactory.CreateListAction(ClassNames.Product, DomainRes.AllProduct_Caption_List);
		

		public static Action UpdateAnalytics;
		public static Action ConsignmentList;

		public static Action NewOrder;
		public static Action QuickReceipt;
		public static Action NewPayment;
		public static Action NewTask;

		public static Action AviaConsoleParser;


		public static void Init()
		{
			ConsignmentList = ActionFactory.CreateListAction(ClassNames.Consignment, DomainRes.Consignment_Caption_List);

			UpdateAnalytics = new Action(new ActionConfig()
				.text(Res.UpdateAnalytics_Action)
				.handler(new AnonymousDelegate(
					delegate
					{
						int timeOut = AppService.Service.TimeOut;
						AppService.Service.TimeOut = 300000;

						AppService.UpdateAnalytics(
							delegate
							{
								MessageRegister.Info(Res.UpdateAnalytics_Success);
							},
							delegate (WebServiceFailureArgs args)
							{
								MessageRegister.Error(Res.UpdateAnalytics_Error + args.Error.Message);
								args.Handled = true;
							});

						AppService.Service.TimeOut = timeOut;

						MessageRegister.Info(Res.UpdateAnalytics_Info);
					}))
				.ToDictionary());

			NewOrder = new Action(new ActionConfig()
				.text(Res.NewOrder_Action)
				.handler(new AnonymousDelegate(
					delegate
					{
						FormsRegistry.EditObject(ClassNames.Order, null, null,
							delegate(object arg)
							{
								ItemResponse response = (ItemResponse) arg;
								FormsRegistry.ViewObject(ClassNames.Order, ((OrderDto) response.Item).Id);
							},
							null);
					}))
				.ToDictionary()
			);


			QuickReceipt = new Action(new ActionConfig()

				.text(Res.QuickReceipt_Action)

				.handler(new AnonymousDelegate(delegate
				{

					QuickReceiptEditForm form = new QuickReceiptEditForm();

					form.Saved += delegate(object arg)
					{

						QuickReceiptResponse result = (QuickReceiptResponse) arg;

						FormsRegistry.ViewObject(ClassNames.Order, result.Order.Id);

						ReportPrinter.GetOrderDocument(
							result.Receipt.Id, 
							result.Receipt.Name, 
							InvoiceType.Receipt, 
							null
						);

					};

					form.Open();

				}))

				.ToDictionary()
			);


			NewTask = new Action(new ActionConfig()
				.text(Res.NewTask_Action)
				.handler(new AnonymousDelegate(delegate { FormsRegistry.EditObject(ClassNames.Task, null, null,
					delegate(object arg)
					{
						ItemResponse response = (ItemResponse) arg;
						FormsRegistry.ViewObject(ClassNames.Task, ((Reference)response.Item).Id);
					}, null); }))
				.ToDictionary());

			AviaConsoleParser = new Action(new ActionConfig()
				.text(Res.AviaConsoleParser_Action)
				.handler(new AnonymousDelegate(CreateAviaConsoleParser))
				.ToDictionary());
		}

		public static void ListPositioning(string type, object id, bool newTab)
		{
			RangeRequest request = new RangeRequest();
			request.PositionableObjectId = id;

			FormsRegistry.ListObjects(type, request, newTab);
		}

		//public static void EditOrListPositioning(string type, object id, bool newTab)
		//{
		//	OperationPermissions permissions = (OperationPermissions)AppManager.AllowedActions[type];
		//	OperationStatus canEdit = permissions != null ? permissions.Can : null;

		//	if (canEdit != null && canEdit.IsEnabled)
		//	{
		//		AutoFormCallbacks.ViewObject(type, id, newTab);
		//	}
		//	else
		//	{
		//		RangeRequest request = new RangeRequest();
		//		request.PositionableObjectId = id;

		//		FormsRegistry.ListObjects(type, request, newTab);
		//	}
		//}


		private static void CreateAviaConsoleParser()
		{
			AviaConsoleParserForm form = new AviaConsoleParserForm();
			form.Open();
		}

		public static readonly Action SignOutAction = new Action(new ActionConfig()
			.text(Res.SignOut_Text)
			.handler(new AnonymousDelegate(SignOut))
			.ToDictionary());

		public static void SignOut()
		{
			jQuery.FromHtml("<form action=\"LogOut\" method=\"post\"></form>")
				.AppendTo(Document.Body)
				.Submit();
		}

	}

}