//using System;
//using System.Collections;
//
//using Ext.form;
//
//using Luxena.Travel.Cfg;
//
//
//namespace Luxena.Travel
//{
//
//	public partial class OrderEditForm
//	{
//
//		protected override Dictionary GetInitData()
//		{
//			return new Dictionary(
//				"IssueDate", Date.Today,
//				"AssignedTo", AppManager.CurrentPerson,
//				"IsSubjectOfPaymentsControl", true,
//				"separateServiceFee", true 
//			);
//		}
//
//
//		protected override void CreateControls()
//		{
//			Field separateServiceFee = null;
//
//			if (AppManager.SystemConfiguration.AviaOrderItemGenerationOption == AviaOrderItemGenerationOption.ManualSetting)
//			{
//				separateServiceFee = se.SeparateServiceFee.ToField(-1, delegate(FormMember m)
//				{
//					m.OnChangeValue(delegate(Field field, object value, object oldValue)
//					{
//						_itemsControl.UseServiceFeeOnlyInVat = (bool)value;
//					});
//				});
//			}
//
//			Form.add(ColumnPanel(new object[]
//			{
//				MainDataPanel(new object[]
//				{
//					se.IssueDate,
//					se.Customer,
//					se.BillTo,
//					se.ShipTo,
//				}),
//
//				MainDataPanel(new object[]
//				{
//					se.Owner,
//					se.AssignedTo,
//					se.IsPublic,
//					se.IsSubjectOfPaymentsControl,
//					separateServiceFee,
//				})
//			}));
//
//
//			_itemsControl = new OrderItemGridControl(227);
//			_itemsControl.UseServiceFeeOnlyInVat = AppManager.SystemConfiguration.Order_UseServiceFeeOnlyInVat;
//			_itemsControl.FinanceDataChanged += OrderFinanceDataChanged;
//			_itemsControl.SetParentForm(this);
//
//
//			Form.add()
//		}
//
//
//		private OrderItemGridControl _itemsControl;
//
//	}
//
//}