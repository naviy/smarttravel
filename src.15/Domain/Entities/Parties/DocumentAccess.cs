using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Доступ к документам")]
	[SupervisorPrivileges]
	public partial class DocumentAccess : Entity2
	{

		[EntityName]
		protected Person _Person;

		[ActiveOwner]
		protected Party _Owner;

		[RU("Полный доступ")]
		public bool FullDocumentControl { get; set; }


		static partial void Config_(Domain.EntityConfiguration<DocumentAccess> entity)
		{
			entity.Association(a => a.Person, a => a.DocumentAccesses);
			entity.Association(a => a.Owner);//, a => a.DocumentAccesses_Owner);
		}

	}

	
	partial class Domain
	{
		public DbSet<DocumentAccess> DocumentAccesses { get; set; }
	}

}