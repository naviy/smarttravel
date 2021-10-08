namespace Luxena.Travel.Domain
{

	partial class TaskManager
	{
		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ChangeTaskStatus", CanUpdate() } };
		}

		public override Permissions GetCustomPermissions(object obj)
		{
			return new Permissions { { "ChangeTaskStatus", CanUpdate(obj) } };
		}
	}

}