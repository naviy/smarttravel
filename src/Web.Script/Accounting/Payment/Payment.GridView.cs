using Ext.grid;

using LxnBase.UI.AutoControls;

using Record = Ext.data.Record;


namespace Luxena.Travel
{
	public class PaymentGridView : AutoGridView
	{
		public PaymentGridView() : base(new GridViewConfig().forceFit(true).ToDictionary())
		{
		}

		public override string GetCustomRowClass(Record record)
		{
			if ((bool) record.get("IsVoid"))
				return "textColor-gray";

			if (!(bool) record.get("PostedOn"))
				return "rowColor-red";

			return null;
		}
	}
}