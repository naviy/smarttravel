using Ext;


namespace Luxena.Travel
{

	public partial class SystemConfigurationEditForm
	{

		protected override void CreateControls()
		{
			Window.cls += " aviaDocument-edit tabbed";
			Form.cls = "tabbed";
			Window.width = 800;
			Form.labelWidth = 350;
//			Window.width = 1320;


			Form.add(TabPanel(550, new Component[]
			{

				TabPane("Турагенство", new object[]
				{
					se.Company.ToField(-3),
					se.CompanyDetails.ToField(-3),
					se.AccountantDisplayString.ToField(-3),
					EmptyRow(),
					se.Country,
					se.DefaultCurrency,
					se.UseDefaultCurrencyForInput,
					se.VatRate,
				}),

				TabPane("Услуги", new object[]
				{
					se.UseConsolidatorCommission,
					se.DefaultConsolidatorCommission,
					se.UseAviaHandling,
					se.UseBonuses,
					EmptyRow(),
					se.IsPassengerPassportRequired,
					se.AviaDocumentVatOptions,
					se.NeutralAirlineCode,
					se.Ticket_NoPrintReservations,
					EmptyRow(),
					se.DrctWebService_LoadedOn,
					se.GalileoWebService_LoadedOn,
					se.GalileoRailWebService_LoadedOn,
					se.GalileoBusWebService_LoadedOn,
					se.TravelPointWebService_LoadedOn,
					EmptyRow(),
					se.Pasterboard_DefaultPaymentType,
					EmptyRow(),
					se.AllowOtherAgentsToModifyProduct,
				}),

				TabPane("Заказы", new object[]
				{
					se.AviaOrderItemGenerationOption.ToField(-3),
					se.AmadeusRizUsingMode.ToField(-3),
					se.IncomingCashOrderCorrespondentAccount,
					se.DaysBeforeDeparture,
					se.MetricsFromDate,
					EmptyRow(),
					se.UseAviaDocumentVatInOrder,
					se.AllowAgentSetOrderVat,
					se.SeparateDocumentAccess,
					se.IsOrderRequiredForProcessedDocument,
					se.ReservationsInOfficeMetrics,
					se.McoRequiresDescription,
					se.Order_UseServiceFeeOnlyInVat,
				}),

				TabPane("Счета", new object[]
				{
					se.Invoice_NumberMode.ToField(-3),
					se.InvoicePrinter_ShowVat,
					se.InvoicePrinter_FooterDetails.ToField(-3),
				}),

				TabPane("Накладные", new object[]
				{
					se.Consignment_NumberMode.ToField(-3),
					se.Consignment_SeparateBookingFee.ToField(-3),
				}),

				TabPane("Прочее", new object[]
				{
					se.BirthdayTaskResponsible,
					se.IsOrganizationCodeRequired,
				}),

			}));
		}

	}

}


//using LxnBase.Data;
//using LxnBase.Services;
//using LxnBase.UI;
//using LxnBase.UI.AutoForms;
//
//namespace Luxena.Travel.Configuration
//{
//	public class SystemConfigurationEditForm : AutoEditForm
//	{
//		static SystemConfigurationEditForm()
//		{
//			FormsRegistry.RegisterEdit(ClassNames.SystemConfiguration, EditObject);
//		}
//
//		public static void EditObject(EditFormArgs args)
//		{
//			ConfigManager.GetEditConfig(args.Type,
//				delegate(ItemConfig config)
//				{
//					SystemConfigurationEditForm form = new SystemConfigurationEditForm(args, config);
//					form.Open();
//				});
//		}
//
//		private SystemConfigurationEditForm(EditFormArgs args, ItemConfig itemConfig) : base(args, itemConfig)
//		{
//		}
//
//		protected override void OnLoadForm()
//		{
//			base.OnLoadForm();
//
//			Window.width = 600;
//			Form.labelWidth = 250;
//		}
//	}
//}