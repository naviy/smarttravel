using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{
	public class InvoiceGridView : AutoGridView
	{
		public override string GetCustomRowClass(Ext.data.Record record)
		{
			bool isOrderVoid = (bool) record.get("IsOrderVoid");

			return isOrderVoid ? "textColor-gray" : null;
		}
	}
}