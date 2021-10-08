using Luxena.Base.Serialization;

namespace Luxena.Travel.Domain
{
	[DataContract]
	public class AppStateRequest
	{
		public bool ClearUserData { get; set; }

		public bool CheckImportedDocuments { get; set; }

		public bool CheckNewTasks { get; set; }

		public bool CheckUserRoleChanges { get; set; }
	}
}