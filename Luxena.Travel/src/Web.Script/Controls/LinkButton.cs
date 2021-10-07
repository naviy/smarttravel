using Ext;

namespace Luxena.Travel.Controls
{

	public class LinkButton : Button
	{
		public LinkButton(ButtonConfig config) : base(config.cls("link-button").ToDictionary())
		{
		}

		protected string[] ObjectInfo()
		{
			return new string[] { id, type };
		}
	}


	public class LinkButton2 : Button
	{
		public LinkButton2(string cls, ButtonConfig config) : base(config.cls("link-button " + cls).ToDictionary())
		{
		}

		protected string[] ObjectInfo()
		{
			return new string[] { id, type };
		}
	}


}