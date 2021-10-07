using System;
using System.Data.Entity;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Владелец документов", "Владельцы документов"), Small]
	[SupervisorPrivileges]
	public partial class DocumentOwner : Entity
	{

		[Patterns.Owner, EntityName]
		protected Party _Owner;

		[RU("Действующий")]
		public bool IsActive { get; set; }


		static partial void Config_(Domain.EntityConfiguration<DocumentOwner> entity)
		{
			entity.Association(a => a.Owner, a => a.DocumentOwners);
		}

	}


	[RU("Владелец документов (активный)", "Владельцы документов (активные)"), Small]
	public class ActiveOwner : Domain.EntityQuery<Party>
	{
		protected override IQueryable<Party> GetQuery()
		{
			if (db.DocumentOwners.Any(a => a.IsActive))
				return
					from a in db.DocumentOwners
					where a.IsActive
					select a.Owner;

			return new[] { db.AppConfiguration.Company }.AsQueryable();
		}
	}


	partial class Domain
	{

		public DbSet<DocumentOwner> DocumentOwners { get; set; }

		public ActiveOwner ActiveOwners { get; set; }
		

		public Party DefaultOwner => _defaultOwner.Get(this);
		readonly Lazy<Party> _defaultOwner = NewLazy(db => 
			db.ActiveOwners.Count() == 1 ? db.ActiveOwners.One() : null
		);

	}


	[RU("Владелец"), Lookup(typeof(ActiveOwner))]
	public class ActiveOwnerAttribute : Attribute { }

}