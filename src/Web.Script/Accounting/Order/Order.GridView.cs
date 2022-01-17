using System;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{



	public class OrderGridView : AutoGridView
	{

		public override string GetCustomRowClass(Ext.data.Record record)
		{

			if ((bool) record.get("IsVoid"))
				return "textColor-gray";


			decimal balance = (decimal) record.get("DeliveryBalance");


			if (balance < 0)
				return "rowColor-red";


			object[] totalDue = (object[]) record.get("TotalDue");


			if (Script.IsValue(totalDue) && (decimal) totalDue[0] > 0)
				return "rowColor-orange";


			return balance > 0 ? "rowColor-green" : null;

		}

	}



}