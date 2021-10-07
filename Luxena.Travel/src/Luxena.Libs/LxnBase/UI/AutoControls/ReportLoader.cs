using System.Collections;


namespace LxnBase.UI.AutoControls
{
	public static class ReportLoader
	{
		public static IReportProvider Instance
		{
			get { return _instance; }
			set { _instance = value; }
		}

		public static void Load(string url, Dictionary parameters)
		{
			Instance.LoadReport(url, parameters);
		}

		private static IReportProvider _instance;
	}
}