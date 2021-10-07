using System.Collections;

using LxnBase.Data;


namespace Luxena.Travel
{
	public static class AppManager
	{

		private static AppParameters _appParameters;

		public static SystemConfigurationDto SystemConfiguration
		{
			get { return _appParameters.SystemConfiguration; }
		}

		public static Reference CurrentPerson
		{
			get { return _appParameters.UserPerson; }
		}

		public static Reference CurrentUser
		{
			get { return _appParameters.CurrentUser; }
		}

		public static Dictionary MainPageSettings
		{
			get { return (Dictionary) _appParameters.MainPageSettings; }
		}

		/// <summary>
		/// Distionary(string ClassName, OperationPermissions)
		/// </summary>
		public static Dictionary AllowedActions
		{
			get { return (Dictionary) _appParameters.AllowedActions; }
		}

		public static Reference[] Departments
		{
			get { return _appParameters.Departments; }
		}

		public static Reference[] BankAccounts
		{
			get { return _appParameters.BankAccounts; }
		}

		public static bool AllowSetDocumentOwner
		{
			get { return _appParameters.Departments.Length > 1; }
		}

		public static string Version
		{
			get { return _appParameters.Version; }
		}

		public static void Init(AppParameters parameters)
		{
			_appParameters = parameters;
		}
	}
}