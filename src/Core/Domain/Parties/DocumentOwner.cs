using System.Linq;

using Luxena.Base.Data;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Владелец документов", "Владельцы документов"), Small]
	[SupervisorPrivileges]
	public partial class DocumentOwner : Entity
	{

		[Patterns.Owner, EntityName]
		public virtual Party Owner { get; set; }

		[RU("Действующий")]
		public virtual bool IsActive { get; set; }


		public class Service : EntityService<DocumentOwner>
		{

			public override RangeResponse Suggest(RangeRequest request)
			{
				return db.Party.Suggest3(request, db.DocumentAccess.GetDocumentOwners().AsQueryable());
			}

		}

	}

}