using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public class AppStateResponse
	{
		public EntityReference[] ImportedDocuments { get; set; }

		public EntityReference[] AssignedTasks { get; set; }

		public bool IsUserRolesChanged { get; set; }

		public string Version { get; set; }
	}

}