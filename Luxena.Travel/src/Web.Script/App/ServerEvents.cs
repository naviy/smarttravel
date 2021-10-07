using System;
using System.Html;

using LxnBase;
using LxnBase.Net;

using Luxena.Travel.Services;


namespace Luxena.Travel
{
	public class ServerEvents
	{
		public static ServerEvents Instance = new ServerEvents();

		public event Action VersionChanged;

		public event GenericOneArgDelegate NewAviaDocumentImported;

		public event GenericOneArgDelegate NewTaskAssigned;

		public event Action UserRolesChanged;

		public void Setup(int timeout)
		{
			_timeout = timeout;

			_failureTimeout = timeout;

			Refresh();
		}

		public void Refresh()
		{
			if (_isWaitingForResponse)
			{
				_refreshData = true;
			}
			else
			{
				ClearTimeout();

				CheckAppStateChanges();
			}
		}

		private void CheckAppStateChanges()
		{
			// DEBUG
			//return;

			AppStateRequest request = new AppStateRequest();

			request.ClearUserData = _isFirstCall;

			request.CheckImportedDocuments = !Script.IsNullOrUndefined(NewAviaDocumentImported);
			request.CheckNewTasks = !Script.IsNullOrUndefined(NewTaskAssigned);
			request.CheckUserRoleChanges = !Script.IsNullOrUndefined(UserRolesChanged);

			_isWaitingForResponse = true;

			AppService.GetAppStateChanges(request, Load, OnFailure);
		}

		private void Load(object result)
		{
			_isFirstCall = false;
			_isWaitingForResponse = false;

			_failureTimeout = _timeout;

			AppStateResponse response = (AppStateResponse) result;

			if (AppManager.Version != response.Version && VersionChanged != null)
				VersionChanged();

			if (response.ImportedDocuments != null && response.ImportedDocuments.Length > 0 && NewAviaDocumentImported != null)
				NewAviaDocumentImported(response.ImportedDocuments);

			if (response.AssignedTasks != null && response.AssignedTasks.Length > 0 && NewTaskAssigned != null)
				NewTaskAssigned(response.AssignedTasks);

			if (response.IsUserRolesChanged && UserRolesChanged != null)
				UserRolesChanged();

			if (_refreshData)
			{
				_refreshData = false;

				CheckAppStateChanges();
			}
			else
			{
				SetTimeout(_timeout);
			}
		}

		private void SetTimeout(int timeout)
		{
			if (timeout != 0)
				_timeoutId = Window.SetTimeout(CheckAppStateChanges, timeout);
		}

		private void ClearTimeout()
		{
			if (_timeoutId == 0)
				return;

			Window.ClearTimeout(_timeoutId);

			_timeoutId = 0;
		}

		private void OnFailure(WebServiceFailureArgs args)
		{
			args.Handled = true;

			_failureTimeout *= 2;

			SetTimeout(_failureTimeout);
		}

		private int _timeout;
		private int _failureTimeout;

		private int _timeoutId;

		private bool _isFirstCall = true;
		private bool _refreshData;
		private bool _isWaitingForResponse;
	}
}