using System.Collections;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{
	public static class PartyFile
	{
		public static void GetFile(object fileId, string fileName)
		{
			ReportLoader.Load(string.Format("files/party/{0}", fileName), new Dictionary("file", fileId));
		}
	}
}