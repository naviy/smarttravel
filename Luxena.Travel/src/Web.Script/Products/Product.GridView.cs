using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	public class ProductGridView : AutoGridView
	{
		public ProductGridView(object config): base(config) { }

		public override string GetCustomRowClass(Ext.data.Record record)
		{
			if ((bool) record.get("IsVoid"))
				return "textColor-gray";

			if ((bool) record.get("RequiresProcessing"))
				return "rowColor-red";

			return null;
		}
	}

}