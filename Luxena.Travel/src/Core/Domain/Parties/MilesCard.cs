namespace Luxena.Travel.Domain
{

	[AgentPrivileges]
	public partial class MilesCard : Entity2
	{
		[ReadOnly]
		public virtual Person Owner { get; set; }

		[EntityName]
		public virtual string Number { get; set; }

		public virtual Organization Organization { get; set; }
	}

}