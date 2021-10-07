using System.Web.UI;


namespace Luxena.Web
{
	public class BasePage : Page
	{
		public BasePage()
		{
			ServiceResolver.Current.Resolve(this);
		}
	}
}