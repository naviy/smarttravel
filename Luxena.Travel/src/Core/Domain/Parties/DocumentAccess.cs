using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Доступ к документам")]
	[SupervisorPrivileges]
	public partial class DocumentAccess : Entity2
	{
		[EntityName]
		public virtual Person Person { get; set; }

		[Patterns.Owner, Suggest(typeof(DocumentOwner))]
		public virtual Party Owner { get; set; }

		[RU("Полный доступ")]
		public virtual bool FullDocumentControl { get; set; }
	}

}