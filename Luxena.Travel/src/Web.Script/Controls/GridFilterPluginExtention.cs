using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls.ColumnFilters;

namespace Luxena.Travel.Controls
{
	public class GridFilterPluginExtention
	{
		public static void Init()
		{
			GridFilterConfig[] configs = new GridFilterConfig[]
			{
				new GridFilterConfig()
					.setCaption(Res.Money_AmountFilter_Text)
					.setFilter(new NumberFilter())
					.setDataPath("Amount"),
				new GridFilterConfig()
					.setCaption(Res.Money_CurrencyFilter_Text)
					.setFilter(new StringFilter())
					.setDataPath("Currency")
			};

			GridFilterPlugin.RegisterCustomFilters("Money", configs);
		}
	}
}