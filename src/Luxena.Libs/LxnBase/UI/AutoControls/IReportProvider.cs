using System.Collections;


namespace LxnBase.UI.AutoControls
{
	public interface IReportProvider
	{
		void LoadReport(string url, Dictionary parameters);
	}
}