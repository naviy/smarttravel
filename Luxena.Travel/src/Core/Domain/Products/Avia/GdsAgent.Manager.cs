namespace Luxena.Travel.Domain
{

	public partial class GdsAgentManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions
			{
				{ "ApplyGdsAgentToDocuments", db.GdsAgent.CanApply() }
			};
		}

	}

}